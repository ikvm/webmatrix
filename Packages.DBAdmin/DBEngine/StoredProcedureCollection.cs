namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine
{
    using System;
    using System.Reflection;

    public abstract class StoredProcedureCollection : DBObjectCollection
    {
        public StoredProcedureCollection(Database database) : base(database)
        {
        }

        public abstract StoredProcedure Add(string commandText);
        public abstract StoredProcedure AddNew(string name);
        protected void AddStoredProcedure(StoredProcedure storedProcedure)
        {
            base.Objects.Add(storedProcedure.Name, storedProcedure);
        }

        public void Delete(string name)
        {
            StoredProcedure procedure = this[name];
            if (procedure != null)
            {
                procedure.Delete();
            }
        }

        public override string Caption
        {
            get
            {
                return "Stored Procedures";
            }
        }

        public StoredProcedure this[string name]
        {
            get
            {
                return (StoredProcedure) base.Objects[name];
            }
        }
    }
}

