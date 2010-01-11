namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine
{
    using System;
    using System.Data;
    using System.Data.Common;

    public interface IDataProviderDatabase
    {
        DbDataAdapter CreateAdapter(IDbCommand selectCommand);
        DbDataAdapter CreateAdapter(IDbConnection connection, Table table, bool readOnly);
        IDbCommand CreateCommand(string commandText);
        IDbConnection CreateConnection();
        IDataParameter CreateParameter(string name, DbType dbType);

        Type AdapterType { get; }

        Type CommandType { get; }

        string ConnectionString { get; }

        Type ConnectionType { get; }

        Type ParameterType { get; }

        string ProviderName { get; }
    }
}

