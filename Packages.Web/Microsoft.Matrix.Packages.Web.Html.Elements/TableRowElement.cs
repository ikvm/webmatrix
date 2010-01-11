namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Web.UI.WebControls;

    public sealed class TableRowElement : StyledElement
    {
        internal TableRowElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [DefaultValue(typeof(Color), ""), Description("The color of the table background"), TypeConverter(typeof(WebColorConverter)), Category("Appearance")]
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

        [Category("Appearance"), DefaultValue(typeof(Color), ""), Description("The color of the row border"), TypeConverter(typeof(WebColorConverter))]
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

        [Description("The height of the row in pixels"), Category("Appearance"), DefaultValue(0x19)]
        public int height
        {
            get
            {
                return base.GetIntegerAttribute("height", 0x19);
            }
            set
            {
                base.SetIntegerAttribute("height", value, 0x19);
            }
        }
    }
}

