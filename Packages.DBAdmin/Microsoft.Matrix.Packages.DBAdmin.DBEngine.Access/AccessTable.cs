namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Access
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.OleDb;
    using System.Reflection;

    internal sealed class AccessTable : Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table
    {
        public AccessTable(Database database) : base(database)
        {
        }

        protected override void CreateKeys(System.Collections.ICollection keys)
        {
            Interop.ITables adoxTables = ((AccessDatabase) base.Database).AdoxCatalog.GetTables();
            foreach (Microsoft.Matrix.Packages.DBAdmin.DBEngine.Key key in keys)
            {
                if (key.Table.Name == this.Name)
                {
                    Interop.ITable table = GetAdoxTable(adoxTables, key.Table.Name);
                    if (key.KeyType == KeyType.Unique)
                    {
                        Interop.IKey key2 = (Interop.IKey) new Interop.Key();
                        key2.SetType(3);
                        key2.SetName("UQ_" + Guid.NewGuid().ToString());
                        Interop.IColumns columns = key2.GetColumns();
                        foreach (string str in key.ReferencedColumns)
                        {
                            columns.Append(str, 0xca, 0);
                        }
                        table.GetKeys().Append(key2, 1, Missing.Value, "", "");
                    }
                    else
                    {
                        Interop.IKey key3 = (Interop.IKey) new Interop.Key();
                        key3.SetType(2);
                        key3.SetName("FK_" + Guid.NewGuid().ToString());
                        key3.SetRelatedTable(key.ReferencedTableName);
                        Interop.IColumns columns2 = key3.GetColumns();
                        foreach (string str2 in key.ReferencedColumns)
                        {
                            Interop.IColumn column = (Interop.IColumn) new Interop.Column();
                            column.SetName(str2);
                            column.SetRelatedColumn(key.ReferencedColumns[str2]);
                            columns2.Append(column, 0xca, 0);
                        }
                        table.GetKeys().Append(key3, 1, Missing.Value, "", "");
                    }
                    continue;
                }
                Interop.ITable adoxTable = GetAdoxTable(adoxTables, key.Table.Name);
                Interop.IKey item = (Interop.IKey) new Interop.Key();
                item.SetType(2);
                item.SetName("FK_" + Guid.NewGuid().ToString());
                item.SetRelatedTable(this.Name);
                Interop.IColumns columns3 = item.GetColumns();
                foreach (string str3 in key.ReferencedColumns)
                {
                    Interop.IColumn column2 = (Interop.IColumn) new Interop.Column();
                    column2.SetName(str3);
                    column2.SetRelatedColumn(key.ReferencedColumns[str3]);
                    columns3.Append(column2, 0xca, 0);
                }
                adoxTable.GetKeys().Append(item, 1, Missing.Value, "", "");
            }
        }

        public override bool Delete()
        {
            bool flag;
            try
            {
                base.Database.Connect();
                DeleteInternal(((AccessDatabase) base.Database).AdoxCatalog.GetTables(), this.Name);
                base.Database.Tables.Refresh();
                flag = true;
            }
            finally
            {
                base.Database.Disconnect();
            }
            return flag;
        }

        private static void DeleteInternal(Interop.ITables adoxTables, string name)
        {
            Interop.ITable adoxTable = GetAdoxTable(adoxTables, name);
            if (adoxTable != null)
            {
                Interop.IKeys keys = adoxTable.GetKeys();
                keys.Refresh();
                int count = keys.GetCount();
                ArrayList list = new ArrayList();
                for (int i = 0; i < count; i++)
                {
                    Interop.IKey item = keys.GetItem(i);
                    if (item.GetType() == 2)
                    {
                        list.Add(item.GetName());
                    }
                }
                foreach (string str in list)
                {
                    keys.Delete(str);
                }
                int num3 = adoxTables.GetCount();
                for (int j = 0; j < num3; j++)
                {
                    Interop.IKeys keys2 = adoxTables.GetItem(j).GetKeys();
                    keys2.Refresh();
                    int num5 = keys2.GetCount();
                    ArrayList list2 = new ArrayList();
                    for (int k = 0; k < num5; k++)
                    {
                        Interop.IKey key2 = keys2.GetItem(k);
                        if ((key2.GetType() == 2) && (key2.GetRelatedTable() == name))
                        {
                            list2.Add(key2.GetName());
                        }
                    }
                    foreach (string str2 in list2)
                    {
                        keys2.Delete(str2);
                    }
                }
                adoxTables.Delete(name);
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
            Interop.ITable adoxTable = this.GetAdoxTable(this.Name);
            if (adoxTable != null)
            {
                Interop.IKeys keys = adoxTable.GetKeys();
                keys.Refresh();
                int count = keys.GetCount();
                for (int i = 0; i < count; i++)
                {
                    if (keys.GetItem(i).GetName() == key.Name)
                    {
                        keys.Delete(key.Name);
                        return true;
                    }
                }
            }
            return false;
        }

        private Interop.ITable GetAdoxTable(string name)
        {
            Interop.ITables adoxTables = ((AccessDatabase) base.Database).AdoxCatalog.GetTables();
            adoxTables.Refresh();
            return GetAdoxTable(adoxTables, name);
        }

        private static Interop.ITable GetAdoxTable(Interop.ITables adoxTables, string name)
        {
            Interop.ITable item = null;
            int count = adoxTables.GetCount();
            for (int i = 0; i < count; i++)
            {
                item = adoxTables.GetItem(i);
                if (item.GetName().ToLower() == name.ToLower())
                {
                    return item;
                }
            }
            return null;
        }

        private ArrayList GetColumnOrderList()
        {
            ArrayList list = new ArrayList();
            Interop.ICatalog adoxCatalog = null;
            try
            {
                Interop.IAdoConnection pVal = (Interop.IAdoConnection) new Interop.AdoConnection();
                pVal.SetProvider("MSDASQL.1;");
                pVal.Open("Driver={Microsoft Access Driver (*.mdb)};DBQ=" + ((AccessDatabase) base.Database).ConnectionSettings.Filename, "", "", -1);
                adoxCatalog = (Interop.ICatalog) new Interop.Catalog();
                adoxCatalog.SetActiveConnection(pVal);
                Interop.ITables tables = adoxCatalog.GetTables();
                Interop.ITable item = null;
                int count = tables.GetCount();
                for (int i = 0; i < count; i++)
                {
                    item = tables.GetItem(i);
                    if (item.GetName().ToLower() == this.Name.ToLower())
                    {
                        break;
                    }
                }
                Interop.IColumns columns = item.GetColumns();
                int num3 = columns.GetCount();
                for (int j = 0; j < num3; j++)
                {
                    Interop.IColumn column = columns.GetItem(j);
                    new AccessColumn(this);
                    list.Add(column.GetName());
                }
            }
            finally
            {
                AccessDatabase.CloseAdoxCatalog(adoxCatalog);
            }
            return list;
        }

        public override int GetRowCount()
        {
            int num;
            OleDbConnection connection = (OleDbConnection) ((IDataProviderDatabase) base.Database).CreateConnection();
            try
            {
                OleDbCommand command = new OleDbCommand("select count(*) from [" + this.Name + "]", connection);
                connection.Open();
                num = (int) command.ExecuteScalar();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                OleDbConnection.ReleaseObjectPool();
            }
            return num;
        }

        internal void Initialize(Interop.ITable adoxTable)
        {
            base.SetName(adoxTable.GetName());
            base.SetCreationDate((DateTime) adoxTable.GetDateCreated());
            base.SetLastModifiedDate((DateTime) adoxTable.GetDateModified());
        }

        protected override void PopulateColumnCollection()
        {
            ArrayList columnOrderList = this.GetColumnOrderList();
            for (int i = 0; i < columnOrderList.Count; i++)
            {
                base.ColumnObjects.Add(new object());
            }
            try
            {
                base.Database.Connect();
                Interop.IColumns columns = this.GetAdoxTable(this.Name).GetColumns();
                int count = columns.GetCount();
                for (int j = 0; j < count; j++)
                {
                    Interop.IColumn item = columns.GetItem(j);
                    AccessColumn column2 = new AccessColumn(this);
                    column2.Initialize(item);
                    int index = columnOrderList.IndexOf(column2.Name);
                    base.ColumnObjects[index] = column2;
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
                Interop.IKeys keys = this.GetAdoxTable(this.Name).GetKeys();
                int count = keys.GetCount();
                for (int i = 0; i < count; i++)
                {
                    Interop.IKey item = keys.GetItem(i);
                    Microsoft.Matrix.Packages.DBAdmin.DBEngine.Key key2 = new Microsoft.Matrix.Packages.DBAdmin.DBEngine.Key(this);
                    key2.Name = item.GetName();
                    switch (item.GetType())
                    {
                        case 1:
                            key2.KeyType = KeyType.Primary;
                            break;

                        case 2:
                            key2.KeyType = KeyType.Foreign;
                            break;

                        case 3:
                            key2.KeyType = KeyType.Unique;
                            break;

                        default:
                            throw new Exception("Unknown key type: " + item.GetType());
                    }
                    if (key2.KeyType == KeyType.Foreign)
                    {
                        key2.ReferencedTableName = item.GetRelatedTable();
                        Interop.IColumns columns = item.GetColumns();
                        int num3 = columns.GetCount();
                        for (int j = 0; j < num3; j++)
                        {
                            Interop.IColumn column = columns.GetItem(j);
                            key2.ReferencedColumns.Add(column.GetName(), column.GetRelatedColumn());
                        }
                    }
                    else
                    {
                        Interop.IColumns columns2 = item.GetColumns();
                        int num5 = columns2.GetCount();
                        for (int k = 0; k < num5; k++)
                        {
                            Interop.IColumn column2 = columns2.GetItem(k);
                            key2.ReferencedColumns.Add(column2.GetName(), string.Empty);
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
                Interop.ITable adoxTable = this.GetAdoxTable(this.Name);
                this.Initialize(adoxTable);
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
            string pVal = "temp__" + Guid.NewGuid().ToString();
            try
            {
                base.Database.Connect();
                string[] dirtyColumnNames = base.GetDirtyColumnNames(base.Columns, newColumns);
                System.Collections.ICollection referencedKeys = base.GetReferencedKeys();
                System.Collections.ICollection keys = base.RemoveReferencedDirtyKeys(dirtyColumnNames, referencedKeys);
                this.GetAdoxTable(this.Name).SetName(pVal);
                flag = true;
                base.Database.Tables.Add(this.Name, newColumns);
                this.CreateKeys(keys);
                DeleteInternal(((AccessDatabase) base.Database).AdoxCatalog.GetTables(), pVal);
                base.Database.Tables.Refresh();
                this.Refresh();
            }
            catch
            {
                if (flag)
                {
                    Interop.ITables tables = ((AccessDatabase) base.Database).AdoxCatalog.GetTables();
                    tables.Refresh();
                    DeleteInternal(tables, this.Name);
                    tables.GetItem(pVal).SetName(this.Name);
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

