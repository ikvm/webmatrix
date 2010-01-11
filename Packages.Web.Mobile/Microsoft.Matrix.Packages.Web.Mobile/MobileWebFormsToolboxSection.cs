namespace Microsoft.Matrix.Packages.Web.Mobile
{
    using Microsoft.Matrix.Packages.Web.Designer;
    using System;
    using System.Drawing;
    using System.Runtime.Serialization;
    using System.Web.UI.MobileControls;

    [Serializable]
    public class MobileWebFormsToolboxSection : WebFormsToolboxSection
    {
        public static MobileWebFormsToolboxSection MobileWebForms;

        protected MobileWebFormsToolboxSection()
        {
        }

        public MobileWebFormsToolboxSection(string name) : base(name)
        {
            base.Icon = new Icon(typeof(WebFormsToolboxSection), "WebFormsToolboxSection.ico");
            if (MobileWebForms == null)
            {
                MobileWebForms = this;
            }
        }

        private MobileWebFormsToolboxSection(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
            if (MobileWebForms == null)
            {
                MobileWebForms = this;
            }
        }

        public override Type ComponentType
        {
            get
            {
                return typeof(MobileControl);
            }
        }
    }
}

