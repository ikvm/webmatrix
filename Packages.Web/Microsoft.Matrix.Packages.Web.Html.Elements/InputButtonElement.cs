namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<input type=\"button\" value=\"Button\">", "Button")]
    public class InputButtonElement : InputElement
    {
        internal InputButtonElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        protected override string type
        {
            get
            {
                return "button";
            }
        }

        [Category("Appearance"), Description("The text displayed on this button")]
        public override string value
        {
            get
            {
                return base.value;
            }
            set
            {
                base.value = value;
            }
        }
    }
}

