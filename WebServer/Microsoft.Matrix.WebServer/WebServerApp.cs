namespace Microsoft.Matrix.WebServer
{
    using Microsoft.Matrix.Utility;
    using Microsoft.Matrix.WebHost;
    using System;
    using System.IO;
    using System.Windows.Forms;

    public sealed class WebServerApp
    {
        [STAThread]
        public static int Main(string[] args)
        {
            CommandLine line = new CommandLine(args);
            bool flag = line.Options["silent"] != null;
            if (!flag && line.ShowHelp)
            {
                ShowUsage();
                return 0;
            }
            string virtualPath = (string) line.Options["vpath"];
            if (virtualPath != null)
            {
                virtualPath = virtualPath.Trim();
            }
            if ((virtualPath == null) || (virtualPath.Length == 0))
            {
                virtualPath = "/";
            }
            else if (!virtualPath.StartsWith("/"))
            {
                if (!flag)
                {
                    ShowUsage();
                }
                return -1;
            }
            string path = (string) line.Options["path"];
            if (path != null)
            {
                path = path.Trim();
            }
            if ((path == null) || (path.Length == 0))
            {
                if (!flag)
                {
                    ShowUsage();
                }
                return -1;
            }
            if (!Directory.Exists(path))
            {
                if (!flag)
                {
                    ShowMessage("The directory '" + path + "' does not exist.");
                }
                return -2;
            }
            int port = 0;
            string s = (string) line.Options["port"];
            if (s != null)
            {
                s = s.Trim();
            }
            if ((s != null) && (s.Length != 0))
            {
                try
                {
                    port = int.Parse(s);
                    if ((port >= 1) && (port <= 0xffff))
                    {
                        goto Label_0153;
                    }
                    if (!flag)
                    {
                        ShowUsage();
                    }
                    return -1;
                }
                catch
                {
                    if (!flag)
                    {
                        ShowMessage("Invalid port '" + s + "'");
                    }
                    return -3;
                }
            }
            port = 80;
        Label_0153:
            try
            {
                Server server = new Server(port, virtualPath, path);
                server.Start();
                Application.Run(new WebServerForm(server));
            }
            catch (Exception exception)
            {
                if (!flag)
                {
                    ShowMessage(string.Concat(new object[] { "Web Server failed to start listening on port ", port, ".\r\nError message:\r\n", exception.Message }));
                }
                return -4;
            }
            return 0;
        }

        private static void ShowMessage(string msg)
        {
            MessageBox.Show(msg, "Microsoft ASP.NET Web Matrix Server", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        private static void ShowMessage(string msg, MessageBoxIcon icon)
        {
            MessageBox.Show(msg, "Microsoft ASP.NET Web Matrix Server", MessageBoxButtons.OK, icon);
        }

        private static void ShowUsage()
        {
            string msg = "Microsoft ASP.NET Web Matrix Server Usage:\r\nWebServer /port:<port number> /path:<physical path> [/vpath:<virtual path>]\r\n\r\n    port number:\r\n        [Optional] An unused port number between 1 and 65535.\r\n        The default is 80 (usable if you do not also have IIS listening on the same port).\r\n\r\n    physical path:\r\n        A valid directory name where the Web application is rooted.\r\n\r\n    virtual path:\r\n        [Optional] The virtual path or application root in the form of '/<app name>'.\r\n        The default is simply '/'.\r\n\r\nExample:\r\nWebServer /port:8080 /path:\"c:\\inetpub\\wwwroot\\MyApp\" /vpath=\"/MyApp\"\r\n\r\nYou can then access the Web application using a URL of the form:\r\n    http://localhost:8080/MyApp";
            ShowMessage(msg, MessageBoxIcon.Asterisk);
        }
    }
}

