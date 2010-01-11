namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<input type=\"password\">", "Password")]
    public sealed class InputPasswordElement : InputTextElement
    {
        internal InputPasswordElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        protected override string type
        {
            get
            {
                return "password";
            }
        }

        [Browsable(false)]
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

