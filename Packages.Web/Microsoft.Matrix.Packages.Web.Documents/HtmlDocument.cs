namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.Web.Services;
    using System;

    public class HtmlDocument : TextDocument, IRunnableDocument
    {
        public HtmlDocument(DocumentProjectItem projectItem) : base(projectItem)
        {
        }

        protected override IDocumentStorage CreateStorage()
        {
            return new HtmlDocumentStorage(this);
        }

        void IRunnableDocument.Run()
        {
            this.Run();
        }

        protected virtual void Run()
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

        public override DocumentLanguage Language
        {
            get
            {
                return HtmlDocumentLanguage.Instance;
            }
        }

        bool IRunnableDocument.CanRun
        {
            get
            {
                return this.CanRun;
            }
        }
    }
}

