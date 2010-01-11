namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    public abstract class StyledElement : Element
    {
        internal StyledElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [DefaultValue(""), Description("The style class associated with this object"), Category("Style")]
        public string @class
        {
            get
            {
                return base.Peer.GetClassName();
            }
            set
            {
                base.Peer.SetClassName(value);
            }
        }

        [MergableProperty(false), Description("The identifier used to refer to this object in script"), DefaultValue(""), Category("Default"), ParenthesizePropertyName(true)]
        public string id
        {
            get
            {
                return base.Peer.GetId();
            }
            set
            {
                base.Peer.SetId(value);
            }
        }

        [Description("Event raised when the user clicks on this object"), DefaultValue(""), Category("Events")]
        public string onClick
        {
            get
            {
                return base.GetStringAttribute("onClick");
            }
            set
            {
                base.SetStringAttribute("onClick", value);
            }
        }

        [Description("Event raised when the mouse pointer leaves the area over this object"), DefaultValue(""), Category("Events")]
        public string onMouseOut
        {
            get
            {
                return base.GetStringAttribute("onMouseOut");
            }
            set
            {
                base.SetStringAttribute("onMouseOut", value);
            }
        }

        [Description("Event raised when the mouse pointer enters the area over this object"), Category("Events"), DefaultValue("")]
        public string onMouseOver
        {
            get
            {
                return base.GetStringAttribute("onMouseOver");
            }
            set
            {
                base.SetStringAttribute("onMouseOver", value);
            }
        }

        [Description("The inline style of this object"), Editor(typeof(ElementStyleEditor), typeof(UITypeEditor)), Category("Style"), DefaultValue("")]
        public string style
        {
            get
            {
                return base.Peer.GetStyle().GetCssText();
            }
            set
            {
                base.Peer.GetStyle().SetCssText(value);
            }
        }

        [DefaultValue(""), Category("Default"), Description("The title of the object, which is used as its 'ToolTip' description")]
        public string title
        {
            get
            {
                return base.GetStringAttribute("title");
            }
            set
            {
                base.SetStringAttribute("title", value);
            }
        }
    }
}

