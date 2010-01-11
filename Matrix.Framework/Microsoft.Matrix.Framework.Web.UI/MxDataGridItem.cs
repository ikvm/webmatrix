namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class MxDataGridItem : TableRow, INamingContainer
    {
        private object dataItem;
        private int dataSetIndex;
        private int itemIndex;
        private ListItemType itemType;

        public MxDataGridItem(int itemIndex, int dataSetIndex, ListItemType itemType)
        {
            this.itemIndex = itemIndex;
            this.dataSetIndex = dataSetIndex;
            this.itemType = itemType;
        }

        protected override bool OnBubbleEvent(object source, EventArgs e)
        {
            if (e is CommandEventArgs)
            {
                MxDataGridCommandEventArgs args = new MxDataGridCommandEventArgs(this, source, (CommandEventArgs) e);
                base.RaiseBubbleEvent(this, args);
                return true;
            }
            return false;
        }

        protected internal virtual void SetItemType(ListItemType itemType)
        {
            this.itemType = itemType;
        }

        public virtual object DataItem
        {
            get
            {
                return this.dataItem;
            }
            set
            {
                this.dataItem = value;
            }
        }

        public virtual int DataSetIndex
        {
            get
            {
                return this.dataSetIndex;
            }
        }

        public virtual int ItemIndex
        {
            get
            {
                return this.itemIndex;
            }
        }

        public virtual ListItemType ItemType
        {
            get
            {
                return this.itemType;
            }
        }
    }
}

