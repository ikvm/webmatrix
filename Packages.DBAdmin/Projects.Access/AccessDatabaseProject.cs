namespace Microsoft.Matrix.Packages.DBAdmin.Projects.Access
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine.Access;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using System;

    internal sealed class AccessDatabaseProject : DatabaseProject, IDataProviderDatabaseProject
    {
        private AccessDatabase _accessDatabase;
        private AccessDatabaseProjectItem _rootItem;

        public AccessDatabaseProject(IProjectFactory factory, IServiceProvider serviceProvider, AccessDatabase database) : base(factory, serviceProvider, database)
        {
            this._accessDatabase = database;
            this._rootItem = new AccessDatabaseProjectItem(this);
        }

        protected override string GetProjectItemPathInternal(Microsoft.Matrix.Core.Projects.ProjectItem projectItem)
        {
            if (projectItem is DatabaseProjectItem)
            {
                return (this._accessDatabase.ConnectionSettings.Filename + "." + ((DatabaseProjectItem) projectItem).Caption);
            }
            if (projectItem is TableProjectItem)
            {
                return (this._accessDatabase.ConnectionSettings.Filename + "." + ((TableProjectItem) projectItem).Caption);
            }
            if (projectItem is StoredProcedureProjectItem)
            {
                return (this._accessDatabase.ConnectionSettings.Filename + "." + ((StoredProcedureProjectItem) projectItem).Caption);
            }
            return string.Empty;
        }

        public override bool ValidateProjectItemName(string name)
        {
            return AccessHelper.IsValidIdentifier(name);
        }

        IDataProviderDatabase IDataProviderDatabaseProject.Database
        {
            get
            {
                return (IDataProviderDatabase) base.Database;
            }
        }

        public override RootProjectItem ProjectItem
        {
            get
            {
                return this._rootItem;
            }
        }
    }
}

