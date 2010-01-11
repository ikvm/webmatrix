namespace Microsoft.Matrix.Packages.Community.UserInterface
{
    using Microsoft.Matrix.UIComponents.HtmlLite;
    using System;
    using System.Drawing;

    public abstract class CommunityTab : IDisposable
    {
        private EventHandler _displayChangedHandler;
        private Page _page;
        private IServiceProvider _serviceProvider;
        private static Image watermarkImage;

        public event EventHandler DisplayChanged
        {
            add
            {
                this._displayChangedHandler = (EventHandler) Delegate.Combine(this._displayChangedHandler, value);
            }
            remove
            {
                if (this._displayChangedHandler != null)
                {
                    this._displayChangedHandler = (EventHandler) Delegate.Remove(this._displayChangedHandler, value);
                }
            }
        }

        protected CommunityTab()
        {
        }

        protected virtual void Dispose()
        {
            this._serviceProvider = null;
        }

        protected virtual Page GenerateDisplayPage()
        {
            CommunityPageBuilder builder = new CommunityPageBuilder();
            this.GenerateDisplayPage(builder);
            CommunityPage page = (CommunityPage) builder.CreatePage(typeof(CommunityPage));
            page.SetOwner(this);
            return page;
        }

        protected virtual void GenerateDisplayPage(CommunityPageBuilder builder)
        {
            builder.SetPageColors(Color.Black, Color.White, Color.Blue, Color.Red);
            builder.SetPageMargins(new BoxEdges(4, 4, 4, 4));
            builder.SetPageWatermark(new Watermark(WatermarkImage, WatermarkPlacement.BottomRight));
            builder.SetPageFont("Tahoma", 8f);
        }

        internal Page GetPage(bool recreate)
        {
            if (recreate || (this._page == null))
            {
                this._page = this.GenerateDisplayPage();
            }
            return this._page;
        }

        protected object GetService(Type serviceType)
        {
            if (this._serviceProvider != null)
            {
                return this._serviceProvider.GetService(serviceType);
            }
            return null;
        }

        public virtual void Initialize(IServiceProvider serviceProvider, string initializationData)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            this._serviceProvider = serviceProvider;
        }

        protected virtual void OnDisplayChanged(EventArgs e)
        {
            this._page = null;
            if (this._displayChangedHandler != null)
            {
                this._displayChangedHandler(this, e);
            }
        }

        protected virtual void OnLinkClicked(object userData)
        {
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        public abstract Image Glyph { get; }

        public abstract string Name { get; }

        public virtual int RefreshRate
        {
            get
            {
                return 0;
            }
        }

        protected IServiceProvider ServiceProvider
        {
            get
            {
                return this._serviceProvider;
            }
        }

        public virtual bool SupportsRefresh
        {
            get
            {
                return false;
            }
        }

        private static Image WatermarkImage
        {
            get
            {
                if (watermarkImage == null)
                {
                    watermarkImage = new Bitmap(typeof(CommunityTab), "Background.jpg");
                }
                return watermarkImage;
            }
        }

        private sealed class CommunityPage : Page
        {
            private CommunityTab _owner;

            protected override bool OnElementClick(ElementEventArgs e)
            {
                HyperLinkElement element = e.Element as HyperLinkElement;
                if (element != null)
                {
                    this._owner.OnLinkClicked(element.UserData);
                }
                return false;
            }

            public void SetOwner(CommunityTab owner)
            {
                this._owner = owner;
            }
        }

        protected class CommunityPageBuilder : PageBuilder
        {
            public void AddHeading(string heading, string subHeading)
            {
                base.BeginNewSection(false);
                base.PushFontSize(10f);
                base.PushBold();
                base.PushForeColor(Color.Navy);
                base.AddText(heading);
                base.PopForeColor();
                base.PopBold();
                base.PopFontSize();
                base.EndCurrentSection();
                if ((subHeading != null) && (subHeading.Length != 0))
                {
                    base.BeginNewSection(false);
                    base.PushFontFamily("Verdana");
                    base.PushItalic();
                    base.AddText(subHeading);
                    base.PopItalic();
                    base.PopFontFamily();
                    base.EndCurrentSection();
                }
            }

            public void AddHorizontalLine()
            {
                base.BeginNewSection();
                base.PushForeColor(Color.LightSkyBlue);
                base.AddDivider();
                base.PopForeColor();
                base.EndCurrentSection();
            }

            public void AddHyperLinkWithGlyph(string text, object data, string toolTip, Image glyph)
            {
                base.BeginNewSection(false);
                if (glyph != null)
                {
                    base.AddImage(glyph).Padding = new BoxEdges(0, 1, 4, 1);
                }
                HyperLinkElement element2 = base.AddHyperLink(text, data);
                element2.FontStyle = FontStyle.Regular;
                if ((toolTip != null) && (toolTip.Length != 0))
                {
                    element2.ToolTip = toolTip;
                }
                base.EndCurrentSection();
            }

            public void AddSectionBreak()
            {
                base.BeginNewSection();
                base.AddDivider(0);
                base.EndCurrentSection();
            }
        }
    }
}

