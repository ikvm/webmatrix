namespace Microsoft.Matrix.Core.Designer
{
    using Microsoft.Matrix.Core.Documents;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.ComponentModel.Design.Serialization;
    using System.Reflection;

    public class DesignerHost : IDesignerHost, IServiceContainer, IServiceProvider, IDocumentDesignerHost, IContainer, IComponentChangeService, ITypeDescriptorFilterService, ISelectionService, ITypeResolutionService, IDisposable, INameCreationService, ILayeredDesignerHost
    {
        private bool _active;
        private DesignLayer _activeLayer;
        private Hashtable _componentTable;
        private Hashtable _designerTable;
        private Hashtable _designLayers;
        private IComponent _documentComponent;
        private IDesigner _documentDesigner;
        private EventHandlerList _eventHandlerList;
        private bool _hasClearableComponents;
        private bool _loading;
        private DesignSite _newComponentSite;
        private IComponent _rootComponent;
        private IDesigner _rootDesigner;
        private ICollection _selectedObjects;
        private ServiceContainer _serviceContainer;
        private DesignerTransactionStack _transactionStack;
        private static readonly object ActivatedEvent = new object();
        private static readonly object ComponentAddedEvent = new object();
        private static readonly object ComponentAddingEvent = new object();
        private static readonly object ComponentChangedEvent = new object();
        private static readonly object ComponentChangingEvent = new object();
        private static readonly object ComponentRemovedEvent = new object();
        private static readonly object ComponentRemovingEvent = new object();
        private static readonly object ComponentRenameEvent = new object();
        private static readonly object DeactivatedEvent = new object();
        private static readonly object LoadCompleteEvent = new object();
        private static readonly object SelectionChangedEvent = new object();
        private static readonly object SelectionChangingEvent = new object();
        private static readonly object TransactionClosedEvent = new object();
        private static readonly object TransactionClosingEvent = new object();
        private static readonly object TransactionOpenedEvent = new object();
        private static readonly object TransactionOpeningEvent = new object();

        event ComponentEventHandler IComponentChangeService.ComponentAdded
        {
            add
            {
                this.Events.AddHandler(ComponentAddedEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(ComponentAddedEvent, value);
            }
        }

        event ComponentEventHandler IComponentChangeService.ComponentAdding
        {
            add
            {
                this.Events.AddHandler(ComponentAddingEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(ComponentAddingEvent, value);
            }
        }

        event ComponentChangedEventHandler IComponentChangeService.ComponentChanged
        {
            add
            {
                this.Events.AddHandler(ComponentChangedEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(ComponentChangedEvent, value);
            }
        }

        event ComponentChangingEventHandler IComponentChangeService.ComponentChanging
        {
            add
            {
                this.Events.AddHandler(ComponentChangingEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(ComponentChangingEvent, value);
            }
        }

        event ComponentEventHandler IComponentChangeService.ComponentRemoved
        {
            add
            {
                this.Events.AddHandler(ComponentRemovedEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(ComponentRemovedEvent, value);
            }
        }

        event ComponentEventHandler IComponentChangeService.ComponentRemoving
        {
            add
            {
                this.Events.AddHandler(ComponentRemovingEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(ComponentRemovingEvent, value);
            }
        }

        event ComponentRenameEventHandler IComponentChangeService.ComponentRename
        {
            add
            {
                this.Events.AddHandler(ComponentRenameEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(ComponentRenameEvent, value);
            }
        }

        event EventHandler IDesignerHost.Activated
        {
            add
            {
                this.Events.AddHandler(ActivatedEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(ActivatedEvent, value);
            }
        }

        event EventHandler IDesignerHost.Deactivated
        {
            add
            {
                this.Events.AddHandler(DeactivatedEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(DeactivatedEvent, value);
            }
        }

        event EventHandler IDesignerHost.LoadComplete
        {
            add
            {
                if (this.ActiveLayer != null)
                {
                    this.ActiveLayer.LoadComplete += value;
                }
                else
                {
                    this.Events.AddHandler(LoadCompleteEvent, value);
                }
            }
            remove
            {
                if (this.ActiveLayer != null)
                {
                    this.ActiveLayer.LoadComplete -= value;
                }
                else
                {
                    this.Events.RemoveHandler(LoadCompleteEvent, value);
                }
            }
        }

        event DesignerTransactionCloseEventHandler IDesignerHost.TransactionClosed
        {
            add
            {
                this.Events.AddHandler(TransactionClosedEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(TransactionClosedEvent, value);
            }
        }

        event DesignerTransactionCloseEventHandler IDesignerHost.TransactionClosing
        {
            add
            {
                this.Events.AddHandler(TransactionClosingEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(TransactionClosingEvent, value);
            }
        }

        event EventHandler IDesignerHost.TransactionOpened
        {
            add
            {
                this.Events.AddHandler(TransactionOpenedEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(TransactionOpenedEvent, value);
            }
        }

        event EventHandler IDesignerHost.TransactionOpening
        {
            add
            {
                this.Events.AddHandler(TransactionOpeningEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(TransactionOpeningEvent, value);
            }
        }

        event EventHandler ISelectionService.SelectionChanged
        {
            add
            {
                this.Events.AddHandler(SelectionChangedEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(SelectionChangedEvent, value);
            }
        }

        event EventHandler ISelectionService.SelectionChanging
        {
            add
            {
                this.Events.AddHandler(SelectionChangingEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(SelectionChangingEvent, value);
            }
        }

        public DesignerHost(Document document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            this._serviceContainer = new ServiceContainer(document.ProjectItem.Project);
            this._componentTable = new Hashtable();
            this._designerTable = new Hashtable();
            ((IContainer) this).Add(document, this.DocumentComponentName);
        }

        private void ClearComponents()
        {
            if (this.HasClearableComponents)
            {
                this.ClearComponents(this.DesignerTable, this.ComponentTable);
                if (this._activeLayer == null)
                {
                    if (this._rootComponent != null)
                    {
                        this.ComponentTable[this._rootComponent.Site.Name] = this._rootComponent;
                        this.DesignerTable[this._rootComponent] = this._rootDesigner;
                    }
                    if (this._documentComponent != null)
                    {
                        this.ComponentTable[this._documentComponent.Site.Name] = this._documentComponent;
                        this.DesignerTable[this._documentComponent] = this._documentDesigner;
                    }
                }
                this.HasClearableComponents = false;
            }
        }

        private void ClearComponents(IDictionary designers, IDictionary components)
        {
            foreach (IDesigner designer in designers.Values)
            {
                if ((designer != this._documentDesigner) && (designer != this._rootDesigner))
                {
                    designer.Dispose();
                }
            }
            designers.Clear();
            IComponent[] componentArray = this.GetComponents(components);
            components.Clear();
            for (int i = componentArray.Length - 1; i >= 0; i--)
            {
                IComponent component = componentArray[i];
                if ((component != this._rootComponent) && (component != this._documentComponent))
                {
                    component.Dispose();
                }
            }
        }

        protected virtual IDesigner CreateComponentDesigner(IComponent component, Type designerType)
        {
            return TypeDescriptor.CreateDesigner(component, designerType);
        }

        private object CreateObject(Type objectType, object[] args, Type[] argTypes)
        {
            ConstructorInfo info = null;
            object obj2;
            if ((args != null) && (argTypes.Length != 0))
            {
                if (argTypes == null)
                {
                    argTypes = new Type[args.Length];
                    for (int i = args.Length - 1; i >= 0; i--)
                    {
                        if (args[i] != null)
                        {
                            argTypes[i] = args[i].GetType();
                        }
                    }
                }
                info = objectType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, argTypes, null);
                if (info == null)
                {
                    throw new ArgumentException("Cannot find constructor with right arguments for type " + objectType.FullName);
                }
            }
            try
            {
                if (info != null)
                {
                    return info.Invoke(args);
                }
                obj2 = Activator.CreateInstance(objectType);
            }
            catch (Exception innerException)
            {
                if (innerException is TargetInvocationException)
                {
                    innerException = innerException.InnerException;
                }
                throw new Exception("Cannot create an instance of type " + objectType.FullName, innerException);
            }
            return obj2;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        private void EnsureSelection()
        {
            if (((this._selectedObjects == null) || (this._selectedObjects.Count == 0)) && (this._documentComponent != null))
            {
                this._selectedObjects = new object[] { this._documentComponent };
            }
        }

        ~DesignerHost()
        {
            this.Dispose(false);
        }

        private IComponent[] GetComponents()
        {
            return this.GetComponents(this.ComponentTable);
        }

        private IComponent[] GetComponents(IDictionary componentTable)
        {
            int count = componentTable.Count;
            IComponent[] componentArray = new IComponent[count];
            if (count != 0)
            {
                int num2 = 0;
                foreach (IComponent component in componentTable.Values)
                {
                    componentArray[num2++] = component;
                }
            }
            return componentArray;
        }

        protected virtual string GetNewComponentName(Type componentType)
        {
            if (componentType == null)
            {
                throw new ArgumentNullException("componentType");
            }
            INameCreationService service = (INameCreationService) ((IServiceProvider) this).GetService(typeof(INameCreationService));
            return service.CreateName(this, componentType);
        }

        public string GetPathOfAssembly(AssemblyName name)
        {
            return name.CodeBase;
        }

        //protected virtual object GetService(Type serviceType) //NOTE: changed
        public virtual object GetService(Type serviceType)
        {
            if (this._serviceContainer != null)
            {
                return this._serviceContainer.GetService(serviceType);
            }
            return null;
        }

        protected virtual bool IsValidComponentName(string name)
        {
            if ((name == null) || (name.Length == 0))
            {
                return false;
            }
            INameCreationService service = (INameCreationService) ((IServiceProvider) this).GetService(typeof(INameCreationService));
            return service.IsValidName(name);
        }

        void IDocumentDesignerHost.BeginLoad()
        {
            this._loading = true;
        }

        void IDocumentDesignerHost.EndLoad()
        {
            this._loading = false;
            this.OnLoadComplete(EventArgs.Empty);
        }

        IDesignLayer ILayeredDesignerHost.AddLayer(IServiceProvider serviceProvider, string name, IDesigner parentDesigner, bool clearOnActivate)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            if (parentDesigner == null)
            {
                throw new ArgumentNullException("parentDesigner");
            }
            if (!this.DesignerTable.Contains(parentDesigner.Component))
            {
                throw new ArgumentException("parentDesigner");
            }
            IDesignLayer key = new DesignLayer(this, serviceProvider, name, parentDesigner, clearOnActivate);
            this.DesignLayers.Add(key, string.Empty);
            return key;
        }

        void ILayeredDesignerHost.RemoveLayer(IDesignLayer layer)
        {
            if (layer == null)
            {
                throw new ArgumentNullException("layer");
            }
            if (!this.DesignLayers.Contains(layer))
            {
                throw new ArgumentException("layer");
            }
            if (this.ActiveLayer == layer)
            {
                this.ActiveLayer = null;
            }
            DesignLayer layer2 = (DesignLayer) layer;
            if (layer2.HasClearableComponents)
            {
                this.ClearComponents(layer2.DesignerTable, layer2.ComponentTable);
            }
            this.DesignLayers.Remove(layer);
            layer.Dispose();
        }

        protected virtual void OnActivated(EventArgs e)
        {
            EventHandler handler = (EventHandler) this.Events[ActivatedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnComponentAdded(ComponentEventArgs e)
        {
            ComponentEventHandler handler = (ComponentEventHandler) this.Events[ComponentAddedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnComponentAdding(ComponentEventArgs e)
        {
            ComponentEventHandler handler = (ComponentEventHandler) this.Events[ComponentAddingEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnComponentChanged(ComponentChangedEventArgs e)
        {
            ComponentChangedEventHandler handler = (ComponentChangedEventHandler) this.Events[ComponentChangedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnComponentChanging(ComponentChangingEventArgs e)
        {
            ComponentChangingEventHandler handler = (ComponentChangingEventHandler) this.Events[ComponentChangingEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnComponentRemoved(ComponentEventArgs e)
        {
            ComponentEventHandler handler = (ComponentEventHandler) this.Events[ComponentRemovedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnComponentRemoving(ComponentEventArgs e)
        {
            ComponentEventHandler handler = (ComponentEventHandler) this.Events[ComponentRemovingEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnComponentRename(ComponentRenameEventArgs e)
        {
            ComponentRenameEventHandler handler = (ComponentRenameEventHandler) this.Events[ComponentRenameEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnDeactivated(EventArgs e)
        {
            EventHandler handler = (EventHandler) this.Events[DeactivatedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnLoadComplete(EventArgs e)
        {
            if (this.ActiveLayer != null)
            {
                this.ActiveLayer.RaiseLoadComplete();
            }
            else
            {
                EventHandler handler = (EventHandler) this.Events[LoadCompleteEvent];
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) this.Events[SelectionChangedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSelectionChanging(EventArgs e)
        {
            EventHandler handler = (EventHandler) this.Events[SelectionChangingEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnTransactionClosed(DesignerTransactionCloseEventArgs e)
        {
            DesignerTransactionCloseEventHandler handler = (DesignerTransactionCloseEventHandler) this.Events[TransactionClosedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnTransactionClosing(DesignerTransactionCloseEventArgs e)
        {
            DesignerTransactionCloseEventHandler handler = (DesignerTransactionCloseEventHandler) this.Events[TransactionClosingEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnTransactionOpened(EventArgs e)
        {
            EventHandler handler = (EventHandler) this.Events[TransactionOpenedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnTransactionOpening(EventArgs e)
        {
            EventHandler handler = (EventHandler) this.Events[TransactionOpeningEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal void RenameComponent(DesignSite site, string newName)
        {
            if (!this.IsValidComponentName(newName))
            {
                throw new ArgumentException("'" + newName + "' is not a valid name for the component.");
            }
            if (this.ComponentTable[newName] != null)
            {
                throw new ArgumentException("A component with the name '" + newName + "' already exists.");
            }
            string name = site.Name;
            ComponentRenameEventArgs e = new ComponentRenameEventArgs(site.Component, name, newName);
            this.ComponentTable.Remove(name);
            this.ComponentTable[newName] = site.Component;
            site.SetName(newName);
            this.OnComponentRename(e);
        }

        void IComponentChangeService.OnComponentChanged(object component, MemberDescriptor member, object oldValue, object newValue)
        {
            if (!this._loading)
            {
                ComponentChangedEventArgs e = new ComponentChangedEventArgs(component, member, oldValue, newValue);
                this.OnComponentChanged(e);
            }
        }

        void IComponentChangeService.OnComponentChanging(object component, MemberDescriptor member)
        {
            if (!this._loading)
            {
                ComponentChangingEventArgs e = new ComponentChangingEventArgs(component, member);
                this.OnComponentChanging(e);
            }
        }

        void IDesignerHost.Activate()
        {
        }

        IComponent IDesignerHost.CreateComponent(Type componentType)
        {
            string newComponentName = this.GetNewComponentName(componentType);
            return ((IDesignerHost) this).CreateComponent(componentType, newComponentName);
        }

        IComponent IDesignerHost.CreateComponent(Type componentType, string name)
        {
            if (componentType == null)
            {
                throw new ArgumentNullException("componentType");
            }
            object obj2 = null;
            IComponent component = null;
            IServiceProvider serviceProvider = null;
            if (this.ActiveLayer != null)
            {
                serviceProvider = this.ActiveLayer;
            }
            else
            {
                serviceProvider = this;
            }
            this._newComponentSite = new DesignSite(serviceProvider, name);
            try
            {
                try
                {
                    object[] args = new object[] { this };
                    Type[] argTypes = new Type[] { typeof(IContainer) };
                    obj2 = this.CreateObject(componentType, args, argTypes);
                }
                catch
                {
                }
                if (obj2 == null)
                {
                    obj2 = this.CreateObject(componentType, null, null);
                }
                component = obj2 as IComponent;
                if (component == null)
                {
                    throw new ArgumentException(componentType.FullName + " is not an IComponent");
                }
                if (!(component.Site is DesignSite))
                {
                    ((IContainer) this).Add(component);
                }
            }
            catch
            {
                if (component != null)
                {
                    try
                    {
                        ((IDesignerHost) this).DestroyComponent(component);
                    }
                    catch
                    {
                    }
                }
                this._newComponentSite = null;
                throw;
            }
            return component;
        }

        DesignerTransaction IDesignerHost.CreateTransaction()
        {
            return ((IDesignerHost) this).CreateTransaction(null);
        }

        DesignerTransaction IDesignerHost.CreateTransaction(string description)
        {
            return new MxDesignerTransaction(this, description);
        }

        void IDesignerHost.DestroyComponent(IComponent component)
        {
            ((IContainer) this).Remove(component);
        }

        IDesigner IDesignerHost.GetDesigner(IComponent component)
        {
            if ((this.ActiveLayer != null) && (this.ActiveLayer.ParentComponent == component))
            {
                return this.ActiveLayer.ParentDesigner;
            }
            return (IDesigner) this.DesignerTable[component];
        }

        Type IDesignerHost.GetType(string typeName)
        {
            ITypeResolutionService service = (ITypeResolutionService) this.GetService(typeof(ITypeResolutionService));
            if (service != null)
            {
                return service.GetType(typeName);
            }
            return Type.GetType(typeName);
        }

        bool ISelectionService.GetComponentSelected(object component)
        {
            this.EnsureSelection();
            if ((this._selectedObjects != null) && (this._selectedObjects.Count != 0))
            {
                foreach (object obj2 in this._selectedObjects)
                {
                    if (obj2 == component)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        ICollection ISelectionService.GetSelectedComponents()
        {
            this.EnsureSelection();
            return this._selectedObjects;
        }

        void ISelectionService.SetSelectedComponents(ICollection components)
        {
            ((ISelectionService) this).SetSelectedComponents(components, SelectionTypes.Auto);
        }

        void ISelectionService.SetSelectedComponents(ICollection components, SelectionTypes selectionType)
        {
            if (selectionType != SelectionTypes.Auto)
            {
                throw new ArgumentOutOfRangeException("selectionType");
            }
            this.OnSelectionChanging(EventArgs.Empty);
            this._selectedObjects = components;
            this.OnSelectionChanged(EventArgs.Empty);
        }

        void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            this._serviceContainer.AddService(serviceType, callback);
        }

        void IServiceContainer.AddService(Type serviceType, object serviceInstance)
        {
            this._serviceContainer.AddService(serviceType, serviceInstance);
        }

        void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            this._serviceContainer.AddService(serviceType, callback, promote);
        }

        void IServiceContainer.AddService(Type serviceType, object serviceInstance, bool promote)
        {
            this._serviceContainer.AddService(serviceType, serviceInstance, promote);
        }

        void IServiceContainer.RemoveService(Type serviceType)
        {
            this._serviceContainer.RemoveService(serviceType);
        }

        void IServiceContainer.RemoveService(Type serviceType, bool promote)
        {
            this._serviceContainer.RemoveService(serviceType, promote);
        }

        bool ITypeDescriptorFilterService.FilterAttributes(IComponent component, IDictionary attributes)
        {
            IDesigner designer = ((IDesignerHost) this).GetDesigner(component);
            if (designer is IDesignerFilter)
            {
                ((IDesignerFilter) designer).PreFilterAttributes(attributes);
                ((IDesignerFilter) designer).PostFilterAttributes(attributes);
            }
            return (designer != null);
        }

        bool ITypeDescriptorFilterService.FilterEvents(IComponent component, IDictionary events)
        {
            IDesigner designer = ((IDesignerHost) this).GetDesigner(component);
            if (designer is IDesignerFilter)
            {
                ((IDesignerFilter) designer).PreFilterEvents(events);
                ((IDesignerFilter) designer).PostFilterEvents(events);
            }
            return (designer != null);
        }

        bool ITypeDescriptorFilterService.FilterProperties(IComponent component, IDictionary properties)
        {
            IDesigner designer = ((IDesignerHost) this).GetDesigner(component);
            if (designer is IDesignerFilter)
            {
                ((IDesignerFilter) designer).PreFilterProperties(properties);
                ((IDesignerFilter) designer).PostFilterProperties(properties);
            }
            return (designer != null);
        }

        Assembly ITypeResolutionService.GetAssembly(AssemblyName name)
        {
            return ((ITypeResolutionService) this).GetAssembly(name, false);
        }

        Assembly ITypeResolutionService.GetAssembly(AssemblyName name, bool throwOnError)
        {
            try
            {
                return Assembly.Load(name.Name);
            }
            catch
            {
                if (throwOnError)
                {
                    throw;
                }
            }
            return null;
        }

        Type ITypeResolutionService.GetType(string name)
        {
            return ((ITypeResolutionService) this).GetType(name, false, false);
        }

        Type ITypeResolutionService.GetType(string name, bool throwOnError)
        {
            return ((ITypeResolutionService) this).GetType(name, throwOnError, false);
        }

        Type ITypeResolutionService.GetType(string name, bool throwOnError, bool ignoreCase)
        {
            return Type.GetType(name, throwOnError, ignoreCase);
        }

        void ITypeResolutionService.ReferenceAssembly(AssemblyName name)
        {
        }

        string INameCreationService.CreateName(IContainer cont, Type componentType)
        {
            string name = componentType.Name;
            IContainer container = this;
            int num = 1;
            string str2 = name + num;
            while (container.Components[str2] != null)
            {
                num++;
                str2 = name + num;
            }
            return str2;
        }

        bool INameCreationService.IsValidName(string name)
        {
            for (int i = name.Length - 1; i >= 1; i--)
            {
                char ch = name[i];
                if (!char.IsLetterOrDigit(ch) && (ch != '_'))
                {
                    return false;
                }
            }
            char c = name[0];
            if (!char.IsLetter(c) && (c != '_'))
            {
                return false;
            }
            return true;
        }

        void INameCreationService.ValidateName(string name)
        {
            if (!((INameCreationService) this).IsValidName(name))
            {
                throw new Exception("Not a valid name : " + name);
            }
        }

        void IContainer.Add(IComponent component)
        {
            ((IContainer) this).Add(component, null);
        }

        void IContainer.Add(IComponent component, string name)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            DesignSite site = this._newComponentSite;
            this._newComponentSite = null;
            if (name == null)
            {
                if (site != null)
                {
                    name = site.Name;
                }
                else
                {
                    name = this.GetNewComponentName(component.GetType());
                }
            }
            ISite site2 = component.Site;
            if (((site2 != null) && (site2.Container == this)) && ((name != null) && !name.Equals(site2.Name)))
            {
                site2.Name = name;
            }
            if (!this.IsValidComponentName(name))
            {
                throw new ArgumentException("'" + name + "' is not a valid name");
            }
            if (this.ComponentTable[name] != null)
            {
                throw new ArgumentException("Component with name '" + name + "' already exists");
            }
            if (site2 != null)
            {
                site2.Container.Remove(component);
            }
            ComponentEventArgs e = new ComponentEventArgs(component);
            this.OnComponentAdding(e);
            if (site == null)
            {
                IServiceProvider serviceProvider = null;
                if (this.ActiveLayer != null)
                {
                    serviceProvider = this.ActiveLayer;
                }
                else
                {
                    serviceProvider = this;
                }
                site = new DesignSite(serviceProvider, name);
            }
            else
            {
                site.Name = name;
            }
            bool flag = true;
            Type designerType = typeof(IDesigner);
            if ((this._documentComponent != null) && (this._rootComponent == null))
            {
                designerType = typeof(IRootDesigner);
            }
            IDesigner designer = this.CreateComponentDesigner(component, designerType);
            if (designer == null)
            {
                throw new Exception("Unable to create a designer for component of type " + component.GetType().FullName);
            }
            if ((this._documentComponent == null) && name.Equals(this.DocumentComponentName))
            {
                this._documentComponent = component;
                this._documentDesigner = designer;
                flag = false;
            }
            else if (this._rootComponent == null)
            {
                this._rootComponent = component;
                this._rootDesigner = designer;
                flag = false;
            }
            this.HasClearableComponents |= flag;
            site.SetComponent(component);
            component.Site = site;
            this.ComponentTable[name] = component;
            designer.Initialize(component);
            this.DesignerTable[component] = designer;
            this.OnComponentAdded(e);
        }

        void IContainer.Remove(IComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            if (component.Site != null)
            {
                string name = component.Site.Name;
                if ((name != null) && (this.ComponentTable[name] == component))
                {
                    ComponentEventArgs e = new ComponentEventArgs(component);
                    this.OnComponentRemoving(e);
                    if (this.DesignerTable != null)
                    {
                        IDesigner designer = (IDesigner) this.DesignerTable[component];
                        if (designer != null)
                        {
                            this.DesignerTable.Remove(component);
                            designer.Dispose();
                        }
                    }
                    this.ComponentTable.Remove(name);
                    component.Dispose();
                    try
                    {
                        this.OnComponentRemoved(e);
                    }
                    catch
                    {
                    }
                    component.Site = null;
                }
            }
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if ((((serviceType != typeof(IDesignerHost)) && (serviceType != typeof(IContainer))) && ((serviceType != typeof(IComponentChangeService)) && (serviceType != typeof(ISelectionService)))) && (((serviceType != typeof(ITypeDescriptorFilterService)) && (serviceType != typeof(IDocumentDesignerHost))) && (serviceType != typeof(ILayeredDesignerHost))))
            {
                object service = this.GetService(serviceType);
                if ((service != null) || ((serviceType != typeof(ITypeResolutionService)) && (serviceType != typeof(INameCreationService))))
                {
                    return service;
                }
            }
            return this;
        }

        private DesignLayer ActiveLayer
        {
            get
            {
                return this._activeLayer;
            }
            set
            {
                if (this._activeLayer != value)
                {
                    this._activeLayer = value;
                    if ((this._activeLayer != null) && this._activeLayer.ClearOnActivate)
                    {
                        this.ClearComponents();
                    }
                    ((ISelectionService) this).SetSelectedComponents(null);
                    this.OnSelectionChanged(EventArgs.Empty);
                }
            }
        }

        private Hashtable ComponentTable
        {
            get
            {
                if (this.ActiveLayer != null)
                {
                    return this.ActiveLayer.ComponentTable;
                }
                return this._componentTable;
            }
        }

        private Hashtable DesignerTable
        {
            get
            {
                if (this.ActiveLayer != null)
                {
                    return this.ActiveLayer.DesignerTable;
                }
                return this._designerTable;
            }
        }

        private Hashtable DesignLayers
        {
            get
            {
                if (this._designLayers == null)
                {
                    this._designLayers = new Hashtable();
                }
                return this._designLayers;
            }
        }

        protected virtual string DocumentComponentName
        {
            get
            {
                return "Document";
            }
        }

        protected EventHandlerList Events
        {
            get
            {
                if (this._eventHandlerList == null)
                {
                    this._eventHandlerList = new EventHandlerList();
                }
                return this._eventHandlerList;
            }
        }

        private bool HasClearableComponents
        {
            get
            {
                if (this.ActiveLayer != null)
                {
                    return this.ActiveLayer.HasClearableComponents;
                }
                return this._hasClearableComponents;
            }
            set
            {
                if (this.ActiveLayer != null)
                {
                    this.ActiveLayer.HasClearableComponents = value;
                }
                else
                {
                    this._hasClearableComponents = value;
                }
            }
        }

        bool IDocumentDesignerHost.DesignerActive
        {
            get
            {
                return this._active;
            }
            set
            {
                this._active = value;
                if (!this._active)
                {
                    this.ClearComponents();
                }
            }
        }

        Document IDocumentDesignerHost.Document
        {
            get
            {
                return (Document) this._documentComponent;
            }
        }

        IDesignLayer ILayeredDesignerHost.ActiveLayer
        {
            get
            {
                return this.ActiveLayer;
            }
            set
            {
                if (value == null)
                {
                    this.ActiveLayer = null;
                }
                else
                {
                    if (!this.DesignLayers.Contains(value))
                    {
                        throw new ArgumentException("value");
                    }
                    this.ActiveLayer = (DesignLayer) value;
                }
            }
        }

        IContainer IDesignerHost.Container
        {
            get
            {
                return this;
            }
        }

        bool IDesignerHost.InTransaction
        {
            get
            {
                return ((this._transactionStack != null) && (this._transactionStack.Count != 0));
            }
        }

        bool IDesignerHost.Loading
        {
            get
            {
                return this._loading;
            }
        }

        IComponent IDesignerHost.RootComponent
        {
            get
            {
                return this._rootComponent;
            }
        }

        string IDesignerHost.RootComponentClassName
        {
            get
            {
                if (this._rootComponent != null)
                {
                    return this._rootComponent.GetType().FullName;
                }
                return null;
            }
        }

        string IDesignerHost.TransactionDescription
        {
            get
            {
                if (this._transactionStack != null)
                {
                    return this._transactionStack.GetTransactionDescription();
                }
                return string.Empty;
            }
        }

        object ISelectionService.PrimarySelection
        {
            get
            {
                this.EnsureSelection();
                if ((this._selectedObjects != null) && (this._selectedObjects.Count != 0))
                {
                    try
                    {
                        IEnumerator enumerator = this._selectedObjects.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            return enumerator.Current;
                        }
                        enumerator = null;
                    }
                    finally
                    { 
                    }
                    //using (IEnumerator enumerator = this._selectedObjects.GetEnumerator())
                    //{
                    //    while (enumerator.MoveNext())
                    //    {
                    //        return enumerator.Current;
                    //    }
                    //}
                }
                return null;
            }
        }

        int ISelectionService.SelectionCount
        {
            get
            {
                this.EnsureSelection();
                if (this._selectedObjects != null)
                {
                    return this._selectedObjects.Count;
                }
                return 0;
            }
        }

        ComponentCollection IContainer.Components
        {
            get
            {
                return new ComponentCollection(this.GetComponents());
            }
        }

        private class DesignerTransactionStack : Stack
        {
            internal string GetTransactionDescription()
            {
                int count = this.Count;
                if (count != 0)
                {
                    object[] objArray = this.ToArray();
                    for (int i = count - 1; i >= 0; i--)
                    {
                        string str = objArray[i] as string;
                        if ((str != null) && (str.Length > 0))
                        {
                            return str;
                        }
                    }
                }
                return string.Empty;
            }
        }

        private sealed class DesignLayer : IDesignLayer, IServiceProvider, IDisposable
        {
            private bool _clearOnActivate;
            private Hashtable _componentTable;
            private DesignerHost _designerHost;
            private Hashtable _designerTable;
            private bool _hasClearableComponents;
            private EventHandler _loadCompleteHandler;
            private string _name;
            private IDesigner _parentDesigner;
            private IServiceProvider _serviceProvider;

            public event EventHandler LoadComplete
            {
                add
                {
                    this._loadCompleteHandler = (EventHandler) Delegate.Combine(this._loadCompleteHandler, value);
                }
                remove
                {
                    if (this._loadCompleteHandler != null)
                    {
                        this._loadCompleteHandler = (EventHandler) Delegate.Remove(this._loadCompleteHandler, value);
                    }
                }
            }

            public DesignLayer(DesignerHost designerHost, IServiceProvider serviceProvider, string name, IDesigner parentDesigner, bool clearOnActivate)
            {
                this._designerHost = designerHost;
                this._serviceProvider = serviceProvider;
                this._name = name;
                this._parentDesigner = parentDesigner;
                this._clearOnActivate = clearOnActivate;
            }

            public void RaiseLoadComplete()
            {
                if (this._loadCompleteHandler != null)
                {
                    this._loadCompleteHandler(this._designerHost, EventArgs.Empty);
                }
            }

            void IDisposable.Dispose()
            {
                this._hasClearableComponents = false;
                this._serviceProvider = null;
            }

            object IServiceProvider.GetService(Type type)
            {
                if (this._serviceProvider != null)
                {
                    return this._serviceProvider.GetService(type);
                }
                return null;
            }

            public bool ClearOnActivate
            {
                get
                {
                    return this._clearOnActivate;
                }
            }

            public Hashtable ComponentTable
            {
                get
                {
                    if (this._componentTable == null)
                    {
                        this._componentTable = new Hashtable();
                    }
                    return this._componentTable;
                }
            }

            public Hashtable DesignerTable
            {
                get
                {
                    if (this._designerTable == null)
                    {
                        this._designerTable = new Hashtable();
                    }
                    return this._designerTable;
                }
            }

            public bool HasClearableComponents
            {
                get
                {
                    return this._hasClearableComponents;
                }
                set
                {
                    this._hasClearableComponents = value;
                }
            }

            public string Name
            {
                get
                {
                    return this._name;
                }
            }

            public IComponent ParentComponent
            {
                get
                {
                    return this._parentDesigner.Component;
                }
            }

            public IDesigner ParentDesigner
            {
                get
                {
                    return this._parentDesigner;
                }
            }
        }

        private class MxDesignerTransaction : DesignerTransaction
        {
            private DesignerHost _host;

            public MxDesignerTransaction(DesignerHost host, string description) : base(description)
            {
                this._host = host;
                if (this._host._transactionStack == null)
                {
                    this._host._transactionStack = new DesignerHost.DesignerTransactionStack();
                }
                this._host._transactionStack.Push(description);
                if (this._host._transactionStack.Count == 1)
                {
                    this._host.OnTransactionOpening(EventArgs.Empty);
                    this._host.OnTransactionOpened(EventArgs.Empty);
                }
            }

            protected override void OnCancel()
            {
                if (this._host != null)
                {
                    string text1 = (string) this._host._transactionStack.Pop();
                    if (this._host._transactionStack.Count == 0)
                    {
                        DesignerTransactionCloseEventArgs e = new DesignerTransactionCloseEventArgs(false);
                        this._host.OnTransactionClosing(e);
                        this._host.OnTransactionClosed(e);
                    }
                    this._host = null;
                }
            }

            protected override void OnCommit()
            {
                if (this._host != null)
                {
                    string text1 = (string) this._host._transactionStack.Pop();
                    if (this._host._transactionStack.Count == 0)
                    {
                        DesignerTransactionCloseEventArgs e = new DesignerTransactionCloseEventArgs(true);
                        this._host.OnTransactionClosing(e);
                        this._host.OnTransactionClosed(e);
                    }
                    this._host = null;
                }
            }
        }
    }
}

