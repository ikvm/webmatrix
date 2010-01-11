namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design.Editors;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Runtime.InteropServices;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.Design.MobileControls;
    using System.Web.UI.MobileControls;

    public class StyleSheetDesigner : MobileTemplatedControlDesigner
    {
        private const int _contentTemplate = 3;
        private Style _currentStyle;
        private ArrayList _cycledStyles;
        private DesignerVerbCollection _designerVerbs;
        private const string _designTimeHTML = "\r\n                <table cellpadding=4 cellspacing=0 width='300px' style='font-family:tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow'>\r\n                  <tr><td colspan=2><span style='font-weight:bold'>StyleSheet</span> - {0}</td></tr>\r\n                  <tr><td style='padding-top:0;padding-bottom:0;width:55%;padding-left:10px;font-weight:bold'>Template Style:</td><td style='padding-top:0;padding-bottom:0'>{1}</td></tr>\r\n                  <tr><td colspan=2 style='padding-top:4px'>{2}</td></tr>\r\n                </table>\r\n             ";
        private const string _duplicateStyleMessage = "You can specify only one style sheet per page. Please remove this control.";
        private const string _duplicateStyleNamesMessage = "This style sheet contains multiple styles with identical names. Each style must have a unique 'Name' attribute.";
        private static readonly Attribute[] _emptyAttrs = new Attribute[0];
        private const int _headerFooterTemplates = 0;
        private bool _isDuplicate;
        private const int _itemTemplates = 1;
        private const int _numberOfTemplateFrames = 4;
        private const string _persistedStylesPropName = "PersistedStyles";
        private const int _separatorTemplate = 2;
        private const string _specialCaseDesignTimeHTML = "\r\n                <table cellpadding=4 cellspacing=0 width='300px' style='font-family:tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow'>\r\n                  <tr><td colspan=2><span style='font-weight:bold'>StyleSheet</span> - {0}</td></tr>\r\n                  <tr><td style='padding-top:0;padding-bottom:0;width:55%;padding-left:10px;font-weight:bold'>Template Style:</td><td style='padding-top:0;padding-bottom:0'>{1}</td></tr>\r\n                  <tr><td colspan=2 style='padding-top:4px'>{2}</td></tr>\r\n                  <tr><td colspan=2>\r\n                    <table style='font-size:8pt;color:window;background-color:ButtonShadow'>\r\n                      <tr><td valign='top'><img src='{3}'/></td><td>{4}</td></tr>\r\n                    </table>\r\n                  </td></tr>\r\n                </table>\r\n             ";
        private StyleSheet _styleSheet;
        private const string _styleSheetDefaultMessage = "Choose the Edit Styles command or use the Property Pages to define styles.";
        private static readonly string[][] _templateFrameNames = new string[][] { new string[] { Constants.HeaderTemplateTag, Constants.FooterTemplateTag }, new string[] { Constants.ItemTemplateTag, Constants.AlternatingItemTemplateTag, Constants.ItemDetailsTemplateTag }, new string[] { Constants.SeparatorTemplateTag }, new string[] { Constants.ContentTemplateTag } };
        private const string _templatesStylePropName = "TemplateStyle";
        private const string _templateStyleKey = "__TemplateStyle";

        private ArrayList DetectCycles()
        {
            if (this._cycledStyles == null)
            {
                this._cycledStyles = new ArrayList();
                ICollection is2 = this._styleSheet.get_Styles();
                foreach (string str in is2)
                {
                    Style style = this._styleSheet.get_Item(str);
                    bool flag = false;
                    string strB = style.get_StyleReference();
                    string strA = style.get_Name();
                    int num = is2.Count + 1;
                    while (((strB != null) && (strB.Length > 0)) && (num > 0))
                    {
                        if (string.Compare(strA, strB, true) == 0)
                        {
                            flag = true;
                            break;
                        }
                        Style style2 = this._styleSheet.get_Item(strB);
                        if (style2 != null)
                        {
                            strB = style2.get_StyleReference();
                            num--;
                        }
                        else
                        {
                            strB = null;
                        }
                    }
                    if (flag)
                    {
                        this._cycledStyles.Add(style);
                    }
                }
            }
            return this._cycledStyles;
        }

        protected override void Dispose(bool disposing)
        {
            if ((disposing && (this.MobilePage != null)) && (this.MobilePage.get_StyleSheet() == this._styleSheet))
            {
                this.MobilePage.set_StyleSheet(null);
                Utils.RefreshPageView(this.MobilePage);
            }
            base.Dispose(disposing);
        }

        protected override DeviceSpecificChoice FindChoiceInDeviceSpecific(string filter)
        {
            if (this.CurrentStyle == null)
            {
                return null;
            }
            return Utils.FindChoiceInDeviceSpecific((filter == null) ? string.Empty : filter, this.CurrentStyle.get_DeviceSpecific());
        }

        protected override string GetDesignTimeNormalHtml()
        {
            string styleSheetPropNotSet;
            if (this.CurrentStyle == null)
            {
                styleSheetPropNotSet = Constants.StyleSheetPropNotSet;
            }
            else
            {
                styleSheetPropNotSet = HttpUtility.HtmlEncode(this.CurrentStyle.get_Name());
            }
            string str2 = "Choose the Edit Styles command or use the Property Pages to define styles.";
            string errorIconUrl = Constants.ErrorIconUrl;
            if (this._isDuplicate)
            {
                return string.Format("\r\n                <table cellpadding=4 cellspacing=0 width='300px' style='font-family:tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow'>\r\n                  <tr><td colspan=2><span style='font-weight:bold'>StyleSheet</span> - {0}</td></tr>\r\n                  <tr><td style='padding-top:0;padding-bottom:0;width:55%;padding-left:10px;font-weight:bold'>Template Style:</td><td style='padding-top:0;padding-bottom:0'>{1}</td></tr>\r\n                  <tr><td colspan=2 style='padding-top:4px'>{2}</td></tr>\r\n                  <tr><td colspan=2>\r\n                    <table style='font-size:8pt;color:window;background-color:ButtonShadow'>\r\n                      <tr><td valign='top'><img src='{3}'/></td><td>{4}</td></tr>\r\n                    </table>\r\n                  </td></tr>\r\n                </table>\r\n             ", new object[] { this._styleSheet.ID, styleSheetPropNotSet, str2, errorIconUrl, "You can specify only one style sheet per page. Please remove this control." });
            }
            ICollection duplicateStyles = Utils.GetDuplicateStyles(this._styleSheet);
            if ((duplicateStyles != null) && (duplicateStyles.Count > 0))
            {
                return string.Format("\r\n                <table cellpadding=4 cellspacing=0 width='300px' style='font-family:tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow'>\r\n                  <tr><td colspan=2><span style='font-weight:bold'>StyleSheet</span> - {0}</td></tr>\r\n                  <tr><td style='padding-top:0;padding-bottom:0;width:55%;padding-left:10px;font-weight:bold'>Template Style:</td><td style='padding-top:0;padding-bottom:0'>{1}</td></tr>\r\n                  <tr><td colspan=2 style='padding-top:4px'>{2}</td></tr>\r\n                  <tr><td colspan=2>\r\n                    <table style='font-size:8pt;color:window;background-color:ButtonShadow'>\r\n                      <tr><td valign='top'><img src='{3}'/></td><td>{4}</td></tr>\r\n                    </table>\r\n                  </td></tr>\r\n                </table>\r\n             ", new object[] { this._styleSheet.ID, styleSheetPropNotSet, str2, errorIconUrl, "This style sheet contains multiple styles with identical names. Each style must have a unique 'Name' attribute." });
            }
            return string.Format("\r\n                <table cellpadding=4 cellspacing=0 width='300px' style='font-family:tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow'>\r\n                  <tr><td colspan=2><span style='font-weight:bold'>StyleSheet</span> - {0}</td></tr>\r\n                  <tr><td style='padding-top:0;padding-bottom:0;width:55%;padding-left:10px;font-weight:bold'>Template Style:</td><td style='padding-top:0;padding-bottom:0'>{1}</td></tr>\r\n                  <tr><td colspan=2 style='padding-top:4px'>{2}</td></tr>\r\n                </table>\r\n             ", new object[] { this._styleSheet.ID, styleSheetPropNotSet, str2 });
        }

        protected override string GetErrorMessage(out bool infoMode)
        {
            infoMode = false;
            if (!Utils.InMobileUserControl(this._styleSheet))
            {
                if (Utils.InUserControl(this._styleSheet))
                {
                    infoMode = true;
                    return Constants.UserControlWarningMessage;
                }
                if (!Utils.InMobilePage(this._styleSheet))
                {
                    return Constants.MobilePageErrorMessage;
                }
            }
            if (base.ContainmentStatus != ContainmentStatus.AtTopLevel)
            {
                return Constants.TopPageContainmentErrorMessage;
            }
            return null;
        }

        protected override string[] GetTemplateFrameNames(int index)
        {
            return _templateFrameNames[index];
        }

        public override Type GetTemplatePropertyParentType(string templateName)
        {
            return typeof(MobileTemplatedControlDesigner.TemplateContainer);
        }

        protected override TemplateEditingVerb[] GetTemplateVerbs()
        {
            return new TemplateEditingVerb[] { new TemplateEditingVerb(Constants.TemplateFrameHeaderFooterTemplates, 0, this), new TemplateEditingVerb(Constants.TemplateFrameItemTemplates, 1, this), new TemplateEditingVerb(Constants.TemplateFrameSeparatorTemplate, 2, this), new TemplateEditingVerb(Constants.TemplateFrameContentTemplate, 3, this) };
        }

        public override void Initialize(IComponent component)
        {
            this._styleSheet = (StyleSheet) component;
            base.Initialize(component);
        }

        protected internal void InvokePropertyBuilder(int initialPage)
        {
            IComponentChangeService service = null;
            service = (IComponentChangeService) this.GetService(typeof(IComponentChangeService));
            if (service != null)
            {
                try
                {
                    service.OnComponentChanging(this._styleSheet, null);
                }
                catch (CheckoutException exception)
                {
                    if (exception != CheckoutException.Canceled)
                    {
                        throw;
                    }
                    return;
                }
            }
            try
            {
                new StyleSheetComponentEditor().EditComponent(this._styleSheet);
            }
            finally
            {
                if (service != null)
                {
                    service.OnComponentChanged(this._styleSheet, null, null, null);
                }
            }
        }

        public override void OnComponentChanged(object sender, ComponentChangedEventArgs e)
        {
            base.OnComponentChanged(sender, e);
        }

        protected void OnPropertyBuilder(object sender, EventArgs e)
        {
            this.InvokePropertyBuilder(0);
        }

        public override void OnSetParent()
        {
            base.OnSetParent();
            if ((this._styleSheet.Parent is System.Web.UI.MobileControls.MobilePage) || (this._styleSheet.Parent is MobileUserControl))
            {
                IDesignerHost service = (IDesignerHost) this.GetService(typeof(IDesignerHost));
                if ((this.MobilePage.get_StyleSheet() != StyleSheet.get_Default()) && (this.MobilePage.get_StyleSheet().ID != this._styleSheet.ID))
                {
                    this._isDuplicate = true;
                }
                else
                {
                    this.MobilePage.set_StyleSheet(this._styleSheet);
                }
            }
            this.UpdateRendering();
        }

        protected override void PreFilterAttributes(IDictionary attributes)
        {
            base.PreFilterAttributes(attributes);
            EditorAttribute attribute = new EditorAttribute(typeof(StyleSheetComponentEditor), typeof(ComponentEditor));
            attributes[attribute.TypeId] = attribute;
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            if (this.ActiveDeviceFilter != null)
            {
                PropertyDescriptor descriptor = (PropertyDescriptor) properties["ReferencePath"];
                Utils.AddAttributesToProperty(descriptor.ComponentType, properties, descriptor.Name, new Attribute[] { new BrowsableAttribute(false) });
            }
            PropertyDescriptor descriptor2 = TypeDescriptor.CreateProperty(base.GetType(), "TemplateStyle", typeof(string), new Attribute[] { base.InTemplateMode ? ReadOnlyAttribute.Yes : ReadOnlyAttribute.No, base.InTemplateMode ? BrowsableAttribute.No : BrowsableAttribute.Yes, new DefaultValueAttribute(Constants.StyleSheetPropNotSet), new TypeConverterAttribute("System.Web.UI.Design.MobileControls.Converters.StyleConverter," + Constants.MobileAssemblyFullName), new DescriptionAttribute(Constants.StyleSheetTemplateStyleDescription) });
            properties["TemplateStyle"] = descriptor2;
            PropertyDescriptor descriptor3 = TypeDescriptor.CreateProperty(base.GetType(), "PersistedStyles", typeof(ICollection), new Attribute[0]);
            properties["PersistedStyles"] = descriptor3;
        }

        internal void RefreshMobilePage()
        {
            if (this.MobilePage != null)
            {
                Utils.RefreshPageView(this.MobilePage);
            }
        }

        protected override void SetSelectedDeviceSpecificChoice(string activeDeviceFilter)
        {
            foreach (string str in this._styleSheet.get_Styles())
            {
                Style style = this._styleSheet.get_Item(str);
                if (style.get_DeviceSpecific() != null)
                {
                    DeviceSpecificChoice choice = Utils.FindChoiceInDeviceSpecific(activeDeviceFilter, style.get_DeviceSpecific());
                    DeviceSpecificChoice choice2 = null;
                    if (choice == null)
                    {
                        foreach (DeviceSpecificChoice choice3 in style.get_DeviceSpecific().get_Choices())
                        {
                            if (choice3.get_Filter().Equals(string.Empty))
                            {
                                choice2 = choice3;
                                break;
                            }
                        }
                        choice = choice2;
                    }
                    Utils.SetDeviceSpecificChoice(style, choice);
                }
            }
            Utils.RefreshPageView(this._styleSheet.get_MobilePage());
        }

        protected override void SetStyleAttributes()
        {
            string str = null;
            string str2 = null;
            string str3 = null;
            if (base.ContainmentStatus == ContainmentStatus.AtTopLevel)
            {
                str = "5px";
                str2 = "5px";
                str3 = "30%";
            }
            else
            {
                str = "3px";
                str2 = "3px";
                str3 = "5px";
            }
            base.Behavior.SetStyleAttribute("marginTop", true, str, true);
            base.Behavior.SetStyleAttribute("marginBottom", true, str2, true);
            base.Behavior.SetStyleAttribute("marginRight", true, str3, true);
            base.Behavior.SetStyleAttribute("marginLeft", true, "5px", true);
        }

        protected override IControlAdapter Adapter
        {
            get
            {
                return null;
            }
        }

        protected override bool AllowTemplateEditing
        {
            get
            {
                return (!this.ErrorMode && (this.CurrentStyle != null));
            }
        }

        protected override DeviceSpecificChoice CurrentChoice
        {
            get
            {
                if (this.CurrentStyle == null)
                {
                    return null;
                }
                if (this.CurrentStyle.get_DeviceSpecific() == null)
                {
                    this.CurrentStyle.set_DeviceSpecific(new DeviceSpecific());
                }
                if ((((this.ActiveDeviceFilter != null) || (this.CurrentStyle.get_DeviceSpecific().get_SelectedChoice() == null)) || !this.CurrentStyle.get_DeviceSpecific().get_SelectedChoice().get_Filter().Equals(string.Empty)) && ((this.CurrentStyle.get_DeviceSpecific().get_SelectedChoice() == null) || !this.CurrentStyle.get_DeviceSpecific().get_SelectedChoice().get_Filter().Equals(this.ActiveDeviceFilter)))
                {
                    DeviceSpecificChoice choice = new DeviceSpecificChoice();
                    choice.set_Filter((this.ActiveDeviceFilter == null) ? string.Empty : Utils.GetFilterFromActiveDeviceFilter(this.ActiveDeviceFilter));
                    choice.set_Argument((this.ActiveDeviceFilter == null) ? null : Utils.GetArgumentFromActiveDeviceFilter(this.ActiveDeviceFilter));
                    int num = choice.get_Filter().Equals(string.Empty) ? this.CurrentStyle.get_DeviceSpecific().get_Choices().get_Count() : 0;
                    this.CurrentStyle.get_DeviceSpecific().get_Choices().AddAt(num, choice);
                    Utils.SetDeviceSpecificChoice(this.CurrentStyle, choice);
                }
                return this.CurrentStyle.get_DeviceSpecific().get_SelectedChoice();
            }
        }

        public Style CurrentStyle
        {
            get
            {
                if (this._currentStyle == null)
                {
                    IMobileWebFormServices service = (IMobileWebFormServices) this.GetService(typeof(IMobileWebFormServices));
                    if (service != null)
                    {
                        string cache = service.GetCache(this._styleSheet.ID, "__TemplateStyle") as string;
                        if (cache != null)
                        {
                            this._currentStyle = this._styleSheet.get_Item(cache);
                        }
                    }
                    if ((this._currentStyle == null) && (this._styleSheet.get_Styles().Count > 0))
                    {
                        foreach (string str2 in this._styleSheet.get_Styles())
                        {
                            this._currentStyle = this._styleSheet.get_Item(str2);
                            break;
                        }
                    }
                }
                return this._currentStyle;
            }
            set
            {
                if (this._currentStyle != value)
                {
                    this._currentStyle = value;
                    IMobileWebFormServices service = (IMobileWebFormServices) this.GetService(typeof(IMobileWebFormServices));
                    if (service != null)
                    {
                        if (value == null)
                        {
                            service.SetCache(this._styleSheet.ID, "__TemplateStyle", string.Empty);
                        }
                        else
                        {
                            service.SetCache(this._styleSheet.ID, "__TemplateStyle", value.get_Name());
                        }
                    }
                }
            }
        }

        private System.Web.UI.MobileControls.MobilePage MobilePage
        {
            get
            {
                IComponent rootComponent = Utils.GetRootComponent(base.Component);
                if (rootComponent is MobileUserControl)
                {
                    return (((Control) rootComponent).Page as System.Web.UI.MobileControls.MobilePage);
                }
                return (rootComponent as System.Web.UI.MobileControls.MobilePage);
            }
        }

        [Browsable(false), PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public ICollection PersistedStyles
        {
            get
            {
                ICollection is2 = this._styleSheet.get_Styles();
                ArrayList list = new ArrayList();
                foreach (string str in is2)
                {
                    Style style = this._styleSheet.get_Item(str);
                    list.Add(style);
                }
                foreach (Style style2 in Utils.GetDuplicateStyles(this._styleSheet))
                {
                    list.Add(style2);
                }
                return list;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Design")]
        public string TemplateStyle
        {
            get
            {
                if (this.CurrentStyle == null)
                {
                    return Constants.StyleSheetPropNotSet;
                }
                return this.CurrentStyle.get_Name();
            }
            set
            {
                Style style = null;
                if (((value != null) && !value.Equals(string.Empty)) && !value.Equals(Constants.StyleSheetPropNotSet))
                {
                    foreach (string str in this._styleSheet.get_Styles())
                    {
                        Style style2 = this._styleSheet.get_Item(str);
                        if (style2.get_Name().Equals(value))
                        {
                            style = style2;
                            break;
                        }
                    }
                }
                this.CurrentStyle = style;
            }
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (this._designerVerbs == null)
                {
                    this._designerVerbs = new DesignerVerbCollection();
                    this._designerVerbs.Add(new DesignerVerb(Constants.StyleSheetStylesEditorVerb, new EventHandler(this.OnPropertyBuilder)));
                }
                this._designerVerbs[0].Enabled = this.ActiveDeviceFilter == null;
                return this._designerVerbs;
            }
        }
    }
}

