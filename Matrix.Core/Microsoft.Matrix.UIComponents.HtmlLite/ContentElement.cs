namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Collections;
    using System.Drawing;

    public abstract class ContentElement : Element
    {
        private IContentElementHost _host;
        private BoxEdges _padding;

        protected ContentElement()
        {
        }

        protected override ElementCollection CreateElementCollection()
        {
            return new SectionElementCollection(this);
        }

        public virtual Size CreateLayout(ElementRenderData renderData, Point offset, Size initialMaxSize)
        {
            int num = this._padding.Top + offset.Y;
            int num2 = this._padding.Left + offset.X;
            int width = initialMaxSize.Width;
            int y = num;
            int count = this.Elements.Count;
            int num6 = 0;
            for (int i = 0; i < count; i++)
            {
                SectionElement element = this.Elements[i];
                if (element.Visible)
                {
                    bool flag = false;
                    int height = 0;
                    int indentation = element.Indentation;
                    int x = num2 + indentation;
                    RenderedElements renderedElements = element.RenderedElements;
                    int num11 = renderedElements.Count;
                    if (num11 != 0)
                    {
                        ArrayList list = new ArrayList();
                        int num12 = -1;
                        bool flag2 = true;
                        for (int j = 0; j < num11; j++)
                        {
                            VisualElement element2 = renderedElements[j];
                            element2.SetBounds(Rectangle.Empty);
                            if (element2.Visible)
                            {
                                renderData.SetFontFamily(element2.GetFontFamily());
                                renderData.SetFontSize(element2.GetFontSize());
                                renderData.SetFontStyle(element2.GetFontStyle());
                                Size size = element2.GetSize(renderData);
                                if (!flag2 && ((element2.LayoutMode & LayoutMode.LineBreakBefore) != LayoutMode.Flow))
                                {
                                    x = num2 + indentation;
                                    y += height;
                                    height = 0;
                                    flag2 = true;
                                }
                                if (flag2)
                                {
                                    if (num12 != -1)
                                    {
                                        list.Add(num12);
                                    }
                                    num12 = j;
                                }
                                if (element2.SizeMode == SizeMode.StretchHorizontal)
                                {
                                    if (!flag2 && (size.Height != height))
                                    {
                                        flag = true;
                                    }
                                    if (size.Height > height)
                                    {
                                        height = size.Height;
                                    }
                                    Point location = new Point(x, y);
                                    Size size2 = new Size((width - x) - this._padding.Right, height);
                                    element2.SetBounds(new Rectangle(location, size2));
                                }
                                else
                                {
                                    if ((x + size.Width) > (width - this._padding.Right))
                                    {
                                        if (element.AllowWrap && !flag2)
                                        {
                                            x = num2 + indentation;
                                            y += height;
                                            height = 0;
                                            list.Add(num12);
                                            flag2 = true;
                                        }
                                        else
                                        {
                                            y = num;
                                            width = (x + size.Width) + this._padding.Right;
                                            height = 0;
                                            if (this._host != null)
                                            {
                                                this._host.ClearTrackedElements();
                                            }
                                            i = -1;
                                            break;
                                        }
                                    }
                                    if (flag2 && element2.IsEmpty)
                                    {
                                        goto Label_02BD;
                                    }
                                    element2.SetBounds(new Rectangle(new Point(x, y), size));
                                    if (!flag2 && (size.Height != height))
                                    {
                                        flag = true;
                                    }
                                    x += size.Width;
                                    if (size.Height > height)
                                    {
                                        height = size.Height;
                                    }
                                    flag2 = false;
                                }
                                if ((this._host != null) && (element2.RequiresTracking || element2.Clickable))
                                {
                                    this._host.AddTrackedElement(element2);
                                }
                                if ((element2.LayoutMode & LayoutMode.LineBreakAfter) != LayoutMode.Flow)
                                {
                                    x = num2 + indentation;
                                    y += height;
                                    height = 0;
                                    flag2 = true;
                                }
                            Label_02BD:;
                            }
                        }
                        list.Add(num11);
                        if (flag)
                        {
                            int num14 = -1;
                            for (int k = 0; k < list.Count; k++)
                            {
                                int num16 = num14 + 1;
                                num14 = (int) list[k];
                                bool flag3 = false;
                                int num17 = 0;
                                bool flag4 = true;
                                for (int m = num16; m < num14; m++)
                                {
                                    Rectangle bounds = renderedElements[m].Bounds;
                                    if (!bounds.IsEmpty)
                                    {
                                        int num19 = bounds.Height;
                                        if (!flag4 && (num19 != num17))
                                        {
                                            flag3 = true;
                                        }
                                        if (num19 > num17)
                                        {
                                            num17 = num19;
                                        }
                                        flag4 = false;
                                    }
                                }
                                if (flag3)
                                {
                                    for (int n = num16; n < num14; n++)
                                    {
                                        VisualElement element3 = renderedElements[n];
                                        Rectangle rectangle2 = element3.Bounds;
                                        if (!rectangle2.IsEmpty)
                                        {
                                            int num21 = ((num17 - rectangle2.Height) + 1) / 2;
                                            if (num21 != 0)
                                            {
                                                element3.SetBounds(new Rectangle(rectangle2.Left, rectangle2.Top + num21, rectangle2.Width, rectangle2.Height));
                                                element3.SetOuterBounds(new Rectangle(rectangle2.Left, rectangle2.Top, rectangle2.Width, num17));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (num6 == 0)
                        {
                            Font font = null;
                            if (this._host != null)
                            {
                                font = this._host.Font;
                            }
                            if (font != null)
                            {
                                num6 = renderData.Graphics.MeasureString(" ", font).ToSize().Height;
                            }
                            else
                            {
                                num6 = 10;
                            }
                        }
                        height = num6;
                    }
                    y += height;
                }
            }
            return new Size(width, y + this._padding.Bottom);
        }

        public virtual void CreateRendering(ElementRenderData renderData, Point offset, Point canvasExtent)
        {
            foreach (SectionElement element in this.Elements)
            {
                if (element.Visible)
                {
                    RenderedElements renderedElements = element.RenderedElements;
                    int count = renderedElements.Count;
                    if (renderedElements.Count != 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            VisualElement element2 = renderedElements[i];
                            Rectangle bounds = element2.Bounds;
                            if (!bounds.IsEmpty)
                            {
                                bounds.Offset(offset);
                                if ((bounds.Bottom >= 0) || (bounds.Top <= canvasExtent.Y))
                                {
                                    renderData.SetFontFamily(element2.GetFontFamily());
                                    renderData.SetFontSize(element2.GetFontSize());
                                    renderData.SetFontStyle(element2.GetFontStyle());
                                    renderData.SetBackColor(element2.GetBackColor());
                                    renderData.SetForeColor(element2.GetForeColor());
                                    element2.Render(renderData);
                                }
                            }
                        }
                    }
                }
            }
        }

        internal void SetHost(IContentElementHost host)
        {
            this._host = host;
        }

        public SectionElementCollection Elements
        {
            get
            {
                return (SectionElementCollection) base.Elements;
            }
        }

        public override string FontFamily
        {
            get
            {
                string fontFamily = base.FontFamily;
                if ((fontFamily.Length == 0) && (this._host != null))
                {
                    Font font = this._host.Font;
                    if (font != null)
                    {
                        fontFamily = font.FontFamily.Name;
                    }
                }
                return fontFamily;
            }
            set
            {
                base.FontFamily = value;
            }
        }

        public override float FontSize
        {
            get
            {
                float fontSize = base.FontSize;
                if ((fontSize == 0f) && (this._host != null))
                {
                    Font font = this._host.Font;
                    if (font != null)
                    {
                        fontSize = font.Size;
                    }
                }
                return fontSize;
            }
            set
            {
                base.FontSize = value;
            }
        }

        public override Color ForeColor
        {
            get
            {
                Color foreColor = base.ForeColor;
                if (foreColor.IsEmpty && (this._host != null))
                {
                    foreColor = this._host.ForeColor;
                }
                return foreColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        protected BoxEdges PaddingInternal
        {
            get
            {
                return this._padding;
            }
            set
            {
                if (this._padding != value)
                {
                    this._padding = value;
                    this.OnChanged(true);
                }
            }
        }
    }
}

