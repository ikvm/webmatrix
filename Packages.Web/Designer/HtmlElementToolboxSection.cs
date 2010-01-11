namespace Microsoft.Matrix.Packages.Web.Designer
{
    using Microsoft.Matrix.Core.Toolbox;
    using System;
    using System.Drawing;
    using System.Runtime.Serialization;

    [Serializable]
    public class HtmlElementToolboxSection : ToolboxSection
    {
        public static HtmlElementToolboxSection HtmlElements;

        protected HtmlElementToolboxSection()
        {
        }

        public HtmlElementToolboxSection(string name) : base(name)
        {
            base.Icon = new Icon(typeof(HtmlElementToolboxSection), "HtmlElementToolboxSection.ico");
            if (HtmlElements == null)
            {
                HtmlElements = this;
            }
        }

        private HtmlElementToolboxSection(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
            if (HtmlElements == null)
            {
                HtmlElements = this;
            }
        }

        public override ToolboxDataItem CreateToolboxDataItem(string toolboxData)
        {
            return new HtmlElementToolboxDataItem(toolboxData);
        }
    }
}

