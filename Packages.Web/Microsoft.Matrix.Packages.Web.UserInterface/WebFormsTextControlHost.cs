namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents.Code;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Collections;

    internal sealed class WebFormsTextControlHost : ITextControlHost, IDisposable
    {
        private ITextControlHost _codeTextControlHost;
        private TextControl _control;
        private ArrayList _handlerList;
        private IServiceProvider _serviceProvider;

        public WebFormsTextControlHost(TextControl control, IServiceProvider provider)
        {
            this._serviceProvider = provider;
            this._control = control;
        }

        public void Dispose()
        {
            if (this._handlerList != null)
            {
                for (int i = this._handlerList.Count - 1; i >= 0; i--)
                {
                    ((IDisposable) this._handlerList[i]).Dispose();
                }
                this._handlerList.Clear();
            }
            if (this._codeTextControlHost != null)
            {
                this._codeTextControlHost.Dispose();
            }
            this._control = null;
            this._serviceProvider = null;
        }

        public void OnTextViewCreated(TextView view)
        {
            if (this._control is CodeBehindView)
            {
                if (this._codeTextControlHost == null)
                {
                    IDocumentDesignerHost service = this._serviceProvider.GetService(typeof(IDocumentDesignerHost)) as IDocumentDesignerHost;
                    IDocumentWithCode document = service.Document as IDocumentWithCode;
                    if (document.Code != null)
                    {
                        ITextLanguage language = document.Code.Language;
                        this._codeTextControlHost = language.GetTextControlHost(this._control, this._serviceProvider);
                    }
                }
                if (this._codeTextControlHost != null)
                {
                    this._codeTextControlHost.OnTextViewCreated(view);
                }
            }
            else if (this._control is WebFormsSourceView)
            {
                this.HandlerList.Add(new BlockIndentTextViewCommandHandler(view));
                this.HandlerList.Add(new WebFormsTextViewCommandHandler(view));
            }
            else if (this._control is WebFormsAllView)
            {
                this.HandlerList.Add(new BlockIndentTextViewCommandHandler(view));
            }
        }

        private ArrayList HandlerList
        {
            get
            {
                if (this._handlerList == null)
                {
                    this._handlerList = new ArrayList();
                }
                return this._handlerList;
            }
        }
    }
}

