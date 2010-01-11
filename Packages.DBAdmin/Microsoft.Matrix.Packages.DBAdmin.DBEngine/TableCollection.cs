namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine
{
    using System;
    using System.Reflection;

    public abstract class TableCollection : DBObjectCollection
    {
        public TableCollection(Database database) : base(database)
        {
        }

        public abstract Table Add(string name, Column[] columns);
        public abstract Table AddNew(string name);
        protected void AddTable(Table table)
        {
            base.Objects.Add(table.Name, table);
        }

        public void Delete(string name)
        {
            Table table = this[name];
            if (table != null)
            {
                table.Delete();
            }
        }

        public override string Caption
        {
            get
            {
                return "Tables";
            }
        }

        public Table this[string name]
        {
            get
            {
                return (Table) base.Objects[name];
            }
        }
    }
}

