namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;

    public class MxDataGridItemEventArgs : EventArgs
    {
        private MxDataGridItem item;

        public MxDataGridItemEventArgs(MxDataGridItem item)
        {
            this.item = item;
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

