namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine
{
    using System;
    using System.Collections;

    public abstract class DBObjectCollection : ICollection, IEnumerable, IDisposable
    {
        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database _database;
        private SortedList _objects;

        protected DBObjectCollection(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database database)
        {
            this._database = database;
        }

        public bool Contains(string sprocName)
        {
            return (this.Objects[sprocName] != null);
        }

        protected void Dispose(bool disposing)
        {
        }

        protected abstract void PopulateCollection();
        public void Refresh()
        {
            this.Objects.Clear();
            this.PopulateCollection();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.Objects.CopyTo(array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Objects.Values.GetEnumerator();
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
        }

        public abstract string Caption { get; }

        public int Count
        {
            get
            {
                if (this._objects != null)
                {
                    return this._objects.Count;
                }
                return 0;
            }
        }

        public Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database Database
        {
            get
            {
                return this._database;
            }
        }

        protected IDictionary Objects
        {
            get
            {
                if (this._objects == null)
                {
                    this._objects = new SortedList();
                }
                return this._objects;
            }
        }

        int ICollection.Count
        {
            get
            {
                return this.Objects.Count;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return this.Objects.IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return this.Objects.SyncRoot;
            }
        }
    }
}

