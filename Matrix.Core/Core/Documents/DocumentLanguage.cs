namespace Microsoft.Matrix.Core.Documents
{
    using System;

    public abstract class DocumentLanguage : IDisposable
    {
        private string _name;
        private IServiceProvider _serviceProvider;

        protected DocumentLanguage(string name)
        {
            if ((name == null) || (name.Length == 0))
            {
                throw new ArgumentException();
            }
            this._name = name;
        }

        protected virtual void Dispose()
        {
            this._serviceProvider = null;
        }

        public virtual void Initialize(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        public override string ToString()
        {
            return this.DisplayName;
        }

        public virtual string DisplayName
        {
            get
            {
                return this.Name;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        protected IServiceProvider ServiceProvider
        {
            get
            {
                return this._serviceProvider;
            }
        }
    }
}

