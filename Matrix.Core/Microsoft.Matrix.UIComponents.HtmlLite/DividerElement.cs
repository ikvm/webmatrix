namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Drawing;

    public class DividerElement : VisualElement
    {
        private int _thickness = 1;

        protected override Size Measure(ElementRenderData renderData)
        {
            int num = (int) (1.5 * renderData.FontSize);
            return new Size(1, num + this._thickness);
        }

        protected override void RenderForeground(ElementRenderData renderData)
        {
            if (this._thickness > 0)
            {
                Point location = this.ContentBounds.Location;
                int dy = ((this.ContentBounds.Height - this._thickness) / 2) + 1;
                location.Offset(0, dy);
                Pen pen = new Pen(renderData.ForeColor, (float) this._thickness);
                renderData.Graphics.DrawLine(pen, location.X, location.Y, location.X + this.ContentBounds.Width, location.Y);
                pen.Dispose();
            }
        }

        public override Microsoft.Matrix.UIComponents.HtmlLite.LayoutMode LayoutMode
        {
            get
            {
                return (Microsoft.Matrix.UIComponents.HtmlLite.LayoutMode.LineBreakAfter | Microsoft.Matrix.UIComponents.HtmlLite.LayoutMode.LineBreakBefore);
            }
        }

        public override Microsoft.Matrix.UIComponents.HtmlLite.SizeMode SizeMode
        {
            get
            {
                return Microsoft.Matrix.UIComponents.HtmlLite.SizeMode.StretchHorizontal;
            }
        }

        public int Thickness
        {
            get
            {
                return this._thickness;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                if (this._thickness != value)
                {
                    this._thickness = value;
                    this.OnChanged(true);
                }
            }
        }
    }
}

