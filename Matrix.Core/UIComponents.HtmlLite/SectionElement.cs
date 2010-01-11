namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;

    public sealed class SectionElement : Element
    {
        private bool _allowWrap = true;
        private int _indentation;
        private Microsoft.Matrix.UIComponents.HtmlLite.RenderedElements _renderedElements;

        protected override ElementCollection CreateElementCollection()
        {
            return new VisualElementCollection(this);
        }

        protected internal override void OnChanged(bool requiresLayoutUpdate)
        {
            if (requiresLayoutUpdate)
            {
                this._renderedElements = null;
            }
            base.OnChanged(requiresLayoutUpdate);
        }

        public bool AllowWrap
        {
            get
            {
                return this._allowWrap;
            }
            set
            {
                if (this._allowWrap != value)
                {
                    this._allowWrap = value;
                    this.OnChanged(true);
                }
            }
        }

        public VisualElementCollection Elements
        {
            get
            {
                return (VisualElementCollection) base.Elements;
            }
        }

        public int Indentation
        {
            get
            {
                return this._indentation;
            }
            set
            {
                if (this._indentation != value)
                {
                    this._indentation = value;
                    this.OnChanged(true);
                }
            }
        }

        internal Microsoft.Matrix.UIComponents.HtmlLite.RenderedElements RenderedElements
        {
            get
            {
                if (this._renderedElements == null)
                {
                    this._renderedElements = new Microsoft.Matrix.UIComponents.HtmlLite.RenderedElements();
                    if (this.Elements.Count != 0)
                    {
                        foreach (VisualElement element in this.Elements)
                        {
                            element.AddRenderedElementsToList(this._renderedElements);
                        }
                    }
                }
                return this._renderedElements;
            }
        }
    }
}

