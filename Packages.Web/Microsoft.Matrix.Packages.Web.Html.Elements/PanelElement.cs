namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;

    [ToolboxHtml("<div style=\"height = 100; width=100\">Div</div>", "Panel")]
    public sealed class PanelElement : DivElement
    {
        internal PanelElement(Interop.IHTMLElement peer) : base(peer)
        {
        }
    }
}

