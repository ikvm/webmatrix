namespace Microsoft.Matrix.Packages.DBAdmin.Projects
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;

    public sealed class TableGroupProjectItem : FolderProjectItem
    {
        public TableGroupProjectItem(DatabaseProject owningProject) : base("Tables")
        {
        }

        protected override void CreateChildItems()
        {
            DatabaseProject project = (DatabaseProject) this.Project;
            TableCollection tables = null;
            try
            {
                tables = project.Database.Tables;
                tables.Refresh();
            }
            catch (Exception exception)
            {
                ((IMxUIService) ((IServiceProvider) this.Project).GetService(typeof(IMxUIService))).ReportError(exception, "Error retrieving list of tables.", false);
            }
            if ((tables != null) && (tables.Count != 0))
            {
                foreach (Table table in (IEnumerable) tables)
                {
                    ProjectItem item = new TableProjectItem(table.Name);
                    base.AddChildItem(item);
                }
            }
            base.CreateChildItems();
        }
    }
}

