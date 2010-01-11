namespace Microsoft.Matrix.Core.Toolbox
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    [Serializable]
    public abstract class ToolboxSection : ISerializable
    {
        private System.Drawing.Icon _icon;
        private ArrayList _items;
        private EventHandler _itemsChangedHandler;
        private string _name;

        public event EventHandler ItemsChanged
        {
            add
            {
                this._itemsChangedHandler = (EventHandler) Delegate.Combine(this._itemsChangedHandler, value);
            }
            remove
            {
                if (this._itemsChangedHandler != null)
                {
                    this._itemsChangedHandler = (EventHandler) Delegate.Remove(this._itemsChangedHandler, value);
                }
            }
        }

        protected ToolboxSection()
        {
        }

        public ToolboxSection(string name)
        {
            if ((name == null) || (name.Length == 0))
            {
                throw new ArgumentNullException("name");
            }
            this._name = name;
            this._items = new ArrayList();
        }

        private ToolboxSection(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
        }

        public void AddToolboxDataItem(ToolboxDataItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            this.AddToolboxDataItem(item, true);
        }

        internal void AddToolboxDataItem(ToolboxDataItem item, bool fireItemsChanged)
        {
            this._items.Add(item);
            if (fireItemsChanged)
            {
                this.OnItemsChanged(EventArgs.Empty);
            }
        }

        public virtual bool CanCreateToolboxDataItem(IDataObject dataObject)
        {
            return false;
        }

        protected internal virtual void ClearToolboxDataItems()
        {
            this._items.Clear();
        }

        public abstract ToolboxDataItem CreateToolboxDataItem(string toolboxData);
        public virtual ToolboxDataItem CreateToolboxDataItem(IDataObject dataObject)
        {
            return null;
        }

        public virtual bool Customize(int command, IServiceProvider provider)
        {
            throw new NotSupportedException();
        }

        protected virtual void Deserialize(SerializationInfo info, StreamingContext context)
        {
            this._name = info.GetString("Name");
            this._icon = info.GetValue("Icon", typeof(System.Drawing.Icon)) as System.Drawing.Icon;
            int capacity = info.GetInt32("Count");
            this._items = new ArrayList(capacity);
            for (int i = 0; i < capacity; i++)
            {
                this._items.Add(info.GetValue("Item" + i, typeof(ToolboxDataItem)));
            }
        }

        internal void FireItemsChanged()
        {
            this.OnItemsChanged(EventArgs.Empty);
        }

        public virtual string GetCustomizationText(int command)
        {
            return null;
        }

        protected virtual void OnItemsChanged(EventArgs e)
        {
            if (this._itemsChangedHandler != null)
            {
                this._itemsChangedHandler(this, e);
            }
        }

        public void RemoveToolboxDataItem(ToolboxDataItem item)
        {
            int index = this._items.IndexOf(item);
            if (index >= 0)
            {
                this._items.RemoveAt(index);
                this.OnItemsChanged(EventArgs.Empty);
            }
        }

        public bool RenameToolboxDataItem(ToolboxDataItem item, string newName)
        {
            if (item.CanSetDisplayName && this.ValidateToolboxDataItemName(item, newName))
            {
                item.SetDisplayName(newName);
                return true;
            }
            return false;
        }

        public void ReorderToolboxDataItem(int oldIndex, int newIndex)
        {
            if ((oldIndex < 0) || (oldIndex > this.ToolboxDataItems.Count))
            {
                throw new IndexOutOfRangeException();
            }
            if ((newIndex < 0) || (newIndex > this.ToolboxDataItems.Count))
            {
                throw new IndexOutOfRangeException();
            }
            if (oldIndex != newIndex)
            {
                object obj2 = this._items[oldIndex];
                this._items.Insert(newIndex, obj2);
                if (oldIndex > newIndex)
                {
                    oldIndex++;
                }
                this._items.RemoveAt(oldIndex);
                this.OnItemsChanged(EventArgs.Empty);
            }
        }

        protected virtual void Serialize(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", this._name);
            info.AddValue("Icon", this._icon);
            int count = this._items.Count;
            info.AddValue("Count", count);
            for (int i = 0; i < count; i++)
            {
                info.AddValue("Item" + i, this._items[i]);
            }
        }

        public void SortToolbox()
        {
            this._items.Sort(new ToolboxDataItemComparer());
            this.OnItemsChanged(EventArgs.Empty);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.Serialize(info, context);
        }

        protected virtual bool ValidateToolboxDataItemName(ToolboxDataItem item, string name)
        {
            return ((name != null) && (name.Length > 0));
        }

        public virtual bool AllowDrop
        {
            get
            {
                return false;
            }
        }

        public virtual bool CanRemove
        {
            get
            {
                return false;
            }
        }

        public virtual bool CanRename
        {
            get
            {
                return false;
            }
        }

        public virtual bool CanReorder
        {
            get
            {
                return false;
            }
        }

        public virtual string CustomizationHint
        {
            get
            {
                return "Right-click on the toolbox to customize...";
            }
        }

        public System.Drawing.Icon Icon
        {
            get
            {
                if (this._icon == null)
                {
                    this._icon = new System.Drawing.Icon(typeof(ToolboxSection), "ToolboxSection.ico");
                }
                return this._icon;
            }
            set
            {
                this._icon = value;
            }
        }

        public virtual bool IsCustomizable
        {
            get
            {
                return false;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public ICollection ToolboxDataItems
        {
            get
            {
                return this._items;
            }
        }

        private sealed class ToolboxDataItemComparer : IComparer
        {
            public int Compare(object o1, object o2)
            {
                ToolboxDataItem item = (ToolboxDataItem) o1;
                ToolboxDataItem item2 = (ToolboxDataItem) o2;
                return item.DisplayName.CompareTo(item2.DisplayName);
            }
        }
    }
}

