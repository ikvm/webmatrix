namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    [ToolboxHtml("<div>Div</div>", "Div")]
    public class DivElement : StyledElement
    {
        internal DivElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Category("Appearance"), Description("The alignment of the content of the div"), DefaultValue(0)]
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

        [Category("Behavior"), Description("True if this object is disabled"), DefaultValue(false)]
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

        [Description("Specifies whether to keep text on one line instead of word-wrapping"), Category("Behavior"), DefaultValue(false)]
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

        [Category("Events"), Description("Event raised when the user changes the scrolling position of this object"), DefaultValue("")]
        public string onScroll
        {
            get
            {
                return base.GetStringAttribute("onScroll");
            }
            set
            {
                base.SetStringAttribute("onScroll", value);
            }
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
    }
}

