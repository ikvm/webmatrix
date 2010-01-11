namespace Microsoft.Matrix.Framework.Web.UI
{
    using Microsoft.Matrix.Framework.Web.UI.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;

    [Designer(typeof(AccessDataSourceControlDesigner))]
    public class AccessDataSourceControl : DataControl
    {
        private string _connectionString;
        private DataSet _dataSet;
        private Hashtable _parameters;

        public override int Delete(string listName, IDictionary selectionFilters)
        {
            throw new InvalidOperationException(string.Format("AccessDataSourceControl_DeleteNotSupported", this.ID));
        }

        public override IEnumerable GetDataSource(string listName)
        {
            DataView defaultView;
            DataSet dataSet = new DataSet();
            this.PopulateDataSet(dataSet, listName);
            if (dataSet == null)
            {
                return null;
            }
            if ((listName == string.Empty) || (listName.Length == 0))
            {
                defaultView = dataSet.Tables[0].DefaultView;
            }
            else if (dataSet.Tables[listName] != null)
            {
                defaultView = dataSet.Tables[listName].DefaultView;
            }
            else
            {
                return null;
            }
            string sortExpression = this.GetSortExpression(listName);
            if (!this.IsSortDirectionAscending(listName))
            {
                sortExpression = sortExpression + " DESC";
            }
            defaultView.Sort = sortExpression;
            string str2 = string.Empty;
            if (this.GetFilters(listName) != null)
            {
                IDictionaryEnumerator enumerator = this.GetFilters(listName).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    if (str2 != string.Empty)
                    {
                        str2 = str2 + " AND ";
                    }
                    object obj2 = str2;
                    str2 = string.Concat(new object[] { obj2, current.Key, " = '", current.Value, "'" });
                }
                defaultView.RowFilter = str2;
            }
            return defaultView;
        }

        public override int Insert(string listName, IDictionary values)
        {
            throw new InvalidOperationException(string.Format("AccessDataSourceControl_InsertNotSupported", this.ID));
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            object obj2 = this.ViewState["Parameters"];
            if (obj2 != null)
            {
                this._parameters = (Hashtable) obj2;
            }
        }

        private int PerformSqlCommand(OleDbCommand command)
        {
            int num;
            OleDbConnection connection = new OleDbConnection(this.ConnectionString);
            connection.Open();
            try
            {
                command.Connection = connection;
                num = command.ExecuteNonQuery();
                if (num > 0)
                {
                    this.OnDataSourceChanged(EventArgs.Empty);
                }
            }
            finally
            {
                connection.Close();
            }
            return num;
        }

        private void PopulateDataSet(DataSet dataSet, string listName)
        {
            if ((this.ConnectionString != string.Empty) && (this.SelectCommand != string.Empty))
            {
                OleDbConnection connection = new OleDbConnection(this.ConnectionString);
                OleDbCommand selectCommand = new OleDbCommand(this.SelectCommand, connection);
                if (this.Parameters.Count > 0)
                {
                    IDictionaryEnumerator enumerator = this.Parameters.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                        selectCommand.Parameters.Add(current.Key.ToString(), current.Value);
                    }
                }
                OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
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
        }

        protected override object SaveViewState()
        {
            if ((this._parameters != null) && (this._parameters.Count > 0))
            {
                this.ViewState["Parameters"] = this._parameters;
            }
            else
            {
                this.ViewState.Remove("Parameters");
            }
            return base.SaveViewState();
        }

        public override int Update(string listName, IDictionary selectionFilters, IDictionary newValues)
        {
            throw new InvalidOperationException(string.Format("AccessDataSourceControl_UpdateNotSupported", this.ID));
        }

        [Browsable(false), DefaultValue(false)]
        public override bool AutoGenerateDeleteCommand
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        [Browsable(false), DefaultValue(false)]
        public override bool AutoGenerateInsertCommand
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        [DefaultValue(false), Browsable(false)]
        public override bool AutoGenerateUpdateCommand
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public override bool CanDelete
        {
            get
            {
                return false;
            }
        }

        public override bool CanFilter
        {
            get
            {
                return true;
            }
        }

        public override bool CanInsert
        {
            get
            {
                return false;
            }
        }

        public override bool CanSort
        {
            get
            {
                return true;
            }
        }

        public override bool CanUpdate
        {
            get
            {
                return false;
            }
        }

        [WebSysDescription("AccessDataSourceControl_ConnectionString"), WebCategory("Data")]
        public string ConnectionString
        {
            get
            {
                return this._connectionString;
            }
            set
            {
                this._connectionString = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override object DataSource
        {
            get
            {
                if (this._dataSet == null)
                {
                    this._dataSet = new DataSet();
                    this.PopulateDataSet(this._dataSet, string.Empty);
                }
                return this._dataSet;
            }
        }

        [Browsable(false), WebCategory("Data"), WebSysDescription("AccessDataSourceControl_Parameters"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDictionary Parameters
        {
            get
            {
                if (this._parameters == null)
                {
                    this._parameters = new Hashtable();
                }
                return this._parameters;
            }
        }

        [WebCategory("Data"), WebSysDescription("AccessDataSourceControl_SelectCommand")]
        public string SelectCommand
        {
            get
            {
                object obj2 = this.ViewState["SelectCommand"];
                if (obj2 == null)
                {
                    return string.Empty;
                }
                return (string) obj2;
            }
            set
            {
                this.ViewState["SelectCommand"] = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Type UnderlyingDataSourceType
        {
            get
            {
                return typeof(DataSet);
            }
        }
    }
}

