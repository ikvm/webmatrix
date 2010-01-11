namespace Microsoft.Matrix.Packages.Web.Utility.Formatting
{
    using System;
    using System.IO;

    internal class HtmlWriter
    {
        private TextWriter _baseWriter;
        private int _maxLineLength;
        private FormattedTextWriter _writer;

        public HtmlWriter(TextWriter writer, string indentString, int maxLineLength)
        {
            this._baseWriter = writer;
            this._maxLineLength = maxLineLength;
            this._writer = new FormattedTextWriter(this._baseWriter, indentString);
        }

        public void Flush()
        {
            this._writer.Flush();
        }

        public virtual void Write(char c)
        {
            this._writer.Write(c);
        }

        public virtual void Write(string s)
        {
            this._writer.Write(s);
        }

        public virtual void WriteLineIfNotOnNewLine()
        {
            this._writer.WriteLineIfNotOnNewLine();
        }

        public virtual void WriteLiteral(string s, bool frontWhiteSpace)
        {
            this._writer.WriteLiteralWrapped(FormattedTextWriter.Trim(s, frontWhiteSpace), this._maxLineLength);
        }

        protected TextWriter BaseWriter
        {
            get
            {
                return this._baseWriter;
            }
        }

        public virtual string Content
        {
            get
            {
                this._writer.Flush();
                return this._baseWriter.ToString();
            }
        }

        public int Indent
        {
            get
            {
                return this._writer.Indent;
            }
            set
            {
                this._writer.Indent = value;
            }
        }

        public FormattedTextWriter Writer
        {
            get
            {
                return this._writer;
            }
        }
    }
}

