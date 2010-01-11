namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    [ToolboxHtml("<img src=\"\">", "Image")]
    public sealed class ImageElement : SelectableElement
    {
        internal ImageElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Category("Appearance"), Description("The alignment of the image on the page"), DefaultValue(0)]
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

        [Category("Appearance"), Description("Alternate text that can be used if the image is unavailable"), DefaultValue("")]
        public string alt
        {
            get
            {
                return base.GetStringAttribute("alt");
            }
            set
            {
                base.SetStringAttribute("alt", value);
            }
        }

        [Description("The thickness of the border around the image"), Category("Appearance"), DefaultValue(1)]
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

        [Description("The height of the image in pixels"), Category("Layout"), DefaultValue(30)]
        public int height
        {
            get
            {
                return base.GetIntegerAttribute("height", 30);
            }
            set
            {
                base.SetIntegerAttribute("height", value, 30);
            }
        }

        [Category("Appearance"), DefaultValue(0), Description("The amount of padding to the left and right of the image")]
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

        [Category("Events"), Description("Event raised when the user double-clicks on the image"), DefaultValue("")]
        public string onDblClick
        {
            get
            {
                return base.GetStringAttribute("onDblClick");
            }
            set
            {
                base.SetStringAttribute("onDblClick", value);
            }
        }

        [Category("Events"), Description("Event raised when a mouse click begins on the image"), DefaultValue("")]
        public string onMouseDown
        {
            get
            {
                return base.GetStringAttribute("onMouseDown");
            }
            set
            {
                base.SetStringAttribute("onMouseDown", value);
            }
        }

        [Description("Event raised when a mouse click ends on the image"), Category("Events"), DefaultValue("")]
        public string onMouseUp
        {
            get
            {
                return base.GetStringAttribute("onMouseUp");
            }
            set
            {
                base.SetStringAttribute("onMouseUp", value);
            }
        }

        [Description("The image URL"), DefaultValue(""), Category("Default")]
        public string src
        {
            get
            {
                string stringAttribute = base.GetStringAttribute("src");
                return base.GetRelativeUrl(stringAttribute);
            }
            set
            {
                base.SetStringAttribute("src", value);
            }
        }

        [Category("Appearance"), Description("The amount of padding above and below the image"), DefaultValue(0)]
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

        [Category("Layout"), Description("The width of the image in pixels"), DefaultValue(0x1c)]
        public int width
        {
            get
            {
                return base.GetIntegerAttribute("width", 0x1c);
            }
            set
            {
                base.SetIntegerAttribute("width", value, 0x1c);
            }
        }
    }
}

