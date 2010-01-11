namespace Microsoft.Matrix.Packages.Web.Html.Css
{
    using System;
    using System.Globalization;

    internal sealed class CssAttribute
    {
        private string _attributeName;
        private string _attributeValue;
        private bool _caseSignificant;
        private bool _dirty;
        private string _omName;

        public CssAttribute(string attributeName, string omName) : this(attributeName, omName, false)
        {
        }

        public CssAttribute(string attributeName, string omName, bool caseSignificant)
        {
            this._attributeName = attributeName;
            this._omName = omName;
            this._caseSignificant = caseSignificant;
        }

        public void Load(IStyle[] styles)
        {
            this._attributeValue = null;
            this._dirty = false;
            string attribute = styles[0].GetAttribute(this._omName);
            if (attribute != null)
            {
                attribute = attribute.Trim();
                for (int i = 1; i < styles.Length; i++)
                {
                    string str2 = styles[i].GetAttribute(this._omName);
                    if (str2 == null)
                    {
                        str2 = null;
                        break;
                    }
                    str2 = str2.Trim();
                    if ((this._caseSignificant && !attribute.Equals(str2)) || (string.Compare(attribute, str2, true, CultureInfo.InvariantCulture) != 0))
                    {
                        attribute = null;
                        break;
                    }
                }
                if (attribute != null)
                {
                    this._attributeValue = this._caseSignificant ? attribute : attribute.ToLower(CultureInfo.InvariantCulture);
                }
            }
        }

        public void Save(IStyle[] styles)
        {
            if ((this._attributeValue == null) || (this._attributeValue.Length == 0))
            {
                for (int i = 0; i < styles.Length; i++)
                {
                    styles[i].ResetAttribute(this._omName);
                }
                this.Load(styles);
            }
            else
            {
                for (int j = 0; j < styles.Length; j++)
                {
                    styles[j].SetAttribute(this._omName, this._attributeValue);
                }
                this._dirty = false;
            }
        }

        public bool IsDirty
        {
            get
            {
                return this._dirty;
            }
        }

        public string Name
        {
            get
            {
                return this._attributeName;
            }
        }

        public string Value
        {
            get
            {
                return this._attributeValue;
            }
            set
            {
                this._attributeValue = value;
                this._dirty = true;
            }
        }
    }
}

