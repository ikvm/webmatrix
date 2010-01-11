namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine
{
    using System;
    using System.Collections;

    public abstract class Table : DBObject
    {
        private ArrayList _columnObjects;
        private DateTime _creationDate;
        private ArrayList _keyObjects;
        private DateTime _lastModifiedDate;

        public Table(Database database) : base(database)
        {
        }

        public bool CheckForDirtyKeys(ICollection newColumns)
        {
            bool flag;
            try
            {
                base.Database.Connect();
                string[] dirtyColumnNames = this.GetDirtyColumnNames(this.Columns, newColumns);
                ICollection referencedKeys = this.GetReferencedKeys();
                int count = referencedKeys.Count;
                flag = this.RemoveReferencedDirtyKeys(dirtyColumnNames, referencedKeys).Count < count;
            }
            finally
            {
                base.Database.Disconnect();
            }
            return flag;
        }

        public void CreateKey(Key key)
        {
            if (key.KeyType == KeyType.Primary)
            {
                throw new ArgumentException("The key specified must not be a primary key. Only foreign and unique keys can be created using this method.", "key");
            }
            try
            {
                base.Database.Connect();
                ArrayList keys = new ArrayList(1);
                keys.Add(key);
                this.CreateKeys(keys);
            }
            finally
            {
                base.Database.Disconnect();
            }
        }

        protected abstract void CreateKeys(ICollection keys);
        public abstract bool Delete();
        protected internal abstract bool DeleteKey(Key key);
        protected string[] GetDirtyColumnNames(ICollection originalColumns, ICollection newColumns)
        {
            ArrayList list = new ArrayList();
            foreach (Column column in originalColumns)
            {
                bool flag = false;
                foreach (Column column2 in newColumns)
                {
                    if (column.IsEquivalentTo(column2))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    list.Add(column.Name);
                }
            }
            return (string[]) list.ToArray(typeof(string));
        }

        protected ICollection GetReferencedKeys()
        {
            ArrayList list = new ArrayList();
            TableCollection tables = base.Database.Tables;
            tables.Refresh();
            foreach (Table table in (IEnumerable) tables)
            {
                if (table.Name != this.Name)
                {
                    foreach (Key key in table.Keys)
                    {
                        if ((key.KeyType == KeyType.Foreign) && (key.ReferencedTableName == this.Name))
                        {
                            list.Add(key);
                        }
                    }
                }
                else
                {
                    foreach (Key key2 in table.Keys)
                    {
                        if (key2.KeyType == KeyType.Foreign)
                        {
                            list.Add(key2);
                        }
                        else if ((key2.KeyType == KeyType.Unique) && (key2.ReferencedColumns.Count > 1))
                        {
                            list.Add(key2);
                        }
                    }
                }
            }
            return list;
        }

        public abstract int GetRowCount();
        protected abstract void PopulateColumnCollection();
        protected abstract void PopulateKeyCollection();
        public virtual void Refresh()
        {
            this._columnObjects = null;
            this._keyObjects = null;
        }

        protected ICollection RemoveReferencedDirtyKeys(string[] dirtyColumnNames, ICollection referencedKeys)
        {
            ArrayList list = new ArrayList(dirtyColumnNames);
            ArrayList list2 = new ArrayList();
            foreach (Key key in referencedKeys)
            {
                if (key.Table.Name == this.Name)
                {
                    foreach (string str in key.ReferencedColumns)
                    {
                        if (list.Contains(str) && !list2.Contains(key))
                        {
                            list2.Add(key);
                        }
                    }
                }
                else
                {
                    foreach (string str2 in key.ReferencedColumns)
                    {
                        if (list.Contains(key.ReferencedColumns[str2]) && !list2.Contains(key))
                        {
                            list2.Add(key);
                        }
                    }
                }
            }
            ArrayList list3 = new ArrayList(referencedKeys);
            foreach (Key key2 in list2)
            {
                list3.Remove(key2);
            }
            return list3;
        }

        public abstract bool Rename(string newName);
        protected void SetCreationDate(DateTime creationDate)
        {
            this._creationDate = creationDate;
        }

        protected void SetLastModifiedDate(DateTime lastModifiedDate)
        {
            this._lastModifiedDate = lastModifiedDate;
        }

        public abstract void UpdateColumns(Column[] newColumns);

        protected IList ColumnObjects
        {
            get
            {
                if (this._columnObjects == null)
                {
                    this._columnObjects = new ArrayList();
                    this.PopulateColumnCollection();
                }
                return this._columnObjects;
            }
        }

        public ICollection Columns
        {
            get
            {
                return this.ColumnObjects;
            }
        }

        public virtual DateTime CreationDate
        {
            get
            {
                return this._creationDate;
            }
        }

        protected IList KeyObjects
        {
            get
            {
                if (this._keyObjects == null)
                {
                    this._keyObjects = new ArrayList();
                    this.PopulateKeyCollection();
                }
                return this._keyObjects;
            }
        }

        public ICollection Keys
        {
            get
            {
                return this.KeyObjects;
            }
        }

        public virtual DateTime LastModifiedDate
        {
            get
            {
                return this._lastModifiedDate;
            }
        }
    }
}

