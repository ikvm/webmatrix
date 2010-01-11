namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    public sealed class UnorderedListElement : StyledElement
    {
        internal UnorderedListElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Category("Default"), Description("True if this is a server-side control"), DefaultValue(false)]
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

        [Description("The type of bullets to use in an unordered list or numbering scheme in an ordered list"), DefaultValue(0), Category("Appearance")]
        public UnorderedListType type
        {
            get
            {
                return (UnorderedListType) base.GetEnumAttribute("type", UnorderedListType.Default);
            }
            set
            {
                base.SetEnumAttribute("type", value, UnorderedListType.Default);
            }
        }
    }
}

