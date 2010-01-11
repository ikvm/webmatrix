namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Printing;

    public class ImageDocument : Document, IPrintableDocument
    {
        public ImageDocument(DocumentProjectItem projectItem) : base(projectItem)
        {
        }

        protected virtual PrintDocument CreatePrintDocument()
        {
            return new ImagePrintDocument(this);
        }

        protected override IDocumentStorage CreateStorage()
        {
            return new ImageDocumentStorage(this);
        }

        PrintDocument IPrintableDocument.CreatePrintDocument()
        {
            return this.CreatePrintDocument();
        }

        [Category("Image"), Description("The file format of the image.")]
        public ImageFormat Format
        {
            get
            {
                return ((ImageDocumentStorage) base.Storage).Format;
            }
        }

        [Browsable(false)]
        public System.Drawing.Image Image
        {
            get
            {
                return ((ImageDocumentStorage) base.Storage).Image;
            }
            set
            {
                ((ImageDocumentStorage) base.Storage).Image = value;
                base.SetDirty();
            }
        }
    }
}

