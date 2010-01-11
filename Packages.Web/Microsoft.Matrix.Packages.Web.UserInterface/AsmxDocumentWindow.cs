namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using System;

    public class AsmxDocumentWindow : AspNetDocumentWindow
    {
        public AsmxDocumentWindow(IServiceProvider provider, Document document) : base(provider, document)
        {
        }
    }
}

