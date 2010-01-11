namespace Microsoft.Matrix.Framework.Web.UI
{
    using Microsoft.Matrix.Framework;
    using Microsoft.Matrix.Framework.Web.UI.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using System.Web;

    [Designer(typeof(SqlDataSourceControlDesigner))]
    public class SqlDataSourceControl : DataControl
    {
        private string _connectionString;
        private DataSet _dataSet;
        private Hashtable _parameters;

        private SqlCommand BuildDeleteCommand(string listName, IDictionary selectionFilters)
        {
            SqlCommand command = new SqlCommand();
            int num = 0;
            if ((listName == string.Empty) || (listName.Length == 0))
            {
                throw new HttpException(string.Format(Microsoft.Matrix.Framework.SR.GetString("SqlDataSourceControl_MustHaveTableNameForDel"), this.ID));
            }
            StringBuilder builder = new StringBuilder("DELETE FROM " + listName + " WHERE ", (selectionFilters.Count * 0x20) + 0x40);
            if ((selectionFilters != null) && (selectionFilters.Count > 0))
            {
                IDictionaryEnumerator enumerator = selectionFilters.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    if (num < (selectionFilters.Count - 1))
                    {
                        builder.Append(string.Concat(new object[] { "[", current.Key, "] = '", current.Value, "', " }));
                    }
                    else
                    {
                        builder.Append(string.Concat(new object[] { "[", current.Key, "] = '", current.Value, "'" }));
                    }
                    num++;
                }
            }
            command.CommandText = builder.ToString();
            return command;
        }

        private SqlCommand BuildSqlCommand(string commandString, IDictionary selectionFilters, IDictionary newValues)
        {
            IDictionaryEnumerator enumerator;
            SqlCommand command = new SqlCommand(commandString);
            if ((selectionFilters != null) && (selectionFilters.Count > 0))
            {
                enumerator = selectionFilters.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    command.Parameters.Add(current.Key.ToString(), current.Value);
                }
            }
            if ((newValues != null) && (newValues.Count > 0))
            {
                enumerator = newValues.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry entry2 = (DictionaryEntry) enumerator.Current;
                    command.Parameters.Add(entry2.Key.ToString(), entry2.Value);
                }
            }
            return command;
        }

        private SqlCommand BuildUpdateCommand(string listName, IDictionary selectionFilters, IDictionary newValues)
        {
            IDictionaryEnumerator enumerator;
            SqlCommand command = new SqlCommand();
            if ((listName == string.Empty) || (listName.Length <= 0))
            {
                throw new HttpException(string.Format(Microsoft.Matrix.Framework.SR.GetString("SqlDataSourceControl_MustHaveTableNameForUpd"), this.ID));
            }
            StringBuilder builder = new StringBuilder("UPDATE " + listName + " SET ", ((selectionFilters.Count * 0x20) + (newValues.Count * 0x20)) + 0x20);
            int num = 0;
            if ((newValues != null) && (newValues.Count > 0))
            {
                enumerator = newValues.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    if (num < (newValues.Count - 1))
                    {
                        builder.Append(string.Concat(new object[] { "[", current.Key, "] = '", current.Value, "', " }));
                    }
                    else
                    {
                        builder.Append(string.Concat(new object[] { "[", current.Key, "] = '", current.Value, "'" }));
                    }
                    num++;
                }
            }
            builder.Append(" WHERE ");
            if ((selectionFilters != null) && (selectionFilters.Count > 0))
            {
                enumerator = selectionFilters.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry entry2 = (DictionaryEntry) enumerator.Current;
                    if (num < (selectionFilters.Count - 1))
                    {
                        builder.Append(string.Concat(new object[] { "[", entry2.Key, "] = '", entry2.Value, "', " }));
                    }
                    else
                    {
                        builder.Append(string.Concat(new object[] { "[", entry2.Key, "] = '", entry2.Value, "'" }));
                    }
                    num++;
                }
            }
            command.CommandText = builder.ToString();
            return command;
        }

        public override int Delete(string listName, IDictionary selectionFilters)
        {
            SqlCommand command;
            string deleteCommand = this.DeleteCommand;
            if (this.AutoGenerateDeleteCommand)
            {
                command = this.BuildDeleteCommand(listName, selectionFilters);
            }
            else
            {
                if ((deleteCommand == string.Empty) || (deleteCommand.Length == 0))
                {
                    throw new HttpException(string.Format(Microsoft.Matrix.Framework.SR.GetString("SqlDataSourceControl_NoDeleteCommand"), this.ID));
                }
                command = this.BuildSqlCommand(deleteCommand, selectionFilters, null);
            }
            return this.PerformSqlCommand(command);
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
            return 0;
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

        private int PerformSqlCommand(SqlCommand command)
        {
            int num;
            SqlConnection connection = new SqlConnection(this.ConnectionString);
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
                SqlConnection connection = new SqlConnection(this.ConnectionString);
                SqlCommand selectCommand = new SqlCommand(this.SelectCommand, connection);
                if (this.Parameters.Count > 0)
                {
                    IDictionaryEnumerator enumerator = this.Parameters.GetEnumerator();
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
            SqlCommand command;
            string updateCommand = this.UpdateCommand;
            if (this.AutoGenerateUpdateCommand)
            {
                command = this.BuildUpdateCommand(listName, selectionFilters, newValues);
            }
            else
            {
                if ((updateCommand == string.Empty) || (updateCommand.Length == 0))
                {
                    throw new HttpException(string.Format(Microsoft.Matrix.Framework.SR.GetString("SqlDataSourceControl_NoUpdateCommand"), this.ID));
                }
                command = this.BuildSqlCommand(updateCommand, selectionFilters, newValues);
            }
            return this.PerformSqlCommand(command);
        }

        public override bool CanDelete
        {
            get
            {
                return true;
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
                return true;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("SqlDataSourceControl_ConnectionString"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Data")]
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

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("SqlDataSourceControl_DeleteCommand"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Data")]
        public string DeleteCommand
        {
            get
            {
                object obj2 = this.ViewState["DeleteCommand"];
                if (obj2 == null)
                {
                    return string.Empty;
                }
                return (string) obj2;
            }
            set
            {
                this.ViewState["DeleteCommand"] = value;
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("SqlDataSourceControl_Parameters"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Data"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        [Microsoft.Matrix.Framework.Web.UI.WebCategory("Data"), Microsoft.Matrix.Framework.Web.UI.WebSysDescription("SqlDataSourceControl_SelectCommand")]
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public override Type UnderlyingDataSourceType
        {
            get
            {
                return typeof(DataSet);
            }
        }

        [Microsoft.Matrix.Framework.Web.UI.WebSysDescription("SqlDataSourceControl_UpdateCommand"), Microsoft.Matrix.Framework.Web.UI.WebCategory("Data")]
        public string UpdateCommand
        {
            get
            {
                object obj2 = this.ViewState["UpdateCommand"];
                if (obj2 == null)
                {
                    return string.Empty;
                }
                return (string) obj2;
            }
            set
            {
                this.ViewState["UpdateCommand"] = value;
            }
        }
    }
}

