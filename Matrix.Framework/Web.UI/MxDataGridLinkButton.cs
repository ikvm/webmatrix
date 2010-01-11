namespace Microsoft.Matrix.Framework.Web.UI
{
    using System;
    using System.Drawing;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    internal sealed class MxDataGridLinkButton : LinkButton
    {
        protected override void Render(HtmlTextWriter writer)
        {
            this.SetForeColor();
            base.Render(writer);
        }

        private void SetForeColor()
        {
            if (base.ControlStyle.ForeColor == Color.Empty)
            {
                Control parent = this;
                for (int i = 0; i < 3; i++)
                {
                    parent = parent.Parent;
                    Color foreColor = ((WebControl) parent).ForeColor;
                    if (foreColor != Color.Empty)
                    {
                        this.ForeColor = foreColor;
                        return;
                    }
                }
            }
        }
    }
}

