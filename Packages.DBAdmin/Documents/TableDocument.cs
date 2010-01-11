namespace Microsoft.Matrix.Packages.DBAdmin.Documents
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.Packages.DBAdmin.Documents.Design;
    using System;
    using System.ComponentModel;

    [Designer(typeof(TableDocumentDesigner))]
    public sealed class TableDocument : Document
    {
        public TableDocument(DocumentProjectItem projectItem) : base(projectItem)
        {
        }

        protected override IDocumentStorage CreateStorage()
        {
            return new TableDocumentStorage(this);
        }

        [Category("Table")]
        public DateTime CreationDate
        {
            get
            {
                return ((TableDocumentStorage) base.Storage).Table.CreationDate;
            }
        }

        [Category("Table")]
        public DateTime LastModifiedDate
        {
            get
            {
                return ((TableDocumentStorage) base.Storage).Table.LastModifiedDate;
            }
        }

        [Category("Table")]
        public string Name
        {
            get
            {
                return ((TableDocumentStorage) base.Storage).Table.Name;
            }
        }

        [Browsable(false)]
        public Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table Table
        {
            get
            {
                return ((TableDocumentStorage) base.Storage).Table;
            }
        }
    }
}

