namespace Microsoft.Matrix.Packages.Community.UserInterface
{
    using System.Drawing;

    public sealed class HomeCommunityTab : LinksCommunityTab
    {
        public override Image Glyph
        {
            get
            {
                Bitmap bitmap = new Bitmap(typeof(HomeCommunityTab), "Home.bmp");
                bitmap.MakeTransparent(Color.Fuchsia);
                return bitmap;
            }
        }
    }
}

