namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;

    public sealed class ElementRenderData : IDisposable
    {
        private bool _antiAliasedText;
        private Color _backColor;
        private Brush _backColorBrush;
        private System.Drawing.Font _font;
        private string _fontFamily;
        private IntPtr _fontHandle;
        private float _fontSize;
        private System.Drawing.FontStyle _fontStyle;
        private Color _foreColor;
        private Brush _foreColorBrush;
        private System.Drawing.Graphics _graphics;
        private IntPtr _graphicsHandle;
        private Color _hoverColor;
        private Brush _hoverColorBrush;
        private Color _linkColor;
        private Brush _linkColorBrush;
        private Point _scrollPosition;
        private bool _showFocus;
        private StringFormat _textFormat;

        internal ElementRenderData(System.Drawing.Graphics g, Point scrollPosition, bool showFocus, bool antiAliasedText)
        {
            this._graphics = g;
            this._scrollPosition = scrollPosition;
            this._showFocus = showFocus;
            this._antiAliasedText = antiAliasedText;
            this._textFormat = StringFormat.GenericTypographic;
            this._textFormat.FormatFlags |= StringFormatFlags.NoClip | StringFormatFlags.NoWrap | StringFormatFlags.MeasureTrailingSpaces;
        }

        public void Dispose()
        {
            this._textFormat.Dispose();
            this._textFormat = null;
            this.DisposeFont();
            this.DisposeBackColorBrush();
            this.DisposeForeColorBrush();
            if (this._hoverColorBrush != null)
            {
                this._hoverColorBrush.Dispose();
                this._hoverColorBrush = null;
            }
            if (this._linkColorBrush != null)
            {
                this._linkColorBrush.Dispose();
                this._linkColorBrush = null;
            }
            if (this._graphicsHandle != IntPtr.Zero)
            {
                this._graphics.ReleaseHdc(this._graphicsHandle);
                this._graphicsHandle = IntPtr.Zero;
            }
        }

        private void DisposeBackColorBrush()
        {
            if (this._backColorBrush != null)
            {
                this._backColorBrush.Dispose();
                this._backColorBrush = null;
            }
        }

        private void DisposeFont()
        {
            if (this._fontHandle != IntPtr.Zero)
            {
                Interop.DeleteObject(this._fontHandle);
                this._fontHandle = IntPtr.Zero;
            }
            if (this._font != null)
            {
                this._font.Dispose();
                this._font = null;
            }
        }

        private void DisposeForeColorBrush()
        {
            if (this._foreColorBrush != null)
            {
                this._foreColorBrush.Dispose();
                this._foreColorBrush = null;
            }
        }

        internal void SetBackColor(Color value)
        {
            if (this._backColor != value)
            {
                this._backColor = value;
                this.DisposeBackColorBrush();
            }
        }

        internal void SetFontFamily(string value)
        {
            if (!this.FontFamily.Equals(value))
            {
                this._fontFamily = value;
                this.DisposeFont();
            }
        }

        internal void SetFontSize(float value)
        {
            if (this._fontSize != value)
            {
                this._fontSize = value;
                this.DisposeFont();
            }
        }

        internal void SetFontStyle(System.Drawing.FontStyle value)
        {
            if (this._fontStyle != value)
            {
                this._fontStyle = value;
                this.DisposeFont();
            }
        }

        internal void SetForeColor(Color value)
        {
            if (this._foreColor != value)
            {
                this._foreColor = value;
                this.DisposeForeColorBrush();
            }
        }

        internal void SetHoverColor(Color value)
        {
            this._hoverColor = value;
        }

        internal void SetLinkColor(Color value)
        {
            this._linkColor = value;
        }

        public bool AntiAliasedText
        {
            get
            {
                return this._antiAliasedText;
            }
        }

        public Color BackColor
        {
            get
            {
                return this._backColor;
            }
        }

        public Brush BackColorBrush
        {
            get
            {
                if ((this._backColorBrush == null) && (this._backColor != Color.Transparent))
                {
                    this._backColorBrush = new SolidBrush(this._backColor);
                }
                return this._backColorBrush;
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                if (this._font == null)
                {
                    this._font = new System.Drawing.Font(this._fontFamily, this._fontSize, this._fontStyle);
                }
                return this._font;
            }
        }

        public string FontFamily
        {
            get
            {
                if (this._fontFamily == null)
                {
                    return string.Empty;
                }
                return this._fontFamily;
            }
        }

        public IntPtr FontHandle
        {
            get
            {
                if (this._fontHandle == IntPtr.Zero)
                {
                    this._fontHandle = this.Font.ToHfont();
                }
                return this._fontHandle;
            }
        }

        public float FontSize
        {
            get
            {
                return this._fontSize;
            }
        }

        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                return this._fontStyle;
            }
        }

        public Color ForeColor
        {
            get
            {
                return this._foreColor;
            }
        }

        public Brush ForeColorBrush
        {
            get
            {
                if (this._foreColorBrush == null)
                {
                    this._foreColorBrush = new SolidBrush(this._foreColor);
                }
                return this._foreColorBrush;
            }
        }

        public System.Drawing.Graphics Graphics
        {
            get
            {
                if (this._graphicsHandle != IntPtr.Zero)
                {
                    this._graphics.ReleaseHdc(this._graphicsHandle);
                    this._graphicsHandle = IntPtr.Zero;
                }
                return this._graphics;
            }
        }

        public IntPtr GraphicsHandle
        {
            get
            {
                if (this._graphicsHandle == IntPtr.Zero)
                {
                    this._graphicsHandle = this._graphics.GetHdc();
                }
                return this._graphicsHandle;
            }
        }

        public Color HoverColor
        {
            get
            {
                return this._hoverColor;
            }
        }

        public Brush HoverColorBrush
        {
            get
            {
                if (this._hoverColorBrush == null)
                {
                    this._hoverColorBrush = new SolidBrush(this._hoverColor);
                }
                return this._hoverColorBrush;
            }
        }

        public Color LinkColor
        {
            get
            {
                return this._linkColor;
            }
        }

        public Brush LinkColorBrush
        {
            get
            {
                if (this._linkColorBrush == null)
                {
                    this._linkColorBrush = new SolidBrush(this._linkColor);
                }
                return this._linkColorBrush;
            }
        }

        public Point ScrollPosition
        {
            get
            {
                return this._scrollPosition;
            }
        }

        public bool ShowFocus
        {
            get
            {
                return this._showFocus;
            }
        }

        public StringFormat TextFormat
        {
            get
            {
                return this._textFormat;
            }
        }
    }
}

