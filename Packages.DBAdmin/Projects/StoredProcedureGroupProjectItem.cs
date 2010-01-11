namespace Microsoft.Matrix.Packages.DBAdmin.Projects
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;

    public sealed class StoredProcedureGroupProjectItem : FolderProjectItem
    {
        public StoredProcedureGroupProjectItem(DatabaseProject owningProject) : base("Stored Procedures")
        {
        }

        protected override void CreateChildItems()
        {
            DatabaseProject project = (DatabaseProject) this.Project;
            StoredProcedureCollection storedProcedures = null;
            try
            {
                storedProcedures = project.Database.StoredProcedures;
                storedProcedures.Refresh();
            }
            catch (Exception exception)
            {
                ((IMxUIService) ((IServiceProvider) this.Project).GetService(typeof(IMxUIService))).ReportError(exception, "Error retrieving list of stored procedures.", false);
            }
            if ((storedProcedures != null) && (storedProcedures.Count != 0))
            {
                foreach (StoredProcedure procedure in (IEnumerable) storedProcedures)
                {
                    ProjectItem item = new StoredProcedureProjectItem(procedure.Name);
                    base.AddChildItem(item);
                }
            }
            base.CreateChildItems();
        }
    }
}

