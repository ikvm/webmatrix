namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class WhereClauseBuilder : UserControl
    {
        private Button _andClausesButton;
        private Panel _clausePanel;
        private string _clauseString;
        private TreeView _clauseTreeView;
        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database _database;
        private string _defaultTableName;
        private IDictionary _filters;
        private bool _isDirty;
        private Button _orButton;
        private ArrayList _parameters;
        private IServiceProvider _provider;
        private Button _removeClauseButton;
        private IDictionary _tables;
        private readonly object ClauseAddedEventObject = new object();
        private readonly object ClauseRemovedEventObject = new object();

        public event EventHandler ClauseAdded
        {
            add
            {
                base.Events.AddHandler(this.ClauseAddedEventObject, value);
            }
            remove
            {
                base.Events.RemoveHandler(this.ClauseAddedEventObject, value);
            }
        }

        public event EventHandler ClauseRemoved
        {
            add
            {
                base.Events.AddHandler(this.ClauseRemovedEventObject, value);
            }
            remove
            {
                base.Events.RemoveHandler(this.ClauseRemovedEventObject, value);
            }
        }

        public WhereClauseBuilder()
        {
            this.InitializeComponent();
            this._isDirty = true;
        }

        private void _andClausesButton_Click(object sender, EventArgs e)
        {
            this.AddClause("AND");
        }

        private void _orButton_Click(object sender, EventArgs e)
        {
            this.AddClause("OR");
        }

        private void _removeClauseButton_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = this._clauseTreeView.SelectedNode;
            if (selectedNode != null)
            {
                this._isDirty = true;
                TreeNode parent = selectedNode.Parent;
                if (selectedNode.Nodes.Count > 0)
                {
                    this.RemoveNodesRecursive(selectedNode);
                }
                selectedNode.Remove();
                if (selectedNode is FilterNode)
                {
                    FilterNode node3 = (FilterNode) selectedNode;
                    if (node3.FilterValue.StartsWith("@"))
                    {
                        FilterInfo info = (FilterInfo) this.Filters[node3.FilterValue];
                        info.Count--;
                        if (info.Count == 0)
                        {
                            this.Filters.Remove(node3.FilterValue);
                        }
                    }
                }
                if ((parent != null) && (parent.Nodes.Count == 1))
                {
                    TreeNode node = parent.Nodes[0];
                    node.Remove();
                    TreeNode node5 = parent.Parent;
                    int index = parent.Index;
                    parent.Remove();
                    TreeNodeCollection nodes = null;
                    if (node5 == null)
                    {
                        nodes = this._clauseTreeView.Nodes;
                    }
                    else
                    {
                        nodes = node5.Nodes;
                    }
                    nodes.Insert(index, node);
                }
                this.UpdateButtonText();
                this.OnClauseRemoved();
            }
        }

        private void AddClause(string operatorString)
        {
            IUIService service = (IUIService) this.ServiceProvider.GetService(typeof(IUIService));
            CreateClauseDialog form = new CreateClauseDialog(this.ServiceProvider, this.Database, this.Filters, this.DefaultTableName);
            if (service.ShowDialog(form) == DialogResult.OK)
            {
                this._isDirty = true;
                TreeNode clauseNode = form.ClauseNode;
                if (clauseNode is FilterNode)
                {
                    string filterValue = ((FilterNode) clauseNode).FilterValue;
                    if (filterValue.StartsWith("@"))
                    {
                        FilterInfo info = (FilterInfo) this.Filters[filterValue];
                        if (info == null)
                        {
                            info = new FilterInfo(filterValue, ((FilterNode) clauseNode).Type);
                            this.Filters[filterValue] = info;
                        }
                        info.Count++;
                    }
                }
                if (this._clauseTreeView.Nodes.Count == 0)
                {
                    this._clauseTreeView.Nodes.Add(clauseNode);
                }
                else
                {
                    TreeNode selectedNode = this._clauseTreeView.SelectedNode;
                    if (selectedNode != null)
                    {
                        TreeNode parent = selectedNode.Parent;
                        if ((parent != null) && (parent.Text == operatorString))
                        {
                            parent.Nodes.Add(clauseNode);
                            parent.Expand();
                        }
                        else if (selectedNode.Text == operatorString)
                        {
                            selectedNode.Nodes.Add(clauseNode);
                            selectedNode.Expand();
                        }
                        else
                        {
                            int index = selectedNode.Index;
                            selectedNode.Remove();
                            TreeNode node = new TreeNode(operatorString);
                            node.Nodes.Add(selectedNode);
                            node.Nodes.Add(clauseNode);
                            TreeNodeCollection nodes = null;
                            if (parent == null)
                            {
                                nodes = this._clauseTreeView.Nodes;
                            }
                            else
                            {
                                nodes = parent.Nodes;
                            }
                            nodes.Insert(index, node);
                            node.Expand();
                        }
                    }
                }
                this.UpdateButtonText();
                this.OnClauseAdded();
            }
        }

        private string GetClauseStringRecursive(TreeNode node, IDictionary tables)
        {
            if (node is ClauseNode)
            {
                if (node is FilterNode)
                {
                    FilterNode node2 = (FilterNode) node;
                    if (node2.FilterValue.StartsWith("@"))
                    {
                        IDictionary dictionary = new HybridDictionary(true);
                        if (!dictionary.Contains(node2.FilterValue))
                        {
                            this.ParametersInternal.Add(new QueryParameter(node2.FilterValue, node2.LeftOperand + ' ' + node2.Operator, node2.Type));
                            dictionary.Add(node2.FilterValue, string.Empty);
                        }
                    }
                }
                foreach (string str in ((ClauseNode) node).Tables)
                {
                    if (!tables.Contains(str))
                    {
                        tables[str] = string.Empty;
                    }
                }
            }
            if (node.Nodes.Count == 0)
            {
                return ('(' + node.Text + ')');
            }
            StringBuilder builder = new StringBuilder();
            builder.Append('(');
            int num = 0;
            int count = node.Nodes.Count;
            foreach (TreeNode node3 in node.Nodes)
            {
                builder.Append(this.GetClauseStringRecursive(node3, tables));
                num++;
                if (num < count)
                {
                    builder.Append(' ');
                    builder.Append(node.Text);
                    builder.Append(' ');
                }
            }
            builder.Append(')');
            return builder.ToString();
        }

        private void InitializeComponent()
        {
            this._orButton = new Button();
            this._andClausesButton = new Button();
            this._removeClauseButton = new Button();
            this._clausePanel = new Panel();
            this._clauseTreeView = new TreeView();
            this._clausePanel.SuspendLayout();
            base.SuspendLayout();
            this._orButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._orButton.Enabled = false;
            this._orButton.FlatStyle = FlatStyle.System;
            this._orButton.Location = new Point(0x194, 0x24);
            this._orButton.Name = "_orButton";
            this._orButton.TabIndex = 2;
            this._orButton.Text = "O&R Clause";
            this._orButton.Click += new EventHandler(this._orButton_Click);
            this._andClausesButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._andClausesButton.FlatStyle = FlatStyle.System;
            this._andClausesButton.Location = new Point(0x194, 8);
            this._andClausesButton.Name = "_andClausesButton";
            this._andClausesButton.TabIndex = 1;
            this._andClausesButton.Text = "&WHERE";
            this._andClausesButton.Click += new EventHandler(this._andClausesButton_Click);
            this._removeClauseButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._removeClauseButton.Enabled = false;
            this._removeClauseButton.FlatStyle = FlatStyle.System;
            this._removeClauseButton.Location = new Point(0x194, 0x70);
            this._removeClauseButton.Name = "_removeClauseButton";
            this._removeClauseButton.TabIndex = 3;
            this._removeClauseButton.Text = "&Delete";
            this._removeClauseButton.Click += new EventHandler(this._removeClauseButton_Click);
            this._clausePanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._clausePanel.BackColor = SystemColors.ControlDark;
            this._clausePanel.Controls.AddRange(new Control[] { this._clauseTreeView });
            this._clausePanel.DockPadding.All = 1;
            this._clausePanel.Location = new Point(8, 8);
            this._clausePanel.Name = "_clausePanel";
            this._clausePanel.Size = new Size(0x180, 0x80);
            this._clausePanel.TabIndex = 0;
            this._clauseTreeView.BorderStyle = BorderStyle.None;
            this._clauseTreeView.Dock = DockStyle.Fill;
            this._clauseTreeView.HideSelection = false;
            this._clauseTreeView.ImageIndex = -1;
            this._clauseTreeView.Location = new Point(1, 1);
            this._clauseTreeView.Name = "_clauseTreeView";
            this._clauseTreeView.SelectedImageIndex = -1;
            this._clauseTreeView.Size = new Size(0x17e, 0x7e);
            this._clauseTreeView.TabIndex = 0;
            this._clauseTreeView.AfterSelect += new TreeViewEventHandler(this.OnClauseTreeViewAfterSelect);
            base.Controls.AddRange(new Control[] { this._orButton, this._andClausesButton, this._removeClauseButton, this._clausePanel });
            base.Name = "WhereClauseBuilder";
            base.Size = new Size(0x1e8, 0x94);
            this._clausePanel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected virtual void OnClauseAdded()
        {
            EventHandler handler = (EventHandler) base.Events[this.ClauseAddedEventObject];
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnClauseRemoved()
        {
            EventHandler handler = (EventHandler) base.Events[this.ClauseRemovedEventObject];
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnClauseTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            this.UpdateButtons();
        }

        private void ProcessTree()
        {
            if (this._isDirty)
            {
                this.ParametersInternal.Clear();
                this.TablesInternal.Clear();
                if (this._clauseTreeView.Nodes.Count == 0)
                {
                    this._clauseString = string.Empty;
                    this._isDirty = false;
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (TreeNode node in this._clauseTreeView.Nodes)
                    {
                        builder.Append(this.GetClauseStringRecursive(node, this.TablesInternal));
                    }
                    this._clauseString = builder.ToString();
                    this._isDirty = false;
                }
            }
        }

        private void RemoveNodesRecursive(TreeNode node)
        {
            TreeNodeCollection nodes = node.Nodes;
            if (nodes.Count > 0)
            {
                foreach (TreeNode node2 in nodes)
                {
                    if (node2.Nodes.Count > 0)
                    {
                        this.RemoveNodesRecursive(node2);
                    }
                    if (node2 is FilterNode)
                    {
                        FilterNode node3 = (FilterNode) node2;
                        if (node3.FilterValue.StartsWith("@"))
                        {
                            FilterInfo info = (FilterInfo) this.Filters[node3.FilterValue];
                            info.Count--;
                            if (info.Count == 0)
                            {
                                this.Filters.Remove(node3.FilterValue);
                            }
                        }
                    }
                }
                nodes.Clear();
            }
        }

        private void UpdateButtons()
        {
            if (this._clauseTreeView.Nodes.Count == 0)
            {
                this._andClausesButton.Enabled = true;
                this._orButton.Enabled = false;
                this._orButton.Visible = false;
                this._removeClauseButton.Enabled = false;
            }
            else
            {
                bool flag = this._clauseTreeView.SelectedNode != null;
                this._andClausesButton.Enabled = flag;
                this._orButton.Enabled = flag;
                this._orButton.Visible = flag;
                this._removeClauseButton.Enabled = flag;
            }
        }

        private void UpdateButtonText()
        {
            if (this._clauseTreeView.Nodes.Count == 0)
            {
                this._andClausesButton.Text = "&WHERE";
            }
            else
            {
                this._andClausesButton.Text = "&AND Clause";
                this._andClausesButton.Enabled = this._clauseTreeView.SelectedNode != null;
            }
        }

        public Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database Database
        {
            get
            {
                return this._database;
            }
            set
            {
                if (this._database != value)
                {
                    this._isDirty = true;
                    this._clauseTreeView.Nodes.Clear();
                    this.ProcessTree();
                    this._database = value;
                }
            }
        }

        internal string DefaultTableName
        {
            get
            {
                if (this._defaultTableName == null)
                {
                    return string.Empty;
                }
                return this._defaultTableName;
            }
            set
            {
                this._defaultTableName = value;
            }
        }

        private IDictionary Filters
        {
            get
            {
                if (this._filters == null)
                {
                    this._filters = new HybridDictionary(true);
                }
                return this._filters;
            }
        }

        public ICollection Parameters
        {
            get
            {
                this.ProcessTree();
                return this._parameters;
            }
        }

        private ArrayList ParametersInternal
        {
            get
            {
                if (this._parameters == null)
                {
                    this._parameters = new ArrayList();
                }
                return this._parameters;
            }
        }

        public IServiceProvider ServiceProvider
        {
            get
            {
                return this._provider;
            }
            set
            {
                this._provider = value;
            }
        }

        private IDictionary TablesInternal
        {
            get
            {
                if (this._tables == null)
                {
                    this._tables = new HybridDictionary(true);
                }
                return this._tables;
            }
        }

        public ICollection TablesUsed
        {
            get
            {
                this.ProcessTree();
                return this._tables.Keys;
            }
        }

        public string WhereClause
        {
            get
            {
                this.ProcessTree();
                return this._clauseString;
            }
        }
    }
}

