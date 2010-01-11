namespace Microsoft.Matrix.Packages.DBAdmin.Projects
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;

    public sealed class TableProjectItem : DocumentProjectItem
    {
        private string _name;

        public TableProjectItem(string tableName) : base(tableName)
        {
            this._name = tableName;
        }

        public Table GetTable()
        {
            DatabaseProject project = (DatabaseProject) this.Project;
            return project.Database.Tables[this._name];
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

