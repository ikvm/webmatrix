namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web.Documents;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class ImageView : Panel, IDocumentView, IDesignView
    {
        private ImageDocument _document;
        private bool _isDirty;
        private IServiceProvider _serviceProvider;
        private ImageSurface _surface;
        private static Bitmap ViewImage;

        event EventHandler IDocumentView.DocumentChanged
        {
            add
            {
            }
            remove
            {
            }
        }

        public ImageView(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this.AutoScroll = true;
            this.BackColor = SystemColors.Control;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._serviceProvider = null;
            }
            base.Dispose(disposing);
        }

        protected virtual void InitializeUserInterface()
        {
            this._surface = new ImageSurface();
            base.Controls.Add(this._surface);
        }

        void IDocumentView.Activate(bool viewSwitch)
        {
            base.Focus();
        }

        void IDocumentView.Deactivate(bool closing)
        {
        }

        void IDocumentView.LoadFromDocument(Microsoft.Matrix.Core.Documents.Document document)
        {
            this._document = (ImageDocument) document;
            if (this._surface == null)
            {
                this.InitializeUserInterface();
            }
            this._surface.Image = this._document.Image;
        }

        bool IDocumentView.SaveToDocument()
        {
            this._document.Image = this._surface.Image;
            this._isDirty = false;
            return true;
        }

        protected ImageDocument Document
        {
            get
            {
                return this._document;
            }
        }

        bool IDocumentView.CanDeactivate
        {
            get
            {
                return true;
            }
        }

        bool IDocumentView.IsDirty
        {
            get
            {
                return this._isDirty;
            }
        }

        Image IDocumentView.ViewImage
        {
            get
            {
                if (ViewImage == null)
                {
                    ViewImage = new Bitmap(typeof(ImageView), "ImageView.bmp");
                    ViewImage.MakeTransparent();
                }
                return ViewImage;
            }
        }

        string IDocumentView.ViewName
        {
            get
            {
                return "Image";
            }
        }

        DocumentViewType IDocumentView.ViewType
        {
            get
            {
                return DocumentViewType.Design;
            }
        }

        private sealed class ImageSurface : Control
        {
            private System.Drawing.Image _image;
            private int _requestedHeight;
            private int _requestedWidth;

            public ImageSurface()
            {
                this.BackColor = SystemColors.Control;
                base.TabStop = false;
                base.Size = new Size(100, 50);
                base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.Opaque | ControlStyles.UserPaint, true);
            }

            private void AdjustControlSize()
            {
                int num = this._requestedWidth;
                int num2 = this._requestedHeight;
                try
                {
                    base.ClientSize = this.GetPreferredSize();
                }
                finally
                {
                    this._requestedHeight = num2;
                    this._requestedWidth = num;
                }
            }

            private Size GetPreferredSize()
            {
                if (this._image == null)
                {
                    return base.Size;
                }
                return this._image.Size;
            }

            protected override void OnPaint(PaintEventArgs pe)
            {
                pe.Graphics.FillRectangle(SystemBrushes.Control, base.ClientRectangle);
                if (this._image != null)
                {
                    try
                    {
                        pe.Graphics.DrawImage(this._image, new Rectangle(new Point(0, 0), this._image.Size));
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
            {
                if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
                {
                    this._requestedHeight = height;
                }
                if ((specified & BoundsSpecified.Width) != BoundsSpecified.None)
                {
                    this._requestedWidth = width;
                }
                Size preferredSize = this.GetPreferredSize();
                width = preferredSize.Width;
                height = preferredSize.Height;
                base.SetBoundsCore(x, y, width, height, specified);
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
                    this.AdjustControlSize();
                    if (base.IsHandleCreated)
                    {
                        base.Invalidate();
                    }
                }
            }
        }
    }
}

