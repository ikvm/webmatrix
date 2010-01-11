namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;

    public sealed class MxDataGridSortCommandEventArgs : EventArgs
    {
        private object commandSource;
        private string sortExpression;

        public MxDataGridSortCommandEventArgs(object commandSource, MxDataGridCommandEventArgs dce)
        {
            this.commandSource = commandSource;
            this.sortExpression = (string) dce.CommandArgument;
        }

        public object CommandSource
        {
            get
            {
                return this.commandSource;
            }
        }

        public string SortExpression
        {
            get
            {
                return this.sortExpression;
            }
        }
    }
}

