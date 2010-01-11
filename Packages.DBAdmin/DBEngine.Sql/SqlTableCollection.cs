namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Sql
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;

    internal sealed class SqlTableCollection : TableCollection
    {
        public SqlTableCollection(SqlDatabase database) : base(database)
        {
            this.PopulateCollection();
        }

        public override Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table Add(string name, Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column[] columns)
        {
            Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table table2;
            try
            {
                base.Database.Connect();
                Interop.ITable table = (Interop.ITable) new Interop.Table();
                table.SetName(name);
                Interop.IColumns columns2 = table.GetColumns();
                bool flag = false;
                foreach (Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column column in columns)
                {
                    Interop.IColumn column2 = (Interop.IColumn) new Interop.Column();
                    column2.SetName(column.Name);
                    column2.SetDatatype(column.DataType);
                    column2.SetLength(column.Size);
                    column2.SetAllowNulls(column.AllowNulls);
                    column2.SetNumericPrecision(column.NumericPrecision);
                    column2.SetNumericScale(column.NumericScale);
                    column2.SetIdentity(column.IsIdentity);
                    column2.SetIdentitySeed(column.IdentitySeed);
                    column2.SetIdentityIncrement(column.IdentityIncrement);
                    column2.SetIsRowGuidCol(column.IsRowGuid);
                    flag |= column.InPrimaryKey;
                    column2.GetDRIDefault().SetText(column.DefaultValue);
                    columns2.Add(column2);
                }
                Interop.IKeys keys = table.GetKeys();
                if (flag)
                {
                    Interop.IKey key = (Interop.IKey) new Interop.Key();
                    key.SetType(1);
                    Interop.INames keyColumns = key.GetKeyColumns();
                    foreach (Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column column3 in columns)
                    {
                        if (column3.InPrimaryKey)
                        {
                            keyColumns.Add(column3.Name);
                        }
                    }
                    keys.Add(key);
                }
                foreach (Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column column4 in columns)
                {
                    if (column4.IsUniqueKey)
                    {
                        Interop.IKey key2 = (Interop.IKey) new Interop.Key();
                        key2.SetType(2);
                        key2.GetKeyColumns().Add(column4.Name);
                        keys.Add(key2);
                    }
                }
                ((SqlDatabase) base.Database).DmoDatabase.GetTables().Add(table);
                base.Refresh();
                table2 = base[name];
            }
            finally
            {
                base.Database.Disconnect();
            }
            return table2;
        }

        public override Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table AddNew(string name)
        {
            return this.Add(name, new SqlColumn[] { SqlColumn.DefaultColumn });
        }

        protected override void PopulateCollection()
        {
            try
            {
                base.Database.Connect();
                Interop.ITables tables = ((SqlDatabase) base.Database).DmoDatabase.GetTables();
                tables.Refresh(true);
                Interop.ITable dmoTable = null;
                int count = tables.GetCount();
                for (int i = 1; i <= count; i++)
                {
                    dmoTable = tables.Item(i, string.Empty);
                    if (!dmoTable.GetSystemObject())
                    {
                        SqlTable table = new SqlTable(base.Database);
                        table.Initialize(dmoTable);
                        base.AddTable(table);
                    }
                }
            }
            finally
            {
                base.Database.Disconnect();
            }
        }
    }
}

