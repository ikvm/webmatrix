namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Drawing;

    public abstract class PageFactory
    {
        private PageCreationData _data;

        protected PageFactory()
        {
        }

        protected void AddElementInternal(Element element, string name)
        {
            this.EnsurePageCreationData();
            this._data.AddElement(element, name);
        }

        protected SectionElement BeginNewSectionInternal(bool allowWrap, int indentation, string name)
        {
            this.EnsurePageCreationData();
            return this._data.BeginNewSection(allowWrap, indentation, name);
        }

        protected void ClearInternal()
        {
            this._data = null;
        }

        public Page CreatePage()
        {
            Page newPage = new Page();
            this.InitializePage(newPage);
            return newPage;
        }

        public Page CreatePage(Type pageType)
        {
            if (pageType == null)
            {
                throw new ArgumentNullException("pageType");
            }
            if (!typeof(Page).IsAssignableFrom(pageType))
            {
                throw new ArgumentException("Specified type must be a derived page type");
            }
            Page newPage = (Page) Activator.CreateInstance(pageType);
            this.InitializePage(newPage);
            return newPage;
        }

        protected void EndCurrentSectionInternal()
        {
            this.EnsurePageCreationData();
            this._data.EndCurrentSection();
        }

        private void EnsurePageCreationData()
        {
            if (this._data == null)
            {
                this._data = new PageCreationData();
            }
        }

        protected virtual void InitializePage(Page newPage)
        {
            if (this._data != null)
            {
                newPage.Initialize(this._data);
                this._data = null;
            }
        }

        protected void SetPageAntiAliasedTextInternal(bool antiAliasedText)
        {
            this.EnsurePageCreationData();
            this._data.SetAntiAliasedText(antiAliasedText);
        }

        protected void SetPageColorsInternal(Color foreColor, Color backColor, Color linkColor, Color hoverColor)
        {
            this.EnsurePageCreationData();
            this._data.SetColors(foreColor, backColor, linkColor, hoverColor);
        }

        protected void SetPageFontInternal(string fontFamily, float fontSize)
        {
            this.EnsurePageCreationData();
            this._data.SetFont(fontFamily, fontSize);
        }

        protected void SetPageMarginsInternal(BoxEdges margins)
        {
            this.EnsurePageCreationData();
            this._data.SetMargins(margins);
        }

        protected void SetPageWatermarkInternal(Watermark watermark)
        {
            this.EnsurePageCreationData();
            this._data.SetWatermark(watermark);
        }
    }
}

