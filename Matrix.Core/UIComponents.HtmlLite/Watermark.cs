namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Watermark
    {
        private System.Drawing.Image _image;
        private WatermarkPlacement _placement;
        public Watermark(System.Drawing.Image image) : this(image, WatermarkPlacement.TopLeft)
        {
        }

        public Watermark(System.Drawing.Image image, WatermarkPlacement placement)
        {
            if ((placement < WatermarkPlacement.TopLeft) || (placement > WatermarkPlacement.Center))
            {
                throw new ArgumentOutOfRangeException("placement");
            }
            this._image = image;
            this._placement = placement;
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this._image;
            }
        }
        public WatermarkPlacement Placement
        {
            get
            {
                return this._placement;
            }
        }
    }
}

