namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    [ToolboxHtml("<a href=\"\">Anchor</a>", "Anchor")]
    public sealed class AnchorElement : SelectableElement
    {
        internal AnchorElement(Interop.IHTMLElement peer) : base(peer)
        {
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

        [DefaultValue(""), Description("The URL to which this anchor links"), Category("Behavior")]
        public string href
        {
            get
            {
                string stringAttribute = base.GetStringAttribute("href");
                return base.GetRelativeUrl(stringAttribute);
            }
            set
            {
                base.SetStringAttribute("href", value);
            }
        }

        [Description("Event raised when a mouse click begins over this object"), Category("Events"), DefaultValue("")]
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

        [DefaultValue(""), Description("Event raised when a mouse click ends over this object"), Category("Events")]
        public string onMouseUp
        {
            get
            {
                return base.GetStringAttribute("onmouseup");
            }
            set
            {
                base.SetStringAttribute("onmouseup", value);
            }
        }

        [Category("Behavior"), DefaultValue(""), Description("The target of this link"), TypeConverter(typeof(TargetConverter))]
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

