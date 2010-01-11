namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    internal sealed class ImageUtility
    {
        private static ColorMatrix disabledImageColorMatrix;

        private ImageUtility()
        {
        }

        public static Image CreateDisabledImage(Image normalImage)
        {
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.ClearColorKey();
            imageAttr.SetColorMatrix(DisabledImageColorMatrix);
            Size size = normalImage.Size;
            Bitmap image = new Bitmap(size.Width, size.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.DrawImage(normalImage, new Rectangle(0, 0, size.Width, size.Height), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, imageAttr);
            graphics.Dispose();
            return image;
        }

        private static ColorMatrix DisabledImageColorMatrix
        {
            get
            {
                if (disabledImageColorMatrix == null)
                {
                    float[][] newColorMatrix = new float[5][];
                    newColorMatrix[0] = new float[] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f };
                    newColorMatrix[1] = new float[] { 0.2577f, 0.2577f, 0.2577f, 0f, 0f };
                    newColorMatrix[2] = new float[] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f };
                    float[] numArray2 = new float[5];
                    numArray2[3] = 1f;
                    newColorMatrix[3] = numArray2;
                    newColorMatrix[4] = new float[] { 0.38f, 0.38f, 0.38f, 0f, 1f };
                    disabledImageColorMatrix = new ColorMatrix(newColorMatrix);
                }
                return disabledImageColorMatrix;
            }
        }
    }
}

