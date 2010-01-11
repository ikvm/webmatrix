namespace Microsoft.Matrix.Core.Packages
{
    using System;
    using System.Collections;
    using System.Configuration;

    internal sealed class PackageManager : IPackageManager, IDisposable
    {
        private Hashtable _packageTable;
        private IServiceProvider _serviceProvider;

        public PackageManager(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        private void LoadPackage(string packageTypeName)
        {
            Type type = Type.GetType(packageTypeName, true);
            IPackage package = (IPackage) Activator.CreateInstance(type);
            package.Initialize(this._serviceProvider);
            this._packageTable.Add(type, package);
        }

        public void LoadPackages(IPackage appPackage)
        {
            this._packageTable = new Hashtable();
            if (appPackage != null)
            {
                appPackage.Initialize(this._serviceProvider);
                this._packageTable.Add(typeof(IApplicationPackage), appPackage);
            }
            IPackage package = new CorePackage();
            package.Initialize(this._serviceProvider);
            this._packageTable.Add(typeof(CorePackage), package);
            IDictionary config = (IDictionary) ConfigurationSettings.GetConfig("microsoft.matrix/packages");
            if (config != null)
            {
                IDictionaryEnumerator enumerator = config.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    string packageTypeName = (string) current.Value;
                    try
                    {
                        this.LoadPackage(packageTypeName);
                        continue;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        IPackage IPackageManager.GetPackage(Type packageType)
        {
            if (this._packageTable != null)
            {
                return (IPackage) this._packageTable[packageType];
            }
            return null;
        }

        void IDisposable.Dispose()
        {
            if (this._packageTable != null)
            {
                foreach (IDisposable disposable in this._packageTable.Values)
                {
                    disposable.Dispose();
                }
                this._packageTable = null;
            }
            this._serviceProvider = null;
        }

        ICollection IPackageManager.Packages
        {
            get
            {
                return this._packageTable.Values;
            }
        }
    }
}

