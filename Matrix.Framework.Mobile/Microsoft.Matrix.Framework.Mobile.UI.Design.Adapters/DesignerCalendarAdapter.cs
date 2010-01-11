namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters
{
    using System;
    using System.Web.Mobile;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;

    public class DesignerCalendarAdapter : HtmlCalendarAdapter
    {
        public DesignerCalendarAdapter(Calendar calendar)
        {
            base.set_Control(calendar);
        }

        public override unsafe void Render(HtmlMobileTextWriter writer)
        {
            writer.WriteBeginTag("div");
            writer.WriteAttribute("style", "cellpadding=2px;width:" + "100%");
            Alignment alignment = *((Alignment*) base.get_Style().get_Item(Style.AlignmentKey, true));
            if (alignment != null)
            {
                writer.WriteAttribute("align", Enum.GetName(typeof(Alignment), alignment));
            }
            writer.Write("/>");
            Utils.EnterZeroFontSizeTag(writer);
            Utils.ApplyStyleToWebControl(base.get_Style(), base.get_Control().get_WebCalendar());
            base.Render(writer);
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

