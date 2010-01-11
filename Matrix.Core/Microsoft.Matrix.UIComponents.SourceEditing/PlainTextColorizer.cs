namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;

    public sealed class PlainTextColorizer : ITextColorizer, IDisposable
    {
        public int Colorize(char[] text, byte[] colors, int startIndex, int length, int initialColorState)
        {
            return 0;
        }

        public void Dispose()
        {
        }

        public ColorInfo[] ColorTable
        {
            get
            {
                return new ColorInfo[0];
            }
        }
    }
}

