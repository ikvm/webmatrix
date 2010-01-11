namespace Microsoft.Matrix.Packages.DBAdmin.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using System;

    internal sealed class StoredProcedureDocumentWindow : TextDocumentWindow
    {
        public StoredProcedureDocumentWindow(IServiceProvider designerHost, Document document) : base(designerHost, document)
        {
        }

        protected override SourceView CreateSourceView()
        {
            return new StoredProcedureSourceView(base.ServiceProvider);
        }
    }
}

