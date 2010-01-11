namespace Microsoft.Matrix.Packages.DBAdmin.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class DataToolWindow : ToolWindow
    {
        private MxToolBarButton _closeDatabaseButton;
        private MxToolBarButton _deleteButton;
        private MxToolBarButton _newDatabaseButton;
        private MxToolBarButton _newItemButton;
        private MxToolBarButton _openButton;
        private MxToolBarButton _refreshButton;
        private MxToolBar _toolBar;
        private MxTreeView _treeView;
        internal const int ConnectIconIndex = 0;
        internal const int DatabaseIconIndex = 6;
        internal const int DeleteIconIndex = 4;
        internal const int DisconnectIconIndex = 1;
        internal const int FolderIconIndex = 9;
        internal const int NewItemIconIndex = 5;
        internal const int OpenFolderIconIndex = 10;
        internal const int OpenIconIndex = 2;
        internal const int RefreshIconIndex = 3;
        internal const int StoredProcedureIconIndex = 8;
        internal const int TableIconIndex = 7;

        public DataToolWindow(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
            base.Icon = new Icon(typeof(DataToolWindow), "DataToolWindow.ico");
            this.Text = "Data";
            IProjectManager service = (IProjectManager) this.GetService(typeof(IProjectManager));
            if (service != null)
            {
                service.ProjectAdded += new ProjectEventHandler(this.OnProjectAdded);
                service.ProjectClosed += new ProjectEventHandler(this.OnProjectClosed);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IProjectManager service = (IProjectManager) this.GetService(typeof(IProjectManager));
                if (service != null)
                {
                    service.ProjectAdded -= new ProjectEventHandler(this.OnProjectAdded);
                    service.ProjectClosed -= new ProjectEventHandler(this.OnProjectClosed);
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            Panel panel = new Panel();
            ImageList list = new ImageList();
            ToolBarButton button = new MxToolBarButton();
            ToolBarButton button2 = new MxToolBarButton();
            ToolBarButton button3 = new MxToolBarButton();
            this._toolBar = new MxToolBar();
            this._newDatabaseButton = new MxToolBarButton();
            this._closeDatabaseButton = new MxToolBarButton();
            this._refreshButton = new MxToolBarButton();
            this._deleteButton = new MxToolBarButton();
            this._newItemButton = new MxToolBarButton();
            this._openButton = new MxToolBarButton();
            this._treeView = new MxTreeView();
            panel.SuspendLayout();
            base.SuspendLayout();
            list.ImageSize = new Size(0x10, 0x10);
            list.TransparentColor = Color.Lime;
            list.ColorDepth = ColorDepth.Depth32Bit;
            list.Images.AddStrip(new Bitmap(typeof(DataToolWindow), "DataToolBar.bmp"));
            this._toolBar.Appearance = ToolBarAppearance.Flat;
            this._toolBar.Divider = false;
            this._toolBar.DropDownArrows = true;
            this._toolBar.ShowToolTips = true;
            this._toolBar.TabIndex = 0;
            this._toolBar.TextAlign = ToolBarTextAlign.Right;
            this._toolBar.Wrappable = false;
            this._toolBar.ImageList = list;
            this._toolBar.ButtonClick += new ToolBarButtonClickEventHandler(this.OnToolBarButtonClick);
            this._newDatabaseButton.ImageIndex = 0;
            this._newDatabaseButton.ToolTipText = "Add Database Connection";
            this._closeDatabaseButton.ImageIndex = 1;
            this._closeDatabaseButton.ToolTipText = "Close Database Connection";
            button.Style = ToolBarButtonStyle.Separator;
            button2.Style = ToolBarButtonStyle.Separator;
            button3.Style = ToolBarButtonStyle.Separator;
            this._refreshButton.ImageIndex = 3;
            this._refreshButton.ToolTipText = "Refresh";
            this._deleteButton.ImageIndex = 4;
            this._deleteButton.ToolTipText = "Delete";
            this._newItemButton.ImageIndex = 5;
            this._newItemButton.ToolTipText = "New Database Object";
            this._openButton.ImageIndex = 2;
            this._openButton.ToolTipText = "Open";
            this._toolBar.Buttons.AddRange(new ToolBarButton[] { this._openButton, button, this._newItemButton, this._deleteButton, button2, this._refreshButton, button3, this._newDatabaseButton, this._closeDatabaseButton });
            this._treeView.AllowDrop = true;
            this._treeView.BorderStyle = BorderStyle.None;
            this._treeView.Dock = DockStyle.Fill;
            this._treeView.HideSelection = false;
            this._treeView.TabIndex = 0;
            this._treeView.ImageList = list;
            this._treeView.AfterSelect += new TreeViewEventHandler(this.OnAfterSelectTreeView);
            this._treeView.BeforeExpand += new TreeViewCancelEventHandler(this.OnBeforeExpandTreeView);
            this._treeView.NodeDoubleClick += new TreeViewEventHandler(this.OnDoubleClickTreeView);
            this._treeView.ItemDrag += new ItemDragEventHandler(this.OnItemDragTreeView);
            panel.BackColor = SystemColors.ControlDark;
            panel.Controls.Add(this._treeView);
            panel.Dock = DockStyle.Fill;
            panel.DockPadding.All = 1;
            panel.TabIndex = 1;
            base.Controls.AddRange(new Control[] { panel, this._toolBar });
            panel.ResumeLayout(false);
            base.ResumeLayout(false);
            this.UpdateToolBarButtons(null);
        }

        private void OnAfterSelectTreeView(object sender, TreeViewEventArgs e)
        {
            this.UpdateToolBarButtons((DataTreeNode) e.Node);
        }

        private void OnBeforeExpandTreeView(object sender, TreeViewCancelEventArgs e)
        {
            DataTreeNode node = (DataTreeNode) e.Node;
            TreeNodeCollection nodes = node.Nodes;
            if ((nodes.Count == 1) && (nodes[0] is DummyTreeNode))
            {
                nodes.Clear();
                Cursor cursor = this._treeView.Cursor;
                try
                {
                    if (node.SupportsCommand(DataCommand.CreateChildren))
                    {
                        this._treeView.Cursor = Cursors.WaitCursor;
                        node.ProcessCommand(DataCommand.CreateChildren);
                    }
                }
                catch (Exception exception)
                {
                    ((IMxUIService) base.ServiceProvider.GetService(typeof(IMxUIService))).ReportError(exception.Message, "Couldn't get nodes", false);
                }
                finally
                {
                    this._treeView.Cursor = cursor;
                }
            }
        }

        private void OnDoubleClickTreeView(object sender, TreeViewEventArgs e)
        {
            DataTreeNode node = e.Node as DataTreeNode;
            if (node != null)
            {
                Cursor cursor = this._treeView.Cursor;
                try
                {
                    if (node.SupportsCommand(DataCommand.DoubleClick))
                    {
                        this._treeView.Cursor = Cursors.WaitCursor;
                        node.ProcessCommand(DataCommand.DoubleClick);
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    this._treeView.Cursor = cursor;
                }
            }
        }

        private void OnItemDragTreeView(object sender, ItemDragEventArgs e)
        {
            IDesignerEventService service = (IDesignerEventService) this.GetService(typeof(IDesignerEventService));
            if (service != null)
            {
                IDesignerHost activeDesigner = service.ActiveDesigner;
                DataObject dataObject = ((DataTreeNode) e.Item).GetDataObject(activeDesigner);
                if (dataObject != null)
                {
                    base.DoDragDrop(dataObject, DragDropEffects.Copy);
                }
            }
        }

        private void OnProjectAdded(object sender, ProjectEventArgs e)
        {
            if (e.Project is DatabaseProject)
            {
                RootProjectItem projectItem = e.Project.ProjectItem;
                this._treeView.Nodes.Add(new DatabaseProjectTreeNode(projectItem));
                if (this._treeView.Nodes.Count == 1)
                {
                    this._treeView.SelectedNode = this._treeView.Nodes[0];
                }
            }
        }

        private void OnProjectClosed(object sender, ProjectEventArgs e)
        {
            if (e.Project is DatabaseProject)
            {
                TreeNode itemNode = e.Project.ProjectItem.ItemNode;
                if (itemNode != null)
                {
                    itemNode.Remove();
                }
            }
        }

        private void OnToolBarButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            DataTreeNode selectedNode = this._treeView.SelectedNode as DataTreeNode;
            ToolBarButton button = e.Button;
            Cursor cursor = this._treeView.Cursor;
            try
            {
                this._treeView.Cursor = Cursors.WaitCursor;
                if (button == this._newDatabaseButton)
                {
                    ((IProjectManager) this.GetService(typeof(IProjectManager))).CreateProject(typeof(DatabaseProject));
                }
                else if (button == this._closeDatabaseButton)
                {
                    if (selectedNode.SupportsCommand(DataCommand.Disconnect))
                    {
                        selectedNode.ProcessCommand(DataCommand.Disconnect);
                        this.UpdateToolBarButtons((DataTreeNode) this._treeView.SelectedNode);
                    }
                }
                else
                {
                    if (button == this._refreshButton)
                    {
                        if (!selectedNode.SupportsCommand(DataCommand.Refresh))
                        {
                            return;
                        }
                        try
                        {
                            this._treeView.BeginUpdate();
                            selectedNode.ProcessCommand(DataCommand.Refresh);
                            return;
                        }
                        finally
                        {
                            this._treeView.EndUpdate();
                        }
                    }
                    if (button == this._deleteButton)
                    {
                        if (selectedNode.SupportsCommand(DataCommand.Delete))
                        {
                            selectedNode.ProcessCommand(DataCommand.Delete);
                            this.UpdateToolBarButtons((DataTreeNode) this._treeView.SelectedNode);
                        }
                    }
                    else if (button == this._newItemButton)
                    {
                        if (selectedNode.SupportsCommand(DataCommand.AddItem))
                        {
                            selectedNode.ProcessCommand(DataCommand.AddItem);
                        }
                    }
                    else if (button == this._openButton)
                    {
                        this.OpenDocument((DocumentProjectItem) selectedNode.ProjectItem, DocumentViewType.Default);
                    }
                }
            }
            finally
            {
                this._treeView.Cursor = cursor;
            }
        }

        private void OpenDocument(DocumentProjectItem projectItem, DocumentViewType viewType)
        {
            IDocumentManager service = (IDocumentManager) base.ServiceProvider.GetService(typeof(IDocumentManager));
            if (service != null)
            {
                service.OpenDocument(projectItem, false, viewType);
            }
        }

        private void UpdateToolBarButtons(DataTreeNode node)
        {
            if (node == null)
            {
                this._deleteButton.Enabled = false;
                this._refreshButton.Enabled = false;
                this._newItemButton.Enabled = false;
                this._openButton.Enabled = false;
                this._closeDatabaseButton.Enabled = false;
            }
            else
            {
                this._deleteButton.Enabled = node.SupportsCommand(DataCommand.Delete);
                this._refreshButton.Enabled = node.SupportsCommand(DataCommand.Refresh);
                this._newItemButton.Enabled = node.SupportsCommand(DataCommand.AddItem);
                this._closeDatabaseButton.Enabled = node.SupportsCommand(DataCommand.Disconnect);
                if ((node is TableTreeNode) || (node is StoredProcedureTreeNode))
                {
                    this._openButton.Enabled = true;
                }
                else
                {
                    this._openButton.Enabled = false;
                }
            }
        }
    }
}

