namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class ImageButton : MxButton
    {
        private System.Drawing.Image _image;
        private ImageList _imageList;

        private void DisposeImage()
        {
            if (this._imageList != null)
            {
                base.ImageList = null;
                this._imageList.Dispose();
                this._imageList = null;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.UpdateImage();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (!base.RecreatingHandle)
            {
                this.DisposeImage();
            }
            base.OnHandleDestroyed(e);
        }

        private void UpdateImage()
        {
            if (!MxTheme.IsAppThemed)
            {
                base.Image = this._image;
            }
            if (this._imageList != null)
            {
                this.DisposeImage();
            }
            if (this._image != null)
            {
                this._imageList = new ImageList();
                this._imageList.ImageSize = this._image.Size;
                System.Drawing.Image image = ImageUtility.CreateDisabledImage(this._image);
                this._imageList.Images.Add(this._image);
                this._imageList.Images.Add(this._image);
                this._imageList.Images.Add(this._image);
                this._imageList.Images.Add(image);
                this._imageList.Images.Add(this._image);
                Interop.BUTTON_IMAGELIST lParam = new Interop.BUTTON_IMAGELIST();
                lParam.hImageList = this._imageList.Handle;
                int num = 4;
                switch (base.ImageAlign)
                {
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomCenter:
                    case ContentAlignment.BottomRight:
                        num = 3;
                        break;

                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopCenter:
                    case ContentAlignment.TopRight:
                        num = 2;
                        break;

                    case ContentAlignment.MiddleLeft:
                        num = 0;
                        break;

                    case ContentAlignment.MiddleRight:
                        num = 1;
                        break;
                }
                lParam.align = num;
                Interop.SendMessage(base.Handle, 0x1602, IntPtr.Zero, lParam);
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
                if (base.IsHandleCreated)
                {
                    this.UpdateImage();
                }
            }
        }
    }
}

