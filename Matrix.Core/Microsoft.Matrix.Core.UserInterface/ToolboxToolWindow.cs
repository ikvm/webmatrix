namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix;
    using Microsoft.Matrix.Core.Toolbox;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class ToolboxToolWindow : ToolWindow, ICommandHandlerWithContext
    {
        private IToolboxClient _activeClient;
        private IDesignerHost _activeDesigner;
        private ToolboxSection _currentSection;
        private GroupView _groupView;
        private ImageList _groupViewImages;
        private ToolboxGroupViewListView _groupViewListView;
        private bool _internalChange;
        private ArrayList _sections;
        private static readonly string ToolboxIndexDataFormat = "MxToolboxIndexDataFormat";

        public ToolboxToolWindow(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
            this._sections = new ArrayList();
            IDesignerEventService service = (IDesignerEventService) this.GetService(typeof(IDesignerEventService));
            if (service != null)
            {
                service.ActiveDesignerChanged += new ActiveDesignerEventHandler(this.OnActiveDesignerChanged);
            }
            IToolboxService service2 = (IToolboxService) this.GetService(typeof(IToolboxService));
            if (service2 != null)
            {
                service2.SectionAdded += new ToolboxSectionEventHandler(this.OnSectionAdded);
                service2.SectionRemoved += new ToolboxSectionEventHandler(this.OnSectionRemoved);
                service2.ActiveSectionChanged += new ToolboxSectionEventHandler(this.OnActiveSectionChanged);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IServiceContainer container = (IServiceContainer) this.GetService(typeof(IServiceContainer));
                if (container != null)
                {
                    container.RemoveService(typeof(IToolboxService));
                }
                IToolboxService service = (IToolboxService) this.GetService(typeof(IToolboxService));
                if (service != null)
                {
                    service.SectionAdded -= new ToolboxSectionEventHandler(this.OnSectionAdded);
                    service.SectionRemoved -= new ToolboxSectionEventHandler(this.OnSectionRemoved);
                    service.ActiveSectionChanged -= new ToolboxSectionEventHandler(this.OnActiveSectionChanged);
                }
                this._activeDesigner = null;
                this._activeClient = null;
                this._currentSection = null;
                IDesignerEventService service2 = (IDesignerEventService) this.GetService(typeof(IDesignerEventService));
                if (service2 != null)
                {
                    service2.ActiveDesignerChanged -= new ActiveDesignerEventHandler(this.OnActiveDesignerChanged);
                }
            }
            base.Dispose(disposing);
        }

        private int GetNextIndexFromPoint(int x, int y)
        {
            Point point = this._groupViewListView.PointToClient(new Point(x, y));
            ListViewItem itemAt = this._groupViewListView.GetItemAt(point.X, point.Y);
            if (itemAt != null)
            {
                return (this._groupViewListView.Items.IndexOf(itemAt) + 1);
            }
            if (point.Y < 0)
            {
                return 0;
            }
            return this._groupViewListView.Items.Count;
        }

        private void InitializeComponent()
        {
            this._groupViewListView = new ToolboxGroupViewListView();
            this._groupViewListView.MultiLineToolTips = true;
            this._groupView = new GroupView(this._groupViewListView);
            this._groupViewImages = new ImageList();
            base.SuspendLayout();
            this._groupViewImages.ImageSize = new Size(0x10, 0x10);
            this._groupView.Dock = DockStyle.Fill;
            this._groupView.AllowDrop = true;
            this._groupView.AllowDrag = true;
            this._groupView.SectionImages = this._groupViewImages;
            this._groupView.SelectedSectionChanged += new EventHandler(this.OnGroupViewSelectedSectionChanged);
            this._groupView.DragDrop += new DragEventHandler(this.OnGroupViewDragDrop);
            this._groupView.DragEnter += new DragEventHandler(this.OnGroupViewDragEnter);
            this._groupView.DragOver += new DragEventHandler(this.OnGroupViewDragOver);
            this._groupView.DragLeave += new EventHandler(this.OnGroupViewDragLeave);
            this._groupViewListView.ItemActivate += new EventHandler(this.OnGroupViewItemActivate);
            this._groupViewListView.ShowContextMenu += new ShowContextMenuEventHandler(this.OnGroupViewShowContextMenu);
            this._groupViewListView.LabelEdit = true;
            this._groupViewListView.AfterLabelEdit += new LabelEditEventHandler(this.OnGroupViewLabelEdit);
            this._groupViewListView.BeforeLabelEdit += new LabelEditEventHandler(this.OnGroupViewBeforeLabelEdit);
            this.Text = "Toolbox";
           
            base.Icon = new Icon(typeof(ToolboxToolWindow), "ToolboxToolWindow.ico");
            base.Controls.Add(this._groupView);
            base.ResumeLayout(false);
        }

        bool ICommandHandlerWithContext.HandleCommand(Command command, object context)
        {
            ToolboxDataGroupViewItem item = (ToolboxDataGroupViewItem) context;
            bool flag = false;
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 0x145:
                    case 0x146:
                    case 0x147:
                        if (this.CurrentSection != null)
                        {
                            flag = this.CurrentSection.Customize(command.CommandID - 0x145, base.ServiceProvider);
                        }
                        return flag;

                    case 0x148:
                        if (this._groupViewListView.SelectedItems.Count > 0)
                        {
                            this._groupViewListView.SelectedItems[0].BeginEdit();
                        }
                        return true;

                    case 0x149:
                        if (this.CurrentSection != null)
                        {
                            this.CurrentSection.RemoveToolboxDataItem(item.Item);
                        }
                        return true;

                    case 330:
                    {
                        IToolboxService service = (IToolboxService) base.ServiceProvider.GetService(typeof(IToolboxService));
                        this._groupView.BeginUpdate();
                        try
                        {
                            service.ResetToolbox();
                        }
                        finally
                        {
                            this._groupView.EndUpdate();
                        }
                        return true;
                    }
                    case 0x14b:
                        if (this.CurrentSection != null)
                        {
                            this.CurrentSection.SortToolbox();
                        }
                        return true;
                }
            }
            return flag;
        }

        bool ICommandHandlerWithContext.UpdateCommand(Command command, object context)
        {
            ToolboxDataGroupViewItem item = (ToolboxDataGroupViewItem) context;
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 0x145:
                    case 0x146:
                    case 0x147:
                        if (this.CurrentSection != null)
                        {
                            string customizationText = this.CurrentSection.GetCustomizationText(command.CommandID - 0x145);
                            if (customizationText == null)
                            {
                                command.Visible = false;
                                command.Enabled = false;
                            }
                            else
                            {
                                command.Text = customizationText;
                                command.Visible = true;
                                command.Enabled = true;
                            }
                        }
                        return true;

                    case 0x148:
                        if (this.CurrentSection != null)
                        {
                            command.Enabled = this.CurrentSection.CanRename && (item != null);
                        }
                        return true;

                    case 0x149:
                        if (this.CurrentSection != null)
                        {
                            command.Enabled = this.CurrentSection.CanRemove && (item != null);
                        }
                        return true;

                    case 330:
                        command.Enabled = this.CurrentSection.IsCustomizable;
                        return true;

                    case 0x14b:
                        if (this.CurrentSection != null)
                        {
                            command.Enabled = this.CurrentSection.CanReorder;
                        }
                        return true;
                }
            }
            return false;
        }

        private void OnActiveDesignerChanged(object sender, ActiveDesignerEventArgs e)
        {
            bool flag = false;
            IToolboxClient client = this._activeClient;
            this._activeDesigner = e.NewDesigner;
            this._activeClient = null;
            if (this._activeDesigner != null)
            {
                this._activeClient = (IToolboxClient) e.NewDesigner.GetService(typeof(DocumentWindow));
            }
            if (client != this._activeClient)
            {
                if (((client != null) && (this._activeClient == null)) || ((client == null) && (this._activeClient != null)))
                {
                    flag = true;
                }
                else if (client.GetType() != this._activeClient.GetType())
                {
                    flag = true;
                }
            }
            if (!flag && (this._activeClient is IMultiViewDocumentWindow))
            {
                flag = true;
            }
            if (flag)
            {
                this.UpdateToolboxClient();
            }
        }

        private void OnActiveSectionChanged(object sender, ToolboxSectionEventArgs e)
        {
            if (!this._internalChange)
            {
                ToolboxSection section = e.Section;
                if (this.CurrentSection != section)
                {
                    this.CurrentSection = section;
                    SectionInfo info = null;
                    foreach (SectionInfo info2 in this._sections)
                    {
                        if (info2.ToolboxSection == section)
                        {
                            info = info2;
                            break;
                        }
                    }
                    this._groupView.SelectedSection = info.UISection;
                }
            }
        }

        private void OnGroupViewBeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (!this.CurrentSection.CanRename)
            {
                e.CancelEdit = true;
            }
            else
            {
                ToolboxDataGroupViewItem selectedItem = (ToolboxDataGroupViewItem) this._groupView.SelectedItem;
                int index = this._groupViewListView.Items[e.Item].Text.IndexOf(Environment.NewLine);
                this._groupViewListView.Items[e.Item].Text = string.Empty;
                if (index != -1)
                {
                    this._groupViewListView.SetEditControlText(selectedItem.Text.Substring(0, index));
                }
            }
        }

        private void OnGroupViewDragDrop(object sender, DragEventArgs e)
        {
            IDataObject data = e.Data;
            if (this.CurrentSection.CanReorder && data.GetDataPresent(ToolboxIndexDataFormat))
            {
                this._groupViewListView.DropCueIndex = -1;
                int oldIndex = (int) data.GetData(ToolboxIndexDataFormat);
                int nextIndexFromPoint = this.GetNextIndexFromPoint(e.X, e.Y);
                if (nextIndexFromPoint != -1)
                {
                    this.CurrentSection.ReorderToolboxDataItem(oldIndex, nextIndexFromPoint);
                }
            }
            else if (this.CurrentSection.AllowDrop && this.CurrentSection.CanCreateToolboxDataItem(data))
            {
                ToolboxDataItem item = this.CurrentSection.CreateToolboxDataItem(e.Data);
                if (item != null)
                {
                    this.CurrentSection.AddToolboxDataItem(item);
                }
            }
        }

        private void OnGroupViewDragEnter(object sender, DragEventArgs e)
        {
            if (this.CurrentSection.CanReorder && e.Data.GetDataPresent(ToolboxIndexDataFormat))
            {
                e.Effect = DragDropEffects.Move;
            }
            else if (this.CurrentSection.AllowDrop && this.CurrentSection.CanCreateToolboxDataItem(e.Data))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void OnGroupViewDragLeave(object sender, EventArgs e)
        {
            this._groupViewListView.DropCueIndex = -1;
        }

        private void OnGroupViewDragOver(object sender, DragEventArgs e)
        {
            if (this.CurrentSection.CanReorder && e.Data.GetDataPresent(ToolboxIndexDataFormat))
            {
                e.Effect = DragDropEffects.Move;
                this._groupViewListView.DropCueIndex = this.GetNextIndexFromPoint(e.X, e.Y);
            }
            else if (this.CurrentSection.AllowDrop && this.CurrentSection.CanCreateToolboxDataItem(e.Data))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void OnGroupViewItemActivate(object sender, EventArgs e)
        {
            ToolboxDataGroupViewItem selectedItem = (ToolboxDataGroupViewItem) this._groupView.SelectedItem;
            if (selectedItem != null)
            {
                this._activeClient.OnToolboxDataItemPicked(selectedItem.Item);
            }
        }

        private void OnGroupViewLabelEdit(object sender, LabelEditEventArgs e)
        {
            ToolboxDataGroupViewItem selectedItem = (ToolboxDataGroupViewItem) this._groupView.SelectedItem;
            if (selectedItem != null)
            {
                ToolboxDataItem item = selectedItem.Item;
                if (e.Label == null)
                {
                    this._groupViewListView.Items[e.Item].Text = item.DisplayName;
                }
                else
                {
                    string editControlText = this._groupViewListView.GetEditControlText();
                    if (!this.CurrentSection.RenameToolboxDataItem(item, editControlText))
                    {
                        MessageBox.Show(this, "Invalid toolbox display name", "Toolbox Rename", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                        this._groupViewListView.Items[e.Item].Text = item.DisplayName;
                        this._groupViewListView.Items[e.Item].BeginEdit();
                    }
                    else
                    {
                        this._groupViewListView.Items[e.Item].Text = item.DisplayName;
                    }
                }
            }
        }

        private void OnGroupViewSelectedSectionChanged(object sender, EventArgs e)
        {
            GroupViewSection selectedSection = this._groupView.SelectedSection;
            if (selectedSection != null)
            {
                for (int i = 0; i < this._sections.Count; i++)
                {
                    SectionInfo info = (SectionInfo) this._sections[i];
                    if (info.UISection == selectedSection)
                    {
                        this.CurrentSection = info.ToolboxSection;
                        return;
                    }
                }
            }
        }

        private void OnGroupViewShowContextMenu(object sender, ShowContextMenuEventArgs e)
        {
            if (this._currentSection.IsCustomizable)
            {
                ToolboxDataGroupViewItem context = null;
                if (this._groupViewListView.SelectedItems.Count > 0)
                {
                    context = (ToolboxDataGroupViewItem) this._groupView.SelectedItem;
                }
                ICommandManager service = (ICommandManager) this.GetService(typeof(ICommandManager));
                if (service != null)
                {
                    service.ShowContextMenu(typeof(GlobalCommands), 2, this, context, this._groupViewListView, e.Location);
                }
            }
        }

        private void OnSectionAdded(object sender, ToolboxSectionEventArgs e)
        {
            ToolboxSection section = e.Section;
            this._groupViewImages.Images.Add(section.Icon);
            GroupViewSection section2 = new GroupViewSection();
            section2.ImageIndex = this._groupViewImages.Images.Count - 1;
            section2.Text = section.Name;
            section2.Visible = false;
            foreach (ToolboxDataItem item in section.ToolboxDataItems)
            {
                ToolboxDataGroupViewItem item2 = new ToolboxDataGroupViewItem(item, this);
                section2.Items.Add(item2);
            }
            SectionInfo info = new SectionInfo();
            info.ToolboxSection = section;
            info.UISection = section2;
            this._sections.Add(info);
            this._groupView.Sections.Add(section2);
            section.ItemsChanged += new EventHandler(this.OnToolboxSectionItemsChanged);
            this.UpdateToolboxClient();
        }

        private void OnSectionRemoved(object sender, ToolboxSectionEventArgs e)
        {
            ToolboxSection section = e.Section;
            int index = -1;
            for (int i = this._sections.Count - 1; i >= 0; i--)
            {
                if (((SectionInfo) this._sections[i]).ToolboxSection == section)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new ArgumentException("Unknown section");
            }
            section.ItemsChanged -= new EventHandler(this.OnToolboxSectionItemsChanged);
            this._groupView.Sections.Remove(((SectionInfo) this._sections[index]).UISection);
            this._sections.RemoveAt(index);
        }

        private void OnToolboxSectionItemsChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this._sections.Count; i++)
            {
                SectionInfo info = (SectionInfo) this._sections[i];
                if (info.ToolboxSection == sender)
                {
                    this._groupView.BeginUpdate();
                    try
                    {
                        ((IList) info.UISection.Items).Clear();
                        foreach (ToolboxDataItem item in info.ToolboxSection.ToolboxDataItems)
                        {
                            ToolboxDataGroupViewItem item2 = new ToolboxDataGroupViewItem(item, this);
                            info.UISection.Items.Add(item2);
                        }
                    }
                    finally
                    {
                        this._groupView.EndUpdate();
                    }
                }
            }
        }

        private void UpdateToolboxClient()
        {
            if (this._activeClient == null)
            {
                for (int i = 0; i < this._sections.Count; i++)
                {
                    ((SectionInfo) this._sections[i]).UISection.Visible = false;
                }
                this.CurrentSection = null;
            }
            else
            {
                SectionInfo info = null;
                SectionInfo info2 = null;
                for (int j = 0; j < this._sections.Count; j++)
                {
                    SectionInfo info3 = (SectionInfo) this._sections[j];
                    if (this._activeClient.SupportsToolboxSection(info3.ToolboxSection))
                    {
                        if (info3.ToolboxSection == this._activeClient.DefaultToolboxSection)
                        {
                            info2 = info3;
                        }
                        if (info == null)
                        {
                            info = info3;
                        }
                        info3.UISection.Visible = true;
                    }
                    else
                    {
                        info3.UISection.Visible = false;
                    }
                }
                if (info2 != null)
                {
                    this.CurrentSection = info2.ToolboxSection;
                    this._groupView.SelectedSection = info2.UISection;
                }
                else if (info != null)
                {
                    this.CurrentSection = info.ToolboxSection;
                    this._groupView.SelectedSection = info.UISection;
                }
                else
                {
                    this.CurrentSection = null;
                }
            }
        }

        private ToolboxSection CurrentSection
        {
            get
            {
                return this._currentSection;
            }
            set
            {
                if (this._currentSection != value)
                {
                    this._currentSection = value;
                    if ((this._currentSection == null) || !this._currentSection.IsCustomizable)
                    {
                        this._groupViewListView.WatermarkText = null;
                    }
                    else
                    {
                        this._groupViewListView.WatermarkText = this._currentSection.CustomizationHint;
                    }
                    IToolboxService service = this.GetService(typeof(IToolboxService)) as IToolboxService;
                    this._internalChange = true;
                    try
                    {
                        service.ActiveSection = this._currentSection;
                    }
                    finally
                    {
                        this._internalChange = false;
                    }
                }
            }
        }

        private class SectionInfo
        {
            public Microsoft.Matrix.Core.Toolbox.ToolboxSection ToolboxSection;
            public GroupViewSection UISection;
        }

        private sealed class ToolboxDataGroupViewItem : GroupViewItem
        {
            private ToolboxDataItem _item;
            private ToolboxToolWindow _owner;

            public ToolboxDataGroupViewItem(ToolboxDataItem item, ToolboxToolWindow owner)
            {
                this._item = item;
                this._owner = owner;
            }

            public override DataObject GetDragDropDataObject()
            {
                DataObject dataObject = this._item.GetDataObject(this._owner._activeDesigner);
                if (dataObject != null)
                {
                    int selectedItemIndex = this._owner._groupView.SelectedItemIndex;
                    dataObject.SetData(ToolboxToolWindow.ToolboxIndexDataFormat, selectedItemIndex);
                }
                return dataObject;
            }

            public override System.Drawing.Image Image
            {
                get
                {
                    return this._item.Glyph;
                }
                set
                {
                    throw new Exception();
                }
            }

            public ToolboxDataItem Item
            {
                get
                {
                    return this._item;
                }
            }

            public override string Text
            {
                get
                {
                    return this._item.DisplayName;
                }
                set
                {
                    base.Text = value;
                }
            }
        }

        private sealed class ToolboxGroupViewListView : GroupViewListView
        {
            private int _dropCueIndex = -1;

            private Rectangle GetDropCueRectangle(int index)
            {
                if ((index != -1) && (index < base.Items.Count))
                {
                    Rectangle bounds = base.Items[index].GetBounds(ItemBoundsPortion.Entire);
                    return new Rectangle(bounds.X + 6, bounds.Y - 1, bounds.Width - 12, 2);
                }
                if ((index == base.Items.Count) && (base.Items.Count != 0))
                {
                    Rectangle rectangle2 = base.Items[index - 1].GetBounds(ItemBoundsPortion.Entire);
                    return new Rectangle(rectangle2.X + 6, rectangle2.Bottom, rectangle2.Width - 12, 2);
                }
                return Rectangle.Empty;
            }

            internal string GetEditControlText()
            {
                IntPtr zero = IntPtr.Zero;
                zero = Interop.SendMessage(base.Handle, 0x1018, IntPtr.Zero, IntPtr.Zero);
                if (zero != IntPtr.Zero)
                {
                    StringBuilder lpString = new StringBuilder(0x200);
                    Interop.GetWindowText(zero, lpString, 0x200);
                    return lpString.ToString();
                }
                return string.Empty;
            }

            internal void SetEditControlText(string newText)
            {
                IntPtr zero = IntPtr.Zero;
                zero = Interop.SendMessage(base.Handle, 0x1018, IntPtr.Zero, IntPtr.Zero);
                if (zero != IntPtr.Zero)
                {
                    Interop.SetWindowText(zero, newText);
                }
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 15)
                {
                    base.WndProc(ref m);
                    Graphics graphics = Graphics.FromHwndInternal(base.Handle);
                    Rectangle dropCueRectangle = this.GetDropCueRectangle(this._dropCueIndex);
                    if (dropCueRectangle != Rectangle.Empty)
                    {
                        graphics.FillRectangle(SystemBrushes.ControlDarkDark, dropCueRectangle);
                    }
                    graphics.Dispose();
                }
                else
                {
                    base.WndProc(ref m);
                }
            }

            public int DropCueIndex
            {
                get
                {
                    return this._dropCueIndex;
                }
                set
                {
                    if (this._dropCueIndex != value)
                    {
                        int index = this._dropCueIndex;
                        this._dropCueIndex = value;
                        Rectangle dropCueRectangle = this.GetDropCueRectangle(index);
                        if (dropCueRectangle != Rectangle.Empty)
                        {
                            base.Invalidate(dropCueRectangle);
                        }
                        dropCueRectangle = this.GetDropCueRectangle(this._dropCueIndex);
                        if (dropCueRectangle != Rectangle.Empty)
                        {
                            base.Invalidate(dropCueRectangle);
                        }
                    }
                }
            }
        }
    }
}

