namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters
{
    using System;
    using System.Web.Mobile;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;

    public class DesignerTextBoxAdapter : HtmlTextBoxAdapter
    {
        public DesignerTextBoxAdapter(TextBox textBox)
        {
            base.set_Control(textBox);
        }

        public override unsafe void Render(HtmlMobileTextWriter writer)
        {
            bool flag = base.get_Control().get_Password();
            int num = base.get_Control().get_Size();
            Alignment alignment = *((Alignment*) base.get_Style().get_Item(Style.AlignmentKey, true));
            writer.Write("<div style='width:" + "100%");
            if (alignment != null)
            {
                writer.Write(";text-align:" + Enum.GetName(typeof(Alignment), alignment));
            }
            writer.Write("'>");
            Utils.EnterZeroFontSizeTag(writer);
            writer.EnterLayout(base.get_Style());
            writer.WriteBeginTag("input");
            Utils.WriteStyleAttribute(writer, base.get_Style(), null);
            if (base.get_Control().get_Text() != string.Empty)
            {
                writer.Write(" value=\"");
                writer.WriteText(base.get_Control().get_Text(), true);
                writer.Write("\" ");
            }
            writer.WriteAttribute("size", num.ToString());
            if (flag)
            {
                writer.WriteAttribute("type", "password");
            }
            writer.Write("/>");
            writer.ExitLayout(base.get_Style());
            Utils.ExitZeroFontSizeTag(writer);
            writer.Write("</div>");
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

