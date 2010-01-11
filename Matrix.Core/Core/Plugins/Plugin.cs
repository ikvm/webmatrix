namespace Microsoft.Matrix.Core.Plugins
{
    using System;

    public abstract class Plugin : IDisposable
    {
        private IServiceProvider _serviceProvider;

        protected Plugin()
        {
        }

        protected virtual void Dispose()
        {
            this._serviceProvider = null;
        }

        protected object GetService(Type serviceType)
        {
            if (this._serviceProvider != null)
            {
                return this._serviceProvider.GetService(serviceType);
            }
            return null;
        }

        protected abstract object RunPlugin(object initializationData);
        internal object RunPlugin(IServiceProvider serviceProvider, object initializationData)
        {
            this._serviceProvider = serviceProvider;
            return this.RunPlugin(initializationData);
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        protected internal IServiceProvider ServiceProvider
        {
            get
            {
                return this._serviceProvider;
            }
        }
    }
}

