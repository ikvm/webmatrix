namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    internal class MxDataGridTable : Table
    {
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            if (this.ID == null)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.Parent.ClientID);
            }
        }
    }
}

