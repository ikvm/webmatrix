namespace Microsoft.Matrix.Packages.Web.Utility
{
    using System;

    internal sealed class Token
    {
        private char[] _chars;
        private int _charsLength;
        private int _endIndex;
        private int _endState;
        private int _startIndex;
        private string _text;
        private int _type;

        public const int Whitespace          = 0x00;
        public const int TagName             = 0x01;
        public const int AttrName            = 0x02;
        public const int AttrVal             = 0x03;
        public const int TextToken           = 0x04;
        public const int SelfTerminating     = 0x05;
        public const int Empty               = 0x06;
        public const int Comment             = 0x07;
        public const int Error               = 0x08;
        public const int OpenBracket         = 0x0A;
        public const int CloseBracket        = 0x0B;
        public const int ForwardSlash        = 0x0C;
        public const int DoubleQuote         = 0x0D;
        public const int SingleQuote         = 0x0E;
        public const int EqualsChar          = 0x0F;
        public const int ClientScriptBlock   = 0x14;
        public const int Style               = 0x15;
        public const int InlineServerScript  = 0x16;
        public const int ServerScriptBlock   = 0x17;
        public const int XmlDirective        = 0x18;

        public Token(int type, int endState, int startIndex, int endIndex, char[] chars, int charsLength)
        {
            this._type = type;
            this._chars = chars;
            this._charsLength = charsLength;
            this._startIndex = startIndex;
            this._endIndex = endIndex;
            this._endState = endState;
        }

        internal char[] Chars
        {
            get
            {
                return this._chars;
            }
        }

        internal int CharsLength
        {
            get
            {
                return this._charsLength;
            }
        }

        public int EndIndex
        {
            get
            {
                return this._endIndex;
            }
        }

        public int EndState
        {
            get
            {
                return this._endState;
            }
        }

        public int Length
        {
            get
            {
                return (this._endIndex - this._startIndex);
            }
        }

        public int StartIndex
        {
            get
            {
                return this._startIndex;
            }
        }

        public string Text
        {
            get
            {
                if (this._text == null)
                {
                    this._text = new string(this._chars, this.StartIndex, this.EndIndex - this.StartIndex);
                }
                return this._text;
            }
        }

        public int Type
        {
            get
            {
                return this._type;
            }
        }
    }
}

