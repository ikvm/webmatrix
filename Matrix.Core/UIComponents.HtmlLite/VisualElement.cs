namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;

    public abstract class VisualElement : Element
    {
        private Rectangle _bounds;
        private Rectangle _contentBounds;
        private bool _focus;
        private Size _size;
        private bool _sizeCalculated;

        protected VisualElement()
        {
        }

        protected internal virtual void AddRenderedElementsToList(RenderedElements elements)
        {
            elements.Add(this);
        }

        public virtual void BeginTracking()
        {
            if (base.LogicalElement != null)
            {
                base.LogicalElement.BeginTracking();
            }
        }

        public virtual void EndTracking()
        {
            if (base.LogicalElement != null)
            {
                base.LogicalElement.EndTracking();
            }
        }

        internal Size GetSize(ElementRenderData renderData)
        {
            if (!this._sizeCalculated)
            {
                this._size = this.Measure(renderData);
                this._sizeCalculated = true;
            }
            return this._size;
        }

        protected abstract Size Measure(ElementRenderData renderData);
        protected internal override void OnChanged(bool requiresLayoutUpdate)
        {
            if (requiresLayoutUpdate)
            {
                this._sizeCalculated = false;
            }
            base.OnChanged(requiresLayoutUpdate);
        }

        public void Render(ElementRenderData renderData)
        {
            this.RenderBackground(renderData);
            this.RenderForeground(renderData);
        }

        protected virtual void RenderBackground(ElementRenderData renderData)
        {
            Brush backColorBrush = renderData.BackColorBrush;
            if (backColorBrush != null)
            {
                renderData.Graphics.FillRectangle(backColorBrush, this.Bounds);
            }
        }

        internal void RenderFocusCues(ElementRenderData renderData)
        {
            Rectangle focusBounds = this.FocusBounds;
            if (!focusBounds.IsEmpty)
            {
                focusBounds.Offset(renderData.ScrollPosition.X, renderData.ScrollPosition.Y);
                Interop.RECT r = new Interop.RECT(focusBounds.Left, focusBounds.Top, focusBounds.Right, focusBounds.Bottom);
                Interop.DrawFocusRect(renderData.GraphicsHandle, ref r);
            }
        }

        protected virtual void RenderForeground(ElementRenderData renderData)
        {
        }

        internal void SetBounds(Rectangle bounds)
        {
            this._bounds = bounds;
            this._contentBounds = bounds;
        }

        internal void SetFocus(bool focus)
        {
            this._focus = focus;
        }

        internal void SetOuterBounds(Rectangle bounds)
        {
            this._bounds = bounds;
        }

        public Rectangle Bounds
        {
            get
            {
                return this._bounds;
            }
        }

        public virtual bool Clickable
        {
            get
            {
                return false;
            }
        }

        protected bool ContainsFocus
        {
            get
            {
                return this._focus;
            }
        }

        public virtual Rectangle ContentBounds
        {
            get
            {
                return this._contentBounds;
            }
        }

        protected virtual Rectangle FocusBounds
        {
            get
            {
                Rectangle contentBounds = this.ContentBounds;
                contentBounds.Inflate(1, 1);
                return contentBounds;
            }
        }

        public virtual bool IsEmpty
        {
            get
            {
                return false;
            }
        }

        public virtual Microsoft.Matrix.UIComponents.HtmlLite.LayoutMode LayoutMode
        {
            get
            {
                return Microsoft.Matrix.UIComponents.HtmlLite.LayoutMode.Flow;
            }
        }

        public virtual bool RequiresTracking
        {
            get
            {
                if (!this.Clickable)
                {
                    return (base.ToolTip.Length != 0);
                }
                return true;
            }
        }

        public virtual Microsoft.Matrix.UIComponents.HtmlLite.SizeMode SizeMode
        {
            get
            {
                return Microsoft.Matrix.UIComponents.HtmlLite.SizeMode.Exact;
            }
        }
    }
}

