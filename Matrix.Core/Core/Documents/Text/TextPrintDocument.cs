namespace Microsoft.Matrix.Core.Documents.Text
{
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.IO;

    public class TextPrintDocument : PrintDocument
    {
        private bool _disposeTextFont;
        private TextDocument _document;
        private string _filePath;
        private StringFormat _footerFormat;
        private Brush _grayBrush;
        private Font _headerFooterFont;
        private float _headerFooterFontHeight;
        private StringFormat _headerFormat;
        private int _lineNumber;
        private StringFormat _lineNumberFormat;
        private string _overflowText;
        private int _pageNumber;
        private string _text;
        private Brush _textBrush;
        private Font _textFont;
        private float _textFontHeight;
        private StringFormat _textFormat;
        private StringReader _textReader;

        public TextPrintDocument(TextDocument document)
        {
            this._document = document;
        }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);
            this._text = this._document.Text;
            this._textReader = new StringReader(this._text);
            TextManager service = (TextManager) this._document.Site.GetService(typeof(TextManager));
            if (service != null)
            {
                this._textFont = service.PrintFont;
            }
            else
            {
                this._textFont = new Font("Lucida Console", 8f);
                this._disposeTextFont = true;
            }
            this._headerFooterFont = new Font("Verdana", 8f);
            this._textBrush = Brushes.Black;
            this._filePath = this._document.DocumentPath;
            this._pageNumber = 1;
            this._lineNumber = 0;
            this._overflowText = null;
        }

        protected override void OnEndPrint(PrintEventArgs e)
        {
            base.OnEndPrint(e);
            if (this._disposeTextFont)
            {
                this._textFont.Dispose();
            }
            this._textFormat.Dispose();
            this._headerFooterFont.Dispose();
            this._headerFormat.Dispose();
            this._footerFormat.Dispose();
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);
            Graphics graphics = e.Graphics;
            if (this._pageNumber == 1)
            {
                this._textFontHeight = this._textFont.GetHeight(graphics);
                this._textFormat = new StringFormat();
                this._lineNumberFormat = new StringFormat(StringFormatFlags.NoWrap);
                this._lineNumberFormat.Alignment = StringAlignment.Far;
                this._lineNumberFormat.Trimming = StringTrimming.None;
                this._headerFooterFontHeight = this._headerFooterFont.GetHeight(graphics);
                this._headerFormat = new StringFormat(StringFormatFlags.NoWrap);
                this._headerFormat.Trimming = StringTrimming.EllipsisPath;
                this._footerFormat = new StringFormat(StringFormatFlags.NoWrap);
                this._footerFormat.Alignment = StringAlignment.Far;
                this._textBrush = Brushes.Black;
                this._grayBrush = Brushes.Gray;
            }
            Margins margins = e.PageSettings.Margins;
            graphics.DrawString(this._filePath, this._headerFooterFont, this._textBrush, (float) margins.Left, (float) margins.Top, this._headerFormat);
            float num = (margins.Top + this._headerFooterFontHeight) + 2f;
            graphics.DrawLine(Pens.Black, (float) margins.Left, num, (float) (margins.Left + e.MarginBounds.Width), num);
            graphics.DrawString("Page " + this._pageNumber, this._headerFooterFont, this._textBrush, new RectangleF((float) margins.Left, (margins.Top + e.MarginBounds.Height) - this._headerFooterFontHeight, (float) e.MarginBounds.Width, this._headerFooterFontHeight), this._footerFormat);
            num = ((margins.Top + e.MarginBounds.Height) - this._headerFooterFontHeight) - 2f;
            graphics.DrawLine(Pens.Black, (float) margins.Left, num, (float) (margins.Left + e.MarginBounds.Width), num);
            float num2 = (((margins.Top + this._headerFooterFontHeight) + 2f) + 1f) + 4f;
            float num3 = e.MarginBounds.Height - (2f * (7f + this._headerFooterFontHeight));
            int num4 = (int) (num3 / this._textFontHeight);
            int num5 = 0;
            string text = null;
            while ((num5 < num4) && (this._overflowText != null))
            {
                num = num2 + (num5 * this._textFontHeight);
                num5 += this.PrintLine(e, this._overflowText, num, 0);
            }
            while ((num5 < num4) && ((text = this._textReader.ReadLine()) != null))
            {
                this._lineNumber++;
                num = num2 + (num5 * this._textFontHeight);
                for (num5 += this.PrintLine(e, text, num, this._lineNumber); (num5 < num4) && (this._overflowText != null); num5 += this.PrintLine(e, this._overflowText, num, 0))
                {
                    num = num2 + (num5 * this._textFontHeight);
                }
            }
            e.HasMorePages = text != null;
            this._pageNumber++;
        }

        private int PrintLine(PrintPageEventArgs e, string text, float yPos, int lineNumber)
        {
            int num;
            int num2;
            Graphics graphics = e.Graphics;
            Margins margins = e.PageSettings.Margins;
            if (lineNumber != 0)
            {
                string s = lineNumber.ToString() + ":";
                RectangleF ef = new RectangleF(0f, yPos, (float) (margins.Left - 8), this._textFontHeight);
                graphics.DrawString(s, this._textFont, this._grayBrush, ef, this._lineNumberFormat);
            }
            if (text.Length == 0)
            {
                return 1;
            }
            RectangleF layoutRectangle = new RectangleF((float) margins.Left, yPos, (float) e.MarginBounds.Width, this._textFontHeight);
            graphics.MeasureString(text, this._textFont, layoutRectangle.Size, this._textFormat, out num2, out num);
            if (num2 < text.Length)
            {
                this._overflowText = text.Substring(num2);
            }
            else
            {
                this._overflowText = null;
            }
            graphics.DrawString(text, this._textFont, this._textBrush, layoutRectangle, this._textFormat);
            return num;
        }
    }
}

