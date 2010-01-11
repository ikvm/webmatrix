namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Web.UI.WebControls;

    [ToolboxHtml("<table><tr><td>Table</td><td> </td><td> </td></tr><tr><td> </td><td> </td><td> </td></tr><tr><td> </td><td> </td><td> </td></tr></table>", "Table")]
    public sealed class TableElement : SelectableElement
    {
        internal TableElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [DefaultValue(0), Description("The alignment of the table on the page"), Category("Layout")]
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

        [DefaultValue(""), Description("The background image of the table"), Category("Appearance")]
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

        [Category("Appearance"), DefaultValue(typeof(Color), ""), Description("The color of the table background"), TypeConverter(typeof(WebColorConverter))]
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

        [DefaultValue(0), Description("The thickness of the border around the table"), Category("Appearance")]
        public int border
        {
            get
            {
                return base.GetIntegerAttribute("border", 0);
            }
            set
            {
                base.SetIntegerAttribute("border", value, 0);
            }
        }

        [Category("Appearance"), Description("The color of the page border"), TypeConverter(typeof(WebColorConverter)), DefaultValue(typeof(Color), "")]
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

        [DefaultValue(1), Category("Appearance"), Description("The padding between the border and the contents for each cell in the table")]
        public int cellPadding
        {
            get
            {
                return base.GetIntegerAttribute("cellpadding", 1);
            }
            set
            {
                base.SetIntegerAttribute("cellPadding", value, 1);
            }
        }

        [DefaultValue(1), Category("Appearance"), Description("The spacing between the cells in the table")]
        public int cellSpacing
        {
            get
            {
                return base.GetIntegerAttribute("cellSpacing", 1);
            }
            set
            {
                base.SetIntegerAttribute("cellSpacing", value, 1);
            }
        }

        [DefaultValue(typeof(Unit), ""), Category("Layout"), Description("The height of the table")]
        public Unit height
        {
            get
            {
                return base.GetUnitAttribute("height");
            }
            set
            {
                base.Peer.GetStyle().RemoveAttribute("height", 1);
                base.SetUnitAttribute("height", value);
            }
        }

        [Category("Events"), DefaultValue(""), Description("Event raised when the size of the table changes")]
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

        [Description("Specifies which inner borders are displayed"), Category("Appearance"), DefaultValue(0)]
        public RulesType rules
        {
            get
            {
                return (RulesType) base.GetEnumAttribute("rules", RulesType.NotSet);
            }
            set
            {
                base.SetEnumAttribute("rules", value, RulesType.NotSet);
            }
        }

        [Description("The width of the table"), Category("Layout"), DefaultValue(typeof(Unit), "")]
        public Unit width
        {
            get
            {
                return base.GetUnitAttribute("width");
            }
            set
            {
                base.Peer.GetStyle().RemoveAttribute("width", 1);
                base.SetUnitAttribute("width", value);
            }
        }
    }
}

