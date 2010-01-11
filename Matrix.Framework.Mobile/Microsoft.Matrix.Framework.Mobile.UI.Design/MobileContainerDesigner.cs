namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Web.UI.Design;
    using System.Web.UI.Design.MobileControls;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;

    public abstract class MobileContainerDesigner : MobileTemplatedControlDesigner, IMobileDesigner
    {
        private IDictionary _behaviorAttributes;
        private IHtmlControlDesignerBehavior _cachedBehavior;
        private string _currentErrorMessage;
        private readonly Size _defaultSize;
        private IDeviceSpecificSelectionProvider _deviceSpecificSelectionProvider;
        private bool _hasAttributesCached;
        private MobileControl _mobileControl;
        private bool _shouldDirtyPage;
        private bool _waitingForDeviceSpecific;

        public MobileContainerDesigner()
        {
            base.ReadOnly = false;
            this._defaultSize = new Size(300, 100);
            this._behaviorAttributes = new HybridDictionary();
        }

        private unsafe void ApplyPropertyToBehavior(string propName)
        {
            if (this.Style != null)
            {
                if ((propName == null) || propName.Equals("BackColor"))
                {
                    Color c = (Color) this.Style.get_Item(System.Web.UI.MobileControls.Style.BackColorKey, true);
                    this.SetBehaviorStyle("backgroundColor", ColorTranslator.ToHtml(c));
                }
                if ((propName == null) || propName.Equals("ForeColor"))
                {
                    Color color2 = (Color) this.Style.get_Item(System.Web.UI.MobileControls.Style.ForeColorKey, true);
                    this.SetBehaviorStyle("color", ColorTranslator.ToHtml(color2));
                }
                if ((propName == null) || propName.Equals("Font"))
                {
                    bool flag = *(this.Style.get_Item(System.Web.UI.MobileControls.Style.BoldKey, true)) == 1;
                    bool flag2 = *(this.Style.get_Item(System.Web.UI.MobileControls.Style.ItalicKey, true)) == 1;
                    FontSize size = *((FontSize*) this.Style.get_Item(System.Web.UI.MobileControls.Style.FontSizeKey, true));
                    string str = (string) this.Style.get_Item(System.Web.UI.MobileControls.Style.FontNameKey, true);
                    this.SetBehaviorStyle("fontWeight", flag ? "bold" : "normal");
                    this.SetBehaviorStyle("fontStyle", flag2 ? "italic" : "normal");
                    if (size == 3)
                    {
                        this.SetBehaviorStyle("fontSize", "medium");
                    }
                    else if (size == 2)
                    {
                        this.SetBehaviorStyle("fontSize", "x-small");
                    }
                    else
                    {
                        this.RemoveBehaviorStyle("fontSize");
                    }
                    this.SetBehaviorStyle("fontFamily", str);
                }
                if ((propName == null) || propName.Equals("Alignment"))
                {
                    Alignment alignment = *((Alignment*) this.Style.get_Item(System.Web.UI.MobileControls.Style.AlignmentKey, true));
                    bool flag3 = alignment == 0;
                    this.SetBehaviorStyle("textAlign", flag3 ? "" : Enum.Format(typeof(Alignment), alignment, "G"));
                }
            }
        }

        protected void CreateDeviceSpecific()
        {
            base._temporaryChoice = new DeviceSpecificChoice();
            string activeDeviceFilter = string.Empty;
            if (this.ActiveDeviceFilter != null)
            {
                activeDeviceFilter = this.ActiveDeviceFilter;
            }
            base._temporaryChoice.set_Filter(Utils.GetFilterFromActiveDeviceFilter(activeDeviceFilter));
            base._temporaryChoice.set_Argument(Utils.GetArgumentFromActiveDeviceFilter(activeDeviceFilter));
            if ((this._mobileControl.get_DeviceSpecific() == null) && this._deviceSpecificSelectionProvider.DeviceSpecificSelectionProviderEnabled)
            {
                this.InsertDeviceSpecific();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
                if (service != null)
                {
                    service.LoadComplete -= new EventHandler(this.OnLoadComplete);
                }
            }
            base.Dispose(disposing);
        }

        protected virtual Size GetDefaultSize()
        {
            return this._defaultSize;
        }

        protected override string GetErrorMessage(out bool infoMode)
        {
            infoMode = false;
            return null;
        }

        public override void Initialize(IComponent component)
        {
            this._mobileControl = (MobileControl) component;
            base.Initialize(component);
            IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            if (service != null)
            {
                service.LoadComplete += new EventHandler(this.OnLoadComplete);
            }
            this._deviceSpecificSelectionProvider = (IDeviceSpecificSelectionProvider) this.GetService(typeof(IDeviceSpecificSelectionProvider));
        }

        protected void InsertDeviceSpecific()
        {
            if (this._mobileControl.get_DeviceSpecific() == null)
            {
                Interop.IHTMLElement designTimeElement = (Interop.IHTMLElement) base.DesignTimeElement;
                if (designTimeElement != null)
                {
                    designTimeElement.SetInnerHTML("<mobile:DeviceSpecific runat=server />" + designTimeElement.GetInnerHTML());
                    this._waitingForDeviceSpecific = true;
                }
            }
        }

        private bool IsAppearanceAttribute(string propertyName)
        {
            if (((!propertyName.Equals("Font") && !propertyName.Equals("ForeColor")) && (!propertyName.Equals("BackColor") && !propertyName.Equals("Wrapping"))) && !propertyName.Equals("Alignment"))
            {
                return propertyName.Equals("StyleReference");
            }
            return true;
        }

        protected virtual void OnBackgroundImageChange(string message, bool infoMode)
        {
        }

        protected override void OnBehaviorAttached()
        {
            this._cachedBehavior = base.Behavior;
            this.PrefixDeviceSpecificTags();
            base.OnBehaviorAttached();
            if (this._hasAttributesCached)
            {
                this.ReloadBehaviorState();
            }
        }

        protected override void OnBehaviorDetaching()
        {
            this._cachedBehavior = null;
        }

        public override void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
        {
            base.OnComponentChanged(sender, ce);
            MemberDescriptor member = ce.Member;
            if ((member != null) && member.GetType().FullName.Equals(Constants.ReflectPropertyDescriptorTypeFullName))
            {
                PropertyDescriptor descriptor2 = (PropertyDescriptor) member;
                string name = descriptor2.Name;
                if (this.IsAppearanceAttribute(name) && !this._waitingForDeviceSpecific)
                {
                    this.UpdateRenderingRecursive();
                }
            }
        }

        protected virtual void OnContainmentChanged()
        {
        }

        protected override void OnCurrentDeviceSpecificChanged()
        {
            if (this._deviceSpecificSelectionProvider != null)
            {
                if (!Utils.IsChoiceEmpty(base._temporaryChoice))
                {
                    this.InsertDeviceSpecific();
                }
                else
                {
                    base._temporaryChoice = null;
                }
            }
            base.OnCurrentDeviceSpecificChanged();
            this.UpdateRenderingRecursive();
        }

        public virtual void OnDeviceSpecificApplied()
        {
            if (this._waitingForDeviceSpecific)
            {
                int num = base._temporaryChoice.get_Filter().Equals(string.Empty) ? this._mobileControl.get_DeviceSpecific().get_Choices().get_Count() : 0;
                this._mobileControl.get_DeviceSpecific().get_Choices().AddAt(num, base._temporaryChoice);
                Utils.SetDeviceSpecificChoice(this._mobileControl, base._temporaryChoice);
                base._temporaryChoice = null;
                this._waitingForDeviceSpecific = false;
            }
            else
            {
                if (this._deviceSpecificSelectionProvider != null)
                {
                    this.ActiveDeviceFilter = this._deviceSpecificSelectionProvider.CurrentFilter;
                }
                base.InitializeOverridenProperties();
                base.PrepareProperties();
            }
            this.UpdateRenderingRecursive();
        }

        public virtual void OnDeviceSpecificRemoved()
        {
            if (this._deviceSpecificSelectionProvider != null)
            {
                this.ActiveDeviceFilter = this._deviceSpecificSelectionProvider.CurrentFilter;
            }
            base.InitializeOverridenProperties();
            base.PrepareProperties();
            this.UpdateRenderingRecursive();
        }

        private void OnInternalChange()
        {
            ISite site = this._mobileControl.Site;
            if (site != null)
            {
                IComponentChangeService service = (IComponentChangeService) site.GetService(typeof(IComponentChangeService));
                if (service != null)
                {
                    try
                    {
                        service.OnComponentChanging(this._mobileControl, null);
                    }
                    catch (CheckoutException exception)
                    {
                        if (exception != CheckoutException.Canceled)
                        {
                            throw;
                        }
                        return;
                    }
                    service.OnComponentChanged(this._mobileControl, null, null, null);
                }
            }
        }

        private void OnLoadComplete(object source, EventArgs e)
        {
            if ((this._deviceSpecificSelectionProvider == null) || this._deviceSpecificSelectionProvider.DeviceSpecificSelectionProviderEnabled)
            {
                if (!this._hasAttributesCached)
                {
                    this.SetControlDefaultAppearance();
                    this.ApplyPropertyToBehavior(null);
                }
                bool infoMode = false;
                string errorMessage = this.GetErrorMessage(out infoMode);
                if ((errorMessage != this._currentErrorMessage) || !this._hasAttributesCached)
                {
                    this.OnBackgroundImageChange(errorMessage, infoMode);
                    this._currentErrorMessage = errorMessage;
                }
                this._hasAttributesCached = true;
                this.OnContainmentChanged();
                this.UpdateRenderingRecursive();
                if (this._shouldDirtyPage)
                {
                    this.OnInternalChange();
                    this._shouldDirtyPage = false;
                }
            }
        }

        public override void OnSetParent()
        {
            base.OnSetParent();
            IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            if ((service != null) && !service.Loading)
            {
                this.OnLoadComplete(this, EventArgs.Empty);
            }
        }

        private void PrefixDeviceSpecificTags()
        {
            Interop.IHTMLElement designTimeElement = (Interop.IHTMLElement) base.DesignTimeElement;
            string tagPrefix = ((IWebFormReferenceManager) this.GetService(typeof(IWebFormReferenceManager))).GetTagPrefix(typeof(DeviceSpecific));
            if ((tagPrefix == null) || (tagPrefix.Length == 0))
            {
                tagPrefix = "mobile";
            }
            Interop.IHTMLElementCollection children = (Interop.IHTMLElementCollection) designTimeElement.GetChildren();
            if (children != null)
            {
                bool flag = false;
                int num = 0;
                string p = string.Empty;
                for (int i = 0; i < children.GetLength(); i++)
                {
                    string outerHTML = ((Interop.IHTMLElement) children.Item(i, 0)).GetOuterHTML();
                    string strA = outerHTML.ToUpper();
                    if ((outerHTML.StartsWith("<") && !outerHTML.StartsWith("</")) && !outerHTML.EndsWith("/>"))
                    {
                        if (!strA.StartsWith("<" + tagPrefix.ToUpper() + ":"))
                        {
                            num++;
                        }
                    }
                    else if (outerHTML.StartsWith("</"))
                    {
                        num--;
                    }
                    if (((1 == num) && strA.StartsWith("<DEVICESPECIFIC")) && strA.EndsWith(">"))
                    {
                        p = p + "<" + tagPrefix + ":DeviceSpecific runat=\"server\">\r\n";
                        flag = true;
                    }
                    else if (((1 == num) && strA.StartsWith("<DEVICESPECIFIC")) && strA.EndsWith("/>"))
                    {
                        string str5 = p;
                        p = str5 + "<" + tagPrefix + ":DeviceSpecific runat=\"server\"></" + tagPrefix + ":DeviceSpecific>\r\n";
                        flag = true;
                    }
                    else if ((num == 0) && (string.Compare(strA, "</DEVICESPECIFIC>", false) == 0))
                    {
                        p = p + "</" + tagPrefix + ":DeviceSpecific>\r\n";
                    }
                    else
                    {
                        p = p + outerHTML + "\r\n";
                    }
                }
                if (flag)
                {
                    this._shouldDirtyPage = true;
                    designTimeElement.SetInnerHTML(p);
                }
            }
        }

        private void ReloadBehaviorState()
        {
            IDictionaryEnumerator enumerator = this._behaviorAttributes.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string key = (string) enumerator.Key;
                object obj2 = this._behaviorAttributes[key];
                base.Behavior.SetStyleAttribute(key, true, obj2, true);
            }
        }

        protected void RemoveBehaviorStyle(string attribute)
        {
            if (base.Behavior != null)
            {
                base.Behavior.RemoveStyleAttribute(attribute, true, true);
            }
            this._behaviorAttributes.Remove(attribute);
        }

        protected void SetBehaviorStyle(string attribute, object obj)
        {
            this._behaviorAttributes[attribute] = obj;
            if (base.Behavior != null)
            {
                base.Behavior.SetStyleAttribute(attribute, true, obj, true);
            }
        }

        protected virtual void SetControlDefaultAppearance()
        {
            this.SetBehaviorStyle("borderWidth", "1px");
            this.SetBehaviorStyle("borderColor", ColorTranslator.ToHtml(SystemColors.ControlDark));
            this.SetBehaviorStyle("paddingTop", "8px");
            this.SetBehaviorStyle("paddingBottom", "8px");
            this.SetBehaviorStyle("paddingRight", "4px");
            this.SetBehaviorStyle("paddingLeft", "5px");
            this.SetBehaviorStyle("marginTop", "3px");
            this.SetBehaviorStyle("marginBottom", "3px");
            this.SetBehaviorStyle("marginRight", "5px");
            this.SetBehaviorStyle("marginLeft", "5px");
            this.SetBehaviorStyle("backgroundRepeat", "no-repeat");
            this.SetBehaviorStyle("backgroundAttachment", "fixed");
            this.SetBehaviorStyle("backgroundPositionX", "left");
            this.SetBehaviorStyle("backgroundPositionY", "top");
            this.SetBehaviorStyle("height", this.GetDefaultSize().Height);
            this.SetBehaviorStyle("width", this.GetDefaultSize().Width);
        }

        public override void SetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, string templateContent)
        {
            base.SetTemplateContent(editingFrame, templateName, templateContent);
        }

        public override void UpdateRendering()
        {
            if (!this._waitingForDeviceSpecific)
            {
                Utils.RefreshMobileControlStyle(this._mobileControl);
                this.ApplyPropertyToBehavior(null);
                Utils.UpdateRenderingRecursive(this._mobileControl);
            }
        }

        private void UpdateRenderingRecursive()
        {
            this.UpdateRendering();
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return null;
            }
        }

        protected override DeviceSpecificChoice CurrentChoice
        {
            get
            {
                if (base._temporaryChoice != null)
                {
                    return base._temporaryChoice;
                }
                if (this._mobileControl.get_DeviceSpecific() == null)
                {
                    this.CreateDeviceSpecific();
                    return base._temporaryChoice;
                }
                return base.CurrentChoice;
            }
        }

        protected System.Web.UI.MobileControls.Style Style
        {
            get
            {
                if (!Utils.InMobilePage(this._mobileControl))
                {
                    return null;
                }
                return ((ControlAdapter) this._mobileControl.get_Adapter()).get_Style();
            }
        }
    }
}

