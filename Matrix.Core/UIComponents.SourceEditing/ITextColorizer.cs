namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;

    public interface ITextColorizer : IDisposable
    {
        int Colorize(char[] text, byte[] colors, int startIndex, int endIndex, int initialColorState);

        ColorInfo[] ColorTable { get; }
    }
}

