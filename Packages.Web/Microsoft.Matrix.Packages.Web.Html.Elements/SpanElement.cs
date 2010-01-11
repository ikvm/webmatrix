namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;

    [ToolboxHtml("<span>Span</span>", "Span")]
    public sealed class SpanElement : SelectableElement
    {
        internal SpanElement(Interop.IHTMLElement peer) : base(peer)
        {
        }
    }
}

