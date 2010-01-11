namespace Microsoft.Matrix.Packages.ClassView.Documents
{
    using System;
    using System.Reflection;
    using System.Text;

    public sealed class PropertyItem : TypeDocumentItem
    {
        private string _text;

        public PropertyItem(TypeDocument document, PropertyInfo pi, MethodInfo underlyingMethod) : base(document, pi, underlyingMethod)
        {
        }

        private string GeneratePropertyCaption(PropertyInfo pi)
        {
            ParameterInfo[] indexParameters = pi.GetIndexParameters();
            if ((indexParameters == null) || (indexParameters.Length == 0))
            {
                return pi.Name;
            }
            StringBuilder builder = new StringBuilder(0x100);
            builder.Append(pi.Name);
            builder.Append('[');
            for (int i = 0; i < indexParameters.Length; i++)
            {
                if (i != 0)
                {
                    builder.Append(", ");
                }
                builder.Append(indexParameters[i].ParameterType.Name);
            }
            builder.Append(']');
            return builder.ToString();
        }

        protected override TypeDocumentFilter GetVisibilityFilter()
        {
            return base.GetVisibilityFilter(base.MemberMethod);
        }

        public override string Text
        {
            get
            {
                if (this._text == null)
                {
                    this._text = this.GeneratePropertyCaption((PropertyInfo) base.Member);
                }
                return this._text;
            }
        }

        public MethodInfo UnderlyingMethod
        {
            get
            {
                return base.MemberMethod;
            }
        }
    }
}

