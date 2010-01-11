namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    public sealed class PreformattedElement : StyledElement
    {
        internal PreformattedElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Category("Behavior"), DefaultValue(false), Description("Specifies whether long lines of text should be wrapped.")]
        public bool noWrap
        {
            get
            {
                return base.GetBooleanAttribute("noWrap");
            }
            set
            {
                base.SetBooleanAttribute("noWrap", value);
            }
        }

        [DefaultValue(false), Category("Default"), Description("True if this is a server-side control")]
        public bool runAtServer
        {
            get
            {
                string attribute = base.GetAttribute("runAt") as string;
                return ((attribute != null) && attribute.Equals("server"));
            }
            set
            {
                if (value)
                {
                    base.SetAttribute("runAt", "server");
                }
                else
                {
                    base.RemoveAttribute("runAt");
                }
            }
        }
    }
}

