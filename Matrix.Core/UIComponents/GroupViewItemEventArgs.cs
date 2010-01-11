namespace Microsoft.Matrix.UIComponents
{
    using System;

    public class GroupViewItemEventArgs : EventArgs
    {
        private GroupViewItem _item;

        public GroupViewItemEventArgs(GroupViewItem item)
        {
            this._item = item;
        }

        public GroupViewItem Item
        {
            get
            {
                return this._item;
            }
        }
    }
}

