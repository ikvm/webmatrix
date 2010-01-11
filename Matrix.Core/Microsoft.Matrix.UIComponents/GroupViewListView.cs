namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class GroupViewListView : MxListView
    {
        private int _currentWidth;

        public GroupViewListView()
        {
            base.BorderStyle = BorderStyle.None;
            base.FlatScrollBars = true;
            base.ShowToolTips = true;
            base.FullRowSelect = true;
            base.HideSelection = false;
            base.LabelEdit = false;
            base.View = View.Details;
            base.HeaderStyle = ColumnHeaderStyle.None;
            base.MultiSelect = false;
            this.Dock = DockStyle.Fill;
            this.BackColor = SystemColors.Control;
            base.Columns.Add(new ColumnHeader());
            ImageList list = new ImageList();
            list.ImageSize = new Size(0x13, 0x16);
            list.Images.Add(new Bitmap(0x13, 0x16));
            base.SmallImageList = list;
            this._currentWidth = -1;
        }

        protected virtual void OnDrawItem(DrawItemEventArgs e)
        {
            int index = e.Index;
            if (index < base.Items.Count)
            {
                Rectangle bounds = e.Bounds;
                Graphics graphics = e.Graphics;
                GroupViewListViewItem item = (GroupViewListViewItem) base.Items[index];
                Image image = item.GroupViewItem.Image;
                string text = item.Text;
                bool flag = false;
                bool flag2 = false;
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    flag = true;
                }
                if ((e.State & DrawItemState.HotLight) == DrawItemState.HotLight)
                {
                    flag2 = true;
                }
                Brush brush = new SolidBrush(this.BackColor);
                graphics.FillRectangle(brush, bounds);
                brush.Dispose();
                Rectangle rect = new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 2, bounds.Height - 1);
                if (flag2 || flag)
                {
                    Brush brush2 = new SolidBrush(Color.FromArgb(50, SystemColors.Highlight));
                    Brush brush3 = new SolidBrush(Color.FromArgb(50, Color.White));
                    graphics.FillRectangle(brush2, rect);
                    graphics.FillRectangle(brush3, rect);
                    brush2.Dispose();
                    brush3.Dispose();
                }
                if (flag && base.ContainsFocus)
                {
                    Pen pen = new Pen(SystemColors.Highlight);
                    graphics.DrawRectangle(pen, rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
                    pen.Dispose();
                }
                if (image != null)
                {
                    Rectangle rectangle3 = new Rectangle(bounds.X + 3, bounds.Y + 3, 0x10, 0x10);
                    graphics.DrawImage(image, rectangle3);
                }
                StringFormat format = new StringFormat(StringFormatFlags.LineLimit | StringFormatFlags.NoWrap);
                format.Trimming = StringTrimming.EllipsisCharacter;
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Near;
                Rectangle layoutRectangle = new Rectangle(((bounds.X + 2) + 0x10) + 3, bounds.Y, bounds.Width - 0x15, bounds.Height);
                graphics.DrawString(text, this.Font, SystemBrushes.WindowText, layoutRectangle, format);
                format.Dispose();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.UpdateColumnWidth();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (base.IsHandleCreated)
            {
                this.UpdateColumnWidth();
            }
        }

        private void OnWmReflectDrawItem(ref Message m)
        {
            Interop.DRAWITEMSTRUCT lParam = (Interop.DRAWITEMSTRUCT) m.GetLParam(typeof(Interop.DRAWITEMSTRUCT));
            Graphics graphics = Graphics.FromHdc(lParam.hDC);
            DrawItemEventArgs e = new DrawItemEventArgs(graphics, this.Font, Rectangle.FromLTRB(lParam.rcItem.left, lParam.rcItem.top, lParam.rcItem.right, lParam.rcItem.bottom), lParam.itemID, (DrawItemState) lParam.itemState);
            this.OnDrawItem(e);
            graphics.Dispose();
            m.Result = (IntPtr) 1;
        }

        private void UpdateColumnWidth()
        {
            int width = base.ClientRectangle.Width;
            if (width != this._currentWidth)
            {
                this._currentWidth = width;
                base.Columns[0].Width = this._currentWidth;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x202b:
                    this.OnWmReflectDrawItem(ref m);
                    return;

                case 0x204e:
                {
                    Interop.NMHDR lParam = (Interop.NMHDR) m.GetLParam(typeof(Interop.NMHDR));
                    if (lParam.code == -12)
                    {
                        return;
                    }
                    break;
                }
            }
            base.WndProc(ref m);
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                createParams.Style |= 0x400;
                return createParams;
            }
        }
    }
}

