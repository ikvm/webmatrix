namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Sql
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;

    internal sealed class SqlStoredProcedureCollection : StoredProcedureCollection
    {
        private const string NewStoredProcedureTemplate = "CREATE PROCEDURE {0} AS\r\nGO";

        public SqlStoredProcedureCollection(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database database) : base(database)
        {
            this.PopulateCollection();
        }

        public override Microsoft.Matrix.Packages.DBAdmin.DBEngine.StoredProcedure Add(string commandText)
        {
            Microsoft.Matrix.Packages.DBAdmin.DBEngine.StoredProcedure procedure2;
            try
            {
                base.Database.Connect();
                Interop.IStoredProcedure procedure = (Interop.IStoredProcedure) new Interop.StoredProcedure();
                procedure.SetText(commandText);
                ((SqlDatabase) base.Database).DmoDatabase.GetStoredProcedures().Add(procedure);
                string name = procedure.GetName();
                base.Refresh();
                procedure2 = base[name];
            }
            finally
            {
                base.Database.Disconnect();
            }
            return procedure2;
        }

        public override Microsoft.Matrix.Packages.DBAdmin.DBEngine.StoredProcedure AddNew(string name)
        {
            return this.Add(string.Format("CREATE PROCEDURE {0} AS\r\nGO", name));
        }

        protected override void PopulateCollection()
        {
            try
            {
                base.Database.Connect();
                Interop.IStoredProcedures storedProcedures = ((SqlDatabase) base.Database).DmoDatabase.GetStoredProcedures();
                storedProcedures.Refresh(true);
                Interop.IStoredProcedure dmoSproc = null;
                int count = storedProcedures.GetCount();
                for (int i = 1; i <= count; i++)
                {
                    dmoSproc = storedProcedures.Item(i, string.Empty);
                    if (!dmoSproc.GetSystemObject())
                    {
                        SqlStoredProcedure storedProcedure = new SqlStoredProcedure(base.Database);
                        storedProcedure.Initialize(dmoSproc);
                        base.AddStoredProcedure(storedProcedure);
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

