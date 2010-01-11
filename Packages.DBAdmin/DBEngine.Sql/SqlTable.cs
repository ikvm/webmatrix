namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Sql
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;
    using System.Collections;

    internal sealed class SqlTable : Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table
    {
        public SqlTable(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database database) : base(database)
        {
        }

        protected override void CreateKeys(ICollection keys)
        {
            foreach (Microsoft.Matrix.Packages.DBAdmin.DBEngine.Key key in keys)
            {
                if (key.Table.Name == this.Name)
                {
                    Interop.ITable table = this.GetDmoTable(key.Table.Name);
                    if (key.KeyType == KeyType.Unique)
                    {
                        Interop.IKey key2 = (Interop.IKey) new Interop.Key();
                        key2.SetType(2);
                        key2.SetName("UQ_" + Guid.NewGuid().ToString());
                        Interop.INames names = key2.GetKeyColumns();
                        foreach (string str in key.ReferencedColumns)
                        {
                            names.Add(str);
                        }
                        table.GetKeys().Add(key2);
                    }
                    else
                    {
                        Interop.IKey key3 = (Interop.IKey) new Interop.Key();
                        key3.SetType(3);
                        key3.SetName("FK_" + Guid.NewGuid().ToString());
                        key3.SetReferencedTable(key.ReferencedTableName);
                        Interop.INames names2 = key3.GetKeyColumns();
                        Interop.INames names3 = key3.GetReferencedColumns();
                        foreach (string str2 in key.ReferencedColumns)
                        {
                            names2.Add(str2);
                            names3.Add(key.ReferencedColumns[str2]);
                        }
                        table.GetKeys().Add(key3);
                    }
                    continue;
                }
                Interop.ITable dmoTable = this.GetDmoTable(key.Table.Name);
                Interop.IKey key4 = (Interop.IKey) new Interop.Key();
                key4.SetType(3);
                key4.SetName("FK_" + Guid.NewGuid().ToString());
                key4.SetReferencedTable(this.Name);
                Interop.INames keyColumns = key4.GetKeyColumns();
                Interop.INames referencedColumns = key4.GetReferencedColumns();
                foreach (string str3 in key.ReferencedColumns)
                {
                    keyColumns.Add(str3);
                    referencedColumns.Add(key.ReferencedColumns[str3]);
                }
                dmoTable.GetKeys().Add(key4);
            }
        }

        public override bool Delete()
        {
            bool flag;
            try
            {
                base.Database.Connect();
                DeleteInternal(((SqlDatabase) base.Database).DmoDatabase.GetTables(), this.Name);
                base.Database.Tables.Refresh();
                flag = true;
            }
            finally
            {
                base.Database.Disconnect();
            }
            return flag;
        }

        private static void DeleteInternal(Interop.ITables dmoTables, string name)
        {
            Interop.ITable dmoTable = GetDmoTable(dmoTables, name);
            if (dmoTable != null)
            {
                Interop.IKeys keys = dmoTable.GetKeys();
                keys.Refresh(true);
                int count = keys.GetCount();
                ArrayList list = new ArrayList();
                for (int i = count; i > 0; i--)
                {
                    if (keys.Item(i).GetType() == 3)
                    {
                        list.Add(i);
                    }
                }
                foreach (int num3 in list)
                {
                    keys.Remove(num3);
                }
                int num4 = dmoTables.GetCount();
                for (int j = 1; j <= num4; j++)
                {
                    Interop.IKeys keys2 = dmoTables.Item(j, "").GetKeys();
                    keys2.Refresh(true);
                    int num6 = keys2.GetCount();
                    ArrayList list2 = new ArrayList();
                    for (int k = num6; k > 0; k--)
                    {
                        Interop.IKey key2 = keys2.Item(k);
                        if ((key2.GetType() == 3) && SqlHelper.TableNamesEqual(name, key2.GetReferencedTable()))
                        {
                            list2.Add(k);
                        }
                    }
                    foreach (int num8 in list2)
                    {
                        keys2.Remove(num8);
                    }
                }
                dmoTable.Remove();
            }
        }

        protected internal override bool DeleteKey(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Key key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (key.Table != this)
            {
                throw new ArgumentException("Cannot delete key that is not associated with this table.", "key");
            }
            Interop.ITable dmoTable = this.GetDmoTable(this.Name);
            if (dmoTable != null)
            {
                Interop.IKeys keys = dmoTable.GetKeys();
                keys.Refresh(true);
                int count = keys.GetCount();
                for (int i = 1; i <= count; i++)
                {
                    if (keys.Item(i).GetName() == key.Name)
                    {
                        keys.Remove(i);
                        return true;
                    }
                }
            }
            return false;
        }

        private Interop.ITable GetDmoTable(string name)
        {
            Interop.ITables dmoTables = ((SqlDatabase) base.Database).DmoDatabase.GetTables();
            dmoTables.Refresh(false);
            return GetDmoTable(dmoTables, name);
        }

        private static Interop.ITable GetDmoTable(Interop.ITables dmoTables, string name)
        {
            Interop.ITable table = null;
            int count = dmoTables.GetCount();
            for (int i = 1; i <= count; i++)
            {
                table = dmoTables.Item(i, string.Empty);
                if (table.GetName().ToLower() == name.ToLower())
                {
                    return table;
                }
            }
            return null;
        }

        public override int GetRowCount()
        {
            int rows;
            try
            {
                base.Database.Connect();
                rows = this.GetDmoTable(this.Name).GetRows();
            }
            finally
            {
                base.Database.Disconnect();
            }
            return rows;
        }

        internal void Initialize(Interop.ITable dmoTable)
        {
            base.SetName(dmoTable.GetName());
            base.SetCreationDate(DateTime.Parse(dmoTable.GetCreateDate()));
            base.SetLastModifiedDate(DateTime.MinValue);
        }

        protected override void PopulateColumnCollection()
        {
            try
            {
                base.Database.Connect();
                Interop.IColumns columns = this.GetDmoTable(this.Name).GetColumns();
                int count = columns.GetCount();
                for (int i = 1; i <= count; i++)
                {
                    Interop.IColumn dmoColumn = columns.Item(i);
                    SqlColumn column2 = new SqlColumn(this);
                    column2.Initialize(dmoColumn);
                    base.ColumnObjects.Add(column2);
                }
            }
            finally
            {
                base.Database.Disconnect();
            }
        }

        protected override void PopulateKeyCollection()
        {
            try
            {
                base.Database.Connect();
                Interop.IKeys keys = this.GetDmoTable(this.Name).GetKeys();
                int count = keys.GetCount();
                for (int i = 1; i <= count; i++)
                {
                    Interop.IKey key = keys.Item(i);
                    Microsoft.Matrix.Packages.DBAdmin.DBEngine.Key key2 = new Microsoft.Matrix.Packages.DBAdmin.DBEngine.Key(this);
                    key2.Name = key.GetName();
                    switch (key.GetType())
                    {
                        case 1:
                            key2.KeyType = KeyType.Primary;
                            break;

                        case 2:
                            key2.KeyType = KeyType.Unique;
                            break;

                        case 3:
                            key2.KeyType = KeyType.Foreign;
                            break;

                        default:
                            throw new Exception("Unknown key type: " + key.GetType());
                    }
                    if (key2.KeyType == KeyType.Foreign)
                    {
                        string[] identifierParts = SqlHelper.GetIdentifierParts(key.GetReferencedTable());
                        key2.ReferencedTableName = identifierParts[identifierParts.Length - 1];
                        Interop.INames keyColumns = key.GetKeyColumns();
                        Interop.INames referencedColumns = key.GetReferencedColumns();
                        int num3 = keyColumns.GetCount();
                        for (int j = 1; j <= num3; j++)
                        {
                            key2.ReferencedColumns.Add(keyColumns.Item(j), referencedColumns.Item(j));
                        }
                    }
                    else
                    {
                        Interop.INames names3 = key.GetKeyColumns();
                        int num5 = names3.GetCount();
                        for (int k = 1; k <= num5; k++)
                        {
                            key2.ReferencedColumns.Add(names3.Item(k), string.Empty);
                        }
                    }
                    base.KeyObjects.Add(key2);
                }
            }
            finally
            {
                base.Database.Disconnect();
            }
        }

        public override void Refresh()
        {
            try
            {
                base.Database.Connect();
                base.Refresh();
                Interop.ITable dmoTable = this.GetDmoTable(this.Name);
                dmoTable.Refresh();
                this.Initialize(dmoTable);
            }
            finally
            {
                base.Database.Disconnect();
            }
        }

        public override bool Rename(string newName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateColumns(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column[] newColumns)
        {
            bool flag = false;
            string pRetVal = "temp__" + Guid.NewGuid().ToString();
            try
            {
                base.Database.Connect();
                string[] dirtyColumnNames = base.GetDirtyColumnNames(base.Columns, newColumns);
                ICollection referencedKeys = base.GetReferencedKeys();
                ICollection keys = base.RemoveReferencedDirtyKeys(dirtyColumnNames, referencedKeys);
                this.GetDmoTable(this.Name).SetName(pRetVal);
                flag = true;
                base.Database.Tables.Add(this.Name, newColumns);
                this.CreateKeys(keys);
                DeleteInternal(((SqlDatabase) base.Database).DmoDatabase.GetTables(), pRetVal);
                base.Database.Tables.Refresh();
                this.Refresh();
            }
            catch
            {
                if (flag)
                {
                    Interop.ITables tables = ((SqlDatabase) base.Database).DmoDatabase.GetTables();
                    tables.Refresh(false);
                    DeleteInternal(tables, this.Name);
                    tables.Item(pRetVal, "").SetName(this.Name);
                }
                throw;
            }
            finally
            {
                base.Database.Disconnect();
            }
        }
    }
}

