namespace Microsoft.Matrix.Packages.DBAdmin.Projects.Access
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine.Access;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using System;

    internal sealed class AccessDatabaseProjectItem : DatabaseProjectItem
    {
        public AccessDatabaseProjectItem(AccessDatabaseProject owningProject) : base(owningProject)
        {
        }

        protected override void CreateChildItems()
        {
            if (!AccessHelper.IsAdoxPresent())
            {
                AccessHelper.PromptInstallAdox(this.Project);
            }
            else
            {
                base.CreateChildItems();
            }
        }
    }
}

