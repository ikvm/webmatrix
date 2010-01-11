namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GroupViewPanel : Panel
    {
        private int _imageIndex;
        private ImageList _imageList;
        private int _tabHeight = -1;
        private const int IconSize = 0x10;
        private const int ImagePadding = 1;
        private const int SectionPadding = 2;

        public GroupViewPanel()
        {
            base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.Opaque, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle clientRectangle = base.ClientRectangle;
            Brush brush = new SolidBrush(Color.FromArgb(0x80, SystemColors.ControlDark));
            Rectangle tabRectangle = this.TabRectangle;
            Rectangle rect = new Rectangle(clientRectangle.Location, new Size(clientRectangle.Width - 1, clientRectangle.Height - 1));
            Rectangle layoutRectangle = Rectangle.Inflate(tabRectangle, -2, -2);
            g.FillRectangle(SystemBrushes.Control, clientRectangle);
            g.FillRectangle(brush, tabRectangle);
            g.DrawRectangle(SystemPens.ControlDark, tabRectangle);
            g.DrawRectangle(SystemPens.ControlDark, rect);
            if (this._imageList != null)
            {
                if ((this.ImageIndex != -1) && (this._imageList.Images.Count > this.ImageIndex))
                {
                    int x = (base.ClientRectangle.Left + 2) + 1;
                    int y = (this.TabHeight - 0x10) / 2;
                    this._imageList.Draw(g, x, y, this.ImageIndex);
                }
                int num3 = 20;
                layoutRectangle.X += num3;
                layoutRectangle.Width -= num3;
            }
            StringFormat format = new StringFormat(StringFormatFlags.NoWrap);
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Center;
            format.Trimming = StringTrimming.EllipsisCharacter;
            g.DrawString(this.Text, this.Font, SystemBrushes.ControlText, layoutRectangle, format);
            format.Dispose();
        }

        [Browsable(true)]
        public int ImageIndex
        {
            get
            {
                return this._imageIndex;
            }
            set
            {
                this._imageIndex = value;
                base.Invalidate(this.TabRectangle, false);
            }
        }

        [Browsable(true)]
        public ImageList Images
        {
            get
            {
                return this._imageList;
            }
            set
            {
                this._imageList = value;
                base.Invalidate(this.TabRectangle, false);
            }
        }

        public int TabHeight
        {
            get
            {
                if (this._tabHeight == -1)
                {
                    this._tabHeight = Math.Max(0x10, this.Font.Height) + 6;
                }
                return this._tabHeight;
            }
        }

        private Rectangle TabRectangle
        {
            get
            {
                Rectangle clientRectangle = base.ClientRectangle;
                return new Rectangle(clientRectangle.Left, clientRectangle.Top, clientRectangle.Width - 1, this.TabHeight);
            }
        }

        [Browsable(true)]
        public string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
    }
}

