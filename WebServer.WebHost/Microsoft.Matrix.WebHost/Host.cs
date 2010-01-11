namespace Microsoft.Matrix.WebHost
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal sealed class Host : MarshalByRefObject
    {
        private string _installPath;
        private string _localIP;
        private string _lowerCasedClientScriptPathWithTrailingSlash;
        private string _lowerCasedVirtualPath;
        private string _lowerCasedVirtualPathWithTrailingSlash;
        private EventHandler _onAppDomainUnload;
        private WaitCallback _onSocketAccept;
        private WaitCallback _onStart;
        private string _physicalClientScriptPath;
        private string _physicalPath;
        private int _port;
        private Server _server;
        private Socket _socket;
        private bool _started;
        private bool _stopped;
        private string _virtualPath;

        public void Configure(Server server, int port, string virtualPath, string physicalPath, string clientScriptPath)
        {
            this._server = server;
            this._port = port;
            this._installPath = null;
            this._virtualPath = virtualPath;
            this._lowerCasedVirtualPath = CultureInfo.InvariantCulture.TextInfo.ToLower(this._virtualPath);
            this._lowerCasedVirtualPathWithTrailingSlash = virtualPath.EndsWith("/") ? virtualPath : (virtualPath + "/");
            this._lowerCasedVirtualPathWithTrailingSlash = CultureInfo.InvariantCulture.TextInfo.ToLower(this._lowerCasedVirtualPathWithTrailingSlash);
            this._physicalPath = physicalPath;
            this._physicalClientScriptPath = clientScriptPath + @"\";
            this._lowerCasedClientScriptPathWithTrailingSlash = "/aspnet_client/";
            this._onSocketAccept = new WaitCallback(this.OnSocketAccept);
            this._onStart = new WaitCallback(this.OnStart);
            this._onAppDomainUnload = new EventHandler(this.OnAppDomainUnload);
            Thread.GetDomain().DomainUnload += this._onAppDomainUnload;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public bool IsVirtualPathAppPath(string path)
        {
            if (path == null)
            {
                return false;
            }
            path = CultureInfo.InvariantCulture.TextInfo.ToLower(path);
            if (!(path == this._lowerCasedVirtualPath))
            {
                return (path == this._lowerCasedVirtualPathWithTrailingSlash);
            }
            return true;
        }

        public bool IsVirtualPathInApp(string path)
        {
            bool flag;
            return this.IsVirtualPathInApp(path, out flag);
        }

        public bool IsVirtualPathInApp(string path, out bool isClientScriptPath)
        {
            isClientScriptPath = false;
            if (path != null)
            {
                if ((this._virtualPath == "/") && path.StartsWith("/"))
                {
                    if (path.StartsWith(this._lowerCasedClientScriptPathWithTrailingSlash))
                    {
                        isClientScriptPath = true;
                    }
                    return true;
                }
                path = CultureInfo.InvariantCulture.TextInfo.ToLower(path);
                if (path.StartsWith(this._lowerCasedVirtualPathWithTrailingSlash))
                {
                    return true;
                }
                if (path == this._lowerCasedVirtualPath)
                {
                    return true;
                }
                if (path.StartsWith(this._lowerCasedClientScriptPathWithTrailingSlash))
                {
                    isClientScriptPath = true;
                    return true;
                }
            }
            return false;
        }

        private void OnAppDomainUnload(object unusedObject, EventArgs unusedEventArgs)
        {
            Thread.GetDomain().DomainUnload -= this._onAppDomainUnload;
            if (!this._stopped)
            {
                this.Stop();
                this._server.Restart();
                this._server = null;
            }
        }

        private void OnSocketAccept(object acceptedSocket)
        {
            new Microsoft.Matrix.WebHost.Connection(this, (Socket) acceptedSocket).ProcessOneRequest();
        }

        private void OnStart(object unused)
        {
            while (this._started)
            {
                try
                {
                    Socket state = this._socket.Accept();
                    ThreadPool.QueueUserWorkItem(this._onSocketAccept, state);
                    continue;
                }
                catch
                {
                    Thread.Sleep(100);
                    continue;
                }
            }
            this._stopped = true;
        }

        public void Start()
        {
            if (this._started)
            {
                throw new InvalidOperationException();
            }
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._socket.Bind(new IPEndPoint(IPAddress.Any, this._port));
            this._socket.Listen(0x7fffffff);
            this._started = true;
            ThreadPool.QueueUserWorkItem(this._onStart);
        }

        public void Stop()
        {
            if (!this._started)
            {
                return;
            }
            this._started = false;
            try
            {
                this._socket.Close();
                goto Label_0031;
            }
            catch
            {
                goto Label_0031;
            }
            finally
            {
                this._socket = null;
            }
        Label_002A:
            Thread.Sleep(100);
        Label_0031:
            if (!this._stopped)
            {
                goto Label_002A;
            }
        }

        public string InstallPath
        {
            get
            {
                return this._installPath;
            }
        }

        public string LocalIP
        {
            get
            {
                if (this._localIP == null)
                {
                    this._localIP = Dns.GetHostByName(Environment.MachineName).AddressList[0].ToString();
                }
                return this._localIP;
            }
        }

        public string NormalizedClientScriptPath
        {
            get
            {
                return this._lowerCasedClientScriptPathWithTrailingSlash;
            }
        }

        public string NormalizedVirtualPath
        {
            get
            {
                return this._lowerCasedVirtualPathWithTrailingSlash;
            }
        }

        public string PhysicalClientScriptPath
        {
            get
            {
                return this._physicalClientScriptPath;
            }
        }

        public string PhysicalPath
        {
            get
            {
                return this._physicalPath;
            }
        }

        public int Port
        {
            get
            {
                return this._port;
            }
        }

        public string VirtualPath
        {
            get
            {
                return this._virtualPath;
            }
        }
    }
}

