namespace Microsoft.Matrix.Framework.Web.UI.Design
{
    using System;

    internal class DataTableSchema
    {
        private DataColumns _tableColumns;
        private string _tableName;

        public DataColumns Columns
        {
            get
            {
                if (this._tableColumns == null)
                {
                    this._tableColumns = new DataColumns();
                }
                return this._tableColumns;
            }
        }

        public string TableName
        {
            get
            {
                return this._tableName;
            }
            set
            {
                this._tableName = value;
            }
        }
    }
}

