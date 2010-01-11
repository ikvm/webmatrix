namespace Microsoft.Matrix.Packages.ClassView.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.ClassView.Documents;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    internal sealed class TypeView : Control, IDocumentView, ICommandHandler
    {
        private DescriptionView _descriptionView;
        private Microsoft.Matrix.UIComponents.TabControl _detailsTabControl;
        private TypeDocument _document;
        private TreeView _memberTree;
        private OutlineView _outlineView;
        private TypeViewTreeNode _rootNode;
        private IServiceProvider _serviceProvider;
        private MxCheckBox _showInheritedCheckBox;
        private MxCheckBox _showNonPublicCheckBox;
        private MxCheckBox _showObsoleteCheckBox;
        private static Bitmap viewImage;

        event EventHandler IDocumentView.DocumentChanged
        {
            add
            {
            }
            remove
            {
            }
        }

        public TypeView(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        protected override void Dispose(bool disposing)
        {
            this._descriptionView.Dispose();
            this._descriptionView = null;
            base.Dispose(disposing);
        }

        private void InitializeUserInterface()
        {
            this._detailsTabControl = new Microsoft.Matrix.UIComponents.TabControl();
            Panel panel = new Panel();
            ImageList list = new ImageList();
            Splitter splitter = new Splitter();
            Microsoft.Matrix.UIComponents.TabPage page = new Microsoft.Matrix.UIComponents.TabPage();
            Panel panel2 = new Panel();
            Microsoft.Matrix.UIComponents.TabPage page2 = new Microsoft.Matrix.UIComponents.TabPage();
            Panel panel3 = new Panel();
            Panel panel4 = new Panel();
            this._memberTree = new TreeView();
            this._descriptionView = new DescriptionView(this._serviceProvider, true);
            this._outlineView = new OutlineView(this._serviceProvider);
            this._showNonPublicCheckBox = new MxCheckBox();
            this._showInheritedCheckBox = new MxCheckBox();
            this._showObsoleteCheckBox = new MxCheckBox();
            list.ImageSize = new Size(0x10, 0x10);
            list.TransparentColor = Color.Green;
            list.ColorDepth = ColorDepth.Depth32Bit;
            list.Images.AddStrip(new Bitmap(typeof(TypeView), "MemberTreeImages.bmp"));
            this._memberTree.BorderStyle = BorderStyle.None;
            this._memberTree.Dock = DockStyle.Fill;
            this._memberTree.HideSelection = false;
            this._memberTree.ShowRootLines = false;
            this._memberTree.ShowLines = false;
            this._memberTree.TabIndex = 0;
            this._memberTree.ImageList = list;
            this._memberTree.AfterSelect += new TreeViewEventHandler(this.OnAfterSelectMemberTree);
            this._memberTree.BeforeCollapse += new TreeViewCancelEventHandler(this.OnBeforeCollapseMemberTree);
            panel.BackColor = SystemColors.ControlDark;
            panel.Size = new Size(200, 4);
            panel.Dock = DockStyle.Left;
            panel.DockPadding.All = 1;
            panel.TabIndex = 0;
            panel.Controls.Add(this._memberTree);
            splitter.Dock = DockStyle.Left;
            splitter.Size = new Size(3, 4);
            splitter.TabIndex = 1;
            this._descriptionView.Dock = DockStyle.Fill;
            this._descriptionView.TabIndex = 0;
            panel2.BackColor = SystemColors.ControlDark;
            panel2.Dock = DockStyle.Fill;
            panel2.DockPadding.All = 1;
            panel2.TabIndex = 0;
            panel2.TabStop = false;
            panel2.Controls.Add(this._descriptionView);
            page.Text = "Description";
            page.Controls.Add(panel2);
            this._detailsTabControl.TabPlacement = TabPlacement.Bottom;
            this._detailsTabControl.Mode = TabControlMode.TextOnly;
            this._detailsTabControl.Dock = DockStyle.Fill;
            this._detailsTabControl.TabIndex = 2;
            this._detailsTabControl.Tabs.Add(page);
            this._outlineView.Dock = DockStyle.Fill;
            this._outlineView.TabIndex = 0;
            panel3.BackColor = SystemColors.ControlDark;
            panel3.Dock = DockStyle.Fill;
            panel3.DockPadding.All = 1;
            panel3.TabIndex = 0;
            panel3.TabStop = false;
            panel3.Controls.Add(this._outlineView);
            page2.Text = "Class Outline";
            page2.Controls.Add(panel3);
            this._detailsTabControl.Tabs.Add(page2);
            this._showNonPublicCheckBox.Text = "Show Non-Public Members";
            this._showNonPublicCheckBox.TabIndex = 0;
            this._showNonPublicCheckBox.Size = new Size(200, 0x10);
            this._showNonPublicCheckBox.Location = new Point(2, 2);
            this._showNonPublicCheckBox.CheckedChanged += new EventHandler(this.OnCheckedChangedShowNonPublicCheckBox);
            this._showInheritedCheckBox.Text = "Show Inherited Members";
            this._showInheritedCheckBox.TabIndex = 1;
            this._showInheritedCheckBox.Size = new Size(200, 0x10);
            this._showInheritedCheckBox.Location = new Point(0xd0, 2);
            this._showInheritedCheckBox.CheckedChanged += new EventHandler(this.OnCheckedChangedShowInheritedCheckBox);
            this._showObsoleteCheckBox.Text = "Show Obsolete Members";
            this._showObsoleteCheckBox.TabIndex = 2;
            this._showObsoleteCheckBox.Size = new Size(200, 0x10);
            this._showObsoleteCheckBox.Location = new Point(0x19e, 2);
            this._showObsoleteCheckBox.CheckedChanged += new EventHandler(this.OnCheckedChangedShowObsoleteCheckBox);
            panel4.TabIndex = 3;
            panel4.Dock = DockStyle.Bottom;
            panel4.Size = new Size(10, 20);
            panel4.Controls.Add(this._showNonPublicCheckBox);
            panel4.Controls.Add(this._showInheritedCheckBox);
            panel4.Controls.Add(this._showObsoleteCheckBox);
            base.Controls.Add(this._detailsTabControl);
            base.Controls.Add(splitter);
            base.Controls.Add(panel);
            base.Controls.Add(panel4);
        }

        private void LoadDocumentItems()
        {
            System.Type type = this._document.Type;
            TypeDocumentFilter filter = TypeDocumentFilter.Declared | TypeDocumentFilter.Current | TypeDocumentFilter.Protected | TypeDocumentFilter.Public;
            if (this._showNonPublicCheckBox.Checked)
            {
                filter |= TypeDocumentFilter.Private;
            }
            if (this._showInheritedCheckBox.Checked)
            {
                filter |= TypeDocumentFilter.Inherited;
            }
            if (this._showObsoleteCheckBox.Checked)
            {
                filter |= TypeDocumentFilter.Obsolete;
            }
            this._document.SetFilter(filter);
            this._memberTree.BeginUpdate();
            this._outlineView.BeginOutline(type);
            try
            {
                bool[] flagArray = null;
                bool flag = this._rootNode != null;
                if (flag && !type.IsEnum)
                {
                    int index = 0;
                    flagArray = new bool[this._rootNode.Nodes.Count];
                    foreach (TreeNode node in this._rootNode.Nodes)
                    {
                        flagArray[index] = node.IsExpanded;
                        index++;
                    }
                }
                this._memberTree.Nodes.Clear();
                this._rootNode = new TypeViewTreeNode(this._document);
                this._memberTree.Nodes.Add(this._rootNode);
                if (type.IsEnum)
                {
                    TreeNodeCollection nodes = this._rootNode.Nodes;
                    foreach (FieldItem item in this._document.FieldItems)
                    {
                        this._outlineView.AddEnumField(item);
                        nodes.Add(new TypeViewTreeNode(item));
                    }
                    this._showNonPublicCheckBox.Enabled = false;
                    this._showInheritedCheckBox.Enabled = false;
                }
                else
                {
                    TreeNodeCollection nodes2;
                    TypeViewTreeNode node2 = null;
                    TypeViewTreeNode node3 = null;
                    TypeViewTreeNode node4 = null;
                    TypeViewTreeNode node5 = null;
                    TypeViewTreeNode node6 = null;
                    bool flag2 = true;
                    bool flag3 = true;
                    bool flag4 = true;
                    bool flag5 = true;
                    bool flag6 = true;
                    bool flag7 = true;
                    bool flag8 = true;
                    if (type.IsInterface)
                    {
                        flag2 = false;
                        flag3 = false;
                        flag8 = false;
                        flag7 = false;
                    }
                    else if (type.IsPrimitive)
                    {
                        flag7 = true;
                    }
                    else if (type.IsValueType)
                    {
                        flag8 = false;
                    }
                    this._showNonPublicCheckBox.Enabled = flag7;
                    this._showInheritedCheckBox.Enabled = flag8;
                    if (flag2)
                    {
                        node2 = new TypeViewTreeNode("Fields");
                        this._rootNode.Nodes.Add(node2);
                    }
                    if (flag3)
                    {
                        node3 = new TypeViewTreeNode("Constructors");
                        this._rootNode.Nodes.Add(node3);
                    }
                    if (flag4)
                    {
                        node4 = new TypeViewTreeNode("Properties");
                        this._rootNode.Nodes.Add(node4);
                    }
                    if (flag5)
                    {
                        node5 = new TypeViewTreeNode("Methods");
                        this._rootNode.Nodes.Add(node5);
                    }
                    if (flag6)
                    {
                        node6 = new TypeViewTreeNode("Events");
                        this._rootNode.Nodes.Add(node6);
                    }
                    if (flag2 && (this._document.FieldItems.Count != 0))
                    {
                        this._outlineView.StartFields();
                        nodes2 = node2.Nodes;
                        foreach (FieldItem item2 in this._document.FieldItems)
                        {
                            this._outlineView.AddField(item2);
                            nodes2.Add(new TypeViewTreeNode(item2));
                        }
                    }
                    if (flag3 && (this._document.ConstructorItems.Count != 0))
                    {
                        this._outlineView.StartConstructors();
                        nodes2 = node3.Nodes;
                        foreach (MethodItem item3 in this._document.ConstructorItems)
                        {
                            this._outlineView.AddMethod(item3);
                            nodes2.Add(new TypeViewTreeNode(item3));
                        }
                    }
                    if (flag4 && (this._document.PropertyItems.Count != 0))
                    {
                        this._outlineView.StartProperties();
                        nodes2 = node4.Nodes;
                        foreach (PropertyItem item4 in this._document.PropertyItems)
                        {
                            this._outlineView.AddProperty(item4);
                            nodes2.Add(new TypeViewTreeNode(item4));
                        }
                    }
                    if (flag6 && (this._document.EventItems.Count != 0))
                    {
                        this._outlineView.StartEvents();
                        nodes2 = node6.Nodes;
                        foreach (EventItem item5 in this._document.EventItems)
                        {
                            this._outlineView.AddEvent(item5);
                            nodes2.Add(new TypeViewTreeNode(item5));
                        }
                    }
                    if (flag5 && (this._document.MethodItems.Count != 0))
                    {
                        this._outlineView.StartMethods();
                        nodes2 = node5.Nodes;
                        foreach (MethodItem item6 in this._document.MethodItems)
                        {
                            this._outlineView.AddMethod(item6);
                            nodes2.Add(new TypeViewTreeNode(item6));
                        }
                    }
                }
                if (flag && !type.IsEnum)
                {
                    int num2 = 0;
                    foreach (TreeNode node7 in this._rootNode.Nodes)
                    {
                        if (flagArray[num2])
                        {
                            node7.Expand();
                        }
                        num2++;
                    }
                }
                this._rootNode.Expand();
                this._memberTree.SelectedNode = this._rootNode;
            }
            catch (Exception)
            {
            }
            finally
            {
                this._outlineView.EndOutline();
                this._memberTree.EndUpdate();
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
            this._document = (TypeDocument) document;
            System.Type type = this._document.Type;
            if (this._detailsTabControl == null)
            {
                this.InitializeUserInterface();
            }
            this._descriptionView.SetCurrentDocument(this._document);
            this.LoadDocumentItems();
        }

        bool IDocumentView.SaveToDocument()
        {
            return true;
        }

        bool ICommandHandler.HandleCommand(Microsoft.Matrix.UIComponents.Command command)
        {
            return false;
        }

        bool ICommandHandler.UpdateCommand(Microsoft.Matrix.UIComponents.Command command)
        {
            return false;
        }

        private void OnAfterSelectMemberTree(object sender, TreeViewEventArgs e)
        {
            TypeViewTreeNode node = (TypeViewTreeNode) e.Node;
            if (((node.Item == null) && (node.Document == null)) || this._document.Type.IsEnum)
            {
                node = this._rootNode;
            }
            this._document.SetCurrentItem(node.Item);
            this._descriptionView.SetCurrentItem(node.Item);
        }

        private void OnBeforeCollapseMemberTree(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node == this._rootNode)
            {
                e.Cancel = true;
            }
        }

        private void OnCheckedChangedShowInheritedCheckBox(object sender, EventArgs e)
        {
            this.LoadDocumentItems();
        }

        private void OnCheckedChangedShowNonPublicCheckBox(object sender, EventArgs e)
        {
            this.LoadDocumentItems();
        }

        private void OnCheckedChangedShowObsoleteCheckBox(object sender, EventArgs e)
        {
            this.LoadDocumentItems();
        }

        bool IDocumentView.CanDeactivate
        {
            get
            {
                return true;
            }
        }

        bool IDocumentView.IsDirty
        {
            get
            {
                return false;
            }
        }

        Image IDocumentView.ViewImage
        {
            get
            {
                if (viewImage == null)
                {
                    viewImage = new Bitmap(typeof(TypeView), "TypeView.bmp");
                    viewImage.MakeTransparent();
                }
                return viewImage;
            }
        }

        string IDocumentView.ViewName
        {
            get
            {
                return "Type View";
            }
        }

        DocumentViewType IDocumentView.ViewType
        {
            get
            {
                return DocumentViewType.Default;
            }
        }

        private sealed class TypeViewTreeNode : TreeNode
        {
            private TypeDocument _document;
            private TypeDocumentItem _item;
            private const int AssemblyVisibilityOffset = 1;
            private const int EmptyImageIndex = 0;
            private const int EventMemberImageIndex = 2;
            private const int FamilyVisibilityOffset = 3;
            private const int FieldMemberImageIndex = 12;
            private const int FolderImageIndex = 1;
            private const int MethodMemberImageIndex = 7;
            private const int PrivateVisibilityOffset = 4;
            private const int PropertyMemberImageIndex = 0x11;
            private const int PublicVisibilityOffset = 0;
            private const int StaticOffset = 2;
            private const int TypeImageIndex = 0x16;

            public TypeViewTreeNode(TypeDocument document) : base(document.Type.Name, 0x16, 0x16)
            {
                this._document = document;
            }

            public TypeViewTreeNode(TypeDocumentItem item) : base(item.Text)
            {
                this._item = item;
                base.ImageIndex = base.SelectedImageIndex = this.GetImageIndex();
                if (item.IsObsolete)
                {
                    base.ForeColor = SystemColors.ControlDark;
                }
            }

            public TypeViewTreeNode(string text) : base(text, 1, 1)
            {
            }

            private int GetImageIndex()
            {
                int num = 0;
                int imageOffset = 0;
                if (this._item is FieldItem)
                {
                    num = 12;
                    imageOffset = this.GetImageOffset((FieldInfo) this._item.Member);
                }
                else if (this._item is PropertyItem)
                {
                    num = 0x11;
                    imageOffset = this.GetImageOffset(((PropertyItem) this._item).UnderlyingMethod);
                }
                else if (this._item is EventItem)
                {
                    num = 2;
                    imageOffset = this.GetImageOffset(((EventItem) this._item).UnderlyingMethod);
                }
                else
                {
                    num = 7;
                    imageOffset = this.GetImageOffset((MethodBase) this._item.Member);
                }
                return (num + imageOffset);
            }

            private int GetImageOffset(FieldInfo fi)
            {
                int num = 0;
                if (fi.IsFamily || fi.IsFamilyAndAssembly)
                {
                    num = 3;
                }
                else if (fi.IsAssembly)
                {
                    num = 1;
                }
                else if (fi.IsPrivate)
                {
                    num = 4;
                }
                if ((!fi.IsLiteral && fi.IsStatic) && !fi.DeclaringType.IsEnum)
                {
                    num = 2;
                }
                return num;
            }

            private int GetImageOffset(MethodBase mb)
            {
                int num = 0;
                if (mb.IsFamily || mb.IsFamilyAndAssembly)
                {
                    num = 3;
                }
                else if (mb.IsAssembly)
                {
                    num = 1;
                }
                else if (mb.IsPrivate)
                {
                    num = 4;
                }
                if (mb.IsStatic)
                {
                    num = 2;
                }
                return num;
            }

            public TypeDocument Document
            {
                get
                {
                    return this._document;
                }
            }

            public TypeDocumentItem Item
            {
                get
                {
                    return this._item;
                }
            }
        }
    }
}

