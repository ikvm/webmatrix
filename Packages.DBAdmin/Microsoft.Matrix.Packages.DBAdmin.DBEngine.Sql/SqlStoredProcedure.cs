namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine.Sql
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;

    internal sealed class SqlStoredProcedure : Microsoft.Matrix.Packages.DBAdmin.DBEngine.StoredProcedure
    {
        public SqlStoredProcedure(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database database) : base(database)
        {
        }

        public override bool Delete()
        {
            bool flag;
            try
            {
                base.Database.Connect();
                this.GetDmoSproc().Remove();
                base.Database.StoredProcedures.Refresh();
                flag = true;
            }
            finally
            {
                base.Database.Disconnect();
            }
            return flag;
        }

        private Interop.IStoredProcedure GetDmoSproc()
        {
            Interop.IStoredProcedures storedProcedures = ((SqlDatabase) base.Database).DmoDatabase.GetStoredProcedures();
            Interop.IStoredProcedure procedure = null;
            int count = storedProcedures.GetCount();
            for (int i = 1; i <= count; i++)
            {
                procedure = storedProcedures.Item(i, string.Empty);
                if (procedure.GetName().ToLower() == this.Name.ToLower())
                {
                    return procedure;
                }
            }
            return procedure;
        }

        internal void Initialize(Interop.IStoredProcedure dmoSproc)
        {
            base.SetName(dmoSproc.GetName());
            base.SetCommandText(dmoSproc.GetText());
            base.SetCreationDate(DateTime.Parse(dmoSproc.GetCreateDate()));
        }

        public override void Refresh()
        {
            try
            {
                base.Database.Connect();
                Interop.IStoredProcedure dmoSproc = this.GetDmoSproc();
                this.Initialize(dmoSproc);
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

        public override bool Update(string commandText)
        {
            bool flag;
            try
            {
                base.Database.Connect();
                this.GetDmoSproc().Alter(commandText);
                this.Refresh();
                flag = true;
            }
            finally
            {
                base.Database.Disconnect();
            }
            return flag;
        }
    }
}

