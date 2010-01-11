namespace Microsoft.Matrix.Core.Documents.Text
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.ComponentModel;
    using System.Drawing.Printing;

    public class TextDocument : Document, IPrintableDocument
    {
        public TextDocument(DocumentProjectItem projectItem) : base(projectItem)
        {
        }

        protected virtual PrintDocument CreatePrintDocument()
        {
            return new TextPrintDocument(this);
        }

        protected override IDocumentStorage CreateStorage()
        {
            return new TextDocumentStorage(this);
        }

        PrintDocument IPrintableDocument.CreatePrintDocument()
        {
            return this.CreatePrintDocument();
        }

        public override DocumentLanguage Language
        {
            get
            {
                return TextDocumentLanguage.Instance;
            }
        }

        [Browsable(false)]
        public virtual string Text
        {
            get
            {
                return ((TextDocumentStorage) base.Storage).Text;
            }
            set
            {
                ((TextDocumentStorage) base.Storage).Text = value;
            }
        }
    }
}

