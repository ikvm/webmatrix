namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ToolboxHtmlAttribute : Attribute
    {
        private string _html;
        private string _name;
        public static readonly ToolboxHtmlAttribute Default = new ToolboxHtmlAttribute(string.Empty, string.Empty);

        public ToolboxHtmlAttribute(string html, string name)
        {
            this._html = html;
            this._name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            ToolboxHtmlAttribute attribute = obj as ToolboxHtmlAttribute;
            if (attribute == null)
            {
                return false;
            }
            return (attribute.Html.Equals(this._html) && attribute.Name.Equals(this._name));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool IsDefaultAttribute()
        {
            return this.Equals(Default);
        }

        public string Html
        {
            get
            {
                if (this._html == null)
                {
                    return string.Empty;
                }
                return this._html;
            }
        }

        public string Name
        {
            get
            {
                if (this._name == null)
                {
                    return string.Empty;
                }
                return this._name;
            }
        }
    }
}

