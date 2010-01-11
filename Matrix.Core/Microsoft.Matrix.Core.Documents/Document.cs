namespace Microsoft.Matrix.Core.Documents
{
    using Microsoft.Matrix.Core.Documents.Design;
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.IO;

    [Designer(typeof(DocumentDesigner), typeof(IDesigner)), DesignOnly(true)]
    public abstract class Document : Component
    {
        private bool _dirty;
        private DocumentProjectItem _projectItem;
        private bool _readOnly;
        private IDocumentStorage _storage;
        public Guid _guid;
        protected Document(DocumentProjectItem projectItem)
        {
            this._guid = Guid.NewGuid();
            if (projectItem == null)
            {
                throw new ArgumentNullException("projectItem");
            }
            this._projectItem = projectItem;
        }

        protected abstract IDocumentStorage CreateStorage();
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeStorage();
                this._projectItem = null;
            }
            base.Dispose(disposing);
        }

        private void DisposeStorage()
        {
            if (this._storage != null)
            {
                this._storage.Dispose();
                this._storage = null;
            }
        }

        public void Load(bool forceReadOnly)
        {
            if (this.IsLoaded)
            {
                throw new InvalidOperationException();
            }
            Stream stream = null;
            try
            {
                stream = this._projectItem.GetStream(ProjectItemStreamMode.Read);
                this._storage = this.CreateStorage();
                this.LoadStreamIntoStorage(stream);
                this._readOnly = forceReadOnly;
                if (!forceReadOnly)
                {
                    ProjectItemAttributes attributes = this._projectItem.Attributes;
                    this._readOnly = (attributes & ProjectItemAttributes.ReadOnly) != ProjectItemAttributes.Normal;
                }
                this.OnAfterLoad(EventArgs.Empty);
                this._dirty = false;
            }
            catch
            {
                this.DisposeStorage();
                throw;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

        protected virtual void LoadStreamIntoStorage(Stream stream)
        {
            this._storage.Load(stream);
        }

        protected virtual void OnAfterLoad(EventArgs e)
        {
        }

        protected virtual void OnAfterSave(EventArgs e)
        {
        }

        protected virtual void OnBeforeSave(EventArgs e)
        {
        }

        public void Save()
        {
            if (this._readOnly)
            {
                throw new InvalidOperationException();
            }
            if (!this.IsLoaded)
            {
                throw new InvalidOperationException();
            }
            Stream stream = null;
            try
            {
                this.OnBeforeSave(EventArgs.Empty);
                try
                {
                    stream = this._projectItem.GetStream(ProjectItemStreamMode.Write);
                    this.SaveStorageToStream(stream);
                }
                finally
                {
                    this.OnAfterSave(EventArgs.Empty);
                }
                this._dirty = false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

        public void SaveAs(DocumentProjectItem item)
        {
            if (item == null)
            {
                throw new ArgumentException("fileName");
            }
            DocumentProjectItem item2 = this._projectItem;
            bool flag = this._readOnly;
            try
            {
                this._projectItem = item;
                this._readOnly = false;
                this.Save();
            }
            catch (Exception)
            {
                this._projectItem = item2;
                this._readOnly = flag;
                throw;
            }
        }

        protected virtual void SaveStorageToStream(Stream stream)
        {
            this._storage.Save(stream);
        }

        public void SetDirty()
        {
            this._dirty = true;
        }

        [Browsable(false)]
        public string DocumentName
        {
            get
            {
                return this._projectItem.Caption;
            }
        }

        [Description("The full path of the project item represented by this document."), Category("File")]
        public string DocumentPath
        {
            get
            {
                return this._projectItem.Path;
            }
        }

        [Browsable(false)]
        public bool IsDirty
        {
            get
            {
                return this._dirty;
            }
        }

        protected bool IsLoaded
        {
            get
            {
                return (this._storage != null);
            }
        }

        [Browsable(false)]
        public bool IsReadOnly
        {
            get
            {
                return this._readOnly;
            }
        }

        [Browsable(false)]
        public virtual DocumentLanguage Language
        {
            get
            {
                return null;
            }
        }

        [Browsable(false)]
        public DocumentProjectItem ProjectItem
        {
            get
            {
                return this._projectItem;
            }
        }

        protected IDocumentStorage Storage
        {
            get
            {
                return this._storage;
            }
        }
    }
}

