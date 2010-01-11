namespace Microsoft.Matrix.Packages.Web.Html.Css
{
    using Microsoft.Matrix.Packages.Web;
    using System;

    internal sealed class InlineStyle : IStyle
    {
        private Interop.IHTMLStyle _style;

        public InlineStyle(Interop.IHTMLStyle style)
        {
            this._style = style;
        }

        string IStyle.GetAttribute(string name)
        {
            string str = null;
            try
            {
                object attribute = this._style.GetAttribute(name, 1);
                if (attribute != null)
                {
                    str = attribute.ToString();
                }
            }
            catch (Exception)
            {
            }
            if (str == null)
            {
                str = string.Empty;
            }
            return str;
        }

        void IStyle.ResetAttribute(string name)
        {
            try
            {
                this._style.RemoveAttribute(name, 1);
            }
            catch (Exception)
            {
            }
        }

        void IStyle.SetAttribute(string name, string value)
        {
            try
            {
                if (value.Length == 0)
                {
                    ((IStyle) this).ResetAttribute(name);
                }
                else
                {
                    this._style.SetAttribute(name, value, 1);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}

