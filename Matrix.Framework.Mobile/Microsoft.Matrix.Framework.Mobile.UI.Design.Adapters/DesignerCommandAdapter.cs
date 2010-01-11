namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters
{
    using System;
    using System.Web.Mobile;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;

    public class DesignerCommandAdapter : HtmlCommandAdapter
    {
        public DesignerCommandAdapter(Command command)
        {
            base.set_Control(command);
        }

        public override unsafe void Render(HtmlMobileTextWriter writer)
        {
            Alignment alignment = *((Alignment*) base.get_Style().get_Item(Style.AlignmentKey, true));
            if (base.get_Control().get_ImageUrl().Length == 0)
            {
                Utils.EnterZeroFontSizeTag(writer);
                writer.WriteBeginTag("div");
                if (base.get_Control().get_Format() == null)
                {
                    writer.WriteAttribute("style", "width:100%");
                    if (alignment != null)
                    {
                        writer.WriteAttribute("align", Enum.GetName(typeof(Alignment), alignment));
                    }
                    writer.Write(">");
                    writer.EnterLayout(base.get_Style());
                    writer.WriteBeginTag("input");
                    Utils.WriteStyleAttribute(writer, base.get_Style(), null);
                    writer.WriteAttribute("type", "submit");
                    writer.Write(" value=\"");
                    writer.WriteText(base.get_Control().get_Text(), true);
                    writer.Write("\"/>");
                    writer.ExitLayout(base.get_Style());
                }
                else
                {
                    Wrapping wrapping = *((Wrapping*) base.get_Style().get_Item(Style.WrappingKey, true));
                    if ((wrapping != 1) && (wrapping != 0))
                    {
                        writer.WriteAttribute("style", "overflow-x:hidden;width:100%");
                    }
                    else
                    {
                        writer.WriteAttribute("style", "word-wrap:break-word;width:100%");
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
                }
                writer.WriteEndTag("div");
                Utils.ExitZeroFontSizeTag(writer);
            }
            else
            {
                writer.WriteBeginTag("div");
                if (alignment == 2)
                {
                    writer.WriteAttribute("align", "center");
                }
                writer.WriteAttribute("style", "overflow-x:hidden;width:100%");
                writer.Write(">");
                writer.WriteBeginTag("img");
                Utils.WriteStyleAttribute(writer, base.get_Style(), null);
                writer.WriteAttribute("src", base.get_Control().get_ImageUrl(), true);
                if ((alignment == 3) || (alignment == 1))
                {
                    writer.WriteAttribute("align", Enum.GetName(typeof(Alignment), alignment));
                }
                writer.WriteAttribute("border", "0");
                writer.Write(">");
                writer.WriteEndTag("div");
            }
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

