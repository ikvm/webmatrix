namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.Design.MobileControls;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;
    using System.Web.UI.WebControls;

    public abstract class MobileTemplatedControlDesigner : TemplatedControlDesigner, IMobileDesigner
    {
        private string _activeDeviceFilter;
        private Microsoft.Matrix.Framework.Mobile.UI.Design.ContainmentStatus _containmentStatus;
        private bool _containmentStatusDirty = true;
        private Hashtable _defaultValues;
        private IDeviceSpecificSelectionProvider _deviceSpecificSelectionProvider;
        protected bool _isResetting = false;
        private MobileControl _mobileControl;
        private Hashtable _overridenValues;
        private TemplateEditingVerb[] _templateVerbs;
        private bool _templateVerbsDirty = true;
        protected DeviceSpecificChoice _temporaryChoice = null;

        protected MobileTemplatedControlDesigner()
        {
        }

        protected virtual void ApplyDefaultProperties()
        {
            Utils.ApplyPropertyValues(this._mobileControl, this._defaultValues);
        }

        protected virtual void ApplyPropertyOverrides()
        {
            if (this.ActiveDeviceFilter != null)
            {
                Utils.ApplyPropertyValues(this._mobileControl, (Hashtable) this._overridenValues[this.ActiveDeviceFilter]);
            }
        }

        [Conditional("DEBUG")]
        private void CheckTemplateName(string templateName)
        {
        }

        protected override ITemplateEditingFrame CreateTemplateEditingFrame(TemplateEditingVerb verb)
        {
            ITemplateEditingService service = (ITemplateEditingService) this.GetService(typeof(ITemplateEditingService));
            string[] templateFrameNames = this.GetTemplateFrameNames(verb.Index);
            ITemplateEditingFrame frame = service.CreateFrame(this, verb.Text, templateFrameNames, this.WebCtrlStyle, null);
            frame.InitialWidth = Constants.TemplateWidth;
            return frame;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._deviceSpecificSelectionProvider != null)
                {
                    this._deviceSpecificSelectionProvider.SelectionChanged -= new EventHandler(this.OnHostDeviceSpecificChanged);
                }
                IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
                if (service != null)
                {
                    service.LoadComplete -= new EventHandler(this.OnLoadComplete);
                }
            }
            base.Dispose(disposing);
        }

        private void DisposeTemplateVerbs()
        {
            if (this._templateVerbs != null)
            {
                for (int i = 0; i < this._templateVerbs.Length; i++)
                {
                    this._templateVerbs[i].Dispose();
                }
                this._templateVerbs = null;
                this._templateVerbsDirty = true;
            }
        }

        protected virtual DeviceSpecificChoice FindChoiceInDeviceSpecific(string filter)
        {
            return Utils.FindChoiceInDeviceSpecific((filter == null) ? string.Empty : filter, this._mobileControl.get_DeviceSpecific());
        }

        protected override TemplateEditingVerb[] GetCachedTemplateEditingVerbs()
        {
            if (this.ErrorMode || !this.AllowTemplateEditing)
            {
                return null;
            }
            if (this._templateVerbsDirty && !base.InTemplateMode)
            {
                this.DisposeTemplateVerbs();
                this._templateVerbs = this.GetTemplateVerbs();
                this._templateVerbsDirty = false;
            }
            foreach (TemplateEditingVerb verb in this._templateVerbs)
            {
                verb.Enabled = this.AllowTemplateEditing;
            }
            return this._templateVerbs;
        }

        protected virtual string GetDesignTimeErrorHtml(string errorMessage, bool infoMode)
        {
            return Utils.GetDesignTimeErrorHtml(errorMessage, infoMode, this._mobileControl, base.Behavior, this.ContainmentStatus);
        }

        public override string GetDesignTimeHtml()
        {
            IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            if ((service != null) && service.Loading)
            {
                return string.Empty;
            }
            bool infoMode = false;
            string errorMessage = this.GetErrorMessage(out infoMode);
            this.SetStyleAttributes();
            if (errorMessage != null)
            {
                return this.GetDesignTimeErrorHtml(errorMessage, infoMode);
            }
            try
            {
                return this.GetDesignTimeNormalHtml();
            }
            catch (Exception exception)
            {
                return this.GetDesignTimeErrorHtml(exception.Message, false);
            }
        }

        protected virtual string GetDesignTimeNormalHtml()
        {
            HtmlMobileTextWriter designerTextWriter = Utils.GetDesignerTextWriter();
            this.Adapter.Render(designerTextWriter);
            return designerTextWriter.ToString();
        }

        protected override string GetEmptyDesignTimeHtml()
        {
            return ("<div style='width:100%'>" + base.GetEmptyDesignTimeHtml() + "</div>");
        }

        protected virtual string GetErrorMessage(out bool infoMode)
        {
            infoMode = false;
            if (!Utils.InMobileUserControl(this._mobileControl))
            {
                if (Utils.InUserControl(this._mobileControl))
                {
                    infoMode = true;
                    return Constants.UserControlWarningMessage;
                }
                if (!Utils.InMobilePage(this._mobileControl))
                {
                    return Constants.MobilePageErrorMessage;
                }
                if (!this.ValidContainment)
                {
                    return Constants.FormPanelContainmentErrorMessage;
                }
            }
            return null;
        }

        public override string GetPersistInnerHtml()
        {
            string str = null;
            if (base.InTemplateMode)
            {
                base.SaveActiveTemplateEditingFrame();
            }
            if (base.IsDirty)
            {
                str = Utils.InvokePersistInnerProperties(this._mobileControl);
            }
            if (base.InTemplateMode)
            {
                base.IsDirty = true;
            }
            return str;
        }

        public override string GetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, out bool allowEditing)
        {
            allowEditing = true;
            ITemplate template = null;
            string textFromTemplate = string.Empty;
            DeviceSpecificChoice choice = this.FindChoiceInDeviceSpecific(this.ActiveDeviceFilter);
            if (choice != null)
            {
                template = (ITemplate) choice.get_Templates()[templateName];
            }
            if (template != null)
            {
                textFromTemplate = base.GetTextFromTemplate(template);
            }
            return textFromTemplate;
        }

        protected abstract string[] GetTemplateFrameNames(int index);
        protected abstract TemplateEditingVerb[] GetTemplateVerbs();
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            this._mobileControl = (MobileControl) component;
            this._defaultValues = new Hashtable();
            this._overridenValues = new Hashtable();
            this._deviceSpecificSelectionProvider = (IDeviceSpecificSelectionProvider) this.GetService(typeof(IDeviceSpecificSelectionProvider));
            this.InitializeDefaultProperties();
            this.InitializeOverridenProperties();
            if (this._deviceSpecificSelectionProvider != null)
            {
                this._deviceSpecificSelectionProvider.SelectionChanged += new EventHandler(this.OnHostDeviceSpecificChanged);
                this.ActiveDeviceFilter = this._deviceSpecificSelectionProvider.CurrentFilter;
                if (this._mobileControl.get_DeviceSpecific() != null)
                {
                    foreach (DeviceSpecificChoice choice in this._mobileControl.get_DeviceSpecific().get_Choices())
                    {
                        string filterName = Utils.CreateUniqueChoiceName(choice);
                        if (!this._deviceSpecificSelectionProvider.FilterExists(filterName))
                        {
                            this._deviceSpecificSelectionProvider.AddFilter(filterName);
                        }
                    }
                }
            }
            this.PrepareProperties();
            IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            if (service != null)
            {
                service.LoadComplete += new EventHandler(this.OnLoadComplete);
            }
        }

        protected void InitializeDefaultProperties()
        {
            Utils.InitializeDefaultProperties(this._mobileControl, this._defaultValues);
        }

        protected void InitializeOverridenProperties()
        {
            Utils.InitializeOverridenProperties(this._mobileControl, this._overridenValues);
        }

        public override void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
        {
            PropertyDescriptor member = ce.Member as PropertyDescriptor;
            string key = null;
            object propertyValue = null;
            object component = ce.Component;
            base.OnComponentChanged(sender, ce);
            if ((this.ActiveDeviceFilter != null) && (this._overridenValues[this.ActiveDeviceFilter] == null))
            {
                this._overridenValues[this.ActiveDeviceFilter] = new Hashtable();
            }
            if (member != null)
            {
                if (member.Converter is ExpandableObjectConverter)
                {
                    Utils.ChangedSubProperty property = Utils.FindChangedSubProperty(string.Empty, this._mobileControl, member, (this.ActiveDeviceFilter != null) ? ((Hashtable) this._overridenValues[this.ActiveDeviceFilter]) : null, this._defaultValues);
                    component = property.parentObject;
                    member = property.propertyDescriptor;
                    key = property.propertyName;
                    propertyValue = property.propertyValue;
                }
                else
                {
                    key = ce.Member.Name;
                    propertyValue = ce.NewValue;
                }
                if (!this._isResetting)
                {
                    if ((this.ActiveDeviceFilter != null) && this._deviceSpecificSelectionProvider.DeviceSpecificSelectionProviderEnabled)
                    {
                        IAttributeAccessor currentChoice = this.CurrentChoice;
                        base.IsDirty = true;
                        if (currentChoice != null)
                        {
                            currentChoice.SetAttribute(key, member.Converter.ConvertToInvariantString(propertyValue));
                            ((Hashtable) this._overridenValues[this.ActiveDeviceFilter])[key] = propertyValue;
                        }
                        if (propertyValue.Equals(Utils.GetDefaultAttributeValue(member)))
                        {
                            this.CurrentChoice.get_Contents().Remove(key);
                            ((Hashtable) this._overridenValues[this.ActiveDeviceFilter]).Remove(key);
                            if (((Hashtable) this._overridenValues[this.ActiveDeviceFilter]).Count == 0)
                            {
                                this._overridenValues.Remove(this.ActiveDeviceFilter);
                            }
                            if (Utils.IsChoiceEmpty(this.CurrentChoice) && (this._mobileControl.get_DeviceSpecific() != null))
                            {
                                this._mobileControl.get_DeviceSpecific().get_Choices().Remove(this.CurrentChoice);
                            }
                            this._isResetting = true;
                            member.SetValue(component, this._defaultValues[key]);
                            this._isResetting = false;
                        }
                    }
                    else
                    {
                        this._defaultValues[key] = propertyValue;
                    }
                }
            }
        }

        protected virtual void OnCurrentDeviceSpecificChanged()
        {
            if (this._deviceSpecificSelectionProvider != null)
            {
                this.ActiveDeviceFilter = this._deviceSpecificSelectionProvider.CurrentFilter;
                this.PrepareProperties();
                TypeDescriptor.Refresh(base.Component);
                this.UpdateRendering();
            }
        }

        private void OnHostDeviceSpecificChanged(object sender, EventArgs e)
        {
            this.OnCurrentDeviceSpecificChanged();
        }

        private void OnLoadComplete(object sender, EventArgs e)
        {
            if ((this._deviceSpecificSelectionProvider == null) || this._deviceSpecificSelectionProvider.DeviceSpecificSelectionProviderEnabled)
            {
                this.UpdateRendering();
            }
        }

        public override void OnSetParent()
        {
            base.OnSetParent();
            this._containmentStatusDirty = true;
            IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            if ((service == null) || !service.Loading)
            {
                this.UpdateRendering();
            }
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            if (((this.ActiveDeviceFilter != null) && (this._deviceSpecificSelectionProvider != null)) && this._deviceSpecificSelectionProvider.DeviceSpecificSelectionProviderEnabled)
            {
                Utils.FilterNonOverridableProperties(properties);
            }
        }

        protected void PrepareProperties()
        {
            this.ApplyDefaultProperties();
            this.ApplyPropertyOverrides();
        }

        protected virtual void SetSelectedDeviceSpecificChoice(string activeDeviceFilter)
        {
            if (this._mobileControl.get_DeviceSpecific() != null)
            {
                DeviceSpecificChoice choice = null;
                choice = Utils.FindChoiceInDeviceSpecific(activeDeviceFilter, this._mobileControl.get_DeviceSpecific());
                Utils.SetDeviceSpecificChoice(this._mobileControl, choice);
            }
        }

        protected virtual void SetStyleAttributes()
        {
            Utils.SetStandardStyleAttributes(base.Behavior, this.ContainmentStatus);
        }

        public override void SetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, string templateContent)
        {
            ITemplate templateFromText = null;
            if ((templateContent != null) && (templateContent.Length != 0))
            {
                templateFromText = base.GetTemplateFromText(templateContent);
            }
            else
            {
                this.CurrentChoice.get_Templates().Remove(templateName);
                if (Utils.IsChoiceEmpty(this.CurrentChoice) && (this._mobileControl.get_DeviceSpecific() != null))
                {
                    this._mobileControl.get_DeviceSpecific().get_Choices().Remove(this.CurrentChoice);
                    Utils.SetDeviceSpecificChoice(this._mobileControl, null);
                }
                return;
            }
            this.CurrentChoice.get_Templates()[templateName] = templateFromText;
        }

        protected internal void SetTemplateVerbsDirty()
        {
            this._templateVerbsDirty = true;
        }

        public virtual void UpdateRendering()
        {
            Utils.RefreshMobileControlStyle(this._mobileControl);
            this.UpdateDesignTimeHtml();
        }

        public virtual string ActiveDeviceFilter
        {
            get
            {
                return this._activeDeviceFilter;
            }
            set
            {
                string activeDeviceFilter = string.Empty;
                if (value != null)
                {
                    activeDeviceFilter = value;
                }
                this.SetSelectedDeviceSpecificChoice(activeDeviceFilter);
                this._activeDeviceFilter = value;
            }
        }

        protected virtual IControlAdapter Adapter
        {
            get
            {
                return this._mobileControl.get_Adapter();
            }
        }

        protected virtual bool AllowTemplateEditing
        {
            get
            {
                return !this.ErrorMode;
            }
        }

        protected Microsoft.Matrix.Framework.Mobile.UI.Design.ContainmentStatus ContainmentStatus
        {
            get
            {
                if (this._containmentStatusDirty)
                {
                    this._containmentStatus = Utils.GetContainmentStatus(this._mobileControl);
                    this._containmentStatusDirty = false;
                }
                return this._containmentStatus;
            }
        }

        protected virtual DeviceSpecificChoice CurrentChoice
        {
            get
            {
                DeviceSpecificChoice choice = null;
                string activeDeviceFilter = string.Empty;
                if (this.ActiveDeviceFilter != null)
                {
                    activeDeviceFilter = this.ActiveDeviceFilter;
                }
                if (this._mobileControl.get_DeviceSpecific() != null)
                {
                    if ((this._mobileControl.get_DeviceSpecific().get_SelectedChoice() != null) && this._mobileControl.get_DeviceSpecific().get_SelectedChoice().get_Filter().Equals(activeDeviceFilter))
                    {
                        return this._mobileControl.get_DeviceSpecific().get_SelectedChoice();
                    }
                    choice = Utils.FindChoiceInDeviceSpecific(activeDeviceFilter, this._mobileControl.get_DeviceSpecific());
                }
                else
                {
                    this._mobileControl.set_DeviceSpecific(new DeviceSpecific());
                }
                if (choice == null)
                {
                    choice = new DeviceSpecificChoice();
                    choice.set_Filter(Utils.GetFilterFromActiveDeviceFilter(activeDeviceFilter));
                    choice.set_Argument(Utils.GetArgumentFromActiveDeviceFilter(activeDeviceFilter));
                    int num = choice.get_Filter().Equals(string.Empty) ? this._mobileControl.get_DeviceSpecific().get_Choices().get_Count() : 0;
                    this._mobileControl.get_DeviceSpecific().get_Choices().AddAt(num, choice);
                }
                Utils.SetDeviceSpecificChoice(this._mobileControl, choice);
                return choice;
            }
        }

        protected virtual bool ErrorMode
        {
            get
            {
                bool flag;
                return ((this.GetErrorMessage(out flag) != null) && !flag);
            }
        }

        private bool ValidContainment
        {
            get
            {
                if ((this.ContainmentStatus != Microsoft.Matrix.Framework.Mobile.UI.Design.ContainmentStatus.InForm) && (this.ContainmentStatus != Microsoft.Matrix.Framework.Mobile.UI.Design.ContainmentStatus.InPanel))
                {
                    return (this.ContainmentStatus == Microsoft.Matrix.Framework.Mobile.UI.Design.ContainmentStatus.InTemplateFrame);
                }
                return true;
            }
        }

        protected virtual Style WebCtrlStyle
        {
            get
            {
                return Utils.MapToWebControlStyle(this._mobileControl);
            }
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        internal class TemplateContainer
        {
            [TemplateContainer(typeof(MobileListItem))]
            public ITemplate AlternatingItemTemplate
            {
                get
                {
                    return null;
                }
            }

            [TemplateContainer(typeof(MobileListItem))]
            public ITemplate ContentTemplate
            {
                get
                {
                    return null;
                }
            }

            [TemplateContainer(typeof(MobileListItem))]
            public ITemplate FooterTemplate
            {
                get
                {
                    return null;
                }
            }

            [TemplateContainer(typeof(MobileListItem))]
            public ITemplate HeaderTemplate
            {
                get
                {
                    return null;
                }
            }

            [TemplateContainer(typeof(MobileListItem))]
            public ITemplate ItemDetailsTemplate
            {
                get
                {
                    return null;
                }
            }

            [TemplateContainer(typeof(MobileListItem))]
            public ITemplate ItemTemplate
            {
                get
                {
                    return null;
                }
            }

            [TemplateContainer(typeof(MobileListItem))]
            public ITemplate LabelTemplate
            {
                get
                {
                    return null;
                }
            }

            [TemplateContainer(typeof(MobileListItem))]
            public ITemplate SeparatorTemplate
            {
                get
                {
                    return null;
                }
            }
        }
    }
}

