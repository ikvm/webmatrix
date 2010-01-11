namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Sql
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public sealed class SqlDatabase : Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database, IDataProviderDatabase
    {
        private SqlConnectionSettings _connectionSettings;
        private Interop.IDatabase _dmoDatabase;
        private Interop.ISqlServer _dmoServer;

        public SqlDatabase(SqlConnectionSettings connectionSettings)
        {
            if (connectionSettings == null)
            {
                throw new ArgumentNullException("connectionSettings");
            }
            this._connectionSettings = connectionSettings;
        }

        protected override void ConnectInternal()
        {
            this._dmoServer = (Interop.ISqlServer) new Interop.SqlServer();
            this._dmoServer.SetLoginSecure(this._connectionSettings.SecureConnection);
            this._dmoServer.Connect(this._connectionSettings.Server, this._connectionSettings.Username, this._connectionSettings.Password);
            if ((this._connectionSettings.Database != null) && (this._connectionSettings.Database.Length > 0))
            {
                Interop.IDatabases databases = this._dmoServer.GetDatabases();
                for (int i = 1; i <= databases.GetCount(); i++)
                {
                    Interop.IDatabase database = databases.Item(i, string.Empty);
                    if (database.GetName().ToLower() == this._connectionSettings.Database.ToLower())
                    {
                        this._dmoDatabase = database;
                        return;
                    }
                }
            }
        }

        public override Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column CreateColumn()
        {
            return SqlColumn.DefaultColumn;
        }

        public void CreateDatabase(string name)
        {
            try
            {
                base.Connect();
                Interop.IDatabase database = (Interop.IDatabase) new Interop.Database();
                database.SetName(name);
                this._dmoServer.GetDatabases().Add(database);
            }
            finally
            {
                base.Disconnect();
            }
        }

        protected override StoredProcedureCollection CreateStoredProcedureCollection()
        {
            return new SqlStoredProcedureCollection(this);
        }

        protected override TableCollection CreateTableCollection()
        {
            return new SqlTableCollection(this);
        }

        public bool DatabaseExists(string name)
        {
            bool flag;
            try
            {
                base.Connect();
                Interop.IDatabases databases = this._dmoServer.GetDatabases();
                Interop.IDatabase database = null;
                for (int i = 1; i <= databases.GetCount(); i++)
                {
                    Interop.IDatabase database2 = databases.Item(i, string.Empty);
                    if (database2.GetName().ToLower() == this._connectionSettings.Database.ToLower())
                    {
                        database = database2;
                        break;
                    }
                }
                flag = database != null;
            }
            finally
            {
                base.Disconnect();
            }
            return flag;
        }

        protected override void DisconnectInternal()
        {
            this._dmoServer.DisConnect();
            this._dmoServer = null;
            this._dmoDatabase = null;
        }

        public string[] GetDatabaseNames()
        {
            string[] strArray2;
            try
            {
                base.Connect();
                Interop.IDatabases databases = this._dmoServer.GetDatabases();
                int count = databases.GetCount();
                string[] strArray = new string[count];
                for (int i = 1; i <= count; i++)
                {
                    strArray[i - 1] = databases.Item(i, string.Empty).GetName();
                }
                strArray2 = strArray;
            }
            finally
            {
                base.Disconnect();
            }
            return strArray2;
        }

        DbDataAdapter IDataProviderDatabase.CreateAdapter(IDbCommand selectCommand)
        {
            return new SqlDataAdapter((SqlCommand) selectCommand);
        }

        DbDataAdapter IDataProviderDatabase.CreateAdapter(IDbConnection connection, Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table table, bool readOnly)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from [" + table.Name.Replace("]", "]]") + "]", (SqlConnection) connection);
            if (!readOnly)
            {
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                builder.QuotePrefix = "[";
                builder.QuoteSuffix = "]";
                SqlCommand insertCommand = builder.GetInsertCommand();
                SqlCommand deleteCommand = builder.GetDeleteCommand();
                SqlCommand updateCommand = builder.GetUpdateCommand();
                foreach (Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column column in table.Columns)
                {
                    if (column.IsIdentity)
                    {
                        insertCommand.CommandText = insertCommand.CommandText + ";SELECT @retId=@@IDENTITY";
                        insertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
                        SqlParameter parameter = new SqlParameter();
                        parameter.ParameterName = "@retId";
                        parameter.DbType = column.DbType;
                        parameter.Size = column.Size;
                        parameter.Direction = ParameterDirection.Output;
                        parameter.IsNullable = column.AllowNulls;
                        parameter.Precision = (byte) column.NumericPrecision;
                        parameter.Scale = (byte) column.NumericScale;
                        parameter.SourceColumn = column.Name;
                        parameter.SourceVersion = DataRowVersion.Current;
                        parameter.Value = DBNull.Value;
                        insertCommand.Parameters.Add(parameter);
                        break;
                    }
                }
                adapter.DeleteCommand = deleteCommand;
                adapter.UpdateCommand = updateCommand;
                adapter.InsertCommand = insertCommand;
            }
            return adapter;
        }

        IDbCommand IDataProviderDatabase.CreateCommand(string commandText)
        {
            return new SqlCommand(commandText);
        }

        IDbConnection IDataProviderDatabase.CreateConnection()
        {
            return new SqlConnection(this.ConnectionString);
        }

        IDataParameter IDataProviderDatabase.CreateParameter(string name, DbType dbType)
        {
            return new SqlParameter(name, dbType);
        }

        public SqlConnectionSettings ConnectionSettings
        {
            get
            {
                return this._connectionSettings;
            }
        }

        public override string DisplayName
        {
            get
            {
                return (this.ConnectionSettings.Server + "." + this.ConnectionSettings.Database);
            }
        }

        internal Interop.IDatabase DmoDatabase
        {
            get
            {
                return this._dmoDatabase;
            }
        }

        Type IDataProviderDatabase.AdapterType
        {
            get
            {
                return typeof(SqlDataAdapter);
            }
        }

        Type IDataProviderDatabase.CommandType
        {
            get
            {
                return typeof(SqlCommand);
            }
        }

        public string ConnectionString
        {
            get
            {
                if (this._connectionSettings.SecureConnection)
                {
                    return string.Format("server='{0}'; trusted_connection=true; database='{1}'", this._connectionSettings.Server, this._connectionSettings.Database);
                }
                return string.Format("server='{0}'; user id='{1}'; password='{2}'; database='{3}'", new object[] { this._connectionSettings.Server, this._connectionSettings.Username, this._connectionSettings.Password, this._connectionSettings.Database });
            }
        }

        Type IDataProviderDatabase.ConnectionType
        {
            get
            {
                return typeof(SqlConnection);
            }
        }

        Type IDataProviderDatabase.ParameterType
        {
            get
            {
                return typeof(SqlParameter);
            }
        }

        string IDataProviderDatabase.ProviderName
        {
            get
            {
                return "Sql";
            }
        }

        public override bool SupportsStoredProcedures
        {
            get
            {
                return true;
            }
        }
    }
}

