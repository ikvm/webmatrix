namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;
    using System.Web.UI.WebControls;

    public sealed class MxDataGridCommandEventArgs : CommandEventArgs
    {
        private object commandSource;
        private MxDataGridItem item;

        public MxDataGridCommandEventArgs(MxDataGridItem item, object commandSource, CommandEventArgs originalArgs) : base(originalArgs)
        {
            this.item = item;
            this.commandSource = commandSource;
        }

        public object CommandSource
        {
            get
            {
                return this.commandSource;
            }
        }

        public MxDataGridItem Item
        {
            get
            {
                return this.item;
            }
        }
    }
}

