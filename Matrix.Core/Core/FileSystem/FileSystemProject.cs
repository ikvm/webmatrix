namespace Microsoft.Matrix.Core.Projects.FileSystem
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public abstract class FileSystemProject : WorkspaceProject
    {
        public FileSystemProject(IProjectFactory factory, IServiceProvider serviceProvider) : base(factory, serviceProvider)
        {
        }

        public override string CombinePath(string path1, string path2)
        {
            if ((path2 == null) || (path2.Length == 0))
            {
                throw new ArgumentNullException("path2");
            }
            string str = Path.Combine(path1, path2);
            if (str.Length > 0x100)
            {
                throw new ArgumentException(string.Format("Path cannot be longer than {0} characters", 0x100));
            }
            return str;
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
            string fullPath = Path.GetFullPath(Path.Combine(path1, Path.Combine("..", path2)));
            if (fullPath.Length > 0x100)
            {
                throw new ArgumentException(string.Format("Path cannot be longer than {0} characters", 0x100));
            }
            return fullPath;
        }

        protected internal override DocumentProjectItem CreateDocumentInternal(FolderProjectItem parentItem)
        {
            IDocumentManager service = (IDocumentManager) base.GetService(typeof(IDocumentManager));
            return service.CreateDocument(this, parentItem, true);
        }

        protected internal override FolderProjectItem CreateFolderInternal(FolderProjectItem parentItem)
        {
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
            string path = this.CombinePath(parentItem.Path, entryValue);
            if (Directory.Exists(path))
            {
                throw new ArgumentException("A folder with the path '" + path + "' already exists.");
            }
            Directory.CreateDirectory(path);
            return new DirectoryProjectItem(entryValue);
        }

        protected internal override bool DeleteProjectItemInternal(ProjectItem item)
        {
            bool flag = false;
            try
            {
                string path = item.Path;
                if (item is DirectoryProjectItem)
                {
                    Directory.Delete(path);
                }
                else
                {
                    File.Delete(path);
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

        private DragDropEffects DragDropFileByLocalPath(string sourcePath, string destinationPath, bool copy)
        {
            if (File.Exists(destinationPath))
            {
                IMxUIService service = (IMxUIService) base.GetService(typeof(IUIService));
                if (service.ShowMessage("A file with the name '" + Path.GetFileName(destinationPath) + "' already exists in the target folder.\r\nDo you want to overwrite it?", string.Empty, MessageBoxIcon.Question, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return DragDropEffects.None;
                }
            }
            DragDropEffects none = DragDropEffects.None;
            try
            {
                if (copy)
                {
                    File.Copy(sourcePath, destinationPath, true);
                    return DragDropEffects.Copy;
                }
                File.Move(sourcePath, destinationPath);
                none = DragDropEffects.Move;
            }
            catch (Exception exception)
            {
                IMxUIService service2 = (IMxUIService) base.GetService(typeof(IUIService));
                if (copy)
                {
                    service2.ReportError(exception.Message, "Error copying the file to the target folder.", false);
                    return none;
                }
                service2.ReportError(exception.Message, "Error moving the file to the target folder.", false);
            }
            return none;
        }

        private DragDropEffects DragDropFileByProjectItem(DocumentProjectItem sourceItem, string destinationPath)
        {
            if (File.Exists(destinationPath))
            {
                IMxUIService service = (IMxUIService) base.GetService(typeof(IUIService));
                if (service.ShowMessage("A file with the name '" + Path.GetFileName(destinationPath) + "' already exists in the target folder.\r\nDo you want to overwrite it?", string.Empty, MessageBoxIcon.Question, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return DragDropEffects.None;
                }
            }
            DragDropEffects none = DragDropEffects.None;
            Stream stream = null;
            FileStream stream2 = null;
            try
            {
                stream = sourceItem.GetStream(ProjectItemStreamMode.Read);
                stream2 = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
                byte[] buffer = new byte[0x1000];
                while (true)
                {
                    int count = stream.Read(buffer, 0, 0x1000);
                    if (count <= 0)
                    {
                        break;
                    }
                    stream2.Write(buffer, 0, count);
                }
                none = DragDropEffects.Copy;
            }
            catch (Exception exception)
            {
                ((IMxUIService) base.GetService(typeof(IUIService))).ReportError(exception.Message, "Error copying the file to the target folder.", false);
            }
            finally
            {
                if (stream2 != null)
                {
                    stream2.Close();
                    stream2 = null;
                }
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
            return none;
        }

        protected internal override ProjectItemAttributes GetProjectItemAttributesInternal(ProjectItem projectItem)
        {
            if ((File.GetAttributes(projectItem.Path) & FileAttributes.ReadOnly) != 0)
            {
                return ProjectItemAttributes.ReadOnly;
            }
            return ProjectItemAttributes.Normal;
        }

        protected internal override DocumentType GetProjectItemDocumentTypeInternal(ProjectItem projectItem)
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

        protected override DragDropEffects GetProjectItemDragDropEffectsInternal(ProjectItem item, IDataObject dataObject)
        {
            if (dataObject.GetDataPresent(typeof(ProjectItem).Name))
            {
                ProjectItem data = dataObject.GetData(typeof(ProjectItem).Name) as ProjectItem;
                if (((data != null) && (data is DocumentProjectItem)) && (data.Project is WorkspaceProject))
                {
                    Project project = data.Project;
                    if ((project == this) || (project is FileSystemProject))
                    {
                        if ((Control.ModifierKeys & Keys.Control) != Keys.None)
                        {
                            return DragDropEffects.Copy;
                        }
                        string projectItemPathInternal = this.GetProjectItemPathInternal(item);
                        if (Path.GetPathRoot(this.GetProjectItemPathInternal(data)).Equals(Path.GetPathRoot(projectItemPathInternal)))
                        {
                            return DragDropEffects.Move;
                        }
                    }
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

        protected internal override string GetProjectItemPathInternal(ProjectItem projectItem)
        {
            if (projectItem is IMiscProjectItem)
            {
                return projectItem.Path;
            }
            return Path.Combine(projectItem.Parent.Path, projectItem.Caption);
        }

        protected internal override Stream GetProjectItemStream(DocumentProjectItem item, ProjectItemStreamMode mode)
        {
            string projectItemPathInternal = this.GetProjectItemPathInternal(item);
            if (mode == ProjectItemStreamMode.Read)
            {
                return File.Open(projectItemPathInternal, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            return File.Open(projectItemPathInternal, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        }

        protected internal override string GetProjectItemUrlInternal(ProjectItem projectItem)
        {
            return ("file:///" + this.GetProjectItemPathInternal(projectItem));
        }

        public override DocumentProjectItem GetSaveAsProjectItem(DocumentProjectItem item)
        {
            DocumentProjectItem item2 = null;
            IDocumentTypeManager service = (IDocumentTypeManager) base.GetService(typeof(IDocumentTypeManager));
            if (service == null)
            {
                return null;
            }
            string openFilters = service.OpenFilters;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = openFilters;
            dialog.AddExtension = true;
            dialog.CheckPathExists = true;
            dialog.DefaultExt = item.DocumentType.Extension;
            dialog.FileName = Path.GetFileNameWithoutExtension(item.Caption);
            dialog.InitialDirectory = Path.GetDirectoryName(item.Path);
            dialog.OverwritePrompt = true;
            dialog.Title = "Save Document As";
            openFilters = openFilters.ToUpper(CultureInfo.InvariantCulture);
            int num = openFilters.LastIndexOf("." + item.DocumentType.Extension + "|");
            if (num == -1)
            {
                num = openFilters.LastIndexOf("." + item.DocumentType.Extension + ";");
            }
            if (num != -1)
            {
                int num2 = -1;
                int num3 = 0;
                while (num3 != -1)
                {
                    int index = openFilters.IndexOf('|', num3 + 1);
                    if (index != -1)
                    {
                        num3 = openFilters.IndexOf('|', index + 1);
                    }
                    if (num3 == -1)
                    {
                        num2 = -1;
                        break;
                    }
                    num2++;
                    if (num3 > num)
                    {
                        break;
                    }
                }
                if (num2 != -1)
                {
                    num2++;
                    dialog.FilterIndex = num2;
                }
            }
            ICommandManager manager2 = (ICommandManager) base.GetService(typeof(ICommandManager));
            if (manager2 != null)
            {
                manager2.SuspendCommandUpdate();
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                item2 = new MiscFileProjectItem(Path.GetFileName(dialog.FileName), dialog.FileName, this);
            }
            if (manager2 != null)
            {
                manager2.ResumeCommandUpdate();
            }
            return item2;
        }

        protected override bool HandleProjectItemCommand(Command command, ProjectItem item)
        {
            bool flag = false;
            if ((command.CommandGroup == typeof(GlobalCommands)) && (command.CommandID == 720))
            {
                this.OnCommandCreateShortcut(item);
                flag = true;
            }
            if (!flag)
            {
                flag = base.HandleProjectItemCommand(command, item);
            }
            return flag;
        }

        protected override DragDropEffects HandleProjectItemDragDropInternal(ProjectItem item, IDataObject dataObject)
        {
            if (dataObject.GetDataPresent(typeof(ProjectItem).Name))
            {
                ProjectItem data = dataObject.GetData(typeof(ProjectItem).Name) as ProjectItem;
                if (data != null)
                {
                    string destinationPath = Path.Combine(this.GetProjectItemPathInternal(item), data.Caption);
                    if (!(data.Project is FileSystemProject))
                    {
                        return this.DragDropFileByProjectItem((DocumentProjectItem) data, destinationPath);
                    }
                    string projectItemPathInternal = this.GetProjectItemPathInternal(data);
                    bool copy = (Control.ModifierKeys & Keys.Control) != Keys.None;
                    if (!copy && !Path.GetPathRoot(projectItemPathInternal).Equals(Path.GetPathRoot(destinationPath)))
                    {
                        copy = true;
                    }
                    return this.DragDropFileByLocalPath(projectItemPathInternal, destinationPath, copy);
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
                        string str4 = Path.Combine(this.GetProjectItemPathInternal(item), Path.GetFileName(path));
                        if (this.DragDropFileByLocalPath(path, str4, true) == DragDropEffects.Copy)
                        {
                            none = DragDropEffects.Copy;
                        }
                    }
                }
                return none;
            }
            return DragDropEffects.None;
        }

        private void OnCommandCreateShortcut(ProjectItem item)
        {
            if (item is DirectoryProjectItem)
            {
                string path = item.Path;
                ((IProjectManager) base.GetService(typeof(IProjectManager))).CreateProject(typeof(ShortcutProjectFactory), path);
            }
        }

        protected override ProjectItem ParsePathInternal(string path, bool newFile)
        {
            try
            {
                path = Path.GetFullPath(path);
            }
            catch
            {
                return null;
            }
            if (!Path.IsPathRooted(path))
            {
                return null;
            }
            if (!newFile && !File.Exists(path))
            {
                return null;
            }
            return new MiscFileProjectItem(Path.GetFileName(path), path, this);
        }

        public override bool ProjectItemExists(string itemPath)
        {
            if ((itemPath == null) || (itemPath.Length == 0))
            {
                throw new ArgumentNullException("itemPath");
            }
            return File.Exists(itemPath);
        }

        protected override bool UpdateProjectItemCommand(Command command, ProjectItem item)
        {
            bool flag = false;
            if (((command.CommandGroup == typeof(GlobalCommands)) && (command.CommandID == 720)) && (item is DirectoryProjectItem))
            {
                command.Text = "Create Shortcut";
                flag = true;
            }
            if (!flag)
            {
                flag = base.UpdateProjectItemCommand(command, item);
            }
            return flag;
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
            char[] anyOf = new char[] { 
                '"', '<', '>', '|', '?', '*', '\0', '\b', '\x0010', '\x0011', '\x0012', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', 
                '\x0019'
             };
            try
            {
                if (((itemPath.IndexOfAny(anyOf) >= 0) || !Path.IsPathRooted(itemPath)) || ((Path.GetFullPath(itemPath) == null) || (itemPath.Length > 0x100)))
                {
                    return false;
                }
                if (createIfNeeded)
                {
                    Directory.CreateDirectory(itemPath);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override IComparer ProjectItemComparer
        {
            get
            {
                return new FileSystemProjectItemComparer();
            }
        }

        private class FileSystemProjectItemComparer : IComparer
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

