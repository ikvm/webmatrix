namespace Microsoft.Matrix.Core.Toolbox
{
    using Microsoft.Matrix.Core.Plugins;
    using System;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    [Serializable]
    public sealed class CodeWizardToolboxDataItem : ToolboxDataItem, ISerializable
    {
        private string _name;
        private static Bitmap codeWizardGlyph;

        public CodeWizardToolboxDataItem(string toolboxData) : base(toolboxData)
        {
            Type element = Type.GetType(toolboxData, false);
            if (element == null)
            {
                throw new ArgumentException("toolboxData");
            }
            PluginAttribute attribute = Attribute.GetCustomAttribute(element, typeof(PluginAttribute), false) as PluginAttribute;
            if (attribute == null)
            {
                this._name = element.Name;
            }
            else
            {
                this._name = attribute.Name + Environment.NewLine + attribute.Description;
            }
        }

        private CodeWizardToolboxDataItem(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
        }

        protected override void Deserialize(SerializationInfo info, StreamingContext context)
        {
            base.Deserialize(info, context);
            this._name = info.GetString("Name");
        }

        public override DataObject GetDataObject(IDesignerHost designerHost)
        {
            return new DataObject(CodeWizard.CodeWizardDataFormat, base.ToolboxData);
        }

        protected override void Serialize(SerializationInfo info, StreamingContext context)
        {
            base.Serialize(info, context);
            info.AddValue("Name", this._name);
        }

        private static Image CodeWizardGlyph
        {
            get
            {
                if (codeWizardGlyph == null)
                {
                    codeWizardGlyph = new Bitmap(typeof(CodeWizardToolboxDataItem), "CodeWizard.bmp");
                    codeWizardGlyph.MakeTransparent(Color.Red);
                }
                return codeWizardGlyph;
            }
        }

        public override string DisplayName
        {
            get
            {
                return this._name;
            }
        }

        public override Image Glyph
        {
            get
            {
                return CodeWizardGlyph;
            }
        }
    }
}

