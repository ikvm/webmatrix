namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters
{
    using System;
    using System.Web.Mobile;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;

    public class DesignerLinkAdapter : HtmlLinkAdapter
    {
        public DesignerLinkAdapter(Link link)
        {
            base.set_Control(link);
        }

        public override unsafe void Render(HtmlMobileTextWriter writer)
        {
            Alignment alignment = *((Alignment*) base.get_Style().get_Item(Style.AlignmentKey, true));
            Wrapping wrapping = *((Wrapping*) base.get_Style().get_Item(Style.WrappingKey, true));
            bool flag = (wrapping == 1) || (wrapping == 0);
            Utils.EnterZeroFontSizeTag(writer);
            writer.WriteBeginTag("div");
            string str = "100%";
            if (!flag)
            {
                writer.WriteAttribute("style", "overflow-x:hidden;width:" + str);
            }
            else
            {
                writer.WriteAttribute("style", "word-wrap:break-word;width:" + str);
            }
            if (alignment != null)
            {
                writer.WriteAttribute("align", Enum.GetName(typeof(Alignment), alignment));
            }
            writer.Write(">");
            writer.WriteBeginTag("a");
            writer.WriteAttribute("href", "NavigationUrl");
            writer.Write(">");
            Utils.WriteCssStyleText(writer, base.get_Style(), null, base.get_Control().get_Text(), true);
            writer.WriteEndTag("a");
            writer.WriteEndTag("div");
            Utils.ExitZeroFontSizeTag(writer);
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

