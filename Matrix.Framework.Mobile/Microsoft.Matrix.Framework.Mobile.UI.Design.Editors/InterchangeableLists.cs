namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    internal sealed class InterchangeableLists : Panel
    {
        private Button _addButton;
        private Label _availableFieldLabel;
        private TreeView _availableList;
        private static readonly object _componentChangedEvent = new object();
        private Button _downButton;
        private Hashtable _eventTable;
        private Button _removeButton;
        private Label _selectedFieldLabel;
        private TreeView _selectedList;
        private Button _upButton;

        internal InterchangeableLists()
        {
            this.InitializeComponent();
            this._downButton.Image = GenericUI.SortDownIcon;
            this._upButton.Image = GenericUI.SortUpIcon;
            this.UpdateButtonEnabling();
            this._eventTable = new Hashtable();
        }

        private void AddItem(TreeView list, TreeNode node)
        {
            list.Nodes.Add(node);
            list.SelectedNode = node;
        }

        private void AddNode(object sender, EventArgs e)
        {
            TreeNode selectedNode = this._availableList.SelectedNode;
            if (selectedNode != null)
            {
                this.RemoveItem(this._availableList, selectedNode);
                this.AddItem(this._selectedList, selectedNode);
                this.UpdateButtonEnabling();
                this.NotifyChangeEvent();
            }
        }

        internal void AddToAvailableList(object obj)
        {
            this.AddItem(this._availableList, new TreeNode(obj.ToString()));
        }

        internal void AddToSelectedList(object obj)
        {
            this.AddItem(this._selectedList, new TreeNode(obj.ToString()));
        }

        private void AvailableList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.UpdateButtonEnabling();
        }

        internal void Clear()
        {
            this._availableList.Nodes.Clear();
            this._selectedList.Nodes.Clear();
        }

        private void Down_Click(object sender, EventArgs e)
        {
            this.MoveItem(1);
            this.UpdateButtonEnabling();
            this.NotifyChangeEvent();
        }

        internal ICollection GetSelectedItems()
        {
            ArrayList list = new ArrayList();
            foreach (TreeNode node in this._selectedList.Nodes)
            {
                list.Add(node.Text);
            }
            return list;
        }

        internal void Initialize()
        {
            if (this._availableList.Nodes.Count > 0)
            {
                this._availableList.SelectedNode = this._availableList.Nodes[0];
            }
            if (this._selectedList.Nodes.Count > 0)
            {
                this._selectedList.SelectedNode = this._selectedList.Nodes[0];
            }
        }

        private void InitializeComponent()
        {
            this._removeButton = new Button();
            this._selectedFieldLabel = new Label();
            this._addButton = new Button();
            this._selectedList = new TreeView();
            this._availableList = new TreeView();
            this._availableFieldLabel = new Label();
            this._upButton = new Button();
            this._downButton = new Button();
            this._removeButton.Location = new Point(0xa6, 0x45);
            this._removeButton.Size = new Size(0x20, 0x19);
            this._removeButton.TabIndex = 4;
            this._removeButton.Text = "<";
            this._removeButton.Click += new EventHandler(this.RemoveNode);
            this._selectedFieldLabel.Location = new Point(0xca, 8);
            this._selectedFieldLabel.Size = new Size(0xa4, 0x10);
            this._selectedFieldLabel.TabIndex = 5;
            this._addButton.Location = new Point(0xa6, 40);
            this._addButton.Size = new Size(0x20, 0x19);
            this._addButton.TabIndex = 3;
            this._addButton.Text = ">";
            this._addButton.Click += new EventHandler(this.AddNode);
            this._selectedList.HideSelection = false;
            this._selectedList.Indent = 15;
            this._selectedList.Location = new Point(0xca, 0x18);
            this._selectedList.ShowLines = false;
            this._selectedList.ShowPlusMinus = false;
            this._selectedList.ShowRootLines = false;
            this._selectedList.Size = new Size(0x9a, 0x59);
            this._selectedList.TabIndex = 6;
            this._selectedList.DoubleClick += new EventHandler(this.RemoveNode);
            this._selectedList.AfterSelect += new TreeViewEventHandler(this.SelectedList_AfterSelect);
            this._availableList.HideSelection = false;
            this._availableList.Indent = 15;
            this._availableList.Location = new Point(8, 0x18);
            this._availableList.ShowLines = false;
            this._availableList.ShowPlusMinus = false;
            this._availableList.ShowRootLines = false;
            this._availableList.Size = new Size(0x9a, 0x59);
            this._availableList.TabIndex = 2;
            this._availableList.DoubleClick += new EventHandler(this.AddNode);
            this._availableList.AfterSelect += new TreeViewEventHandler(this.AvailableList_AfterSelect);
            this._availableFieldLabel.Location = new Point(8, 8);
            this._availableFieldLabel.Size = new Size(0xa4, 0x10);
            this._availableFieldLabel.TabIndex = 1;
            this._upButton.Location = new Point(360, 0x18);
            this._upButton.Size = new Size(0x1c, 0x1b);
            this._upButton.TabIndex = 7;
            this._upButton.Click += new EventHandler(this.Up_Click);
            this._downButton.Location = new Point(360, 0x37);
            this._downButton.Size = new Size(0x1c, 0x1b);
            this._downButton.TabIndex = 8;
            this._downButton.Click += new EventHandler(this.Down_Click);
            base.Controls.AddRange(new Control[] { this._availableFieldLabel, this._selectedFieldLabel, this._upButton, this._downButton, this._removeButton, this._selectedList, this._addButton, this._availableList });
            base.Size = new Size(0x18c, 0x77);
        }

        private void MoveItem(int direction)
        {
            int index = this._selectedList.SelectedNode.Index;
            int num2 = index + direction;
            TreeNode selectedNode = this._selectedList.SelectedNode;
            this._selectedList.Nodes.RemoveAt(index);
            this._selectedList.Nodes.Insert(num2, selectedNode);
            this._selectedList.SelectedNode = selectedNode;
        }

        private void NotifyChangeEvent()
        {
            EventHandler handler = (EventHandler) this._eventTable[_componentChangedEvent];
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void RemoveItem(TreeView list, TreeNode node)
        {
            int count = list.Nodes.Count;
            int index = list.SelectedNode.Index;
            list.Nodes.Remove(node);
            if (index < (count - 1))
            {
                list.SelectedNode = list.Nodes[index];
            }
            else if (index >= 1)
            {
                list.SelectedNode = list.Nodes[index - 1];
            }
            else
            {
                list.SelectedNode = null;
            }
        }

        private void RemoveNode(object sender, EventArgs e)
        {
            TreeNode selectedNode = this._selectedList.SelectedNode;
            if (selectedNode != null)
            {
                this.RemoveItem(this._selectedList, selectedNode);
                this.AddItem(this._availableList, selectedNode);
                this.UpdateButtonEnabling();
            }
            this.NotifyChangeEvent();
        }

        private void SelectedList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.UpdateButtonEnabling();
        }

        internal void SetTitles(string availableListTitle, string selectedListTitle)
        {
            this._selectedFieldLabel.Text = selectedListTitle;
            this._availableFieldLabel.Text = availableListTitle;
        }

        private void Up_Click(object sender, EventArgs e)
        {
            this.MoveItem(-1);
            this.UpdateButtonEnabling();
            this.NotifyChangeEvent();
        }

        private void UpdateButtonEnabling()
        {
            bool flag = this._availableList.SelectedNode != null;
            bool flag2 = this._selectedList.SelectedNode != null;
            this._addButton.Enabled = flag;
            this._removeButton.Enabled = flag2;
            if (flag2)
            {
                int index = this._selectedList.SelectedNode.Index;
                this._upButton.Enabled = index > 0;
                this._downButton.Enabled = index < (this._selectedList.Nodes.Count - 1);
            }
            else
            {
                this._downButton.Enabled = false;
                this._upButton.Enabled = false;
            }
        }

        internal EventHandler OnComponentChanged
        {
            get
            {
                return (EventHandler) this._eventTable[_componentChangedEvent];
            }
            set
            {
                this._eventTable[_componentChangedEvent] = value;
            }
        }
    }
}

