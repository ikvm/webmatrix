namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix;
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public abstract class MxApplicationWindow : MxMainForm
    {
        private UIService _uiService;
        private const int WM_STARTUPACTION = 0x401;

        public MxApplicationWindow(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            IServiceContainer service = (IServiceContainer) this.GetService(typeof(IServiceContainer));
            if (service != null)
            {
                this._uiService = new UIService(base.ServiceProvider, this);
                service.AddService(typeof(IMxUIService), this._uiService);
                service.AddService(typeof(IUIService), this._uiService);
                this.Font = this._uiService.UIFont;
            }
            IApplicationIdentity identity = (IApplicationIdentity) this.GetService(typeof(IApplicationIdentity));
            if (identity != null)
            {
                try
                {
                    string setting = identity.GetSetting("WindowMaximized");
                    if ((setting != null) && setting.Equals("true"))
                    {
                        base.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        base.WindowState = FormWindowState.Normal;
                    }
                    string str2 = identity.GetSetting("WindowLeft", false);
                    if ((str2 != null) && (str2.Length != 0))
                    {
                        int x = Convert.ToInt32(str2);
                        int y = Convert.ToInt32(identity.GetSetting("WindowTop", false));
                        int width = Convert.ToInt32(identity.GetSetting("WindowWidth", false));
                        int height = Convert.ToInt32(identity.GetSetting("WindowHeight", false));
                        base.Bounds = new Rectangle(x, y, width, height);
                    }
                    else
                    {
                        base.Bounds = Screen.FromPoint(Cursor.Position).WorkingArea;
                    }
                    base.StartPosition = FormStartPosition.Manual;
                }
                catch (Exception)
                {
                }
            }
            this.AllowDrop = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IServiceContainer service = (IServiceContainer) this.GetService(typeof(IServiceContainer));
                if (service != null)
                {
                    service.RemoveService(typeof(IUIService));
                }
                if (this._uiService != null)
                {
                    ((IDisposable) this._uiService).Dispose();
                    this._uiService = null;
                }
            }
            base.Dispose(disposing);
        }

        protected override bool HandleCommand(Command command)
        {
            if ((command.CommandGroup == typeof(GlobalCommands)) && (command.CommandID == 9))
            {
                base.Close();
                return true;
            }
            return false;
        }

        protected virtual bool IsSupportedDocumentType(string extension)
        {
            IDocumentTypeManager service = (IDocumentTypeManager) this.GetService(typeof(IDocumentTypeManager));
            return ((service != null) && service.IsRegisteredDocumentType(extension));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            IApplicationIdentity service = (IApplicationIdentity) this.GetService(typeof(IApplicationIdentity));
            if (service != null)
            {
                try
                {
                    if (base.WindowState == FormWindowState.Maximized)
                    {
                        service.SetSetting("WindowMaximized", "true");
                    }
                    else if (base.WindowState != FormWindowState.Minimized)
                    {
                        service.SetSetting("WindowMaximized", null);
                        Rectangle bounds = base.Bounds;
                        service.SetSetting("WindowLeft", Convert.ToString(bounds.Left));
                        service.SetSetting("WindowTop", Convert.ToString(bounds.Top));
                        service.SetSetting("WindowWidth", Convert.ToString(bounds.Width));
                        service.SetSetting("WindowHeight", Convert.ToString(bounds.Height));
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                this.OpenDocuments((string[]) e.Data.GetData(DataFormats.FileDrop));
            }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                foreach (string str in (string[]) e.Data.GetData(DataFormats.FileDrop))
                {
                    if (this.IsSupportedDocumentType(Path.GetExtension(str)))
                    {
                        e.Effect = DragDropEffects.Copy;
                        return;
                    }
                }
            }
            e.Effect = DragDropEffects.None;
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            Interop.PostMessage(base.Handle, 0x401, IntPtr.Zero, IntPtr.Zero);
        }

        protected void OpenDocuments(string[] fileNames)
        {
            if ((fileNames != null) && (fileNames.Length != 0))
            {
                IProjectManager service = (IProjectManager) this.GetService(typeof(IProjectManager));
                if (service != null)
                {
                    Project localFileSystemProject = service.LocalFileSystemProject;
                    if (localFileSystemProject != null)
                    {
                        for (int i = 0; i < fileNames.Length; i++)
                        {
                            string path = fileNames[i];
                            if (File.Exists(path) && this.IsSupportedDocumentType(Path.GetExtension(path)))
                            {
                                ProjectItem item = localFileSystemProject.ParsePath(Path.GetFullPath(path));
                                if (item != null)
                                {
                                    bool readOnly = (File.GetAttributes(path) & FileAttributes.ReadOnly) != 0;
                                    localFileSystemProject.OpenProjectItem((DocumentProjectItem) item, readOnly, DocumentViewType.Default);
                                }
                            }
                        }
                    }
                }
            }
        }

        protected virtual bool PerformStartupAction()
        {
            return false;
        }

        protected override bool UpdateCommand(Command command)
        {
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 200:
                        command.Text = "Source";
                        command.Enabled = false;
                        command.Glyph = null;
                        return true;

                    case 0xc9:
                    case 0xca:
                    case 0xcb:
                        command.Enabled = false;
                        command.Visible = false;
                        return true;

                    case 0x3eb:
                        if ((Interop.GetKeyState(20) & 1) != 0)
                        {
                            command.Text = "CAPS";
                        }
                        else
                        {
                            command.Text = string.Empty;
                        }
                        return true;

                    case 9:
                        return true;
                }
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x401)
            {
                if (!this.PerformStartupAction())
                {
                    MxApplication.Current.RaiseStartupEvent();
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}

