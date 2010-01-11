namespace Microsoft.Matrix.Packages.DBAdmin.Projects.Sql
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine.Sql;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using System;

    internal sealed class SqlDatabaseProjectItem : DatabaseProjectItem
    {
        public SqlDatabaseProjectItem(SqlDatabaseProject owningProject) : base(owningProject)
        {
        }

        protected override void CreateChildItems()
        {
            if (!SqlHelper.IsDmoPresent())
            {
                SqlHelper.PromptInstallDmo(this.Project);
            }
            else
            {
                base.CreateChildItems();
            }
        }
    }
}

