namespace Microsoft.Matrix.Core.Services
{
    using Microsoft.Matrix.Core.Application;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    internal sealed class PreferencesService : IPreferencesService, IDisposable
    {
        private string _prefFileName;
        private string _prefPath;
        private IServiceProvider _serviceProvider;
        private Hashtable _stores;

        public PreferencesService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            IApplicationIdentity service = (IApplicationIdentity) this._serviceProvider.GetService(typeof(IApplicationIdentity));
            this._prefPath = this.GetPreferencesPath(service.GetType());
            this._prefFileName = service.PreferencesFileName;
        }

        private string GetPreferencesPath(Type applicationType)
        {
            StringBuilder builder = new StringBuilder(0x100);
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(applicationType.Module.FullyQualifiedName);
            builder.Append(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            builder.Append('\\');
            builder.Append(versionInfo.CompanyName);
            builder.Append('\\');
            builder.Append(versionInfo.ProductName);
            builder.Append('\\');
            builder.Append(versionInfo.ProductMajorPart);
            builder.Append('.');
            builder.Append(versionInfo.ProductMinorPart);
            builder.Append('.');
            builder.Append(versionInfo.FileBuildPart);
            return builder.ToString();
        }

        public bool Load(bool reset)
        {
            bool flag = false;
            if (!reset)
            {
                Stream serializationStream = null;
                string path = Path.Combine(this._prefPath, this._prefFileName);
                try
                {
                    if (File.Exists(path))
                    {
                        serializationStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                        BinaryFormatter formatter = new BinaryFormatter();
                        this._stores = (Hashtable) formatter.Deserialize(serializationStream);
                        flag = true;
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (serializationStream != null)
                    {
                        serializationStream.Close();
                        serializationStream = null;
                    }
                }
            }
            if (this._stores == null)
            {
                this._stores = new Hashtable();
            }
            return flag;
        }

        PreferencesStore IPreferencesService.GetPreferencesStore(Type storeOwnerType)
        {
            if (storeOwnerType == null)
            {
                throw new ArgumentNullException();
            }
            string fullName = storeOwnerType.FullName;
            PreferencesStore store = (PreferencesStore) this._stores[fullName];
            if (store == null)
            {
                store = new PreferencesStore();
                this._stores[fullName] = store;
            }
            return store;
        }

        bool IPreferencesService.GetPreferencesStore(Type storeOwnerType, out PreferencesStore preferencesStore)
        {
            if (storeOwnerType == null)
            {
                throw new ArgumentNullException();
            }
            string fullName = storeOwnerType.FullName;
            PreferencesStore store = (PreferencesStore) this._stores[fullName];
            preferencesStore = store;
            return (store != null);
        }

        void IPreferencesService.ResetPreferencesStore(Type storeOwnerType)
        {
            if (storeOwnerType == null)
            {
                throw new ArgumentNullException();
            }
            string fullName = storeOwnerType.FullName;
            this._stores.Remove(fullName);
        }

        public bool Save()
        {
            Stream serializationStream = null;
            string path = Path.Combine(this._prefPath, this._prefFileName);
            bool flag = false;
            try
            {
                if (!Directory.Exists(this._prefPath))
                {
                    Directory.CreateDirectory(this._prefPath);
                }
                serializationStream = File.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                new BinaryFormatter().Serialize(serializationStream, this._stores);
                flag = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (serializationStream != null)
                {
                    serializationStream.Close();
                    serializationStream = null;
                }
            }
            return flag;
        }

        void IDisposable.Dispose()
        {
            this._serviceProvider = null;
            this._stores = null;
        }
    }
}

