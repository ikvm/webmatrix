namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<input type=\"reset\" value=\"Reset\">", "Reset Button")]
    public sealed class InputResetElement : InputButtonElement
    {
        internal InputResetElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        protected override string type
        {
            get
            {
                return "reset";
            }
        }

        [DefaultValue("Reset")]
        public override string value
        {
            get
            {
                return base.GetStringAttribute("value", "Reset");
            }
            set
            {
                base.SetStringAttribute("value", value, "Reset");
            }
        }
    }
}

