namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public sealed class ParagraphElement : StyledElement
    {
        internal ParagraphElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Category("Appearance"), Description("The alignment of the content of the paragraph"), DefaultValue(0)]
        public HorizontalAlign align
        {
            get
            {
                return (HorizontalAlign) base.GetEnumAttribute("align", HorizontalAlign.NotSet);
            }
            set
            {
                base.SetEnumAttribute("align", value, HorizontalAlign.NotSet);
            }
        }

        [DefaultValue(false), Category("Behavior"), Description("True if this object is disabled")]
        public bool disabled
        {
            get
            {
                return base.GetBooleanAttribute("disabled");
            }
            set
            {
                base.SetBooleanAttribute("disabled", value);
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

        [Category("Behavior"), DefaultValue((short) 0), Description("The position of this object in the page's tab ordering")]
        public short tabIndex
        {
            get
            {
                return (short) base.GetIntegerAttribute("tabIndex", 0);
            }
            set
            {
                if (value < -1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                base.SetIntegerAttribute("tabIndex", value, 0);
            }
        }
    }
}

