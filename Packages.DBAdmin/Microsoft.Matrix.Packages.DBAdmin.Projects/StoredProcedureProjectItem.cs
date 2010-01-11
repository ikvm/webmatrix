namespace Microsoft.Matrix.Packages.DBAdmin.Projects
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;

    public sealed class StoredProcedureProjectItem : DocumentProjectItem
    {
        private string _name;

        public StoredProcedureProjectItem(string sprocName) : base(sprocName)
        {
            this._name = sprocName;
        }

        public StoredProcedure GetStoredProcedure()
        {
            StoredProcedure procedure2;
            DatabaseProject project = (DatabaseProject) this.Project;
            try
            {
                project.Database.Connect();
                StoredProcedure procedure = project.Database.StoredProcedures[this._name];
                procedure.Refresh();
                procedure2 = procedure;
            }
            finally
            {
                project.Database.Disconnect();
            }
            return procedure2;
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }
    }
}

