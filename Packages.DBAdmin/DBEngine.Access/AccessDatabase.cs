namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Access
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.OleDb;
    using System.IO;
    using System.Runtime.InteropServices;

    public sealed class AccessDatabase : Database, IDataProviderDatabase
    {
        private Interop.ICatalog _adoxCatalog;
        private AccessConnectionSettings _connectionSettings;

        public AccessDatabase(AccessConnectionSettings connectionSettings)
        {
            if (connectionSettings == null)
            {
                throw new ArgumentNullException("connectionSettings");
            }
            this._connectionSettings = connectionSettings;
        }

        internal static void CloseAdoxCatalog(Interop.ICatalog adoxCatalog)
        {
            if (adoxCatalog != null)
            {
                Interop.IAdoConnection activeConnection = (Interop.IAdoConnection) adoxCatalog.GetActiveConnection();
                activeConnection.Close();
                adoxCatalog.SetActiveConnection(null);
                while (Marshal.ReleaseComObject(adoxCatalog) > 0)
                {
                }
                while (Marshal.ReleaseComObject(activeConnection) > 0)
                {
                }
                adoxCatalog = null;
                activeConnection = null;
                GC.Collect();
            }
        }

        protected override void ConnectInternal()
        {
            string filename = this._connectionSettings.Filename;
            if (!File.Exists(filename))
            {
                throw new InvalidOperationException(string.Format("The file '{0}' does not exist.", filename));
            }
            if (!Path.IsPathRooted(filename))
            {
                throw new InvalidOperationException("The filename must contain a full path.\r\nExample: c:\\example.mdb");
            }
            if (!filename.ToLower().EndsWith(".mdb"))
            {
                throw new InvalidOperationException("The filename must have the '.mdb' file extension.\r\nExample: c:\\example.mdb");
            }
            this._adoxCatalog = (Interop.ICatalog) new Interop.Catalog();
            int ret = this._adoxCatalog.SetActiveConnection(this.ConnectionString);
        }

        public override Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column CreateColumn()
        {
            return AccessColumn.DefaultColumn;
        }

        private static string CreateConnectionString(string fileName, bool disableConnectionPooling)
        {
            if (disableConnectionPooling)
            {
                return ("Provider=Microsoft.Jet.OLEDB.4.0; Ole DB Services=-4; Data Source=" + fileName);
            }
            return ("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + fileName);
        }

        public static void CreateDatabase(string fileName)
        {
            Interop.ICatalog adoxCatalog = (Interop.ICatalog) new Interop.Catalog();
            try
            {
                adoxCatalog.Create(CreateConnectionString(fileName, false));
            }
            finally
            {
                CloseAdoxCatalog(adoxCatalog);
            }
        }

        protected override TableCollection CreateTableCollection()
        {
            return new AccessTableCollection(this);
        }

        protected override void DisconnectInternal()
        {
            CloseAdoxCatalog(this._adoxCatalog);
        }

        DbDataAdapter IDataProviderDatabase.CreateAdapter(IDbCommand selectCommand)
        {
            return new OleDbDataAdapter((OleDbCommand) selectCommand);
        }

        DbDataAdapter IDataProviderDatabase.CreateAdapter(IDbConnection connection, Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table table, bool readOnly)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter("select * from [" + table.Name.Replace("]", "]]") + "]", (OleDbConnection) connection);
            if (!readOnly)
            {
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                builder.QuotePrefix = "[";
                builder.QuoteSuffix = "]";
                OleDbCommand insertCommand = builder.GetInsertCommand();
                OleDbCommand deleteCommand = builder.GetDeleteCommand();
                OleDbCommand updateCommand = builder.GetUpdateCommand();
                foreach (Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column column in table.Columns)
                {
                    if (column.IsIdentity)
                    {
                        insertCommand.CommandText = insertCommand.CommandText + ";SELECT @retId=@@IDENTITY";
                        insertCommand.UpdatedRowSource = UpdateRowSource.OutputParameters;
                        OleDbParameter parameter = new OleDbParameter();
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
            return new OleDbCommand(commandText);
        }

        IDbConnection IDataProviderDatabase.CreateConnection()
        {
            return new OleDbConnection(this.ConnectionString);
        }

        IDataParameter IDataProviderDatabase.CreateParameter(string name, DbType dbType)
        {
            return new OleDbParameter(name, dbType);
        }

        internal Interop.ICatalog AdoxCatalog
        {
            get
            {
                return this._adoxCatalog;
            }
        }

        public AccessConnectionSettings ConnectionSettings
        {
            get
            {
                return this._connectionSettings;
            }
        }

        private string ConnectionString
        {
            get
            {
                return CreateConnectionString(this._connectionSettings.Filename, true);
            }
        }

        public override string DisplayName
        {
            get
            {
                string filename = this.ConnectionSettings.Filename;
                return (Path.GetFileName(filename) + " on " + Path.GetDirectoryName(filename));
            }
        }

        Type IDataProviderDatabase.AdapterType
        {
            get
            {
                return typeof(OleDbDataAdapter);
            }
        }

        Type IDataProviderDatabase.CommandType
        {
            get
            {
                return typeof(OleDbCommand);
            }
        }

        string IDataProviderDatabase.ConnectionString
        {
            get
            {
                return this.ConnectionString;
            }
        }

        Type IDataProviderDatabase.ConnectionType
        {
            get
            {
                return typeof(OleDbConnection);
            }
        }

        Type IDataProviderDatabase.ParameterType
        {
            get
            {
                return typeof(OleDbParameter);
            }
        }

        string IDataProviderDatabase.ProviderName
        {
            get
            {
                return "OleDb";
            }
        }
    }
}

