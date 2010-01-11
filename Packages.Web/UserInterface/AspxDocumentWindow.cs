namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using System;

    public class AspxDocumentWindow : WebFormsDocumentWindow
    {
        public AspxDocumentWindow(IServiceProvider provider, Document document, DocumentViewType initialView) : base(provider, document, initialView)
        {
        }
    }
}

