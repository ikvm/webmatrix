namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Drawing;

    public abstract class BlockElement : VisualElement
    {
        private Color _borderColor;
        private int _borderWidth;
        private BoxEdges _padding;

        protected override Size Measure(ElementRenderData renderData)
        {
            Size size = this.MeasureContents(renderData);
            return new Size(((size.Width + this.Padding.Left) + this.Padding.Right) + (2 * this.BorderWidth), ((size.Height + this.Padding.Top) + this.Padding.Bottom) + (2 * this.BorderWidth));
        }

        protected abstract Size MeasureContents(ElementRenderData renderData);
        protected abstract void RenderContents(ElementRenderData renderData);
        protected override void RenderForeground(ElementRenderData renderData)
        {
            if (this.BorderWidth != 0)
            {
                Pen pen = new Pen(this.BorderColor, (float) this.BorderWidth);
                renderData.Graphics.DrawRectangle(pen, base.ContentBounds);
                pen.Dispose();
            }
            this.RenderContents(renderData);
        }

        public Color BorderColor
        {
            get
            {
                return this._borderColor;
            }
            set
            {
                if (this._borderColor != value)
                {
                    this._borderColor = value;
                    this.OnChanged(false);
                }
            }
        }

        public int BorderWidth
        {
            get
            {
                return this._borderWidth;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                if (this._borderWidth != value)
                {
                    this._borderWidth = value;
                    this.OnChanged(true);
                }
            }
        }

        public override Rectangle ContentBounds
        {
            get
            {
                Rectangle contentBounds = base.ContentBounds;
                return new Rectangle((contentBounds.X + this.Padding.Left) + this.BorderWidth, (contentBounds.Y + this.Padding.Top) + this.BorderWidth, ((contentBounds.Width - this.Padding.Left) - this.Padding.Right) - (2 * this.BorderWidth), ((contentBounds.Height - this.Padding.Top) - this.Padding.Bottom) - (2 * this.BorderWidth));
            }
        }

        public BoxEdges Padding
        {
            get
            {
                return this._padding;
            }
            set
            {
                if (this._padding != value)
                {
                    this._padding = value;
                    this.OnChanged(true);
                }
            }
        }
    }
}

