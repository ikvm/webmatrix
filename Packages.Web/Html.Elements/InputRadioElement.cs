namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;

    [ToolboxHtml("<input type=\"radio\">", "Radio Button")]
    public sealed class InputRadioElement : InputCheckboxElement
    {
        internal InputRadioElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        protected override string type
        {
            get
            {
                return "radio";
            }
        }
    }
}

