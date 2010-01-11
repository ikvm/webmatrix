namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Documents;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    public class ImageDocumentStorage : IDocumentStorage, IDisposable
    {
        private ImageFormat _format;
        private System.Drawing.Image _image;
        private ImageDocument _owner;
        private PixelFormat _pixelFormat;

        public ImageDocumentStorage(ImageDocument owner)
        {
            this._owner = owner;
        }

        void IDocumentStorage.Load(Stream contentStream)
        {
            System.Drawing.Image image = System.Drawing.Image.FromStream(contentStream);
            this._format = image.RawFormat;
            this._pixelFormat = image.PixelFormat;
            this._image = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(this._image);
            graphics.DrawImage(image, 0, 0, image.Width, image.Height);
            graphics.Dispose();
        }

        void IDocumentStorage.Save(Stream contentStream)
        {
            if (this._image != null)
            {
                this._image.Save(contentStream, this._format);
            }
        }

        void IDisposable.Dispose()
        {
            this._image = null;
            this._owner = null;
        }

        public PixelFormat ColorDepth
        {
            get
            {
                return this._pixelFormat;
            }
        }

        public ImageFormat Format
        {
            get
            {
                return this._format;
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
                this._image = value;
                if (this._owner != null)
                {
                    this._owner.SetDirty();
                }
            }
        }
    }
}

