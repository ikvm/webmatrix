namespace Microsoft.Matrix.Core.Toolbox
{
    using System;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    [Serializable]
    public abstract class ToolboxDataItem : ISerializable
    {
        private string _toolboxData;

        protected ToolboxDataItem()
        {
        }

        public ToolboxDataItem(string toolboxData)
        {
            if ((toolboxData == null) || (toolboxData.Length == 0))
            {
                throw new ArgumentNullException("toolboxData");
            }
            this._toolboxData = toolboxData;
        }

        protected virtual void Deserialize(SerializationInfo info, StreamingContext context)
        {
            this._toolboxData = info.GetString("ToolboxData");
        }

        public abstract DataObject GetDataObject(IDesignerHost designerHost);
        protected virtual void Serialize(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ToolboxData", this._toolboxData);
        }

        public virtual void SetDisplayName(string name)
        {
            throw new NotSupportedException();
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.Serialize(info, context);
        }

        public virtual bool CanSetDisplayName
        {
            get
            {
                return false;
            }
        }

        public abstract string DisplayName { get; }

        public abstract Image Glyph { get; }

        public string ToolboxData
        {
            get
            {
                return this._toolboxData;
            }
        }
    }
}

