namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class GradientBand : Control
    {
        private ColorBlend _colorBlend;
        private Color[] _colors;
        private Color _endColor;
        private int _gradientSpeed;
        private Color[] _internalColors;
        private float[] _internalPositions;
        private int _offset;
        private int _scrollSpeed;
        private Color _startColor;
        private Timer _timer;

        public GradientBand()
        {
            base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.Opaque | ControlStyles.UserPaint, true);
            this._gradientSpeed = 5;
            this._scrollSpeed = 5;
        }

        private void CalculateInternalColors()
        {
            this._internalColors = new Color[(this.Colors.Length * this._gradientSpeed) * 3];
            this._internalPositions = new float[this._internalColors.Length];
            for (int i = 0; i < this._internalPositions.Length; i++)
            {
                this._internalPositions[i] = ((float) i) / ((float) this._internalPositions.Length);
            }
            this._internalPositions[this._internalPositions.Length - 1] = 1f;
            for (int j = 0; j < this.Colors.Length; j++)
            {
                for (int k = 0; k < (this._gradientSpeed * 3); k++)
                {
                    this._internalColors[j + (k * this.Colors.Length)] = this.Colors[j];
                }
            }
            this._colorBlend = new ColorBlend(this._internalPositions.Length);
            this._colorBlend.Colors = this._internalColors;
            this._colorBlend.Positions = this._internalPositions;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this._timer != null))
            {
                this._timer.Enabled = false;
                this._timer.Dispose();
                this._timer = null;
            }
            base.Dispose(disposing);
        }

        protected void OnColorsChanged()
        {
            this._colors = null;
            this.CalculateInternalColors();
            if (base.IsHandleCreated && base.Visible)
            {
                base.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Control topLevelControl = base.TopLevelControl;
            if ((topLevelControl != null) && topLevelControl.ContainsFocus)
            {
                Rectangle clipRectangle = e.ClipRectangle;
                clipRectangle.X -= (base.Width / this._gradientSpeed) - this._offset;
                clipRectangle.Width *= 3;
                try
                {
                    LinearGradientBrush brush = new LinearGradientBrush(clipRectangle, this.Colors[0], this.Colors[this.Colors.Length - 1], LinearGradientMode.Horizontal);
                    brush.InterpolationColors = this._colorBlend;
                    brush.WrapMode = WrapMode.TileFlipX;
                    e.Graphics.FillRectangle(brush, clipRectangle);
                    brush.Dispose();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                e.Graphics.FillRectangle(SystemBrushes.Control, e.ClipRectangle);
            }
            if (base.DesignMode)
            {
                Rectangle rect = new Rectangle(0, 0, base.ClientRectangle.Width - 1, base.ClientRectangle.Height - 1);
                e.Graphics.DrawRectangle(SystemPens.ControlDark, rect);
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            this._offset += this._scrollSpeed;
            if (this._offset >= (base.Width / this._gradientSpeed))
            {
                this._offset -= base.Width / this._gradientSpeed;
            }
            base.Invalidate();
        }

        public void Start()
        {
            base.Invalidate();
            Application.DoEvents();
            this._timer = new Timer();
            this._timer.Interval = 0x19;
            this._timer.Tick += new EventHandler(this.OnTimerTick);
            this._timer.Enabled = true;
        }

        public void Stop()
        {
            if (this._timer != null)
            {
                this._timer.Enabled = false;
                this._timer.Dispose();
                this._timer = null;
            }
            base.Invalidate();
            Application.DoEvents();
        }

        protected virtual Color[] Colors
        {
            get
            {
                if (this._colors == null)
                {
                    Color backColor = this._startColor;
                    if (backColor.IsEmpty)
                    {
                        backColor = this.BackColor;
                    }
                    Color color2 = this._endColor;
                    if (color2.IsEmpty)
                    {
                        color2 = this.BackColor;
                    }
                    this._colors = new Color[] { backColor, color2 };
                }
                return this._colors;
            }
        }

        public Color EndColor
        {
            get
            {
                return this._endColor;
            }
            set
            {
                this._endColor = value;
                this.OnColorsChanged();
            }
        }

        public int GradientSpeed
        {
            get
            {
                return this._gradientSpeed;
            }
            set
            {
                this._gradientSpeed = value;
                this.OnColorsChanged();
            }
        }

        public int ScrollSpeed
        {
            get
            {
                return this._scrollSpeed;
            }
            set
            {
                this._scrollSpeed = value;
            }
        }

        public Color StartColor
        {
            get
            {
                return this._startColor;
            }
            set
            {
                this._startColor = value;
                this.OnColorsChanged();
            }
        }
    }
}

