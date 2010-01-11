namespace Microsoft.Matrix.Packages.DBAdmin.DBEngine
{
    using System;

    public abstract class StoredProcedure : DBObject
    {
        private string _commandText;
        private DateTime _creationDate;

        public StoredProcedure(Database database) : base(database)
        {
        }

        public abstract bool Delete();
        public abstract void Refresh();
        public abstract bool Rename(string newName);
        protected void SetCommandText(string commandText)
        {
            this._commandText = commandText;
        }

        protected void SetCreationDate(DateTime creationDate)
        {
            this._creationDate = creationDate;
        }

        public abstract bool Update(string commandText);

        public string CommandText
        {
            get
            {
                return this._commandText;
            }
        }

        public virtual DateTime CreationDate
        {
            get
            {
                return this._creationDate;
            }
        }
    }
}

