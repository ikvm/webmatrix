namespace Microsoft.Matrix.Packages.Code.JSharp
{
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Collections;

    public class JSharpColorizer : ITextColorizer, IDisposable
    {
        private const int AtStringState = 3;
        private const int CommentState = 2;
        private const int DefaultState = 0;
        private static IDictionary delimiters = new Hashtable();
        private const string delimiterString = " {}\n,()\r\t;:+-/*%[]\"~|&<>=.\\@";
        private static IDictionary keywords;
        private const int StringState = 1;

        static JSharpColorizer()
        {
            for (int i = 0; i < " {}\n,()\r\t;:+-/*%[]\"~|&<>=.\\@".Length; i++)
            {
                delimiters.Add(" {}\n,()\r\t;:+-/*%[]\"~|&<>=.\\@"[i], string.Empty);
            }
            keywords = new Hashtable();
            keywords.Add("abstract", string.Empty);
            keywords.Add("boolean", string.Empty);
            keywords.Add("break", string.Empty);
            keywords.Add("byte", string.Empty);
            keywords.Add("case", string.Empty);
            keywords.Add("catch", string.Empty);
            keywords.Add("char", string.Empty);
            keywords.Add("class", string.Empty);
            keywords.Add("delegate", string.Empty);
            keywords.Add("const", string.Empty);
            keywords.Add("continue", string.Empty);
            keywords.Add("default", string.Empty);
            keywords.Add("do", string.Empty);
            keywords.Add("double", string.Empty);
            keywords.Add("else", string.Empty);
            keywords.Add("extends", string.Empty);
            keywords.Add("false", string.Empty);
            keywords.Add("final", string.Empty);
            keywords.Add("finally", string.Empty);
            keywords.Add("float", string.Empty);
            keywords.Add("for", string.Empty);
            keywords.Add("goto", string.Empty);
            keywords.Add("if", string.Empty);
            keywords.Add("implements", string.Empty);
            keywords.Add("import", string.Empty);
            keywords.Add("instanceof", string.Empty);
            keywords.Add("int", string.Empty);
            keywords.Add("interface", string.Empty);
            keywords.Add("long", string.Empty);
            keywords.Add("multicast", string.Empty);
            keywords.Add("native", string.Empty);
            keywords.Add("new", string.Empty);
            keywords.Add("null", string.Empty);
            keywords.Add("package", string.Empty);
            keywords.Add("private", string.Empty);
            keywords.Add("protected", string.Empty);
            keywords.Add("public", string.Empty);
            keywords.Add("return", string.Empty);
            keywords.Add("short", string.Empty);
            keywords.Add("static", string.Empty);
            keywords.Add("super", string.Empty);
            keywords.Add("switch", string.Empty);
            keywords.Add("synchronized", string.Empty);
            keywords.Add("this", string.Empty);
            keywords.Add("throw", string.Empty);
            keywords.Add("throws", string.Empty);
            keywords.Add("transient", string.Empty);
            keywords.Add("true", string.Empty);
            keywords.Add("try", string.Empty);
            keywords.Add("ubyte", string.Empty);
            keywords.Add("void", string.Empty);
            keywords.Add("volatile", string.Empty);
            keywords.Add("while", string.Empty);
        }

        public int Colorize(char[] text, byte[] colors, int startIndex, int endIndex, int initialColorState)
        {
            if ((text == null) || (endIndex == 0))
            {
                return initialColorState;
            }
            if (initialColorState == 1)
            {
                startIndex = this.ProcessString(text, colors, startIndex, endIndex);
            }
            else if (initialColorState == 2)
            {
                startIndex = this.ProcessComment(text, colors, startIndex, endIndex);
            }
            else if (initialColorState != 3)
            {
                while ((startIndex < endIndex) && char.IsWhiteSpace(text[startIndex]))
                {
                    colors[startIndex] = 0;
                    startIndex++;
                }
            }
            else
            {
                startIndex = this.ProcessAtString(text, colors, startIndex, endIndex);
            }
            if (startIndex == -1)
            {
                return initialColorState;
            }
            if (startIndex < endIndex)
            {
                int index = startIndex;
                while (index < endIndex)
                {
                    if (((text[startIndex] == '/') && ((startIndex + 1) < endIndex)) && (text[startIndex + 1] == '/'))
                    {
                        this.Fill(colors, startIndex, endIndex - startIndex, 3);
                        return 0;
                    }
                    char key = text[index];
                    bool flag = index == (endIndex - 1);
                    bool flag2 = delimiters.Contains(key);
                    if (flag2 || flag)
                    {
                        int length = index - startIndex;
                        if (length == 0)
                        {
                            this.Fill(colors, startIndex, 1, 0);
                        }
                        else
                        {
                            if (!flag2 && flag)
                            {
                                length++;
                            }
                            string str = new string(text, startIndex, length);
                            if (keywords.Contains(str))
                            {
                                this.Fill(colors, startIndex, length, 1);
                            }
                            else
                            {
                                this.Fill(colors, startIndex, length, 0);
                            }
                            if (flag2)
                            {
                                colors[index] = 0;
                            }
                            startIndex = index;
                        }
                        if (key == '\\')
                        {
                            startIndex = index + 2;
                        }
                        else if (((key == '@') && ((index + 1) < endIndex)) && (text[index + 1] == '"'))
                        {
                            index = this.ProcessAtString(text, colors, startIndex + 2, endIndex);
                            if (index == -1)
                            {
                                return 3;
                            }
                        }
                        else if (key == '"')
                        {
                            colors[startIndex] = 2;
                            index = this.ProcessString(text, colors, startIndex + 1, endIndex);
                            if (index == -1)
                            {
                                return 1;
                            }
                        }
                        else if (((key == '/') && ((index + 1) < endIndex)) && (text[index + 1] == '*'))
                        {
                            index = this.ProcessComment(text, colors, startIndex + 2, endIndex);
                            if (index == -1)
                            {
                                return 2;
                            }
                        }
                        index++;
                        startIndex = index;
                    }
                    else
                    {
                        index++;
                    }
                }
            }
            return 0;
        }

        public void Dispose()
        {
        }

        private void Fill(byte[] data, int startIndex, int length, byte value)
        {
            for (int i = startIndex; i < (startIndex + length); i++)
            {
                data[i] = value;
            }
        }

        private int ProcessAtString(char[] text, byte[] colors, int startIndex, int endIndex)
        {
            int index = startIndex;
            while (true)
            {
                if (index >= endIndex)
                {
                    this.Fill(colors, startIndex, index - startIndex, 2);
                    return -1;
                }
                if (text[index] == '"')
                {
                    if ((index + 1) >= endIndex)
                    {
                        this.Fill(colors, startIndex, index - startIndex, 2);
                        return index;
                    }
                    if (text[index + 1] != '"')
                    {
                        this.Fill(colors, startIndex, index - startIndex, 2);
                        return index;
                    }
                    index++;
                }
                index++;
            }
        }

        private int ProcessComment(char[] text, byte[] colors, int startIndex, int endIndex)
        {
            int index = startIndex;
            while (true)
            {
                if (index >= endIndex)
                {
                    startIndex = Math.Max(0, startIndex - 2);
                    this.Fill(colors, startIndex, index - startIndex, 3);
                    return -1;
                }
                if (((text[index] == '*') && ((index + 1) < endIndex)) && (text[index + 1] == '/'))
                {
                    startIndex = Math.Max(0, startIndex - 2);
                    this.Fill(colors, startIndex, (index + 2) - startIndex, 3);
                    return (index + 1);
                }
                index++;
            }
        }

        private int ProcessString(char[] text, byte[] colors, int startIndex, int endIndex)
        {
            int index = startIndex;
            while (true)
            {
                if (index >= endIndex)
                {
                    int num2 = index - startIndex;
                    if (num2 > 0)
                    {
                        this.Fill(colors, startIndex, index - startIndex, 2);
                        if (text[index - 1] == '\\')
                        {
                            return -1;
                        }
                    }
                    return index;
                }
                if (text[index] == '"')
                {
                    this.Fill(colors, startIndex, (index - startIndex) + 1, 2);
                    if (startIndex == 0)
                    {
                        index++;
                    }
                    return index;
                }
                if (((text[index] == '\\') && ((index + 1) < endIndex)) && ((text[index + 1] == '"') || (text[index + 1] == '\\')))
                {
                    index++;
                }
                index++;
            }
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

