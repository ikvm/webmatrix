namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<button>Button</button>", "DHTML Button")]
    public sealed class ButtonElement : SelectableElement
    {
        internal ButtonElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Category("Behavior"), DefaultValue(false), Description("True if this object is disabled")]
        public bool disabled
        {
            get
            {
                return base.GetBooleanAttribute("disabled");
            }
            set
            {
                base.SetBooleanAttribute("disabled", value);
            }
        }

        [Category("Default"), DefaultValue(""), Description("The name used to identify this link when submitting form data")]
        public string name
        {
            get
            {
                return base.GetStringAttribute("name");
            }
            set
            {
                base.SetStringAttribute("name", value);
            }
        }
    }
}

