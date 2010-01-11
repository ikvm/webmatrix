namespace Microsoft.Matrix.Packages.Web.Documents
{
    using System;
    using System.Drawing;
    using System.Drawing.Printing;

    public class ImagePrintDocument : PrintDocument
    {
        private ImageDocument _document;

        public ImagePrintDocument(ImageDocument document)
        {
            this._document = document;
        }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);
            Image image = this._document.Image;
            Size size = image.Size;
            float num = 1f;
            Graphics graphics = e.Graphics;
            Margins margins = e.PageSettings.Margins;
            float height = 0f;
            Font font = new Font("Verdana", 8f);
            StringFormat format = new StringFormat(StringFormatFlags.NoWrap);
            format.Trimming = StringTrimming.EllipsisPath;
            height = font.GetHeight(graphics);
            graphics.DrawString(this._document.DocumentPath, font, Brushes.Black, (float) margins.Left, (float) margins.Top, format);
            float num3 = (margins.Top + height) + 2f;
            graphics.DrawLine(Pens.Black, (float) margins.Left, num3, (float) (margins.Left + e.MarginBounds.Width), num3);
            format.Dispose();
            font.Dispose();
            float num4 = (((e.MarginBounds.Height - height) - 2f) - 1f) - 4f;
            if ((size.Width > e.MarginBounds.Width) || (size.Height > num4))
            {
                float num5 = ((float) e.MarginBounds.Width) / ((float) size.Width);
                float num6 = num4 / ((float) size.Height);
                num = (num5 < num6) ? num5 : num6;
            }
            try
            {
                graphics.DrawImage(image, (float) margins.Left, (float) ((margins.Top + height) + 7f), (float) (image.Size.Width * num), (float) (image.Size.Height * num));
            }
            catch
            {
                graphics.FillRectangle(SystemBrushes.Control, (float) margins.Left, (margins.Top + height) + 7f, image.Size.Width * num, image.Size.Height * num);
                graphics.DrawImageUnscaled(SystemIcons.Error.ToBitmap(), margins.Left + 4, (int) (((margins.Top + height) + 7f) + 4f));
            }
            e.HasMorePages = false;
        }
    }
}

