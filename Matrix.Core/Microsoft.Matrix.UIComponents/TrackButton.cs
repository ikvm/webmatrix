namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    public sealed class TrackButton : Control
    {
        private bool _checked;
        private Image _disabledImage;
        private Image _enabledImage;
        private System.Drawing.Imaging.ImageAttributes _imageAttributes;
        private MouseButtons _mouseButton;
        private bool _mouseCapture;
        private bool _mouseOver;

        public TrackButton()
        {
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.StandardDoubleClick, false);
            base.SetStyle(ControlStyles.Selectable, false);
            this._mouseButton = MouseButtons.None;
            base.TabStop = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!this._mouseCapture)
            {
                this._mouseOver = true;
                this._mouseCapture = true;
                this._mouseButton = e.Button;
                base.Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this._mouseOver = true;
            base.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this._mouseOver = false;
            base.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool flag = base.ClientRectangle.Contains(new Point(e.X, e.Y));
            if (flag != this._mouseOver)
            {
                this._mouseOver = flag;
                base.Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == this._mouseButton)
            {
                this._mouseOver = false;
                this._mouseCapture = false;
                base.Invalidate();
            }
            else
            {
                base.Capture = true;
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this._enabledImage != null)
            {
                if (base.Enabled)
                {
                    ButtonBorderStyle solid;
                    if (this._imageAttributes == null)
                    {
                        Point point;
                        if ((this._mouseOver && this._mouseCapture) && !this._checked)
                        {
                            point = new Point(2, 2);
                        }
                        else
                        {
                            point = new Point(1, 1);
                        }
                        e.Graphics.DrawImage(this._enabledImage, point);
                    }
                    else
                    {
                        Point[] destPoints = new Point[3];
                        destPoints[0].X = ((this._mouseOver && this._mouseCapture) && !this._checked) ? 2 : 1;
                        destPoints[0].Y = ((this._mouseOver && this._mouseCapture) && !this._checked) ? 2 : 1;
                        destPoints[1].X = destPoints[0].X + this._enabledImage.Width;
                        destPoints[1].Y = destPoints[0].Y;
                        destPoints[2].X = destPoints[0].X;
                        destPoints[2].Y = destPoints[1].Y + this._enabledImage.Height;
                        e.Graphics.DrawImage(this._enabledImage, destPoints, new Rectangle(0, 0, this._enabledImage.Width, this._enabledImage.Height), GraphicsUnit.Pixel, this._imageAttributes);
                    }
                    Color backColor = this.BackColor;
                    if (this._checked)
                    {
                        solid = ButtonBorderStyle.Solid;
                        backColor = SystemColors.Highlight;
                    }
                    else if (this._mouseOver)
                    {
                        solid = this._mouseCapture ? ButtonBorderStyle.Inset : ButtonBorderStyle.Outset;
                    }
                    else
                    {
                        solid = ButtonBorderStyle.Solid;
                    }
                    ControlPaint.DrawBorder(e.Graphics, base.ClientRectangle, backColor, 1, solid, backColor, 1, solid, backColor, 1, solid, backColor, 1, solid);
                }
                else if (this._disabledImage != null)
                {
                    if (this._imageAttributes == null)
                    {
                        e.Graphics.DrawImage(this._disabledImage, new Point(1, 1));
                    }
                    else
                    {
                        Point[] pointArray2 = new Point[3];
                        pointArray2[0].X = 1;
                        pointArray2[0].Y = 1;
                        pointArray2[1].X = pointArray2[0].X + this._disabledImage.Width;
                        pointArray2[1].Y = pointArray2[0].Y;
                        pointArray2[2].X = pointArray2[0].X;
                        pointArray2[2].Y = pointArray2[1].Y + this._disabledImage.Height;
                        e.Graphics.DrawImage(this._disabledImage, pointArray2, new Rectangle(0, 0, this._disabledImage.Width, this._disabledImage.Height), GraphicsUnit.Pixel, this._imageAttributes);
                    }
                }
                else
                {
                    ControlPaint.DrawImageDisabled(e.Graphics, this._enabledImage, 1, 1, this.BackColor);
                }
            }
        }

        public bool Checked
        {
            get
            {
                return this._checked;
            }
            set
            {
                if (this._checked != value)
                {
                    this._checked = value;
                    if (base.IsHandleCreated)
                    {
                        base.Invalidate();
                    }
                }
            }
        }

        public Image DisabledImage
        {
            get
            {
                return this._disabledImage;
            }
            set
            {
                if (this._disabledImage != value)
                {
                    this._disabledImage = value;
                    if (base.IsHandleCreated)
                    {
                        base.Invalidate();
                    }
                }
            }
        }

        public Image EnabledImage
        {
            get
            {
                return this._enabledImage;
            }
            set
            {
                if (this._enabledImage != value)
                {
                    this._enabledImage = value;
                    if (base.IsHandleCreated)
                    {
                        base.Invalidate();
                    }
                }
            }
        }

        internal System.Drawing.Imaging.ImageAttributes ImageAttributes
        {
            get
            {
                return this._imageAttributes;
            }
            set
            {
                if (this._imageAttributes != value)
                {
                    this._imageAttributes = value;
                    if (base.IsHandleCreated)
                    {
                        base.Invalidate();
                    }
                }
            }
        }
    }
}

