namespace Microsoft.Matrix.Core.Projects
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Windows.Forms;

    public abstract class Project : IServiceProvider, IDisposable, ICommandHandlerWithContext
    {
        private IProjectFactory _factory;
        private ProjectItemEventHandler _itemAddedHandler;
        private ProjectItemEventHandler _itemChangedHandler;
        private ProjectItemEventHandler _itemRemovedHandler;
        private ServiceContainer _serviceContainer;
        private IServiceProvider _serviceProvider;

        public event ProjectItemEventHandler ItemAdded
        {
            add
            {
                this._itemAddedHandler = (ProjectItemEventHandler) Delegate.Combine(this._itemAddedHandler, value);
            }
            remove
            {
                if (this._itemAddedHandler != null)
                {
                    this._itemAddedHandler = (ProjectItemEventHandler) Delegate.Remove(this._itemAddedHandler, value);
                }
            }
        }

        public event ProjectItemEventHandler ItemChanged
        {
            add
            {
                this._itemChangedHandler = (ProjectItemEventHandler) Delegate.Combine(this._itemChangedHandler, value);
            }
            remove
            {
                if (this._itemChangedHandler != null)
                {
                    this._itemChangedHandler = (ProjectItemEventHandler) Delegate.Remove(this._itemChangedHandler, value);
                }
            }
        }

        public event ProjectItemEventHandler ItemRemoved
        {
            add
            {
                this._itemRemovedHandler = (ProjectItemEventHandler) Delegate.Combine(this._itemRemovedHandler, value);
            }
            remove
            {
                if (this._itemRemovedHandler != null)
                {
                    this._itemRemovedHandler = (ProjectItemEventHandler) Delegate.Remove(this._itemRemovedHandler, value);
                }
            }
        }

        public Project(IProjectFactory factory, IServiceProvider serviceProvider)
        {
            this._factory = factory;
            this._serviceProvider = serviceProvider;
            this._serviceContainer = new ServiceContainer(serviceProvider);
        }

        public abstract string CombinePath(string path1, string path2);
        public abstract string CombineRelativePath(string path1, string path2);
        protected internal abstract DocumentProjectItem CreateDocumentInternal(FolderProjectItem parentItem);
        protected internal abstract FolderProjectItem CreateFolderInternal(FolderProjectItem parentItem);
        public bool DeleteProjectItem(Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (item.Project != this)
            {
                throw new ArgumentException();
            }
            if (!item.IsDeletable)
            {
                throw new NotSupportedException("Can't delete project item");
            }
            return this.DeleteProjectItemInternal(item);
        }

        protected internal abstract bool DeleteProjectItemInternal(Microsoft.Matrix.Core.Projects.ProjectItem item);
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._serviceContainer = null;
                this._serviceProvider = null;
                this._factory = null;
            }
        }

        public ProjectItemAttributes GetProjectItemAttributes(Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (item.Project != this)
            {
                throw new ArgumentException();
            }
            return this.GetProjectItemAttributesInternal(item);
        }

        protected internal abstract ProjectItemAttributes GetProjectItemAttributesInternal(Microsoft.Matrix.Core.Projects.ProjectItem item);
        public IDataObject GetProjectItemDataObject(Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if ((item.Project != this) || !item.IsDragSource)
            {
                throw new ArgumentException();
            }
            return this.GetProjectItemDataObjectInternal(item);
        }

        protected internal virtual IDataObject GetProjectItemDataObjectInternal(Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            DataObject obj2 = new DataObject();
            obj2.SetData(typeof(Microsoft.Matrix.Core.Projects.ProjectItem).Name, item);
            return obj2;
        }

        internal string GetProjectItemDisplayName(Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("projectItem");
            }
            if (item.Project != this)
            {
                throw new ArgumentException();
            }
            return this.GetProjectItemDisplayNameInternal(item);
        }

        protected internal virtual string GetProjectItemDisplayNameInternal(Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            return this.GetProjectItemPathInternal(item);
        }

        public DocumentType GetProjectItemDocumentType(Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("projectItem");
            }
            if (item.Project != this)
            {
                throw new ArgumentException();
            }
            return this.GetProjectItemDocumentTypeInternal(item);
        }

        protected internal abstract DocumentType GetProjectItemDocumentTypeInternal(Microsoft.Matrix.Core.Projects.ProjectItem item);
        public DragDropEffects GetProjectItemDragDropEffects(Microsoft.Matrix.Core.Projects.ProjectItem item, IDataObject dataObject)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if ((item.Project != this) || !item.IsDropTarget)
            {
                throw new ArgumentException();
            }
            return this.GetProjectItemDragDropEffectsInternal(item, dataObject);
        }

        protected virtual DragDropEffects GetProjectItemDragDropEffectsInternal(Microsoft.Matrix.Core.Projects.ProjectItem item, IDataObject dataObject)
        {
            return DragDropEffects.None;
        }

        public string GetProjectItemPath(Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (item.Project != this)
            {
                throw new ArgumentException();
            }
            return this.GetProjectItemPathInternal(item);
        }

        protected internal abstract string GetProjectItemPathInternal(Microsoft.Matrix.Core.Projects.ProjectItem projectItem);
        protected internal abstract Stream GetProjectItemStream(DocumentProjectItem projectItem, ProjectItemStreamMode mode);
        public string GetProjectItemUrl(Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (item.Project != this)
            {
                throw new ArgumentException();
            }
            return this.GetProjectItemUrlInternal(item);
        }

        protected internal abstract string GetProjectItemUrlInternal(Microsoft.Matrix.Core.Projects.ProjectItem item);
        public abstract DocumentProjectItem GetSaveAsProjectItem(DocumentProjectItem item);
        protected object GetService(Type serviceType)
        {
            if (this._serviceContainer != null)
            {
                return this._serviceContainer.GetService(serviceType);
            }
            return null;
        }

        protected virtual bool HandleProjectItemCommand(Command command, Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            return false;
        }

        public DragDropEffects HandleProjectItemDragDrop(Microsoft.Matrix.Core.Projects.ProjectItem item, IDataObject dataObject)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if ((item.Project != this) || !item.IsDropTarget)
            {
                throw new ArgumentException();
            }
            return this.HandleProjectItemDragDropInternal(item, dataObject);
        }

        protected virtual DragDropEffects HandleProjectItemDragDropInternal(Microsoft.Matrix.Core.Projects.ProjectItem item, IDataObject dataObject)
        {
            return DragDropEffects.None;
        }

        bool ICommandHandlerWithContext.HandleCommand(Command command, object context)
        {
            bool flag = false;
            Microsoft.Matrix.Core.Projects.ProjectItem item = context as Microsoft.Matrix.Core.Projects.ProjectItem;
            if (item != null)
            {
                flag = this.HandleProjectItemCommand(command, item);
            }
            return flag;
        }

        bool ICommandHandlerWithContext.UpdateCommand(Command command, object context)
        {
            bool flag = false;
            Microsoft.Matrix.Core.Projects.ProjectItem item = context as Microsoft.Matrix.Core.Projects.ProjectItem;
            if (item != null)
            {
                flag = this.UpdateProjectItemCommand(command, item);
            }
            return flag;
        }

        protected internal virtual void OnProjectItemAdded(ProjectItemEventArgs e)
        {
            if (this._itemAddedHandler != null)
            {
                this._itemAddedHandler(this, e);
            }
        }

        protected internal virtual void OnProjectItemChanged(ProjectItemEventArgs e)
        {
            if (this._itemChangedHandler != null)
            {
                this._itemChangedHandler(this, e);
            }
        }

        protected internal virtual void OnProjectItemRemoved(ProjectItemEventArgs e)
        {
            if (this._itemRemovedHandler != null)
            {
                this._itemRemovedHandler(this, e);
            }
        }

        public abstract Document OpenProjectItem(DocumentProjectItem projectItem, bool readOnly, DocumentViewType initialView);
        public Microsoft.Matrix.Core.Projects.ProjectItem ParsePath(string path)
        {
            if ((path == null) || (path.Length == 0))
            {
                throw new ArgumentNullException("path");
            }
            return this.ParsePathInternal(path, false);
        }

        public Microsoft.Matrix.Core.Projects.ProjectItem ParsePath(string path, bool newFile)
        {
            if ((path == null) || (path.Length == 0))
            {
                throw new ArgumentNullException("path");
            }
            return this.ParsePathInternal(path, newFile);
        }

        protected abstract Microsoft.Matrix.Core.Projects.ProjectItem ParsePathInternal(string path, bool newFile);
        public abstract bool ProjectItemExists(string itemPath);
        void IDisposable.Dispose()
        {
            this.Dispose(true);
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if (this._serviceContainer != null)
            {
                return this._serviceContainer.GetService(serviceType);
            }
            return null;
        }

        protected virtual bool UpdateProjectItemCommand(Command command, Microsoft.Matrix.Core.Projects.ProjectItem item)
        {
            return false;
        }

        public abstract bool ValidateProjectItemName(string itemName);
        public abstract bool ValidateProjectItemPath(string itemPath, bool createIfNeeded);

        public IProjectFactory ProjectFactory
        {
            get
            {
                return this._factory;
            }
        }

        public abstract RootProjectItem ProjectItem { get; }

        public virtual IComparer ProjectItemComparer
        {
            get
            {
                return null;
            }
        }

        protected IServiceProvider ServiceProvider
        {
            get
            {
                return this._serviceContainer;
            }
        }
    }
}

