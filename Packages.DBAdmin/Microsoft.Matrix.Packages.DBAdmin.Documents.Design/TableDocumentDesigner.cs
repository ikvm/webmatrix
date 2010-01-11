namespace Microsoft.Matrix.Packages.DBAdmin.Documents.Design
{
    using Microsoft.Matrix.Core.Documents.Design;
    using Microsoft.Matrix.Packages.DBAdmin.Documents;
    using System;
    using System.ComponentModel;

    public class TableDocumentDesigner : DocumentDesigner
    {
        private TableDocument tableDocument;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            this.tableDocument = (TableDocument) component;
        }
    }
}

