namespace Microsoft.Matrix.Framework.Web.UI.Design
{
    using System;

    internal class DataColumnSchema
    {
        private string _columnName;
        private Type _columnType;

        public string ColumnName
        {
            get
            {
                return this._columnName;
            }
            set
            {
                this._columnName = value;
            }
        }

        public Type ColumnType
        {
            get
            {
                return this._columnType;
            }
            set
            {
                this._columnType = value;
            }
        }
    }
}

