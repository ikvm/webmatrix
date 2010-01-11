namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;

    public class TextElement : VisualElement
    {
        private bool _preserveLeadingSpace;
        private string _text;
        private bool _whitespace;

        public TextElement() : this(null)
        {
        }

        public TextElement(string text)
        {
            this._whitespace = true;
            this.Text = text;
        }

        protected override Size Measure(ElementRenderData renderData)
        {
            string text = this.Text;
            Size empty = Size.Empty;
            if (text.Length != 0)
            {
                if (renderData.AntiAliasedText)
                {
                    return renderData.Graphics.MeasureString(this.Text, renderData.Font, new PointF(0f, 0f), renderData.TextFormat).ToSize();
                }
                IntPtr graphicsHandle = renderData.GraphicsHandle;
                IntPtr hObject = Interop.SelectObject(graphicsHandle, renderData.FontHandle);
                try
                {
                    Interop.SIZE size = new Interop.SIZE();
                    Interop.GetTextExtentPoint32W(graphicsHandle, text, text.Length, ref size);
                    empty = new Size(size.x, size.y);
                }
                finally
                {
                    Interop.SelectObject(graphicsHandle, hObject);
                }
            }
            return empty;
        }

        protected override void RenderForeground(ElementRenderData renderData)
        {
            if (this.Text.Length != 0)
            {
                if (renderData.AntiAliasedText)
                {
                    this.RenderText(renderData, renderData.ForeColorBrush);
                }
                else
                {
                    this.RenderText(renderData, renderData.ForeColor);
                }
            }
        }

        protected void RenderText(ElementRenderData renderData, Brush textBrush)
        {
            renderData.Graphics.DrawString(this.Text, renderData.Font, textBrush, (PointF) this.ContentBounds.Location, renderData.TextFormat);
        }

        protected void RenderText(ElementRenderData renderData, Color textColor)
        {
            IntPtr graphicsHandle = renderData.GraphicsHandle;
            IntPtr hObject = Interop.SelectObject(graphicsHandle, renderData.FontHandle);
            int nBkMode = Interop.SetBkMode(graphicsHandle, 1);
            int color = Interop.SetTextColor(graphicsHandle, ColorTranslator.ToWin32(textColor));
            try
            {
                Point location = this.ContentBounds.Location;
                location.Offset(renderData.ScrollPosition.X, renderData.ScrollPosition.Y);
                Interop.RECT lpRect = new Interop.RECT(0, 0, 0, 0);
                Interop.ExtTextOut(graphicsHandle, location.X, location.Y, 0, ref lpRect, this.Text, this.Text.Length, null);
            }
            finally
            {
                Interop.SelectObject(graphicsHandle, hObject);
                Interop.SetBkMode(graphicsHandle, nBkMode);
                Interop.SetTextColor(graphicsHandle, color);
            }
        }

        public override bool IsEmpty
        {
            get
            {
                return (!this._preserveLeadingSpace && this._whitespace);
            }
        }

        public bool PreserveLeadingSpace
        {
            get
            {
                return this._preserveLeadingSpace;
            }
            set
            {
                if (this._preserveLeadingSpace != value)
                {
                    this._preserveLeadingSpace = value;
                    if (this._whitespace)
                    {
                        this.OnChanged(true);
                    }
                }
            }
        }

        public string Text
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
                if (!this.Text.Equals(value))
                {
                    this._text = value;
                    this._whitespace = false;
                    if ((value == null) || (value.Trim().Length == 0))
                    {
                        this._whitespace = true;
                    }
                    this.OnChanged(true);
                }
            }
        }
    }
}

