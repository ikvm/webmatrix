namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Drawing;

    public class ImageElement : BlockElement
    {
        private bool _behaveAsButton;
        private System.Drawing.Image _image;

        public ImageElement()
        {
        }

        public ImageElement(System.Drawing.Image image)
        {
            this._image = image;
        }

        protected override Size MeasureContents(ElementRenderData renderData)
        {
            if (this._image != null)
            {
                return this._image.Size;
            }
            return Size.Empty;
        }

        protected override void RenderContents(ElementRenderData renderData)
        {
            if (this._image != null)
            {
                renderData.Graphics.DrawImageUnscaled(this._image, this.ContentBounds.Location);
            }
        }

        public bool BehaveAsButton
        {
            get
            {
                return this._behaveAsButton;
            }
            set
            {
                if (this._behaveAsButton != value)
                {
                    this._behaveAsButton = value;
                    this.OnChanged(true);
                }
            }
        }

        public override bool Clickable
        {
            get
            {
                if (!this.BehaveAsButton)
                {
                    return base.Clickable;
                }
                return true;
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this._image;
            }
            set
            {
                if (this._image != value)
                {
                    this._image = value;
                    this.OnChanged(true);
                }
            }
        }
    }
}

