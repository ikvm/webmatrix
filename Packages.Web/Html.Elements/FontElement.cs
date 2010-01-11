namespace Microsoft.Matrix.Packages.Web.Html.Elements
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public sealed class FontElement : Element
    {
        internal FontElement(Interop.IHTMLElement peer) : base(peer)
        {
        }

        [DefaultValue(typeof(Color), ""), Description("The color of the font"), TypeConverter(typeof(WebColorConverter)), Category("Appearance")]
        public Color color
        {
            get
            {
                return base.GetColorAttribute("color");
            }
            set
            {
                base.SetColorAttribute("color", value);
            }
        }

        [TypeConverter(typeof(FontFaceTypeConverter)), Category("Appearance"), Description("The font face"), DefaultValue("")]
        public string face
        {
            get
            {
                return base.GetStringAttribute("face");
            }
            set
            {
                base.SetStringAttribute("face", value);
            }
        }

        [Description("The size of the font"), DefaultValue(typeof(FontUnit), ""), Category("Appearance")]
        public FontUnit size
        {
            get
            {
                int num;
                object attribute = base.GetAttribute("size");
                if (attribute == null)
                {
                    return FontUnit.Empty;
                }
                if (attribute is int)
                {
                    num = (int) attribute;
                }
                else
                {
                    string s = (string) attribute;
                    if (s.Equals("+0"))
                    {
                        return FontUnit.Empty;
                    }
                    if (s.Equals("+1"))
                    {
                        return FontUnit.Larger;
                    }
                    if (s.Equals("-1"))
                    {
                        return FontUnit.Smaller;
                    }
                    try
                    {
                        num = int.Parse(s, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        return FontUnit.Empty;
                    }
                }
                if (num > 7)
                {
                    return FontUnit.Empty;
                }
                return new FontUnit((FontSize) (num + 3));
            }
            set
            {
                string str = null;
                switch (value.Type)
                {
                    case FontSize.Smaller:
                        str = "-1";
                        break;

                    case FontSize.Larger:
                        str = "+1";
                        break;

                    case FontSize.XXSmall:
                    case FontSize.XSmall:
                    case FontSize.Small:
                    case FontSize.Medium:
                    case FontSize.Large:
                    case FontSize.XLarge:
                    case FontSize.XXLarge:
                        str = (((int) value.Type) - 3).ToString();
                        break;
                }
                base.SetStringAttribute("size", str);
            }
        }
    }
}

