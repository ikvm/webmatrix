namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    [ToolboxHtml("<fieldset><legend>Legend<legend></fieldset>", "GroupBox")]
    public sealed class FieldSetElement : SelectableElement
    {
        internal FieldSetElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [Category("Appearance"), DefaultValue(0), Description("The alignment of the content of the cell")]
        public HorizontalAlign align
        {
            get
            {
                return (HorizontalAlign) base.GetEnumAttribute("align", HorizontalAlign.NotSet);
            }
            set
            {
                base.SetEnumAttribute("align", value, HorizontalAlign.NotSet);
            }
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

        [DefaultValue(""), Description("Event raised when the size of this object changes"), Category("Events")]
        public string onResize
        {
            get
            {
                return base.GetStringAttribute("onResize");
            }
            set
            {
                base.SetStringAttribute("onResize", value);
            }
        }

        [Description("Event raised when the user changes the scrolling position of this object"), Category("Events"), DefaultValue("")]
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
    }
}

