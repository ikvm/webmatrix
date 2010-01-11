namespace Microsoft.Matrix.Packages.Web.Html.Css
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public sealed class FontCssAttributes : CssAttributes
    {
        private CssAttribute _colorAttribute = new CssAttribute("color", "color");
        private CssAttribute _decorationAttribute = new CssAttribute("text-decoration", "textDecoration");
        private CssAttribute _fontAttribute = new CssAttribute("font", "font");
        private CssAttribute _fontFamilyAttribute = new CssAttribute("font-family", "fontFamily");
        private CssAttribute _sizeAttribute = new CssAttribute("font-size", "fontSize");
        private CssAttribute _styleAttribute = new CssAttribute("font-style", "fontStyle");
        private CssAttribute _variantAttribute = new CssAttribute("font-variant", "fontVariant");
        private CssAttribute _weightAttribute = new CssAttribute("font-weight", "fontWeight");

        protected override PropertyDescriptorCollection CreateProperties(PropertyDescriptorCollection reflectedProperties)
        {
            ArrayList list = new ArrayList();
            list.Add(new CssAttributes.StylePropertyDescriptor(reflectedProperties["Name"], "font-family:"));
            list.Add(new CssAttributes.StylePropertyDescriptor(reflectedProperties["Size"], "font-size:"));
            list.Add(new CssAttributes.StylePropertyDescriptor(reflectedProperties["Weight"], "font-weight:"));
            list.Add(new CssAttributes.StylePropertyDescriptor(reflectedProperties["Style"], "font-style:"));
            list.Add(new CssAttributes.StylePropertyDescriptor(reflectedProperties["Color"], "color:"));
            list.Add(new CssAttributes.StylePropertyDescriptor(reflectedProperties["Decoration"], "text-decoration:"));
            list.Add(new CssAttributes.StylePropertyDescriptor(reflectedProperties["Variant"], "font-variant:"));
            return new PropertyDescriptorCollection((PropertyDescriptor[]) list.ToArray(typeof(PropertyDescriptor)));
        }

        protected override void FillAttributes(IList attributes)
        {
            attributes.Add(this._fontAttribute);
            attributes.Add(this._fontFamilyAttribute);
            attributes.Add(this._colorAttribute);
            attributes.Add(this._styleAttribute);
            attributes.Add(this._sizeAttribute);
            attributes.Add(this._variantAttribute);
            attributes.Add(this._weightAttribute);
            attributes.Add(this._decorationAttribute);
        }

        [Category("(Default)"), Description("The color of the font to be used.\r\nExamples: Red; #FF0000")]
        public System.Drawing.Color Color
        {
            get
            {
                string htmlColor = this._colorAttribute.Value;
                if (htmlColor != null)
                {
                    return ColorTranslator.FromHtml(htmlColor);
                }
                return System.Drawing.Color.Empty;
            }
            set
            {
                if (value.IsEmpty)
                {
                    this._colorAttribute.Value = null;
                }
                else
                {
                    this._colorAttribute.Value = ColorTranslator.ToHtml(value);
                }
            }
        }

        [Description("Toggles line decorations on the font being used.\r\nExamples: None; Underline Strikethrough"), Category("Effects")]
        public TextDecoration Decoration
        {
            get
            {
                string str = this._decorationAttribute.Value;
                TextDecoration notSet = TextDecoration.NotSet;
                if ((str != null) && (str.Length != 0))
                {
                    str = str.ToLower();
                    if (str.IndexOf("none") >= 0)
                    {
                        return TextDecoration.None;
                    }
                    if (str.IndexOf("underline") >= 0)
                    {
                        notSet |= TextDecoration.Underline;
                    }
                    if (str.IndexOf("overline") >= 0)
                    {
                        notSet |= TextDecoration.Overline;
                    }
                    if (str.IndexOf("line-through") >= 0)
                    {
                        notSet |= TextDecoration.Strikethrough;
                    }
                }
                return notSet;
            }
            set
            {
                string str = null;
                if ((value & TextDecoration.None) != TextDecoration.NotSet)
                {
                    str = "none";
                }
                else
                {
                    if ((value & TextDecoration.Underline) != TextDecoration.NotSet)
                    {
                        str = "underline";
                    }
                    if ((value & TextDecoration.Strikethrough) != TextDecoration.NotSet)
                    {
                        str = str + " line-through";
                    }
                    if ((value & TextDecoration.Overline) != TextDecoration.NotSet)
                    {
                        str = str + " overline";
                    }
                }
                this._decorationAttribute.Value = str;
            }
        }

        [Description("The name or the font or a sequence of fonts to be used.\r\nExamples: Verdana, Helvetica; Menu"), Category("(Default)"), Editor(typeof(FontEditor), typeof(UITypeEditor))]
        public string Name
        {
            get
            {
                string str = this._fontAttribute.Value;
                if ((str != null) && (str.Length != 0))
                {
                    return str;
                }
                str = this._fontFamilyAttribute.Value;
                if (str != null)
                {
                    return str;
                }
                return string.Empty;
            }
            set
            {
                this._fontAttribute.Value = null;
                if ((value == null) || (value.Length == 0))
                {
                    this._fontFamilyAttribute.Value = null;
                }
                else
                {
                    this._fontFamilyAttribute.Value = value;
                }
                base.ResetProperties();
            }
        }

        [Description("The size of the font to be used.\r\nExamples: 10pt; Larger; X-Large"), Category("Details")]
        public FontUnit Size
        {
            get
            {
                string s = this._sizeAttribute.Value;
                if (s != null)
                {
                    return FontUnit.Parse(s, CultureInfo.InvariantCulture);
                }
                return FontUnit.Empty;
            }
            set
            {
                if (value.IsEmpty)
                {
                    this._sizeAttribute.Value = null;
                }
                else
                {
                    this._sizeAttribute.Value = value.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        [Description("The style of the font to be used.\r\nExamples: Normal; Italic"), Category("Details")]
        public FontStyle Style
        {
            get
            {
                string str = this._styleAttribute.Value;
                if ((str != null) && (str.Length != 0))
                {
                    str = str.ToLower();
                    if (str.Equals("normal"))
                    {
                        return FontStyle.Normal;
                    }
                    if (str.Equals("italic"))
                    {
                        return FontStyle.Italic;
                    }
                }
                return FontStyle.NotSet;
            }
            set
            {
                string str = null;
                switch (value)
                {
                    case FontStyle.Normal:
                        str = "normal";
                        break;

                    case FontStyle.Italic:
                        str = "italic";
                        break;
                }
                this._styleAttribute.Value = str;
            }
        }

        [Description("Toggles the small cap variant of the font used.\r\nExamples: Normal; SmallCaps"), Category("Effects")]
        public FontVariant Variant
        {
            get
            {
                string str = this._variantAttribute.Value;
                if ((str != null) && (str.Length != 0))
                {
                    str = str.ToLower();
                    if (str.Equals("normal"))
                    {
                        return FontVariant.Normal;
                    }
                    if (str.Equals("small-caps"))
                    {
                        return FontVariant.SmallCaps;
                    }
                }
                return FontVariant.NotSet;
            }
            set
            {
                string str = null;
                switch (value)
                {
                    case FontVariant.Normal:
                        str = "normal";
                        break;

                    case FontVariant.SmallCaps:
                        str = "small-caps";
                        break;
                }
                this._variantAttribute.Value = str;
            }
        }

        [Category("Details"), Description("The weight of the font to be used.\r\nExamples: Bold; Lighter")]
        public FontWeight Weight
        {
            get
            {
                string str = this._weightAttribute.Value;
                if ((str != null) && (str.Length != 0))
                {
                    str = str.ToLower();
                    if (str.Equals("normal") || str.Equals("400"))
                    {
                        return FontWeight.Normal;
                    }
                    if (str.Equals("bold") || str.Equals("700"))
                    {
                        return FontWeight.Bold;
                    }
                    if (str.Equals("lighter"))
                    {
                        return FontWeight.Lighter;
                    }
                    if (str.Equals("bolder"))
                    {
                        return FontWeight.Bolder;
                    }
                }
                return FontWeight.NotSet;
            }
            set
            {
                string str = null;
                switch (value)
                {
                    case FontWeight.Normal:
                        str = "normal";
                        break;

                    case FontWeight.Bold:
                        str = "bold";
                        break;

                    case FontWeight.Lighter:
                        str = "lighter";
                        break;

                    case FontWeight.Bolder:
                        str = "bolder";
                        break;
                }
                this._weightAttribute.Value = str;
            }
        }
    }
}

