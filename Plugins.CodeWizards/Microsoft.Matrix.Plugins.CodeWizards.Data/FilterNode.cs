namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using System;
    using System.Data;

    internal class FilterNode : ClauseNode
    {
        private string _leftColumn;
        private string _leftTable;
        private string _op;
        private DbType _type;
        private string _value;

        public FilterNode(string lTable, string lCol, string op, string val, DbType type)
        {
            this._leftTable = lTable;
            this._leftColumn = lCol;
            this._op = op;
            this._value = val;
            this._type = type;
            base.Text = "[" + this._leftTable + "].[" + this._leftColumn + "] " + this._op + " " + this._value;
        }

        public string FilterValue
        {
            get
            {
                return this._value;
            }
        }

        public string LeftOperand
        {
            get
            {
                return ("[" + this._leftTable + "].[" + this._leftColumn + "]");
            }
        }

        public string Operator
        {
            get
            {
                return this._op;
            }
        }

        public override string[] Tables
        {
            get
            {
                return new string[] { this._leftTable };
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

