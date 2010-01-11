namespace Microsoft.Matrix.Framework.Web.UI.Design
{
    using System;

    internal class DataSourceSchema
    {
        private DataTables _tables;

        public DataTables Tables
        {
            get
            {
                if (this._tables == null)
                {
                    this._tables = new DataTables();
                }
                return this._tables;
            }
        }
    }
}

