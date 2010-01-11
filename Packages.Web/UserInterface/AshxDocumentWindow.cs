namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using System;

    public class AshxDocumentWindow : AspNetDocumentWindow
    {
        public AshxDocumentWindow(IServiceProvider provider, Document document) : base(provider, document)
        {
        }
    }
}

