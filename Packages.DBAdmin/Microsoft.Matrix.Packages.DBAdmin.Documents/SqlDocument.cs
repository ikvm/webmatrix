namespace Microsoft.Matrix.Packages.DBAdmin.Documents
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Core.Projects;
    using System;

    public sealed class SqlDocument : TextDocument
    {
        public SqlDocument(DocumentProjectItem projectItem) : base(projectItem)
        {
        }

        protected override IDocumentStorage CreateStorage()
        {
            return new SqlDocumentStorage(this);
        }

        public override DocumentLanguage Language
        {
            get
            {
                return SqlLanguage.Instance;
            }
        }
    }
}

