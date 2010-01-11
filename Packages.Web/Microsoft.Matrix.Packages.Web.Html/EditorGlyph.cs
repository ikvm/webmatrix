namespace Microsoft.Matrix.Packages.Web.Html
{
    using System;
    using System.Text;

    public sealed class EditorGlyph
    {
        private int _height;
        private string _imageUrl;
        private string _tag;
        private EditorGlyphType _type;
        private bool _visible;
        private int _width;

        public EditorGlyph(string tag, EditorGlyphType type, string imageUrl, int width, int height)
        {
            this._tag = tag;
            this._imageUrl = imageUrl;
            this._type = type;
            this._width = width;
            this._height = height;
            this._visible = true;
        }

        internal static string GetGlyphKey(string tag, EditorGlyphType type)
        {
            return (tag + type);
        }

        public string DefinitionString
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("%%");
                builder.Append(this._tag);
                builder.Append("^^%%");
                builder.Append(this._imageUrl);
                builder.Append("^^%%");
                builder.Append((int) this._type);
                builder.Append("^^%%");
                builder.Append(3);
                builder.Append("^^%%");
                builder.Append(3);
                builder.Append("^^%%");
                builder.Append(4);
                builder.Append("^^%%");
                builder.Append(this._width);
                builder.Append("^^%%");
                builder.Append(this._height);
                builder.Append("^^%%");
                builder.Append(this._width);
                builder.Append("^^%%");
                builder.Append(this._height);
                builder.Append("^^**");
                return builder.ToString();
            }
        }

        public int Height
        {
            get
            {
                return this._height;
            }
            set
            {
                this._height = value;
            }
        }

        public string ImageUrl
        {
            get
            {
                return this._imageUrl;
            }
            set
            {
                this._imageUrl = value;
            }
        }

        public string Key
        {
            get
            {
                return GetGlyphKey(this.Tag, this.Type);
            }
        }

        public string Tag
        {
            get
            {
                return this._tag;
            }
            set
            {
                this._tag = value;
            }
        }

        public EditorGlyphType Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        public bool Visible
        {
            get
            {
                return this._visible;
            }
            set
            {
                this._visible = value;
            }
        }

        public int Width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }
    }
}

