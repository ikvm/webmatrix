namespace Microsoft.Matrix.Packages.Web.Designer
{
    using Microsoft.Matrix.Core.Toolbox;
    using System;
    using System.ComponentModel.Design;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    [Serializable]
    public class WebFormsToolboxDataItem : TypeToolboxDataItem
    {
        internal static string WebFormsToolboxDataFormat = "TypeToolboxItem";

        protected WebFormsToolboxDataItem()
        {
        }

        public WebFormsToolboxDataItem(string toolboxData) : base(toolboxData)
        {
        }

        private WebFormsToolboxDataItem(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
        }

        public override DataObject GetDataObject(IDesignerHost designerHost)
        {
            DataObject obj2 = new DataObject();
            obj2.SetData(WebFormsToolboxDataFormat, base.ComponentType);
            return obj2;
        }
    }
}

