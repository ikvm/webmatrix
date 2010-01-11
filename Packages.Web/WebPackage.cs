namespace Microsoft.Matrix.Packages.Web
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Packages;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web.Services;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using System;
    using System.ComponentModel.Design;

    public sealed class WebPackage : IPackage, IDisposable
    {
        private bool _designModeEnabled;
        private IServiceProvider _serviceProvider;
        private DocumentViewType _webDefaultView;
        private WebDocumentRunService _webDocumentRunService;
        public static readonly bool DesignViewModeDefault = true;
        public static readonly string DesignViewModePreference = "DesignViewModeEnabled";
        internal static WebPackage Instance;
        private static DocumentViewType WebDefaultViewDefault = DocumentViewType.Design;
        private static string WebDefaultViewPreference = "WebDefaultView";

        public WebPackage()
        {
            Instance = this;
        }

        OptionsPage[] IPackage.GetOptionsPages()
        {
            return new OptionsPage[] { new WebOptionsPage(this._serviceProvider, this) };
        }

        void IPackage.Initialize(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            IServiceContainer container = (IServiceContainer) this._serviceProvider.GetService(typeof(IServiceContainer));
            if (container != null)
            {
                container.AddService(typeof(IWebDocumentRunService), new ServiceCreatorCallback(this.OnCreateService));
            }
            IPreferencesService service = (IPreferencesService) this._serviceProvider.GetService(typeof(IPreferencesService));
            if (service != null)
            {
                PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(WebPackage));
                this._webDefaultView = (DocumentViewType) preferencesStore.GetValue(WebDefaultViewPreference, (int) WebDefaultViewDefault);
                this._designModeEnabled = preferencesStore.GetValue(DesignViewModePreference, DesignViewModeDefault);
            }
        }

        private object OnCreateService(IServiceContainer serviceContainer, Type type)
        {
            if ((this._serviceProvider == null) || (type != typeof(IWebDocumentRunService)))
            {
                return null;
            }
            if (this._webDocumentRunService == null)
            {
                this._webDocumentRunService = new WebDocumentRunService(serviceContainer);
            }
            return this._webDocumentRunService;
        }

        void IDisposable.Dispose()
        {
            if (this._serviceProvider != null)
            {
                IPreferencesService service = (IPreferencesService) this._serviceProvider.GetService(typeof(IPreferencesService));
                if (service != null)
                {
                    PreferencesStore preferencesStore = service.GetPreferencesStore(typeof(WebPackage));
                    preferencesStore.SetValue(WebDefaultViewPreference, (int) this._webDefaultView, (int) WebDefaultViewDefault);
                    preferencesStore.SetValue(DesignViewModePreference, this._designModeEnabled, DesignViewModeDefault);
                }
                ((IServiceContainer) this._serviceProvider.GetService(typeof(IServiceContainer))).RemoveService(typeof(IWebDocumentRunService));
                if (this._webDocumentRunService != null)
                {
                    ((IDisposable) this._webDocumentRunService).Dispose();
                    this._webDocumentRunService = null;
                }
            }
            this._serviceProvider = null;
        }

        public bool DesignModeEnabled
        {
            get
            {
                return this._designModeEnabled;
            }
            set
            {
                this._designModeEnabled = value;
            }
        }

        internal static IServiceProvider ServiceProvider
        {
            get
            {
                return Instance._serviceProvider;
            }
        }

        public DocumentViewType WebDefaultView
        {
            get
            {
                if (!this._designModeEnabled)
                {
                    return DocumentViewType.Source;
                }
                return this.WebDefaultViewInternal;
            }
            set
            {
                this.WebDefaultViewInternal = value;
            }
        }

        internal DocumentViewType WebDefaultViewInternal
        {
            get
            {
                return this._webDefaultView;
            }
            set
            {
                this._webDefaultView = value;
            }
        }
    }
}

