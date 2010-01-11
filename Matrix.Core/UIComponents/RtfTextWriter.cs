namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;

    public class RtfTextWriter : TextWriter
    {
        private int _colorTableCount;
        private bool _enableFormatting;
        private int _fontTableCount;
        private int _indentLevel;
        private InnerWriter _innerWriter;
        private bool _tabsPending;
        private static readonly char[] tab = "    ".ToCharArray();

        public RtfTextWriter() : this(new StringWriter(), true)
        {
        }

        public RtfTextWriter(bool enableFormatting) : this(new StringWriter(), enableFormatting)
        {
        }

        public RtfTextWriter(TextWriter underlyingWriter) : this(underlyingWriter, true)
        {
        }

        public RtfTextWriter(TextWriter underlyingWriter, bool enableFormatting)
        {
            this._innerWriter = new InnerWriter(underlyingWriter);
            this._enableFormatting = enableFormatting;
        }

        protected virtual void OutputTabs()
        {
            if (this._tabsPending)
            {
                for (int i = 0; i < this._indentLevel; i++)
                {
                    this._innerWriter.Write(tab);
                }
                this._tabsPending = false;
            }
        }

        public override string ToString()
        {
            return this._innerWriter.ToString();
        }

        public override void Write(bool value)
        {
            this.OutputTabs();
            this._innerWriter.Write(value);
        }

        public override void Write(char value)
        {
            this.OutputTabs();
            this._innerWriter.Write(value);
        }

        public override void Write(double value)
        {
            this.OutputTabs();
            this._innerWriter.Write(value);
        }

        public override void Write(char[] buffer)
        {
            this.OutputTabs();
            this._innerWriter.Write(buffer);
        }

        public override void Write(int value)
        {
            this.OutputTabs();
            this._innerWriter.Write(value);
        }

        public override void Write(long value)
        {
            this.OutputTabs();
            this._innerWriter.Write(value);
        }

        public override void Write(object value)
        {
            this.OutputTabs();
            this._innerWriter.Write(value);
        }

        public override void Write(float value)
        {
            this.OutputTabs();
            this._innerWriter.Write(value);
        }

        public override void Write(string s)
        {
            this.OutputTabs();
            this._innerWriter.Write(s);
        }

        [CLSCompliant(false)]
        public override void Write(uint value)
        {
            this.OutputTabs();
            this._innerWriter.Write(value);
        }

        public override void Write(string format, object arg0)
        {
            this.OutputTabs();
            this._innerWriter.Write(format, arg0);
        }

        public override void Write(string format, params object[] arg)
        {
            this.OutputTabs();
            this._innerWriter.Write(format, arg);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            this.OutputTabs();
            this._innerWriter.Write(format, arg0, arg1);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            this.OutputTabs();
            this._innerWriter.Write(buffer, index, count);
        }

        public void WriteBackColorAttribute(int colorID)
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this._innerWriter.Write(@"\cb");
                this._innerWriter.Write(colorID);
                this._innerWriter.Escape = true;
            }
        }

        public void WriteBeginColorTable()
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this.Write(@"{\colortbl ;");
                this._innerWriter.Escape = true;
                this._indentLevel++;
            }
        }

        public void WriteBeginFontTable()
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this.WriteLine(@"{\fonttbl");
                this._innerWriter.Escape = true;
                this._indentLevel++;
            }
        }

        public void WriteBeginGroup()
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this.Write('{');
                this._innerWriter.Escape = true;
            }
        }

        public void WriteBeginRtf()
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this.WriteLine(@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033");
                this._innerWriter.Escape = true;
            }
        }

        public void WriteBoldAttribute()
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this._innerWriter.Write(@"\b");
                this._innerWriter.Escape = true;
            }
        }

        public int WriteColor(Color value)
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this.Write(@"\red");
                this.Write((int) value.R);
                this.Write(@"\green");
                this.Write((int) value.G);
                this.Write(@"\blue");
                this.Write((int) value.B);
                this.Write(';');
                this._innerWriter.Escape = true;
                this._colorTableCount++;
                return this._colorTableCount;
            }
            return 0;
        }

        public void WriteEndColorTable()
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this._indentLevel--;
                this.WriteLine('}');
                this._innerWriter.Escape = true;
            }
        }

        public void WriteEndFontTable()
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this._indentLevel--;
                this.WriteLine('}');
                this._innerWriter.Escape = true;
            }
        }

        public void WriteEndGroup()
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this.Write('}');
                this._innerWriter.Escape = true;
            }
        }

        public void WriteEndRtf()
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this.WriteLine('}');
                this._innerWriter.Escape = true;
            }
        }

        public int WriteFont(Font font)
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this.Write(@"{\f");
                this.Write(this._fontTableCount);
                this.Write(@"\fnil\fcharset" + font.GdiCharSet + " ");
                this.Write(font.Name);
                this.WriteLine(";}");
                this._innerWriter.Escape = true;
                this._fontTableCount++;
                return this._fontTableCount;
            }
            return 0;
        }

        public void WriteFontAttribute(int fontID, int fontSize)
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this._innerWriter.Write(@"\f");
                this._innerWriter.Write(fontID);
                this._innerWriter.Write(@"\fs");
                this._innerWriter.Write((int) (fontSize * 2));
                this._innerWriter.Escape = true;
            }
        }

        public void WriteForeColorAttribute(int colorID)
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this._innerWriter.Write(@"\cf");
                this._innerWriter.Write(colorID);
                this._innerWriter.Escape = true;
            }
        }

        public void WriteItalicAttribute()
        {
            if (this._enableFormatting)
            {
                this._innerWriter.Escape = false;
                this._innerWriter.Write(@"\i");
                this._innerWriter.Escape = true;
            }
        }

        public override void WriteLine()
        {
            this.OutputTabs();
            this._innerWriter.WriteLine();
            this._tabsPending = true;
        }

        public override void WriteLine(bool value)
        {
            this.OutputTabs();
            this._innerWriter.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(char value)
        {
            this.OutputTabs();
            this._innerWriter.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(string s)
        {
            this.OutputTabs();
            this._innerWriter.WriteLine(s);
            this._tabsPending = true;
        }

        public override void WriteLine(char[] buffer)
        {
            this.OutputTabs();
            this._innerWriter.WriteLine(buffer);
            this._tabsPending = true;
        }

        public override void WriteLine(double value)
        {
            this.OutputTabs();
            this._innerWriter.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(int value)
        {
            this.OutputTabs();
            this._innerWriter.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(long value)
        {
            this.OutputTabs();
            this._innerWriter.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(object value)
        {
            this.OutputTabs();
            this._innerWriter.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(float value)
        {
            this.OutputTabs();
            this._innerWriter.WriteLine(value);
            this._tabsPending = true;
        }

        [CLSCompliant(false)]
        public override void WriteLine(uint value)
        {
            this.OutputTabs();
            this._innerWriter.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(string format, object arg0)
        {
            this.OutputTabs();
            this._innerWriter.WriteLine(format, arg0);
            this._tabsPending = true;
        }

        public override void WriteLine(string format, params object[] arg)
        {
            this.OutputTabs();
            this._innerWriter.WriteLine(format, arg);
            this._tabsPending = true;
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            this.OutputTabs();
            this._innerWriter.WriteLine(format, arg0, arg1);
            this._tabsPending = true;
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            this.OutputTabs();
            this._innerWriter.WriteLine(buffer, index, count);
            this._tabsPending = true;
        }

        public void WriteLineNoTabs(string s)
        {
            this._innerWriter.WriteLine(s);
        }

        public void WriteUnderlineAttribute()
        {
            if (this._enableFormatting)
            {
                this.WriteUnderlineAttribute(UnderlineStyle.Single);
            }
        }

        private void WriteUnderlineAttribute(UnderlineStyle style)
        {
            this._innerWriter.Escape = false;
            switch (style)
            {
                case UnderlineStyle.None:
                    this._innerWriter.Write(@"\ul0");
                    break;

                case UnderlineStyle.Single:
                    this._innerWriter.Write(@"\ul");
                    break;

                case UnderlineStyle.Word:
                    this._innerWriter.Write(@"\ulw");
                    break;

                case UnderlineStyle.Dotted:
                    this._innerWriter.Write(@"\uld");
                    break;

                case UnderlineStyle.Doubled:
                    this._innerWriter.Write(@"\uldb");
                    break;

                case UnderlineStyle.Wave:
                    this._innerWriter.Write(@"\ulwave");
                    break;
            }
            this._innerWriter.Escape = true;
        }

        public override System.Text.Encoding Encoding
        {
            get
            {
                return this._innerWriter.Encoding;
            }
        }

        private sealed class InnerWriter : TextWriter
        {
            private bool _escape;
            private TextWriter _underlyingWriter;
            private static readonly char[] newLine = "\\line\r\n".ToCharArray();

            public InnerWriter(TextWriter underlyingWriter)
            {
                this._underlyingWriter = underlyingWriter;
                this._escape = true;
            }

            public override string ToString()
            {
                return this._underlyingWriter.ToString();
            }

            public override void Write(char value)
            {
                if (this._escape)
                {
                    if (value == '{')
                    {
                        this._underlyingWriter.Write('\\');
                        this._underlyingWriter.Write('{');
                    }
                    else if (value == '}')
                    {
                        this._underlyingWriter.Write('\\');
                        this._underlyingWriter.Write('}');
                    }
                    else if (value == '\\')
                    {
                        this._underlyingWriter.Write('\\');
                        this._underlyingWriter.Write('\\');
                    }
                    else
                    {
                        this._underlyingWriter.Write(@"\u" + ((int) value) + "*");
                    }
                }
                else
                {
                    this._underlyingWriter.Write(value);
                }
            }

            public override void Write(string value)
            {
                if (value == null)
                {
                    this._underlyingWriter.Write("");
                }
                else if (this._escape)
                {
                    char[] chArray = value.ToCharArray();
                    for (int i = 0; i < chArray.Length; i++)
                    {
                        this.Write(chArray[i]);
                    }
                }
                else
                {
                    this._underlyingWriter.Write(value);
                }
            }

            public override void WriteLine()
            {
                if (this._escape)
                {
                    this._underlyingWriter.Write(newLine);
                }
                else
                {
                    this._underlyingWriter.WriteLine();
                }
            }

            public override void WriteLine(string value)
            {
                if (this._escape)
                {
                    this.Write(value);
                    this._underlyingWriter.Write(newLine);
                }
                else
                {
                    this._underlyingWriter.WriteLine(value);
                }
            }

            public override System.Text.Encoding Encoding
            {
                get
                {
                    return this._underlyingWriter.Encoding;
                }
            }

            public bool Escape
            {
                get
                {
                    return this._escape;
                }
                set
                {
                    this._escape = value;
                }
            }
        }

        private enum UnderlineStyle
        {
            None,
            Single,
            Word,
            Dotted,
            Doubled,
            Wave
        }
    }
}

