namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    public abstract class InputElement : SelectableElement
    {
        internal InputElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        public override string ToString()
        {
            return ("<" + base.TagName + " type=\"" + this.type + "\">");
        }

        [DefaultValue(false), Category("Behavior"), Description("True if the input object is disabled")]
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

        [Description("The name used to identify the input object when submitting form data"), DefaultValue(""), Category("Default")]
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

        [Description("Event raised when the input object's value has changed"), DefaultValue(""), Category("Events")]
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

        protected abstract string type { get; }

        [Description("The data or text to include when submitting form data"), Category("Default"), DefaultValue("")]
        public virtual string value
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

