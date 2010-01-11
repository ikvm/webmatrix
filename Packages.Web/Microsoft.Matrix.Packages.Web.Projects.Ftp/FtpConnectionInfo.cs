namespace Microsoft.Matrix.Packages.Web.Projects.Ftp
{
    using Microsoft.Matrix.Utility;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class FtpConnectionInfo : ISerializable
    {
        private string _httpRoot;
        private string _httpUrl;
        private string _password;
        private string _remoteHostName;
        private int _remotePort;
        private string _userName;
        public const string AnonymousUser = "anonymous";
        public const int DefaultPort = 0x15;

        public FtpConnectionInfo()
        {
            this._remoteHostName = string.Empty;
            this._remotePort = 0x15;
            this._userName = "anonymous";
            this._password = string.Empty;
            this._httpRoot = string.Empty;
            this._httpUrl = string.Empty;
        }

        private FtpConnectionInfo(SerializationInfo info, StreamingContext context)
        {
            string data = info.GetString("Password");
            if ((data != null) && (data.Length != 0))
            {
                data = DataProtection.DecryptUserData(data);
            }
            this._userName = info.GetString("UserName");
            this._password = data;
            this._remoteHostName = info.GetString("RemoteHostName");
            this._remotePort = info.GetInt32("RemotePort");
            this._httpRoot = info.GetString("HttpRoot");
            this._httpUrl = info.GetString("HttpUrl");
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            string data = this._password;
            if ((data != null) && (data.Length != 0))
            {
                data = DataProtection.EncryptUserData(data);
            }
            info.AddValue("UserName", this._userName);
            info.AddValue("Password", data);
            info.AddValue("RemoteHostName", this._remoteHostName);
            info.AddValue("RemotePort", this._remotePort);
            info.AddValue("HttpRoot", this._httpRoot);
            info.AddValue("HttpUrl", this._httpUrl);
        }

        public string HttpRoot
        {
            get
            {
                return this._httpRoot;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this._httpRoot = value;
            }
        }

        public string HttpUrl
        {
            get
            {
                return this._httpUrl;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this._httpUrl = value;
            }
        }

        internal bool IsValid
        {
            get
            {
                if (this.RemoteHostName.Length == 0)
                {
                    return false;
                }
                if (this.UserName.Equals("anonymous") && (this.Password.Length == 0))
                {
                    return false;
                }
                return true;
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

        public string RemoteHostName
        {
            get
            {
                return this._remoteHostName;
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    throw new ArgumentNullException("value");
                }
                this._remoteHostName = value;
            }
        }

        public int RemotePort
        {
            get
            {
                return this._remotePort;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._remotePort = value;
            }
        }

        public string UserName
        {
            get
            {
                return this._userName;
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    throw new ArgumentNullException("value");
                }
                this._userName = value;
            }
        }
    }
}

