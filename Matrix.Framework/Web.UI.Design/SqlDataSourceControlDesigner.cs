namespace Microsoft.Matrix.Framework.Web.UI.Design
{
    using Microsoft.Matrix.Framework.Web.UI;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.UI.Design;

    public class SqlDataSourceControlDesigner : DataControlDesigner
    {
        private DataTable _dummyDataTable;

        public override object GetDataSource()
        {
            return this.SqlDataSourceControl.DataSource;
        }

        public override IEnumerable GetDataSource(string listName)
        {
            DataTable dataTable = null;
            IEnumerable designTimeDataSource = null;
            try
            {
                IEnumerable realDataSet = this.GetRealDataSet(listName);
                if (realDataSet != null)
                {
                    dataTable = DesignTimeData.CreateSampleDataTable(realDataSet);
                }
                if (dataTable != null)
                {
                    return designTimeDataSource;
                }
                if (this._dummyDataTable == null)
                {
                    this._dummyDataTable = DesignTimeData.CreateDummyDataTable();
                }
                dataTable = this._dummyDataTable;
            }
            catch (Exception)
            {
                if (this._dummyDataTable == null)
                {
                    this._dummyDataTable = DesignTimeData.CreateDummyDataTable();
                }
                dataTable = this._dummyDataTable;
            }
            finally
            {
                designTimeDataSource = DesignTimeData.GetDesignTimeDataSource(dataTable, 5);
            }
            return designTimeDataSource;
        }

        public override string GetDesignTimeHtml()
        {
            return base.CreatePlaceHolderDesignTimeHtml();
        }

        private IEnumerable GetRealDataSet(string listName)
        {
            DataSet dataSet = new DataSet();
            this.PopulateDataSet(ref dataSet, listName);
            if (dataSet != null)
            {
                if ((listName == string.Empty) || (listName.Length == 0))
                {
                    return dataSet.Tables[0].DefaultView;
                }
                if (dataSet.Tables[listName] != null)
                {
                    return dataSet.Tables[listName].DefaultView;
                }
            }
            return null;
        }

        public override object GetSchema()
        {
            return null;
        }

        protected override void OnDataSourceChanged(EventArgs e)
        {
        }

        private void PopulateDataSet(ref DataSet dataSet, string listName)
        {
            SqlConnection connection = new SqlConnection(this.SqlDataSourceControl.ConnectionString);
            SqlCommand selectCommand = new SqlCommand(this.SqlDataSourceControl.SelectCommand, connection);
            if (this.SqlDataSourceControl.Parameters.Count > 0)
            {
                IDictionaryEnumerator enumerator = this.SqlDataSourceControl.Parameters.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    selectCommand.Parameters.Add(current.Key.ToString(), current.Value);
                }
            }
            SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            try
            {
                if ((listName != null) && (listName != string.Empty))
                {
                    adapter.Fill(dataSet, listName);
                }
                else
                {
                    adapter.Fill(dataSet);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        protected override void PostFilterProperties(IDictionary properties)
        {
            base.PostFilterProperties(properties);
            if (((PropertyDescriptor) properties["DataBindings"]) != null)
            {
                properties.Remove("DataBindings");
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.SqlDataSourceControl.ConnectionString;
            }
            set
            {
                this.SqlDataSourceControl.ConnectionString = value;
                this.OnDataSourceChanged(new EventArgs());
            }
        }

        public string SelectCommand
        {
            get
            {
                return this.SqlDataSourceControl.SelectCommand;
            }
            set
            {
                this.SqlDataSourceControl.SelectCommand = value;
                this.OnDataSourceChanged(new EventArgs());
            }
        }

        private Microsoft.Matrix.Framework.Web.UI.SqlDataSourceControl SqlDataSourceControl
        {
            get
            {
                return (Microsoft.Matrix.Framework.Web.UI.SqlDataSourceControl) base.Component;
            }
        }
    }
}

