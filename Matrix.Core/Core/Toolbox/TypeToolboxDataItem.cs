namespace Microsoft.Matrix.Core.Toolbox
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    [Serializable]
    public class TypeToolboxDataItem : ToolboxDataItem
    {
        private Type _componentType;
        private string _displayName;
        private Image _glyph;

        protected TypeToolboxDataItem()
        {
        }

        public TypeToolboxDataItem(string toolboxData) : base(toolboxData)
        {
            this._componentType = Type.GetType(toolboxData, false);
            if ((this._componentType == null) || !this.SupportedComponentType.IsAssignableFrom(this._componentType))
            {
                throw new ArgumentException("Toolbox data does not represent a valid component type");
            }
        }

        private TypeToolboxDataItem(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
        }

        protected override void Deserialize(SerializationInfo info, StreamingContext context)
        {
            base.Deserialize(info, context);
            this._glyph = (Image) info.GetValue("Glyph", typeof(Image));
            this._displayName = info.GetString("DisplayName");
        }

        public override DataObject GetDataObject(IDesignerHost designerHost)
        {
            return null;
        }

        protected override void Serialize(SerializationInfo info, StreamingContext context)
        {
            base.Serialize(info, context);
            info.AddValue("Glyph", this._glyph);
            info.AddValue("DisplayName", this.DisplayName);
        }

        protected Type ComponentType
        {
            get
            {
                if (this._componentType == null)
                {
                    this._componentType = Type.GetType(base.ToolboxData, false);
                    if (this._componentType == null)
                    {
                        MessageBox.Show("Couldn't find the assembly containing " + this.DisplayName, "Web Matrix", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
                return this._componentType;
            }
        }

        public override string DisplayName
        {
            get
            {
                if (this._displayName == null)
                {
                    Type componentType = this.ComponentType;
                    if (componentType != null)
                    {
                        this._displayName = componentType.Name;
                    }
                    else
                    {
                        this._displayName = string.Empty;
                    }
                }
                return this._displayName;
            }
        }

        public override Image Glyph
        {
            get
            {
                if (this._glyph == null)
                {
                    Type componentType = this.ComponentType;
                    if (componentType != null)
                    {
                        ToolboxBitmapAttribute attribute = (ToolboxBitmapAttribute) TypeDescriptor.GetAttributes(componentType)[typeof(ToolboxBitmapAttribute)];
                        if (attribute != null)
                        {
                            this._glyph = attribute.GetImage(componentType, false) as Bitmap;
                        }
                    }
                    if (this._glyph == null)
                    {
                        this._glyph = new Bitmap(0x10, 0x10);
                    }
                }
                return this._glyph;
            }
        }

        protected virtual Type SupportedComponentType
        {
            get
            {
                return typeof(IComponent);
            }
        }
    }
}

