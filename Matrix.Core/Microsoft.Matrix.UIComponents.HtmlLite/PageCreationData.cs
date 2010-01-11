namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Collections;
    using System.Drawing;

    internal sealed class PageCreationData
    {
        private bool _antiAliasedText;
        private Color _backColor = SystemColors.Window;
        private SectionElement _currentSection = null;
        private string _fontFamily;
        private float _fontSize;
        private Color _foreColor = SystemColors.WindowText;
        private Color _hoverColor = SystemColors.HotTrack;
        private Color _linkColor = SystemColors.Highlight;
        private BoxEdges _margins = new BoxEdges(4, 4, 4, 4);
        private ArrayList _sections = new ArrayList();
        private Microsoft.Matrix.UIComponents.HtmlLite.Watermark _watermark;

        public void AddElement(Element element, string name)
        {
            if (this._currentSection == null)
            {
                throw new InvalidOperationException("No current panel");
            }
            element.SetName(name);
            this._currentSection.Elements.Add(element);
        }

        public SectionElement BeginNewSection(bool allowWrap, int indentation, string name)
        {
            if (indentation < 0)
            {
                throw new ArgumentOutOfRangeException("indentation");
            }
            SectionElement element = new SectionElement();
            element.AllowWrap = allowWrap;
            element.Indentation = indentation;
            element.SetName(name);
            this._sections.Add(element);
            this._currentSection = element;
            return this._currentSection;
        }

        public void EndCurrentSection()
        {
            this._currentSection = null;
        }

        public void SetAntiAliasedText(bool antiAliasedText)
        {
            this._antiAliasedText = antiAliasedText;
        }

        public void SetColors(Color foreColor, Color backColor, Color linkColor, Color hoverColor)
        {
            if (!foreColor.IsEmpty)
            {
                this._foreColor = foreColor;
            }
            if (!backColor.IsEmpty)
            {
                this._backColor = backColor;
            }
            if (!linkColor.IsEmpty)
            {
                this._linkColor = linkColor;
            }
            if (!hoverColor.IsEmpty)
            {
                this._hoverColor = hoverColor;
            }
        }

        public void SetFont(string fontFamily, float fontSize)
        {
            if (fontSize < 0f)
            {
                throw new ArgumentOutOfRangeException("fontSize");
            }
            this._fontFamily = fontFamily;
            this._fontSize = fontSize;
        }

        public void SetMargins(BoxEdges margins)
        {
            this._margins = margins;
        }

        public void SetWatermark(Microsoft.Matrix.UIComponents.HtmlLite.Watermark watermark)
        {
            this._watermark = watermark;
        }

        public bool AntiAliasedText
        {
            get
            {
                return this._antiAliasedText;
            }
        }

        public Color BackColor
        {
            get
            {
                return this._backColor;
            }
        }

        public string FontFamily
        {
            get
            {
                return this._fontFamily;
            }
        }

        public float FontSize
        {
            get
            {
                return this._fontSize;
            }
        }

        public Color ForeColor
        {
            get
            {
                return this._foreColor;
            }
        }

        public Color HoverColor
        {
            get
            {
                return this._hoverColor;
            }
        }

        public Color LinkColor
        {
            get
            {
                return this._linkColor;
            }
        }

        public BoxEdges Margins
        {
            get
            {
                return this._margins;
            }
        }

        public IList Sections
        {
            get
            {
                return this._sections;
            }
        }

        public Microsoft.Matrix.UIComponents.HtmlLite.Watermark Watermark
        {
            get
            {
                return this._watermark;
            }
        }
    }
}

