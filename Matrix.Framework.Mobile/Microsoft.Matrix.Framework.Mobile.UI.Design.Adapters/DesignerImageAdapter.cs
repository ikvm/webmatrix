namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters
{
    using System;
    using System.Web.Mobile;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;

    public class DesignerImageAdapter : HtmlImageAdapter
    {
        public DesignerImageAdapter(Image image)
        {
            base.set_Control(image);
        }

        public override unsafe void Render(HtmlMobileTextWriter writer)
        {
            Alignment alignment = *((Alignment*) base.get_Style().get_Item(Style.AlignmentKey, true));
            string str = "100%";
            writer.WriteBeginTag("div");
            if (alignment == 2)
            {
                writer.WriteAttribute("align", "center");
            }
            writer.WriteAttribute("style", "overflow-x:hidden;width:" + str);
            writer.Write(">");
            string str2 = base.get_Control().get_ImageUrl();
            writer.WriteBeginTag("img");
            Utils.WriteStyleAttribute(writer, base.get_Style(), null);
            if (str2 != "")
            {
                writer.WriteAttribute("src", str2, true);
            }
            if (base.get_Control().get_AlternateText() != "")
            {
                writer.Write(" alt=\"");
                writer.WriteText(base.get_Control().get_AlternateText(), true);
                writer.Write("\"");
            }
            if ((alignment == 3) || (alignment == 1))
            {
                writer.WriteAttribute("align", Enum.GetName(typeof(Alignment), alignment));
            }
            writer.WriteAttribute("border", "0");
            writer.Write(">");
            writer.WriteEndTag("div");
        }

        public override MobileCapabilities Device
        {
            get
            {
                return Utils.DesignerCapabilities;
            }
        }
    }
}

