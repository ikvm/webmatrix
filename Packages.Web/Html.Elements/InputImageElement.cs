namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<input type=\"image\">", "Image Button")]
    public sealed class InputImageElement : InputElement
    {
        internal InputImageElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Description("Alternate text can be used if this image is unavailable"), Category("Behavior"), DefaultValue("")]
        public string alt
        {
            get
            {
                return base.GetStringAttribute("alt");
            }
            set
            {
                base.SetStringAttribute("alt", value);
            }
        }

        [Description("The amount of padding to the left and right of this object"), DefaultValue(0), Category("Layout")]
        public int hSpace
        {
            get
            {
                return base.GetIntegerAttribute("hSpace", 0);
            }
            set
            {
                base.SetIntegerAttribute("hSpace", value, 0);
            }
        }

        [Description("The URL used to locate the image"), Category("Behavior"), DefaultValue("")]
        public string src
        {
            get
            {
                string stringAttribute = base.GetStringAttribute("src");
                return base.GetRelativeUrl(stringAttribute);
            }
            set
            {
                base.SetStringAttribute("src", value);
            }
        }

        protected override string type
        {
            get
            {
                return "image";
            }
        }

        [Category("Layout"), DefaultValue(0), Description("The amount of padding above and below this object")]
        public int vSpace
        {
            get
            {
                return base.GetIntegerAttribute("vSpace", 0);
            }
            set
            {
                base.SetIntegerAttribute("vSpace", value, 0);
            }
        }
    }
}

