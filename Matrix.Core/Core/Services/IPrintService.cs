namespace Microsoft.Matrix.Core.Services
{
    using Microsoft.Matrix.Core.Documents;
    using System;

    public interface IPrintService
    {
        void ConfigurePrintSettings();
        void PreviewDocument(IPrintableDocument document);
        void PrintDocument(IPrintableDocument document);
    }
}

