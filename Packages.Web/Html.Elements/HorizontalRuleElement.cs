namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Web.UI.WebControls;

    [ToolboxHtml("<hr>", "Horizontal Rule")]
    public sealed class HorizontalRuleElement : Element
    {
        internal HorizontalRuleElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Category("Appearance"), Description("The alignment of this horizontal rule"), DefaultValue(0)]
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

        [Description("The color of this horizontal rule"), TypeConverter(typeof(WebColorConverter)), Category("Appearance"), DefaultValue(typeof(Color), "")]
        public Color color
        {
            get
            {
                return base.GetColorAttribute("color");
            }
            set
            {
                base.SetColorAttribute("color", value);
            }
        }

        [Category("Appearance"), DefaultValue(false), Description("Specifies whether to draw the horizontal rule with 3-D shading")]
        public bool noShade
        {
            get
            {
                return base.GetBooleanAttribute("noShade");
            }
            set
            {
                base.SetBooleanAttribute("noShade", value);
            }
        }

        [DefaultValue(1), Description("The thickness of this horizontal rule"), Category("Appearance")]
        public int size
        {
            get
            {
                return base.GetIntegerAttribute("size", 1);
            }
            set
            {
                base.SetIntegerAttribute("size", value, 1);
            }
        }
    }
}

