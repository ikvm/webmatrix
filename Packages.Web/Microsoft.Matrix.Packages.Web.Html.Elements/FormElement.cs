namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<form></form>", "Form")]
    public sealed class FormElement : StyledElement
    {
        internal FormElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Category("Behavior"), Description("The uri to which form data will be submitted"), DefaultValue("")]
        public string action
        {
            get
            {
                return base.GetStringAttribute("action");
            }
            set
            {
                base.SetStringAttribute("action", value);
            }
        }

        [Description("The MIME encoding type to use when submitting the data from this form."), Category("Behavior"), DefaultValue(0)]
        public EncodingType encType
        {
            get
            {
                if (!base.GetStringAttribute("encoding").Equals("multipart/form-data"))
                {
                    return EncodingType.UrlEncoded;
                }
                return EncodingType.Multipart;
            }
            set
            {
                base.SetStringAttribute("encoding", (value == EncodingType.Multipart) ? "multipart/form-data" : string.Empty);
            }
        }

        [Description("The method to use when submitting this form"), Category("Behavior"), DefaultValue(0)]
        public FormMethod method
        {
            get
            {
                return (FormMethod) base.GetEnumAttribute("method", FormMethod.Get);
            }
            set
            {
                base.SetEnumAttribute("method", value, FormMethod.Get);
            }
        }

        [Description("Event raised after the user types a character while this object is selected"), DefaultValue(""), Category("Events")]
        public string onKeyPress
        {
            get
            {
                return base.GetStringAttribute("onKeyPress");
            }
            set
            {
                base.SetStringAttribute("onKeyPress", value);
            }
        }

        [Description("Event raised when a mouse click begins within the bounds of this object"), DefaultValue(""), Category("Events")]
        public string onMouseDown
        {
            get
            {
                return base.GetStringAttribute("onMouseDown");
            }
            set
            {
                base.SetStringAttribute("onMouseDown", value);
            }
        }

        [DefaultValue(""), Category("Events"), Description("Event raised when a mouse click finishes within the bounds of this object")]
        public string onMouseUp
        {
            get
            {
                return base.GetStringAttribute("onMouseUp");
            }
            set
            {
                base.SetStringAttribute("onMouseUp", value);
            }
        }

        [Description("Event raised when the data on this form is reset back to its initial state."), DefaultValue(""), Category("Events")]
        public string onReset
        {
            get
            {
                return base.GetStringAttribute("onReset");
            }
            set
            {
                base.SetStringAttribute("onReset", value);
            }
        }

        [DefaultValue(""), Description("Event raised when the data from this form is submitted"), Category("Events")]
        public string onSubmit
        {
            get
            {
                return base.GetStringAttribute("onSubmit");
            }
            set
            {
                base.SetStringAttribute("onSubmit", value);
            }
        }

        [DefaultValue(""), Category("Behavior"), Description("The window to contain the results of submitting this form")]
        public string target
        {
            get
            {
                return base.GetStringAttribute("target");
            }
            set
            {
                base.SetStringAttribute("target", value);
            }
        }
    }
}

