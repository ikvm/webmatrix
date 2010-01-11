namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;

    internal class TableListItem
    {
        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table _table;

        public TableListItem(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table table)
        {
            this._table = table;
        }

        public override string ToString()
        {
            return this._table.Name;
        }

        public Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table Table
        {
            get
            {
                return this._table;
            }
        }
    }
}

