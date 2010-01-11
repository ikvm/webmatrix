namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<span><label>Label</label></span>", "Label")]
    public sealed class LabelElement : StyledElement
    {
        internal LabelElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Description("The shortcut key for the label's associated element"), Category("Behavior"), DefaultValue("")]
        public string accessKey
        {
            get
            {
                return base.GetStringAttribute("accessKey");
            }
            set
            {
                if ((value != null) && (value.Length != 1))
                {
                    throw new ArgumentException("AccessKey cannot be longer than one character");
                }
                base.SetStringAttribute("accessKey", value);
            }
        }

        [Category("Behavior"), DefaultValue(""), Description("The object to which the label corresponds")]
        public string @for
        {
            get
            {
                return base.GetStringAttribute("htmlFor");
            }
            set
            {
                base.SetStringAttribute("htmlFor", value);
            }
        }
    }
}

