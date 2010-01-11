namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using System;
    using System.Drawing;

    internal sealed class ImageCreator
    {
        private const string _fontFamily = "Tahoma";

        public static void CreateBackgroundImage(ref TemporaryBitmapFile bmpFile, string controlID, string title, string message, bool infoMode, int controlWidth)
        {
            if (controlWidth < 0x4b)
            {
                controlWidth = 0x4b;
            }
            Bitmap image = infoMode ? Constants.InfoIcon : Constants.ErrorIcon;
            bool flag = (message != null) && (message.Length != 0);
            bool flag2 = ((title != null) && (title.Length != 0)) || ((controlID != null) && (controlID.Length != 0));
            using (Font font = new Font("Tahoma", 8f, FontStyle.Regular))
            {
                using (Font font2 = new Font(font.FontFamily, 8f, FontStyle.Bold))
                {
                    using (Brush brush = new SolidBrush(SystemColors.ControlText))
                    {
                        using (Brush brush2 = new SolidBrush(SystemColors.ControlDark))
                        {
                            using (Brush brush3 = new SolidBrush(SystemColors.Control))
                            {
                                using (Brush brush4 = new SolidBrush(SystemColors.Window))
                                {
                                    using (Pen pen = new Pen(SystemColors.ControlDark))
                                    {
                                        using (Pen pen2 = new Pen(SystemColors.Window))
                                        {
                                            int height = 0;
                                            if (flag2)
                                            {
                                                height = GetHeight("'", font, controlWidth - 30) + 6;
                                            }
                                            int num2 = 0;
                                            if (flag)
                                            {
                                                int num3 = GetHeight(message, font, controlWidth - 30);
                                                num2 = (num3 < (image.Height + 6)) ? (image.Height + 6) : (num3 + 3);
                                            }
                                            int width = 500;
                                            int num5 = height + num2;
                                            Bitmap bitmap2 = new Bitmap(width, num5);
                                            using (Graphics graphics = Graphics.FromImage(bitmap2))
                                            {
                                                if (flag2)
                                                {
                                                    graphics.FillRectangle(brush3, 0, 0, width, height);
                                                    graphics.DrawLine(pen, 0, height - 1, width, height - 1);
                                                    graphics.DrawString(controlID, font2, brush, (float) 2f, (float) 2f);
                                                    if ((title != null) && (title.Length > 0))
                                                    {
                                                        int num6 = (int) graphics.MeasureString(controlID, font2).Width;
                                                        graphics.DrawString(" - " + title, font, brush, (float) (4 + num6), 2f);
                                                    }
                                                }
                                                if (flag)
                                                {
                                                    graphics.DrawLine(pen2, 0, height, width, height);
                                                    graphics.FillRectangle(brush2, 0, height + 1, width, num2 - 1);
                                                    graphics.DrawString(message, font, brush4, new RectangleF(20f, (float) (height + 1), (float) (controlWidth - 30), (float) (num2 - 1)));
                                                    graphics.DrawImage(image, 2, height + 3);
                                                }
                                                if (bmpFile == null)
                                                {
                                                    bmpFile = new TemporaryBitmapFile(bitmap2);
                                                }
                                                else
                                                {
                                                    bmpFile.UnderlyingBitmap = bitmap2;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static int GetHeight(string text, Font font, int width)
        {
            int num;
            using (Bitmap bitmap = new Bitmap(1, 1))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    SizeF layoutArea = new SizeF((float) width, 0f);
                    num = (int) (graphics.MeasureString(text, font, layoutArea).Height + 1f);
                }
            }
            return num;
        }
    }
}

