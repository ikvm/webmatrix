namespace Microsoft.Matrix.Core.Designer
{
    using Microsoft.Matrix.Core.Documents;
    using System;

    public interface IDocumentDesignerHost
    {
        void BeginLoad();
        void EndLoad();

        bool DesignerActive { get; set; }

        Microsoft.Matrix.Core.Documents.Document Document { get; }
    }
}

