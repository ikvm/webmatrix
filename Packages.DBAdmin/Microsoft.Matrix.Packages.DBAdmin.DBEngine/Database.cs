namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine
{
    using System;

    public abstract class Database : IDisposable
    {
        private int _connectionCount;
        private StoredProcedureCollection _storedProcedureCollection;
        private TableCollection _tableCollection;

        protected Database()
        {
        }

        public void Connect()
        {
            if (this._connectionCount == 0)
            {
                this.ConnectInternal();
            }
            this._connectionCount++;
        }

        protected abstract void ConnectInternal();
        public abstract Column CreateColumn();
        protected virtual StoredProcedureCollection CreateStoredProcedureCollection()
        {
            throw new NotSupportedException();
        }

        protected abstract TableCollection CreateTableCollection();
        public void Disconnect()
        {
            if (this._connectionCount != 0)
            {
                this._connectionCount--;
                if (this._connectionCount == 0)
                {
                    this.DisconnectInternal();
                }
            }
        }

        protected abstract void DisconnectInternal();
        protected void Dispose()
        {
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        public bool Connected
        {
            get
            {
                return (this._connectionCount > 0);
            }
        }

        public abstract string DisplayName { get; }

        public StoredProcedureCollection StoredProcedures
        {
            get
            {
                if (!this.SupportsStoredProcedures)
                {
                    throw new NotSupportedException();
                }
                if (this._storedProcedureCollection == null)
                {
                    this._storedProcedureCollection = this.CreateStoredProcedureCollection();
                }
                return this._storedProcedureCollection;
            }
        }

        public virtual bool SupportsStoredProcedures
        {
            get
            {
                return false;
            }
        }

        public TableCollection Tables
        {
            get
            {
                if (this._tableCollection == null)
                {
                    this._tableCollection = this.CreateTableCollection();
                }
                return this._tableCollection;
            }
        }
    }
}

