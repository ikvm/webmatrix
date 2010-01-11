namespace Microsoft.Matrix.Packages.Web.Utility
{
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Drawing;

    internal class HtmlColorizer : ITextColorizer, IDisposable
    {
        public const byte AttrName = 0x41;
        public const byte AttrVal = 0x42;
        private static ColorInfo[] htmlColors = new ColorInfo[] { new ColorInfo(Color.Maroon, SystemColors.Window, false, false), new ColorInfo(Color.Red, SystemColors.Window, false, false), new ColorInfo(Color.Blue, SystemColors.Window, false, false), new ColorInfo(Color.Black, Color.Yellow, false, false) };
        public const byte MaxColorIndex = 3;
        public const byte TagName = 0x40;

        protected virtual int ColorizeToken(Token t, char[] text, byte[] colors, int colorState)
        {
            byte num = 0;
            int endState = t.EndState;
            bool flag = false;
            int startIndex = t.StartIndex;
            int endIndex = t.EndIndex;
            switch (t.Type)
            {
                case 0:
                    num = 0;
                    break;

                case 1:
                    num = 0x40;
                    break;

                case 2:
                    num = 0x41;
                    break;

                case 3:
                    num = 0x42;
                    break;

                case 5:
                case 11:
                    num = 1;
                    break;

                case 7:
                    num = 3;
                    break;

                case 10:
                    num = 1;
                    endState &= 0xffff;
                    break;

                default:
                    num = 0;
                    break;
            }
            if (!flag)
            {
                this.Fill(colors, startIndex, endIndex - startIndex, num);
            }
            return endState;
        }

        protected virtual void Dispose()
        {
        }

        protected void Fill(byte[] data, int startIndex, int length, byte value)
        {
            for (int i = startIndex; i < (startIndex + length); i++)
            {
                data[i] = value;
            }
        }

        int ITextColorizer.Colorize(char[] text, byte[] colors, int startIndex, int length, int initialColorState)
        {
            if ((text == null) || (text.Length == 0))
            {
                return initialColorState;
            }
            Token t = HtmlTokenizer.GetFirstToken(text, length, initialColorState & 0xffff);
            if (t == null)
            {
                return initialColorState;
            }
            int endState = t.EndState;
            while (t != null)
            {
                startIndex = t.StartIndex;
                endState = this.ColorizeToken(t, text, colors, initialColorState);
                t = HtmlTokenizer.GetNextToken(t);
                if (t != null)
                {
                    initialColorState = endState;
                    endState = t.EndState;
                }
            }
            colors[length] = 0;
            return endState;
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        protected virtual ColorInfo[] ColorTable
        {
            get
            {
                return htmlColors;
            }
        }

        ColorInfo[] ITextColorizer.ColorTable
        {
            get
            {
                return this.ColorTable;
            }
        }
    }
}

