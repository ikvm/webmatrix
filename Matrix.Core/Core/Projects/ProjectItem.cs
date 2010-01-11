namespace Microsoft.Matrix.Core.Projects
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Windows.Forms;

    public abstract class ProjectItem
    {
        private string _caption;
        internal ArrayList _childItems;
        private ProjectItemCollection _childItemsCollection;
        private int _iconIndex;
        private TreeNode _node;
        private ProjectItem _parentItem;
        private BitVector32 _state;
        private static readonly BitVector32.Section StateChildItemsCreated = BitVector32.CreateSection(1);
        private static readonly BitVector32.Section StateFlags = BitVector32.CreateSection(0x1f, StateChildItemsCreated);

        public ProjectItem(string caption)
        {
            if ((caption == null) || (caption.Length == 0))
            {
                throw new ArgumentNullException("caption");
            }
            this._caption = caption;
            this._state = new BitVector32(0);
        }

        protected internal void AddChildItem(ProjectItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            if (this._childItems == null)
            {
                this._childItems = new ArrayList();
            }
            this._childItems.Add(item);
            item.SetParent(this);
        }

        protected bool ContainsChildItem(ProjectItem item)
        {
            return (this.IndexOfChildItem(item) != -1);
        }

        protected virtual void CreateChildItems()
        {
            this._state[StateChildItemsCreated] = 1;
        }

        public bool Delete()
        {
            if (!this.IsDeletable)
            {
                throw new NotSupportedException("Can't delete project item");
            }
            bool flag = this.Project.DeleteProjectItemInternal(this);
            if (flag)
            {
                this.Parent.RemoveChildItem(this);
            }
            return flag;
        }

        protected void DirtyChildItems()
        {
            this._state[StateChildItemsCreated] = 0;
        }

        public override bool Equals(object o)
        {
            ProjectItem item = o as ProjectItem;
            return (((item != null) && (this.Project == item.Project)) && (string.Compare(this.Path, item.Path, true) == 0));
        }

        public IDataObject GetDataObject()
        {
            if (!this.IsDragSource)
            {
                throw new NotSupportedException();
            }
            return this.Project.GetProjectItemDataObjectInternal(this);
        }

        public override int GetHashCode()
        {
            return this.Path.ToLower().GetHashCode();
        }

        protected int IndexOfChildItem(ProjectItem item)
        {
            if (this._childItems != null)
            {
                return this._childItems.IndexOf(item);
            }
            return -1;
        }

        protected void InsertChildItem(int index, ProjectItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            if (this._childItems == null)
            {
                if (index != 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._childItems = new ArrayList();
            }
            this._childItems.Insert(index, item);
            item.SetParent(this);
        }

        public void Refresh()
        {
            if (this.IsExpandable)
            {
                if (this._childItems != null)
                {
                    this._childItems.Clear();
                }
                this.DirtyChildItems();
            }
        }

        protected internal void RemoveChildItem(ProjectItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            int index = this.IndexOfChildItem(item);
            if (index == -1)
            {
                throw new ArgumentException();
            }
            item.SetParent(null);
            this._childItems.RemoveAt(index);
        }

        protected void SetFlags(ProjectItemFlags flags, bool isSet)
        {
            int num = this._state[StateFlags];
            this._state[StateFlags] = isSet ? (num | (int)flags) : (num & ~(int)flags); //TODO: ÊÖ¶¯ÐÞ¸Ä
            //this._state[StateFlags] = isSet ? (num | flags) : (num & ~flags); 
        }

        protected void SetIconIndex(int index)
        {
            this._iconIndex = index;
        }

        internal void SetParent(ProjectItem parentItem)
        {
            this._parentItem = parentItem;
        }

        public ProjectItemAttributes Attributes
        {
            get
            {
                return this.Project.GetProjectItemAttributesInternal(this);
            }
        }

        public string Caption
        {
            get
            {
                return this._caption;
            }
        }

        public ProjectItemCollection ChildItems
        {
            get
            {
                if (!this.IsExpandable)
                {
                    return null;
                }
                if ((this._childItems == null) || (this._state[StateChildItemsCreated] == 0))
                {
                    this.CreateChildItems();
                }
                if (this._childItemsCollection == null)
                {
                    this._childItemsCollection = new ProjectItemCollection(this);
                }
                return this._childItemsCollection;
            }
        }

        public bool ContainsChildItems
        {
            get
            {
                if (!this.IsExpandable)
                {
                    return false;
                }
                if ((this._childItems == null) || (this._state[StateChildItemsCreated] == 0))
                {
                    this.CreateChildItems();
                }
                return ((this._childItems != null) && (this._childItems.Count != 0));
            }
        }

        public virtual string Description
        {
            get
            {
                return this.Caption;
            }
        }

        public string DisplayName
        {
            get
            {
                return this.Project.GetProjectItemDisplayNameInternal(this);
            }
        }

        public int IconIndex
        {
            get
            {
                return this._iconIndex;
            }
        }

        public bool IsDeletable
        {
            get
            {
                return ((this._state[StateFlags] & 2) == 0);
            }
        }

        public bool IsDragSource
        {
            get
            {
                return ((this._state[StateFlags] & 8) != 0);
            }
        }

        public bool IsDropTarget
        {
            get
            {
                return ((this._state[StateFlags] & 0x10) != 0);
            }
        }

        public bool IsExpandable
        {
            get
            {
                return ((this._state[StateFlags] & 1) == 0);
            }
        }

        public TreeNode ItemNode
        {
            get
            {
                return this._node;
            }
            set
            {
                this._node = value;
            }
        }

        public ProjectItem Parent
        {
            get
            {
                return this.ParentInternal;
            }
        }

        internal virtual ProjectItem ParentInternal
        {
            get
            {
                if (this._parentItem != null)
                {
                    return this._parentItem;
                }
                return this.Project.ProjectItem;
            }
        }

        public virtual string Path
        {
            get
            {
                return this.Project.GetProjectItemPathInternal(this);
            }
        }

        public virtual Microsoft.Matrix.Core.Projects.Project Project
        {
            get
            {
                if (this._parentItem != null)
                {
                    return this._parentItem.Project;
                }
                return null;
            }
        }

        public bool SupportsAddItem
        {
            get
            {
                return ((this._state[StateFlags] & 4) != 0);
            }
        }

        public virtual string Url
        {
            get
            {
                return this.Project.GetProjectItemUrlInternal(this);
            }
        }
    }
}

