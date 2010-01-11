namespace Microsoft.Matrix.Core.Documents
{
    using System.Drawing.Printing;

    public interface IPrintableDocument
    {
        PrintDocument CreatePrintDocument();
    }
}

