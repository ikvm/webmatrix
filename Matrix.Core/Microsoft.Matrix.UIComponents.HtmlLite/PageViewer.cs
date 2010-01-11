namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using Microsoft.Matrix;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Windows.Forms;

    public class PageViewer : ScrollableControl, IContentElementHost
    {
        private ArrayList _focusableElements;
        private int _focusIndex;
        private VisualElement _hotElement;
        private Microsoft.Matrix.UIComponents.HtmlLite.Page _page;
        private bool _suppressFocusUpdate;
        private RegionToolTip _toolTip;
        private int _toolTipRegionCount;
        private ArrayList _trackedElements;
        private static readonly object ElementClickEvent = new object();

        public event ElementEventHandler ElementClick
        {
            add
            {
                base.Events.AddHandler(ElementClickEvent, value);
            }
            remove
            {
                base.Events.RemoveHandler(ElementClickEvent, value);
            }
        }

        public PageViewer()
        {
            base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserMouse | ControlStyles.Selectable | ControlStyles.Opaque | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ContainerControl, false);
            this._toolTip = new RegionToolTip(this);
            this._focusIndex = -1;
        }

        private Size CalculateRendering(Graphics g)
        {
            if ((this._page == null) || (this._page.Elements.Count == 0))
            {
                return Size.Empty;
            }
            if (this._page.AntiAliasedText)
            {
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }
            ElementRenderData renderData = new ElementRenderData(g, Point.Empty, false, this._page.AntiAliasedText);
            Size empty = Size.Empty;
            try
            {
                empty = this._page.CreateLayout(renderData, Point.Empty, new Size(base.ClientSize.Width, 0));
            }
            finally
            {
                if (renderData != null)
                {
                    renderData.Dispose();
                    renderData = null;
                }
            }
            return empty;
        }

        public void EnsureVisible(VisualElement element)
        {
            Point autoScrollPosition = base.AutoScrollPosition;
            Rectangle contentBounds = element.ContentBounds;
            Rectangle clientRectangle = base.ClientRectangle;
            contentBounds.Inflate(2, 2);
            clientRectangle.Offset(-autoScrollPosition.X, -autoScrollPosition.Y);
            if (!clientRectangle.Contains(contentBounds))
            {
                Point location = contentBounds.Location;
                clientRectangle = base.ClientRectangle;
                clientRectangle.Offset(location.X, -autoScrollPosition.Y);
                if (clientRectangle.Contains(contentBounds))
                {
                    location = new Point(location.X, -autoScrollPosition.Y);
                }
                base.AutoScrollPosition = location;
            }
        }

        private bool FocusNextElement(bool forward)
        {
            bool flag = false;
            bool flag2 = false;
            int num = -1;
            if (forward)
            {
                if (this._focusIndex < (this._focusableElements.Count - 1))
                {
                    num = this._focusIndex + 1;
                }
            }
            else if (this._focusIndex > 0)
            {
                num = this._focusIndex - 1;
            }
            if (this._focusIndex != -1)
            {
                ((VisualElement) this._focusableElements[this._focusIndex]).SetFocus(false);
                this._focusIndex = -1;
                flag2 = true;
            }
            if (num != -1)
            {
                this._focusIndex = num;
                VisualElement element = (VisualElement) this._focusableElements[this._focusIndex];
                element.SetFocus(true);
                this.EnsureVisible(element);
                flag2 = true;
                flag = true;
            }
            if (flag2)
            {
                base.Invalidate();
            }
            return flag;
        }

        private void GenerateRendering(Graphics g)
        {
            Rectangle clientRectangle = base.ClientRectangle;
            Brush brush = new SolidBrush(this._page.BackColor);
            g.FillRectangle(brush, clientRectangle);
            brush.Dispose();
            Watermark watermark = this._page.Watermark;
            if (watermark.Image != null)
            {
                Image image = watermark.Image;
                Size size = image.Size;
                Point empty = Point.Empty;
                switch (watermark.Placement)
                {
                    case WatermarkPlacement.TopRight:
                        empty = new Point(clientRectangle.Width - size.Width, 0);
                        break;

                    case WatermarkPlacement.BottomLeft:
                        empty = new Point(0, clientRectangle.Height - size.Height);
                        break;

                    case WatermarkPlacement.BottomRight:
                        empty = new Point(clientRectangle.Width - size.Width, clientRectangle.Height - size.Height);
                        break;

                    case WatermarkPlacement.Center:
                        empty = new Point((clientRectangle.Width - size.Width) / 2, (clientRectangle.Height - size.Height) / 2);
                        break;
                }
                g.DrawImageUnscaled(image, empty);
            }
            Point autoScrollPosition = base.AutoScrollPosition;
            ElementRenderData renderData = null;
            g.TranslateTransform((float) autoScrollPosition.X, (float) autoScrollPosition.Y);
            try
            {
                if (this._page.AntiAliasedText)
                {
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                }
                renderData = new ElementRenderData(g, autoScrollPosition, base.ContainsFocus, this._page.AntiAliasedText);
                renderData.SetLinkColor(this._page.LinkColor);
                if (renderData.LinkColor.IsEmpty)
                {
                    renderData.SetLinkColor(this.ForeColor);
                }
                renderData.SetHoverColor(this._page.HoverColor);
                if (renderData.HoverColor.IsEmpty)
                {
                    renderData.SetHoverColor(this._page.LinkColor);
                }
                if (renderData.HoverColor.IsEmpty)
                {
                    renderData.SetHoverColor(this.ForeColor);
                }
                this._page.CreateRendering(renderData, autoScrollPosition, new Point(0, base.ClientSize.Height));
                if (renderData.ShowFocus && (this._focusIndex != -1))
                {
                    VisualElement element = (VisualElement) this._focusableElements[this._focusIndex];
                    Rectangle bounds = element.Bounds;
                    if (!bounds.IsEmpty)
                    {
                        bounds.Offset(autoScrollPosition);
                        if ((bounds.Bottom >= 0) && (bounds.Top <= base.ClientSize.Height))
                        {
                            element.RenderFocusCues(renderData);
                        }
                    }
                }
            }
            finally
            {
                if (renderData != null)
                {
                    renderData.Dispose();
                    renderData = null;
                }
                g.TranslateTransform((float) -autoScrollPosition.X, (float) -autoScrollPosition.Y);
            }
        }

        void IContentElementHost.AddTrackedElement(VisualElement element)
        {
            if (element.RequiresTracking)
            {
                if (this._trackedElements == null)
                {
                    this._trackedElements = new ArrayList();
                }
                this._trackedElements.Add(element);
            }
            if (element.Clickable)
            {
                if (this._focusableElements == null)
                {
                    this._focusableElements = new ArrayList();
                }
                this._focusableElements.Add(element);
            }
        }

        void IContentElementHost.ClearTrackedElements()
        {
            this._trackedElements = null;
            this._focusableElements = null;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Element logicalElement = null;
            if (this._trackedElements != null)
            {
                Point autoScrollPosition = base.AutoScrollPosition;
                Point point2 = base.PointToClient(Control.MousePosition);
                point2.Offset(-autoScrollPosition.X, -autoScrollPosition.Y);
                foreach (VisualElement element2 in this._trackedElements)
                {
                    if (element2.ContentBounds.Contains(point2.X, point2.Y))
                    {
                        logicalElement = element2;
                        if (logicalElement.LogicalElement != null)
                        {
                            logicalElement = logicalElement.LogicalElement;
                        }
                        break;
                    }
                }
            }
            bool flag = false;
            if (this._focusIndex != -1)
            {
                ((VisualElement) this._focusableElements[this._focusIndex]).SetFocus(false);
                this._focusIndex = -1;
                flag = true;
            }
            if (logicalElement != null)
            {
                int index = -1;
                if (this._focusableElements != null)
                {
                    index = this._focusableElements.IndexOf(logicalElement);
                }
                if (index != -1)
                {
                    this._focusIndex = index;
                    ((VisualElement) this._focusableElements[this._focusIndex]).SetFocus(true);
                    flag = true;
                }
                this.RaiseElementClickEvent(logicalElement);
            }
            if (flag)
            {
                base.Invalidate();
            }
        }

        protected virtual void OnElementClick(ElementEventArgs e)
        {
            ElementEventHandler handler = (ElementEventHandler) base.Events[ElementClickEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.UpdateRendering(true);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (this._focusableElements != null)
            {
                if (this._focusIndex != -1)
                {
                    ((VisualElement) this._focusableElements[this._focusIndex]).SetFocus(false);
                    this._focusIndex = -1;
                }
                if (!this._suppressFocusUpdate)
                {
                    this.FocusNextElement(true);
                }
            }
            this._suppressFocusUpdate = false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.UpdateRendering(true);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if ((this._page != null) && !e.Handled)
            {
                bool flag = false;
                if (e.KeyCode == Keys.Tab)
                {
                    bool forward = !e.Shift;
                    flag = this.FocusNextElement(forward);
                }
                else if (e.KeyCode == Keys.Return)
                {
                    if (this._focusIndex != -1)
                    {
                        VisualElement element = (VisualElement) this._focusableElements[this._focusIndex];
                        this.RaiseElementClickEvent(element);
                    }
                }
                else
                {
                    int num = 0;
                    switch (e.KeyCode)
                    {
                        case Keys.Prior:
                            num = 2;
                            flag = true;
                            break;

                        case Keys.Next:
                            num = 3;
                            flag = true;
                            break;

                        case Keys.End:
                            if (e.Control)
                            {
                                num = 7;
                                flag = true;
                            }
                            break;

                        case Keys.Home:
                            if (e.Control)
                            {
                                num = 6;
                                flag = true;
                            }
                            break;

                        case Keys.Up:
                            num = 0;
                            flag = true;
                            break;

                        case Keys.Down:
                            num = 1;
                            flag = true;
                            break;
                    }
                    if (flag)
                    {
                        Interop.SendMessage(base.Handle, 0x115, (IntPtr) num, IntPtr.Zero);
                    }
                }
                e.Handled = flag;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            bool flag = false;
            if (this._focusIndex != -1)
            {
                ((VisualElement) this._focusableElements[this._focusIndex]).SetFocus(false);
                this._focusIndex = -1;
                flag = true;
            }
            if (!this.UpdateTrackingElement(null) && flag)
            {
                base.Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.UpdateTrackingElement(null);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            VisualElement element = null;
            if (this._trackedElements != null)
            {
                Point autoScrollPosition = base.AutoScrollPosition;
                Point point2 = new Point(e.X - autoScrollPosition.X, e.Y - autoScrollPosition.Y);
                foreach (VisualElement element2 in this._trackedElements)
                {
                    if (element2.ContentBounds.Contains(point2.X, point2.Y))
                    {
                        element = element2;
                        break;
                    }
                }
            }
            this.UpdateTrackingElement(element);
        }

        private void OnPageChanged(object sender, PageChangedEventArgs e)
        {
            if (e.RequiresLayoutUpdate)
            {
                this.UpdateRendering(true);
            }
            else
            {
                base.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this._page != null)
            {
                this.GenerateRendering(e.Graphics);
            }
            else
            {
                e.Graphics.FillRectangle(SystemBrushes.Window, base.ClientRectangle);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.UpdateRendering(true);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch ((keyData & Keys.KeyCode))
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Tab:
                {
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    this.OnKeyDown(e);
                    if (e.Handled)
                    {
                        return true;
                    }
                    break;
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        private void RaiseElementClickEvent(Element element)
        {
            ElementEventArgs e = new ElementEventArgs(element);
            if (this._page.RaiseElementClickEvent(e))
            {
                this.OnElementClick(e);
            }
        }

        private void UpdateRendering(bool maintainLocality)
        {
            Point autoScrollPosition = base.AutoScrollPosition;
            int num = this._focusIndex;
            if (num != -1)
            {
                ((VisualElement) this._focusableElements[num]).SetFocus(false);
            }
            if (this._hotElement != null)
            {
                this._hotElement.EndTracking();
                this._hotElement = null;
            }
            this._trackedElements = null;
            this._focusIndex = -1;
            this._focusableElements = null;
            if (base.IsHandleCreated)
            {
                Size empty = Size.Empty;
                if (this._page != null)
                {
                    using (Graphics graphics = base.CreateGraphics())
                    {
                        empty = this.CalculateRendering(graphics);
                    }
                }
                base.AutoScrollMinSize = empty;
                if (maintainLocality)
                {
                    if (((num != -1) && (this._focusableElements != null)) && (this._focusableElements.Count > num))
                    {
                        this._focusIndex = num;
                        ((VisualElement) this._focusableElements[this._focusIndex]).SetFocus(true);
                    }
                    base.AutoScrollPosition = new Point(-autoScrollPosition.X, -autoScrollPosition.Y);
                }
                this.UpdateToolTip();
                base.Invalidate();
            }
        }

        private void UpdateToolTip()
        {
            if (this._toolTipRegionCount != 0)
            {
                for (int i = 0; i < this._toolTipRegionCount; i++)
                {
                    this._toolTip.RemoveToolTipRegion(i);
                }
                this._toolTipRegionCount = 0;
            }
            if ((this._trackedElements != null) && (this._trackedElements.Count != 0))
            {
                Point autoScrollPosition = base.AutoScrollPosition;
                int height = base.ClientSize.Height;
                foreach (VisualElement element in this._trackedElements)
                {
                    string toolTip = element.ToolTip;
                    if (toolTip.Length != 0)
                    {
                        Rectangle contentBounds = element.ContentBounds;
                        contentBounds.Offset(autoScrollPosition);
                        if ((contentBounds.Bottom >= 0) && (contentBounds.Top <= height))
                        {
                            this._toolTip.AddToolTipRegion(toolTip, this._toolTipRegionCount, contentBounds);
                            this._toolTipRegionCount++;
                        }
                    }
                }
            }
        }

        private bool UpdateTrackingElement(VisualElement element)
        {
            if (element == this._hotElement)
            {
                return false;
            }
            if (this._hotElement != null)
            {
                this._hotElement.EndTracking();
                this._hotElement = null;
            }
            Cursor hand = Cursors.Default;
            this._hotElement = element;
            if (this._hotElement != null)
            {
                this._hotElement.BeginTracking();
                if (this._hotElement.Clickable)
                {
                    hand = Cursors.Hand;
                }
            }
            this.Cursor = hand;
            base.Invalidate();
            return true;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x201)
            {
                this._suppressFocusUpdate = true;
            }
            base.WndProc(ref m);
            if ((m.Msg == 0x114) || (m.Msg == 0x115))
            {
                base.Invalidate();
                this.UpdateToolTip();
            }
        }

        Color IContentElementHost.BackColor
        {
            get
            {
                return this.BackColor;
            }
        }

        Font IContentElementHost.Font
        {
            get
            {
                return this.Font;
            }
        }

        Color IContentElementHost.ForeColor
        {
            get
            {
                return this.ForeColor;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Microsoft.Matrix.UIComponents.HtmlLite.Page Page
        {
            get
            {
                return this._page;
            }
            set
            {
                if (this._page != value)
                {
                    if (this._page != null)
                    {
                        this._page.Changed -= new PageChangedEventHandler(this.OnPageChanged);
                        this._page.SetHost(null);
                    }
                    this._page = value;
                    if (this._page != null)
                    {
                        this._page.SetHost(this);
                        this._page.Changed += new PageChangedEventHandler(this.OnPageChanged);
                    }
                    this.UpdateRendering(false);
                }
            }
        }
    }
}

