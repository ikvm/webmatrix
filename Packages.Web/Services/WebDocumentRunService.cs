namespace Microsoft.Matrix.Packages.Web.Services
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.Projects.FileSystem;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class WebDocumentRunService : IWebDocumentRunService, IDisposable
    {
        private IDictionary _localDirectoryMappings;
        private IServiceProvider _serviceProvider;

        public WebDocumentRunService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        private string GetLocalFileProjectItemRunUrl(DocumentProjectItem projectItem)
        {
            string path = projectItem.Path;
            string directoryName = Path.GetDirectoryName(path);
            string str3 = null;
            string appDirectory = null;
            string uriString = null;
            if (this._localDirectoryMappings != null)
            {
                for (string str6 = directoryName; str6 != null; str6 = Path.GetDirectoryName(str6))
                {
                    uriString = (string) this._localDirectoryMappings[str6.ToLower()];
                    if (uriString != null)
                    {
                        appDirectory = str6;
                        break;
                    }
                }
                if (uriString != null)
                {
                    Socket socket = null;
                    bool flag = false;
                    try
                    {
                        Uri uri = new Uri(uriString);
                        IPAddress address = Dns.GetHostByName(uri.Host).AddressList[0];
                        if (IPAddress.IsLoopback(address) || address.Equals(Dns.GetHostByName(Environment.MachineName).AddressList[0]))
                        {
                            IPEndPoint remoteEP = new IPEndPoint(address, uri.Port);
                            socket = new Socket(remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                            socket.Connect(remoteEP);
                            if (!socket.Connected)
                            {
                                flag = true;
                            }
                        }
                    }
                    catch
                    {
                        flag = true;
                    }
                    finally
                    {
                        if (socket != null)
                        {
                            socket.Close();
                        }
                    }
                    if (flag)
                    {
                        this._localDirectoryMappings.Remove(appDirectory.ToLower());
                        uriString = null;
                        appDirectory = null;
                    }
                }
            }
            if (uriString == null)
            {
                IUIService service = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
                DialogResult cancel = DialogResult.Cancel;
                if (service != null)
                {
                    StartWebAppDialog form = new StartWebAppDialog(this._serviceProvider, directoryName);
                    cancel = service.ShowDialog(form);
                    if (cancel == DialogResult.OK)
                    {
                        appDirectory = form.AppDirectory;
                        uriString = form.AppUrl;
                        if (this._localDirectoryMappings == null)
                        {
                            this._localDirectoryMappings = new HybridDictionary(false);
                        }
                        this._localDirectoryMappings[appDirectory.ToLower()] = uriString;
                    }
                }
                if (cancel == DialogResult.Cancel)
                {
                    return string.Empty;
                }
            }
            if (uriString == null)
            {
                return str3;
            }
            if (directoryName.Equals(appDirectory))
            {
                return (uriString + "/" + projectItem.Caption);
            }
            string[] strArray = path.Split(new char[] { '\\' });
            string[] strArray2 = appDirectory.Split(new char[] { '\\' });
            StringBuilder builder = new StringBuilder(uriString, 0x400);
            for (int i = strArray2.Length; i < strArray.Length; i++)
            {
                builder.Append('/');
                builder.Append(strArray[i]);
            }
            return builder.ToString();
        }

        private string GetProjectItemRunUrl(Document document)
        {
            try
            {
                DocumentProjectItem projectItem = document.ProjectItem;
                if (projectItem == null)
                {
                    throw new ArgumentException("Document does not have an associated project item", "document");
                }
                if (projectItem.Project.ProjectFactory.GetType() == typeof(MyComputerProjectFactory))
                {
                    return this.GetLocalFileProjectItemRunUrl(projectItem);
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        void IWebDocumentRunService.Run(Document document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            if (this._serviceProvider == null)
            {
                throw new ApplicationException("Cannot run the document");
            }
            string projectItemRunUrl = this.GetProjectItemRunUrl(document);
            if (projectItemRunUrl == null)
            {
                IUIService service = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
                if (service != null)
                {
                    service.ShowMessage("Unable to generate the URL for document to run it.", "Run");
                }
            }
            else if (projectItemRunUrl.Length != 0)
            {
                IWebBrowsingService service2 = (IWebBrowsingService) this._serviceProvider.GetService(typeof(IWebBrowsingService));
                if (service2 != null)
                {
                    service2.BrowseUrl(projectItemRunUrl);
                }
                else
                {
                    try
                    {
                        Process.Start(projectItemRunUrl);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        void IDisposable.Dispose()
        {
            this._localDirectoryMappings = null;
            this._serviceProvider = null;
        }
    }
}

