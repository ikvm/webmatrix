namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using System;
    using System.Data;

    internal class FilterInfo
    {
        private int _count;
        private string _filterValue;
        private DbType _type;

        public FilterInfo(string filterValue, DbType type)
        {
            this._filterValue = filterValue;
            this._type = type;
            this._count = 0;
        }

        public int Count
        {
            get
            {
                return this._count;
            }
            set
            {
                this._count = value;
            }
        }

        public string FilterValue
        {
            get
            {
                return this._filterValue;
            }
        }

        public DbType Type
        {
            get
            {
                return this._type;
            }
        }
    }
}

