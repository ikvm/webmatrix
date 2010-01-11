namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using System;

    public class CodeDocumentWindow : TextDocumentWindow
    {
        public CodeDocumentWindow(IServiceProvider serviceProvider, Document document) : base(serviceProvider, document)
        {
        }

        protected override SourceView CreateSourceView()
        {
            return new CodeView(base.ServiceProvider);
        }

        protected override void DisposeDocumentView()
        {
            ((CodeView) base.DocumentView).Dispose();
        }
    }
}

