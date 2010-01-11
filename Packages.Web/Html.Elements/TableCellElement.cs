namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Web.UI.WebControls;

    public sealed class TableCellElement : StyledElement
    {
        internal TableCellElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Description("The alignment of the cell's contents"), Category("Appearance"), DefaultValue(0)]
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

        [Category("Appearance"), Description("The background image of the cell"), DefaultValue("")]
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

        [TypeConverter(typeof(WebColorConverter)), Description("The color of the cell's background"), Category("Appearance"), DefaultValue(typeof(Color), "")]
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

        [DefaultValue(1), Description("The thickness of the table's border"), Category("Appearance")]
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

        [Category("Appearance"), DefaultValue(typeof(Color), ""), Description("The color of the page's border"), TypeConverter(typeof(WebColorConverter))]
        public Color borderColor
        {
            get
            {
                return base.GetColorAttribute("borderColor");
            }
            set
            {
                base.SetColorAttribute("borderColor", value);
            }
        }

        [Description("The number of columns the cell spans"), DefaultValue(1), Category("Layout")]
        public int colSpan
        {
            get
            {
                return base.GetIntegerAttribute("colSpan", 1);
            }
            set
            {
                base.SetIntegerAttribute("colSpan", value, 1);
            }
        }

        [Category("Layout"), Description("The height of the cell"), DefaultValue(typeof(Unit), "")]
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

        [Description("Specifies whether to to keep text on one line instead of word-wrapping"), DefaultValue(false), Category("Behavior")]
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

        [Category("Events"), DefaultValue(""), Description("Event raised when the size of the cell changes")]
        public string onResize
        {
            get
            {
                return base.GetStringAttribute("onResize");
            }
            set
            {
                base.SetStringAttribute("onResize", value);
            }
        }

        [Category("Layout"), Description("The number of rows the cell spans"), DefaultValue(1)]
        public int rowSpan
        {
            get
            {
                return base.GetIntegerAttribute("rowSpan", 1);
            }
            set
            {
                base.SetIntegerAttribute("rowSpan", value, 1);
            }
        }

        [Category("Appearance"), DefaultValue(0), Description("The vertical alignment of the cell")]
        public VerticalAlign vAlign
        {
            get
            {
                return (VerticalAlign) base.GetEnumAttribute("vAlign", VerticalAlign.NotSet);
            }
            set
            {
                base.SetEnumAttribute("vAlign", value, VerticalAlign.NotSet);
            }
        }

        [DefaultValue(typeof(Unit), ""), Category("Layout"), Description("The width of the cell")]
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

