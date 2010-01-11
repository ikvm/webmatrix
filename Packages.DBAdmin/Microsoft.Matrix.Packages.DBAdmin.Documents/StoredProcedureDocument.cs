namespace Microsoft.Matrix.Packages.DBAdmin.Documents
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.Documents.Design;
    using System;
    using System.ComponentModel;

    [Designer(typeof(StoredProcedureDocumentDesigner))]
    public sealed class StoredProcedureDocument : TextDocument
    {
        public StoredProcedureDocument(DocumentProjectItem projectItem) : base(projectItem)
        {
        }

        protected override IDocumentStorage CreateStorage()
        {
            return new StoredProcedureDocumentStorage(this);
        }

        [Category("Stored Procedure")]
        public DateTime CreationDate
        {
            get
            {
                return ((StoredProcedureDocumentStorage) base.Storage).CreationDate;
            }
        }

        [Category("Stored Procedure")]
        public string Name
        {
            get
            {
                return ((StoredProcedureDocumentStorage) base.Storage).Name;
            }
        }
    }
}

