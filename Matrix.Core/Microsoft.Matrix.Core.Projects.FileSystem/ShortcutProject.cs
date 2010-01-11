namespace Microsoft.Matrix.Core.Projects.FileSystem
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.UserInterface;
    using System;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class ShortcutProject : FileSystemProject
    {
        private ShortcutProjectItem _rootItem;

        public ShortcutProject(IProjectFactory factory, IServiceProvider serviceProvider, string rootDirectory) : base(factory, serviceProvider)
        {
            this._rootItem = new ShortcutProjectItem(this, rootDirectory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._rootItem = null;
            }
            base.Dispose(disposing);
        }

        protected internal override string GetProjectItemPathInternal(Microsoft.Matrix.Core.Projects.ProjectItem projectItem)
        {
            if (projectItem == this._rootItem)
            {
                return this._rootItem.Path;
            }
            return base.GetProjectItemPathInternal(projectItem);
        }

        private void OnCommandAddItem(Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            if ((item == this._rootItem) || (item is DirectoryProjectItem))
            {
                AddFileDialog form = new AddFileDialog(this, item.Path, true);
                IUIService service = (IUIService) base.GetService(typeof(IUIService));
                if (service.ShowDialog(form) == DialogResult.OK)
                {
                    DocumentProjectItem newProjectItem = form.NewProjectItem;
                    this.OpenProjectItem(newProjectItem, false, DocumentViewType.Default);
                }
            }
        }

        public override Document OpenProjectItem(DocumentProjectItem item, bool readOnly, DocumentViewType initialView)
        {
            IProjectManager service = (IProjectManager) base.GetService(typeof(IProjectManager));
            if (service != null)
            {
                Project localFileSystemProject = service.LocalFileSystemProject;
                if (localFileSystemProject != null)
                {
                    return localFileSystemProject.OpenProjectItem(item, readOnly, initialView);
                }
            }
            return null;
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

