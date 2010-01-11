namespace Microsoft.Matrix.Core.Projects
{
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class ProjectItemCollection : ICollection, IEnumerable
    {
        private ProjectItem _owner;

        public ProjectItemCollection(ProjectItem owner)
        {
            this._owner = owner;
        }

        public bool Contains(ProjectItem item)
        {
            return (this.IndexOf(item) != -1);
        }

        public void CopyTo(Array array, int index)
        {
            if (this._owner._childItems != null)
            {
                this._owner._childItems.CopyTo(array, index);
            }
        }

        public IEnumerator GetEnumerator()
        {
            if (this._owner._childItems != null)
            {
                return this._owner._childItems.GetEnumerator();
            }
            return new ProjectItem[0].GetEnumerator();
        }

        public int IndexOf(ProjectItem item)
        {
            if (this._owner._childItems != null)
            {
                return this._owner._childItems.IndexOf(item);
            }
            return -1;
        }

        public int Count
        {
            get
            {
                if (this._owner._childItems == null)
                {
                    return 0;
                }
                return this._owner._childItems.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public ProjectItem this[int index]
        {
            get
            {
                if (this._owner._childItems == null)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return (ProjectItem) this._owner._childItems[index];
            }
        }

        public object SyncRoot
        {
            get
            {
                return null;
            }
        }
    }
}

