namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Html;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Web.UI.WebControls;

    [DesignOnly(true)]
    public class Element
    {
        private HtmlControl _owner;
        private Interop.IHTMLElement _peer;

        internal Element(Interop.IHTMLElement peer)
        {
            this._peer = peer;
        }

        public object GetAttribute(string attribute)
        {
            try
            {
                object[] pvars = new object[1];
                this._peer.GetAttribute(attribute, 0, pvars);
                object obj2 = pvars[0];
                if (obj2 is DBNull)
                {
                    obj2 = null;
                }
                return obj2;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected internal bool GetBooleanAttribute(string attribute)
        {
            object obj2 = this.GetAttribute(attribute);
            if (obj2 == null)
            {
                return false;
            }
            return ((obj2 is bool) && ((bool) obj2));
        }

        public Element GetChild(int index)
        {
            Interop.IHTMLElementCollection children = (Interop.IHTMLElementCollection) this._peer.GetChildren();
            Interop.IHTMLElement element = (Interop.IHTMLElement) children.Item(null, index);
            return ElementWrapperTable.GetWrapper(element, this._owner);
        }

        public Element GetChild(string name)
        {
            Interop.IHTMLElementCollection children = (Interop.IHTMLElementCollection) this._peer.GetChildren();
            Interop.IHTMLElement element = (Interop.IHTMLElement) children.Item(name, null);
            return ElementWrapperTable.GetWrapper(element, this._owner);
        }

        protected internal Color GetColorAttribute(string attribute)
        {
            string stringAttribute = this.GetStringAttribute(attribute);
            if (stringAttribute.Length == 0)
            {
                return Color.Empty;
            }
            return ColorTranslator.FromHtml(stringAttribute);
        }

        protected internal Enum GetEnumAttribute(string attribute, Enum defaultValue)
        {
            Type enumType = defaultValue.GetType();
            object obj2 = this.GetAttribute(attribute);
            if (obj2 == null)
            {
                return defaultValue;
            }
            string str = obj2 as string;
            if ((str == null) || (str.Length == 0))
            {
                return defaultValue;
            }
            try
            {
                return (Enum) Enum.Parse(enumType, str, true);
            }
            catch
            {
                return defaultValue;
            }
        }

        protected internal int GetIntegerAttribute(string attribute, int defaultValue)
        {
            object obj2 = this.GetAttribute(attribute);
            if (obj2 != null)
            {
                if (obj2 is int)
                {
                    return (int) obj2;
                }
                if (obj2 is short)
                {
                    return (short) obj2;
                }
                if (!(obj2 is string))
                {
                    return defaultValue;
                }
                string str = (string) obj2;
                if ((str.Length == 0) || !char.IsDigit(str[0]))
                {
                    return defaultValue;
                }
                try
                {
                    return int.Parse((string) obj2);
                }
                catch
                {
                }
            }
            return defaultValue;
        }

        public Element GetParent()
        {
            return ElementWrapperTable.GetWrapper(this._peer.GetParentElement(), this._owner);
        }

        protected string GetRelativeUrl(string absoluteUrl)
        {
            if ((absoluteUrl == null) || (absoluteUrl.Length == 0))
            {
                return string.Empty;
            }
            string uriString = absoluteUrl;
            if (this._owner != null)
            {
                string url = this._owner.Url;
                if (url.Length == 0)
                {
                    return uriString;
                }
                try
                {
                    Uri uri = new Uri(url);
                    Uri toUri = new Uri(uriString);
                    uriString = uri.MakeRelative(toUri);
                }
                catch
                {
                }
            }
            return uriString;
        }

        protected internal string GetStringAttribute(string attribute)
        {
            return this.GetStringAttribute(attribute, string.Empty);
        }

        protected internal string GetStringAttribute(string attribute, string defaultValue)
        {
            object obj2 = this.GetAttribute(attribute);
            if ((obj2 != null) && (obj2 is string))
            {
                return (string) obj2;
            }
            return defaultValue;
        }

        protected internal Unit GetUnitAttribute(string attribute)
        {
            object obj2 = this.GetAttribute(attribute);
            if (obj2 == null)
            {
                return Unit.Empty;
            }
            if (obj2 is int)
            {
                return new Unit((int) obj2);
            }
            try
            {
                return new Unit((string) obj2, CultureInfo.InvariantCulture);
            }
            catch
            {
                return Unit.Empty;
            }
        }

        public void RemoveAttribute(string attribute)
        {
            try
            {
                this._peer.RemoveAttribute(attribute, 0);
            }
            catch (Exception)
            {
            }
        }

        public void SetAttribute(string attribute, object value)
        {
            try
            {
                this._peer.SetAttribute(attribute, value, 0);
            }
            catch (Exception)
            {
            }
        }

        protected internal void SetBooleanAttribute(string attribute, bool value)
        {
            if (value)
            {
                this.SetAttribute(attribute, true);
            }
            else
            {
                this.RemoveAttribute(attribute);
            }
        }

        protected internal void SetColorAttribute(string attribute, Color value)
        {
            if (value.IsEmpty)
            {
                this.RemoveAttribute(attribute);
            }
            else
            {
                this.SetAttribute(attribute, ColorTranslator.ToHtml(value));
            }
        }

        protected internal void SetEnumAttribute(string attribute, Enum value, Enum defaultValue)
        {
            if (value.Equals(defaultValue))
            {
                this.RemoveAttribute(attribute);
            }
            else
            {
                this.SetAttribute(attribute, value.ToString(CultureInfo.InvariantCulture));
            }
        }

        protected internal void SetIntegerAttribute(string attribute, int value, int defaultValue)
        {
            if (value == defaultValue)
            {
                this.RemoveAttribute(attribute);
            }
            else
            {
                this.SetAttribute(attribute, value);
            }
        }

        protected internal void SetStringAttribute(string attribute, string value)
        {
            this.SetStringAttribute(attribute, value, string.Empty);
        }

        protected internal void SetStringAttribute(string attribute, string value, string defaultValue)
        {
            if ((value == null) || value.Equals(defaultValue))
            {
                this.RemoveAttribute(attribute);
            }
            else
            {
                this.SetAttribute(attribute, value);
            }
        }

        protected internal void SetUnitAttribute(string attribute, Unit value)
        {
            if (value.IsEmpty)
            {
                this.RemoveAttribute(attribute);
            }
            else
            {
                UnitType type = value.Type;
                if ((type != UnitType.Pixel) && (type != UnitType.Percentage))
                {
                    throw new ArgumentException("Only pixel and percent values are allowed here.");
                }
                this.SetAttribute(attribute, value.ToString(CultureInfo.InvariantCulture));
            }
        }

        public override string ToString()
        {
            if (this._peer != null)
            {
                try
                {
                    return ("<" + this._peer.GetTagName() + ">");
                }
                catch
                {
                }
            }
            return string.Empty;
        }

        [Browsable(false)]
        public string InnerHtml
        {
            get
            {
                try
                {
                    return this._peer.GetInnerHTML();
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    this._peer.SetInnerHTML(value);
                }
                catch (Exception)
                {
                }
            }
        }

        [Browsable(false)]
        public string OuterHtml
        {
            get
            {
                try
                {
                    return this._peer.GetOuterHTML();
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    this._peer.SetOuterHTML(value);
                }
                catch (Exception)
                {
                }
            }
        }

        internal HtmlControl Owner
        {
            get
            {
                return this._owner;
            }
            set
            {
                this._owner = value;
            }
        }

        internal Interop.IHTMLElement Peer
        {
            get
            {
                return this._peer;
            }
        }

        [Browsable(false)]
        public string TagName
        {
            get
            {
                try
                {
                    return this._peer.GetTagName();
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }
    }
}

