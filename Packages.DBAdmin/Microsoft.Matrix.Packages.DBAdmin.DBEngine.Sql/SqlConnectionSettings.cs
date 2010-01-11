namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Sql
{
    using Microsoft.Matrix.Utility;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class SqlConnectionSettings : ISerializable
    {
        private string _database;
        private string _password;
        private bool _secureConnection;
        private string _server;
        private string _username;

        public SqlConnectionSettings()
        {
        }

        private SqlConnectionSettings(SerializationInfo info, StreamingContext context)
        {
            string data = info.GetString("Password");
            if ((data != null) && (data.Length != 0))
            {
                data = DataProtection.DecryptUserData(data);
            }
            this._server = info.GetString("Server");
            this._username = info.GetString("Username");
            this._password = data;
            this._secureConnection = info.GetBoolean("SecureConnection");
            this._database = info.GetString("Database");
        }

        public SqlConnectionSettings(string server, string database)
        {
            this._server = server;
            this._secureConnection = true;
            this._database = database;
        }

        public SqlConnectionSettings(string server, string username, string password, string database)
        {
            this._server = server;
            this._username = username;
            this._password = password;
            this._secureConnection = false;
            this._database = database;
        }

        public SqlConnectionSettings(string server, string username, string password, bool secureConnection, string database)
        {
            this._server = server;
            this._username = username;
            this._password = password;
            this._secureConnection = secureConnection;
            this._database = database;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            string data = this._password;
            if ((data != null) && (data.Length != 0))
            {
                data = DataProtection.EncryptUserData(data);
            }
            info.AddValue("Server", this._server);
            info.AddValue("Username", this._username);
            info.AddValue("Password", data);
            info.AddValue("SecureConnection", this._secureConnection);
            info.AddValue("Database", this._database);
        }

        public string Database
        {
            get
            {
                return this._database;
            }
            set
            {
                this._database = value;
            }
        }

        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        public bool SecureConnection
        {
            get
            {
                return this._secureConnection;
            }
            set
            {
                this._secureConnection = value;
            }
        }

        public string Server
        {
            get
            {
                return this._server;
            }
            set
            {
                this._server = value;
            }
        }

        public string Username
        {
            get
            {
                return this._username;
            }
            set
            {
                this._username = value;
            }
        }
    }
}

