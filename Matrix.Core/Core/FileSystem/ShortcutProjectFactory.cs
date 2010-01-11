namespace Microsoft.Matrix.Core.Projects.FileSystem
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public sealed class ShortcutProjectFactory : IProjectFactory, IDisposable
    {
        private Icon _largeIcon;
        private IServiceProvider _serviceProvider;
        private Icon _smallIcon;

        private ShortcutProject CreateProject(string rootFolder)
        {
            return new ShortcutProject(this, this._serviceProvider, rootFolder);
        }

        Project IProjectFactory.CreateProject(object creationArgs)
        {
            if (creationArgs != null)
            {
                string path = null;
                if (creationArgs is string)
                {
                    path = (string) creationArgs;
                    if (((path.Length == 0) || !Directory.Exists(path)) || !Path.IsPathRooted(path))
                    {
                        path = null;
                    }
                }
                if (path == null)
                {
                    throw new ArgumentException("The specified folder is invalid.");
                }
                return this.CreateProject(path);
            }
            IMxUIService service = (IMxUIService) this._serviceProvider.GetService(typeof(IMxUIService));
            FolderBrowser browser = new FolderBrowser();
            browser.Description = "Select the folder you want to create a shortcut to:";
            browser.StartLocation = FolderBrowserLocation.MyComputer;
            browser.Style = FolderBrowserStyles.ShowTextBox | FolderBrowserStyles.RestrictToFileSystem;
            if (browser.ShowDialog(service.DialogOwner) == DialogResult.OK)
            {
                string directoryPath = browser.DirectoryPath;
                if ((directoryPath != null) && (directoryPath.Length != 0))
                {
                    if (!Directory.Exists(directoryPath) || !Path.IsPathRooted(directoryPath))
                    {
                        throw new ArgumentException("The selected folder is invalid.");
                    }
                    return this.CreateProject(directoryPath);
                }
            }
            return null;
        }

        void IProjectFactory.Initialize(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        object IProjectFactory.SaveProject(Project project)
        {
            if (!(project is ShortcutProject))
            {
                throw new ArgumentException("Unexpected type of project");
            }
            return project.ProjectItem.Path;
        }

        void IDisposable.Dispose()
        {
            this._serviceProvider = null;
        }

        bool IProjectFactory.AutoInstantiate
        {
            get
            {
                return false;
            }
        }

        string IProjectFactory.Category
        {
            get
            {
                return null;
            }
        }

        string IProjectFactory.CreateNewDescription
        {
            get
            {
                return "Create a shortcut to a folder on your computer.";
            }
        }

        bool IProjectFactory.IsSingleInstance
        {
            get
            {
                return false;
            }
        }

        Icon IProjectFactory.LargeIcon
        {
            get
            {
                if (this._largeIcon == null)
                {
                    this._largeIcon = new Icon(typeof(ShortcutProjectFactory), "ShortcutLarge.ico");
                }
                return this._largeIcon;
            }
        }

        string IProjectFactory.Name
        {
            get
            {
                return "Folder Shortcut";
            }
        }

        Type IProjectFactory.ProjectType
        {
            get
            {
                return typeof(ShortcutProject);
            }
        }

        ProjectFactorySaveMode IProjectFactory.SaveMode
        {
            get
            {
                return ProjectFactorySaveMode.SaveToPreferences;
            }
        }

        Icon IProjectFactory.SmallIcon
        {
            get
            {
                if (this._smallIcon == null)
                {
                    this._smallIcon = new Icon(typeof(ShortcutProjectFactory), "ShortcutSmall.ico");
                }
                return this._smallIcon;
            }
        }
    }
}

