namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Documents.Code;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.ComponentModel;

    public class CodeBehindView : CodeView
    {
        public CodeBehindView(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override ISite CreateTextControlSite()
        {
            return new CodeBehindViewTextControlSite(this);
        }

        protected override void LoadDocument()
        {
            IDocumentWithCode document = base.Document as IDocumentWithCode;
            if (document != null)
            {
                CodeDocumentStorage code = document.Code;
                if (code != null)
                {
                    this.Text = code.Text;
                }
            }
        }

        protected override bool SaveToDocument()
        {
            IDocumentWithCode document = base.Document as IDocumentWithCode;
            if (document != null)
            {
                CodeDocumentStorage code = document.Code;
                if (code != null)
                {
                    code.Text = this.Text;
                }
            }
            return true;
        }

        private sealed class CodeBehindViewTextControlSite : ISite, IServiceProvider
        {
            private CodeBehindView _owner;

            public CodeBehindViewTextControlSite(CodeBehindView owner)
            {
                this._owner = owner;
            }

            public object GetService(Type type)
            {
                if (type == typeof(ITextLanguage))
                {
                    IDocumentWithCode document = this._owner.Document as IDocumentWithCode;
                    if (document != null)
                    {
                        CodeDocumentStorage code = document.Code;
                        if (code != null)
                        {
                            return code.Language;
                        }
                    }
                }
                return this._owner.ServiceProvider.GetService(type);
            }

            public IComponent Component
            {
                get
                {
                    return null;
                }
            }

            public IContainer Container
            {
                get
                {
                    return null;
                }
            }

            public bool DesignMode
            {
                get
                {
                    return false;
                }
            }

            public string Name
            {
                get
                {
                    return "SourceViewTextControlSite";
                }
                set
                {
                }
            }
        }
    }
}

