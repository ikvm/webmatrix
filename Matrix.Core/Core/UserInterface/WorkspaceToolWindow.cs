namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.Projects.FileSystem;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class WorkspaceToolWindow : ToolWindow, ICommandHandler, ICommandHandlerWithContext
    {
        private Project _activeProject;
        private ToolBarButton _deleteButton;
        private int _documentIconOffset;
        private ProjectTreeNode _dragNode;
        private ProjectTreeNode _dropNode;
        private bool _firstProject;
        private Project _myComputerProject;
        private ToolBarButton _newDocumentButton;
        private ToolBarButton _newFolderButton;
        private ToolBarButton _newProjectButton;
        private ToolBarButton _openButton;
        private ToolBarButton _refreshButton;
        private ToolBar _toolBar;
        private MxTreeView _treeView;

        public WorkspaceToolWindow(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
            IProjectManager service = (IProjectManager) this.GetService(typeof(IProjectManager));
            if (service != null)
            {
                service.ProjectAdded += new ProjectEventHandler(this.OnProjectAdded);
                service.ProjectClosed += new ProjectEventHandler(this.OnProjectClosed);
            }
            ICommandManager manager2 = (ICommandManager) this.GetService(typeof(ICommandManager));
            if (manager2 != null)
            {
                manager2.AddGlobalCommandHandler(this);
            }
            this._firstProject = true;
        }

        private void AddDocument(ProjectTreeNode node)
        {
            FolderProjectItem projectItem = (FolderProjectItem) node.ProjectItem;
            DocumentProjectItem newItem = null;
            try
            {
                newItem = projectItem.AddDocument();
            }
            catch (Exception exception)
            {
                ((IMxUIService) base.ServiceProvider.GetService(typeof(IMxUIService))).ReportError(exception.Message, "Add Document", false);
            }
            if (newItem != null)
            {
                this.AddNewItem(node, newItem);
                this.OpenNode((ProjectTreeNode) newItem.ItemNode);
            }
        }

        private void AddFolder(ProjectTreeNode node)
        {
            FolderProjectItem projectItem = (FolderProjectItem) node.ProjectItem;
            FolderProjectItem newItem = null;
            try
            {
                newItem = projectItem.AddFolder();
            }
            catch (Exception exception)
            {
                ((IMxUIService) base.ServiceProvider.GetService(typeof(IMxUIService))).ReportError(exception.Message, "Add Folder", false);
            }
            if (newItem != null)
            {
                this.AddNewItem(node, newItem);
            }
        }

        private void AddNewItem(ProjectTreeNode parentNode, ProjectItem newItem)
        {
            TreeNodeCollection nodes = parentNode.Nodes;
            if ((nodes.Count == 1) && (nodes[0].GetType() == typeof(TreeNode)))
            {
                parentNode.Expand();
                this._treeView.SelectedNode = newItem.ItemNode;
            }
            else
            {
                ProjectTreeNode node = this.CreateProjectTreeNode(newItem, newItem is FolderProjectItem);
                IComparer projectItemComparer = newItem.Project.ProjectItemComparer;
                if (projectItemComparer == null)
                {
                    nodes.Add(node);
                }
                else
                {
                    int index = 0;
                    while (index < nodes.Count)
                    {
                        if (projectItemComparer.Compare(newItem, ((ProjectTreeNode) nodes[index]).ProjectItem) < 0)
                        {
                            break;
                        }
                        index++;
                    }
                    nodes.Insert(index, node);
                }
                this._treeView.SelectedNode = node;
            }
            this._treeView.SelectedNode.EnsureVisible();
        }

        private void AddProject()
        {
            IProjectManager service = (IProjectManager) this.GetService(typeof(IProjectManager));
            if (service != null)
            {
                service.CreateProject(typeof(WorkspaceProject));
            }
        }

        private ProjectTreeNode CreateProjectTreeNode(ProjectItem item, bool addDummyNode)
        {
            int iconIndex = item.IconIndex;
            int selectedImageIndex = iconIndex;
            switch (iconIndex)
            {
                case 7:
                {
                    DocumentProjectItem item2 = item as DocumentProjectItem;
                    if (item2 != null)
                    {
                        DocumentType documentType = item2.DocumentType;
                        if (documentType != null)
                        {
                            int num3 = documentType.IconIndex;
                            iconIndex = this._documentIconOffset + num3;
                            selectedImageIndex = iconIndex;
                        }
                    }
                    break;
                }
                case 1:
                    selectedImageIndex = 2;
                    break;
            }
            return new ProjectTreeNode(item, iconIndex, selectedImageIndex, addDummyNode);
        }

        private void DeleteNode(ProjectTreeNode node)
        {
            ProjectItem projectItem = node.ProjectItem;
            if (projectItem is RootProjectItem)
            {
                IProjectManager manager = (IProjectManager) this.GetService(typeof(IProjectManager));
                if (manager != null)
                {
                    manager.CloseProject(projectItem.Project);
                }
            }
            else
            {
                IMxUIService service = (IMxUIService) base.ServiceProvider.GetService(typeof(IMxUIService));
                if (service.ShowMessage("Are you sure you want to delete '" + projectItem.Caption + "'?", "Delete Workspace Item", MessageBoxIcon.Question, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (projectItem.Delete())
                    {
                        ProjectTreeNode parent = (ProjectTreeNode) node.Parent;
                        if (parent != null)
                        {
                            parent.Nodes.Remove(node);
                        }
                        else
                        {
                            this._treeView.Nodes.Remove(node);
                        }
                    }
                    else
                    {
                        service.ReportError("Unable to delete '" + projectItem.Caption + ".'\r\nCheck that the document or folder can be deleted.", "Delete Workspace Item", true);
                    }
                }
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
                ICommandManager manager2 = (ICommandManager) this.GetService(typeof(ICommandManager));
                if (manager2 != null)
                {
                    manager2.RemoveGlobalCommandHandler(this);
                }
                this._myComputerProject = null;
                this._activeProject = null;
            }
            base.Dispose(disposing);
        }

        private DragDropEffects GetTreeViewDragDropEffects(DragEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            p = this._treeView.PointToClient(p);
            ProjectTreeNode nodeAt = this._treeView.GetNodeAt(p) as ProjectTreeNode;
            if ((nodeAt == null) || !nodeAt.ProjectItem.IsDropTarget)
            {
                return DragDropEffects.None;
            }
            if (this._dragNode != null)
            {
                if (this._dragNode.Parent == nodeAt)
                {
                    return DragDropEffects.None;
                }
                for (ProjectTreeNode node2 = nodeAt; node2 != null; node2 = (ProjectTreeNode) node2.Parent)
                {
                    if (this._dragNode == node2)
                    {
                        return DragDropEffects.None;
                    }
                }
            }
            return nodeAt.ProjectItem.Project.GetProjectItemDragDropEffects(nodeAt.ProjectItem, e.Data);
        }

        private void InitializeComponent()
        {
            Panel panel = new Panel();
            ImageList list = new ImageList();
            ImageList list2 = new ImageList();
            ToolBarButton button = new MxToolBarButton();
            ToolBarButton button2 = new MxToolBarButton();
            ToolBarButton button3 = new MxToolBarButton();
            this._toolBar = new MxToolBar();
            this._openButton = new MxToolBarButton();
            this._refreshButton = new MxToolBarButton();
            this._deleteButton = new MxToolBarButton();
            this._newFolderButton = new MxToolBarButton();
            this._newDocumentButton = new MxToolBarButton();
            this._newProjectButton = new MxToolBarButton();
            this._treeView = new MxTreeView();
            panel.SuspendLayout();
            base.SuspendLayout();
            list2.ImageSize = new Size(0x10, 0x10);
            list2.TransparentColor = Color.Red;
            list2.ColorDepth = ColorDepth.Depth32Bit;
            list2.Images.AddStrip(new Bitmap(typeof(WorkspaceToolWindow), "WorkspaceToolBar.bmp"));
            this._toolBar.Appearance = ToolBarAppearance.Flat;
            this._toolBar.Divider = false;
            this._toolBar.DropDownArrows = true;
            this._toolBar.ShowToolTips = true;
            this._toolBar.TabIndex = 0;
            this._toolBar.TextAlign = ToolBarTextAlign.Right;
            this._toolBar.Wrappable = false;
            this._toolBar.ImageList = list2;
            this._toolBar.ButtonClick += new ToolBarButtonClickEventHandler(this.OnToolBarButtonClick);
            button.Style = ToolBarButtonStyle.Separator;
            button2.Style = ToolBarButtonStyle.Separator;
            button3.Style = ToolBarButtonStyle.Separator;
            this._openButton.ImageIndex = 0;
            this._openButton.ToolTipText = "Open File";
            this._openButton.Enabled = false;
            this._refreshButton.ImageIndex = 2;
            this._refreshButton.ToolTipText = "Refresh";
            this._refreshButton.Enabled = false;
            this._deleteButton.ImageIndex = 1;
            this._deleteButton.ToolTipText = "Delete";
            this._deleteButton.Enabled = false;
            this._newFolderButton.ImageIndex = 3;
            this._newFolderButton.ToolTipText = "New Folder";
            this._newFolderButton.Enabled = false;
            this._newDocumentButton.ImageIndex = 4;
            this._newDocumentButton.ToolTipText = "New File";
            this._newDocumentButton.Enabled = false;
            this._newProjectButton.ImageIndex = 5;
            this._newProjectButton.ToolTipText = "New Project";
            this._toolBar.Buttons.AddRange(new ToolBarButton[] { this._openButton, button, this._newDocumentButton, this._newFolderButton, this._deleteButton, button2, this._refreshButton, button3, this._newProjectButton });
            list.ImageSize = new Size(0x10, 0x10);
            list.TransparentColor = Color.Fuchsia;
            list.ColorDepth = ColorDepth.Depth32Bit;
            list.Images.AddStrip(new Bitmap(typeof(WorkspaceToolWindow), "WorkspaceTreeItems.bmp"));
            this._treeView.AllowDrop = true;
            this._treeView.BorderStyle = BorderStyle.None;
            this._treeView.Dock = DockStyle.Fill;
            this._treeView.HideSelection = false;
            this._treeView.ShowLines = false;
            this._treeView.TabIndex = 0;
            this._treeView.ImageList = list;
            this._treeView.AfterSelect += new TreeViewEventHandler(this.OnAfterSelectTreeView);
            this._treeView.AfterCollapse += new TreeViewEventHandler(this.OnAfterCollapseTreeView);
            this._treeView.BeforeExpand += new TreeViewCancelEventHandler(this.OnBeforeExpandTreeView);
            this._treeView.NodeDoubleClick += new TreeViewEventHandler(this.OnDoubleClickTreeView);
            this._treeView.ShowContextMenu += new ShowContextMenuEventHandler(this.OnShowContextMenuTreeView);
            this._treeView.ItemDrag += new ItemDragEventHandler(this.OnItemDragTreeView);
            this._treeView.DragDrop += new DragEventHandler(this.OnDragDropTreeView);
            this._treeView.DragEnter += new DragEventHandler(this.OnDragEnterTreeView);
            this._treeView.DragOver += new DragEventHandler(this.OnDragOverTreeView);
            panel.BackColor = SystemColors.ControlDark;
            panel.Controls.Add(this._treeView);
            panel.Dock = DockStyle.Fill;
            panel.DockPadding.All = 1;
            panel.TabIndex = 1;
            base.Controls.AddRange(new Control[] { panel, this._toolBar });
            this.Text = "Workspace";
            base.Icon = new Icon(typeof(WorkspaceToolWindow), "WorkspaceToolWindow.ico");
            panel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        bool ICommandHandler.HandleCommand(Command command)
        {
            return ((ICommandHandlerWithContext) this).HandleCommand(command, this._treeView.SelectedNode);
        }

        bool ICommandHandler.UpdateCommand(Command command)
        {
            return ((ICommandHandlerWithContext) this).UpdateCommand(command, this._treeView.SelectedNode);
        }

        bool ICommandHandlerWithContext.HandleCommand(Command command, object context)
        {
            ProjectTreeNode node = context as ProjectTreeNode;
            if (node == null)
            {
                return false;
            }
            ProjectItem projectItem = node.ProjectItem;
            if (projectItem == null)
            {
                return false;
            }
            bool flag = false;
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 710:
                        this.OpenNode(node);
                        flag = true;
                        break;

                    case 0x2c7:
                        this.AddDocument(node);
                        flag = true;
                        break;

                    case 0x2c8:
                        this.AddFolder(node);
                        flag = true;
                        break;

                    case 0x2c9:
                        this.DeleteNode(node);
                        flag = true;
                        break;

                    case 0x2ca:
                        this.RefreshNode(node);
                        flag = true;
                        break;
                }
            }
            if (!flag)
            {
                if (this._activeProject != null)
                {
                    flag = ((ICommandHandlerWithContext) this._activeProject).HandleCommand(command, projectItem);
                }
                if ((!flag && (this._activeProject != this._myComputerProject)) && (this._myComputerProject != null))
                {
                    flag = ((ICommandHandlerWithContext) this._myComputerProject).HandleCommand(command, projectItem);
                }
            }
            return flag;
        }

        bool ICommandHandlerWithContext.UpdateCommand(Command command, object context)
        {
            ProjectTreeNode node = context as ProjectTreeNode;
            if (node == null)
            {
                return false;
            }
            ProjectItem projectItem = node.ProjectItem;
            if (projectItem == null)
            {
                return false;
            }
            bool flag = false;
            if (command.CommandGroup == typeof(GlobalCommands))
            {
                switch (command.CommandID)
                {
                    case 710:
                        command.Enabled = (projectItem is DocumentProjectItem) && (((DocumentProjectItem) projectItem).DocumentType != null);
                        flag = true;
                        break;

                    case 0x2c7:
                    case 0x2c8:
                        command.Enabled = projectItem.SupportsAddItem;
                        flag = true;
                        break;

                    case 0x2c9:
                        command.Enabled = projectItem.IsDeletable;
                        flag = true;
                        break;

                    case 0x2ca:
                        command.Enabled = projectItem.IsExpandable;
                        flag = true;
                        break;
                }
            }
            if (!flag)
            {
                if (this._activeProject != null)
                {
                    flag = ((ICommandHandlerWithContext) this._activeProject).UpdateCommand(command, projectItem);
                }
                if ((!flag && (this._activeProject != this._myComputerProject)) && (this._myComputerProject != null))
                {
                    flag = ((ICommandHandlerWithContext) this._myComputerProject).UpdateCommand(command, projectItem);
                }
            }
            if (!flag && (command.CommandGroup == typeof(GlobalCommands)))
            {
                if (command.CommandID == 720)
                {
                    command.Text = "(Custom Actions)";
                    command.Enabled = false;
                    return true;
                }
                if ((command.CommandID > 720) && (command.CommandID <= 0x2d7))
                {
                    command.Enabled = false;
                    command.Visible = false;
                    flag = true;
                }
            }
            return flag;
        }

        private void OnAfterCollapseTreeView(object sender, TreeViewEventArgs e)
        {
            ProjectTreeNode node = (ProjectTreeNode) e.Node;
            node.Nodes.Clear();
            node.Nodes.Add(new TreeNode());
        }

        private void OnAfterSelectTreeView(object sender, TreeViewEventArgs e)
        {
            this.UpdateToolBarButtons();
            ProjectTreeNode selectedNode = this._treeView.SelectedNode as ProjectTreeNode;
            Project project = null;
            if (selectedNode != null)
            {
                ProjectItem projectItem = selectedNode.ProjectItem;
                if (projectItem != null)
                {
                    project = projectItem.Project;
                }
            }
            if (this._activeProject != project)
            {
                this._activeProject = project;
                ICommandManager service = (ICommandManager) this.GetService(typeof(ICommandManager));
                if (service != null)
                {
                    service.UpdateCommands(false);
                }
            }
        }

        private void OnBeforeExpandTreeView(object sender, TreeViewCancelEventArgs e)
        {
            ProjectTreeNode node = (ProjectTreeNode) e.Node;
            TreeNodeCollection nodes = node.Nodes;
            if ((nodes.Count == 1) && (nodes[0].GetType() == typeof(TreeNode)))
            {
                e.Cancel = true;
                nodes.Clear();
                try
                {
                    ProjectItemCollection childItems = node.ProjectItem.ChildItems;
                    if ((childItems != null) && (childItems.Count != 0))
                    {
                        e.Cancel = false;
                        foreach (ProjectItem item in childItems)
                        {
                            ProjectTreeNode node2 = this.CreateProjectTreeNode(item, true);
                            node.Nodes.Add(node2);
                        }
                    }
                }
                catch (Exception exception)
                {
                    ((IMxUIService) base.ServiceProvider.GetService(typeof(IMxUIService))).ReportError(exception.Message, "Couldn't Open Folder", false);
                }
            }
        }

        private void OnDoubleClickTreeView(object sender, TreeViewEventArgs e)
        {
            ProjectTreeNode node = e.Node as ProjectTreeNode;
            if (node != null)
            {
                DocumentProjectItem projectItem = node.ProjectItem as DocumentProjectItem;
                if (projectItem != null)
                {
                    try
                    {
                        projectItem.Project.OpenProjectItem(projectItem, false, DocumentViewType.Default);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void OnDragDropTreeView(object sender, DragEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            p = this._treeView.PointToClient(p);
            ProjectTreeNode nodeAt = this._treeView.GetNodeAt(p) as ProjectTreeNode;
            e.Effect = nodeAt.ProjectItem.Project.HandleProjectItemDragDrop(nodeAt.ProjectItem, e.Data);
            if (e.Effect != DragDropEffects.None)
            {
                if (this._dragNode != null)
                {
                    this._dropNode = nodeAt;
                }
                else
                {
                    this.RefreshNode(nodeAt);
                }
            }
        }

        private void OnDragEnterTreeView(object sender, DragEventArgs e)
        {
            e.Effect = this.GetTreeViewDragDropEffects(e);
        }

        private void OnDragOverTreeView(object sender, DragEventArgs e)
        {
            e.Effect = this.GetTreeViewDragDropEffects(e);
        }

        private void OnItemDragTreeView(object sender, ItemDragEventArgs e)
        {
            ProjectTreeNode item = (ProjectTreeNode) e.Item;
            if (item.ProjectItem.IsDragSource)
            {
                ProjectTreeNode parent = (ProjectTreeNode) item.Parent;
                try
                {
                    this._dragNode = item;
                    DragDropEffects effects = base.DoDragDrop(item.ProjectItem.GetDataObject(), DragDropEffects.Move | DragDropEffects.Copy);
                    if (this._dropNode != null)
                    {
                        if (effects == DragDropEffects.Move)
                        {
                            parent.Nodes.Remove(item);
                        }
                        this.RefreshNode(this._dropNode);
                    }
                }
                finally
                {
                    this._dragNode = null;
                    this._dropNode = null;
                }
            }
        }

        private void OnProjectAdded(object sender, ProjectEventArgs e)
        {
            if (e.Project is WorkspaceProject)
            {
                base.Activate();
                if (this._firstProject)
                {
                    this._firstProject = false;
                    IDocumentTypeManager service = (IDocumentTypeManager) this.GetService(typeof(IDocumentTypeManager));
                    ImageList documentIcons = service.DocumentIcons;
                    ImageList imageList = this._treeView.ImageList;
                    this._documentIconOffset = imageList.Images.Count;
                    foreach (Image image in documentIcons.Images)
                    {
                        imageList.Images.Add(image);
                    }
                    if (e.Project is MyComputerProject)
                    {
                        this._myComputerProject = e.Project;
                    }
                }
                RootProjectItem projectItem = e.Project.ProjectItem;
                ProjectTreeNode node = this.CreateProjectTreeNode(projectItem, !projectItem.ShowChildrenByDefault);
                this._treeView.Nodes.Add(node);
                if (projectItem.ShowChildrenByDefault)
                {
                    ProjectItemCollection childItems = projectItem.ChildItems;
                    if (childItems != null)
                    {
                        foreach (ProjectItem item2 in childItems)
                        {
                            ProjectTreeNode node2 = this.CreateProjectTreeNode(item2, true);
                            node.Nodes.Add(node2);
                        }
                    }
                    if (node.Nodes.Count != 0)
                    {
                        node.Expand();
                    }
                }
                this._treeView.SelectedNode = node;
            }
        }

        private void OnProjectClosed(object sender, ProjectEventArgs e)
        {
            if (e.Project is WorkspaceProject)
            {
                TreeNode itemNode = e.Project.ProjectItem.ItemNode;
                if (itemNode != null)
                {
                    itemNode.Remove();
                }
            }
        }

        private void OnShowContextMenuTreeView(object sender, ShowContextMenuEventArgs e)
        {
            if (!e.UsingKeyboard)
            {
                ProjectTreeNode nodeAt = this._treeView.GetNodeAt(e.Location) as ProjectTreeNode;
                if (nodeAt != null)
                {
                    this._treeView.SelectedNode = nodeAt;
                }
            }
            ICommandManager service = (ICommandManager) this.GetService(typeof(ICommandManager));
            if (service != null)
            {
                service.ShowContextMenu(typeof(GlobalCommands), 1, this, this._treeView.SelectedNode, this._treeView, e.Location);
            }
        }

        private void OnToolBarButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            ProjectTreeNode selectedNode = this._treeView.SelectedNode as ProjectTreeNode;
            if ((selectedNode != null) && (selectedNode.ProjectItem != null))
            {
                ToolBarButton button = e.Button;
                if (button == this._openButton)
                {
                    this.OpenNode(selectedNode);
                }
                else if (button == this._refreshButton)
                {
                    this.RefreshNode(selectedNode);
                }
                else if (button == this._newDocumentButton)
                {
                    this.AddDocument(selectedNode);
                }
                else if (button == this._newFolderButton)
                {
                    this.AddFolder(selectedNode);
                }
                else if (button == this._deleteButton)
                {
                    this.DeleteNode(selectedNode);
                }
                else if (button == this._newProjectButton)
                {
                    this.AddProject();
                }
            }
        }

        private void OpenNode(ProjectTreeNode node)
        {
            DocumentProjectItem projectItem = (DocumentProjectItem) node.ProjectItem;
            try
            {
                projectItem.Project.OpenProjectItem(projectItem, false, DocumentViewType.Default);
            }
            catch (Exception)
            {
            }
        }

        private void RefreshNode(ProjectTreeNode node)
        {
            ProjectItem projectItem = node.ProjectItem;
            projectItem.Refresh();
            try
            {
                this._treeView.BeginUpdate();
                if (node.IsExpanded || (node.Nodes.Count == 0))
                {
                    node.Nodes.Clear();
                    ProjectItemCollection childItems = projectItem.ChildItems;
                    if ((childItems != null) && (childItems.Count != 0))
                    {
                        foreach (ProjectItem item2 in childItems)
                        {
                            ProjectTreeNode node2 = this.CreateProjectTreeNode(item2, true);
                            node.Nodes.Add(node2);
                        }
                    }
                }
                node.Expand();
            }
            finally
            {
                this._treeView.EndUpdate();
            }
        }

        private void UpdateToolBarButtons()
        {
            ProjectTreeNode selectedNode = this._treeView.SelectedNode as ProjectTreeNode;
            ProjectItem projectItem = null;
            if (selectedNode != null)
            {
                projectItem = selectedNode.ProjectItem;
            }
            bool flag = false;
            bool flag2 = false;
            bool isExpandable = false;
            bool isDeletable = false;
            if (projectItem != null)
            {
                DocumentProjectItem item2 = projectItem as DocumentProjectItem;
                if (item2 != null)
                {
                    flag = item2.DocumentType != null;
                }
                isDeletable = projectItem.IsDeletable;
                isExpandable = projectItem.IsExpandable;
                flag2 = (projectItem is FolderProjectItem) && projectItem.SupportsAddItem;
            }
            this._openButton.Enabled = flag;
            this._newDocumentButton.Enabled = flag2;
            this._newFolderButton.Enabled = flag2;
            this._refreshButton.Enabled = isExpandable;
            this._deleteButton.Enabled = isDeletable;
        }

        private class ProjectTreeNode : TreeNode
        {
            private Microsoft.Matrix.Core.Projects.ProjectItem _item;

            public ProjectTreeNode(Microsoft.Matrix.Core.Projects.ProjectItem item, int imageIndex, int selectedImageIndex, bool addDummyNodeIfRequired)
            {
                this._item = item;
                base.ImageIndex = imageIndex;
                base.SelectedImageIndex = selectedImageIndex;
                base.Text = item.Caption;
                if (addDummyNodeIfRequired && item.IsExpandable)
                {
                    TreeNode node = new TreeNode();
                    base.Nodes.Add(node);
                }
                this._item.ItemNode = this;
            }

            public Microsoft.Matrix.Core.Projects.ProjectItem ProjectItem
            {
                get
                {
                    return this._item;
                }
            }
        }
    }
}

