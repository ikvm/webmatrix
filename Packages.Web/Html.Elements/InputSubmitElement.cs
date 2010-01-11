namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<input type=\"submit\" value=\"Submit\">", "Submit Button")]
    public sealed class InputSubmitElement : InputButtonElement
    {
        internal InputSubmitElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        protected override string type
        {
            get
            {
                return "submit";
            }
        }

        [DefaultValue("Submit Query")]
        public override string value
        {
            get
            {
                return base.GetStringAttribute("value", "Submit Query");
            }
            set
            {
                base.SetStringAttribute("value", value, "Submit Query");
            }
        }
    }
}

