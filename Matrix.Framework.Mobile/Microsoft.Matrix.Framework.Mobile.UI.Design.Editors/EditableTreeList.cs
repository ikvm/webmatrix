namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Web.UI.Design.MobileControls;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    internal class EditableTreeList : Panel
    {
        private const string _assertMsgNullNodeSelected = "Caller is responsible for ensuring a TreeNode is selected. Modified TreeViewNode without calling UpdateButtonsEnabling()?";
        private const string _assertMsgOutOfBounds = "Caller is responsible for ensuring this action does not move the selected TreeViewNode out of bounds. Modified TvList without calling UpdateButtonsEnabling()?";
        private bool _caseSensitive;
        internal Button BtnAdd;
        internal Button BtnDown;
        internal Button BtnRemove;
        internal Button BtnUp;
        internal ContextMenu CntxtMenu;
        internal MenuItem CntxtMenuItem;
        internal TreeNode EditCandidateNode;
        internal TreeNode LastNodeChanged;
        internal Label LblTitle;
        internal EventHandler RemoveHandler;
        internal TreeView TvList;

        internal EditableTreeList() : this(true, true, 0x10)
        {
        }

        internal EditableTreeList(bool showAddButton, bool caseSensitive, int Y)
        {
            this.LastNodeChanged = null;
            this.EditCandidateNode = null;
            this.TvList = new TreeView();
            this.BtnAdd = new Button();
            this.BtnDown = new Button();
            this.LblTitle = new Label();
            this.BtnUp = new Button();
            this.BtnRemove = new Button();
            this.CntxtMenuItem = new MenuItem();
            this.CntxtMenu = new ContextMenu();
            this.LblTitle.Size = new Size(210, 0x10);
            this.LblTitle.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.TvList.Location = new Point(0, 0x10);
            this.TvList.Size = new Size(0xb2, 0x94);
            this.TvList.ForeColor = SystemColors.WindowText;
            this.TvList.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.TvList.LabelEdit = true;
            this.TvList.ShowPlusMinus = false;
            this.TvList.HideSelection = false;
            this.TvList.Indent = 15;
            this.TvList.ShowRootLines = false;
            this.TvList.ShowLines = false;
            this.TvList.ContextMenu = this.CntxtMenu;
            this.BtnUp.Location = new Point(0xb6, 0x10);
            this.BtnUp.Size = new Size(0x1c, 0x1b);
            this.BtnUp.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.BtnDown.Location = new Point(0xb6, 0x30);
            this.BtnDown.Size = new Size(0x1c, 0x1b);
            this.BtnDown.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.BtnRemove.Location = new Point(0xb6, 0x88);
            this.BtnRemove.Size = new Size(0x1c, 0x1b);
            this.BtnRemove.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.BtnAdd.Location = new Point(0, 0xa8);
            this.BtnAdd.Size = new Size(0xb2, 0x19);
            this.BtnAdd.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.CntxtMenuItem.Text = MobileResource.GetString("EditableTreeList_Rename");
            this.CntxtMenu.MenuItems.Add(this.CntxtMenuItem);
            base.Location = new Point(8, Y);
            base.Size = new Size(210, 0xc4);
            base.Controls.Add(this.LblTitle);
            base.Controls.Add(this.TvList);
            base.Controls.Add(this.BtnUp);
            base.Controls.Add(this.BtnDown);
            base.Controls.Add(this.BtnRemove);
            base.Controls.Add(this.BtnAdd);
            this.BtnDown.Image = GenericUI.SortDownIcon;
            this.BtnUp.Image = GenericUI.SortUpIcon;
            this.BtnRemove.Image = GenericUI.DeleteIcon;
            this.BtnUp.Click += new EventHandler(this.MoveSelectedItemUp);
            this.BtnDown.Click += new EventHandler(this.MoveSelectedItemDown);
            this.RemoveHandler = new EventHandler(this.OnRemove);
            this.BtnRemove.Click += this.RemoveHandler;
            this.TvList.AfterSelect += new TreeViewEventHandler(this.OnListSelect);
            this.TvList.KeyDown += new KeyEventHandler(this.OnKeyDown);
            this.TvList.MouseUp += new MouseEventHandler(this.OnListMouseUp);
            this.TvList.MouseDown += new MouseEventHandler(this.OnListMouseDown);
            this.CntxtMenu.Popup += new EventHandler(this.OnPopup);
            this.CntxtMenuItem.Click += new EventHandler(this.OnContextMenuItemClick);
            this.UpdateButtonsEnabling();
            if (!showAddButton)
            {
                this.BtnAdd.Visible = false;
                int num = 4 + this.BtnAdd.Height;
                this.TvList.Height += num;
                this.BtnRemove.Top += num;
            }
            this._caseSensitive = caseSensitive;
        }

        internal string GetUniqueLabel(string label)
        {
            int num = 1;
            string str = label + num;
            while (this.LabelExists(str))
            {
                str = label + ++num;
            }
            return str;
        }

        internal bool LabelExists(string label)
        {
            foreach (TreeNode node in this.TvList.Nodes)
            {
                if (string.Compare(node.Text, label, !this._caseSensitive) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        internal void MoveSelectedItemDown(object sender, EventArgs e)
        {
            this.MoveSelectedNode(1);
            this.UpdateButtonsEnabling();
        }

        internal void MoveSelectedItemUp(object sender, EventArgs e)
        {
            this.MoveSelectedNode(-1);
            this.UpdateButtonsEnabling();
        }

        private void MoveSelectedNode(int direction)
        {
            this.LastNodeChanged = this.TvList.SelectedNode;
            int index = this.LastNodeChanged.Index;
            this.TvList.Nodes.RemoveAt(index);
            this.TvList.Nodes.Insert(index + direction, this.LastNodeChanged);
            this.TvList.SelectedNode = this.LastNodeChanged;
        }

        private void OnContextMenuItemClick(object sender, EventArgs e)
        {
            if (this.EditCandidateNode == null)
            {
                if (this.TvList.SelectedNode != null)
                {
                    this.TvList.SelectedNode.BeginEdit();
                }
            }
            else
            {
                this.EditCandidateNode.BeginEdit();
            }
            this.EditCandidateNode = null;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case (Keys.Control | Keys.End):
                {
                    int count = this.TvList.Nodes.Count;
                    if (count <= 0)
                    {
                        break;
                    }
                    this.TvList.SelectedNode = this.TvList.Nodes[count - 1];
                    return;
                }
                case (Keys.Control | Keys.Home):
                    if (this.TvList.Nodes.Count <= 0)
                    {
                        break;
                    }
                    this.TvList.SelectedNode = this.TvList.Nodes[0];
                    return;

                case Keys.F2:
                {
                    TreeNode selectedNode = this.TvList.SelectedNode;
                    if (selectedNode != null)
                    {
                        selectedNode.BeginEdit();
                        return;
                    }
                    break;
                }
                default:
                    return;
            }
        }

        private void OnListMouseDown(object sender, MouseEventArgs e)
        {
            this.EditCandidateNode = null;
            if (e.Button == MouseButtons.Right)
            {
                this.EditCandidateNode = this.TvList.GetNodeAt(e.X, e.Y);
            }
        }

        private void OnListMouseUp(object sender, MouseEventArgs e)
        {
            this.EditCandidateNode = null;
            if (e.Button == MouseButtons.Right)
            {
                this.EditCandidateNode = this.TvList.GetNodeAt(e.X, e.Y);
            }
        }

        private void OnListSelect(object sender, TreeViewEventArgs e)
        {
            this.UpdateButtonsEnabling();
        }

        private void OnPopup(object sender, EventArgs e)
        {
            this.CntxtMenuItem.Enabled = (this.EditCandidateNode != null) || (this.TvList.SelectedNode != null);
        }

        private void OnRemove(object sender, EventArgs e)
        {
            this.RemoveSelectedItem();
        }

        internal void RemoveSelectedItem()
        {
            this.LastNodeChanged = this.SelectedNodeChecked;
            this.TvList.Nodes.Remove(this.LastNodeChanged);
            this.UpdateButtonsEnabling();
        }

        internal void UpdateButtonsEnabling()
        {
            int selectedIndex = this.SelectedIndex;
            bool flag = selectedIndex >= 0;
            this.BtnRemove.Enabled = flag;
            if (flag)
            {
                this.BtnUp.Enabled = selectedIndex > 0;
                this.BtnDown.Enabled = selectedIndex < (this.TvList.Nodes.Count - 1);
            }
            else
            {
                this.BtnUp.Enabled = false;
                this.BtnDown.Enabled = false;
            }
        }

        internal int SelectedIndex
        {
            get
            {
                TreeNode selectedNode = this.TvList.SelectedNode;
                if (selectedNode != null)
                {
                    return selectedNode.Index;
                }
                return -1;
            }
        }

        internal TreeNode SelectedNode
        {
            get
            {
                return this.TvList.SelectedNode;
            }
            set
            {
                this.TvList.SelectedNode = value;
            }
        }

        private TreeNode SelectedNodeChecked
        {
            get
            {
                return this.TvList.SelectedNode;
            }
        }
    }
}

