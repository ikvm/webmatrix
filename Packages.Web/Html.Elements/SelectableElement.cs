namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;

    public abstract class SelectableElement : StyledElement
    {
        internal SelectableElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [DefaultValue(""), Description("The shortcut key for this object"), Category("Behavior")]
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

        [Category("Behavior"), DefaultValue(false), Description("Specifies whether to hide the focus rectangle when this object is selected")]
        public bool hideFocus
        {
            get
            {
                return base.GetBooleanAttribute("hideFocus");
            }
            set
            {
                base.SetBooleanAttribute("hideFocus", value);
            }
        }

        [Description("Event raised when this object is selected"), Category("Events"), DefaultValue("")]
        public string onFocus
        {
            get
            {
                return base.GetStringAttribute("onFocus");
            }
            set
            {
                base.SetStringAttribute("onFocus", value);
            }
        }

        [DefaultValue(""), Description("Event raised after the user types a character while this object is selected"), Category("Events")]
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

        [DefaultValue((short) 0), Category("Behavior"), Description("The tab index for this object")]
        public virtual short tabIndex
        {
            get
            {
                return (short) base.GetIntegerAttribute("tabIndex", 0);
            }
            set
            {
                base.SetIntegerAttribute("tabIndex", value, 0);
            }
        }
    }
}

