namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class TabControl : Panel
    {
        private TabControlMode _effectiveMode;
        private System.Windows.Forms.ImageList _imageList;
        private TabControlMode _mode;
        private Microsoft.Matrix.UIComponents.TabPlacement _placement;
        private int _selectedIndex;
        private TabPage _selectedTabPage;
        private TabPageCollection _tabCollection;
        private int _tabHeight = -1;
        private bool _tabRectanglesCalculated;
        private RegionToolTip _toolTip;
        private static readonly object EventSelectedIndexChanged = new object();
        private static readonly object EventSelectedIndexChanging = new object();
        private const int TabBorder = 1;
        private const int TabImage = 0x10;
        private const int TabPaddingX = 4;
        private const int TabPaddingY = 2;
        private const int TabSeparator = 1;
        private const int TabWellGap = 4;
        private const int TabWellPaddingX = 5;
        private const int TabWellPaddingY = 2;

        public event EventHandler SelectedIndexChanged
        {
            add
            {
                base.Events.AddHandler(EventSelectedIndexChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventSelectedIndexChanged, value);
            }
        }

        public event CancelEventHandler SelectedIndexChanging
        {
            add
            {
                base.Events.AddHandler(EventSelectedIndexChanging, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventSelectedIndexChanging, value);
            }
        }

        public TabControl()
        {
            base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.Opaque | ControlStyles.UserPaint, true);
            this._tabCollection = new TabPageCollection(this);
            this._selectedIndex = -1;
            this._selectedTabPage = null;
            this._toolTip = new RegionToolTip(this);
            this._placement = Microsoft.Matrix.UIComponents.TabPlacement.Top;
            this._mode = TabControlMode.TextAndImage;
            this._effectiveMode = TabControlMode.TextAndImage;
            this._tabRectanglesCalculated = false;
        }

        protected override Control.ControlCollection CreateControlsInstance()
        {
            return new ControlCollection(this);
        }

        private void DirtyTabRectangles(bool repaintNow)
        {
            this._tabRectanglesCalculated = false;
            if (repaintNow)
            {
                base.Invalidate();
            }
        }

        private void EnsureTabRectangles()
        {
            this.EnsureTabRectangles(null, null, Rectangle.Empty);
        }

        // UNDONE: 修改TabPage的Rectangle值使TabControl支持TabPlacement.Left和TabPlacement.Left
        private void EnsureTabRectangles(Graphics g, Font font, Rectangle clientRect)
        {
            #region if calculated
            if (this._tabRectanglesCalculated)
                return;

            if (this._tabCollection.Count < 2)
            {
                this._tabRectanglesCalculated = true;
                return;
            }
            #endregion

            #region Remove Tooltip Region
            if (this._toolTip.IsToolTipCreated)
            {
                foreach (TabPage page in this._tabCollection)
                {
                    if (page.ToolTipID != -1)
                    {
                        this._toolTip.RemoveToolTipRegion(page.ToolTipID);
                        page.ToolTipID = -1;
                    }
                }
            }
            #endregion

            #region Prepare clientRect, grephics and font etc
            this._effectiveMode = this._mode;
            if (clientRect.IsEmpty)
            {
                clientRect = base.ClientRectangle;
            }
            if (font == null)
            {
                font = this.Font;
            }

            bool graphicsCreated = false;
            if (g == null)
            {
                g = base.CreateGraphics();
                graphicsCreated = true;
            }
            #endregion

            #region calc tabs width and save in an array
            int[] tabWidthArray = new int[this._tabCollection.Count];
            int totalTabWidth = 10;
            int minTabWidth = 0x7fffffff;
            int index = 0;
            int rectWidth;

            StringFormat verticalFormat = new StringFormat( StringFormatFlags.DirectionVertical);
            foreach (TabPage page2 in this._tabCollection)
            {
                rectWidth = 11;
                if (this._mode != TabControlMode.TextOnly)
                {
                    rectWidth += 0x10;
                }
                if (this._mode != TabControlMode.ImageOnly)
                {
                    SizeF ef;
                    rectWidth += 4;
                    if (this._placement == TabPlacement.Top || this._placement == TabPlacement.Bottom)
                    {
                        ef = g.MeasureString(page2.Text, font);
                        rectWidth += (int) ef.Width;
                    }
                    else
                    {
                        ef = g.MeasureString(page2.Text, font, font.Height, verticalFormat);
                        rectWidth += (int)ef.Height;
                    }
                }

                minTabWidth = Math.Min(minTabWidth, rectWidth);

                totalTabWidth += rectWidth;
                tabWidthArray[index++] = rectWidth;
            }
            #endregion

            #region tab总宽度大于tabControl宽度时做相应调整
            bool tabWidthAdjusted = false;
            int availableClientWidth;
            if (this._placement == TabPlacement.Top || this._placement == TabPlacement.Bottom)
                availableClientWidth = clientRect.Width - 10;
            else
                availableClientWidth = clientRect.Height - 10;

            if ((availableClientWidth > 0) && (totalTabWidth > availableClientWidth))
            {
                int averageTabWidth = availableClientWidth / this._tabCollection.Count;
                bool textOnly = false;
                tabWidthAdjusted = true;
                if (this._mode == TabControlMode.TextOnly)
                {
                    textOnly = true;
                }
                else
                {
                    int imageOnlyWidth = 0x1b;
                    if (averageTabWidth < imageOnlyWidth)
                    {
                        this._effectiveMode = TabControlMode.TextOnly;
                        for (int i = 0; i < tabWidthArray.Length; i++)
                        {
                            tabWidthArray[i] = averageTabWidth;
                        }
                    }
                    else if (Math.Abs((int) (averageTabWidth - imageOnlyWidth)) < 10)
                    {
                        this._effectiveMode = TabControlMode.ImageOnly;
                        for (int j = 0; j < tabWidthArray.Length; j++)
                        {
                            tabWidthArray[j] = imageOnlyWidth;
                        }
                    }
                    else
                    {
                        textOnly = true;
                    }
                }

                if (textOnly)
                {
                    int originAverageTabWidth = averageTabWidth;
                    int originAvailableClientWidth = availableClientWidth;
                    int num12 = 0;
                    for (int i = 0; i < tabWidthArray.Length; i++)
                    {
                        if (tabWidthArray[i] <= averageTabWidth)
                        {
                            originAvailableClientWidth -= averageTabWidth;
                            num12++;
                        }
                    }
                    if (this._tabCollection.Count == num12)
                    {
                        averageTabWidth = originAvailableClientWidth / this._tabCollection.Count;
                    }
                    else
                    {
                        averageTabWidth = originAvailableClientWidth / (this._tabCollection.Count - num12);
                    }
                    for (int m = 0; m < tabWidthArray.Length; m++)
                    {
                        if (tabWidthArray[m] > originAverageTabWidth)
                        {
                            tabWidthArray[m] = averageTabWidth;
                        }
                    }
                }
            }
            #endregion //tab总宽度大于tabControl宽度时做相应调整


            int rectX = 0;
            int rectY = 0;
            if (this._placement == TabPlacement.Top)
            {
                rectX = 5;
                rectY = 2;
            }
            else if (this._placement == TabPlacement.Bottom)
            {
                rectX = 5;
                rectY = clientRect.Height - this.TabWellHeight;
            }
            else if (this._placement == TabPlacement.Left)
            {
                rectY = 5;
                rectX = clientRect.X + 2;
            }
            else if (this._placement == TabPlacement.Right)
            {
                rectY = 5;
                rectX = clientRect.Width - this.TabWellHeight;
            }

            index = 0;
            foreach (TabPage page3 in this._tabCollection)
            {
                rectWidth = tabWidthArray[index];

                //added to support vertical layout
                if (this._placement == TabPlacement.Top || this._placement == TabPlacement.Bottom)
                {
                    page3.Rectangle = new Rectangle(rectX, rectY, rectWidth, this.TabHeight);
                    rectX += rectWidth;
                }
                else
                {
                    page3.Rectangle = new Rectangle(rectX, rectY, this.TabHeight, rectWidth);
                    rectY += rectWidth;
                }

                if (base.IsHandleCreated)
                {
                    string toolTipText = page3.ToolTipText;
                    if (((toolTipText.Length == 0) && tabWidthAdjusted) && (this._mode != TabControlMode.ImageOnly))
                    {
                        toolTipText = page3.Text;
                    }
                    if (toolTipText.Length != 0)
                    {
                        this._toolTip.AddToolTipRegion(toolTipText, index, page3.Rectangle);
                        page3.ToolTipID = index;
                    }
                }

                index++;
            }

            if (graphicsCreated)
            {
                g.Dispose();
            }
            this._tabRectanglesCalculated = true;
        }

        public TabPage GetPageAt(Point pt)
        {
            int num;
            return this.GetPageAt(pt, out num);
        }

        private TabPage GetPageAt(Point pt, out int index)
        {
            this.EnsureTabRectangles();
            index = -1;
            int num = 0;
            foreach (TabPage page in this._tabCollection)
            {
                if (page.Rectangle.Contains(pt))
                {
                    index = num;
                    return page;
                }
                num++;
            }
            return null;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData & Keys.Alt) != Keys.Alt)
            {
                switch ((keyData & Keys.KeyCode))
                {
                    case Keys.Prior:
                    case Keys.Next:
                        return true;
                }
            }
            return false;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (this._mode != TabControlMode.ImageOnly)
            {
                this._tabHeight = -1;
                this.UpdateLayout();
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            this.DirtyTabRectangles(false);
            base.OnHandleDestroyed(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!e.Handled && e.Control)
            {
                int newIndex = -1;
                if (e.KeyCode == Keys.Prior)
                {
                    e.Handled = true;
                    newIndex = this._selectedIndex - 1;
                    if (newIndex < 0)
                    {
                        newIndex = this._tabCollection.Count - 1;
                    }
                }
                else if (e.KeyCode == Keys.Next)
                {
                    e.Handled = true;
                    newIndex = this._selectedIndex + 1;
                    if (newIndex == this._tabCollection.Count)
                    {
                        newIndex = 0;
                    }
                }
                if ((newIndex != -1) && (newIndex != this._selectedIndex))
                {
                    this.OnTabSelected(newIndex, true);
                }
            }
        }


        // UNDONE: 修改渲染过程使TabControl支持TabPlacement.Left和TabPlacement.Left
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clientRectangle = base.ClientRectangle;
            Graphics g = e.Graphics;
            if (this._tabCollection.Count < 2)
            {
                if (this._tabCollection.Count < 1)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Control, 0, 0, clientRectangle.Width, clientRectangle.Height);
                }
            }
            else
            {
                int rectX;
                int rectY;
                Font font = this.Font;
                this.EnsureTabRectangles(g, font, clientRectangle);
                g.FillRectangle(SystemBrushes.Control, 0, 0, clientRectangle.Width, clientRectangle.Height);
                Brush brush = new SolidBrush(Color.FromArgb(0x40, SystemColors.ControlDark));
                Pen controlDark = SystemPens.ControlDark;
                Pen controlLightLight = SystemPens.ControlLightLight;
                //填充tab区域
                if (this._placement == TabPlacement.Top)
                {
                    rectX = 0;
                    rectY = 0;
                    g.FillRectangle(brush, 0, 0, clientRectangle.Width, this.TabWellHeight);
                    g.DrawLine(SystemPens.ControlLightLight, this.TabWellHeight - 1, this.TabWellHeight - 1, clientRectangle.Width, this.TabWellHeight - 1);
                }
                else if (this._placement == TabPlacement.Bottom)
                {
                    rectY = clientRectangle.Height - this.TabWellHeight;
                    g.FillRectangle(brush, 0, rectY, clientRectangle.Width, this.TabWellHeight);
                    g.DrawLine(controlDark, 0, rectY, clientRectangle.Width, rectY);
                }
                else if (this._placement == TabPlacement.Left)
                {
                    rectY = 0;
                    g.FillRectangle(brush, 0, 0, this.TabWellHeight, clientRectangle.Height);
                    g.DrawLine(controlLightLight, this.TabWellHeight - 1, 0, this.TabWellHeight - 1, clientRectangle.Height);
                }
                else
                {
                    rectY = 0;
                    g.FillRectangle(brush, clientRectangle.Width - this.TabWellHeight, 0, this.TabWellHeight, clientRectangle.Height);
                    g.DrawLine(controlLightLight, clientRectangle.Width - this.TabWellHeight - 1, 0, this.TabWellHeight - 1, clientRectangle.Height);
                }

                brush.Dispose();

                
                #region 逐个绘制tab
                int tabIndex = 0;
                StringFormat format = new StringFormat();
                if (this._effectiveMode != TabControlMode.ImageOnly)
                {
                    format.FormatFlags |= StringFormatFlags.NoWrap;
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    format.Trimming = StringTrimming.EllipsisCharacter;
                }

                if (this._placement == TabPlacement.Left || _placement == TabPlacement.Right)
                    format.FormatFlags |= StringFormatFlags.DirectionVertical;

                foreach (TabPage page in this._tabCollection)
                {
                    Rectangle rectangle = page.Rectangle;
                    Brush controlText = null;
                    if (tabIndex == this._selectedIndex)
                    {
                        g.FillRectangle(SystemBrushes.Control, rectangle);

                        if(this._placement == TabPlacement.Top)
                        {
                            g.DrawLine(SystemPens.ControlLightLight, rectangle.Left, rectangle.Top, rectangle.Left, rectangle.Bottom - 1);
                            g.DrawLine(SystemPens.ControlLightLight, rectangle.Left + 1, rectangle.Top, rectangle.Right, rectangle.Top);
                            g.DrawLine(SystemPens.ControlDark, rectangle.Right, rectangle.Bottom - 2, rectangle.Right, rectangle.Top + 1);
                        }
                        else if (this._placement == Microsoft.Matrix.UIComponents.TabPlacement.Bottom)
                        {
                            g.DrawLine(SystemPens.ControlLightLight, rectangle.Left, rectangle.Top, rectangle.Left, rectangle.Bottom - 1);
                            g.DrawLine(SystemPens.ControlDark, rectangle.Left + 1, rectangle.Bottom - 1, rectangle.Right, rectangle.Bottom - 1);
                            g.DrawLine(SystemPens.ControlDark, rectangle.Right, rectangle.Bottom - 1, rectangle.Right, rectangle.Top);
                        }
                        else if (this._placement == TabPlacement.Left)
                        {
                            g.DrawLine(SystemPens.ControlLightLight, rectangle.Left, rectangle.Top, rectangle.Right - 1, rectangle.Top);
                            g.DrawLine(SystemPens.ControlLightLight, rectangle.Left, rectangle.Top + 1, rectangle.Left, rectangle.Bottom - 1);
                            g.DrawLine(SystemPens.ControlDark, rectangle.Left + 1, rectangle.Bottom, rectangle.Right - 2, rectangle.Bottom);
                        }
                        else
                        {
                            g.DrawLine(SystemPens.ControlLightLight, rectangle.Left, rectangle.Top, rectangle.Right - 1, rectangle.Top);
                            g.DrawLine(SystemPens.ControlDark, rectangle.Right - 1, rectangle.Top, rectangle.Right - 1, rectangle.Bottom);
                            g.DrawLine(SystemPens.ControlDark, rectangle.Left, rectangle.Bottom, rectangle.Right - 1, rectangle.Bottom);
                        }

                        if (this._effectiveMode != TabControlMode.ImageOnly)
                        {
                            controlText = SystemBrushes.ControlText;
                        }
                    }
                    else
                    {
                        if (this._placement == TabPlacement.Top)
                        {
                            g.DrawLine(SystemPens.ControlDark, rectangle.Right, (rectangle.Bottom - 1) - 3, rectangle.Right, (rectangle.Top + 1) + 1);
                        }
                        else if (this._placement == Microsoft.Matrix.UIComponents.TabPlacement.Bottom)
                        {
                            g.DrawLine(SystemPens.ControlDark, rectangle.Right, (rectangle.Bottom - 1) - 1, rectangle.Right, (rectangle.Top + 1) + 2);
                        }
                        else if (this._placement == TabPlacement.Left)
                        {
                            g.DrawLine(SystemPens.ControlDark, rectangle.Left + 1, rectangle.Top, rectangle.Right - 3, rectangle.Top);
                        }
                        else
                        {
                            g.DrawLine(SystemPens.ControlDark, rectangle.Left + 3, rectangle.Top, rectangle.Right - 1, rectangle.Top);
                        }

                        if (this._effectiveMode != TabControlMode.ImageOnly)
                        {
                            controlText = SystemBrushes.ControlText;
                        }
                    }

                    #region 绘制tab上的图标和文字
                    int paddingLeft = 5;
                    int paddingTop = 5;

                    if (this._placement == TabPlacement.Top || _placement == TabPlacement.Bottom)
                    {
                        if (this._effectiveMode != TabControlMode.TextOnly)
                        {
                            if ((this._imageList != null) && (page.ImageIndex != -1))
                            {
                                int x = rectangle.Left + paddingLeft;
                                int y = rectangle.Top + ((rectangle.Height - 0x10) / 2);
                                this._imageList.Draw(g, x, y, page.ImageIndex);
                            }
                            paddingLeft += 20;
                        }
                        if (controlText != null)
                        {
                            int x = rectangle.Left + paddingLeft;
                            int y = (rectangle.Top + 1) + 2;
                            int width = rectangle.Right - x;
                            int height = rectangle.Height - 6;

                            Rectangle layoutRectangle = new Rectangle(x, y, width, height);
                            g.DrawString(page.Text, font, controlText, layoutRectangle, format);
                        }
                    }
                    else
                    { 
                        if (this._effectiveMode != TabControlMode.TextOnly)
                        {
                            if ((this._imageList != null) && (page.ImageIndex != -1))
                            {
                                int x = rectangle.Left + ((rectangle.Width - 0x10) / 2);
                                int y = rectangle.Top + paddingTop; 
                                this._imageList.Draw(g, x, y, page.ImageIndex);
                            }
                            paddingTop += 20;
                        }
                        if (controlText != null)
                        {
                            int x = rectangle.Left;
                            int y = rectangle.Top + paddingTop;
                            int width = rectangle.Width;
                            int height = rectangle.Height - paddingTop;

                            Rectangle layoutRectangle = new Rectangle(x, y, width, height);
                            g.DrawString(page.Text, font, controlText, layoutRectangle, format);
                        }
                    }
                    #endregion

                    tabIndex++;
                }
                #endregion

                if (format != null)
                {
                    format.Dispose();
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this._tabRectanglesCalculated = false;
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[EventSelectedIndexChanged];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSelectedIndexChanging(CancelEventArgs e)
        {
            CancelEventHandler handler = (CancelEventHandler) base.Events[EventSelectedIndexChanging];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnTabAdded(TabPage page)
        {
            if (this._selectedIndex == -1)
            {
                this.SelectedIndex = 0;
            }
        }

        internal void OnTabChanged(TabPage page, bool relayoutRequired)
        {
            if (relayoutRequired)
            {
                this.DirtyTabRectangles(base.IsHandleCreated);
            }
            else if (base.IsHandleCreated)
            {
                base.Invalidate();
            }
        }

        private void OnTabRemoved(TabPage page)
        {
            if (page == this._selectedTabPage)
            {
                int selectedIndex = this.SelectedIndex;
                this.SelectedIndex = -1;
                if (selectedIndex == this._tabCollection.Count)
                {
                    selectedIndex--;
                }
                this.SelectedIndex = selectedIndex;
            }
        }

        private void OnTabSelected(int newIndex, bool uiSelection)
        {
            TabPage page = this._selectedTabPage;
            if (this._selectedTabPage != null)
            {
                if (uiSelection)
                {
                    CancelEventArgs e = new CancelEventArgs();
                    this.OnSelectedIndexChanging(e);
                    if (e.Cancel)
                    {
                        return;
                    }
                }
                this._selectedTabPage = null;
            }
            this._selectedIndex = newIndex;
            if (this._selectedIndex != -1)
            {
                this._selectedTabPage = this._tabCollection[this._selectedIndex];
                this._selectedTabPage.Visible = true;
            }
            if (uiSelection)
            {
                this.OnSelectedIndexChanged(EventArgs.Empty);
            }
            if (page != null)
            {
                page.Visible = false;
            }
            this.DirtyTabRectangles(base.IsHandleCreated);
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            return (this.ProcessKeyEventArgs(ref m) || base.ProcessKeyPreview(ref m));
        }

        private void UpdateLayout()
        {
            this.DirtyTabRectangles(false);
            base.PerformLayout();
            if (base.IsHandleCreated)
            {
                base.Invalidate();
            }
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                // UNDONE: 修改返回的DisplayRectangle使TabControl支持TabPlacement.Left和TabPlacement.Left
                Rectangle clientRectangle = base.ClientRectangle;
                if (this._tabCollection.Count < 2)
                {
                    return clientRectangle;
                }
                int y = this.TabWellHeight + 4;
                if (this._placement == TabPlacement.Top)
                {
                    return new Rectangle(0, y, clientRectangle.Width, clientRectangle.Height - y);
                }
                else if (this._placement == TabPlacement.Bottom)
                {
                    return new Rectangle(0, 0, clientRectangle.Width, clientRectangle.Height - y);
                }
                else if (this._placement == TabPlacement.Left)
                {
                    return new Rectangle(this.TabWellHeight + 4, 0, clientRectangle.Width - this.TabWellHeight - 4, clientRectangle.Height);
                }
                else
                {
                    return new Rectangle(0, 0, clientRectangle.Width - this.TabWellHeight - 4, clientRectangle.Height);
                }
            }
        }

        public System.Windows.Forms.ImageList ImageList
        {
            get
            {
                return this._imageList;
            }
            set
            {
                if (this._imageList != value)
                {
                    this._imageList = value;
                    this.DirtyTabRectangles(base.IsHandleCreated);
                }
            }
        }

        public TabControlMode Mode
        {
            get
            {
                return this._mode;
            }
            set
            {
                if (this._mode != value)
                {
                    this._mode = value;
                    this._effectiveMode = value;
                    this._tabHeight = -1;
                    this.DirtyTabRectangles(base.IsHandleCreated);
                }
            }
        }

        public virtual int SelectedIndex
        {
            get
            {
                return this._selectedIndex;
            }
            set
            {
                if (this._selectedIndex != value)
                {
                    if (((value == -1) && (this._tabCollection.Count > 0)) || ((value < 0) || (value >= this._tabCollection.Count)))
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    this.OnTabSelected(value, false);
                }
            }
        }

        private int TabHeight
        {
            get
            {
                if (this._tabHeight == -1)
                {
                    if (this._mode == TabControlMode.ImageOnly)
                    {
                        this._tabHeight = 0x10;
                    }
                    else
                    {
                        this._tabHeight = Math.Max(this.Font.Height, 0x10);
                    }
                    this._tabHeight += 6;
                }
                return this._tabHeight;
            }
        }

        public Microsoft.Matrix.UIComponents.TabPlacement TabPlacement
        {
            get
            {
                return this._placement;
            }
            set
            {
                if (this._placement != value)
                {
                    this._placement = value;
                    this.UpdateLayout();
                }
            }
        }

        public TabPageCollection Tabs
        {
            get
            {
                return this._tabCollection;
            }
        }

        private int TabWellHeight
        {
            get
            {
                return (this.TabHeight + 2);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int num;
            base.OnMouseDown(e);

            this.GetPageAt(new Point(e.X, e.Y), out num);
            if ((num != -1) && (num != this._selectedIndex))
            {
                this.OnTabSelected(num, true);
            }
        }

        /*
        protected void OnBeginDrag(TabPageDragEventArg e)
        {
            DataObject dataObject = new DataObject(this.DragAndDropTabControlDataFormat, e.TabPage);
            DoDragDrop(dataObject, DragDropEffects.Move);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this._isMouseDown = false;
            this._mouseLocation = Point.Empty;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this._isMouseDown = false;
            this._mouseLocation = Point.Empty;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this._isMouseDown)
            {
                if (Math.Sqrt(Math.Pow(e.Location.X - this._mouseLocation.X, 2) + Math.Pow(e.Location.Y - this._mouseLocation.Y, 2)) > 5)
                {
                     OnBeginDrag(new TabPageDragEventArg(this._selectedTabPage));
                }
            }
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);
            drgevent.Effect = DragDropEffects.Move;
            return;
            if (drgevent.Data.GetDataPresent(this.DragAndDropTabControlDataFormat))
                drgevent.Effect = DragDropEffects.Move;
        }


        protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
        {
            base.OnGiveFeedback(gfbevent);
            gfbevent.UseDefaultCursors = true;
        }

        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs qcdevent)
        {
            base.OnQueryContinueDrag(qcdevent);
            if (qcdevent.EscapePressed)
                qcdevent.Action = DragAction.Cancel;
            else if (!this._isMouseDown)
                qcdevent.Action = DragAction.Drop;
            else
                qcdevent.Action = DragAction.Continue;
        }
        */

        public class ControlCollection : Control.ControlCollection
        {
            private TabControl _owner;

            internal ControlCollection(TabControl owner)
                : base(owner)
            {
                this._owner = owner;
            }

            public override void Add(Control value)
            {
                TabPage page = value as TabPage;
                if (page == null)
                {
                    throw new ArgumentException();
                }
                page.Visible = false;
                page.Dock = DockStyle.Fill;
                this._owner.Tabs.Add(page);
                base.Add(page);
                this._owner.OnTabAdded(page);
            }

            public override void Remove(Control value)
            {
                TabPage page = value as TabPage;
                if (page != null)
                {
                    base.Remove(page);
                    this._owner.Tabs.Remove(page);
                    this._owner.OnTabRemoved(page);
                }
            }
        }

        public sealed class TabPageCollection : IList, ICollection, IEnumerable
        {
            private ArrayList _orderedTabs;
            private TabControl _owner;

            internal TabPageCollection(TabControl owner)
            {
                this._owner = owner;
                this._orderedTabs = new ArrayList();
            }

            public void Add(TabPage value)
            {
                if (!this._orderedTabs.Contains(value))
                {
                    this._orderedTabs.Add(value);
                    this._owner.Controls.Add(value);
                }
            }

            public void AddRange(TabPage[] pages)
            {
                foreach (TabPage page in pages)
                {
                    this.Add(page);
                }
            }

            public void Clear()
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    this.RemoveAt(0);
                }
            }

            public bool Contains(TabPage page)
            {
                return (this.IndexOf(page) != -1);
            }

            public void CopyTo(Array dest, int index)
            {
                if (this.Count > 0)
                {
                    this._orderedTabs.CopyTo(dest, index);
                }
            }

            public IEnumerator GetEnumerator()
            {
                if (this.Count != 0)
                {
                    return this._orderedTabs.GetEnumerator();
                }
                return new TabPage[0].GetEnumerator();
            }

            public int IndexOf(TabPage page)
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this[i] == page)
                    {
                        return i;
                    }
                }
                return -1;
            }

            public void Remove(TabPage value)
            {
                if (this._orderedTabs.Contains(value))
                {
                    this._owner.Controls.Remove(value);
                    this._orderedTabs.Remove(value);
                }
            }

            public void RemoveAt(int index)
            {
                TabPage page = (TabPage) this._orderedTabs[index];
                this._orderedTabs.RemoveAt(index);
                this._owner.Controls.Remove(page);
            }

            int IList.Add(object value)
            {
                TabPage page = value as TabPage;
                if (page == null)
                {
                    throw new ArgumentException();
                }
                this.Add(page);
                return this.IndexOf(page);
            }

            bool IList.Contains(object value)
            {
                TabPage page = value as TabPage;
                return ((page != null) && this.Contains(page));
            }

            int IList.IndexOf(object value)
            {
                TabPage page = value as TabPage;
                if (page != null)
                {
                    return this.IndexOf(page);
                }
                return -1;
            }

            void IList.Insert(int index, object value)
            {
                throw new NotSupportedException();
            }

            void IList.Remove(object value)
            {
                TabPage page = value as TabPage;
                if (page != null)
                {
                    this.Remove(page);
                }
            }

            public int Count
            {
                get
                {
                    return this._orderedTabs.Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public bool IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            public TabPage this[int index]
            {
                get
                {
                    return (TabPage) this._orderedTabs[index];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            public object SyncRoot
            {
                get
                {
                    return this;
                }
            }

            bool IList.IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }
        }

        public class TabPageDragEventArg : EventArgs
        {
            public TabPage TabPage;
            public TabPageDragEventArg(TabPage page)
            {
                this.TabPage = page;
            }
        }

    }
}

