namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Documents.Text;
    using System;

    public class HtmlDocumentStorage : TextDocumentStorage
    {
        public HtmlDocumentStorage(HtmlDocument owner) : base(owner)
        {
        }
    }
}

