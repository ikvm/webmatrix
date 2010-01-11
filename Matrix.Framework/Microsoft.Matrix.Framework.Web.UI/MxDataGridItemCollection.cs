namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class MxDataGridItemCollection : ICollection, IEnumerable
    {
        private ArrayList items;

        public MxDataGridItemCollection(ArrayList items)
        {
            this.items = items;
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
            return this.items.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return this.items.Count;
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

        public MxDataGridItem this[int index]
        {
            get
            {
                return (MxDataGridItem) this.items[index];
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }
    }
}

