namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Adapters
{
    using System;
    using System.Web.Mobile;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;

    public class DesignerObjectListAdapter : HtmlObjectListAdapter
    {
        public DesignerObjectListAdapter(ObjectList objectList)
        {
            base.set_Control(objectList);
        }

        public override void Render(HtmlMobileTextWriter writer)
        {
            writer.WriteBeginTag("div");
            Utils.WriteDesignerStyleAttributes(writer, base.get_Control(), base.get_Style());
            writer.Write("\">");
            base.Render(writer);
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

