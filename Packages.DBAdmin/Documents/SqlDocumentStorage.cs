namespace Microsoft.Matrix.Packages.DBAdmin.Documents
{
    using Microsoft.Matrix.Core.Documents.Text;
    using System;

    internal sealed class SqlDocumentStorage : TextDocumentStorage
    {
        public SqlDocumentStorage(SqlDocument owner) : base(owner)
        {
        }
    }
}

