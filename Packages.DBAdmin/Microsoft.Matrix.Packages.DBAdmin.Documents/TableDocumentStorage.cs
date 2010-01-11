namespace Microsoft.Matrix.Packages.DBAdmin.Documents
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using System;
    using System.IO;

    internal sealed class TableDocumentStorage : IDocumentStorage, IDisposable
    {
        private TableDocument _owner;
        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table _table;

        public TableDocumentStorage(TableDocument owner)
        {
            this._owner = owner;
        }

        void IDocumentStorage.Load(Stream contentStream)
        {
            string caption = this._owner.ProjectItem.Caption;
            Database database = ((DatabaseProject) this._owner.ProjectItem.Project).Database;
            this._table = database.Tables[caption];
        }

        void IDocumentStorage.Save(Stream contentStream)
        {
        }

        void IDisposable.Dispose()
        {
            this._owner = null;
        }

        public Microsoft.Matrix.Packages.DBAdmin.DBEngine.Table Table
        {
            get
            {
                return this._table;
            }
        }
    }
}

