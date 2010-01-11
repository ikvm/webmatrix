namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Access
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;
    using System.Collections;
    using System.Reflection;

    internal sealed class AccessTableCollection : TableCollection
    {
        public AccessTableCollection(AccessDatabase database) : base(database)
        {
            this.PopulateCollection();
        }

        public override Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table Add(string name, Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column[] columns)
        {
            Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table table2;
            try
            {
                base.Database.Connect();
                Interop.ITable adoxTable = (Interop.ITable) new Interop.Table();
                adoxTable.SetName(name);
                adoxTable.SetParentCatalog(((AccessDatabase) base.Database).AdoxCatalog);
                CreateTableInternal(adoxTable, columns);
                ((AccessDatabase) base.Database).AdoxCatalog.GetTables().Append(adoxTable);
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
            return this.Add(name, new AccessColumn[] { AccessColumn.DefaultColumn });
        }

        internal static void CreateTableInternal(Interop.ITable adoxTable, System.Collections.ICollection columns)
        {
            Interop.IColumns columns2 = adoxTable.GetColumns();
            bool flag = false;
            foreach (AccessColumn column in columns)
            {
                AccessDataType accessDataType = AccessColumn.GetAccessDataType(column.DataType);
                columns2.Append(column.Name, (int) AccessColumn.ConvertAccessToAdoxDataType(accessDataType), AccessColumn.GetDefaultSize(accessDataType));
                if (AccessColumn.SupportsSize(accessDataType))
                {
                    columns2.GetItem(column.Name).SetDefinedSize(column.Size);
                }
                if (AccessColumn.SupportsNumericPrecision(accessDataType))
                {
                    columns2.GetItem(column.Name).SetPrecision(column.NumericPrecision);
                }
                if (AccessColumn.SupportsNumericScale(accessDataType))
                {
                    columns2.GetItem(column.Name).SetNumericScale((byte) column.NumericScale);
                }
                if (AccessColumn.SupportsIdentity(accessDataType) && column.IsIdentity)
                {
                    columns2.GetItem(column.Name).GetProperties().GetItem("Autoincrement").SetValue(column.IsIdentity);
                    columns2.GetItem(column.Name).GetProperties().GetItem("Increment").SetValue(column.IdentityIncrement);
                    columns2.GetItem(column.Name).GetProperties().GetItem("Seed").SetValue(column.IdentitySeed);
                }
                columns2.GetItem(column.Name).GetProperties().GetItem("Default").SetValue(column.DefaultValue);
                columns2.GetItem(column.Name).GetProperties().GetItem("Nullable").SetValue(column.AllowNulls);
                if (AccessColumn.SupportsAutoGenerate(accessDataType))
                {
                    columns2.GetItem(column.Name).GetProperties().GetItem("Jet OLEDB:AutoGenerate").SetValue(column.AutoGenerate);
                }
                columns2.GetItem(column.Name).GetProperties().GetItem("Description").SetValue(column.Description);
                flag |= column.InPrimaryKey;
            }
            Interop.IKeys keys = adoxTable.GetKeys();
            if (flag)
            {
                Interop.IKey item = (Interop.IKey) new Interop.Key();
                item.SetName("PK_" + Guid.NewGuid().ToString());
                item.SetType(1);
                Interop.IColumns columns3 = item.GetColumns();
                foreach (Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column column2 in columns)
                {
                    if (column2.InPrimaryKey)
                    {
                        columns3.Append(column2.Name, 0xca, 0);
                    }
                }
                keys.Append(item, 1, Missing.Value, "", "");
            }
            foreach (Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column column3 in columns)
            {
                if (column3.IsUniqueKey)
                {
                    Interop.IKey key2 = (Interop.IKey) new Interop.Key();
                    key2.SetType(3);
                    key2.SetName("UQ_" + Guid.NewGuid().ToString());
                    key2.GetColumns().Append(column3.Name, 0xca, 0);
                    keys.Append(key2, 1, Missing.Value, "", "");
                }
            }
        }

        protected override void PopulateCollection()
        {
            try
            {
                base.Database.Connect();
                Interop.ITables tables = ((AccessDatabase) base.Database).AdoxCatalog.GetTables();
                tables.Refresh();
                int count = tables.GetCount();
                for (int i = 0; i < count; i++)
                {
                    Interop.ITable item = tables.GetItem(i);
                    if (item.GetType() == "TABLE")
                    {
                        AccessTable table = new AccessTable(base.Database);
                        table.Initialize(item);
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

