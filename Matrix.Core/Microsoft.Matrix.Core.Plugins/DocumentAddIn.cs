namespace Microsoft.Matrix.Core.Plugins
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using System;
    using System.ComponentModel.Design;

    public abstract class DocumentAddIn : AddIn
    {
        private Microsoft.Matrix.Core.Documents.Document _document;

        protected DocumentAddIn()
        {
        }

        protected Microsoft.Matrix.Core.Documents.Document Document
        {
            get
            {
                if (this._document == null)
                {
                    IDesignerHost service = (IDesignerHost) base.GetService(typeof(IDesignerHost));
                    if (service != null)
                    {
                        this._document = ((IDocumentDesignerHost) service).Document;
                    }
                }
                return this._document;
            }
        }
    }
}

