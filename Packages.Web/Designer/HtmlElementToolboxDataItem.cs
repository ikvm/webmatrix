namespace Microsoft.Matrix.Packages.Web.Designer
{
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.Packages.Web.Html.Elements;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    [Serializable]
    public class HtmlElementToolboxDataItem : ToolboxDataItem
    {
        private string _displayName;
        private Type _elementType;
        private Image _glyph;
        private bool _isElement;
        private static Bitmap elementGlyph;

        protected HtmlElementToolboxDataItem()
        {
        }

        public HtmlElementToolboxDataItem(string toolboxData) : base(toolboxData)
        {
            if (toolboxData[0] == '!')
            {
                this._isElement = true;
                this._elementType = Type.GetType(toolboxData.Substring(1));
            }
        }

        private HtmlElementToolboxDataItem(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
        }

        protected override void Deserialize(SerializationInfo info, StreamingContext context)
        {
            base.Deserialize(info, context);
            if (base.ToolboxData[0] == '!')
            {
                this._isElement = true;
                this._glyph = (Image) info.GetValue("Glyph", typeof(Image));
            }
        }

        public override DataObject GetDataObject(IDesignerHost designerHost)
        {
            string data = null;
            if (this._isElement)
            {
                Type elementType = this.ElementType;
                if (elementType != null)
                {
                    ToolboxHtmlAttribute attribute = (ToolboxHtmlAttribute) TypeDescriptor.GetAttributes(elementType)[typeof(ToolboxHtmlAttribute)];
                    if (attribute != null)
                    {
                        data = attribute.Html;
                    }
                }
            }
            else
            {
                data = base.ToolboxData;
            }
            if (data != null)
            {
                DataObject obj2 = new DataObject();
                obj2.SetData(DataFormats.Html, data);
                obj2.SetData(DataFormats.Text, data);
                return obj2;
            }
            return null;
        }

        protected override void Serialize(SerializationInfo info, StreamingContext context)
        {
            base.Serialize(info, context);
            if (this._isElement)
            {
                info.AddValue("Glyph", this._glyph);
            }
        }

        public override string DisplayName
        {
            get
            {
                if (!this._isElement)
                {
                    return base.ToolboxData;
                }
                if (this._displayName == null)
                {
                    Type elementType = this.ElementType;
                    if (elementType != null)
                    {
                        ToolboxHtmlAttribute attribute = (ToolboxHtmlAttribute) TypeDescriptor.GetAttributes(elementType)[typeof(ToolboxHtmlAttribute)];
                        if (attribute != null)
                        {
                            this._displayName = attribute.Name;
                        }
                        else
                        {
                            this._displayName = elementType.Name;
                        }
                    }
                    else
                    {
                        this._displayName = string.Empty;
                    }
                }
                return this._displayName;
            }
        }

        private static Image ElementGlyph
        {
            get
            {
                if (elementGlyph == null)
                {
                    ToolboxBitmapAttribute attribute = (ToolboxBitmapAttribute) TypeDescriptor.GetAttributes(typeof(Element))[typeof(ToolboxBitmapAttribute)];
                    elementGlyph = attribute.GetImage(typeof(Element), false) as Bitmap;
                }
                return elementGlyph;
            }
        }

        protected Type ElementType
        {
            get
            {
                if ((this._elementType == null) && this._isElement)
                {
                    string typeName = base.ToolboxData.Substring(1);
                    this._elementType = Type.GetType(typeName, false);
                }
                return this._elementType;
            }
        }

        public override Image Glyph
        {
            get
            {
                if (!this._isElement)
                {
                    return ElementGlyph;
                }
                if (this._glyph == null)
                {
                    Type elementType = this.ElementType;
                    if (elementType != null)
                    {
                        ToolboxBitmapAttribute attribute = (ToolboxBitmapAttribute) TypeDescriptor.GetAttributes(elementType)[typeof(ToolboxBitmapAttribute)];
                        if (attribute != null)
                        {
                            this._glyph = attribute.GetImage(elementType, false) as Bitmap;
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
    }
}

