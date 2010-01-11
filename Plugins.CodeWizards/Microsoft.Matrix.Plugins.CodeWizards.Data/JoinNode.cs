namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using System;

    internal class JoinNode : ClauseNode
    {
        private string _leftColumn;
        private string _leftTable;
        private string _op;
        private string _rightColumn;
        private string _rightTable;

        public JoinNode(string lTable, string lCol, string op, string rTable, string rCol)
        {
            this._leftTable = lTable;
            this._leftColumn = lCol;
            this._op = op;
            this._rightTable = rTable;
            this._rightColumn = rCol;
            base.Text = "[" + this._leftTable + "].[" + this._leftColumn + "] " + this._op + " [" + this._rightTable + "].[" + this._rightColumn + "]";
        }

        public override string[] Tables
        {
            get
            {
                return new string[] { this._leftTable, this._rightTable };
            }
        }
    }
}

