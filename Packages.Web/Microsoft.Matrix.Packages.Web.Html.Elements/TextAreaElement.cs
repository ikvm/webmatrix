namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<textarea></textarea>", "TextArea")]
    public sealed class TextAreaElement : SelectableElement
    {
        internal TextAreaElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Description("The number of columns of text to be shown"), DefaultValue(20), Category("Appearance")]
        public int cols
        {
            get
            {
                return base.GetIntegerAttribute("cols", 20);
            }
            set
            {
                base.SetIntegerAttribute("cols", value, 20);
            }
        }

        [Description("True if this object is disabled"), Category("Behavior"), DefaultValue(false)]
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

        [Description("The name that identifies this object when submitting form data"), Category("Default"), DefaultValue("")]
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

        [DefaultValue(""), Category("Events"), Description("Event raised when the text has changed")]
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

        [Category("Events"), Description("Event raised when the user changes the scrolling position"), DefaultValue("")]
        public string onScroll
        {
            get
            {
                return base.GetStringAttribute("onScroll");
            }
            set
            {
                base.SetStringAttribute("onScroll", value);
            }
        }

        [Description("True if the control is read-only"), Category("Behavior"), DefaultValue(false)]
        public bool readOnly
        {
            get
            {
                return base.GetBooleanAttribute("readOnly");
            }
            set
            {
                base.SetBooleanAttribute("readOnly", value);
            }
        }

        [DefaultValue(2), Description("The number of rows of text to be shown"), Category("Appearance")]
        public int rows
        {
            get
            {
                return base.GetIntegerAttribute("rows", 2);
            }
            set
            {
                base.SetIntegerAttribute("rows", value, 2);
            }
        }
    }
}

