namespace Microsoft.Matrix.Packages.Web.Utility
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents.Code;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Drawing;

    internal sealed class AspNetColorizer : ITextColorizer, IDisposable
    {
        private ITextColorizer _codeColorizer;
        private bool _codeColorizerQueried;
        private IServiceProvider _serviceProvider;
        private static ColorInfo[] aspNetColors = new ColorInfo[] { new ColorInfo(Color.Black, Color.Yellow, false, false) };
        public const byte Directive = 0x40;
        private const int DirectiveState = 1;

        public AspNetColorizer(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        private void Dispose()
        {
            if (this._codeColorizer != null)
            {
                this._codeColorizer.Dispose();
                this._codeColorizer = null;
            }
            this._serviceProvider = null;
        }

        private void Fill(byte[] data, int startIndex, int length, byte value)
        {
            for (int i = startIndex; i < (startIndex + length); i++)
            {
                data[i] = value;
            }
        }

        int ITextColorizer.Colorize(char[] text, byte[] colors, int startIndex, int endIndex, int initialColorState)
        {
            if ((text == null) || (text.Length == 0))
            {
                return initialColorState;
            }
            AspNetToken token = AspNetTokenizer.GetNextToken(text, 0, endIndex, initialColorState);
            int endState = 0;
            while (token != null)
            {
                if (token.type == AspNetToken.Directive)
                {
                    this.Fill(colors, token.startIndex, token.endIndex - token.startIndex, 0x40);
                }
                else if (token.type == AspNetToken.Whitespace)
                {
                    this.Fill(colors, token.startIndex, token.endIndex - token.startIndex, 0);
                }
                else
                {
                    endState = token.endState;
                    if (this.CodeColorizer != null)
                    {
                        int num2 = this.CodeColorizer.Colorize(text, colors, token.startIndex, token.endIndex, (initialColorState >> 0x10) & 0xffff);
                        endState |= num2 << 0x10;
                    }
                    else
                    {
                        this.Fill(colors, token.startIndex, token.endIndex - token.startIndex, 0);
                    }
                }
                token = AspNetTokenizer.GetNextToken(text, endIndex, token);
            }
            return endState;
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        private ITextColorizer CodeColorizer
        {
            get
            {
                if ((this._codeColorizer == null) && !this._codeColorizerQueried)
                {
                    this._codeColorizerQueried = true;
                    IDocumentDesignerHost service = this._serviceProvider.GetService(typeof(IDocumentDesignerHost)) as IDocumentDesignerHost;
                    IDocumentWithCode document = service.Document as IDocumentWithCode;
                    if (document.Code != null)
                    {
                        ITextLanguage language = document.Code.Language;
                        if (language != null)
                        {
                            this._codeColorizer = language.GetColorizer(null);
                        }
                    }
                }
                return this._codeColorizer;
            }
        }

        private ColorInfo[] ColorTable
        {
            get
            {
                return aspNetColors;
            }
        }

        ColorInfo[] ITextColorizer.ColorTable
        {
            get
            {
                return this.ColorTable;
            }
        }

        private class AspNetToken
        {
            public static int Directive = 1;
            public int endIndex;
            public int endState;
            public static int NonDirective = 0;
            public int startIndex;
            public int type;
            public static int Whitespace = 2;

            public AspNetToken(int t, int start, int end, int state)
            {
                this.type = t;
                this.startIndex = start;
                this.endIndex = end;
                this.endState = state;
            }
        }

        private class AspNetTokenizer
        {
            public static int Directive = 1;
            public static int DirectiveWhitespace = 0;
            public static int NonDirective = 2;

            public static AspNetColorizer.AspNetToken GetNextToken(char[] text, int endIndex, AspNetColorizer.AspNetToken token)
            {
                return GetNextToken(text, token.endIndex, endIndex, token.endState);
            }

            public static AspNetColorizer.AspNetToken GetNextToken(char[] text, int startIndex, int endIndex, int startState)
            {
                if (startIndex >= endIndex)
                {
                    return null;
                }
                if (startState == DirectiveWhitespace)
                {
                    int index = startIndex;
                    while (index < endIndex)
                    {
                        if (char.IsWhiteSpace(text[index]))
                        {
                            index++;
                        }
                        else
                        {
                            if (text[index] == '<')
                            {
                                return new AspNetColorizer.AspNetToken(AspNetColorizer.AspNetToken.Whitespace, startIndex, index, Directive);
                            }
                            return new AspNetColorizer.AspNetToken(AspNetColorizer.AspNetToken.Whitespace, startIndex, index, NonDirective);
                        }
                    }
                    return new AspNetColorizer.AspNetToken(AspNetColorizer.AspNetToken.Whitespace, startIndex, index, DirectiveWhitespace);
                }
                if (startState != Directive)
                {
                    return new AspNetColorizer.AspNetToken(AspNetColorizer.AspNetToken.NonDirective, startIndex, endIndex, NonDirective);
                }
                for (int i = startIndex; (i + 1) < endIndex; i++)
                {
                    if ((text[i] == '%') && (text[i + 1] == '>'))
                    {
                        return new AspNetColorizer.AspNetToken(AspNetColorizer.AspNetToken.Directive, startIndex, i + 2, DirectiveWhitespace);
                    }
                }
                return new AspNetColorizer.AspNetToken(AspNetColorizer.AspNetToken.Directive, startIndex, endIndex, Directive);
            }
        }
    }
}

