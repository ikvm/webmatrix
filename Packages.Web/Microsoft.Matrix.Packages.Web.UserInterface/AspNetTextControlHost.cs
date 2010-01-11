namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents.Code;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;

    internal sealed class AspNetTextControlHost : ITextControlHost, IDisposable
    {
        private ITextControlHost _codeTextControlHost;
        private TextControl _control;
        private IServiceProvider _serviceProvider;

        public AspNetTextControlHost(TextControl control, IServiceProvider provider)
        {
            this._serviceProvider = provider;
            this._control = control;
        }

        public void Dispose()
        {
            if (this._codeTextControlHost != null)
            {
                this._codeTextControlHost.Dispose();
            }
            this._control = null;
            this._serviceProvider = null;
        }

        public void OnTextViewCreated(TextView view)
        {
            if (this._codeTextControlHost == null)
            {
                IDocumentDesignerHost service = this._serviceProvider.GetService(typeof(IDocumentDesignerHost)) as IDocumentDesignerHost;
                IDocumentWithCode document = service.Document as IDocumentWithCode;
                if (document.Code != null)
                {
                    ITextLanguage language = document.Code.Language;
                    if (language != null)
                    {
                        this._codeTextControlHost = language.GetTextControlHost(this._control, this._serviceProvider);
                    }
                }
            }
            if (this._codeTextControlHost != null)
            {
                this._codeTextControlHost.OnTextViewCreated(view);
            }
        }
    }
}

