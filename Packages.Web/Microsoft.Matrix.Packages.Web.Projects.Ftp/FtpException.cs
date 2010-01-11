namespace Microsoft.Matrix.Packages.Web.Projects.Ftp
{
    using System;

    internal sealed class FtpException : ApplicationException
    {
        private int _statusCode;

        public FtpException(string message) : this(message, null, 0)
        {
        }

        public FtpException(string message, Exception innerException) : this(message, innerException, 0)
        {
        }

        public FtpException(string message, int statusCode) : this(message, null, statusCode)
        {
        }

        public FtpException(string message, Exception innerException, int statusCode) : base(message, innerException)
        {
            this._statusCode = statusCode;
        }

        public override string ToString()
        {
            if (this._statusCode != 0)
            {
                return (this._statusCode.ToString() + ": " + base.ToString());
            }
            return base.ToString();
        }

        public int StatusCode
        {
            get
            {
                return this._statusCode;
            }
        }
    }
}

