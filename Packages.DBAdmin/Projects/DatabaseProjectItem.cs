namespace Microsoft.Matrix.Packages.DBAdmin.Projects
{
    using Microsoft.Matrix.Core.Projects;
    using System;

    public class DatabaseProjectItem : RootProjectItem
    {
        public DatabaseProjectItem(DatabaseProject owningProject) : base(owningProject, owningProject.Database.DisplayName)
        {
            base.SetFlags(ProjectItemFlags.SupportsAddItem, false);
        }

        protected override void CreateChildItems()
        {
            DatabaseProject owningProject = (DatabaseProject) this.Project;
            base.AddChildItem(new TableGroupProjectItem(owningProject));
            if (owningProject.Database.SupportsStoredProcedures)
            {
                base.AddChildItem(new StoredProcedureGroupProjectItem(owningProject));
            }
            base.CreateChildItems();
        }

        public override bool ShowChildrenByDefault
        {
            get
            {
                return true;
            }
        }
    }
}

