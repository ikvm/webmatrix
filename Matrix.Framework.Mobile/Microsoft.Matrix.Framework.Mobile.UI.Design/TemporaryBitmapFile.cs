namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    internal class TemporaryBitmapFile : IDisposable
    {
        private Bitmap _bitmap;
        private string _path;

        internal TemporaryBitmapFile(Bitmap bitmap)
        {
            this._bitmap = bitmap;
            this._path = Path.GetTempPath() + Guid.NewGuid().ToString() + ".bmp";
            this.Sync();
        }

        public void Dispose()
        {
            if (this._bitmap != null)
            {
                this._bitmap.Dispose();
                this._bitmap = null;
            }
            if (this._path != null)
            {
                FileAttributes attributes = File.GetAttributes(this._path);
                File.SetAttributes(this._path, attributes & ~FileAttributes.ReadOnly);
                File.Delete(this._path);
                this._path = null;
            }
        }

        private void Sync()
        {
            FileAttributes attributes;
            if (File.Exists(this._path))
            {
                attributes = File.GetAttributes(this._path);
                File.SetAttributes(this._path, attributes & ~FileAttributes.ReadOnly);
            }
            this._bitmap.Save(this._path, ImageFormat.Bmp);
            attributes = File.GetAttributes(this._path);
            File.SetAttributes(this._path, attributes | FileAttributes.ReadOnly);
        }

        internal Bitmap UnderlyingBitmap
        {
            get
            {
                return this._bitmap;
            }
            set
            {
                if (this._bitmap != null)
                {
                    this._bitmap.Dispose();
                }
                this._bitmap = value;
                this.Sync();
            }
        }

        internal string Url
        {
            get
            {
                return ("file:///" + this._path);
            }
        }
    }
}

