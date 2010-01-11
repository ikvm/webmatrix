namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<input type=\"file\">", "File Upload")]
    public sealed class InputFileElement : InputElement
    {
        internal InputFileElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Category("Appearance"), Description("The width of this control, measured in characters"), DefaultValue(20)]
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
                return "file";
            }
        }
    }
}

