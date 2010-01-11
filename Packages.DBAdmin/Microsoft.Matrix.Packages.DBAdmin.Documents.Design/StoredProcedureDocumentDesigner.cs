namespace Microsoft.Matrix.Packages.DBAdmin.Documents.Design
{
    using Microsoft.Matrix.Core.Documents.Text.Design;
    using Microsoft.Matrix.Packages.DBAdmin.Documents;
    using System;
    using System.ComponentModel;

  
    public class StoredProcedureDocumentDesigner : TextDocumentDesigner
    {
        private StoredProcedureDocument sprocDocument;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            this.sprocDocument = (StoredProcedureDocument) component;
        }
    }
}

