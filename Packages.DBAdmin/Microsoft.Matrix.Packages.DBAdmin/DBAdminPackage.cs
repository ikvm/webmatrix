namespace Microsoft.Matrix.Packages.DBAdmin
{
    using Microsoft.Matrix.Core.Packages;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.DBAdmin.Services;
    using System;
    using System.ComponentModel.Design;

    public class DBAdminPackage : IPackage, IDisposable
    {
        private DatabaseManager _dbManager;
        private IServiceProvider _serviceProvider;
        public static string MxDataTableDataFormat = "MxDataTable";

        OptionsPage[] IPackage.GetOptionsPages()
        {
            return null;
        }

        void IPackage.Initialize(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            IServiceContainer service = (IServiceContainer) serviceProvider.GetService(typeof(IServiceContainer));
            if (service != null)
            {
                this._dbManager = new DatabaseManager(serviceProvider);
                service.AddService(typeof(IDatabaseManager), this._dbManager);
            }
        }

        void IDisposable.Dispose()
        {
            if (this._dbManager != null)
            {
                IServiceContainer service = (IServiceContainer) this._serviceProvider.GetService(typeof(IServiceContainer));
                if (service != null)
                {
                    service.RemoveService(typeof(IDatabaseManager));
                }
                ((IDisposable) this._dbManager).Dispose();
                this._dbManager = null;
            }
            this._serviceProvider = null;
        }
    }
}

