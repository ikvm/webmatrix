namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters
{
    using System;
    using System.Web.Mobile;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;

    public class DesignerAdRotatorAdapter : HtmlControlAdapter
    {
        public DesignerAdRotatorAdapter(AdRotator adRotator)
        {
            base.set_Control(adRotator);
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
            writer.WriteAttribute("style", "padding=2px;overflow-x:hidden;width:" + str);
            writer.Write(">");
            Utils.EnterZeroFontSizeTag(writer);
            writer.WriteBeginTag("img");
            writer.WriteAttribute("alt", base.get_Control().ID);
            Utils.WriteStyleAttribute(writer, base.get_Style(), null);
            if ((alignment == 3) || (alignment == 1))
            {
                writer.WriteAttribute("align", Enum.GetName(typeof(Alignment), alignment));
            }
            writer.WriteAttribute("height", "40");
            writer.WriteAttribute("width", "250");
            writer.WriteAttribute("border", "0");
            writer.Write(">");
            Utils.ExitZeroFontSizeTag(writer);
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

