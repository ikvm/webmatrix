namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<input type=\"checkbox\">", "CheckBox")]
    public class InputCheckboxElement : InputElement
    {
        internal InputCheckboxElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Category("Behavior"), DefaultValue(false), Description("Indicates whether this object is checked")]
        public bool @checked
        {
            get
            {
                return base.GetBooleanAttribute("checked");
            }
            set
            {
                base.SetBooleanAttribute("checked", value);
            }
        }

        protected override string type
        {
            get
            {
                return "checkbox";
            }
        }

        [DefaultValue("on"), Description("The data or text to include when submitting form data"), Category("Default")]
        public override string value
        {
            get
            {
                return base.GetStringAttribute("value");
            }
            set
            {
                base.SetStringAttribute("value", value);
            }
        }
    }
}

