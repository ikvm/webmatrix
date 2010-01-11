namespace Microsoft.Matrix.Packages.Web.Html
{
    using System;
    using System.Collections;
    using System.Text;

    public sealed class EditorGlyphTable
    {
        private ArrayList _standardGlyphs;
        private Hashtable _table = new Hashtable();

        public void Add(EditorGlyph glyph)
        {
            this._table[glyph.Key] = glyph;
        }

        public void AddStandardGlyphs()
        {
            foreach (EditorGlyph glyph in this.StandardGlyphs)
            {
                this.Add(glyph);
            }
        }

        public EditorGlyph Get(string tag, EditorGlyphType type)
        {
            return (EditorGlyph) this._table[EditorGlyph.GetGlyphKey(tag, type)];
        }

        public void Remove(EditorGlyph glyph)
        {
            this._table.Remove(glyph.Key);
        }

        public void Remove(string tag, EditorGlyphType type)
        {
            this._table.Remove(EditorGlyph.GetGlyphKey(tag, type));
        }

        public void RemoveStandardGlyphs()
        {
            this.Remove("comment", EditorGlyphType.OpenTag);
            this.Remove("form", EditorGlyphType.OpenTag);
            this.Remove("form", EditorGlyphType.CloseTag);
            this.Remove(" ", EditorGlyphType.OpenTag);
            this.Remove(" ", EditorGlyphType.CloseTag);
            this.Remove("div", EditorGlyphType.OpenTag);
            this.Remove("div", EditorGlyphType.CloseTag);
            this.Remove("span", EditorGlyphType.OpenTag);
            this.Remove("span", EditorGlyphType.CloseTag);
            this.Remove("para", EditorGlyphType.OpenTag);
            this.Remove("para", EditorGlyphType.CloseTag);
            this.Remove("br", EditorGlyphType.OpenTag);
        }

        public string DefinitonString
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                IDictionaryEnumerator enumerator = this._table.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    EditorGlyph glyph = (EditorGlyph) current.Value;
                    if (glyph.Visible)
                    {
                        builder.Append(glyph.DefinitionString);
                    }
                }
                return builder.ToString();
            }
        }

        public ICollection StandardGlyphs
        {
            get
            {
                if (this._standardGlyphs == null)
                {
                    string str = "res://" + typeof(EditorGlyphTable).Module.FullyQualifiedName + "/";
                    this._standardGlyphs = new ArrayList();
                    this._standardGlyphs.Add(new EditorGlyph("comment", EditorGlyphType.OpenTag, str + "COMMENT_GLYPH", 0x18, 0x10));
                    this._standardGlyphs.Add(new EditorGlyph("form", EditorGlyphType.OpenTag, str + "FORM_O_GLYPH", 0x42, 0x11));
                    this._standardGlyphs.Add(new EditorGlyph("form", EditorGlyphType.CloseTag, str + "FORM_C_GLYPH", 0x31, 0x11));
                    this._standardGlyphs.Add(new EditorGlyph(" ", EditorGlyphType.OpenTag, str + "UNKNOWN_O_GLYPH", 0x42, 0x11));
                    this._standardGlyphs.Add(new EditorGlyph(" ", EditorGlyphType.CloseTag, str + "UNKNOWN_C_GLYPH", 0x18, 0x10));
                    this._standardGlyphs.Add(new EditorGlyph("div", EditorGlyphType.OpenTag, str + "DIV_O_GLYPH", 50, 0x11));
                    this._standardGlyphs.Add(new EditorGlyph("div", EditorGlyphType.CloseTag, str + "DIV_C_GLYPH", 0x27, 0x11));
                    this._standardGlyphs.Add(new EditorGlyph("span", EditorGlyphType.OpenTag, str + "SPAN_O_GLYPH", 0x3e, 0x11));
                    this._standardGlyphs.Add(new EditorGlyph("span", EditorGlyphType.CloseTag, str + "SPAN_C_GLYPH", 0x31, 0x11));
                    this._standardGlyphs.Add(new EditorGlyph("para", EditorGlyphType.OpenTag, str + "PARA_O_GLYPH", 0x17, 0x10));
                    this._standardGlyphs.Add(new EditorGlyph("para", EditorGlyphType.CloseTag, str + "PARA_C_GLYPH", 0x17, 0x10));
                    this._standardGlyphs.Add(new EditorGlyph("br", EditorGlyphType.OpenTag, str + "BREAK_GLYPH", 0x12, 0x10));
                }
                return this._standardGlyphs;
            }
        }
    }
}

