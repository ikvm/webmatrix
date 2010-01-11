namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Collections;
    using System.Drawing;

    public class PageBuilder : PageFactory
    {
        private Stack _backColorStack;
        private int _boldCount;
        private Stack _fontFamilyStack;
        private Stack _fontSizeStack;
        private Stack _foreColorStack;
        private int _italicCount;
        private int _strikeoutCount;
        private int _underlineCount;

        public DividerElement AddDivider()
        {
            return this.AddDivider(1);
        }

        public DividerElement AddDivider(int thickness)
        {
            DividerElement element = new DividerElement();
            element.Thickness = thickness;
            this.AddElement(element);
            return element;
        }

        public void AddElement(Element element)
        {
            this.AddElement(element, null, true);
        }

        public void AddElement(Element element, string name)
        {
            this.AddElement(element, name, true);
        }

        protected void AddElement(Element element, string name, bool applyFont)
        {
            this.ApplyFormatting(element, applyFont);
            base.AddElementInternal(element, name);
        }

        public HyperLinkElement AddHyperLink(string text, object userData)
        {
            return this.AddHyperLink(text, userData, null);
        }

        public HyperLinkElement AddHyperLink(string text, object userData, string name)
        {
            HyperLinkElement element = new HyperLinkElement(text);
            element.UserData = userData;
            this.AddElement(element, name, true);
            return element;
        }

        public ImageElement AddImage(Image image)
        {
            return this.AddImage(image, null);
        }

        public ImageElement AddImage(Image image, string name)
        {
            ImageElement element = new ImageElement(image);
            this.AddElement(element, name, false);
            return element;
        }

        public LineBreakElement AddLineBreak()
        {
            LineBreakElement element = new LineBreakElement();
            base.AddElementInternal(element, null);
            return element;
        }

        public TextElement AddText(string text)
        {
            return this.AddText(text, null);
        }

        public TextElement AddText(string text, string name)
        {
            TextElement element = new TextElement(text);
            this.AddElement(element, name);
            return element;
        }

        public TextSpanElement AddTextSpan(string text)
        {
            return this.AddTextSpan(text, null);
        }

        public TextSpanElement AddTextSpan(string text, string name)
        {
            TextSpanElement element = new TextSpanElement(text);
            this.AddElement(element, name);
            return element;
        }

        public TextElement AddWordBreak()
        {
            return this.AddText(" ");
        }

        private void ApplyFormatting(Element element, bool applyFont)
        {
            if (applyFont)
            {
                FontStyle fontStyle = element.FontStyle;
                if (this._boldCount != 0)
                {
                    fontStyle |= FontStyle.Bold;
                }
                if (this._italicCount != 0)
                {
                    fontStyle |= FontStyle.Italic;
                }
                if (this._strikeoutCount != 0)
                {
                    fontStyle |= FontStyle.Strikeout;
                }
                if (this._underlineCount != 0)
                {
                    fontStyle |= FontStyle.Underline;
                }
                element.FontStyle = fontStyle;
                if ((this._fontSizeStack != null) && (this._fontSizeStack.Count != 0))
                {
                    element.FontSize = (float) this._fontSizeStack.Peek();
                }
                if ((this._fontFamilyStack != null) && (this._fontFamilyStack.Count != 0))
                {
                    element.FontFamily = (string) this._fontFamilyStack.Peek();
                }
            }
            if ((this._foreColorStack != null) && (this._foreColorStack.Count != 0))
            {
                element.ForeColor = (Color) this._foreColorStack.Peek();
            }
            if ((this._backColorStack != null) && (this._backColorStack.Count != 0))
            {
                element.BackColor = (Color) this._backColorStack.Peek();
            }
        }

        public SectionElement BeginNewSection()
        {
            return this.BeginNewSection(true, 0, null);
        }

        public SectionElement BeginNewSection(bool allowWrap)
        {
            return this.BeginNewSection(allowWrap, 0, null);
        }

        public SectionElement BeginNewSection(int indentation)
        {
            return this.BeginNewSection(true, indentation, null);
        }

        public SectionElement BeginNewSection(string name)
        {
            return this.BeginNewSection(true, 0, name);
        }

        public SectionElement BeginNewSection(bool allowWrap, int indentation)
        {
            return this.BeginNewSection(allowWrap, indentation, null);
        }

        public SectionElement BeginNewSection(bool allowWrap, int indentation, string name)
        {
            SectionElement element = base.BeginNewSectionInternal(allowWrap, indentation, name);
            this.ApplyFormatting(element, true);
            return element;
        }

        public void Clear()
        {
            base.ClearInternal();
        }

        public void EndCurrentSection()
        {
            base.EndCurrentSectionInternal();
        }

        public void PopBackColor()
        {
            if ((this._backColorStack == null) || (this._backColorStack.Count == 0))
            {
                throw new InvalidOperationException("Unbalanced changes to BackColor");
            }
            this._backColorStack.Pop();
        }

        public void PopBold()
        {
            if (this._boldCount < 1)
            {
                throw new InvalidOperationException("Unbalanced changes to Bold");
            }
            this._boldCount--;
        }

        public void PopFontFamily()
        {
            if ((this._fontFamilyStack == null) || (this._fontFamilyStack.Count == 0))
            {
                throw new InvalidOperationException("Unbalanced changes to FontFamily");
            }
            this._fontFamilyStack.Pop();
        }

        public void PopFontSize()
        {
            if ((this._fontSizeStack == null) || (this._fontSizeStack.Count == 0))
            {
                throw new InvalidOperationException("Unbalanced changes to FontSize");
            }
            this._fontSizeStack.Pop();
        }

        public void PopForeColor()
        {
            if ((this._foreColorStack == null) || (this._foreColorStack.Count == 0))
            {
                throw new InvalidOperationException("Unbalanced changes to ForeColor");
            }
            this._foreColorStack.Pop();
        }

        public void PopItalic()
        {
            if (this._italicCount < 1)
            {
                throw new InvalidOperationException("Unbalanced changes to Italic");
            }
            this._italicCount--;
        }

        public void PopStrikeout()
        {
            if (this._strikeoutCount < 1)
            {
                throw new InvalidOperationException("Unbalanced changes to Strikeout");
            }
            this._strikeoutCount--;
        }

        public void PopUnderline()
        {
            if (this._underlineCount < 1)
            {
                throw new InvalidOperationException("Unbalanced changes to Underline");
            }
            this._underlineCount--;
        }

        public void PushBackColor(Color color)
        {
            if (this._backColorStack == null)
            {
                this._backColorStack = new Stack();
            }
            this._backColorStack.Push(color);
        }

        public void PushBold()
        {
            this._boldCount++;
        }

        public void PushFontFamily(string fontFamily)
        {
            if ((fontFamily == null) || (fontFamily.Length == 0))
            {
                throw new ArgumentNullException("fontFamily");
            }
            if (this._fontFamilyStack == null)
            {
                this._fontFamilyStack = new Stack();
            }
            this._fontFamilyStack.Push(fontFamily);
        }

        public void PushFontSize(float fontSize)
        {
            if (fontSize < 0f)
            {
                throw new ArgumentOutOfRangeException("fontSize");
            }
            if (this._fontSizeStack == null)
            {
                this._fontSizeStack = new Stack();
            }
            this._fontSizeStack.Push(fontSize);
        }

        public void PushForeColor(Color color)
        {
            if (this._foreColorStack == null)
            {
                this._foreColorStack = new Stack();
            }
            this._foreColorStack.Push(color);
        }

        public void PushItalic()
        {
            this._italicCount++;
        }

        public void PushStrikeout()
        {
            this._strikeoutCount++;
        }

        public void PushUnderline()
        {
            this._underlineCount++;
        }

        public void SetPageAntiAliasedText(bool antiAliasedText)
        {
            base.SetPageAntiAliasedTextInternal(antiAliasedText);
        }

        public void SetPageColors(Color foreColor, Color backColor, Color linkColor, Color hoverColor)
        {
            base.SetPageColorsInternal(foreColor, backColor, linkColor, hoverColor);
        }

        public void SetPageFont(string fontFamily, float fontSize)
        {
            base.SetPageFontInternal(fontFamily, fontSize);
        }

        public void SetPageMargins(BoxEdges margins)
        {
            base.SetPageMarginsInternal(margins);
        }

        public void SetPageWatermark(Watermark watermark)
        {
            base.SetPageWatermarkInternal(watermark);
        }
    }
}

