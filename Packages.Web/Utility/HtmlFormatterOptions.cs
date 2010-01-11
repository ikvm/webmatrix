namespace Microsoft.Matrix.Packages.Web.Utility
{
    using System;

    internal sealed class HtmlFormatterOptions
    {
        private HtmlFormatterCase _attributeCasing;
        private HtmlFormatterCase _elementCasing;
        private char _indentChar;
        private int _indentSize;
        private bool _makeXhtml;
        private int _maxLineLength;

        public HtmlFormatterOptions(char indentChar, int indentSize, int maxLineLength, bool makeXhtml) : this(indentChar, indentSize, maxLineLength, HtmlFormatterCase.LowerCase, HtmlFormatterCase.LowerCase, makeXhtml)
        {
        }

        public HtmlFormatterOptions(char indentChar, int indentSize, int maxLineLength, HtmlFormatterCase elementCasing, HtmlFormatterCase attributeCasing, bool makeXhtml)
        {
            this._indentChar = indentChar;
            this._indentSize = indentSize;
            this._maxLineLength = maxLineLength;
            this._elementCasing = elementCasing;
            this._attributeCasing = attributeCasing;
            this._makeXhtml = makeXhtml;
        }

        public HtmlFormatterCase AttributeCasing
        {
            get
            {
                return this._attributeCasing;
            }
        }

        public HtmlFormatterCase ElementCasing
        {
            get
            {
                return this._elementCasing;
            }
        }

        public char IndentChar
        {
            get
            {
                return this._indentChar;
            }
        }

        public int IndentSize
        {
            get
            {
                return this._indentSize;
            }
        }

        public bool MakeXhtml
        {
            get
            {
                return this._makeXhtml;
            }
        }

        public int MaxLineLength
        {
            get
            {
                return this._maxLineLength;
            }
        }
    }
}

