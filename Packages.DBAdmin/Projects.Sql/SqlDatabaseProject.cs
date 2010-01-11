namespace Microsoft.Matrix.Packages.DBAdmin.Projects.Sql
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine.Sql;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using System;

    internal sealed class SqlDatabaseProject : DatabaseProject, IDataProviderDatabaseProject
    {
        private SqlDatabaseProjectItem _rootItem;
        private SqlDatabase _sqlDatabase;

        public SqlDatabaseProject(IProjectFactory factory, IServiceProvider serviceProvider, SqlDatabase database) : base(factory, serviceProvider, database)
        {
            this._sqlDatabase = database;
            this._rootItem = new SqlDatabaseProjectItem(this);
        }

        protected override string GetIdentifierPart(string itemName)
        {
            string[] identifierParts = null;
            try
            {
                identifierParts = SqlHelper.GetIdentifierParts(itemName);
            }
            catch
            {
            }
            if (identifierParts != null)
            {
                return identifierParts[identifierParts.Length - 1];
            }
            return itemName;
        }

        protected override string GetProjectItemPathInternal(Microsoft.Matrix.Core.Projects.ProjectItem projectItem)
        {
            if (projectItem is DatabaseProjectItem)
            {
                return (this._sqlDatabase.DisplayName + "." + ((DatabaseProjectItem) projectItem).Caption);
            }
            if (projectItem is TableProjectItem)
            {
                return (this._sqlDatabase.DisplayName + "." + ((TableProjectItem) projectItem).Caption);
            }
            if (projectItem is StoredProcedureProjectItem)
            {
                return (this._sqlDatabase.DisplayName + "." + ((StoredProcedureProjectItem) projectItem).Caption);
            }
            return string.Empty;
        }

        public override bool ValidateProjectItemName(string name)
        {
            return SqlHelper.IsValidIdentifier(name);
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

