namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;

    public sealed class SplashScreenSurface
    {
        private Rectangle _bounds;
        private System.Drawing.Graphics _graphics;

        internal SplashScreenSurface(System.Drawing.Graphics g, Rectangle bounds)
        {
            this._graphics = g;
            this._bounds = bounds;
        }

        public Rectangle Bounds
        {
            get
            {
                return this._bounds;
            }
        }

        public System.Drawing.Graphics Graphics
        {
            get
            {
                return this._graphics;
            }
        }
    }
}

