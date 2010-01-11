namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class MxInfoLabel : Control
    {
        private System.Drawing.Image _image;
        private int _outerPadding = 4;
        private int _padding = 4;
        public static readonly Bitmap ErrorGlyph = new Bitmap(typeof(MxInfoLabel), "ErrorGlyph.bmp");
        public static readonly Bitmap InfoGlyph;
        public static readonly Bitmap WarningGlyph;

        static MxInfoLabel()
        {
            ErrorGlyph.MakeTransparent(Color.Fuchsia);
            InfoGlyph = new Bitmap(typeof(MxInfoLabel), "InfoGlyph.bmp");
            InfoGlyph.MakeTransparent(Color.Fuchsia);
            WarningGlyph = new Bitmap(typeof(MxInfoLabel), "WarningGlyph.bmp");
            WarningGlyph.MakeTransparent(Color.Fuchsia);
        }

        public MxInfoLabel()
        {
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.Opaque, true);
            base.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            this.RecalculateSize();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int num;
            StringFormat format = new StringFormat(StringFormatFlags.FitBlackBox);
            e.Graphics.Clear(SystemColors.Control);
            e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark), this._outerPadding, this._outerPadding, (base.Width - (2 * this._outerPadding)) - 1, (base.Height - (2 * this._outerPadding)) - 1);
            e.Graphics.FillRectangle(new SolidBrush(SystemColors.Info), (int) (this._outerPadding + 1), (int) (this._outerPadding + 1), (int) ((base.Width - (2 * this._outerPadding)) - 2), (int) ((base.Height - (2 * this._outerPadding)) - 2));
            if (this._image != null)
            {
                e.Graphics.DrawImage(this._image, (int) ((this._padding + this._outerPadding) + 1), (int) ((this._padding + this._outerPadding) + 1));
                num = (((2 * this._padding) + this._outerPadding) + this._image.Width) + 1;
            }
            else
            {
                num = (this._padding + this._outerPadding) + 1;
            }
            int num2 = (this._padding + this._outerPadding) + 1;
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(SystemColors.InfoText), new RectangleF((float) num, (float) ((this._padding + this._outerPadding) + 1), (float) ((base.Width - num) - num2), (float) (((base.Height - (2 * this._padding)) - (2 * this._outerPadding)) - 2)), format);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.RecalculateSize();
        }

        private void RecalculateSize()
        {
            if ((this.Dock == DockStyle.Top) || (this.Dock == DockStyle.Bottom))
            {
                int num;
                if (this._image != null)
                {
                    num = (((2 * this._padding) + this._outerPadding) + this._image.Width) + 1;
                }
                else
                {
                    num = (this._padding + this._outerPadding) + 1;
                }
                int num2 = (this._padding + this._outerPadding) + 1;
                SizeF ef = base.CreateGraphics().MeasureString(this.Text, this.Font, (int) ((base.Width - num) - num2));
                int num3 = ((2 * this._padding) + (2 * this._outerPadding)) + 2;
                int num4 = ((int) ef.Height) + num3;
                if ((this._image != null) && (num4 < (this._image.Height + num3)))
                {
                    num4 = this._image.Height + num3;
                }
                base.Height = num4;
            }
            base.Invalidate();
        }

        [Category("Appearance")]
        public System.Drawing.Image Image
        {
            get
            {
                return this._image;
            }
            set
            {
                this._image = value;
                this.RecalculateSize();
            }
        }

        [DefaultValue(4), Category("Appearance")]
        public int OuterPadding
        {
            get
            {
                return this._outerPadding;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "OuterPadding must be greater than or equal to zero.");
                }
                this._outerPadding = value;
                this.RecalculateSize();
            }
        }

        [Category("Appearance"), DefaultValue(4)]
        public int Padding
        {
            get
            {
                return this._padding;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Padding must be greater than or equal to zero.");
                }
                this._padding = value;
                this.RecalculateSize();
            }
        }
    }
}

