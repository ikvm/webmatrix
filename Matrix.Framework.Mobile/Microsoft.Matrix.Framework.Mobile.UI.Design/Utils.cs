namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.Design.MobileControls;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;
    using System.Web.UI.WebControls;
    using System.Windows.Forms;

    internal sealed class Utils
    {
        private static PropertyInfo _styleSheetDuplicateStyles = null;

        private Utils()
        {
        }

        internal static void AddAttributesToPropertiesOfDifferentType(Type designerType, Type newType, IDictionary properties, string propertyName, Attribute newAttribute)
        {
            PropertyDescriptor descriptor = (PropertyDescriptor) properties[propertyName];
            AttributeCollection attributes = descriptor.Attributes;
            Attribute[] array = new Attribute[attributes.Count + 1];
            attributes.CopyTo(array, 0);
            array[attributes.Count] = newAttribute;
            descriptor = TypeDescriptor.CreateProperty(designerType, propertyName, newType, array);
            properties[propertyName] = descriptor;
        }

        internal static void AddAttributesToProperty(Type designerType, IDictionary properties, string propertyName, Attribute[] attributeArray)
        {
            PropertyDescriptor oldPropertyDescriptor = (PropertyDescriptor) properties[propertyName];
            oldPropertyDescriptor = TypeDescriptor.CreateProperty(designerType, oldPropertyDescriptor, attributeArray);
            properties[propertyName] = oldPropertyDescriptor;
        }

        internal static void ApplyPropertyValues(MobileControl mobileControl, Hashtable overrides)
        {
            if ((mobileControl != null) && (overrides != null))
            {
                IDictionaryEnumerator enumerator = overrides.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    int num;
                    string str2;
                    object obj2 = mobileControl;
                    string key = (string) enumerator.Key;
                    object obj3 = enumerator.Value;
                    if (!(key.ToLower() == "id"))
                    {
                        goto Label_006A;
                    }
                    continue;
                Label_0039:
                    str2 = key.Substring(0, num);
                    obj2 = obj2.GetType().GetProperty(str2, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).GetValue(obj2, null);
                    key = key.Substring(num + 1);
                Label_006A:
                    if ((num = key.IndexOf("-")) != -1)
                    {
                        goto Label_0039;
                    }
                    FindAndApplyPropertyValue(obj2, key, obj3);
                }
            }
        }

        internal static Control CreateMSHTMLHost()
        {
            return (Control) Activator.CreateInstance(Type.GetType("System.Web.UI.Design.MobileControls.Util.MSHTMLHost," + Constants.MobileAssemblyFullName), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, null, CultureInfo.InvariantCulture);
        }

        internal static string CreateUniqueChoiceName(DeviceSpecificChoice choice)
        {
            string str = choice.get_Filter();
            string str2 = choice.get_Argument();
            if (str2 == null)
            {
                return str;
            }
            return string.Format("{0} (\"{1}\")", str, str2);
        }

        internal static void FilterNonOverridableProperties(IDictionary properties)
        {
            IDictionaryEnumerator enumerator = properties.GetEnumerator();
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            ArrayList list3 = new ArrayList();
            while (enumerator.MoveNext())
            {
                PropertyDescriptor property = enumerator.Value as PropertyDescriptor;
                if (!property.Name.Equals("DeviceSpecific"))
                {
                    if (!IsOverridableProperty(property))
                    {
                        list.Add(enumerator.Value);
                        if (IsCollection(property))
                        {
                            list3.Add(enumerator.Value);
                        }
                    }
                    else
                    {
                        PersistenceModeAttribute attribute = property.Attributes[typeof(PersistenceModeAttribute)] as PersistenceModeAttribute;
                        if ((attribute != null) && (attribute.Mode == PersistenceMode.Attribute))
                        {
                            list2.Add(enumerator.Value);
                        }
                    }
                }
            }
            foreach (PropertyDescriptor descriptor2 in list3)
            {
                AddAttributesToProperty(descriptor2.ComponentType, properties, descriptor2.Name, new Attribute[] { new BrowsableAttribute(false) });
            }
            foreach (PropertyDescriptor descriptor3 in list)
            {
                AddAttributesToProperty(descriptor3.ComponentType, properties, descriptor3.Name, new Attribute[] { new ReadOnlyAttribute(true) });
            }
            foreach (PropertyDescriptor descriptor4 in list2)
            {
                AddAttributesToProperty(descriptor4.ComponentType, properties, descriptor4.Name, new Attribute[] { new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden) });
            }
        }

        internal static bool FindAndApplyPropertyValue(object parentObject, string name, object value)
        {
            PropertyInfo property = parentObject.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property == null)
            {
                return false;
            }
            Type propertyType = property.PropertyType;
            property.SetValue(parentObject, value, null);
            return true;
        }

        internal static ChangedSubProperty FindChangedSubProperty(string prefix, object parent, PropertyDescriptor property, Hashtable _overridenValues, Hashtable _defaultValues)
        {
            object obj2 = property.GetValue(parent);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(property.PropertyType);
            string str = prefix + property.Name + "-";
            string key = null;
            foreach (PropertyDescriptor descriptor in properties)
            {
                if (descriptor.Converter is ExpandableObjectConverter)
                {
                    ChangedSubProperty property2 = FindChangedSubProperty(str, obj2, descriptor, _overridenValues, _defaultValues);
                    if (property2 != null)
                    {
                        return property2;
                    }
                    continue;
                }
                object obj3 = descriptor.GetValue(obj2);
                key = str + descriptor.Name;
                if (_defaultValues.Contains(key))
                {
                    object obj4 = ((_overridenValues != null) && _overridenValues.Contains(key)) ? _overridenValues[key] : _defaultValues[key];
                    if (((obj4 != null) && !obj4.Equals(obj3)) || ((obj4 == null) && (obj3 != null)))
                    {
                        return new ChangedSubProperty(obj2, descriptor, key, obj3);
                    }
                }
            }
            return null;
        }

        internal static DeviceSpecificChoice FindChoiceInDeviceSpecific(string activeDeviceFilter, DeviceSpecific ds)
        {
            if (ds != null)
            {
                string filterFromActiveDeviceFilter = GetFilterFromActiveDeviceFilter(activeDeviceFilter);
                string argumentFromActiveDeviceFilter = GetArgumentFromActiveDeviceFilter(activeDeviceFilter);
                foreach (DeviceSpecificChoice choice in ds.get_Choices())
                {
                    if ((string.Compare(choice.get_Filter(), filterFromActiveDeviceFilter, true) == 0) && (string.Compare(choice.get_Argument(), argumentFromActiveDeviceFilter, true) == 0))
                    {
                        return choice;
                    }
                }
            }
            return null;
        }

        internal static string GetArgumentFromActiveDeviceFilter(string activeDeviceFilter)
        {
            if (activeDeviceFilter != null)
            {
                int index = activeDeviceFilter.IndexOf("(\"");
                int num2 = activeDeviceFilter.LastIndexOf("\")");
                if ((index != -1) && (num2 >= (index + 2)))
                {
                    return activeDeviceFilter.Substring(index + 2, (num2 - index) - 2);
                }
            }
            return null;
        }

        internal static ContainmentStatus GetContainmentStatus(Control control)
        {
            ContainmentStatus unknown = ContainmentStatus.Unknown;
            Control parent = control.Parent;
            if ((control != null) && (parent != null))
            {
                if (InTemplateFrame(control))
                {
                    return ContainmentStatus.InTemplateFrame;
                }
                if (parent is Form)
                {
                    return ContainmentStatus.InForm;
                }
                if (parent is Panel)
                {
                    return ContainmentStatus.InPanel;
                }
                if ((parent is Page) || (parent is UserControl))
                {
                    return ContainmentStatus.AtTopLevel;
                }
            }
            return unknown;
        }

        internal static IDesigner GetControlDesigner(IComponent component)
        {
            ISite site = component.Site;
            if (site != null)
            {
                return ((IDesignerHost) site.GetService(typeof(IDesignerHost))).GetDesigner(component);
            }
            return null;
        }

        internal static object GetDefaultAttributeValue(PropertyDescriptor property)
        {
            DefaultValueAttribute attribute = (DefaultValueAttribute) property.Attributes[typeof(DefaultValueAttribute)];
            return attribute.Value;
        }

        internal static HtmlMobileTextWriter GetDesignerTextWriter()
        {
            return (HtmlMobileTextWriter) Activator.CreateInstance(Type.GetType("System.Web.UI.Design.MobileControls.Adapters.DesignerTextWriter," + Constants.MobileAssemblyFullName), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[1], CultureInfo.InvariantCulture);
        }

        internal static HtmlMobileTextWriter GetDesignerTextWriter(bool maintainState)
        {
            return (HtmlMobileTextWriter) Activator.CreateInstance(Type.GetType("System.Web.UI.Design.MobileControls.Adapters.DesignerTextWriter," + Constants.MobileAssemblyFullName), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[] { maintainState }, CultureInfo.InvariantCulture);
        }

        internal static string GetDesignTimeErrorHtml(string errorMessage, bool infoMode, Control control, IHtmlControlDesignerBehavior behavior, ContainmentStatus containmentStatus)
        {
            string name = string.Empty;
            if (control.Site != null)
            {
                name = control.Site.Name;
            }
            behavior.SetStyleAttribute("borderWidth", true, "0px", true);
            return string.Format(Constants.DefaultErrorDesignTimeHTML, new object[] { control.GetType().Name, name, errorMessage, infoMode ? Constants.InfoIconUrl : Constants.ErrorIconUrl, (containmentStatus == ContainmentStatus.AtTopLevel) ? Constants.ControlSizeAtToplevelInErrormode : Constants.ControlSizeInContainer });
        }

        internal static object GetDeviceSpecificOwner(DeviceSpecific deviceSpecific)
        {
            return typeof(DeviceSpecific).InvokeMember("_owner", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance, null, deviceSpecific, null);
        }

        public static ICollection GetDuplicateStyles(StyleSheet styleSheet)
        {
            ICollection is2 = null;
            if (_styleSheetDuplicateStyles == null)
            {
                _styleSheetDuplicateStyles = typeof(StyleSheet).GetProperty("DuplicateStyles", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            }
            if ((_styleSheetDuplicateStyles != null) && (styleSheet != null))
            {
                is2 = (ICollection) _styleSheetDuplicateStyles.GetValue(styleSheet, null);
            }
            return is2;
        }

        internal static string GetFilterFromActiveDeviceFilter(string activeDeviceFilter)
        {
            if (activeDeviceFilter != null)
            {
                int index = activeDeviceFilter.IndexOf("(\"");
                if (index != -1)
                {
                    return activeDeviceFilter.Substring(0, index - 1);
                }
            }
            return activeDeviceFilter;
        }

        internal static BooleanOption GetNegatedBooleanOption(BooleanOption oldValue)
        {
            switch (oldValue)
            {
                case -1:
                    return 1;

                case 0:
                    return 1;

                case 1:
                    return 0;
            }
            return -1;
        }

        internal static IComponent GetRootComponent(IComponent component)
        {
            ISite site = component.Site;
            if (site != null)
            {
                IDesignerHost service = (IDesignerHost) site.GetService(typeof(IDesignerHost));
                if (service != null)
                {
                    return service.RootComponent;
                }
            }
            return null;
        }

        private static bool HtmlRequiresUpdateOnRefreshPageView(Control control)
        {
            if (control is StyleSheet)
            {
                return false;
            }
            return true;
        }

        internal static void InitializeDefaultProperties(object obj, Hashtable defaultValues)
        {
            InitializeDefaultProperties(string.Empty, obj, defaultValues);
        }

        internal static void InitializeDefaultProperties(string prefix, object obj, Hashtable defaultValues)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                if ((descriptor.IsBrowsable && (!descriptor.IsReadOnly || (descriptor.Converter is ExpandableObjectConverter))) && (!descriptor.SerializationVisibility.Equals(DesignerSerializationVisibility.Hidden) && (descriptor.Name != "ID")))
                {
                    if (descriptor.Converter is ExpandableObjectConverter)
                    {
                        object obj2 = descriptor.GetValue(obj);
                        InitializeDefaultProperties(prefix + descriptor.Name + "-", obj2, defaultValues);
                    }
                    else
                    {
                        object obj3 = descriptor.GetValue(obj);
                        defaultValues[prefix + descriptor.Name] = obj3;
                    }
                }
            }
        }

        internal static void InitializeOverridenProperties(MobileControl mobileControl, Hashtable overridenValues)
        {
            overridenValues.Clear();
            if (mobileControl.get_DeviceSpecific() != null)
            {
                foreach (DeviceSpecificChoice choice in mobileControl.get_DeviceSpecific().get_Choices())
                {
                    Hashtable hashtable = new Hashtable();
                    overridenValues[CreateUniqueChoiceName(choice)] = hashtable;
                    IDictionaryEnumerator enumerator = choice.get_Contents().GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        int num;
                        object component = mobileControl;
                        string key = (string) enumerator.Key;
                        while ((num = key.IndexOf("-")) != -1)
                        {
                            string name = key.Substring(0, num);
                            component = TypeDescriptor.GetProperties(component).Find(name, true).GetValue(component);
                            key = key.Substring(num + 1);
                        }
                        PropertyDescriptor descriptor = TypeDescriptor.GetProperties(component).Find(key, true);
                        try
                        {
                            hashtable[enumerator.Key] = descriptor.Converter.ConvertFromInvariantString((string) enumerator.Value);
                            continue;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }
        }

        internal static bool InMobilePage(Control control)
        {
            if (control == null)
            {
                return false;
            }
            if (control.Page != null)
            {
                return (control.Page is MobilePage);
            }
            return true;
        }

        internal static bool InMobileUserControl(IComponent component)
        {
            return (GetRootComponent(component) is MobileUserControl);
        }

        internal static bool InTemplateFrame(Control control)
        {
            if (control.Parent == null)
            {
                return false;
            }
            return ((GetControlDesigner(control.Parent) is TemplatedControlDesigner) || InTemplateFrame(control.Parent));
        }

        internal static bool InUserControl(IComponent component)
        {
            return (GetRootComponent(component) is UserControl);
        }

        internal static void InvalidateDisplayFieldIndices(ObjectList objectList)
        {
            typeof(ObjectList).InvokeMember("InvalidateDisplayFieldIndices", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, objectList, null);
        }

        internal static void InvokeActivateTrident(object mshtmlhost)
        {
            mshtmlhost.GetType().InvokeMember("ActivateTrident", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, mshtmlhost, null);
        }

        internal static bool InvokeCreateTrident(object mshtmlhost)
        {
            return (bool) mshtmlhost.GetType().InvokeMember("CreateTrident", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, mshtmlhost, null);
        }

        internal static object InvokeGetDocument(object mshtmlhost)
        {
            return mshtmlhost.GetType().InvokeMember("GetDocument", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, mshtmlhost, null);
        }

        internal static string InvokePersistControl(Control control)
        {
            IDesignerHost host = (control.Site != null) ? ((IDesignerHost) control.Site.GetService(typeof(IDesignerHost))) : null;
            return (string) Type.GetType("System.Web.UI.Design.MobileControls.MobileControlPersister," + Constants.MobileAssemblyFullName).InvokeMember("PersistControl", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, null, new object[] { control, host });
        }

        internal static void InvokePersistControl(StringWriter sw, Control control)
        {
            sw.Write(InvokePersistControl(control));
        }

        internal static string InvokePersistInnerProperties(Control control)
        {
            IDesignerHost host = (control.Site != null) ? ((IDesignerHost) control.Site.GetService(typeof(IDesignerHost))) : null;
            return (string) Type.GetType("System.Web.UI.Design.MobileControls.MobileControlPersister," + Constants.MobileAssemblyFullName).InvokeMember("PersistInnerProperties", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, null, new object[] { control, host });
        }

        internal static void InvokePersistInnerProperties(StringWriter sw, Control control)
        {
            sw.Write(InvokePersistInnerProperties(control));
        }

        internal static void InvokeSetControl(Style style, MobileControl control)
        {
            Type.GetType("System.Web.UI.MobileControls.Style," + Constants.MobileAssemblyFullName).InvokeMember("SetControl", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, style, new object[] { control });
        }

        internal static bool IsChoiceEmpty(DeviceSpecificChoice choice)
        {
            if ((choice != null) && (choice.get_HasTemplates() || ((choice.get_Contents() != null) && (choice.get_Contents().Count != 0))))
            {
                return false;
            }
            return true;
        }

        internal static bool IsCollection(PropertyDescriptor property)
        {
            return (property.PropertyType.GetInterface("ICollection") != null);
        }

        internal static bool IsOverridableProperty(PropertyDescriptor property)
        {
            if (property.Name == "ID")
            {
                return false;
            }
            return (((property.IsBrowsable && (!property.IsReadOnly || (property.Converter is ExpandableObjectConverter))) && !IsCollection(property)) && !property.SerializationVisibility.Equals(DesignerSerializationVisibility.Hidden));
        }

        internal static Style MapToWebControlStyle(MobileControl control)
        {
            object target = typeof(MobileControl).InvokeMember("Style", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, control, null);
            Style style = new Style();
            typeof(Style).InvokeMember("ApplyTo", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, target, new object[] { style });
            return style;
        }

        internal static void RefreshMobileControlStyle(MobileControl control)
        {
            typeof(MobileControl).InvokeMember("RefreshStyle", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, control, null);
        }

        internal static void RefreshPageView(MobilePage page)
        {
            if (page != null)
            {
                UpdateRenderingRecursive(page);
            }
        }

        internal static void RefreshStyle(Style style)
        {
            typeof(Style).InvokeMember("Refresh", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, style, null);
        }

        internal static void SetDeviceSpecificChoice(MobileControl mobileControl, DeviceSpecificChoice choice)
        {
            typeof(DeviceSpecific).InvokeMember("SetDesignerChoice", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, mobileControl.get_DeviceSpecific(), new object[] { choice });
        }

        internal static void SetDeviceSpecificChoice(Style style, DeviceSpecificChoice choice)
        {
            typeof(DeviceSpecific).InvokeMember("SetDesignerChoice", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, style.get_DeviceSpecific(), new object[] { choice });
        }

        internal static void SetDeviceSpecificOwner(DeviceSpecific deviceSpecific, MobileControl control)
        {
            typeof(DeviceSpecific).InvokeMember("SetOwner", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, deviceSpecific, new object[] { control });
        }

        internal static void SetStandardStyleAttributes(IHtmlControlDesignerBehavior behavior, ContainmentStatus containmentStatus)
        {
            bool flag = containmentStatus == ContainmentStatus.AtTopLevel;
            Color window = SystemColors.Window;
            Color windowText = SystemColors.WindowText;
            Color c = Color.FromArgb((short) ((windowText.R * 0.1) + (window.R * 0.9)), (short) ((windowText.G * 0.1) + (window.G * 0.9)), (short) ((windowText.B * 0.1) + (window.B * 0.9)));
            behavior.SetStyleAttribute("borderColor", true, ColorTranslator.ToHtml(c), true);
            behavior.SetStyleAttribute("borderStyle", true, "solid", true);
            behavior.SetStyleAttribute("borderWidth", true, "1px", true);
            behavior.SetStyleAttribute("marginLeft", true, "5px", true);
            behavior.SetStyleAttribute("marginRight", true, flag ? "30%" : "5px", true);
            behavior.SetStyleAttribute("marginTop", true, flag ? "5px" : "2px", true);
            behavior.SetStyleAttribute("marginBottom", true, flag ? "5px" : "2px", true);
        }

        internal static void UpdateRenderingRecursive(Control rootControl)
        {
            foreach (Control control in rootControl.Controls)
            {
                IMobileDesigner controlDesigner = null;
                try
                {
                    controlDesigner = GetControlDesigner(control) as IMobileDesigner;
                }
                catch
                {
                }
                if ((controlDesigner != null) && HtmlRequiresUpdateOnRefreshPageView(control))
                {
                    try
                    {
                        controlDesigner.UpdateRendering();
                        continue;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        internal class ChangedSubProperty
        {
            internal object parentObject;
            internal PropertyDescriptor propertyDescriptor;
            internal string propertyName;
            internal object propertyValue;

            internal ChangedSubProperty(object parent, PropertyDescriptor property, string name, object value)
            {
                this.parentObject = parent;
                this.propertyDescriptor = property;
                this.propertyName = name;
                this.propertyValue = value;
            }
        }
    }
}

