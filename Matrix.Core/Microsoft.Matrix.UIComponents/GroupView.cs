namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class GroupView : Panel
    {
        private bool _allowDrag;
        private IntPtr _dragCursor;
        private GroupViewItem _dragItem;
        private int _hoverIndex;
        private GroupViewListView _itemContainer;
        private DragDropEffects _lastDragEffect;
        private bool _mouseDown;
        private int _mouseDownIndex;
        private GroupViewSectionCollection _sectionCollection;
        private int _sectionHeight;
        private ImageList _sectionImages;
        private GroupViewSection _selectedSection;
        private int _selectedSectionIndex;
        private RegionToolTip _toolTip;
        private static readonly object EventGroupViewItemClicked = new object();
        private static readonly object EventSelectedItemChanged = new object();
        private static readonly object EventSelectedSectionChanged = new object();
        private static readonly object EventSelectedSectionChanging = new object();
        private const int ImagePadding = 1;
        private const int MinItemContainerHeight = 10;
        private const int SectionGap = 2;
        private const int SectionImage = 0x10;
        private const int SectionPadding = 2;
        private bool sectionRectanglesCalculated;

        public event GroupViewItemEventHandler ItemClicked
        {
            add
            {
                base.Events.AddHandler(EventGroupViewItemClicked, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventGroupViewItemClicked, value);
            }
        }

        public event EventHandler SelectedItemChanged
        {
            add
            {
                base.Events.AddHandler(EventSelectedItemChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventSelectedItemChanged, value);
            }
        }

        public event EventHandler SelectedSectionChanged
        {
            add
            {
                base.Events.AddHandler(EventSelectedSectionChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventSelectedSectionChanged, value);
            }
        }

        public event EventHandler SelectedSectionChanging
        {
            add
            {
                base.Events.AddHandler(EventSelectedSectionChanging, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventSelectedSectionChanging, value);
            }
        }

        public GroupView() : this(new GroupViewListView())
        {
        }

        public GroupView(GroupViewListView itemContainer)
        {
            this._sectionHeight = -1;
            if (itemContainer == null)
            {
                throw new ArgumentNullException();
            }
            base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.Opaque | ControlStyles.UserPaint, true);
            this._sectionCollection = new GroupViewSectionCollection(this);
            this._selectedSectionIndex = -1;
            this._hoverIndex = -1;
            this._mouseDownIndex = -1;
            this._itemContainer = itemContainer;
            this._itemContainer.SelectedIndexChanged += new EventHandler(this.OnGroupViewItemSelectionChanged);
            this._itemContainer.Click += new EventHandler(this.OnGroupViewClicked);
            base.Controls.Add(this._itemContainer);
            this._toolTip = new RegionToolTip(this);
        }

        public void BeginUpdate()
        {
            this._itemContainer.BeginUpdate();
        }

        protected override Control.ControlCollection CreateControlsInstance()
        {
            return new ControlCollection(this);
        }

        private void DirtySectionRectangles(bool repaintNow)
        {
            this.sectionRectanglesCalculated = false;
            if (repaintNow)
            {
                base.Invalidate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this._dragCursor != IntPtr.Zero)
            {
                Interop.DestroyCursor(this._dragCursor);
                this._dragCursor = IntPtr.Zero;
            }
            base.Dispose(disposing);
        }

        public void EndUpdate()
        {
            this._itemContainer.EndUpdate();
        }

        private void EnsureSectionRectangles()
        {
            Graphics g = base.CreateGraphics();
            this.EnsureSectionRectangles(g, base.ClientRectangle);
            g.Dispose();
        }

        private void EnsureSectionRectangles(Graphics g, Rectangle clientRect)
        {
            if (!this.sectionRectanglesCalculated)
            {
                if (this._sectionCollection.Count == 0)
                {
                    this.sectionRectanglesCalculated = true;
                }
                else
                {
                    if (this._toolTip.IsToolTipCreated)
                    {
                        foreach (GroupViewSection section in this._sectionCollection)
                        {
                            if (section.ToolTipID != -1)
                            {
                                this._toolTip.RemoveToolTipRegion(section.ToolTipID);
                                section.ToolTipID = -1;
                            }
                        }
                    }
                    int sectionHeight = this.SectionHeight;
                    if (clientRect.Height < this.MinimumHeight)
                    {
                        this.sectionRectanglesCalculated = true;
                    }
                    else
                    {
                        Font font = this.Font;
                        int y = 0;
                        int regionID = 0;
                        while (regionID <= this._selectedSectionIndex)
                        {
                            GroupViewSection section2 = this._sectionCollection[regionID];
                            if (!section2.Visible)
                            {
                                section2.Rectangle = Rectangle.Empty;
                            }
                            else
                            {
                                section2.Rectangle = new Rectangle(0, y, clientRect.Width, sectionHeight);
                                if (base.IsHandleCreated)
                                {
                                    this._toolTip.AddToolTipRegion(section2.Text, regionID, section2.Rectangle);
                                    section2.ToolTipID = regionID;
                                }
                                y += sectionHeight + 2;
                            }
                            regionID++;
                        }
                        y = clientRect.Height - sectionHeight;
                        for (regionID = this._sectionCollection.Count - 1; regionID > this._selectedSectionIndex; regionID--)
                        {
                            GroupViewSection section3 = this._sectionCollection[regionID];
                            if (!section3.Visible)
                            {
                                section3.Rectangle = Rectangle.Empty;
                            }
                            else
                            {
                                section3.Rectangle = new Rectangle(0, y, clientRect.Width, sectionHeight);
                                if (base.IsHandleCreated)
                                {
                                    this._toolTip.AddToolTipRegion(section3.Text, regionID, section3.Rectangle);
                                    section3.ToolTipID = regionID;
                                }
                                y -= sectionHeight + 2;
                            }
                        }
                        this.sectionRectanglesCalculated = true;
                    }
                }
            }
        }

        private int GetNextVisibleSection(int sectionIndex)
        {
            int num2;
            int num = -1;
            for (num2 = sectionIndex + 1; num2 < this._sectionCollection.Count; num2++)
            {
                GroupViewSection section = this._sectionCollection[num2];
                if (section.Visible)
                {
                    num = num2;
                    break;
                }
            }
            if (num == -1)
            {
                for (num2 = 0; num2 <= sectionIndex; num2++)
                {
                    GroupViewSection section2 = this._sectionCollection[num2];
                    if (section2.Visible)
                    {
                        return num2;
                    }
                }
            }
            return num;
        }

        private int GetPreviousVisibleSection(int sectionIndex)
        {
            int num2;
            if ((sectionIndex < 0) || (sectionIndex > this._sectionCollection.Count))
            {
                return sectionIndex;
            }
            int num = -1;
            for (num2 = sectionIndex - 1; num2 >= 0; num2--)
            {
                GroupViewSection section = this._sectionCollection[num2];
                if (section.Visible)
                {
                    num = num2;
                    break;
                }
            }
            if (num == -1)
            {
                for (num2 = this._sectionCollection.Count - 1; num2 >= sectionIndex; num2--)
                {
                    GroupViewSection section2 = this._sectionCollection[num2];
                    if (section2.Visible)
                    {
                        return num2;
                    }
                }
            }
            return num;
        }

        public GroupViewSection GetSectionAt(Point pt)
        {
            int num;
            return this.GetSectionAt(pt, out num);
        }

        private GroupViewSection GetSectionAt(Point pt, out int index)
        {
            this.EnsureSectionRectangles();
            index = -1;
            int num = 0;
            foreach (GroupViewSection section in this._sectionCollection)
            {
                if (section.Rectangle.Contains(pt))
                {
                    index = num;
                    return section;
                }
                num++;
            }
            return null;
        }

        private int GetVisibleSectionCount()
        {
            int num = 0;
            for (int i = 0; i < this._sectionCollection.Count; i++)
            {
                GroupViewSection section = this._sectionCollection[i];
                if (section.Visible)
                {
                    num++;
                }
            }
            return num;
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
            this._sectionHeight = -1;
            this.UpdateLayout();
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            if ((this._allowDrag && (this._dragItem != null)) && ((e.Effect != this._lastDragEffect) || e.UseDefaultCursors))
            {
                this._lastDragEffect = e.Effect;
                if (this._dragCursor != IntPtr.Zero)
                {
                    Interop.DestroyCursor(this._dragCursor);
                    this._dragCursor = IntPtr.Zero;
                }
                e.UseDefaultCursors = false;
                Bitmap image = new Bitmap(0x20, 0x20);
                Graphics graphics = Graphics.FromImage(image);
                graphics.FillRectangle(Brushes.Transparent, 9, 9, 12, 12);
                if (e.Effect == DragDropEffects.None)
                {
                    Pen pen = new Pen(Color.Black, 1.5f);
                    graphics.DrawEllipse(pen, 9, 9, 12, 12);
                    graphics.DrawLine(pen, 12, 12, 0x12, 0x12);
                }
                else
                {
                    graphics.DrawRectangle(Pens.Black, 9, 9, 12, 12);
                    graphics.DrawLine(Pens.Silver, 11, 15, 0x13, 15);
                    graphics.DrawLine(Pens.Silver, 15, 11, 15, 0x13);
                }
                graphics.DrawImage(this._dragItem.Image, 0x10, 0x10);
                graphics.Dispose();
                this._dragCursor = image.GetHicon();
                Interop.SetCursor(this._dragCursor);
            }
        }

        private void OnGroupViewClicked(object sender, EventArgs e)
        {
            GroupViewItem selectedItem = this.SelectedItem;
            if (selectedItem != null)
            {
                this.OnItemClicked(new GroupViewItemEventArgs(selectedItem));
            }
        }

        private void OnGroupViewItemDrag(object sender, ItemDragEventArgs e)
        {
            this._dragItem = null;
            if (this._allowDrag)
            {
                GroupViewItem groupViewItem = ((GroupViewListViewItem) e.Item).GroupViewItem;
                DataObject dragDropDataObject = groupViewItem.GetDragDropDataObject();
                if (dragDropDataObject != null)
                {
                    this._dragItem = groupViewItem;
                    if (this._dragCursor != IntPtr.Zero)
                    {
                        Interop.DestroyCursor(this._dragCursor);
                        this._dragCursor = IntPtr.Zero;
                    }
                    this._lastDragEffect = DragDropEffects.None;
                    base.DoDragDrop(dragDropDataObject, DragDropEffects.Move | DragDropEffects.Copy);
                }
            }
        }

        private void OnGroupViewItemSelectionChanged(object sender, EventArgs e)
        {
            this.OnSelectedItemChanged(EventArgs.Empty);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            this.DirtySectionRectangles(false);
            base.OnHandleDestroyed(e);
        }

        protected virtual void OnItemClicked(GroupViewItemEventArgs e)
        {
            GroupViewItemEventHandler handler = (GroupViewItemEventHandler) base.Events[EventGroupViewItemClicked];
            if (handler != null)
            {
                handler(this, e);
            }
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
                    newIndex = this.GetPreviousVisibleSection(this._selectedSectionIndex);
                }
                else if (e.KeyCode == Keys.Next)
                {
                    e.Handled = true;
                    newIndex = this.GetNextVisibleSection(this._selectedSectionIndex);
                }
                if ((newIndex != -1) && (newIndex != this._selectedSectionIndex))
                {
                    this.OnSectionSelected(newIndex, true);
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this._mouseDown = true;
                Point pt = new Point(e.X, e.Y);
                int num = this._mouseDownIndex;
                int index = -1;
                this.GetSectionAt(pt, out index);
                if (index != num)
                {
                    this._mouseDownIndex = -1;
                    if (num != -1)
                    {
                        base.Invalidate(this._sectionCollection[num].Rectangle);
                    }
                    if (index != -1)
                    {
                        this._mouseDownIndex = index;
                        base.Invalidate(this._sectionCollection[this._mouseDownIndex].Rectangle);
                    }
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            int num = this._hoverIndex;
            this._hoverIndex = -1;
            if (num != -1)
            {
                base.Invalidate(this._sectionCollection[num].Rectangle);
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.EnsureSectionRectangles();
            Point pt = new Point(e.X, e.Y);
            int num = this._hoverIndex;
            int index = -1;
            this.GetSectionAt(pt, out index);
            if (index != num)
            {
                this._hoverIndex = -1;
                this._mouseDownIndex = -1;
                if (num != -1)
                {
                    base.Invalidate(this._sectionCollection[num].Rectangle);
                }
                if (index != -1)
                {
                    this._hoverIndex = index;
                    if (this._mouseDown)
                    {
                        this._mouseDownIndex = this._hoverIndex;
                    }
                    base.Invalidate(this._sectionCollection[this._hoverIndex].Rectangle);
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && this._mouseDown)
            {
                int num2;
                this._mouseDown = false;
                int num = this._mouseDownIndex;
                this._mouseDownIndex = -1;
                if (num != -1)
                {
                    base.Invalidate(this._sectionCollection[num].Rectangle);
                }
                this.GetSectionAt(new Point(e.X, e.Y), out num2);
                if ((num2 != -1) && (num2 != this._selectedSectionIndex))
                {
                    this.OnSectionSelected(num2, true);
                }
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clientRectangle = base.ClientRectangle;
            Graphics g = e.Graphics;
            int visibleSectionCount = this.GetVisibleSectionCount();
            int sectionHeight = this.SectionHeight;
            int num3 = (10 + (visibleSectionCount * sectionHeight)) + ((visibleSectionCount - 1) * 2);
            g.FillRectangle(SystemBrushes.Control, 0, 0, clientRectangle.Width, clientRectangle.Height);
            if ((visibleSectionCount >= 1) && (clientRectangle.Height >= num3))
            {
                int num6;
                this.EnsureSectionRectangles(g, clientRectangle);
                Brush sectionBrush = new SolidBrush(Color.FromArgb(0x40, SystemColors.ControlDark));
                Font font = this.Font;
                int y = 0;
                int height = clientRectangle.Height;
                for (num6 = 0; num6 <= this._selectedSectionIndex; num6++)
                {
                    GroupViewSection section = this._sectionCollection[num6];
                    if (section.Visible)
                    {
                        this.PaintSection(g, section, num6, sectionBrush, font);
                        y = section.Rectangle.Top + sectionHeight;
                    }
                }
                for (num6 = this._sectionCollection.Count - 1; num6 > this._selectedSectionIndex; num6--)
                {
                    GroupViewSection section2 = this._sectionCollection[num6];
                    if (section2.Visible)
                    {
                        this.PaintSection(g, section2, num6, sectionBrush, font);
                        height = section2.Rectangle.Top - 2;
                    }
                }
                g.DrawRectangle(SystemPens.ControlDark, 0, y, clientRectangle.Width - 1, (height - y) - 1);
                if (this._selectedSection != null)
                {
                    g.DrawRectangle(SystemPens.ControlDark, 0, this._selectedSection.Rectangle.Top, clientRectangle.Width - 1, sectionHeight - 1);
                }
                sectionBrush.Dispose();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            this.sectionRectanglesCalculated = false;
            base.OnResize(e);
        }

        internal void OnSectionAdded(GroupViewSection section)
        {
            section.SetOwner(this);
            if ((this._selectedSectionIndex == -1) && section.Visible)
            {
                this.SelectedSectionIndex = 0;
            }
        }

        internal void OnSectionChanged(GroupViewSection section, bool relayoutRequired)
        {
            if (relayoutRequired)
            {
                this.DirtySectionRectangles(base.IsHandleCreated);
                if (section.Visible && (this._selectedSectionIndex == -1))
                {
                    int index = this._sectionCollection.IndexOf(section);
                    this.SelectedSectionIndex = index;
                }
                else if (!section.Visible && (section == this._selectedSection))
                {
                    int nextVisibleSection = this.GetNextVisibleSection(this._selectedSectionIndex);
                    this.SelectedSectionIndex = nextVisibleSection;
                }
                base.PerformLayout();
            }
            if (base.IsHandleCreated)
            {
                base.Invalidate();
            }
        }

        internal void OnSectionRemoved(GroupViewSection section)
        {
            if (section == this._selectedSection)
            {
                int selectedSectionIndex = this.SelectedSectionIndex;
                if (selectedSectionIndex == this._sectionCollection.Count)
                {
                    selectedSectionIndex--;
                }
                this.SelectedSectionIndex = selectedSectionIndex;
            }
            else
            {
                for (int i = 0; i < this._sectionCollection.Count; i++)
                {
                    if (this._sectionCollection[i] == this._selectedSection)
                    {
                        this.SelectedSectionIndex = i;
                    }
                }
            }
            section.SetOwner(null);
        }

        private void OnSectionSelected(int newIndex, bool uiSelection)
        {
            try
            {
                if (uiSelection)
                {
                    this._itemContainer.BeginUpdate();
                }
                if (this._selectedSection != null)
                {
                    if (uiSelection)
                    {
                        this.OnSelectedSectionChanging(EventArgs.Empty);
                    }
                    this._selectedSection.OnDeactivate();
                    this._selectedSection = null;
                }
                this._selectedSectionIndex = newIndex;
                if (this._selectedSectionIndex != -1)
                {
                    this._selectedSection = this._sectionCollection[this._selectedSectionIndex];
                    this._selectedSection.OnActivate(this._itemContainer);
                    if (uiSelection)
                    {
                        this.OnSelectedSectionChanged(EventArgs.Empty);
                    }
                }
            }
            finally
            {
                if (uiSelection)
                {
                    this._itemContainer.EndUpdate();
                }
            }
            this.DirtySectionRectangles(false);
            if (base.IsHandleCreated)
            {
                base.PerformLayout();
                base.Invalidate();
            }
        }

        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[EventSelectedItemChanged];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSelectedSectionChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[EventSelectedSectionChanged];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSelectedSectionChanging(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[EventSelectedSectionChanging];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void PaintSection(Graphics g, GroupViewSection section, int sectionIndex, Brush sectionBrush, Font font)
        {
            Brush control;
            Rectangle rect = section.Rectangle;
            Rectangle layoutRectangle = rect;
            layoutRectangle.Inflate(-2, -2);
            if ((sectionIndex == this._mouseDownIndex) || (sectionIndex == this._selectedSectionIndex))
            {
                g.FillRectangle(SystemBrushes.ControlDark, rect);
                control = SystemBrushes.Control;
            }
            else
            {
                g.FillRectangle(sectionBrush, rect);
                control = SystemBrushes.ControlText;
            }
            if (this._sectionImages != null)
            {
                if ((section.ImageIndex != -1) && (this._sectionImages.Images.Count > section.ImageIndex))
                {
                    int x = (rect.Left + 2) + 1;
                    int y = rect.Top + ((rect.Height - 0x10) / 2);
                    this._sectionImages.Draw(g, x, y, section.ImageIndex);
                }
                int num3 = 20;
                layoutRectangle.X += num3;
                layoutRectangle.Width -= num3;
            }
            StringFormat format = new StringFormat(StringFormatFlags.NoWrap);
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Center;
            format.Trimming = StringTrimming.EllipsisCharacter;
            g.DrawString(section.Text, font, control, layoutRectangle, format);
            format.Dispose();
            if (sectionIndex == this._hoverIndex)
            {
                g.DrawRectangle(SystemPens.ControlDark, rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
            }
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            return (this.ProcessKeyEventArgs(ref m) || base.ProcessKeyPreview(ref m));
        }

        private void UpdateLayout()
        {
            this.DirtySectionRectangles(false);
            base.PerformLayout();
        }

        public bool AllowDrag
        {
            get
            {
                return this._allowDrag;
            }
            set
            {
                this._allowDrag = value;
                if (this._allowDrag)
                {
                    this._itemContainer.ItemDrag += new ItemDragEventHandler(this.OnGroupViewItemDrag);
                }
                else
                {
                    this._itemContainer.ItemDrag -= new ItemDragEventHandler(this.OnGroupViewItemDrag);
                }
            }
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                int num4;
                int sectionHeight = this.SectionHeight;
                Rectangle clientRectangle = base.ClientRectangle;
                if ((clientRectangle.Height < this.MinimumHeight) || (this.GetVisibleSectionCount() == 0))
                {
                    return new Rectangle(0, 0, 0, 0);
                }
                int num2 = 0;
                int num3 = 0;
                for (num4 = 0; num4 < this._selectedSectionIndex; num4++)
                {
                    GroupViewSection section = this._sectionCollection[num4];
                    if (section.Visible)
                    {
                        num2++;
                    }
                }
                for (num4 = this._selectedSectionIndex + 1; num4 < this._sectionCollection.Count; num4++)
                {
                    GroupViewSection section2 = this._sectionCollection[num4];
                    if (section2.Visible)
                    {
                        num3++;
                    }
                }
                int y = ((num2 + 1) * sectionHeight) + (num2 * 2);
                int num6 = num3 * (sectionHeight + 2);
                return new Rectangle(1, y, clientRectangle.Width - 2, ((clientRectangle.Height - y) - num6) - 1);
            }
        }

        private int MinimumHeight
        {
            get
            {
                int visibleSectionCount = this.GetVisibleSectionCount();
                return (((10 + (visibleSectionCount * this.SectionHeight)) + ((visibleSectionCount - 1) * 2)) + 1);
            }
        }

        private int SectionHeight
        {
            get
            {
                if (this._sectionHeight == -1)
                {
                    this._sectionHeight = Math.Max(0x10, this.Font.Height) + 6;
                }
                return this._sectionHeight;
            }
        }

        public ImageList SectionImages
        {
            get
            {
                return this._sectionImages;
            }
            set
            {
                if (this._sectionImages != value)
                {
                    this._sectionImages = value;
                    this.DirtySectionRectangles(false);
                }
            }
        }

        public GroupViewSectionCollection Sections
        {
            get
            {
                return this._sectionCollection;
            }
        }

        public GroupViewItem SelectedItem
        {
            get
            {
                if (this._itemContainer.SelectedItems.Count == 0)
                {
                    return null;
                }
                return ((GroupViewListViewItem) this._itemContainer.SelectedItems[0]).GroupViewItem;
            }
            set
            {
                if (value != null)
                {
                    value.ListViewItem.Selected = true;
                }
                else
                {
                    this._itemContainer.SelectedItems.Clear();
                }
            }
        }

        public int SelectedItemIndex
        {
            get
            {
                if (this._itemContainer.SelectedIndices.Count == 0)
                {
                    return -1;
                }
                return this._itemContainer.SelectedIndices[0];
            }
            set
            {
                if (value == -1)
                {
                    this._itemContainer.SelectedItems.Clear();
                }
                else
                {
                    this._itemContainer.Items[value].Selected = true;
                }
            }
        }

        public GroupViewSection SelectedSection
        {
            get
            {
                return this._selectedSection;
            }
            set
            {
                int index;
                if (value == null)
                {
                    index = -1;
                }
                else
                {
                    index = this._sectionCollection.IndexOf(value);
                    if (index == -1)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                this.SelectedSectionIndex = index;
            }
        }

        public int SelectedSectionIndex
        {
            get
            {
                return this._selectedSectionIndex;
            }
            set
            {
                if (this._selectedSectionIndex != value)
                {
                    int visibleSectionCount = this.GetVisibleSectionCount();
                    if ((visibleSectionCount == 0) && (value != -1))
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    if ((visibleSectionCount > 0) && (((value < 0) || (value >= this._sectionCollection.Count)) || !this._sectionCollection[value].Visible))
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    this.OnSectionSelected(value, false);
                }
            }
        }

        public class ControlCollection : Control.ControlCollection
        {
            private GroupView _owner;

            public ControlCollection(GroupView owner) : base(owner)
            {
                this._owner = owner;
            }

            public override void Add(Control value)
            {
                if (base.Count != 0)
                {
                    throw new InvalidOperationException();
                }
                if (!(value is GroupViewListView))
                {
                    throw new ArgumentException();
                }
                base.Add(value);
            }

            public override void Remove(Control value)
            {
                throw new NotSupportedException();
            }
        }

        public sealed class GroupViewSectionCollection : IList, ICollection, IEnumerable
        {
            private GroupView _owner;
            private ArrayList _sections;

            internal GroupViewSectionCollection(GroupView owner)
            {
                this._owner = owner;
                this._sections = new ArrayList();
            }

            public void Add(GroupViewSection value)
            {
                this._sections.Add(value);
                this._owner.OnSectionAdded(value);
            }

            public void AddRange(GroupViewSection[] sections)
            {
                foreach (GroupViewSection section in sections)
                {
                    this.Add(section);
                }
            }

            public bool Contains(GroupViewSection section)
            {
                return (this.IndexOf(section) != -1);
            }

            public void CopyTo(Array dest, int index)
            {
                if (this.Count > 0)
                {
                    this._sections.CopyTo(dest, index);
                }
            }

            public IEnumerator GetEnumerator()
            {
                if (this.Count != 0)
                {
                    return this._sections.GetEnumerator();
                }
                return new GroupViewSection[0].GetEnumerator();
            }

            public int IndexOf(GroupViewSection section)
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this[i] == section)
                    {
                        return i;
                    }
                }
                return -1;
            }

            public void Remove(GroupViewSection value)
            {
                this._sections.Remove(value);
                this._owner.OnSectionRemoved(value);
            }

            public void RemoveAt(int index)
            {
                GroupViewSection section = (GroupViewSection) this._sections[index];
                this._sections.RemoveAt(index);
                this._owner.OnSectionRemoved(section);
            }

            int IList.Add(object value)
            {
                GroupViewSection section = value as GroupViewSection;
                if (section == null)
                {
                    throw new ArgumentException();
                }
                this.Add(section);
                return this.IndexOf(section);
            }

            void IList.Clear()
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    this.RemoveAt(0);
                }
            }

            bool IList.Contains(object value)
            {
                GroupViewSection section = value as GroupViewSection;
                return ((section != null) && this.Contains(section));
            }

            int IList.IndexOf(object value)
            {
                GroupViewSection section = value as GroupViewSection;
                if (section != null)
                {
                    return this.IndexOf(section);
                }
                return -1;
            }

            void IList.Insert(int index, object value)
            {
                throw new NotSupportedException();
            }

            void IList.Remove(object value)
            {
                GroupViewSection section = value as GroupViewSection;
                if (section != null)
                {
                    this.Remove(section);
                }
            }

            public int Count
            {
                get
                {
                    return this._sections.Count;
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

            public GroupViewSection this[int index]
            {
                get
                {
                    return (GroupViewSection) this._sections[index];
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
    }
}

