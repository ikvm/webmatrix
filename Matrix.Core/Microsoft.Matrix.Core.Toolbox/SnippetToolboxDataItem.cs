namespace Microsoft.Matrix.Core.Toolbox
{
    using System;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Windows.Forms;

    [Serializable]
    public sealed class SnippetToolboxDataItem : ToolboxDataItem
    {
        private string _displayName;
        private static Bitmap snippetGlyph;

        public SnippetToolboxDataItem(string toolboxData) : this(toolboxData, string.Empty)
        {
        }

        private SnippetToolboxDataItem(SerializationInfo info, StreamingContext context)
        {
            this.Deserialize(info, context);
        }

        internal SnippetToolboxDataItem(string toolboxData, string displayName) : base(toolboxData)
        {
            this._displayName = displayName;
        }

        protected override void Deserialize(SerializationInfo info, StreamingContext context)
        {
            base.Deserialize(info, context);
            this._displayName = info.GetString("DisplayName");
        }

        private string EncodeHtml(string html)
        {
            if ((html != null) && (html.Length != 0))
            {
                int length = html.Length;
                StringBuilder builder = new StringBuilder(length);
                builder.Append("<pre>");
                for (int i = 0; i < length; i++)
                {
                    char ch = html[i];
                    switch (ch)
                    {
                        case '<':
                        {
                            builder.Append("&lt;");
                            continue;
                        }
                        case '>':
                        {
                            builder.Append("&gt;");
                            continue;
                        }
                        case '&':
                        {
                            builder.Append("&amp;");
                            continue;
                        }
                    }
                    builder.Append(ch);
                }
                builder.Append("</pre>");
                html = builder.ToString();
            }
            return html;
        }

        public override DataObject GetDataObject(IDesignerHost designerHost)
        {
            string toolboxData = base.ToolboxData;
            DataObject obj2 = new DataObject(DataFormats.Text, toolboxData);
            obj2.SetData(DataFormats.Html, this.EncodeHtml(toolboxData));
            return obj2;
        }

        protected override void Serialize(SerializationInfo info, StreamingContext context)
        {
            base.Serialize(info, context);
            info.AddValue("DisplayName", this._displayName);
        }

        public override void SetDisplayName(string name)
        {
            this._displayName = name;
        }

        public override bool CanSetDisplayName
        {
            get
            {
                return true;
            }
        }

        public override string DisplayName
        {
            get
            {
                if (this.InternalDisplayName.Length > 0)
                {
                    return (this.InternalDisplayName + Environment.NewLine + base.ToolboxData);
                }
                return base.ToolboxData;
            }
        }

        public override Image Glyph
        {
            get
            {
                return SnippetGlyph;
            }
        }

        internal string InternalDisplayName
        {
            get
            {
                return this._displayName;
            }
        }

        private static Image SnippetGlyph
        {
            get
            {
                if (snippetGlyph == null)
                {
                    snippetGlyph = new Bitmap(typeof(SnippetToolboxDataItem), "Snippet.bmp");
                    snippetGlyph.MakeTransparent(Color.Fuchsia);
                }
                return snippetGlyph;
            }
        }
    }
}

