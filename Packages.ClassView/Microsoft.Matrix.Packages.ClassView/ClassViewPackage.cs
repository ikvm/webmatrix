namespace Microsoft.Matrix.Packages.ClassView
{
    using Microsoft.Matrix.Core.Packages;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.ClassView.Projects;
    using System;
    using System.ComponentModel.Design;
    using System.Configuration;
    using System.Globalization;

    public sealed class ClassViewPackage : IPackage, IDisposable
    {
        private static bool _applicationTypeChecked;
        private static bool _classBrowserApplication;
        private IClassViewService _classViewService;
        private IServiceProvider _serviceProvider;

        OptionsPage[] IPackage.GetOptionsPages()
        {
            return null;
        }

        void IPackage.Initialize(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            IServiceContainer service = (IServiceContainer) this._serviceProvider.GetService(typeof(IServiceContainer));
            if (service != null)
            {
                ServiceCreatorCallback callback = new ServiceCreatorCallback(this.OnCreateService);
                service.AddService(typeof(IClassViewService), callback);
            }
        }

        private object OnCreateService(IServiceContainer serviceContainer, Type type)
        {
            if ((this._serviceProvider == null) || (type != typeof(IClassViewService)))
            {
                return null;
            }
            if (this._classViewService == null)
            {
                IProjectManager service = (IProjectManager) this._serviceProvider.GetService(typeof(IProjectManager));
                this._classViewService = (IClassViewService) service.CreateProject(typeof(ClassViewProjectFactory), null);
            }
            return this._classViewService;
        }

        void IDisposable.Dispose()
        {
            this._classViewService = null;
            if (this._serviceProvider != null)
            {
                IServiceContainer service = (IServiceContainer) this._serviceProvider.GetService(typeof(IServiceContainer));
                if (service != null)
                {
                    service.RemoveService(typeof(IClassViewService));
                }
                this._serviceProvider = null;
            }
        }

        internal static bool IsClassBrowserApplication
        {
            get
            {
                if (!_applicationTypeChecked)
                {
                    string strA = ConfigurationSettings.AppSettings["ClassView.ClassBrowserApplication"];
                    if ((strA != null) && (string.Compare(strA, "true", true, CultureInfo.InvariantCulture) == 0))
                    {
                        _classBrowserApplication = true;
                    }
                    _applicationTypeChecked = true;
                }
                return _classBrowserApplication;
            }
        }
    }
}

