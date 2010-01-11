namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine
{
    using System;

    public abstract class DBObject : IDisposable
    {
        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database _database;
        private string _name;

        protected DBObject(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database database)
        {
            this._database = database;
        }

        protected void Dispose(bool disposing)
        {
        }

        protected void SetName(string name)
        {
            this._name = name;
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
        }

        public Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database Database
        {
            get
            {
                return this._database;
            }
        }

        public virtual string Name
        {
            get
            {
                return this._name;
            }
        }
    }
}

