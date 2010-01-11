namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;

    internal class ColumnListItem
    {
        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column _column;

        public ColumnListItem(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column column)
        {
            this._column = column;
        }

        public override string ToString()
        {
            return this._column.Name;
        }

        public Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column Column
        {
            get
            {
                return this._column;
            }
        }
    }
}

