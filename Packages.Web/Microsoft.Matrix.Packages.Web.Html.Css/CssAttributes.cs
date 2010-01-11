namespace Microsoft.Matrix.Packages.Web.Html.Css
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    public abstract class CssAttributes : ICustomTypeDescriptor
    {
        private IList _attributes;
        private PropertyDescriptorCollection _props;
        protected static readonly Attribute[] ReadOnlyModifier = new Attribute[] { ReadOnlyAttribute.Yes };

        protected CssAttributes()
        {
        }

        protected abstract PropertyDescriptorCollection CreateProperties(PropertyDescriptorCollection reflectedProperties);
        private void EnsureProperties()
        {
            if (this._props == null)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this, true);
                this._props = this.CreateProperties(properties);
            }
        }

        protected abstract void FillAttributes(IList attributes);
        protected void ResetProperties()
        {
            this._props = null;
            TypeDescriptor.Refresh(this);
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return new AttributeCollection(null);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return "Style";
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return "Style";
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return null;
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return null;
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            this.EnsureProperties();
            return this._props[0];
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return null;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return new EventDescriptorCollection(null);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return new EventDescriptorCollection(null);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            this.EnsureProperties();
            return this._props;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            this.EnsureProperties();
            return this._props;
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public ICollection Attributes
        {
            get
            {
                if (this._attributes == null)
                {
                    this._attributes = new ArrayList();
                    this.FillAttributes(this._attributes);
                }
                return this._attributes;
            }
        }

        protected sealed class StylePropertyDescriptor : PropertyDescriptor
        {
            private string _displayName;
            private PropertyDescriptor _propDesc;
            private bool _readOnly;

            public StylePropertyDescriptor(PropertyDescriptor pd, string name) : this(pd, name, false, null)
            {
            }

            public StylePropertyDescriptor(PropertyDescriptor pd, string name, bool readOnly) : this(pd, name, readOnly, null)
            {
            }

            public StylePropertyDescriptor(PropertyDescriptor pd, string name, bool readOnly, Attribute[] attrs) : base(pd, attrs)
            {
                this._propDesc = pd;
                this._displayName = name;
                this._readOnly = readOnly;
            }

            public override void AddValueChanged(object instance, EventHandler handler)
            {
                this._propDesc.AddValueChanged(instance, handler);
            }

            public override bool CanResetValue(object instance)
            {
                return false;
            }

            public override object GetValue(object instance)
            {
                return this._propDesc.GetValue(instance);
            }

            public override void RemoveValueChanged(object instance, EventHandler handler)
            {
                this._propDesc.RemoveValueChanged(instance, handler);
            }

            public override void ResetValue(object instance)
            {
            }

            public override void SetValue(object instance, object value)
            {
                this._propDesc.SetValue(instance, value);
            }

            public override bool ShouldSerializeValue(object instance)
            {
                return false;
            }

            public override Type ComponentType
            {
                get
                {
                    return this._propDesc.ComponentType;
                }
            }

            public override string DisplayName
            {
                get
                {
                    return this._displayName;
                }
            }

            public override bool IsReadOnly
            {
                get
                {
                    if (this._readOnly)
                    {
                        return this._readOnly;
                    }
                    ReadOnlyAttribute attribute = (ReadOnlyAttribute) this.Attributes[typeof(ReadOnlyAttribute)];
                    if (attribute != null)
                    {
                        return attribute.IsReadOnly;
                    }
                    return this._propDesc.IsReadOnly;
                }
            }

            public override string Name
            {
                get
                {
                    return this._propDesc.Name;
                }
            }

            public override Type PropertyType
            {
                get
                {
                    return this._propDesc.PropertyType;
                }
            }
        }
    }
}

