namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;

    public sealed class Key : ICloneable
    {
        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.KeyType _keyType;
        private string _name;
        private NameValueCollection _referencedColumns;
        private string _referencedTableName;
        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table _table;

        public Key(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            this._table = table;
        }

        public Key Clone()
        {
            Key key = new Key(this._table);
            key.KeyType = this.KeyType;
            key.Name = this.Name;
            key.ReferencedTableName = this.ReferencedTableName;
            foreach (string str in this.ReferencedColumns)
            {
                key.ReferencedColumns.Add(str, this.ReferencedColumns[str]);
            }
            return key;
        }

        public bool Delete()
        {
            bool flag;
            try
            {
                this._table.Database.Connect();
                flag = this._table.DeleteKey(this);
            }
            finally
            {
                this._table.Database.Disconnect();
            }
            return flag;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public Microsoft.Matrix.Packages.DBAdmin.DBEngine.KeyType KeyType
        {
            get
            {
                return this._keyType;
            }
            set
            {
                this._keyType = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public NameValueCollection ReferencedColumns
        {
            get
            {
                if (this._referencedColumns == null)
                {
                    this._referencedColumns = new NameValueCollection();
                }
                return this._referencedColumns;
            }
        }

        public string ReferencedTableName
        {
            get
            {
                return this._referencedTableName;
            }
            set
            {
                this._referencedTableName = value;
            }
        }

        [Browsable(false)]
        public Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table Table
        {
            get
            {
                return this._table;
            }
        }
    }
}

