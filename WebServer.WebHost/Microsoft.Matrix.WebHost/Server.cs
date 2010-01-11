namespace Microsoft.Matrix.WebHost
{
    using Microsoft.Win32;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Web;
    using System.Web.Hosting;

    public class Server : MarshalByRefObject
    {
        private string _aspNetVersion;
        private string _clientScriptPath;
        private Host _host;
        private string _installPath;
        private bool _isV10;
        private string _physicalPath;
        private int _port;
        private WaitCallback _restartCallback;
        private string _virtualPath;

        public Server(int port, string virtualPath, string physicalPath)
        {
            this._port = port;
            this._virtualPath = virtualPath;
            this._physicalPath = physicalPath.EndsWith(@"\") ? physicalPath : (physicalPath + @"\");
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(typeof(HttpRuntime).Module.FullyQualifiedName);
            this._aspNetVersion = versionInfo.FileVersion;
            this._isV10 = (versionInfo.FileMajorPart == 1) && (versionInfo.FileMinorPart == 0);
            this.GetInstallPathAndConfigureAspNetIfNeeded();
            this.GetClientScriptPathAndCopyFilesIfNeeded();
            this._restartCallback = new WaitCallback(this.RestartCallback);
            this.CreateHost();
        }

        private void CreateHost()
        {
            try
            { 
                this._host = (Host) ApplicationHost.CreateApplicationHost(typeof(Host), this._virtualPath, this._physicalPath);
                this._host.Configure(this, this._port, this._virtualPath, this._physicalPath, this._clientScriptPath);
            }
            catch (Exception ex)
            {
            }
        }


        private void GetClientScriptPathAndCopyFilesIfNeeded()
        {
            this._clientScriptPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"ASP.NET\Client Files");
            string str = this._aspNetVersion;
            if (!this._isV10)
            {
                str = this._aspNetVersion.Substring(0, this._aspNetVersion.LastIndexOf('.'));
            }
            string path = this._clientScriptPath + @"\system_web\" + str.Replace('.', '_');
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                    foreach (string str3 in Directory.GetFiles(this._installPath + @"\asp.netclientfiles"))
                    {
                        File.Copy(str3, path + @"\" + Path.GetFileName(str3));
                    }
                }
                catch
                {
                }
            }
        }

        private void GetInstallPathAndConfigureAspNetIfNeeded()
        {
            RegistryKey key = null;
            RegistryKey key2 = null;
            RegistryKey key3 = null;
            try
            {
                string name = @"Software\Microsoft\ASP.NET\" + this._aspNetVersion;
                key2 = Registry.LocalMachine.OpenSubKey(name);
                if (key2 != null)
                {
                    this._installPath = (string) key2.GetValue("Path");
                }
                else
                {
                    key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\ASP.NET");
                    if (key == null)
                    {
                        key = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\ASP.NET");
                        key.SetValue("RootVer", this._aspNetVersion);
                    }
                    string str2 = "v" + this._aspNetVersion.Substring(0, this._aspNetVersion.LastIndexOf('.'));
                    key3 = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\.NETFramework");
                    string str3 = (string) key3.GetValue("InstallRoot");
                    if (str3.EndsWith(@"\"))
                    {
                        str3 = str3.Substring(0, str3.Length - 1);
                    }
                    this._installPath = str3 + @"\" + str2;
                    key2 = Registry.LocalMachine.CreateSubKey(name);
                    key2.SetValue("Path", this._installPath);
                    key2.SetValue("DllFullPath", this._installPath + @"\aspnet_isapi.dll");
                }
            }
            catch
            {
            }
            finally
            {
                if (key2 != null)
                {
                    key2.Close();
                }
                if (key != null)
                {
                    key.Close();
                }
                if (key3 != null)
                {
                    key3.Close();
                }
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Restart()
        {
            ThreadPool.QueueUserWorkItem(this._restartCallback);
        }

        private void RestartCallback(object unused)
        {
            this.CreateHost();
            this.Start();
        }

        public void Start()
        {
            if (this._host != null)
            {
                this._host.Start();
            }
        }

        public void Stop()
        {
            if (this._host != null)
            {
                try
                {
                    this._host.Stop();
                }
                catch
                {
                }
            }
        }

        public string ClientScriptPath
        {
            get
            {
                return this._clientScriptPath;
            }
        }

        public string InstallPath
        {
            get
            {
                return this._installPath;
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

        public string RootUrl
        {
            get
            {
                if (this._port != 80)
                {
                    return ("http://localhost:" + this._port + this._virtualPath);
                }
                return ("http://localhost" + this._virtualPath);
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

