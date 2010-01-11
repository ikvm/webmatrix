namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Drawing;

    public class HyperLinkElement : TextElement
    {
        private bool _active;
        private Color _hoverColor;
        private Color _linkColor;

        public HyperLinkElement() : this(null)
        {
        }

        public HyperLinkElement(string text) : base(text)
        {
            this.FontStyle = FontStyle.Underline;
        }

        public override void BeginTracking()
        {
            base.BeginTracking();
            this._active = true;
        }

        public override void EndTracking()
        {
            this._active = false;
            base.EndTracking();
        }

        protected internal override FontStyle GetFontStyle()
        {
            FontStyle fontStyle = base.GetFontStyle();
            if (this._active)
            {
                fontStyle |= FontStyle.Underline;
            }
            return fontStyle;
        }

        protected override void RenderForeground(ElementRenderData renderData)
        {
            if (base.Text.Length != 0)
            {
                bool flag = this._active && !this._hoverColor.IsEmpty;
                bool flag2 = !this._active && !this._linkColor.IsEmpty;
                Color empty = Color.Empty;
                if (flag)
                {
                    empty = renderData.HoverColor;
                    renderData.SetHoverColor(this._hoverColor);
                }
                else if (flag2)
                {
                    empty = renderData.LinkColor;
                    renderData.SetLinkColor(this._linkColor);
                }
                try
                {
                    if (renderData.AntiAliasedText)
                    {
                        Brush textBrush = null;
                        if (this._active)
                        {
                            textBrush = renderData.HoverColorBrush;
                        }
                        else if (this.ForeColor.IsEmpty)
                        {
                            textBrush = renderData.LinkColorBrush;
                        }
                        else
                        {
                            textBrush = renderData.ForeColorBrush;
                        }
                        base.RenderText(renderData, textBrush);
                    }
                    else
                    {
                        Color linkColor = renderData.LinkColor;
                        if (this._active)
                        {
                            linkColor = renderData.HoverColor;
                        }
                        else if (!this.ForeColor.IsEmpty)
                        {
                            linkColor = this.ForeColor;
                        }
                        base.RenderText(renderData, linkColor);
                    }
                }
                finally
                {
                    if (flag)
                    {
                        renderData.SetHoverColor(empty);
                    }
                    else if (flag2)
                    {
                        renderData.SetLinkColor(empty);
                    }
                }
            }
        }

        public override bool Clickable
        {
            get
            {
                return true;
            }
        }

        public Color HoverColor
        {
            get
            {
                return this._hoverColor;
            }
            set
            {
                if (this._hoverColor != value)
                {
                    this._hoverColor = value;
                    if (this._active)
                    {
                        this.OnChanged(false);
                    }
                }
            }
        }

        public Color LinkColor
        {
            get
            {
                return this._linkColor;
            }
            set
            {
                if (this._linkColor != value)
                {
                    this._linkColor = value;
                    if (!this._active)
                    {
                        this.OnChanged(false);
                    }
                }
            }
        }

        public override bool RequiresTracking
        {
            get
            {
                return true;
            }
        }
    }
}

