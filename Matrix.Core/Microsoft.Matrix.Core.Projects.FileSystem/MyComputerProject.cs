namespace Microsoft.Matrix.Core.Projects.FileSystem
{
    using Microsoft.Matrix;
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;

    internal sealed class MyComputerProject : FileSystemProject, ILocalFileSystemProject
    {
        private Microsoft.Matrix.Utility.MruList _mruList;
        private MyComputerProjectItem _rootItem;
        private const int MaximumMruEntries = 10;

        public MyComputerProject(IProjectFactory factory, IServiceProvider serviceProvider) : base(factory, serviceProvider)
        {
            this._rootItem = new MyComputerProjectItem(this);
            this.LoadMruEntries();
        }

        private void AddMruEntry(DocumentProjectItem projectItem)
        {
            string path = projectItem.Path;
            this.MruList.AddEntry(path);
            try
            {
                Interop.SHAddToRecentDocs(Interop.SHARED_PATH, path);
            }
            catch
            {
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.SaveMruEntries();
                this._rootItem = null;
            }
            base.Dispose(disposing);
        }

        protected internal override string GetProjectItemPathInternal(Microsoft.Matrix.Core.Projects.ProjectItem projectItem)
        {
            if (projectItem == this._rootItem)
            {
                return string.Empty;
            }
            return base.GetProjectItemPathInternal(projectItem);
        }

        public override DocumentProjectItem GetSaveAsProjectItem(DocumentProjectItem item)
        {
            DocumentProjectItem saveAsProjectItem = base.GetSaveAsProjectItem(item);
            if (saveAsProjectItem != null)
            {
                this.AddMruEntry(saveAsProjectItem);
            }
            return saveAsProjectItem;
        }

        protected override bool HandleProjectItemCommand(Command command, Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            bool flag = false;
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 1:
                        this.OnCommandFileNew();
                        flag = true;
                        break;

                    case 2:
                        this.OnCommandFileOpen();
                        flag = true;
                        break;

                    case 20:
                    case 0x15:
                    case 0x16:
                    case 0x17:
                    case 0x18:
                    case 0x19:
                    case 0x1a:
                    case 0x1b:
                    case 0x1c:
                    case 0x1d:
                        this.OnCommandFileFileMRU(command.CommandID - 20);
                        flag = true;
                        break;
                }
            }
            if (!flag)
            {
                flag = base.HandleProjectItemCommand(command, item);
            }
            return flag;
        }

        private void LoadMruEntries()
        {
            IPreferencesService service = (IPreferencesService) base.GetService(typeof(IPreferencesService));
            if (service != null)
            {
                PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(MyComputerProject));
                int num = preferencesStore.GetValue("MruCount", 0);
                if (num != 0)
                {
                    string[] entries = new string[num];
                    for (int i = 0; i < num; i++)
                    {
                        entries[i] = preferencesStore.GetValue("Mru" + i, string.Empty);
                    }
                    this.MruList.Load(entries);
                }
            }
        }

        private void OnCommandFileFileMRU(int index)
        {
            string path = this.MruList[index];
            MiscFileProjectItem projectItem = new MiscFileProjectItem(Path.GetFileName(path), path, this);
            try
            {
                this.OpenProjectItem(projectItem, false, DocumentViewType.Default);
            }
            catch
            {
                ((IMxUIService) base.ServiceProvider.GetService(typeof(IMxUIService))).ReportError("The file '" + path + "' could not be opened.\r\nIt will be removed from the recent files list.", "Unable to open the selected file.", true);
                this.MruList.RemoveEntry(path);
            }
        }

        private void OnCommandFileNew()
        {
            IDocumentManager service = (IDocumentManager) base.GetService(typeof(IDocumentManager));
            FolderProjectItem parentItem = null;
            Document activeDocument = service.ActiveDocument;
            if ((activeDocument != null) && (activeDocument.ProjectItem.Project == this))
            {
                string directoryName = Path.GetDirectoryName(activeDocument.DocumentPath);
                string caption = Path.GetDirectoryName(directoryName);
                if (caption == null)
                {
                    caption = directoryName;
                }
                parentItem = new MiscDirectoryProjectItem(caption, directoryName, this);
            }
            DocumentProjectItem projectItem = service.CreateDocument(this, parentItem, false);
            if (projectItem != null)
            {
                this.OpenProjectItem(projectItem, false, DocumentViewType.Default);
            }
        }

        private void OnCommandFileOpen()
        {
            IDocumentTypeManager service = (IDocumentTypeManager) base.GetService(typeof(IDocumentTypeManager));
            if (service != null)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = service.OpenFilters;
                dialog.ShowReadOnly = true;
                dialog.Multiselect = true;
                dialog.CheckFileExists = true;
                dialog.Title = "Open Files";
                IDocumentManager manager2 = (IDocumentManager) base.GetService(typeof(IDocumentManager));
                if (manager2 != null)
                {
                    Document activeDocument = manager2.ActiveDocument;
                    if ((activeDocument != null) && (activeDocument.ProjectItem.Project == this))
                    {
                        dialog.InitialDirectory = Path.GetDirectoryName(activeDocument.DocumentPath);
                    }
                }
                ICommandManager manager3 = (ICommandManager) base.GetService(typeof(ICommandManager));
                if (manager3 != null)
                {
                    manager3.SuspendCommandUpdate();
                }
                try
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string[] fileNames = dialog.FileNames;
                        bool readOnlyChecked = dialog.ReadOnlyChecked;
                        for (int i = 0; i < fileNames.Length; i++)
                        {
                            string path = fileNames[i];
                            try
                            {
                                MiscFileProjectItem projectItem = new MiscFileProjectItem(Path.GetFileName(path), path, this);
                                bool readOnly = readOnlyChecked || ((File.GetAttributes(path) & FileAttributes.ReadOnly) != 0);
                                this.OpenProjectItem(projectItem, readOnly, DocumentViewType.Default);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                finally
                {
                    if (manager3 != null)
                    {
                        manager3.ResumeCommandUpdate();
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
            if ((projectItem == null) || (projectItem.Project != this))
            {
                projectItem = new MiscFileProjectItem(item.Caption, item.Path, this);
            }
            Document document = service.OpenDocument(projectItem, readOnly, initialView);
            this.AddMruEntry(item);
            return document;
        }

        private void SaveMruEntries()
        {
            if (this._mruList != null)
            {
                IPreferencesService service = (IPreferencesService) base.GetService(typeof(IPreferencesService));
                if (service != null)
                {
                    PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(MyComputerProject));
                    preferencesStore.SetValue("MruCount", this._mruList.Count, 0);
                    if (this._mruList.Count != 0)
                    {
                        string[] strArray = this._mruList.Save();
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            preferencesStore.SetValue("Mru" + i, strArray[i], string.Empty);
                        }
                    }
                }
            }
        }

        protected override bool UpdateProjectItemCommand(Command command, Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            int num;
            bool flag = false;
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 1:
                        flag = true;
                        goto Label_00F4;

                    case 2:
                        flag = true;
                        goto Label_00F4;

                    case 20:
                        if ((this._mruList == null) || (this._mruList.Count == 0))
                        {
                            command.Text = "(Empty)";
                            command.Enabled = false;
                            flag = true;
                            goto Label_00F4;
                        }
                        goto Label_009D;

                    case 0x15:
                    case 0x16:
                    case 0x17:
                    case 0x18:
                    case 0x19:
                    case 0x1a:
                    case 0x1b:
                    case 0x1c:
                    case 0x1d:
                        goto Label_009D;
                }
            }
            goto Label_00F4;
        Label_009D:
            num = command.CommandID - 20;
            if ((this._mruList != null) && (num < this._mruList.Count))
            {
                command.Text = Microsoft.Matrix.Utility.MruList.MenuPrefixes[num] + this._mruList[num];
                command.Visible = true;
            }
            else
            {
                command.Enabled = false;
                command.Visible = false;
            }
            flag = true;
        Label_00F4:
            if (!flag)
            {
                flag = base.UpdateProjectItemCommand(command, item);
            }
            return flag;
        }

        int ILocalFileSystemProject.MruDocumentsLength
        {
            get
            {
                return this.MruList.MaximumEntries;
            }
            set
            {
                if ((value > 0) && (value <= 10))
                {
                    this.MruList.MaximumEntries = value;
                    ((IApplicationIdentity) base.GetService(typeof(IApplicationIdentity))).SetSetting("MruDocumentsLength", value.ToString(CultureInfo.InvariantCulture));
                }
            }
        }

        private Microsoft.Matrix.Utility.MruList MruList
        {
            get
            {
                if (this._mruList == null)
                {
                    int maxEntries = 10;
                    string setting = ((IApplicationIdentity) base.GetService(typeof(IApplicationIdentity))).GetSetting("MruDocumentsLength");
                    if (setting != null)
                    {
                        try
                        {
                            maxEntries = int.Parse(setting, CultureInfo.InvariantCulture);
                            if ((maxEntries <= 0) || (maxEntries > 10))
                            {
                                maxEntries = 10;
                            }
                        }
                        catch
                        {
                        }
                    }
                    this._mruList = new Microsoft.Matrix.Utility.MruList(maxEntries);
                }
                return this._mruList;
            }
        }

        public override RootProjectItem ProjectItem
        {
            get
            {
                return this._rootItem;
            }
        }
    }
}

