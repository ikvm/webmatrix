namespace Microsoft.Matrix.Core.Designer
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;

    public class DesignSite : ISite, IServiceProvider
    {
        private IComponent _component;
        private string _name;
        private IServiceProvider _serviceProvider;

        public DesignSite(IServiceProvider serviceProvider, string name)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            this._serviceProvider = serviceProvider;
            this._name = name;
        }

        internal void SetComponent(IComponent component)
        {
            this._component = component;
        }

        internal void SetName(string name)
        {
            this._name = name;
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return this._serviceProvider.GetService(serviceType);
        }

        public IComponent Component
        {
            get
            {
                return this._component;
            }
        }

        public IContainer Container
        {
            get
            {
                IDesignerHost service = (IDesignerHost) this._serviceProvider.GetService(typeof(IDesignerHost));
                return service.Container;
            }
        }

        public bool DesignMode
        {
            get
            {
                return true;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    throw new ArgumentNullException("value");
                }
                if (!value.Equals(this._name))
                {
                    ((DesignerHost) this._serviceProvider.GetService(typeof(IDesignerHost))).RenameComponent(this, value);
                }
            }
        }
    }
}

