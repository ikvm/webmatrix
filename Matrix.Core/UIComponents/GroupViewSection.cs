namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    public sealed class GroupViewSection
    {
        private ListView _groupListView;
        private int _imageIndex = -1;
        private GroupViewItemCollection _items;
        private GroupView _owner;
        private System.Drawing.Rectangle _tabRectangle;
        private string _text;
        private int _toolTipID = -1;
        private bool _visible = true;

        public GroupViewSection()
        {
            this._items = new GroupViewItemCollection(this);
        }

        internal void OnActivate(ListView currentListView)
        {
            this._groupListView = currentListView;
            foreach (GroupViewItem item in this._items)
            {
                GroupViewListViewItem item2 = new GroupViewListViewItem(item);
                this._groupListView.Items.Add(item2);
                item.ListViewItem = item2;
            }
        }

        internal void OnDeactivate()
        {
            foreach (GroupViewItem item in this._items)
            {
                item.ListViewItem = null;
            }
            this._groupListView.Items.Clear();
            this._groupListView = null;
        }

        internal void OnItemAdded(GroupViewItem item)
        {
            if (this._groupListView != null)
            {
                int index = this._items.IndexOf(item);
                GroupViewListViewItem item2 = new GroupViewListViewItem(item);
                this._groupListView.Items.Insert(index, item2);
            }
        }

        internal void OnItemRemoved(GroupViewItem item)
        {
            if (this._groupListView != null)
            {
                int index = -1;
                int num2 = 0;
                foreach (GroupViewListViewItem item2 in this._groupListView.Items)
                {
                    if (item2.GroupViewItem == item)
                    {
                        index = num2;
                        break;
                    }
                    num2++;
                }
                if (index != -1)
                {
                    this._groupListView.Items.RemoveAt(index);
                }
            }
        }

        private void OnSectionChanged(bool layoutRequired)
        {
            if (this._owner != null)
            {
                this._owner.OnSectionChanged(this, layoutRequired);
            }
        }

        internal void SetOwner(GroupView owner)
        {
            this._owner = owner;
        }

        public int ImageIndex
        {
            get
            {
                return this._imageIndex;
            }
            set
            {
                if (this._imageIndex != value)
                {
                    this._imageIndex = value;
                    this.OnSectionChanged(false);
                }
            }
        }

        public GroupViewItemCollection Items
        {
            get
            {
                return this._items;
            }
        }

        internal System.Drawing.Rectangle Rectangle
        {
            get
            {
                return this._tabRectangle;
            }
            set
            {
                this._tabRectangle = value;
            }
        }

        public string Text
        {
            get
            {
                if (this._text == null)
                {
                    return string.Empty;
                }
                return this._text;
            }
            set
            {
                if (this._text != value)
                {
                    this._text = value;
                    this.OnSectionChanged(false);
                }
            }
        }

        internal int ToolTipID
        {
            get
            {
                return this._toolTipID;
            }
            set
            {
                this._toolTipID = value;
            }
        }

        public bool Visible
        {
            get
            {
                return this._visible;
            }
            set
            {
                if (this._visible != value)
                {
                    this._visible = value;
                    this.OnSectionChanged(true);
                }
            }
        }

        public sealed class GroupViewItemCollection : IList, ICollection, IEnumerable
        {
            private ArrayList _items;
            private GroupViewSection _owner;

            internal GroupViewItemCollection(GroupViewSection owner)
            {
                this._owner = owner;
                this._items = new ArrayList();
            }

            public void Add(GroupViewItem value)
            {
                this._items.Add(value);
                this._owner.OnItemAdded(value);
            }

            public void AddRange(GroupViewItem[] items)
            {
                foreach (GroupViewItem item in items)
                {
                    this.Add(item);
                }
            }

            public bool Contains(GroupViewItem item)
            {
                return (this.IndexOf(item) != -1);
            }

            public void CopyTo(Array dest, int index)
            {
                if (this.Count > 0)
                {
                    this._items.CopyTo(dest, index);
                }
            }

            public IEnumerator GetEnumerator()
            {
                if (this.Count != 0)
                {
                    return this._items.GetEnumerator();
                }
                return new GroupViewItem[0].GetEnumerator();
            }

            public int IndexOf(GroupViewItem item)
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this[i] == item)
                    {
                        return i;
                    }
                }
                return -1;
            }

            public void Remove(GroupViewItem value)
            {
                this._items.Remove(value);
            }

            public void RemoveAt(int index)
            {
                GroupViewItem item = (GroupViewItem) this._items[index];
                this._items.RemoveAt(index);
                this._owner.OnItemRemoved(item);
            }

            int IList.Add(object value)
            {
                GroupViewItem item = value as GroupViewItem;
                if (item == null)
                {
                    throw new ArgumentException();
                }
                this.Add(item);
                return this.IndexOf(item);
            }

            void IList.Clear()
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    this.RemoveAt(0);
                }
            }

            bool IList.Contains(object value)
            {
                GroupViewItem item = value as GroupViewItem;
                return ((item != null) && this.Contains(item));
            }

            int IList.IndexOf(object value)
            {
                GroupViewItem item = value as GroupViewItem;
                if (item != null)
                {
                    return this.IndexOf(item);
                }
                return -1;
            }

            void IList.Insert(int index, object value)
            {
                throw new NotSupportedException();
            }

            void IList.Remove(object value)
            {
                GroupViewItem item = value as GroupViewItem;
                if (item != null)
                {
                    this.Remove(item);
                }
            }

            public int Count
            {
                get
                {
                    return this._items.Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public bool IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            public GroupViewItem this[int index]
            {
                get
                {
                    return (GroupViewItem) this._items[index];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            public object SyncRoot
            {
                get
                {
                    return this;
                }
            }

            bool IList.IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }
        }
    }
}

