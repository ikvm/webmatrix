namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Documents.Code;
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.Web.Services;
    using Microsoft.Matrix.Packages.Web.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;

    public abstract class AspNetDocument : TextDocument, IDocumentWithCode, IRunnableDocument
    {
        private CodeDocumentStorage _codeStorage;
        private Microsoft.Matrix.Packages.Web.Documents.DocumentDirective _documentDirective;
        private ArrayList _miscDirectives;

        public AspNetDocument(DocumentProjectItem projectItem) : base(projectItem)
        {
        }

        protected abstract Microsoft.Matrix.Packages.Web.Documents.DocumentDirective CreateDocumentDirective();
        void IRunnableDocument.Run()
        {
            this.Run();
        }

        protected override void OnAfterLoad(EventArgs e)
        {
            DirectiveParser parser = new DirectiveParser(this.Text, this.DocumentDirectiveName);
            foreach (Directive directive in parser.Directives)
            {
                if (directive is Microsoft.Matrix.Packages.Web.Documents.DocumentDirective)
                {
                    this._documentDirective = (Microsoft.Matrix.Packages.Web.Documents.DocumentDirective) directive;
                }
                else
                {
                    this.MiscDirectives.Add(directive);
                }
            }
            if (this._documentDirective == null)
            {
                this._documentDirective = this.CreateDocumentDirective();
            }
            CodeDocumentLanguage documentLanguage = null;
            ILanguageManager service = (ILanguageManager) this.GetService(typeof(ILanguageManager));
            if (service != null)
            {
                string name = this.DocumentDirective.Language;
                if ((name != null) && (name.Length != 0))
                {
                    documentLanguage = service.GetDocumentLanguage(name) as CodeDocumentLanguage;
                }
                if (documentLanguage == null)
                {
                    documentLanguage = (CodeDocumentLanguage) service.DefaultCodeLanguage;
                    this.DocumentDirective.Language = documentLanguage.Name;
                }
            }
            this._codeStorage = new CodeDocumentStorage(documentLanguage);
            this._codeStorage.Text = this.Text.Substring(parser.DirectiveEndIndex);
            base.OnAfterLoad(e);
        }

        public virtual void Run()
        {
            IWebDocumentRunService service = (IWebDocumentRunService) this.GetService(typeof(IWebDocumentRunService));
            if (service != null)
            {
                service.Run(this);
            }
        }

        protected virtual bool CanRun
        {
            get
            {
                return true;
            }
        }

        [Browsable(false)]
        public Microsoft.Matrix.Packages.Web.Documents.DocumentDirective DocumentDirective
        {
            get
            {
                if (this._documentDirective == null)
                {
                    this._documentDirective = this.CreateDocumentDirective();
                }
                return this._documentDirective;
            }
        }

        protected abstract string DocumentDirectiveName { get; }

        public override DocumentLanguage Language
        {
            get
            {
                return AspNetDocumentLanguage.Instance;
            }
        }

        CodeDocumentStorage IDocumentWithCode.Code
        {
            get
            {
                return this._codeStorage;
            }
        }

        bool IRunnableDocument.CanRun
        {
            get
            {
                return this.CanRun;
            }
        }

        private ArrayList MiscDirectives
        {
            get
            {
                if (this._miscDirectives == null)
                {
                    this._miscDirectives = new ArrayList();
                }
                return this._miscDirectives;
            }
        }
    }
}

