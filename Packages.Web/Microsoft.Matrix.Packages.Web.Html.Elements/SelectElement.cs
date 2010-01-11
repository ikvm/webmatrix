namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<select></select>", "ComboBox")]
    public class SelectElement : SelectableElement
    {
        internal SelectElement(Interop.IHTMLElement peer) : base(peer)
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

        [Description("The name used to identify this object when submitting form data"), Category("Default"), DefaultValue("")]
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

        [Description("Event raised when the selected value has changed"), Category("Events"), DefaultValue("")]
        public string onChange
        {
            get
            {
                return base.GetStringAttribute("onChange");
            }
            set
            {
                base.SetStringAttribute("onChange", value);
            }
        }
    }
}

