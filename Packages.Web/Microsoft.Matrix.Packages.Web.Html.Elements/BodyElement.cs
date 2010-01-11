namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Web.UI.WebControls;

    public sealed class BodyElement : StyledElement
    {
        internal BodyElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [DefaultValue(typeof(Color), ""), TypeConverter(typeof(WebColorConverter)), Category("Appearance"), Description("The color of active links on this page")]
        public Color aLink
        {
            get
            {
                return base.GetColorAttribute("aLink");
            }
            set
            {
                base.SetColorAttribute("aLink", value);
            }
        }

        [DefaultValue(""), Description("The URL of background image used on this page"), Category("Appearance")]
        public string background
        {
            get
            {
                return base.GetStringAttribute("background");
            }
            set
            {
                base.SetStringAttribute("background", value);
            }
        }

        [Description("The color of the page background"), TypeConverter(typeof(WebColorConverter)), DefaultValue(typeof(Color), ""), Category("Appearance")]
        public Color bgColor
        {
            get
            {
                return base.GetColorAttribute("bgColor");
            }
            set
            {
                base.SetColorAttribute("bgColor", value);
            }
        }

        [Category("Appearance"), DefaultValue(0), Description("Specifies whether the background image is fixed or should scroll")]
        public BackgroundPropertyType bgProperties
        {
            get
            {
                return (BackgroundPropertyType) base.GetEnumAttribute("bgProperties", BackgroundPropertyType.Scroll);
            }
            set
            {
                base.SetEnumAttribute("bgProperties", value, BackgroundPropertyType.Scroll);
            }
        }

        [Description("The size in pixels of the bottom margin"), Category("Layout"), DefaultValue(15)]
        public int bottomMargin
        {
            get
            {
                return base.GetIntegerAttribute("bottomMargin", 15);
            }
            set
            {
                base.SetIntegerAttribute("bottomMargin", value, 15);
            }
        }

        [Description("The size in pixels of the left margin"), DefaultValue(10), Category("Layout")]
        public int leftMargin
        {
            get
            {
                return base.GetIntegerAttribute("leftmargin", 10);
            }
            set
            {
                base.SetIntegerAttribute("leftMargin", value, 10);
            }
        }

        [Category("Appearance"), TypeConverter(typeof(WebColorConverter)), Description("The color of links on the page"), DefaultValue(typeof(Color), "")]
        public Color link
        {
            get
            {
                return base.GetColorAttribute("link");
            }
            set
            {
                base.SetColorAttribute("link", value);
            }
        }

        [Description("Specifies whether to keep text on one line instead of wrapping"), DefaultValue(false), Category("Behavior")]
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

        [Category("Layout"), Description("The size in pixels of the right margin"), DefaultValue(10)]
        public int rightMargin
        {
            get
            {
                return base.GetIntegerAttribute("rightMargin", 10);
            }
            set
            {
                base.SetIntegerAttribute("rightMargin", value, 10);
            }
        }

        [Description("Indicates whether to display scroll bars"), Category("Behavior"), DefaultValue(2)]
        public ScrollType scroll
        {
            get
            {
                return (ScrollType) base.GetEnumAttribute("scroll", ScrollType.Auto);
            }
            set
            {
                base.SetEnumAttribute("scroll", value, ScrollType.Auto);
            }
        }

        [Description("The foreground color of text on this page"), TypeConverter(typeof(WebColorConverter)), Category("Appearance"), DefaultValue(typeof(Color), "")]
        public Color text
        {
            get
            {
                return base.GetColorAttribute("text");
            }
            set
            {
                base.SetColorAttribute("text", value);
            }
        }

        [DefaultValue(15), Category("Layout"), Description("The size in pixels of this page's bottom margin")]
        public int topMargin
        {
            get
            {
                return base.GetIntegerAttribute("topMargin", 15);
            }
            set
            {
                base.SetIntegerAttribute("topMargin", value, 15);
            }
        }

        [DefaultValue(typeof(Color), ""), Description("The color of visited links on this page"), TypeConverter(typeof(WebColorConverter)), Category("Appearance")]
        public Color vLink
        {
            get
            {
                return base.GetColorAttribute("vLink");
            }
            set
            {
                base.SetColorAttribute("vLink", value);
            }
        }
    }
}

