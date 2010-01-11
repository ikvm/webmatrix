namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;
    using System.Web.UI;

    public sealed class MxDataGridFieldCollection : IList, ICollection, IEnumerable, IStateManager
    {
        private ArrayList fields;
        private bool marked;
        private MxDataGrid owner;

        public MxDataGridFieldCollection(MxDataGrid owner, ArrayList fields)
        {
            this.owner = owner;
            this.fields = fields;
        }

        public void Add(MxDataGridField field)
        {
            this.AddAt(-1, field);
        }

        public void AddAt(int index, MxDataGridField field)
        {
            if (index == -1)
            {
                this.fields.Add(field);
            }
            else
            {
                this.fields.Insert(index, field);
            }
            field.SetOwner(this.owner);
            if (this.marked)
            {
                ((IStateManager) field).TrackViewState();
            }
            this.OnFieldsChanged();
        }

        public void Clear()
        {
            this.fields.Clear();
            this.OnFieldsChanged();
        }

        public void CopyTo(Array array, int index)
        {
            IEnumerator enumerator = this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                array.SetValue(enumerator.Current, index++);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this.fields.GetEnumerator();
        }

        public int IndexOf(MxDataGridField field)
        {
            if (field != null)
            {
                return this.fields.IndexOf(field);
            }
            return -1;
        }

        private void OnFieldsChanged()
        {
            if (this.owner != null)
            {
                this.owner.OnFieldsChanged();
            }
        }

        public void Remove(MxDataGridField field)
        {
            int index = this.IndexOf(field);
            if (index >= 0)
            {
                this.RemoveAt(index);
            }
        }

        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= this.Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            this.fields.RemoveAt(index);
            this.OnFieldsChanged();
        }

        int IList.Add(object value)
        {
            if (value is MxDataGridField)
            {
                this.Add((MxDataGridField) value);
                return 0;
            }
            return -1;
        }

        bool IList.Contains(object value)
        {
            return ((value is MxDataGridField) && (this.IndexOf((MxDataGridField) value) > -1));
        }

        int IList.IndexOf(object value)
        {
            if (value is MxDataGridField)
            {
                return this.IndexOf((MxDataGridField) value);
            }
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            if (value is MxDataGridField)
            {
                this.AddAt(index, (MxDataGridField) value);
            }
        }

        void IList.Remove(object value)
        {
            if (value is MxDataGridField)
            {
                this.Remove((MxDataGridField) value);
            }
        }

        void IStateManager.LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[]) savedState;
                if (objArray.Length == this.fields.Count)
                {
                    for (int i = 0; i < objArray.Length; i++)
                    {
                        if (objArray[i] != null)
                        {
                            ((IStateManager) this.fields[i]).LoadViewState(objArray[i]);
                        }
                    }
                }
            }
        }

        object IStateManager.SaveViewState()
        {
            int count = this.fields.Count;
            object[] objArray = new object[count];
            bool flag = false;
            for (int i = 0; i < count; i++)
            {
                objArray[i] = ((IStateManager) this.fields[i]).SaveViewState();
                if (objArray[i] != null)
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                return null;
            }
            return objArray;
        }

        void IStateManager.TrackViewState()
        {
            this.marked = true;
            int count = this.fields.Count;
            for (int i = 0; i < count; i++)
            {
                ((IStateManager) this.fields[i]).TrackViewState();
            }
        }

        [Browsable(false)]
        public int Count
        {
            get
            {
                return this.fields.Count;
            }
        }

        [Browsable(false)]
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        [Browsable(false)]
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        [Browsable(false)]
        public MxDataGridField this[int index]
        {
            get
            {
                return (MxDataGridField) this.fields[index];
            }
        }

        [Browsable(false)]
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
                return this.fields[index];
            }
            set
            {
                if (value is MxDataGridField)
                {
                    this.AddAt(index, (MxDataGridField) value);
                }
            }
        }

        bool IStateManager.IsTrackingViewState
        {
            get
            {
                return this.marked;
            }
        }
    }
}

