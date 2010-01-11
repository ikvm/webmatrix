namespace Microsoft.Matrix.Packages.Web.Utility.Formatting
{
    using System;
    using System.IO;
    using System.Text;

    internal sealed class FormattedTextWriter : TextWriter
    {
        private TextWriter baseWriter;
        private int currentColumn;
        private int indentLevel;
        private bool indentPending;
        private string indentString;
        private bool onNewLine;

        public FormattedTextWriter(TextWriter writer, string indentString)
        {
            this.baseWriter = writer;
            this.indentString = indentString;
            this.onNewLine = true;
            this.currentColumn = 0;
        }

        public override void Close()
        {
            this.baseWriter.Close();
        }

        public override void Flush()
        {
            this.baseWriter.Flush();
        }

        public static bool HasBackWhiteSpace(string s)
        {
            return (((s != null) && (s.Length != 0)) && char.IsWhiteSpace(s[s.Length - 1]));
        }

        public static bool HasFrontWhiteSpace(string s)
        {
            return (((s != null) && (s.Length != 0)) && char.IsWhiteSpace(s[0]));
        }

        public static bool IsWhiteSpace(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!char.IsWhiteSpace(s[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private string MakeSingleLine(string s)
        {
            StringBuilder builder = new StringBuilder();
            int num = 0;
            while (num < s.Length)
            {
                char c = s[num];
                if (char.IsWhiteSpace(c))
                {
                    builder.Append(' ');
                    while ((num < s.Length) && char.IsWhiteSpace(s[num]))
                    {
                        num++;
                    }
                }
                else
                {
                    builder.Append(c);
                    num++;
                }
            }
            return builder.ToString();
        }

        private void OutputIndent()
        {
            if (this.indentPending)
            {
                for (int i = 0; i < this.indentLevel; i++)
                {
                    this.baseWriter.Write(this.indentString);
                }
                this.indentPending = false;
            }
        }

        public static string Trim(string text, bool frontWhiteSpace)
        {
            if (text.Length == 0)
            {
                return string.Empty;
            }
            if (IsWhiteSpace(text))
            {
                if (frontWhiteSpace)
                {
                    return " ";
                }
                return string.Empty;
            }
            string str = text.Trim();
            if (frontWhiteSpace && HasFrontWhiteSpace(text))
            {
                str = ' ' + str;
            }
            if (HasBackWhiteSpace(text))
            {
                str = str + ' ';
            }
            return str;
        }

        public override void Write(bool value)
        {
            this.OutputIndent();
            this.baseWriter.Write(value);
            this.onNewLine = false;
            this.currentColumn += value.ToString().Length;
        }

        public override void Write(char value)
        {
            this.OutputIndent();
            this.baseWriter.Write(value);
            this.onNewLine = false;
            this.currentColumn++;
        }

        public override void Write(double value)
        {
            this.OutputIndent();
            this.baseWriter.Write(value);
            this.onNewLine = false;
            this.currentColumn += value.ToString().Length;
        }

        public override void Write(int value)
        {
            this.OutputIndent();
            this.baseWriter.Write(value);
            this.onNewLine = false;
            this.currentColumn += value.ToString().Length;
        }

        public override void Write(object value)
        {
            this.OutputIndent();
            this.baseWriter.Write(value);
            this.onNewLine = false;
            this.currentColumn += value.ToString().Length;
        }

        public override void Write(string s)
        {
            this.OutputIndent();
            this.baseWriter.Write(s);
            this.onNewLine = false;
            this.currentColumn += s.Length;
        }

        public override void Write(char[] buffer)
        {
            this.OutputIndent();
            this.baseWriter.Write(buffer);
            this.onNewLine = false;
            this.currentColumn += buffer.Length;
        }

        public override void Write(long value)
        {
            this.OutputIndent();
            this.baseWriter.Write(value);
            this.onNewLine = false;
            this.currentColumn += value.ToString().Length;
        }

        public override void Write(float value)
        {
            this.OutputIndent();
            this.baseWriter.Write(value);
            this.onNewLine = false;
            this.currentColumn += value.ToString().Length;
        }

        public override void Write(string format, params object[] arg)
        {
            this.OutputIndent();
            string str = string.Format(format, arg);
            this.baseWriter.Write(str);
            this.onNewLine = false;
            this.currentColumn += str.Length;
        }

        public override void Write(string format, object arg0)
        {
            this.OutputIndent();
            string str = string.Format(format, arg0);
            this.baseWriter.Write(str);
            this.onNewLine = false;
            this.currentColumn += str.Length;
        }

        public override void Write(char[] buffer, int index, int count)
        {
            this.OutputIndent();
            this.baseWriter.Write(buffer, index, count);
            this.onNewLine = false;
            this.currentColumn += count;
        }

        public override void Write(string format, object arg0, object arg1)
        {
            this.OutputIndent();
            string str = string.Format(format, arg0, arg1);
            this.baseWriter.Write(str);
            this.onNewLine = false;
            this.currentColumn += str.Length;
        }

        public override void WriteLine()
        {
            this.OutputIndent();
            this.baseWriter.WriteLine();
            this.indentPending = true;
            this.onNewLine = true;
            this.currentColumn = 0;
        }

        public override void WriteLine(bool value)
        {
            this.OutputIndent();
            this.baseWriter.WriteLine(value);
            this.indentPending = true;
            this.onNewLine = true;
            this.currentColumn = 0;
        }

        public override void WriteLine(char value)
        {
            this.OutputIndent();
            this.baseWriter.WriteLine(value);
            this.indentPending = true;
            this.onNewLine = true;
            this.currentColumn = 0;
        }

        public override void WriteLine(double value)
        {
            this.OutputIndent();
            this.baseWriter.WriteLine(value);
            this.indentPending = true;
            this.onNewLine = true;
            this.currentColumn = 0;
        }

        public override void WriteLine(int value)
        {
            this.OutputIndent();
            this.baseWriter.WriteLine(value);
            this.indentPending = true;
            this.onNewLine = true;
            this.currentColumn = 0;
        }

        public override void WriteLine(long value)
        {
            this.OutputIndent();
            this.baseWriter.WriteLine(value);
            this.indentPending = true;
            this.onNewLine = true;
            this.currentColumn = 0;
        }

        public override void WriteLine(object value)
        {
            this.OutputIndent();
            this.baseWriter.WriteLine(value);
            this.indentPending = true;
            this.onNewLine = true;
            this.currentColumn = 0;
        }

        public override void WriteLine(char[] buffer)
        {
            this.OutputIndent();
            this.baseWriter.WriteLine(buffer);
            this.indentPending = true;
            this.onNewLine = true;
            this.currentColumn = 0;
        }

        public override void WriteLine(float value)
        {
            this.OutputIndent();
            this.baseWriter.WriteLine(value);
            this.indentPending = true;
            this.onNewLine = true;
            this.currentColumn = 0;
        }

        public override void WriteLine(string s)
        {
            this.OutputIndent();
            this.baseWriter.WriteLine(s);
            this.indentPending = true;
            this.onNewLine = true;
            this.currentColumn = 0;
        }

        public override void WriteLine(string format, params object[] arg)
        {
            this.OutputIndent();
            this.baseWriter.WriteLine(format, arg);
            this.indentPending = true;
            this.currentColumn = 0;
            this.onNewLine = true;
        }

        public override void WriteLine(string format, object arg0)
        {
            this.OutputIndent();
            this.baseWriter.WriteLine(format, arg0);
            this.indentPending = true;
            this.onNewLine = true;
            this.currentColumn = 0;
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            this.OutputIndent();
            this.baseWriter.WriteLine(buffer, index, count);
            this.indentPending = true;
            this.onNewLine = true;
            this.currentColumn = 0;
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            this.OutputIndent();
            this.baseWriter.WriteLine(format, arg0, arg1);
            this.indentPending = true;
            this.onNewLine = true;
            this.currentColumn = 0;
        }

        public void WriteLineIfNotOnNewLine()
        {
            if (!this.onNewLine)
            {
                this.baseWriter.WriteLine();
                this.onNewLine = true;
                this.currentColumn = 0;
                this.indentPending = true;
            }
        }

        public void WriteLiteral(string s)
        {
            if (s.Length != 0)
            {
                StringReader reader = new StringReader(s);
                string str = reader.ReadLine();
                string str2 = reader.ReadLine();
                while (str != null)
                {
                    this.Write(str);
                    str = str2;
                    str2 = reader.ReadLine();
                    if (str != null)
                    {
                        this.WriteLine();
                    }
                    if (str2 != null)
                    {
                        str = str.Trim();
                    }
                    else if (str != null)
                    {
                        str = Trim(str, false);
                    }
                }
            }
        }

        public void WriteLiteralWrapped(string s, int maxLength)
        {
            if (s.Length != 0)
            {
                string[] strArray = this.MakeSingleLine(s).Split(null);
                if (HasFrontWhiteSpace(s))
                {
                    this.Write(' ');
                }
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i].Length > 0)
                    {
                        this.Write(strArray[i]);
                        if ((i < (strArray.Length - 1)) && (strArray[i + 1].Length > 0))
                        {
                            if (this.currentColumn > maxLength)
                            {
                                this.WriteLine();
                            }
                            else
                            {
                                this.Write(' ');
                            }
                        }
                    }
                }
                if (HasBackWhiteSpace(s) && !IsWhiteSpace(s))
                {
                    this.Write(' ');
                }
            }
        }

        public override System.Text.Encoding Encoding
        {
            get
            {
                return this.baseWriter.Encoding;
            }
        }

        public int Indent
        {
            get
            {
                return this.indentLevel;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                this.indentLevel = value;
            }
        }

        public override string NewLine
        {
            get
            {
                return this.baseWriter.NewLine;
            }
            set
            {
                this.baseWriter.NewLine = value;
            }
        }
    }
}

