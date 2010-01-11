namespace Microsoft.Matrix.Packages.Web.Utility.Formatting
{
    using System;
    using System.Text;
    using System.IO;

    internal sealed class XmlWriter : HtmlWriter
    {
        private bool _containsText;
        private bool _isUnknownXml;
        private string _tagName;
        private StringBuilder _unformatted;

        public XmlWriter(int initialIndent, string tagName, string indentString, int maxLineLength) : base(new StringWriter(), indentString, maxLineLength)
        {
            base.Writer.Indent = initialIndent;
            this._unformatted = new StringBuilder();
            this._tagName = tagName;
            this._isUnknownXml = this._tagName.IndexOf(':') > -1;
        }

        public override void Write(char c)
        {
            base.Write(c);
            this._unformatted.Append(c);
        }

        public override void Write(string s)
        {
            base.Write(s);
            this._unformatted.Append(s);
        }

        public override void WriteLiteral(string s, bool frontWhiteSpace)
        {
            base.WriteLiteral(s, frontWhiteSpace);
            this._unformatted.Append(s);
        }

        public bool ContainsText
        {
            get
            {
                return this._containsText;
            }
            set
            {
                this._containsText = value;
            }
        }

        public override string Content
        {
            get
            {
                if (this.ContainsText)
                {
                    return this._unformatted.ToString();
                }
                return base.Content;
            }
        }

        public bool IsUnknownXml
        {
            get
            {
                return this._isUnknownXml;
            }
        }

        public string TagName
        {
            get
            {
                return this._tagName;
            }
        }
    }
}

