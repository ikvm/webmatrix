namespace Microsoft.Matrix.Core.Application
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Configuration;
    using System.IO;
    using System.Windows.Forms;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Packages;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Utility;

    public abstract class MxApplication : IServiceProvider, IApplicationIdentity
    {
        private static MxApplication current;
        private MxApplicationWindow _appWindow;

        private ToolboxService _toolboxService;
        private WebBrowsingService _webBrowsingService;
        private ComponentGalleryService _componentGalleryService;
        private DataObjectMappingService _dataObjectMappingService;
        private PreferencesService _prefService;
        private PrintService _printService;

        private ProjectManager _projectManager;
        private LanguageManager _languageManager;
        private PackageManager _packageManager;
        private DocumentManager _docManager;
        private DocumentTypeManager _docTypeManager;

        private ServiceContainer _serviceContainer;

        private GlobalExceptionHandler _exceptionHandler;
        private CommandLine _commandLine;
        private EventHandler _startupHandler;
        private IDictionary _webLinks;
        private bool _webLinksLoaded;

        event EventHandler IApplicationIdentity.Startup
        {
            add
            {
                this._startupHandler = (EventHandler) Delegate.Combine(this._startupHandler, value);
            }
            remove
            {
                if (this._startupHandler != null)
                {
                    this._startupHandler = (EventHandler) Delegate.Remove(this._startupHandler, value);
                }
            }
        }

        protected MxApplication(CommandLine commandLine)
        {
            if (current != null)
            {
                throw new InvalidOperationException();
            }
            current = this;
            this._commandLine = commandLine;
        }

        private void CloseApplication()
        {
            if ((this.ApplicationType & Microsoft.Matrix.Core.Application.ApplicationType.Workspace) != Microsoft.Matrix.Core.Application.ApplicationType.Generic)
            {
                this._serviceContainer.RemoveService(typeof(IToolboxService));
                if (this._toolboxService != null)
                {
                    ((IDisposable) this._toolboxService).Dispose();
                    this._toolboxService = null;
                }
                this._serviceContainer.RemoveService(typeof(IDocumentManager));
                this._serviceContainer.RemoveService(typeof(IDesignerEventService));
                if (this._docManager != null)
                {
                    ((IDisposable) this._docManager).Dispose();
                    this._docManager = null;
                }
                this._serviceContainer.RemoveService(typeof(IDocumentTypeManager));
                if (this._docTypeManager != null)
                {
                    ((IDisposable) this._docTypeManager).Dispose();
                    this._docTypeManager = null;
                }
                this._serviceContainer.RemoveService(typeof(ILanguageManager));
                if (this._languageManager != null)
                {
                    ((IDisposable) this._languageManager).Dispose();
                    this._languageManager = null;
                }
                this._serviceContainer.RemoveService(typeof(IProjectManager));
                if (this._projectManager != null)
                {
                    ((IDisposable) this._projectManager).Dispose();
                    this._projectManager = null;
                }
            }
            this._serviceContainer.RemoveService(typeof(IDataObjectMappingService));
            if (this._dataObjectMappingService != null)
            {
                this._dataObjectMappingService = null;
            }
            this._serviceContainer.RemoveService(typeof(IComponentGalleryService));
            if (this._componentGalleryService != null)
            {
                ((IDisposable) this._componentGalleryService).Dispose();
                this._componentGalleryService = null;
            }
            this._serviceContainer.RemoveService(typeof(IWebBrowsingService));
            if (this._webBrowsingService != null)
            {
                ((IDisposable) this._webBrowsingService).Dispose();
                this._webBrowsingService = null;
            }
            this._serviceContainer.RemoveService(typeof(IPrintService));
            if (this._printService != null)
            {
                ((IDisposable) this._printService).Dispose();
                this._printService = null;
            }
            this._serviceContainer.RemoveService(typeof(IPackageManager));
            if (this._packageManager != null)
            {
                ((IDisposable) this._packageManager).Dispose();
                this._packageManager = null;
            }
            this._serviceContainer.RemoveService(typeof(IPreferencesService));
            if (this._prefService != null)
            {
                this._prefService.Save();
                ((IDisposable) this._prefService).Dispose();
                this._prefService = null;
            }
            ((IDisposable) this._exceptionHandler).Dispose();
            this._exceptionHandler = null;
            this._serviceContainer.RemoveService(typeof(IApplicationIdentity));
            this._serviceContainer = null;
        }

        protected abstract IPackage CreateApplicationPackage();
        protected abstract MxApplicationWindow CreateApplicationWindow();

        //init services
        private void InitializeApplication()
        {
            ServiceCreatorCallback callback = new ServiceCreatorCallback(this.OnCreateService);
            this._serviceContainer = new ServiceContainer();
            this._serviceContainer.AddService(typeof(IApplicationIdentity), this);
            this._exceptionHandler = new GlobalExceptionHandler(this._serviceContainer);
            this._exceptionHandler.Initialize();
            this._prefService = new PreferencesService(this._serviceContainer);
            this._prefService.Load(this._commandLine.Options.Contains("reset"));
            this._serviceContainer.AddService(typeof(IPreferencesService), this._prefService);
            this._packageManager = new PackageManager(this._serviceContainer);
            this._serviceContainer.AddService(typeof(IPackageManager), this._packageManager);
            this._serviceContainer.AddService(typeof(IPrintService), callback);
            this._serviceContainer.AddService(typeof(IWebBrowsingService), callback);
            this._serviceContainer.AddService(typeof(IComponentGalleryService), callback);
            this._serviceContainer.AddService(typeof(IDataObjectMappingService), callback);
            if ((this.ApplicationType & ApplicationType.Workspace) != ApplicationType.Generic)
            {
                this._projectManager = new ProjectManager(this._serviceContainer);
                this._serviceContainer.AddService(typeof(IProjectManager), this._projectManager);
                this._languageManager = new LanguageManager(this._serviceContainer);
                this._serviceContainer.AddService(typeof(ILanguageManager), this._languageManager);
                this._docTypeManager = new DocumentTypeManager(this._serviceContainer);
                this._serviceContainer.AddService(typeof(IDocumentTypeManager), this._docTypeManager);
                this._docManager = new DocumentManager(this._serviceContainer, (this.ApplicationType & ApplicationType.Debugger) != ApplicationType.Generic);
                this._serviceContainer.AddService(typeof(IDocumentManager), this._docManager);
                this._serviceContainer.AddService(typeof(IDesignerEventService), this._docManager);
                this._toolboxService = new ToolboxService(this._serviceContainer);
                this._serviceContainer.AddService(typeof(IToolboxService), this._toolboxService);
            }
        }

        string IApplicationIdentity.GetSetting(string settingName)
        {
            return ((IApplicationIdentity) this).GetSetting(settingName, true);
        }

        string IApplicationIdentity.GetSetting(string settingName, bool allowCommandLineOverride)
        {
            if ((settingName == null) || (settingName.Length == 0))
            {
                throw new ArgumentNullException("settingName");
            }
            string str = null;
            if (allowCommandLineOverride)
            {
                str = (string) this._commandLine.Options[settingName];
                if (str != null)
                {
                    return str;
                }
            }
            if (this._prefService != null)
            {
                str = ((IPreferencesService) this._prefService).GetPreferencesStore(typeof(IApplicationIdentity)).GetValue(settingName, (string) null);
                if (str != null)
                {
                    return str;
                }
            }
            return ConfigurationSettings.AppSettings[settingName];
        }

        bool IApplicationIdentity.OnUnhandledException(Exception e)
        {
            return this.OnUnhandledException(e);
        }

        void IApplicationIdentity.SetSetting(string settingName, string settingValue)
        {
            if ((settingName == null) || (settingName.Length == 0))
            {
                throw new ArgumentNullException("settingName");
            }
            if (this._prefService != null)
            {
                PreferencesStore preferencesStore = ((IPreferencesService) this._prefService).GetPreferencesStore(typeof(IApplicationIdentity));
                if ((settingValue == null) || (settingValue.Length == 0))
                {
                    preferencesStore.ClearValue(settingName);
                }
                else
                {
                    preferencesStore.SetValue(settingName, settingValue, string.Empty);
                }
            }
        }

        void IApplicationIdentity.ShowHelpTopics()
        {
            this.ShowHelpTopics();
        }

        private void OnApplicationWindowActivated(object sender, EventArgs e)
        {
            if ((this.ApplicationType & ApplicationType.Workspace) != ApplicationType.Generic)
            {
                this._projectManager.Initialize();
            }
        }

        private void OnApplicationWindowClosing(object sender, CancelEventArgs e)
        {
            if (((this.ApplicationType & ApplicationType.Workspace) != ApplicationType.Generic) && !this._projectManager.Close())
            {
                e.Cancel = true;
            }
        }

        private object OnCreateService(IServiceContainer serviceContainer, Type type)
        {
            if (type == typeof(IComponentGalleryService))
            {
                if (this._componentGalleryService == null)
                {
                    this._componentGalleryService = new ComponentGalleryService(this._serviceContainer);
                }
                return this._componentGalleryService;
            }
            if (type == typeof(IWebBrowsingService))
            {
                if (this._webBrowsingService == null)
                {
                    this._webBrowsingService = new WebBrowsingService(this._serviceContainer);
                }
                return this._webBrowsingService;
            }
            if (type == typeof(IPrintService))
            {
                if (this._printService == null)
                {
                    this._printService = new PrintService(this._serviceContainer);
                }
                return this._printService;
            }
            if (type != typeof(IDataObjectMappingService))
            {
                return null;
            }
            if (this._dataObjectMappingService == null)
            {
                this._dataObjectMappingService = new DataObjectMappingService(this._serviceContainer);
            }
            return this._dataObjectMappingService;
        }

        protected virtual bool OnUnhandledException(Exception e)
        {
            return false;
        }

        internal void RaiseStartupEvent()
        {
            if (this._startupHandler != null)
            {
                this._startupHandler(this, EventArgs.Empty);
            }
        }

        public void Run()
        {
            try
            {
                this.InitializeApplication();
                this._appWindow = this.CreateApplicationWindow();
                this._appWindow.InitialActivated += new EventHandler(this.OnApplicationWindowActivated);
                this._appWindow.Closing += new CancelEventHandler(this.OnApplicationWindowClosing);
                bool flag = (this.ApplicationType & ApplicationType.Workspace) != ApplicationType.Generic;

                this._packageManager.LoadPackages(this.CreateApplicationPackage());
                if (flag)
                {
                    this._toolboxService.LoadToolbox();
                    this._languageManager.LoadDocumentLanguages();
                    this._docTypeManager.LoadDocumentTypes();
                    //TODO: 上次研究到这里, 有时间继续 2010.10.12 15:40
                    this._projectManager.LoadProjectTypes();
                }
                Application.Run(this._appWindow);
            }
            catch (Exception)
            {
            }
            finally
            {
                try
                {
                    this.CloseApplication();
                }
                catch (Exception)
                {
                }
            }
        }

        protected virtual void ShowHelpTopics()
        {
            string helpUrl = this.HelpUrl;
            if ((helpUrl != null) && (helpUrl.Length != 0))
            {
                IWebBrowsingService service = (IWebBrowsingService) this._serviceContainer.GetService(typeof(IWebBrowsingService));
                if (service != null)
                {
                    service.BrowseUrl(helpUrl);
                }
            }
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return this._serviceContainer.GetService(serviceType);
        }

        protected string ApplicationPath
        {
            get
            {
                return Path.GetDirectoryName(base.GetType().Module.FullyQualifiedName);
            }
        }

        protected string ApplicationPrivatePath
        {
            get
            {
                return Path.Combine(this.ApplicationPath, this.Name);
            }
        }

        protected virtual Microsoft.Matrix.Core.Application.ApplicationType ApplicationType
        {
            get
            {
                return Microsoft.Matrix.Core.Application.ApplicationType.Generic;
            }
        }

        protected virtual string ComponentsPath
        {
            get
            {
                return string.Empty;
            }
        }

        internal static MxApplication Current
        {
            get
            {
                return current;
            }
        }

        protected virtual string HelpUrl
        {
            get
            {
                return string.Empty;
            }
        }

        string IApplicationIdentity.ApplicationPath
        {
            get
            {
                return this.ApplicationPath;
            }
        }

        string IApplicationIdentity.ApplicationPrivatePath
        {
            get
            {
                return this.ApplicationPrivatePath;
            }
        }

        Microsoft.Matrix.Core.Application.ApplicationType IApplicationIdentity.ApplicationType
        {
            get
            {
                return this.ApplicationType;
            }
        }

        MxApplicationWindow IApplicationIdentity.ApplicationWindow
        {
            get
            {
                return this._appWindow;
            }
        }

        CommandLine IApplicationIdentity.CommandLine
        {
            get
            {
                return this._commandLine;
            }
        }

        string IApplicationIdentity.ComponentsPath
        {
            get
            {
                return this.ComponentsPath;
            }
        }

        string IApplicationIdentity.Name
        {
            get
            {
                return base.GetType().Assembly.GetName().Name;
            }
        }

        string IApplicationIdentity.PluginsPath
        {
            get
            {
                return this.PluginsPath;
            }
        }

        string IApplicationIdentity.PreferencesFileName
        {
            get
            {
                return this.PreferencesFileName;
            }
        }

        string IApplicationIdentity.TemplatesPath
        {
            get
            {
                return this.TemplatesPath;
            }
        }

        string IApplicationIdentity.Title
        {
            get
            {
                return this.Title;
            }
        }

        IDictionary IApplicationIdentity.WebLinks
        {
            get
            {
                if (!this._webLinksLoaded)
                {
                    if (this._webLinks == null)
                    {
                        this._webLinks = (IDictionary) ConfigurationSettings.GetConfig("microsoft.matrix/webLinks");
                    }
                    this._webLinksLoaded = true;
                }
                return this._webLinks;
            }
        }

        protected virtual string Name
        {
            get
            {
                return base.GetType().Assembly.GetName().Name;
            }
        }

        protected virtual string PluginsPath
        {
            get
            {
                return string.Empty;
            }
        }

        protected abstract string PreferencesFileName { get; }

        internal IServiceProvider ServiceProvider
        {
            get
            {
                return this._serviceContainer;
            }
        }

        //protected virtual string TemplatesPath
        public virtual string TemplatesPath
        {
            get
            {
                return null;
            }
        }

        protected abstract string Title { get; }
    }
}

