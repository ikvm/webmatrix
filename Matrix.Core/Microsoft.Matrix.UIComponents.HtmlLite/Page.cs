namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Drawing;

    public class Page : ContentElement, INameScopeElement
    {
        private bool _antiAliasedText;
        private int _batchCount;
        private bool _batchLayoutChanged;
        private bool _changed;
        private PageChangedEventHandler _changedHandler;
        private Color _hoverColor;
        private Color _linkColor;
        private Microsoft.Matrix.UIComponents.HtmlLite.Watermark _watermark;

        public event PageChangedEventHandler Changed
        {
            add
            {
                this._changedHandler = (PageChangedEventHandler) Delegate.Combine(this._changedHandler, value);
            }
            remove
            {
                if (this._changedHandler != null)
                {
                    this._changedHandler = (PageChangedEventHandler) Delegate.Remove(this._changedHandler, value);
                }
            }
        }

        public Page()
        {
            this.ForeColor = SystemColors.WindowText;
            this.BackColor = SystemColors.Window;
            this.LinkColor = SystemColors.Highlight;
            this.HoverColor = SystemColors.HotTrack;
            this.Margins = new BoxEdges(4, 4, 4, 4);
        }

        public void BeginBatchUpdate()
        {
            this._batchCount++;
        }

        public void EndBatchUpdate()
        {
            if (this._batchCount > 0)
            {
                this._batchCount--;
                if ((this._batchCount == 0) && this._changed)
                {
                    bool requiresLayoutUpdate = this._batchLayoutChanged;
                    this._changed = false;
                    this._batchLayoutChanged = false;
                    this.OnChanged(requiresLayoutUpdate);
                }
            }
        }

        internal void Initialize(PageCreationData data)
        {
            ElementCollection elements = base.Elements;
            foreach (SectionElement element in data.Sections)
            {
                elements.Add(element);
            }
            this.ForeColor = data.ForeColor;
            this.BackColor = data.BackColor;
            this.LinkColor = data.LinkColor;
            this.HoverColor = data.HoverColor;
            this.FontFamily = data.FontFamily;
            this.FontSize = data.FontSize;
            this.Watermark = data.Watermark;
            this.Margins = data.Margins;
            this.AntiAliasedText = data.AntiAliasedText;
        }

        protected virtual void OnChanged(PageChangedEventArgs e)
        {
            if (this._changedHandler != null)
            {
                this._changedHandler(this, e);
            }
        }

        protected internal override void OnChanged(bool requiresLayoutUpdate)
        {
            if (this._batchCount == 0)
            {
                this.OnChanged(new PageChangedEventArgs(requiresLayoutUpdate));
            }
            else
            {
                this._changed = true;
                this._batchLayoutChanged |= requiresLayoutUpdate;
            }
        }

        protected virtual bool OnElementClick(ElementEventArgs e)
        {
            return true;
        }

        internal bool RaiseElementClickEvent(ElementEventArgs e)
        {
            return this.OnElementClick(e);
        }

        public bool AntiAliasedText
        {
            get
            {
                return this._antiAliasedText;
            }
            set
            {
                if (this._antiAliasedText != value)
                {
                    this._antiAliasedText = value;
                    this.OnChanged(true);
                }
            }
        }

        public Color HoverColor
        {
            get
            {
                return this._hoverColor;
            }
            set
            {
                this._hoverColor = value;
            }
        }

        public Color LinkColor
        {
            get
            {
                return this._linkColor;
            }
            set
            {
                if (this._linkColor != value)
                {
                    this._linkColor = value;
                    this.OnChanged(false);
                }
            }
        }

        public BoxEdges Margins
        {
            get
            {
                return base.PaddingInternal;
            }
            set
            {
                base.PaddingInternal = value;
            }
        }

        public Microsoft.Matrix.UIComponents.HtmlLite.Watermark Watermark
        {
            get
            {
                return this._watermark;
            }
            set
            {
                this._watermark = value;
                this.OnChanged(false);
            }
        }
    }
}

