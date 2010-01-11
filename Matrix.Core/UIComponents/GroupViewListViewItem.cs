namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Windows.Forms;

    public sealed class GroupViewListViewItem : ListViewItem
    {
        private Microsoft.Matrix.UIComponents.GroupViewItem _item;

        public GroupViewListViewItem(Microsoft.Matrix.UIComponents.GroupViewItem item) : base(item.Text, 0)
        {
            this._item = item;
        }

        public Microsoft.Matrix.UIComponents.GroupViewItem GroupViewItem
        {
            get
            {
                return this._item;
            }
        }
    }
}

