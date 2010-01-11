namespace Microsoft.Matrix.Packages.ClassView.UserInterface
{
    using Microsoft.Matrix.UIComponents.HtmlLite;
    using System;
    using System.Drawing;

    internal sealed class LabelElement : TextElement
    {
        private int _width;

        public LabelElement()
        {
        }

        public LabelElement(string text) : base(text)
        {
        }

        protected override Size Measure(ElementRenderData renderData)
        {
            Size size = base.Measure(renderData);
            if ((this._width != 0) && (this._width > size.Width))
            {
                size = new Size(this._width, size.Height);
            }
            return size;
        }

        public int Width
        {
            get
            {
                return this._width;
            }
            set
            {
                if (this._width != value)
                {
                    this._width = value;
                    this.OnChanged(true);
                }
            }
        }
    }
}

