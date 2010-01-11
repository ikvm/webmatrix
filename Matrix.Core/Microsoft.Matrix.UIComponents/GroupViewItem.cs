namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class GroupViewItem
    {
        private System.Drawing.Image _image;
        private GroupViewListViewItem _listViewItem;
        private string _text;

        public GroupViewItem() : this(null, null)
        {
        }

        public GroupViewItem(string text, System.Drawing.Image image)
        {
            this._text = text;
            this._image = image;
        }

        public virtual DataObject GetDragDropDataObject()
        {
            return null;
        }

        public virtual System.Drawing.Image Image
        {
            get
            {
                return this._image;
            }
            set
            {
                this._image = value;
            }
        }

        internal GroupViewListViewItem ListViewItem
        {
            get
            {
                return this._listViewItem;
            }
            set
            {
                this._listViewItem = value;
            }
        }

        public virtual string Text
        {
            get
            {
                if (this._text == null)
                {
                    return string.Empty;
                }
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }
    }
}

