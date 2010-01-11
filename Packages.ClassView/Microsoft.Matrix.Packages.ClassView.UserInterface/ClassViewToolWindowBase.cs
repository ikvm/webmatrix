namespace Microsoft.Matrix.Packages.ClassView.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.ClassView;
    using Microsoft.Matrix.Packages.ClassView.Core;
    using Microsoft.Matrix.Packages.ClassView.Projects;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public abstract class ClassViewToolWindowBase : ToolWindow, ICommandHandler
    {
        private bool _cancelNextTreeNodeExpandCollapse;
        private CommandManager _commandManager;
        private MxListView _listView;
        private ClassViewToolWindowMode _mode;
        private ClassViewProject _project;
        private ClassViewToolWindowSearchMode _searchMode;
        private bool _searchOnly;
        private TypeSearchTask _searchTask;
        private IStatusBar _statusBar;
        private MxToolBar _toolBar1;
        private MxToolBar _toolBar2;
        private MxTreeView _treeView;

        public ClassViewToolWindowBase(IServiceProvider serviceProvider) : this(serviceProvider, false)
        {
        }

        protected ClassViewToolWindowBase(IServiceProvider serviceProvider, bool searchOnly) : base(serviceProvider)
        {
            this._searchOnly = searchOnly;
            this._searchMode = ClassViewToolWindowSearchMode.TypeName;
            if (!searchOnly)
            {
                this._mode = ClassViewToolWindowMode.Browse;
            }
            else
            {
                this._mode = ClassViewToolWindowMode.Search;
            }
            this.InitializeComponent();
        }

        private void CreateClassViewProject()
        {
            IProjectManager service = (IProjectManager) this.GetService(typeof(IProjectManager));
            if (service != null)
            {
                this._project = (ClassViewProject) service.CreateProject(typeof(ClassViewProjectFactory), null);
            }
            if ((this._project != null) && !this._searchOnly)
            {
                this.LoadTree(false);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._searchTask != null)
                {
                    this._searchTask.Cancel();
                    this._searchTask = null;
                }
                this._statusBar = null;
                IProjectManager service = (IProjectManager) this.GetService(typeof(IProjectManager));
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            Panel panel = new Panel();
            ImageList list = new ImageList();
            ImageList list2 = new ImageList();
            list.ImageSize = new Size(0x10, 0x10);
            list.TransparentColor = Color.Green;
            list.ColorDepth = ColorDepth.Depth32Bit;
            list.Images.AddStrip(new Bitmap(typeof(ClassViewToolWindow), "ClassViewTreeImages.bmp"));
            list2.ImageSize = new Size(0x10, 0x10);
            list2.TransparentColor = Color.Red;
            list2.ColorDepth = ColorDepth.Depth32Bit;
            list2.Images.AddStrip(new Bitmap(typeof(ClassViewToolWindow), "ClassViewToolBar.bmp"));
            System.Type commandGroup = typeof(ClassViewCommands);
            this._commandManager = new CommandManager(base.ServiceProvider, this);
            this._toolBar1 = new MxToolBar();
            this._toolBar1.Appearance = ToolBarAppearance.Flat;
            this._toolBar1.Divider = false;
            this._toolBar1.DropDownArrows = true;
            this._toolBar1.ShowToolTips = true;
            this._toolBar1.TabIndex = 0;
            this._toolBar1.TextAlign = ToolBarTextAlign.Right;
            this._toolBar1.Wrappable = false;
            this._toolBar1.ImageList = list2;
            if (!this._searchOnly)
            {
                MxToolBarButton button = new MxToolBarButton();
                MxToolBarButton button2 = new MxToolBarButton();
                MxToolBarButton button3 = new MxToolBarButton();
                MxToolBarButton button4 = new MxToolBarButton();
                MxToolBarButton button5 = new MxToolBarButton();
                ToolBarButton button6 = new ToolBarButton();
                button2.ImageIndex = 1;
                button2.ToolTipText = "Browse By Assembly";
                button.ImageIndex = 2;
                button.ToolTipText = "Browse By Namespace";
                button3.ImageIndex = 3;
                button3.ToolTipText = "Sort Alphabetically";
                button4.ImageIndex = 4;
                button4.ToolTipText = "Sort By Class Type";
                button5.ImageIndex = 5;
                button5.ToolTipText = "Sort By Visibility";
                button6.Style = ToolBarButtonStyle.Separator;
                if (!ClassViewPackage.IsClassBrowserApplication)
                {
                    MxToolBarButton button7 = new MxToolBarButton();
                    MxToolBarButton button8 = new MxToolBarButton();
                    MxToolBarButton button9 = new MxToolBarButton();
                    MxToolBarButton button10 = new MxToolBarButton();
                    MxToolBarButton button11 = new MxToolBarButton();
                    ComboBoxToolBarButton button12 = new ComboBoxToolBarButton();
                    ToolBarButton button13 = new ToolBarButton();
                    button7.ImageIndex = 0;
                    button7.ToolTipText = "Browse Search Results";
                    button8.ImageIndex = 7;
                    button8.ToolTipText = "View Non-Public Classes";
                    button9.ImageIndex = 6;
                    button9.ToolTipText = "Add/Remove Assemblies";
                    button10.ImageIndex = 8;
                    button10.ToolTipText = "Find By Type Name";
                    button11.ImageIndex = 9;
                    button11.ToolTipText = "Find By Member Name";
                    button12.DropDownStyle = ComboBoxStyle.DropDown;
                    button12.Text = new string('_', 0x16);
                    button12.InitialText = "Enter Name to Search";
                    button12.ToolTipText = "Search for a Class";
                    button13.Style = ToolBarButtonStyle.Separator;
                    this._toolBar1.Buttons.AddRange(new ToolBarButton[] { button, button2, button7, button6, button3, button4, button5, button13, button8, button9 });
                    this._toolBar2 = new MxToolBar();
                    this._toolBar2.Appearance = ToolBarAppearance.Flat;
                    this._toolBar2.Divider = false;
                    this._toolBar2.DropDownArrows = false;
                    this._toolBar2.ShowToolTips = true;
                    this._toolBar2.TabIndex = 0;
                    this._toolBar2.TextAlign = ToolBarTextAlign.Right;
                    this._toolBar2.Wrappable = false;
                    this._toolBar2.ImageList = list2;
                    this._toolBar2.Buttons.AddRange(new ToolBarButton[] { button12 });
                    this._commandManager.AddCommand(button7, commandGroup, 15);
                    this._commandManager.AddCommand(button8, commandGroup, 0x10);
                    this._commandManager.AddCommand(button9, commandGroup, 1);
                    this._commandManager.AddCommand(button12, commandGroup, 5);
                    this._commandManager.AddToolBar(this._toolBar2);
                }
                else
                {
                    this._toolBar1.Buttons.AddRange(new ToolBarButton[] { button, button2, button6, button3, button4, button5 });
                }
                this._commandManager.AddCommand(button, commandGroup, 14);
                this._commandManager.AddCommand(button2, commandGroup, 13);
                this._commandManager.AddCommand(button3, commandGroup, 10);
                this._commandManager.AddCommand(button4, commandGroup, 11);
                this._commandManager.AddCommand(button5, commandGroup, 12);
                this._commandManager.AddToolBar(this._toolBar1);
            }
            else
            {
                MxToolBarButton button14 = new MxToolBarButton();
                MxToolBarButton button15 = new MxToolBarButton();
                ComboBoxToolBarButton button16 = new ComboBoxToolBarButton();
                ToolBarButton button17 = new ToolBarButton();
                button14.ImageIndex = 8;
                button14.ToolTipText = "Find By Type Name";
                button14.Text = "Types";
                button15.ImageIndex = 9;
                button15.ToolTipText = "Find By Member Name";
                button15.Text = "Members";
                button16.DropDownStyle = ComboBoxStyle.DropDown;
                button16.Text = new string('_', 0x2d);
                button16.InitialText = "Enter Name to Search";
                button16.ToolTipText = "Search for a Class";
                button17.Style = ToolBarButtonStyle.Separator;
                this._toolBar1.Buttons.AddRange(new ToolBarButton[] { button14, button15, button17, button16 });
                this._commandManager.AddCommand(button14, commandGroup, 6);
                this._commandManager.AddCommand(button15, commandGroup, 7);
                this._commandManager.AddCommand(button16, commandGroup, 5);
                this._commandManager.AddToolBar(this._toolBar1);
            }
            panel.SuspendLayout();
            base.SuspendLayout();
            panel.BackColor = SystemColors.ControlDark;
            panel.Dock = DockStyle.Fill;
            panel.DockPadding.All = 1;
            panel.TabIndex = 1;
            if (!this._searchOnly)
            {
                this._treeView = new MxTreeView();
                this._treeView.BorderStyle = BorderStyle.None;
                this._treeView.Dock = DockStyle.Fill;
                this._treeView.HideSelection = false;
                this._treeView.ShowLines = false;
                this._treeView.TabIndex = 0;
                this._treeView.ImageList = list;
                this._treeView.AfterCollapse += new TreeViewEventHandler(this.OnAfterCollapseTreeView);
                this._treeView.AfterSelect += new TreeViewEventHandler(this.OnAfterSelectTreeView);
                this._treeView.BeforeCollapse += new TreeViewCancelEventHandler(this.OnBeforeCollapseTreeView);
                this._treeView.BeforeExpand += new TreeViewCancelEventHandler(this.OnBeforeExpandTreeView);
                this._treeView.NodeDoubleClick += new TreeViewEventHandler(this.OnNodeDoubleClickTreeView);
                panel.Controls.Add(this._treeView);
            }
            if (!ClassViewPackage.IsClassBrowserApplication || this._searchOnly)
            {
                ColumnHeader header = new ColumnHeader();
                ColumnHeader header2 = new ColumnHeader();
                header.Text = "Class";
                header2.Text = "Namespace";
                if (this._searchOnly)
                {
                    header.Width = 200;
                    header2.Width = 240;
                }
                else
                {
                    header.Width = 0x87;
                    header2.Width = 240;
                }
                this._listView = new MxListView();
                this._listView.BorderStyle = BorderStyle.None;
                this._listView.Dock = DockStyle.Fill;
                this._listView.HideSelection = false;
                this._listView.FullRowSelect = true;
                this._listView.TabIndex = 0;
                this._listView.SmallImageList = list;
                this._listView.View = View.Details;
                this._listView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
                this._listView.LabelWrap = false;
                this._listView.MultiSelect = false;
                this._listView.FlatScrollBars = true;
                this._listView.ShowToolTips = true;
                this._listView.WatermarkText = "No Classes Found";
                this._listView.Sorting = SortOrder.Ascending;
                this._listView.Columns.Add(header);
                this._listView.Columns.Add(header2);
                if (this._searchOnly)
                {
                    ColumnHeader header3 = new ColumnHeader();
                    header3.Width = 200;
                    header3.Text = "Member";
                    this._listView.Columns.Add(header3);
                }
                if (this.Mode == ClassViewToolWindowMode.Browse)
                {
                    this._listView.Visible = false;
                }
                this._listView.DoubleClick += new EventHandler(this.OnDoubleClickListView);
                this._listView.SelectedIndexChanged += new EventHandler(this.OnSelectedIndexChangedListView);
                panel.Controls.Add(this._listView);
            }
            if (ClassViewPackage.IsClassBrowserApplication || this._searchOnly)
            {
                base.Controls.AddRange(new Control[] { panel, this._toolBar1 });
            }
            else
            {
                base.Controls.AddRange(new Control[] { panel, this._toolBar2, this._toolBar1 });
            }
            if (!this._searchOnly)
            {
                this.Text = "Classes";
                if (ClassViewPackage.IsClassBrowserApplication)
                {
                    base.Icon = new Icon(typeof(ClassViewToolWindowBase), "ClassBrowseToolWindow.ico");
                }
                else
                {
                    base.Icon = new Icon(typeof(ClassViewToolWindowBase), "ClassViewToolWindow.ico");
                }
            }
            else
            {
                this.Text = "Search";
                base.Icon = new Icon(typeof(ClassViewToolWindowBase), "ClassSearchToolWindow.ico");
            }
            panel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void LoadTree(bool refresh)
        {
            try
            {
                this._treeView.BeginUpdate();
                ProjectItem projectItem = this._project.ProjectItem;
                if (refresh)
                {
                    this._treeView.Nodes.Clear();
                    projectItem.Refresh();
                }
                ProjectItemCollection childItems = projectItem.ChildItems;
                if (childItems != null)
                {
                    foreach (ProjectItem item2 in childItems)
                    {
                        ClassViewTreeNode node = new ClassViewTreeNode(item2, item2.IconIndex, true);
                        this._treeView.Nodes.Add(node);
                    }
                }
            }
            finally
            {
                this._treeView.EndUpdate();
            }
            this.UpdateCommands();
        }

        bool ICommandHandler.HandleCommand(Microsoft.Matrix.UIComponents.Command command)
        {
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            if (command.CommandGroup == typeof(ClassViewCommands))
            {
                switch (command.CommandID)
                {
                    case 1:
                    {
                        ClassViewProjectData projectData = this._project.ProjectData;
                        ICollection assemblyEntries = projectData.AssemblyEntries;
                        ArrayList list = new ArrayList(assemblyEntries.Count);
                        foreach (ClassViewProjectData.AssemblyEntry entry in assemblyEntries)
                        {
                            try
                            {
                                list.Add(entry.AssemblyName);
                                continue;
                            }
                            catch
                            {
                                continue;
                            }
                        }
                        AssemblyName[] initialList = (AssemblyName[]) list.ToArray(typeof(AssemblyName));
                        IUIService service = (IUIService) base.ServiceProvider.GetService(typeof(IUIService));
                        AssemblySelectionDialog form = new AssemblySelectionDialog(base.ServiceProvider, initialList);
                        if (service.ShowDialog(form) == DialogResult.OK)
                        {
                            projectData.ClearAssemblyEntries();
                            foreach (AssemblyName name in form.SelectedAssemblies)
                            {
                                projectData.AddAssemblyEntry(name);
                            }
                            flag3 = true;
                        }
                        flag = true;
                        break;
                    }
                    case 2:
                        if (this.Mode != ClassViewToolWindowMode.Browse)
                        {
                            ClassViewListViewItem item2 = (ClassViewListViewItem) this._listView.SelectedItems[0];
                            TypeProjectItem projectItem = item2.ProjectItem;
                            this.OpenTypeView(projectItem.Type);
                        }
                        else
                        {
                            ClassViewTreeNode selectedNode = this._treeView.SelectedNode as ClassViewTreeNode;
                            if (selectedNode != null)
                            {
                                TypeProjectItem item = selectedNode.ProjectItem as TypeProjectItem;
                                if (item != null)
                                {
                                    this.OpenTypeView(item.Type);
                                }
                            }
                        }
                        flag = true;
                        break;

                    case 5:
                    {
                        if (this._searchTask != null)
                        {
                            TypeSearchTask task = this._searchTask;
                            this._searchTask = null;
                            task.Cancel();
                        }
                        string searchValue = command.Text.Trim();
                        if (searchValue.Length != 0)
                        {
                            this._listView.Items.Clear();
                            if ((this.SearchMode == ClassViewToolWindowSearchMode.MemberName) && !searchValue.StartsWith("::"))
                            {
                                searchValue = "::" + searchValue;
                            }
                            TypeSearchTask searchTask = TypeSearchTask.CreateSearchTask(this._project.ProjectData, searchValue);
                            if (searchTask != null)
                            {
                                this.PerformSearch(searchTask);
                            }
                            else
                            {
                                IMxUIService service2 = (IMxUIService) this.GetService(typeof(IMxUIService));
                                if (service2 != null)
                                {
                                    string error = "The specified search string did not have a valid syntax.\r\n\r\nUse the following syntax to define your search criteria:\r\n<TypeName> - Search for types by their name.\r\n<Namespace>.<TypeName> - Search for types by their full name.\r\n::<MemberName> - Search for types containing a matching field, property, event or method.";
                                    service2.ReportError(error, "Type Search", true);
                                }
                            }
                        }
                        flag = true;
                        break;
                    }
                    case 6:
                        this.SearchMode = ClassViewToolWindowSearchMode.TypeName;
                        flag = true;
                        flag2 = true;
                        break;

                    case 7:
                        this.SearchMode = ClassViewToolWindowSearchMode.MemberName;
                        flag = true;
                        flag2 = true;
                        break;

                    case 10:
                        this._project.ProjectData.SortMode = ClassViewProjectSortMode.Alphabetical;
                        flag3 = true;
                        flag = true;
                        break;

                    case 11:
                        this._project.ProjectData.SortMode = ClassViewProjectSortMode.ByClassType;
                        flag3 = true;
                        flag = true;
                        break;

                    case 12:
                        this._project.ProjectData.SortMode = ClassViewProjectSortMode.ByClassVisibility;
                        flag3 = true;
                        flag = true;
                        break;

                    case 13:
                        this._project.ProjectData.ViewMode = ClassViewProjectViewMode.Assembly;
                        if (this.Mode != ClassViewToolWindowMode.Browse)
                        {
                            this.Mode = ClassViewToolWindowMode.Browse;
                            flag2 = true;
                        }
                        flag3 = true;
                        flag = true;
                        break;

                    case 14:
                        this._project.ProjectData.ViewMode = ClassViewProjectViewMode.Namespace;
                        if (this.Mode != ClassViewToolWindowMode.Browse)
                        {
                            this.Mode = ClassViewToolWindowMode.Browse;
                            flag2 = true;
                        }
                        flag3 = true;
                        flag = true;
                        break;

                    case 15:
                        if (this.Mode != ClassViewToolWindowMode.Search)
                        {
                            this.Mode = ClassViewToolWindowMode.Search;
                            flag2 = true;
                        }
                        flag = true;
                        break;

                    case 0x10:
                        this._project.ProjectData.ShowNonPublicMembers = !this._project.ProjectData.ShowNonPublicMembers;
                        flag3 = true;
                        flag = true;
                        break;
                }
            }
            if (flag3)
            {
                this.LoadTree(true);
                return flag;
            }
            if (flag2)
            {
                this.UpdateCommands();
            }
            return flag;
        }

        bool ICommandHandler.UpdateCommand(Microsoft.Matrix.UIComponents.Command command)
        {
            bool flag = false;
            if (command.CommandGroup == typeof(ClassViewCommands))
            {
                switch (command.CommandID)
                {
                    case 1:
                        return true;

                    case 2:
                        if (this.Mode == ClassViewToolWindowMode.Browse)
                        {
                            ClassViewTreeNode selectedNode = this._treeView.SelectedNode as ClassViewTreeNode;
                            if ((selectedNode == null) || !(selectedNode.ProjectItem is DocumentProjectItem))
                            {
                                command.Enabled = false;
                            }
                        }
                        else if (this._listView.SelectedItems.Count == 0)
                        {
                            command.Enabled = false;
                        }
                        return true;

                    case 3:
                    case 4:
                    case 8:
                    case 9:
                        return flag;

                    case 5:
                        return true;

                    case 6:
                        flag = true;
                        command.Checked = this.SearchMode == ClassViewToolWindowSearchMode.TypeName;
                        return flag;

                    case 7:
                        flag = true;
                        command.Checked = this.SearchMode == ClassViewToolWindowSearchMode.MemberName;
                        return flag;

                    case 10:
                        command.Enabled = this.Mode == ClassViewToolWindowMode.Browse;
                        command.Checked = this._project.ProjectData.SortMode == ClassViewProjectSortMode.Alphabetical;
                        return true;

                    case 11:
                        command.Enabled = this.Mode == ClassViewToolWindowMode.Browse;
                        command.Checked = this._project.ProjectData.SortMode == ClassViewProjectSortMode.ByClassType;
                        return true;

                    case 12:
                        command.Enabled = this.Mode == ClassViewToolWindowMode.Browse;
                        command.Checked = this._project.ProjectData.SortMode == ClassViewProjectSortMode.ByClassVisibility;
                        return true;

                    case 13:
                        command.Checked = (this.Mode == ClassViewToolWindowMode.Browse) && (this._project.ProjectData.ViewMode == ClassViewProjectViewMode.Assembly);
                        return true;

                    case 14:
                        command.Checked = (this.Mode == ClassViewToolWindowMode.Browse) && (this._project.ProjectData.ViewMode == ClassViewProjectViewMode.Namespace);
                        return true;

                    case 15:
                        command.Checked = this.Mode == ClassViewToolWindowMode.Search;
                        return true;

                    case 0x10:
                        command.Checked = this._project.ProjectData.ShowNonPublicMembers;
                        return true;
                }
            }
            return flag;
        }

        private void OnAfterCollapseTreeView(object sender, TreeViewEventArgs e)
        {
            ClassViewTreeNode node = (ClassViewTreeNode) e.Node;
            node.Nodes.Clear();
            node.Nodes.Add(new TreeNode());
        }

        private void OnAfterSelectTreeView(object sender, TreeViewEventArgs e)
        {
            this.UpdateCommands();
        }

        private void OnBeforeCollapseTreeView(object sender, TreeViewCancelEventArgs e)
        {
            if (this._cancelNextTreeNodeExpandCollapse)
            {
                this._cancelNextTreeNodeExpandCollapse = false;
                e.Cancel = true;
            }
        }

        private void OnBeforeExpandTreeView(object sender, TreeViewCancelEventArgs e)
        {
            if (this._cancelNextTreeNodeExpandCollapse)
            {
                this._cancelNextTreeNodeExpandCollapse = false;
                e.Cancel = true;
            }
            else
            {
                ClassViewTreeNode node = (ClassViewTreeNode) e.Node;
                TreeNodeCollection nodes = node.Nodes;
                if ((nodes.Count == 1) && (nodes[0].GetType() == typeof(TreeNode)))
                {
                    e.Cancel = true;
                    nodes.Clear();
                    ProjectItemCollection childItems = node.ProjectItem.ChildItems;
                    if ((childItems != null) && (childItems.Count != 0))
                    {
                        e.Cancel = false;
                        foreach (ProjectItem item in childItems)
                        {
                            ClassViewTreeNode node2 = new ClassViewTreeNode(item, item.IconIndex, true);
                            node.Nodes.Add(node2);
                        }
                    }
                }
            }
        }

        private void OnDoubleClickListView(object sender, EventArgs e)
        {
            if (this._listView.SelectedItems.Count != 0)
            {
                ClassViewListViewItem item = (ClassViewListViewItem) this._listView.SelectedItems[0];
                TypeProjectItem projectItem = item.ProjectItem;
                try
                {
                    this.OpenTypeView(projectItem.Type);
                }
                catch (Exception)
                {
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.CreateClassViewProject();
            ((ICommandManager) this._commandManager).ResumeCommandUpdate();
        }

        private void OnNodeDoubleClickTreeView(object sender, TreeViewEventArgs e)
        {
            this._cancelNextTreeNodeExpandCollapse = false;
            ClassViewTreeNode node = e.Node as ClassViewTreeNode;
            if (node != null)
            {
                TypeProjectItem projectItem = node.ProjectItem as TypeProjectItem;
                if (projectItem != null)
                {
                    try
                    {
                        this._cancelNextTreeNodeExpandCollapse = true;
                        this.OpenTypeView(projectItem.Type);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void OnSearchResultsPosted(object sender, AsyncTaskResultPostedEventArgs e)
        {
            if ((this._searchTask != null) && (this._searchTask == sender))
            {
                ICollection data = e.Data as ICollection;
                if ((data != null) && (data.Count != 0))
                {
                    try
                    {
                        this._listView.BeginUpdate();
                        foreach (MemberInfo info in data)
                        {
                            System.Type reflectedType = info as System.Type;
                            string extraData = string.Empty;
                            if (reflectedType == null)
                            {
                                reflectedType = info.ReflectedType;
                                if (this._searchOnly)
                                {
                                    extraData = string.Concat(new object[] { info.Name, " [", info.MemberType, "]" });
                                }
                            }
                            OpenTypeProjectItem item = new OpenTypeProjectItem(reflectedType, this._project);
                            item.EnsureTypeInformation();
                            ClassViewListViewItem item2 = new ClassViewListViewItem(item, extraData);
                            this._listView.Items.Add(item2);
                        }
                    }
                    finally
                    {
                        this._listView.EndUpdate();
                    }
                    if (this._statusBar != null)
                    {
                        this._statusBar.SetProgress(Math.Max(10, e.PercentComplete));
                        this._statusBar.SetText("Searching...");
                    }
                    this._listView.Focus();
                    this.UpdateCommands();
                }
                if (e.IsComplete)
                {
                    this._listView.WatermarkText = "No Classes Found";
                    if (this._statusBar != null)
                    {
                        this._statusBar.SetProgress(0);
                        this._statusBar.SetText(string.Empty);
                    }
                    this._searchTask = null;
                }
            }
        }

        private void OnSelectedIndexChangedListView(object sender, EventArgs e)
        {
            this.UpdateCommands();
        }

        private void OpenTypeView(System.Type type)
        {
            OpenTypeProjectItem projectItem = new OpenTypeProjectItem(type, this._project);
            this._project.OpenProjectItem(projectItem, true, DocumentViewType.Default);
        }

        internal virtual void PerformSearch(TypeSearchTask searchTask)
        {
            if (this._searchTask != null)
            {
                TypeSearchTask task = this._searchTask;
                this._searchTask = null;
                task.Cancel();
            }
            this._listView.Items.Clear();
            base.Activate();
            this._searchTask = searchTask;
            this._listView.WatermarkText = "Searching...";
            this.Mode = ClassViewToolWindowMode.Search;
            if (this._statusBar == null)
            {
                this._statusBar = (IStatusBar) this.GetService(typeof(IStatusBar));
            }
            if (this._statusBar != null)
            {
                this._statusBar.SetProgress(10);
                this._statusBar.SetText("Searching...");
            }
            this.UpdateCommands();
            this._searchTask.Start(new AsyncTaskResultPostedEventHandler(this.OnSearchResultsPosted));
        }

        private void UpdateCommands()
        {
            ICommandManager service = (ICommandManager) this.GetService(typeof(ICommandManager));
            if (service != null)
            {
                service.UpdateCommands(false);
            }
            ((ICommandManager) this._commandManager).UpdateCommands(false);
        }

        private ClassViewToolWindowMode Mode
        {
            get
            {
                return this._mode;
            }
            set
            {
                this._mode = value;
                if (!this._searchOnly)
                {
                    this._treeView.Visible = value == ClassViewToolWindowMode.Browse;
                    this._listView.Visible = value == ClassViewToolWindowMode.Search;
                }
            }
        }

        private ClassViewToolWindowSearchMode SearchMode
        {
            get
            {
                return this._searchMode;
            }
            set
            {
                this._searchMode = value;
            }
        }

        private sealed class ClassViewListViewItem : ListViewItem
        {
            private TypeProjectItem _item;

            public ClassViewListViewItem(TypeProjectItem item, string extraData) : base(new string[] { item.Caption, item.Type.Namespace, extraData }, item.IconIndex)
            {
                this._item = item;
            }

            public TypeProjectItem ProjectItem
            {
                get
                {
                    return this._item;
                }
            }
        }

        private enum ClassViewToolWindowMode
        {
            Browse,
            Search
        }

        private enum ClassViewToolWindowSearchMode
        {
            TypeName,
            MemberName
        }

        private sealed class ClassViewTreeNode : TreeNode
        {
            private Microsoft.Matrix.Core.Projects.ProjectItem _item;

            public ClassViewTreeNode(Microsoft.Matrix.Core.Projects.ProjectItem item, int imageIndex, bool addDummyNodeIfRequired)
            {
                this._item = item;
                base.ImageIndex = imageIndex;
                base.SelectedImageIndex = imageIndex;
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

