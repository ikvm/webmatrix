namespace Microsoft.Matrix.WebHost
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Web;

    internal sealed class Connection
    {
        private Host _host;
        private Socket _socket;

        public Connection(Host host, Socket socket)
        {
            this._host = host;
            this._socket = socket;
        }

        public void Close()
        {
            try
            {
                this._socket.Shutdown(SocketShutdown.Both);
                this._socket.Close();
            }
            catch
            {
            }
            finally
            {
                this._socket = null;
            }
        }

        private static string MakeResponseHeaders(int statusCode, string moreHeaders, int contentLength, bool keepAlive)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Concat(new object[] { "HTTP/1.1 ", statusCode, " ", HttpWorkerRequest.GetStatusDescription(statusCode), "\r\n" }));
            builder.Append("Server: Microsoft ASP.NET Web Matrix Server/" + Messages.VersionString + "\r\n");
            builder.Append("Date: " + DateTime.Now.ToUniversalTime().ToString("R", DateTimeFormatInfo.InvariantInfo) + "\r\n");
            if (contentLength >= 0)
            {
                builder.Append("Content-Length: " + contentLength + "\r\n");
            }
            if (moreHeaders != null)
            {
                builder.Append(moreHeaders);
            }
            if (!keepAlive)
            {
                builder.Append("Connection: Close\r\n");
            }
            builder.Append("\r\n");
            return builder.ToString();
        }

        public void ProcessOneRequest()
        {
            if (this.WaitForRequestBytes() == 0)
            {
                this.WriteErrorAndClose(400);
            }
            else
            {
                new Request(this._host, this).Process();
            }
        }

        public byte[] ReadRequestBytes(int maxBytes)
        {
            try
            {
                if (this.WaitForRequestBytes() == 0)
                {
                    return null;
                }
                int available = this._socket.Available;
                if (available > maxBytes)
                {
                    available = maxBytes;
                }
                byte[] buffer = new byte[available];
                if (available > 0)
                {
                    this._socket.Receive(buffer, 0, available, SocketFlags.None);
                }
                return buffer;
            }
            catch
            {
                return null;
            }
        }

        private int WaitForRequestBytes()
        {
            int available = 0;
            try
            {
                if (this._socket.Available == 0)
                {
                    this._socket.Poll(0x186a0, SelectMode.SelectRead);
                    if ((this._socket.Available == 0) && this._socket.Connected)
                    {
                        this._socket.Poll(0x989680, SelectMode.SelectRead);
                    }
                }
                available = this._socket.Available;
            }
            catch
            {
            }
            return available;
        }

        public void Write100Continue()
        {
            this.WriteEntireResponseFromString(100, null, null, true);
        }

        public void WriteBody(byte[] data, int offset, int length)
        {
            this._socket.Send(data, offset, length, SocketFlags.None);
        }

        public void WriteEntireResponseFromString(int statusCode, string extraHeaders, string body, bool keepAlive)
        {
            try
            {
                int contentLength = (body != null) ? Encoding.UTF8.GetByteCount(body) : 0;
                string str = MakeResponseHeaders(statusCode, extraHeaders, contentLength, keepAlive);
                this._socket.Send(Encoding.UTF8.GetBytes(str + body));
            }
            finally
            {
                if (!keepAlive)
                {
                    this.Close();
                }
            }
        }

        public void WriteErrorAndClose(int statusCode)
        {
            this.WriteErrorAndClose(statusCode, null);
        }

        public void WriteErrorAndClose(int statusCode, string message)
        {
            string body = Messages.FormatErrorMessageBody(statusCode, this._host.VirtualPath);
            if ((message != null) && (message.Length > 0))
            {
                body = body + "\r\n<!--\r\n" + message + "\r\n-->";
            }
            this.WriteEntireResponseFromString(statusCode, null, body, false);
        }

        public void WriteHeaders(int statusCode, string extraHeaders)
        {
            string s = MakeResponseHeaders(statusCode, extraHeaders, -1, false);
            this._socket.Send(Encoding.UTF8.GetBytes(s));
        }

        public bool Connected
        {
            get
            {
                return this._socket.Connected;
            }
        }

        public bool IsLocal
        {
            get
            {
                string remoteIP = this.RemoteIP;
                return (remoteIP.Equals("127.0.0.1") || this._host.LocalIP.Equals(remoteIP));
            }
        }

        public string LocalIP
        {
            get
            {
                IPEndPoint localEndPoint = (IPEndPoint) this._socket.LocalEndPoint;
                if ((localEndPoint != null) && (localEndPoint.Address != null))
                {
                    return localEndPoint.Address.ToString();
                }
                return "127.0.0.1";
            }
        }

        public string RemoteIP
        {
            get
            {
                IPEndPoint remoteEndPoint = (IPEndPoint) this._socket.RemoteEndPoint;
                if ((remoteEndPoint != null) && (remoteEndPoint.Address != null))
                {
                    return remoteEndPoint.Address.ToString();
                }
                return "127.0.0.1";
            }
        }
    }
}

