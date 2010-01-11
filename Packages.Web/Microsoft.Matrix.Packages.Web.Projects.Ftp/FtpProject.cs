namespace Microsoft.Matrix.Packages.Web.Projects.Ftp
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Packages.Web.Services;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class FtpProject : WorkspaceProject, IWebDocumentRunService
    {
        private FtpConnection _connection;
        private string _httpRoot;
        private string _httpUrl;
        private FtpConnectionProjectItem _rootItem;

        public FtpProject(IProjectFactory factory, IServiceProvider serviceProvider, FtpConnection connection) : base(factory, serviceProvider)
        {
            this._connection = connection;
            this._rootItem = new FtpConnectionProjectItem(this);
            ((IServiceContainer) base.GetService(typeof(IServiceContainer))).AddService(typeof(IWebDocumentRunService), this);
        }

        public override string CombinePath(string path1, string path2)
        {
            if ((path2 == null) || (path2.Length == 0))
            {
                throw new ArgumentNullException("path2");
            }
            if ((path1 != null) && (path1.Length != 0))
            {
                return (path1 + "/" + path2);
            }
            return path2;
        }

        public override string CombineRelativePath(string path1, string path2)
        {
            if ((path1 == null) || (path1.Length == 0))
            {
                throw new ArgumentNullException("path2");
            }
            if ((path2 == null) || (path2.Length == 0))
            {
                throw new ArgumentNullException("path2");
            }
            Uri baseUri = new Uri("ftp://s/" + path1);
            Uri uri2 = new Uri(baseUri, path2);
            return uri2.ToString().Substring(8);
        }

        protected override DocumentProjectItem CreateDocumentInternal(FolderProjectItem parentItem)
        {
            IDocumentManager service = (IDocumentManager) base.GetService(typeof(IDocumentManager));
            return service.CreateDocument(this, parentItem, true);
        }

        protected override FolderProjectItem CreateFolderInternal(FolderProjectItem parentItem)
        {
            string str2;
            IMxUIService service = (IMxUIService) base.GetService(typeof(IMxUIService));
            PromptDialog dialog = new PromptDialog(base.ServiceProvider);
            dialog.Text = "Create New Folder";
            dialog.EntryText = "Enter the name of the new folder:";
            if (service.ShowDialog(dialog) != DialogResult.OK)
            {
                return null;
            }
            string entryValue = dialog.EntryValue;
            if (!this.ValidateProjectItemName(entryValue))
            {
                service.ReportError("The name, '" + entryValue + "' contains invalid characters.", "Create New Folder", false);
                return null;
            }
            if (parentItem == this._rootItem)
            {
                str2 = entryValue;
            }
            else
            {
                str2 = this.CombinePath(parentItem.Path, entryValue);
            }
            this._connection.CreateDirectory(str2);
            return new DirectoryProjectItem(entryValue);
        }

        protected override bool DeleteProjectItemInternal(Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            bool flag = false;
            try
            {
                string path = item.Path;
                if (item is DirectoryProjectItem)
                {
                    this._connection.DeleteDirectory(path);
                }
                else
                {
                    this._connection.DeleteFile(path);
                    try
                    {
                        IDocumentManager service = (IDocumentManager) base.GetService(typeof(IDocumentManager));
                        Document document = service.GetDocument((DocumentProjectItem) item);
                        if (document != null)
                        {
                            service.CloseDocument(document, true);
                        }
                    }
                    catch
                    {
                    }
                }
                flag = true;
            }
            catch
            {
            }
            return flag;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IServiceContainer service = (IServiceContainer) base.GetService(typeof(IServiceContainer));
                if (service != null)
                {
                    service.RemoveService(typeof(IWebDocumentRunService));
                }
                if (this._connection != null)
                {
                    this._connection.Dispose();
                    this._connection = null;
                }
                this._rootItem = null;
            }
            base.Dispose(disposing);
        }

        private DragDropEffects DragDropFileByLocalPath(string sourcePath, string destinationPath)
        {
            DragDropEffects none = DragDropEffects.None;
            Stream sourceStream = null;
            try
            {
                sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                none = this.DragDropFileByStream(sourceStream, destinationPath);
            }
            finally
            {
                if (sourceStream != null)
                {
                    sourceStream.Close();
                    sourceStream = null;
                }
            }
            return none;
        }

        private DragDropEffects DragDropFileByProjectItem(DocumentProjectItem sourceItem, string destinationPath)
        {
            DragDropEffects none = DragDropEffects.None;
            Stream sourceStream = null;
            try
            {
                sourceStream = sourceItem.GetStream(ProjectItemStreamMode.Read);
                if ((sourceItem is FileProjectItem) && (sourceItem.Project == this))
                {
                    MemoryStream stream2 = new MemoryStream();
                    byte[] buffer = new byte[0x1000];
                    while (true)
                    {
                        int count = sourceStream.Read(buffer, 0, 0x1000);
                        if (count <= 0)
                        {
                            break;
                        }
                        stream2.Write(buffer, 0, count);
                    }
                    sourceStream.Close();
                    stream2.Seek(0L, SeekOrigin.Begin);
                    sourceStream = stream2;
                }
                none = this.DragDropFileByStream(sourceStream, destinationPath);
            }
            finally
            {
                if (sourceStream != null)
                {
                    sourceStream.Close();
                    sourceStream = null;
                }
            }
            return none;
        }

        private DragDropEffects DragDropFileByStream(Stream sourceStream, string destinationPath)
        {
            DragDropEffects none = DragDropEffects.None;
            Stream fileStream = null;
            try
            {
                fileStream = this._connection.GetFileStream(destinationPath, FileAccess.Write);
                byte[] buffer = new byte[0x1000];
                while (true)
                {
                    int count = sourceStream.Read(buffer, 0, 0x1000);
                    if (count <= 0)
                    {
                        break;
                    }
                    fileStream.Write(buffer, 0, count);
                }
                none = DragDropEffects.Copy;
            }
            catch (Exception exception)
            {
                ((IMxUIService) base.GetService(typeof(IUIService))).ReportError(exception.Message, "Error copying the file to the target folder.", false);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream = null;
                }
            }
            return none;
        }

        protected override ProjectItemAttributes GetProjectItemAttributesInternal(Microsoft.Matrix.Core.Projects.ProjectItem projectItem)
        {
            return ProjectItemAttributes.Normal;
        }

        protected override string GetProjectItemDisplayNameInternal(Microsoft.Matrix.Core.Projects.ProjectItem projectItem)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ftp://");
            builder.Append(this._connection.RemoteHostName);
            if (this._connection.RemotePort != 0x15)
            {
                builder.Append(':');
                builder.Append(this._connection.RemotePort.ToString());
            }
            builder.Append('/');
            builder.Append(this.GetProjectItemPathInternal(projectItem));
            return builder.ToString();
        }

        protected override DocumentType GetProjectItemDocumentTypeInternal(Microsoft.Matrix.Core.Projects.ProjectItem projectItem)
        {
            if (projectItem is FileProjectItem)
            {
                IDocumentTypeManager service = (IDocumentTypeManager) base.GetService(typeof(IDocumentTypeManager));
                if (service != null)
                {
                    string extension = Path.GetExtension(projectItem.Caption);
                    return service.GetDocumentType(extension);
                }
            }
            return null;
        }

        protected override DragDropEffects GetProjectItemDragDropEffectsInternal(Microsoft.Matrix.Core.Projects.ProjectItem item, IDataObject dataObject)
        {
            if (dataObject.GetDataPresent(typeof(Microsoft.Matrix.Core.Projects.ProjectItem).Name))
            {
                Microsoft.Matrix.Core.Projects.ProjectItem data = dataObject.GetData(typeof(Microsoft.Matrix.Core.Projects.ProjectItem).Name) as Microsoft.Matrix.Core.Projects.ProjectItem;
                if (((data != null) && (data is DocumentProjectItem)) && (data.Project is WorkspaceProject))
                {
                    return DragDropEffects.Copy;
                }
            }
            else if (dataObject.GetDataPresent(DataFormats.FileDrop))
            {
                string[] strArray = (string[]) dataObject.GetData(DataFormats.FileDrop);
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (File.Exists(strArray[i]))
                    {
                        return DragDropEffects.Copy;
                    }
                }
            }
            return DragDropEffects.None;
        }

        protected override string GetProjectItemPathInternal(Microsoft.Matrix.Core.Projects.ProjectItem projectItem)
        {
            if (projectItem == this._rootItem)
            {
                return string.Empty;
            }
            MiscFileProjectItem item = projectItem as MiscFileProjectItem;
            if (item != null)
            {
                return item.Path;
            }
            if (projectItem.Parent == this._rootItem)
            {
                return projectItem.Caption;
            }
            return (projectItem.Parent.Path + "/" + projectItem.Caption);
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
                Project project = projectItem.Project;
                if (project != this)
                {
                    throw new ArgumentException("Unable to run the specified document", "document");
                }
                string httpUrl = this.HttpUrl;
                if (httpUrl.Length == 0)
                {
                    IUIService service = (IUIService) base.GetService(typeof(IUIService));
                    DialogResult cancel = DialogResult.Cancel;
                    if (service != null)
                    {
                        string displayName = project.ProjectItem.DisplayName;
                        if (this.HttpRoot.Length != 0)
                        {
                            displayName = displayName + this.HttpRoot;
                        }
                        PromptDialog form = new PromptDialog(base.ServiceProvider);
                        form.EntryText = "Enter the URL for your FTP connection's Web directory '" + displayName + "':";
                        cancel = service.ShowDialog(form);
                        if (cancel == DialogResult.OK)
                        {
                            httpUrl = form.EntryValue;
                            if (httpUrl.EndsWith("/"))
                            {
                                httpUrl = httpUrl.Substring(0, httpUrl.Length - 1);
                            }
                            this.HttpUrl = httpUrl;
                        }
                    }
                    if (cancel == DialogResult.Cancel)
                    {
                        return string.Empty;
                    }
                }
                string path = projectItem.Path;
                if (this.HttpRoot.Length != 0)
                {
                    if (!path.StartsWith(this.HttpRoot))
                    {
                        IUIService service2 = (IUIService) base.GetService(typeof(IUIService));
                        if (service2 != null)
                        {
                            service2.ShowMessage("'" + projectItem.Caption + "' exists outside the Web directory, and cannot be run.");
                        }
                        return string.Empty;
                    }
                    path = path.Substring(this.HttpRoot.Length + 1);
                }
                return (httpUrl + "/" + path);
            }
            catch (Exception)
            {
            }
            return null;
        }

        protected override Stream GetProjectItemStream(DocumentProjectItem item, ProjectItemStreamMode mode)
        {
            string projectItemPathInternal = this.GetProjectItemPathInternal(item);
            if (mode == ProjectItemStreamMode.Read)
            {
                return this._connection.GetFileStream(projectItemPathInternal, FileAccess.Read);
            }
            return this._connection.GetFileStream(projectItemPathInternal, FileAccess.Write);
        }

        protected override string GetProjectItemUrlInternal(Microsoft.Matrix.Core.Projects.ProjectItem projectItem)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ftp://");
            this.GetProjectItemPathInternal(projectItem);
            if (!this._connection.IsAnonymous)
            {
                builder.Append(this._connection.UserName);
                builder.Append(':');
                builder.Append(this._connection.Password);
                builder.Append('@');
            }
            builder.Append(this._connection.RemoteHostName);
            if (this._connection.RemotePort != 0x15)
            {
                builder.Append(':');
                builder.Append(this._connection.RemotePort.ToString());
            }
            builder.Append('/');
            builder.Append(this.GetProjectItemPathInternal(projectItem));
            return builder.ToString();
        }

        public override DocumentProjectItem GetSaveAsProjectItem(DocumentProjectItem item)
        {
            DocumentProjectItem item2 = null;
            PromptDialog dialog = new PromptDialog(base.ServiceProvider);
            dialog.Text = "Save Document As";
            dialog.EntryText = "Enter the full path to the new FTP location for the document:\n";
            dialog.EntryValue = item.Path;
            dialog.RequiresNewValue = true;
            ICommandManager service = (ICommandManager) base.GetService(typeof(ICommandManager));
            if (service != null)
            {
                service.SuspendCommandUpdate();
            }
            try
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return item2;
                }
                string entryValue = dialog.EntryValue;
                if (!this.ValidateProjectItemPath(entryValue, false))
                {
                    throw new Exception("'" + entryValue + "' is not a valid file path.");
                }
                string caption = entryValue;
                int num = entryValue.LastIndexOf('/');
                if (num > 0)
                {
                    caption = entryValue.Substring(num + 1);
                }
                item2 = new MiscFileProjectItem(caption, entryValue, this);
            }
            finally
            {
                if (service != null)
                {
                    service.ResumeCommandUpdate();
                }
            }
            return item2;
        }

        protected override DragDropEffects HandleProjectItemDragDropInternal(Microsoft.Matrix.Core.Projects.ProjectItem item, IDataObject dataObject)
        {
            if (dataObject.GetDataPresent(typeof(Microsoft.Matrix.Core.Projects.ProjectItem).Name))
            {
                Microsoft.Matrix.Core.Projects.ProjectItem data = dataObject.GetData(typeof(Microsoft.Matrix.Core.Projects.ProjectItem).Name) as Microsoft.Matrix.Core.Projects.ProjectItem;
                if (data != null)
                {
                    string destinationPath = this.CombinePath(this.GetProjectItemPathInternal(item), data.Caption);
                    return this.DragDropFileByProjectItem((DocumentProjectItem) data, destinationPath);
                }
            }
            else if (dataObject.GetDataPresent(DataFormats.FileDrop))
            {
                string[] strArray = (string[]) dataObject.GetData(DataFormats.FileDrop);
                DragDropEffects none = DragDropEffects.None;
                for (int i = 0; i < strArray.Length; i++)
                {
                    string path = strArray[i];
                    if (File.Exists(path))
                    {
                        string str3 = this.CombinePath(this.GetProjectItemPathInternal(item), Path.GetFileName(path));
                        if (this.DragDropFileByLocalPath(path, str3) == DragDropEffects.Copy)
                        {
                            none = DragDropEffects.Copy;
                        }
                    }
                }
                return none;
            }
            return DragDropEffects.None;
        }

        void IWebDocumentRunService.Run(Document document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            if (base.ServiceProvider == null)
            {
                throw new ApplicationException("Cannot run the document");
            }
            string projectItemRunUrl = this.GetProjectItemRunUrl(document);
            if (projectItemRunUrl == null)
            {
                IUIService service = (IUIService) base.GetService(typeof(IUIService));
                if (service != null)
                {
                    service.ShowMessage("Unable to generate the URL for document to run it.", "Run");
                }
            }
            else if (projectItemRunUrl.Length != 0)
            {
                IWebBrowsingService service2 = (IWebBrowsingService) base.GetService(typeof(IWebBrowsingService));
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

        public override Document OpenProjectItem(DocumentProjectItem item, bool readOnly, DocumentViewType initialView)
        {
            IDocumentManager service = (IDocumentManager) base.GetService(typeof(IDocumentManager));
            if (service == null)
            {
                return null;
            }
            MiscFileProjectItem projectItem = item as MiscFileProjectItem;
            if (projectItem == null)
            {
                projectItem = new MiscFileProjectItem(item.Caption, item.Path, this);
            }
            return service.OpenDocument(projectItem, readOnly, initialView);
        }

        protected override Microsoft.Matrix.Core.Projects.ProjectItem ParsePathInternal(string path, bool newFile)
        {
            if ((path == null) || (path.Length == 0))
            {
                return null;
            }
            if (!newFile && !this._connection.GetFileExists(path))
            {
                return null;
            }
            string caption = path;
            int num = path.LastIndexOf('/');
            if ((num >= 0) && (num < (path.Length - 1)))
            {
                caption = path.Substring(num + 1);
            }
            return new MiscFileProjectItem(caption, path, this);
        }

        public override bool ProjectItemExists(string itemPath)
        {
            return this._connection.GetFileExists(itemPath);
        }

        public override bool ValidateProjectItemName(string itemName)
        {
            char[] anyOf = new char[] { 
                '"', '<', '>', '|', '/', '\\', '?', '*', ':', '\0', '\b', '\x0010', '\x0011', '\x0012', '\x0014', '\x0015', 
                '\x0016', '\x0017', '\x0018', '\x0019'
             };
            return ((((itemName != null) && (itemName.Length != 0)) && ((itemName.IndexOf(Path.DirectorySeparatorChar) < 0) && (itemName.IndexOf(Path.VolumeSeparatorChar) < 0))) && (itemName.IndexOfAny(anyOf) < 0));
        }

        public override bool ValidateProjectItemPath(string itemPath, bool createIfNeeded)
        {
            if ((itemPath == null) || (itemPath.Length == 0))
            {
                return false;
            }
            if (createIfNeeded)
            {
                throw new NotSupportedException();
            }
            char[] anyOf = new char[] { 
                '"', '<', '>', '|', '?', '*', '\0', '\b', '\x0010', '\x0011', '\x0012', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', 
                '\x0019'
             };
            return ((itemPath.IndexOfAny(anyOf) < 0) && (itemPath[itemPath.Length - 1] != '/'));
        }

        public FtpConnection Connection
        {
            get
            {
                return this._connection;
            }
        }

        internal string HttpRoot
        {
            get
            {
                if (this._httpRoot == null)
                {
                    return string.Empty;
                }
                return this._httpRoot;
            }
            set
            {
                this._httpRoot = value;
            }
        }

        internal string HttpUrl
        {
            get
            {
                if (this._httpUrl == null)
                {
                    return string.Empty;
                }
                return this._httpUrl;
            }
            set
            {
                this._httpUrl = value;
            }
        }

        public override RootProjectItem ProjectItem
        {
            get
            {
                return this._rootItem;
            }
        }

        public override IComparer ProjectItemComparer
        {
            get
            {
                return new FtpProjectItemComparer();
            }
        }

        private class FtpProjectItemComparer : IComparer
        {
            int IComparer.Compare(object o1, object o2)
            {
                if (o1 is DirectoryProjectItem)
                {
                    if (o2 is FileProjectItem)
                    {
                        return -1;
                    }
                    if (o1 is DirectoryProjectItem)
                    {
                        ProjectItem item = (ProjectItem) o1;
                        ProjectItem item2 = (ProjectItem) o2;
                        return string.Compare(item.Caption, item2.Caption, true);
                    }
                    return 1;
                }
                if (o1 is FileProjectItem)
                {
                    if (o2 is DirectoryProjectItem)
                    {
                        return 1;
                    }
                    if (o1 is FileProjectItem)
                    {
                        ProjectItem item3 = (ProjectItem) o1;
                        ProjectItem item4 = (ProjectItem) o2;
                        return string.Compare(item3.Caption, item4.Caption, true);
                    }
                }
                return 1;
            }
        }
    }
}

