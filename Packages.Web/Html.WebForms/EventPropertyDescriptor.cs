namespace Microsoft.Matrix.Packages.Web.Html.WebForms
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.Design;

    internal sealed class EventPropertyDescriptor : PropertyDescriptor
    {
        private EventConverter _converter;
        private System.ComponentModel.EventDescriptor _descriptor;
        private bool _isSetForPage;

        public EventPropertyDescriptor(System.ComponentModel.EventDescriptor descriptor) : base(descriptor.Name, null)
        {
            this._descriptor = descriptor;
        }

        public override bool CanResetValue(object component)
        {
            return (this.GetValue(component) != null);
        }

        private Interop.IHTMLElement GetElement(IComponent comp)
        {
            IDesignerHost service = comp.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
            ControlDesigner designer = service.GetDesigner(comp) as ControlDesigner;
            if (designer != null)
            {
                Behavior behavior = designer.Behavior as Behavior;
                if (behavior != null)
                {
                    return behavior.Element;
                }
            }
            return null;
        }

        public override object GetValue(object component)
        {
            if (component is IComponent)
            {
                Interop.IHTMLElement element = this.GetElement((IComponent) component);
                if (element != null)
                {
                    object[] pvars = new object[1];
                    element.GetAttribute("On" + this.Name, 0, pvars);
                    if ((pvars[0] != null) && (pvars[0] != DBNull.Value))
                    {
                        return pvars[0].ToString();
                    }
                    return null;
                }
                if (component is Page)
                {
                    if (this._isSetForPage)
                    {
                        return ("Page_" + this.Name);
                    }
                    return null;
                }
            }
            return null;
        }

        public override void ResetValue(object component)
        {
            this.SetValue(component, null);
        }

        public override void SetValue(object component, object val)
        {
            if (component is IComponent)
            {
                Interop.IHTMLElement element = this.GetElement((IComponent) component);
                if (element != null)
                {
                    if (val == null)
                    {
                        element.RemoveAttribute("On" + this.Name, 0);
                    }
                    else
                    {
                        element.SetAttribute("On" + this.Name, val, 0);
                    }
                }
                else if (component is Page)
                {
                    string str = ("Page_" + this.Name).ToLower();
                    string str2 = (val == null) ? string.Empty : val.ToString();
                    if ((str2.Length > 0) && (str2.ToLower() != str))
                    {
                        throw new Exception("This event must be named Page_" + this.Name + ".");
                    }
                    if (str2.Length == 0)
                    {
                        if (this._isSetForPage)
                        {
                            this._isSetForPage = false;
                        }
                    }
                    else if (!this._isSetForPage)
                    {
                        this._isSetForPage = true;
                    }
                }
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return this.CanResetValue(component);
        }

        public override Type ComponentType
        {
            get
            {
                return this._descriptor.ComponentType;
            }
        }

        public override TypeConverter Converter
        {
            get
            {
                if (this._converter == null)
                {
                    this._converter = new EventConverter(this._descriptor);
                }
                return this._converter;
            }
        }

        public System.ComponentModel.EventDescriptor EventDescriptor
        {
            get
            {
                return this._descriptor;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this._descriptor.EventType;
            }
        }

        private class EventConverter : TypeConverter
        {
            private EventDescriptor _descriptor;

            public EventConverter(EventDescriptor evt)
            {
                this._descriptor = evt;
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return ((destinationType == typeof(string)) || base.CanConvertTo(context, destinationType));
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value != null)
                {
                    if (!(value is string))
                    {
                        return base.ConvertFrom(context, culture, value);
                    }
                    if (((string) value).Length == 0)
                    {
                        return null;
                    }
                }
                return value;
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    if ((value is DBNull) || (value == null))
                    {
                        return string.Empty;
                    }
                    if (value is string)
                    {
                        return value;
                    }
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                string[] values = null;
                if (context != null)
                {
                    IEventBindingService service = (IEventBindingService) context.GetService(typeof(IEventBindingService));
                    if (service != null)
                    {
                        ICollection compatibleMethods = service.GetCompatibleMethods(this._descriptor);
                        values = new string[compatibleMethods.Count];
                        int num = 0;
                        foreach (string str in compatibleMethods)
                        {
                            values[num++] = str;
                        }
                    }
                }
                return new TypeConverter.StandardValuesCollection(values);
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
        }
    }
}

