namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    [ToolboxHtml("<iframe></iframe>", "IFrame")]
    public sealed class IFrameElement : SelectableElement
    {
        internal IFrameElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Category("Appearance"), DefaultValue(0), Description("The alignment of the frame in the document")]
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

        [Category("Appearance"), Description("The space between frames"), DefaultValue(1)]
        public int border
        {
            get
            {
                return base.GetIntegerAttribute("border", 1);
            }
            set
            {
                base.SetIntegerAttribute("border", value, 1);
            }
        }

        [Description("Specifies whether to draw a border around the frame"), Category("Appearance"), DefaultValue(true)]
        public bool frameBorder
        {
            get
            {
                return (base.GetIntegerAttribute("frameBorder", 1) == 1);
            }
            set
            {
                base.SetAttribute("frameBorder", value ? 1 : 0);
            }
        }

        [Category("Layout"), Description("The height of the frame"), DefaultValue(typeof(Unit), "")]
        public Unit height
        {
            get
            {
                return base.GetUnitAttribute("height");
            }
            set
            {
                base.SetUnitAttribute("height", value);
            }
        }

        [Description("The amount of padding to the left and right of the frame"), Category("Appearance"), DefaultValue(0)]
        public int hSpace
        {
            get
            {
                return base.GetIntegerAttribute("hSpace", 0);
            }
            set
            {
                base.SetIntegerAttribute("hSpace", value, 0);
            }
        }

        [Category("Appearance"), Description("The size of the top and bottom margins"), DefaultValue(-1)]
        public int marginHeight
        {
            get
            {
                return base.GetIntegerAttribute("marginHeight", -1);
            }
            set
            {
                base.SetIntegerAttribute("marginHeight", value, -1);
            }
        }

        [Description("The size of the left and right margins"), Category("Appearance"), DefaultValue(-1)]
        public int marginWidth
        {
            get
            {
                return base.GetIntegerAttribute("marginWidth", -1);
            }
            set
            {
                base.SetIntegerAttribute("marginWidth", value, -1);
            }
        }

        [Category("Events"), Description("Event raised when the frame becomes the active object on the page"), DefaultValue("")]
        public string onActivate
        {
            get
            {
                return base.GetStringAttribute("onActivate ");
            }
            set
            {
                base.SetStringAttribute("onActivate ", value);
            }
        }

        [Category("Events"), DefaultValue(""), Description("Event raised when the frame is no longer the active object on the page")]
        public string onDeactivate
        {
            get
            {
                return base.GetStringAttribute("onDeactivate");
            }
            set
            {
                base.SetStringAttribute("onDeactivate", value);
            }
        }

        [Description("Event raised after the frame has been loaded"), DefaultValue(""), Category("Events")]
        public string onLoad
        {
            get
            {
                return base.GetStringAttribute("onLoad");
            }
            set
            {
                base.SetStringAttribute("onLoad", value);
            }
        }

        [Description("Event raised when the user changes the scrolling position of this object"), Category("Events"), DefaultValue("")]
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

        [Description("True if the frame should allow scrolling"), DefaultValue(2), Category("Behavior")]
        public ScrollType scrolling
        {
            get
            {
                return (ScrollType) base.GetEnumAttribute("scrolling", ScrollType.Auto);
            }
            set
            {
                base.SetEnumAttribute("scrolling", value, ScrollType.Auto);
            }
        }

        [DefaultValue(""), Category("Default"), Description("A URL that specifies the location of the frame's source.")]
        public string src
        {
            get
            {
                return base.GetStringAttribute("src");
            }
            set
            {
                base.SetStringAttribute("src", value);
            }
        }

        [DefaultValue((short) (-32768))]
        public override short tabIndex
        {
            get
            {
                return (short) base.GetIntegerAttribute("tabIndex", -32768);
            }
            set
            {
                base.SetIntegerAttribute("tabIndex", value, -32768);
            }
        }

        [DefaultValue(0), Category("Appearance"), Description("The amount of padding above and below the frame")]
        public int vSpace
        {
            get
            {
                return base.GetIntegerAttribute("vSpace", 0);
            }
            set
            {
                base.SetIntegerAttribute("vSpace", value, 0);
            }
        }

        [DefaultValue(typeof(Unit), ""), Category("Layout"), Description("The width of the frame")]
        public Unit width
        {
            get
            {
                return base.GetUnitAttribute("width");
            }
            set
            {
                base.SetUnitAttribute("width", value);
            }
        }
    }
}

