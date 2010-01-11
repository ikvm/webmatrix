namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using System;

    public class AspNetDocumentWindow : TextDocumentWindow
    {
        public AspNetDocumentWindow(IServiceProvider provider, Document document) : base(provider, document)
        {
        }

        protected override SourceView CreateSourceView()
        {
            return new AspNetSourceView(base.ServiceProvider);
        }
    }
}

