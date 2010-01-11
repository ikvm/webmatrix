namespace Microsoft.Matrix.Packages.DBAdmin.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.Packages.DBAdmin.Documents;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class TableView : Control, IDocumentView, ICommandHandler, IPropertyBrowserClient
    {
        private ColumnHeader _allowNullsColumnHeader;
        private Panel _bottomPanel;
        private MxToolBar _columnCommandsToolBar;
        private ImageList _columnImageList;
        private ColumnHeader _columnNameColumnHeader;
        private PropertyGrid _columnPropertyGrid;
        private Label _columnPropertyHelpLabel;
        private MxListView _columnsListView;
        private ColumnHeader _dataTypeColumnHeader;
        private MxToolBarButton _deleteColumnToolBarButton;
        private bool _dirty;
        private TableDocument _document;
        private MxInfoLabel _errorLabel;
        private Splitter _horizontalSplitter;
        private bool _isReadOnly;
        private bool _loadError;
        private MxToolBarButton _moveColumnDownToolBarButton;
        private MxToolBarButton _moveColumnUpToolBarButton;
        private MxToolBarButton _newColumnToolBarButton;
        private MxToolBarButton _separator1ToolBarButton;
        private MxToolBarButton _separator2ToolBarButton;
        private IServiceProvider _serviceProvider;
        private ColumnHeader _sizeColumnHeader;
        private Panel _topPanel;
        private Splitter _verticalSplitter;
        private Bitmap _viewImage;
        private const int ColumnIconIndex = 0;
        private const int DeleteColumnIconIndex = 5;
        private static readonly object DocumentChangedEvent = new object();
        private static readonly string errorFormatString = "The table could not be loaded from the database.  Please check the status of your database and double-click this label to try again.\r\n\r\nError:\r\n{0}";
        private const int KeyColumnIconIndex = 1;
        private const int MoveColumnDownIconIndex = 3;
        private const int MoveColumnUpIconIndex = 2;
        private const int NewColumnIconIndex = 4;

        event EventHandler IDocumentView.DocumentChanged
        {
            add
            {
                base.Events.AddHandler(DocumentChangedEvent, value);
            }
            remove
            {
                base.Events.RemoveHandler(DocumentChangedEvent, value);
            }
        }

        public TableView(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this.InitializeUserInterface();
        }

        private void AddNewColumn()
        {
            string caption = ((TableProjectItem) this._document.ProjectItem).Caption;
            Column column = ((DatabaseProject) this._document.ProjectItem.Project).CreateColumn();
            column.Name = this.GetUniqueColumnName();
            ColumnListViewItem item = new ColumnListViewItem(column);
            this._columnsListView.Items.Add(item);
            item.Selected = true;
            item.Focused = true;
            item.EnsureVisible();
            this._columnsListView.Focus();
            this._dirty = true;
            this.OnDocumentChanged(EventArgs.Empty);
            item.BeginEdit();
        }

        private void DeleteSelectedColumn()
        {
            if (this._columnsListView.SelectedIndices.Count == 1)
            {
                int num = this._columnsListView.SelectedIndices[0];
                this._columnsListView.SelectedItems[0].Remove();
                this._dirty = true;
                this.OnDocumentChanged(EventArgs.Empty);
                int count = this._columnsListView.Items.Count;
                if (count > 0)
                {
                    ListViewItem item;
                    if (num > (count - 1))
                    {
                        item = this._columnsListView.Items[num - 1];
                    }
                    else
                    {
                        item = this._columnsListView.Items[num];
                    }
                    item.Selected = true;
                    item.Focused = true;
                    item.EnsureVisible();
                    this._columnsListView.Focus();
                }
            }
        }

        private Column[] GetColumns()
        {
            int count = this._columnsListView.Items.Count;
            Column[] columnArray = new Column[count];
            for (int i = 0; i < count; i++)
            {
                columnArray[i] = ((ColumnListViewItem) this._columnsListView.Items[i]).Column;
            }
            return columnArray;
        }

        private string GetUniqueColumnName()
        {
            string str;
            Hashtable hashtable = new Hashtable();
            foreach (ColumnListViewItem item in this._columnsListView.Items)
            {
                hashtable[item.Column.Name] = null;
            }
            int num = 0;
            do
            {
                str = "Column" + num;
                num++;
            }
            while (hashtable.Contains(str));
            return str;
        }

        private void InitializeUserInterface()
        {
            this._bottomPanel = new Panel();
            this._columnPropertyGrid = new PropertyGrid();
            this._verticalSplitter = new Splitter();
            this._columnPropertyHelpLabel = new Label();
            this._horizontalSplitter = new Splitter();
            this._topPanel = new Panel();
            this._columnCommandsToolBar = new MxToolBar();
            this._moveColumnUpToolBarButton = new MxToolBarButton();
            this._moveColumnDownToolBarButton = new MxToolBarButton();
            this._deleteColumnToolBarButton = new MxToolBarButton();
            this._newColumnToolBarButton = new MxToolBarButton();
            this._separator1ToolBarButton = new MxToolBarButton();
            this._separator2ToolBarButton = new MxToolBarButton();
            this._columnsListView = new MxListView();
            this._columnNameColumnHeader = new ColumnHeader();
            this._dataTypeColumnHeader = new ColumnHeader();
            this._sizeColumnHeader = new ColumnHeader();
            this._allowNullsColumnHeader = new ColumnHeader();
            this._columnImageList = new ImageList();
            this._errorLabel = new MxInfoLabel();
            this._bottomPanel.SuspendLayout();
            this._topPanel.SuspendLayout();
            base.SuspendLayout();
            this._errorLabel.Dock = DockStyle.Top;
            this._errorLabel.Image = MxInfoLabel.ErrorGlyph;
            this._errorLabel.Location = new Point(0, 0);
            this._errorLabel.Padding = 2;
            this._errorLabel.Size = new Size(660, 160);
            this._errorLabel.Visible = false;
            this._errorLabel.DoubleClick += new EventHandler(this.OnErrorLabelDoubleClick);
            this._bottomPanel.BackColor = SystemColors.Control;
            this._bottomPanel.Controls.AddRange(new Control[] { this._columnPropertyGrid, this._verticalSplitter, this._columnPropertyHelpLabel });
            this._bottomPanel.Dock = DockStyle.Bottom;
            this._bottomPanel.Location = new Point(2, 0xd4);
            this._bottomPanel.Name = "_bottomPanel";
            this._bottomPanel.Size = new Size(660, 200);
            this._bottomPanel.TabIndex = 0;
            this._columnPropertyGrid.CommandsVisibleIfAvailable = false;
            this._columnPropertyGrid.Dock = DockStyle.Fill;
            this._columnPropertyGrid.HelpVisible = false;
            this._columnPropertyGrid.LargeButtons = false;
            this._columnPropertyGrid.LineColor = SystemColors.Control;
            this._columnPropertyGrid.Name = "_columnPropertyGrid";
            this._columnPropertyGrid.PropertySort = PropertySort.Categorized;
            this._columnPropertyGrid.Size = new Size(0x18d, 200);
            this._columnPropertyGrid.TabIndex = 5;
            this._columnPropertyGrid.ToolbarVisible = false;
            this._columnPropertyGrid.ViewBackColor = SystemColors.Window;
            this._columnPropertyGrid.ViewForeColor = SystemColors.WindowText;
            this._columnPropertyGrid.SelectedGridItemChanged += new SelectedGridItemChangedEventHandler(this.OnColumnPropertyGridSelectedGridItemChanged);
            this._columnPropertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.OnColumnPropertyGridPropertyValueChanged);
            this._verticalSplitter.Dock = DockStyle.Right;
            this._verticalSplitter.Location = new Point(0x18d, 0);
            this._verticalSplitter.Name = "_verticalSplitter";
            this._verticalSplitter.Size = new Size(3, 200);
            this._verticalSplitter.TabIndex = 4;
            this._verticalSplitter.TabStop = false;
            this._columnPropertyHelpLabel.BorderStyle = BorderStyle.Fixed3D;
            this._columnPropertyHelpLabel.Dock = DockStyle.Right;
            this._columnPropertyHelpLabel.ForeColor = Color.Blue;
            this._columnPropertyHelpLabel.Location = new Point(400, 0);
            this._columnPropertyHelpLabel.Name = "_columnPropertyHelpLabel";
            this._columnPropertyHelpLabel.Size = new Size(260, 200);
            this._columnPropertyHelpLabel.TabIndex = 3;
            this._columnPropertyHelpLabel.TextAlign = ContentAlignment.MiddleCenter;
            this._horizontalSplitter.Dock = DockStyle.Bottom;
            this._horizontalSplitter.Location = new Point(2, 0xd1);
            this._horizontalSplitter.Name = "_horizontalSplitter";
            this._horizontalSplitter.Size = new Size(660, 3);
            this._horizontalSplitter.TabIndex = 1;
            this._horizontalSplitter.TabStop = false;
            this._topPanel.Controls.AddRange(new Control[] { this._columnsListView, this._columnCommandsToolBar });
            this._topPanel.Dock = DockStyle.Fill;
            this._topPanel.Location = new Point(2, 2);
            this._topPanel.Name = "_topPanel";
            this._topPanel.Size = new Size(660, 0xcf);
            this._topPanel.TabIndex = 2;
            this._columnCommandsToolBar.Appearance = ToolBarAppearance.Flat;
            this._columnCommandsToolBar.Buttons.AddRange(new ToolBarButton[] { this._newColumnToolBarButton, this._separator1ToolBarButton, this._moveColumnUpToolBarButton, this._moveColumnDownToolBarButton, this._separator2ToolBarButton, this._deleteColumnToolBarButton });
            this._columnCommandsToolBar.Divider = false;
            this._columnCommandsToolBar.DropDownArrows = true;
            this._columnCommandsToolBar.ImageList = this._columnImageList;
            this._columnCommandsToolBar.Name = "_columnCommandsToolBar";
            this._columnCommandsToolBar.ShowToolTips = true;
            this._columnCommandsToolBar.TabIndex = 5;
            this._columnCommandsToolBar.TextAlign = ToolBarTextAlign.Right;
            this._columnCommandsToolBar.Wrappable = false;
            this._columnCommandsToolBar.ButtonClick += new ToolBarButtonClickEventHandler(this.OnColumnCommandsToolBarButtonClick);
            this._moveColumnUpToolBarButton.ImageIndex = 2;
            this._moveColumnUpToolBarButton.ToolTipText = "Move Selected Column Up (Ctrl+Up)";
            this._moveColumnDownToolBarButton.ImageIndex = 3;
            this._moveColumnDownToolBarButton.ToolTipText = "Move Selected Column Down (Ctrl+Down)";
            this._deleteColumnToolBarButton.ImageIndex = 5;
            this._deleteColumnToolBarButton.ToolTipText = "Delete Selected Column (Del)";
            this._newColumnToolBarButton.ImageIndex = 4;
            this._newColumnToolBarButton.Text = "New";
            this._newColumnToolBarButton.ToolTipText = "Add New Column (Ins)";
            this._separator1ToolBarButton.Style = ToolBarButtonStyle.Separator;
            this._separator2ToolBarButton.Style = ToolBarButtonStyle.Separator;
            this._columnsListView.Dock = DockStyle.Fill;
            this._columnsListView.Columns.AddRange(new ColumnHeader[] { this._columnNameColumnHeader, this._dataTypeColumnHeader, this._sizeColumnHeader, this._allowNullsColumnHeader });
            this._columnsListView.FullRowSelect = true;
            this._columnsListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this._columnsListView.HideSelection = false;
            this._columnsListView.LabelEdit = true;
            this._columnsListView.LabelWrap = false;
            this._columnsListView.Location = new Point(0, 0x18);
            this._columnsListView.MultiSelect = false;
            this._columnsListView.Name = "_columnsListView";
            this._columnsListView.Size = new Size(0x292, 0x11a);
            this._columnsListView.SmallImageList = this._columnImageList;
            this._columnsListView.TabIndex = 0;
            this._columnsListView.View = View.Details;
            this._columnsListView.AfterLabelEdit += new LabelEditEventHandler(this.OnColumnsListViewAfterLabelEdit);
            this._columnsListView.KeyDown += new KeyEventHandler(this.OnColumnsListViewKeyDown);
            this._columnsListView.SelectedIndexChanged += new EventHandler(this.OnColumnsListViewSelectedIndexChanged);
            this._columnNameColumnHeader.Text = "Column Name";
            this._columnNameColumnHeader.Width = 200;
            this._dataTypeColumnHeader.Text = "Data Type";
            this._dataTypeColumnHeader.Width = 100;
            this._sizeColumnHeader.Text = "Size";
            this._sizeColumnHeader.Width = 100;
            this._allowNullsColumnHeader.Text = "Allow Nulls";
            this._allowNullsColumnHeader.Width = 0x4b;
            this._columnImageList.ColorDepth = ColorDepth.Depth8Bit;
            this._columnImageList.ImageSize = new Size(0x10, 0x10);
            this._columnImageList.TransparentColor = Color.FromArgb(0xff, 0, 0xff, 0);
            this._columnImageList.Images.AddStrip(new Bitmap(typeof(TableView), "TableDesignImages.bmp"));
            this.BackColor = SystemColors.Control;
            base.ClientSize = new Size(0x298, 0x19e);
            base.Controls.AddRange(new Control[] { this._topPanel, this._horizontalSplitter, this._bottomPanel, this._errorLabel });
            this.Font = new Font("Tahoma", 8f);
            this._bottomPanel.ResumeLayout(false);
            this._topPanel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void LoadDocumentItems()
        {
            try
            {
                this._isReadOnly = this._document.Table.GetRowCount() > 0;
                if (this._isReadOnly)
                {
                    this._errorLabel.Visible = true;
                    this._errorLabel.Image = MxInfoLabel.WarningGlyph;
                    this._errorLabel.Text = "The design of this table cannot be edited because it already contains data.";
                }
                else
                {
                    this._errorLabel.Visible = false;
                }
                this._columnsListView.Items.Clear();
                foreach (ICloneable cloneable in this._document.Table.Columns)
                {
                    this._columnsListView.Items.Add(new ColumnListViewItem((Column) cloneable.Clone(), this._isReadOnly));
                }
                if (this._columnsListView.Items.Count > 0)
                {
                    ListViewItem item = this._columnsListView.Items[0];
                    item.Selected = true;
                    item.Focused = true;
                    item.EnsureVisible();
                    this._columnsListView.Focus();
                }
                this._bottomPanel.Visible = true;
                this._topPanel.Visible = true;
                this._horizontalSplitter.Visible = true;
                this._dirty = false;
                this._loadError = false;
            }
            catch (Exception exception)
            {
                this._loadError = true;
                this._errorLabel.Visible = true;
                this._errorLabel.Image = MxInfoLabel.ErrorGlyph;
                this._errorLabel.Text = string.Format(errorFormatString, exception.Message);
                this._bottomPanel.Visible = false;
                this._topPanel.Visible = false;
                this._horizontalSplitter.Visible = false;
            }
        }

        void IDocumentView.Activate(bool viewSwitch)
        {
            ISelectionService service = (ISelectionService) this._serviceProvider.GetService(typeof(ISelectionService));
            if (service != null)
            {
                service.SetSelectedComponents(null);
            }
            base.Focus();
        }

        void IDocumentView.Deactivate(bool closing)
        {
        }

        void IDocumentView.LoadFromDocument(Document document)
        {
            this._document = (TableDocument) document;
            this.UpdateEnabledUI();
            this.LoadDocumentItems();
        }

        bool IDocumentView.SaveToDocument()
        {
            bool flag3;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                IMxUIService service = (IMxUIService) this._serviceProvider.GetService(typeof(IMxUIService));
                Column[] columns = this.GetColumns();
                if (columns.Length == 0)
                {
                    service.ReportError("The table must contain at least one column.", "Cannot save table.", false);
                    return false;
                }
                Hashtable hashtable = new Hashtable();
                bool flag = false;
                foreach (Column column in columns)
                {
                    flag |= column.InPrimaryKey;
                    if (hashtable.Contains(column.Name))
                    {
                        service.ReportError("Duplicate column names exist: " + column.Name + "\r\nPlease correct this and then try to save again.", "Cannot save table.", false);
                        return false;
                    }
                    hashtable.Add(column.Name, null);
                }
                if (!flag && (service.ShowMessage("The table does not contain a primary key. Table data cannot be edited in this tool if they do not contain a primary key. Would you like to save the table anyway?", "Save Table Design", MessageBoxIcon.Exclamation, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2) == DialogResult.No))
                {
                    flag3 = false;
                }
                else
                {
                    try
                    {
                        if (this._document.Table.CheckForDirtyKeys(columns) && (service.ShowMessage("One or more columns participating in either a foreign key table relationship or a multi-column unique key have been changed. If you continue saving, some of these relationships will be lost. Are you sure you want to continue?", "Save Table Design", MessageBoxIcon.Exclamation, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2) == DialogResult.No))
                        {
                            return false;
                        }
                        this._document.Table.UpdateColumns(columns);
                        this._dirty = false;
                        this.OnDocumentChanged(EventArgs.Empty);
                        flag3 = true;
                    }
                    catch (Exception exception)
                    {
                        service.ReportError(exception, "There was an error saving the design of this table. No changes were made.", false);
                        flag3 = false;
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            return flag3;
        }

        bool ICommandHandler.HandleCommand(Microsoft.Matrix.UIComponents.Command command)
        {
            bool flag = false;
            if ((command.CommandGroup == typeof(GlobalCommands)) && (command.CommandID == 4))
            {
                flag = true;
            }
            return flag;
        }

        bool ICommandHandler.UpdateCommand(Microsoft.Matrix.UIComponents.Command command)
        {
            bool flag = false;
            if ((command.CommandGroup == typeof(GlobalCommands)) && (command.CommandID == 4))
            {
                command.Enabled = false;
                flag = true;
            }
            return flag;
        }

        private void MoveSelectedColumnDown()
        {
            if ((this._columnsListView.SelectedIndices.Count == 1) && (this._columnsListView.SelectedIndices[0] < (this._columnsListView.Items.Count - 1)))
            {
                int num = this._columnsListView.SelectedIndices[0];
                ColumnListViewItem item = (ColumnListViewItem) this._columnsListView.Items[num];
                item.Remove();
                this._columnsListView.Items.Insert(num + 1, item);
                item.Selected = true;
                item.Focused = true;
                item.EnsureVisible();
                this._columnsListView.Focus();
                this._dirty = true;
                this.OnDocumentChanged(EventArgs.Empty);
            }
        }

        private void MoveSelectedColumnUp()
        {
            if ((this._columnsListView.SelectedIndices.Count == 1) && (this._columnsListView.SelectedIndices[0] > 0))
            {
                int num = this._columnsListView.SelectedIndices[0];
                ColumnListViewItem item = (ColumnListViewItem) this._columnsListView.Items[num];
                item.Remove();
                this._columnsListView.Items.Insert(num - 1, item);
                item.Selected = true;
                item.Focused = true;
                item.EnsureVisible();
                this._columnsListView.Focus();
                this._dirty = true;
                this.OnDocumentChanged(EventArgs.Empty);
            }
        }

        private void OnColumnCommandsToolBarButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            ToolBarButton button = e.Button;
            if (button == this._moveColumnUpToolBarButton)
            {
                this.MoveSelectedColumnUp();
            }
            else if (button == this._moveColumnDownToolBarButton)
            {
                this.MoveSelectedColumnDown();
            }
            else if (button == this._deleteColumnToolBarButton)
            {
                this.DeleteSelectedColumn();
            }
            else if (button == this._newColumnToolBarButton)
            {
                this.AddNewColumn();
            }
        }

        private void OnColumnPropertyGridPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (this._columnsListView.SelectedIndices.Count == 1)
            {
                ((ColumnListViewItem) this._columnsListView.SelectedItems[0]).Refresh();
            }
            this._dirty = true;
            this.OnDocumentChanged(EventArgs.Empty);
        }

        private void OnColumnPropertyGridSelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            GridItem newSelection = e.NewSelection;
            if ((newSelection != null) && (newSelection.PropertyDescriptor != null))
            {
                DescriptionAttribute attribute = (DescriptionAttribute) newSelection.PropertyDescriptor.Attributes[typeof(DescriptionAttribute)];
                if (attribute != null)
                {
                    this._columnPropertyHelpLabel.Text = attribute.Description;
                }
                else
                {
                    this._columnPropertyHelpLabel.Text = string.Empty;
                }
            }
            else
            {
                this._columnPropertyHelpLabel.Text = string.Empty;
            }
        }

        private void OnColumnsListViewAfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            ColumnListViewItem item = (ColumnListViewItem) this._columnsListView.Items[e.Item];
            Column column = item.Column;
            if ((column.Name != e.Label) && (e.Label != null))
            {
                try
                {
                    column.Name = e.Label;
                    this._columnPropertyGrid.Refresh();
                    this._dirty = true;
                    this.OnDocumentChanged(EventArgs.Empty);
                    item.Refresh();
                }
                catch (Exception exception)
                {
                    e.CancelEdit = true;
                    ((IMxUIService) this._serviceProvider.GetService(typeof(IMxUIService))).ReportError(exception, "Error in column name.", false);
                }
            }
        }

        private void OnColumnsListViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                if (!this._isReadOnly)
                {
                    this.DeleteSelectedColumn();
                }
                e.Handled = true;
            }
            else if (e.KeyData == Keys.Insert)
            {
                if (!this._isReadOnly)
                {
                    this.AddNewColumn();
                }
                e.Handled = true;
            }
            else if ((e.KeyCode == Keys.Up) && (e.Modifiers == Keys.Control))
            {
                if (!this._isReadOnly)
                {
                    this.MoveSelectedColumnUp();
                }
                e.Handled = true;
            }
            else if ((e.KeyCode == Keys.Down) && (e.Modifiers == Keys.Control))
            {
                if (!this._isReadOnly)
                {
                    this.MoveSelectedColumnDown();
                }
                e.Handled = true;
            }
            else if ((e.KeyCode == Keys.G) && (e.Modifiers == (Keys.Alt | Keys.Control | Keys.Shift)))
            {
                this._columnsListView.GridLines = !this._columnsListView.GridLines;
                e.Handled = true;
            }
        }

        private void OnColumnsListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._columnsListView.SelectedIndices.Count == 1)
            {
                this._columnPropertyGrid.SelectedObject = ((ColumnListViewItem) this._columnsListView.SelectedItems[0]).Column;
            }
            else
            {
                this._columnPropertyGrid.SelectedObject = null;
                this._columnPropertyHelpLabel.Text = string.Empty;
            }
            this.UpdateEnabledUI();
        }

        private void OnDocumentChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[DocumentChangedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnErrorLabelDoubleClick(object sender, EventArgs e)
        {
            if (this._loadError)
            {
                ((IDocumentView) this).LoadFromDocument(this._document);
            }
        }

        private void UpdateEnabledUI()
        {
            if (this._isReadOnly)
            {
                this._moveColumnUpToolBarButton.Enabled = false;
                this._moveColumnDownToolBarButton.Enabled = false;
                this._deleteColumnToolBarButton.Enabled = false;
                this._newColumnToolBarButton.Enabled = false;
                this._columnsListView.LabelEdit = false;
            }
            else
            {
                this._newColumnToolBarButton.Enabled = true;
                this._columnsListView.LabelEdit = true;
                if (this._columnsListView.SelectedIndices.Count == 1)
                {
                    this._moveColumnUpToolBarButton.Enabled = this._columnsListView.SelectedIndices[0] > 0;
                    this._moveColumnDownToolBarButton.Enabled = this._columnsListView.SelectedIndices[0] < (this._columnsListView.Items.Count - 1);
                    this._deleteColumnToolBarButton.Enabled = true;
                }
                else
                {
                    this._moveColumnUpToolBarButton.Enabled = false;
                    this._moveColumnDownToolBarButton.Enabled = false;
                    this._deleteColumnToolBarButton.Enabled = false;
                }
            }
        }

        bool IDocumentView.CanDeactivate
        {
            get
            {
                if (!this._dirty)
                {
                    return true;
                }
                IMxUIService service = (IMxUIService) this._serviceProvider.GetService(typeof(IMxUIService));
                switch (service.ShowMessage("The design of this table must be saved before this view can be closed. Would you like to save the design?", "Save Table Design", MessageBoxIcon.Question, MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        return ((IDocumentView) this).SaveToDocument();

                    case DialogResult.No:
                        this._dirty = false;
                        this.OnDocumentChanged(EventArgs.Empty);
                        return true;
                }
                return false;
            }
        }

        bool IDocumentView.IsDirty
        {
            get
            {
                return this._dirty;
            }
        }

        Image IDocumentView.ViewImage
        {
            get
            {
                if (this._viewImage == null)
                {
                    this._viewImage = new Bitmap(typeof(TableView), "TableView.bmp");
                    this._viewImage.MakeTransparent();
                }
                return this._viewImage;
            }
        }

        string IDocumentView.ViewName
        {
            get
            {
                return "Design";
            }
        }

        DocumentViewType IDocumentView.ViewType
        {
            get
            {
                return DocumentViewType.Design;
            }
        }

        bool IPropertyBrowserClient.SupportsPropertyBrowser
        {
            get
            {
                return true;
            }
        }

        private class ColumnListViewItem : ListViewItem
        {
            private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column _column;

            public ColumnListViewItem(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column column) : this(column, false)
            {
            }

            public ColumnListViewItem(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column column, bool readOnly)
            {
                this._column = column;
                this._column.IsReadOnly = readOnly;
                this.Refresh();
            }

            public void Refresh()
            {
                base.SubItems.Clear();
                base.Text = this._column.Name;
                base.SubItems.AddRange(new string[] { this._column.DataType, this._column.Size.ToString(), this._column.AllowNulls.ToString() });
                base.ImageIndex = this._column.InPrimaryKey ? 1 : 0;
            }

            public Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column Column
            {
                get
                {
                    return this._column;
                }
            }
        }
    }
}

