namespace Microsoft.Matrix.Packages.DBAdmin.Documents
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using System;
    using System.IO;

    internal sealed class StoredProcedureDocumentStorage : TextDocumentStorage, IDocumentStorage, IDisposable
    {
        private DateTime _creationDate;
        private string _name;
        private StoredProcedureDocument _owner;

        public StoredProcedureDocumentStorage(StoredProcedureDocument owner) : base(owner)
        {
            this._owner = owner;
        }

        void IDocumentStorage.Load(Stream contentStream)
        {
            StoredProcedure storedProcedure = ((StoredProcedureProjectItem) this._owner.ProjectItem).GetStoredProcedure();
            base.Text = storedProcedure.CommandText;
            this._creationDate = storedProcedure.CreationDate;
            this._name = storedProcedure.Name;
        }

        void IDocumentStorage.Save(Stream contentStream)
        {
        }

        void IDisposable.Dispose()
        {
            this._owner = null;
        }

        public DateTime CreationDate
        {
            get
            {
                return this._creationDate;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }
    }
}

