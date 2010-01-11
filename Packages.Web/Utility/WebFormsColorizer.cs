namespace Microsoft.Matrix.Packages.Web.Utility
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents.Code;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;

    internal sealed class WebFormsColorizer : HtmlColorizer
    {
        private ITextColorizer _serverScriptColorizer;
        private bool _serverScriptColorizerQueried;
        private IServiceProvider _serviceProvider;
        public const byte ServerSide = 0x43;

        public WebFormsColorizer(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        protected override int ColorizeToken(Token t, char[] text, byte[] colors, int colorState)
        {
            int endState = t.EndState;
            int startIndex = t.StartIndex;
            int endIndex = t.EndIndex;
            if ((t.Type == 0x17) && (this.ServerScriptColorizer != null))
            {
                int num4 = this.ServerScriptColorizer.Colorize(text, colors, startIndex, endIndex, (colorState >> 0x10) & 0xffff);
                return (endState | (num4 << 0x10));
            }
            if (t.Type == 0x16)
            {
                base.Fill(colors, startIndex, endIndex - startIndex, 0x43);
                return endState;
            }
            return base.ColorizeToken(t, text, colors, colorState);
        }

        protected override void Dispose()
        {
            if (this._serverScriptColorizer != null)
            {
                this._serverScriptColorizer.Dispose();
                this._serverScriptColorizer = null;
                this._serverScriptColorizerQueried = false;
            }
            base.Dispose();
        }

        private ITextColorizer ServerScriptColorizer
        {
            get
            {
                if ((this._serverScriptColorizer == null) && !this._serverScriptColorizerQueried)
                {
                    this._serverScriptColorizerQueried = true;
                    IDocumentDesignerHost service = (IDocumentDesignerHost) this._serviceProvider.GetService(typeof(IDocumentDesignerHost));
                    IDocumentWithCode document = service.Document as IDocumentWithCode;
                    if (document.Code != null)
                    {
                        ITextLanguage language = document.Code.Language;
                        if (language != null)
                        {
                            this._serverScriptColorizer = language.GetColorizer(null);
                        }
                    }
                }
                return this._serverScriptColorizer;
            }
        }
    }
}

