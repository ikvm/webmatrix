namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<input type=\"text\">", "TextBox")]
    public class InputTextElement : InputElement
    {
        internal InputTextElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Description("The maximum number of characters that can be entered"), Category("Behavior"), DefaultValue(0x7fffffff)]
        public int maxLength
        {
            get
            {
                return base.GetIntegerAttribute("maxLength", 0x7fffffff);
            }
            set
            {
                if (value < 0)
                {
                    value = 0x7fffffff;
                }
                base.SetIntegerAttribute("maxLength", value, 0x7fffffff);
            }
        }

        [DefaultValue(false), Category("Behavior"), Description("Specifies whether the text box is read-only")]
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

        [DefaultValue(20), Description("The width of the text box, specified as a number of characters"), Category("Appearance")]
        public int size
        {
            get
            {
                return base.GetIntegerAttribute("size", 20);
            }
            set
            {
                base.SetIntegerAttribute("size", value, 20);
            }
        }

        protected override string type
        {
            get
            {
                return "text";
            }
        }
    }
}

