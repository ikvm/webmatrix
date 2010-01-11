namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using System;
    using System.Web.Mobile;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;

    public class DesignerValidationSummaryAdapter : HtmlValidationSummaryAdapter
    {
        public DesignerValidationSummaryAdapter(ValidationSummary validationSummary)
        {
            base.set_Control(validationSummary);
        }

        public override unsafe void Render(HtmlMobileTextWriter writer)
        {
            string str;
            Alignment alignment = *((Alignment*) base.get_Style().get_Item(Style.AlignmentKey, true));
            Wrapping wrapping = *((Wrapping*) base.get_Style().get_Item(Style.WrappingKey, true));
            bool flag = (wrapping == 1) || (wrapping == 0);
            string str2 = "100%";
            Utils.EnterZeroFontSizeTag(writer);
            writer.EnterLayout(base.get_Style());
            writer.WriteBeginTag("div");
            if (!flag)
            {
                str = "overflow-x:hidden;width:" + str2 + ";";
            }
            else
            {
                str = "word-wrap:break-word;width:" + str2 + ";";
            }
            Utils.WriteStyleAttribute(writer, base.get_Style(), str);
            if (alignment != null)
            {
                writer.WriteAttribute("align", Enum.GetName(typeof(Alignment), alignment));
            }
            writer.Write(">");
            writer.WriteText(base.get_Control().get_HeaderText(), true);
            writer.WriteFullBeginTag("ul");
            for (int i = 1; i <= 2; i++)
            {
                writer.WriteFullBeginTag("li");
                writer.Write(Constants.ValidationSummaryErrorMessage, i.ToString());
                writer.WriteEndTag("li");
            }
            writer.WriteEndTag("ul");
            writer.WriteBeginTag("a");
            writer.WriteAttribute("href", "NavigationUrl");
            writer.Write(">");
            writer.WriteText((base.get_Control().get_BackLabel() == string.Empty) ? base.GetDefaultLabel(ControlAdapter.BackLabel) : base.get_Control().get_BackLabel(), true);
            writer.WriteEndTag("a");
            writer.WriteEndTag("div");
            writer.ExitLayout(base.get_Style());
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

