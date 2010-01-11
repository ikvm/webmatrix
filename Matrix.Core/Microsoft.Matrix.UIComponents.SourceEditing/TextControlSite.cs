namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;
    using System.ComponentModel;

    public class TextControlSite : ISite, IServiceProvider
    {
        private ITextLanguage _language;
        private IServiceProvider _serviceProvider;

        public TextControlSite(IServiceProvider serviceProvider) : this(serviceProvider, null)
        {
        }

        public TextControlSite(IServiceProvider serviceProvider, ITextLanguage language)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            this._language = language;
            this._serviceProvider = serviceProvider;
        }

        public object GetService(Type type)
        {
            if ((this._language != null) && (type == typeof(ITextLanguage)))
            {
                return this._language;
            }
            return this._serviceProvider.GetService(type);
        }

        public IComponent Component
        {
            get
            {
                return null;
            }
        }

        public IContainer Container
        {
            get
            {
                return null;
            }
        }

        public bool DesignMode
        {
            get
            {
                return false;
            }
        }

        public string Name
        {
            get
            {
                return "TextControlSite";
            }
            set
            {
            }
        }
    }
}

