namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using System;

    internal class TableColumn
    {
        private string _column;
        private string _table;

        public TableColumn(string table, string column)
        {
            this._table = table;
            this._column = column;
        }

        public string Column
        {
            get
            {
                return this._column;
            }
        }

        public string Table
        {
            get
            {
                return this._table;
            }
        }
    }
}

