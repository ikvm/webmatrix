namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<select size=3></select>", "ListBox")]
    public class ListBoxElement : SelectElement
    {
        internal ListBoxElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Description("Indicates whether the user can select multiple items"), DefaultValue(false), Category("Behavior")]
        public bool multiple
        {
            get
            {
                return base.GetBooleanAttribute("multiple");
            }
            set
            {
                base.SetBooleanAttribute("multiple", value);
            }
        }

        [DefaultValue(3), Category("Layout"), Description("The vertical size of this control, measured in rows of text")]
        public int size
        {
            get
            {
                return base.GetIntegerAttribute("size", 3);
            }
            set
            {
                base.SetIntegerAttribute("size", value, 3);
            }
        }
    }
}

