namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    [ToolboxHtml("<input type=\"hidden\">", "Hidden Field")]
    public sealed class InputHiddenElement : Element
    {
        internal InputHiddenElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        public override string ToString()
        {
            return "<input type='hidden'>";
        }

        [Category("Default"), Description("The identifier used to access this object from code"), ParenthesizePropertyName(true), DefaultValue(""), MergableProperty(false)]
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

        [Description("The name associated with this object when submitting form data"), DefaultValue(""), Category("Default")]
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

        [Category("Events"), DefaultValue(""), Description("Event raised when this object's value has changed")]
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

        [Description("True if this is a server-side control"), DefaultValue(false), Category("Default")]
        public bool runAtServer
        {
            get
            {
                string attribute = base.GetAttribute("runAt") as string;
                return ((attribute != null) && attribute.Equals("server"));
            }
            set
            {
                if (value)
                {
                    base.SetAttribute("runAt", "server");
                }
                else
                {
                    base.RemoveAttribute("runAt");
                }
            }
        }

        [DefaultValue(""), Description("The data or text to include when submitting form data"), Category("Default")]
        public string value
        {
            get
            {
                return base.GetStringAttribute("value");
            }
            set
            {
                base.SetStringAttribute("value", value);
            }
        }
    }
}

