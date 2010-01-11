namespace Microsoft.Matrix.Packages.Web.Html.Css
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;

    public sealed class BackgroundCssAttributes : CssAttributes
    {
        private CssAttribute _colorAttribute = new CssAttribute("background-color", "backgroundColor");
        private CssAttribute _hPosAttribute = new CssAttribute("background-position-x", "backgroundPositionX");
        private CssAttribute _imageAttribute = new CssAttribute("background-image", "backgroundImage", true);
        private CssAttribute _repeatAttribute = new CssAttribute("background-repeat", "backgroundRepeat");
        private CssAttribute _scrollAttribute = new CssAttribute("background-scroll", "backgroundScroll");
        private CssAttribute _vPosAttribute = new CssAttribute("background-position-y", "backgroundPositionY");

        protected override PropertyDescriptorCollection CreateProperties(PropertyDescriptorCollection reflectedProperties)
        {
            ArrayList list = new ArrayList();
            bool readOnly = this.Url.ToLower().Equals("none");
            list.Add(new CssAttributes.StylePropertyDescriptor(reflectedProperties["Color"], "background-color:"));
            list.Add(new CssAttributes.StylePropertyDescriptor(reflectedProperties["Url"], "background-image:"));
            list.Add(new CssAttributes.StylePropertyDescriptor(reflectedProperties["Repeat"], "background-repeat:", readOnly));
            list.Add(new CssAttributes.StylePropertyDescriptor(reflectedProperties["Scroll"], "background-scroll:", readOnly));
            list.Add(new CssAttributes.StylePropertyDescriptor(reflectedProperties["HorizontalPosition"], "background-position-x:", readOnly));
            list.Add(new CssAttributes.StylePropertyDescriptor(reflectedProperties["VerticalPosition"], "background-position-y:", readOnly));
            return new PropertyDescriptorCollection((PropertyDescriptor[]) list.ToArray(typeof(PropertyDescriptor)));
        }

        protected override void FillAttributes(IList attributes)
        {
            attributes.Add(this._colorAttribute);
            attributes.Add(this._repeatAttribute);
            attributes.Add(this._scrollAttribute);
            attributes.Add(this._hPosAttribute);
            attributes.Add(this._vPosAttribute);
            attributes.Add(this._imageAttribute);
        }

        [Description("The background color of the element.\r\nExamples: White; #FFFFFF"), Category("(Default)")]
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

        [Category("Behavior"), Description("Where the background is horizontally placed.\r\nExamples: 10px; 25%; Center")]
        public BackgroundPosition HorizontalPosition
        {
            get
            {
                string str = this._hPosAttribute.Value;
                if (str != null)
                {
                    return new BackgroundPosition(str, CultureInfo.InvariantCulture);
                }
                return BackgroundPosition.Empty;
            }
            set
            {
                string str = null;
                switch (value.Type)
                {
                    case BackgroundPositionType.NotSet:
                        break;

                    case BackgroundPositionType.TopOrLeft:
                        str = "left";
                        break;

                    case BackgroundPositionType.BottomOrRight:
                        str = "right";
                        break;

                    default:
                        str = value.ToString(CultureInfo.InvariantCulture);
                        break;
                }
                this._hPosAttribute.Value = str;
            }
        }

        [Category("Behavior"), Description("How the background should be repeated.\r\nExamples: Repeat; NoRepeat")]
        public BackgroundRepeat Repeat
        {
            get
            {
                string str = this._repeatAttribute.Value;
                if ((str != null) && (str.Length != 0))
                {
                    str = str.ToLower();
                    if (str.Equals("repeat"))
                    {
                        return BackgroundRepeat.Repeat;
                    }
                    if (str.Equals("repeat-x"))
                    {
                        return BackgroundRepeat.RepeatX;
                    }
                    if (str.Equals("repeat-y"))
                    {
                        return BackgroundRepeat.RepeatY;
                    }
                    if (str.Equals("no-repeat"))
                    {
                        return BackgroundRepeat.NoRepeat;
                    }
                }
                return BackgroundRepeat.NotSet;
            }
            set
            {
                string str = null;
                switch (value)
                {
                    case BackgroundRepeat.Repeat:
                        str = "repeat";
                        break;

                    case BackgroundRepeat.RepeatX:
                        str = "repeat-x";
                        break;

                    case BackgroundRepeat.RepeatY:
                        str = "repeat-y";
                        break;

                    case BackgroundRepeat.NoRepeat:
                        str = "no-repeat";
                        break;
                }
                this._repeatAttribute.Value = str;
            }
        }

        [Description("How the background should be scrolled with the document.\r\nExamples: Scroll; Fixed"), Category("Behavior")]
        public BackgroundScroll Scroll
        {
            get
            {
                string str = this._scrollAttribute.Value;
                if ((str != null) && (str.Length != 0))
                {
                    str = str.ToLower();
                    if (str.Equals("scroll"))
                    {
                        return BackgroundScroll.Scroll;
                    }
                    if (str.Equals("fixed"))
                    {
                        return BackgroundScroll.Fixed;
                    }
                }
                return BackgroundScroll.NotSet;
            }
            set
            {
                string str = null;
                switch (value)
                {
                    case BackgroundScroll.Scroll:
                        str = "scroll";
                        break;

                    case BackgroundScroll.Fixed:
                        str = "fixed";
                        break;
                }
                this._scrollAttribute.Value = str;
            }
        }

        [Category("(Default)"), Description("The background image of the element.\r\nExamples: url(bg.jpg); none")]
        public string Url
        {
            get
            {
                string str = this._imageAttribute.Value;
                if (str != null)
                {
                    return str;
                }
                return string.Empty;
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    this._imageAttribute.Value = null;
                }
                else if (value.ToLower().Equals("none"))
                {
                    this._scrollAttribute.Value = null;
                    this._repeatAttribute.Value = null;
                    this._hPosAttribute.Value = null;
                    this._vPosAttribute.Value = null;
                    this._imageAttribute.Value = "none";
                }
                else
                {
                    if (!value.StartsWith("url("))
                    {
                        value = "url(" + value + ")";
                    }
                    this._imageAttribute.Value = value;
                }
                base.ResetProperties();
            }
        }

        [Description("Where the background is vertically placed.\r\nExamples: 10px; 25%; Top"), Category("Behavior")]
        public BackgroundPosition VerticalPosition
        {
            get
            {
                string str = this._vPosAttribute.Value;
                if (str != null)
                {
                    return new BackgroundPosition(str, CultureInfo.InvariantCulture);
                }
                return BackgroundPosition.Empty;
            }
            set
            {
                string str = null;
                switch (value.Type)
                {
                    case BackgroundPositionType.NotSet:
                        break;

                    case BackgroundPositionType.TopOrLeft:
                        str = "top";
                        break;

                    case BackgroundPositionType.BottomOrRight:
                        str = "bottom";
                        break;

                    default:
                        str = value.ToString(CultureInfo.InvariantCulture);
                        break;
                }
                this._vPosAttribute.Value = str;
            }
        }
    }
}

