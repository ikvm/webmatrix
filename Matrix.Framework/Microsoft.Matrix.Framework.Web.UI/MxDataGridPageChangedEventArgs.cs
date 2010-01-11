namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;

    public sealed class MxDataGridPageChangedEventArgs : EventArgs
    {
        private object commandSource;
        private int newPageIndex;

        public MxDataGridPageChangedEventArgs(object commandSource, int newPageIndex)
        {
            this.commandSource = commandSource;
            this.newPageIndex = newPageIndex;
        }

        public object CommandSource
        {
            get
            {
                return this.commandSource;
            }
        }

        public int NewPageIndex
        {
            get
            {
                return this.newPageIndex;
            }
        }
    }
}

