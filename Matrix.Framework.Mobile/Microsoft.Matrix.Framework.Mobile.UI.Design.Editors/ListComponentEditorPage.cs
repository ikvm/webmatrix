namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using System;
    using System.ComponentModel;
    using System.Security.Permissions;
    using System.Windows.Forms;

    internal abstract class ListComponentEditorPage : MobileComponentEditorPage
    {
        protected string AddButtonTitle = string.Empty;
        protected bool CaseSensitive;
        protected static readonly int CellSpace = 0x1b;
        protected static readonly int CmbHeight = 20;
        protected static readonly int ControlWidth = 0x98;
        protected ListTreeNode CurrentNode = null;
        protected string DefaultName = string.Empty;
        protected string EmptyNameMessage = string.Empty;
        protected static readonly int Index = 200;
        protected static readonly int LabelHeight = 0x10;
        protected string MessageTitle = string.Empty;
        protected EditableTreeList TreeList = null;
        protected string TreeViewTitle = string.Empty;
        protected static readonly int X = 0xee;
        protected int Y = 0x10;

        protected ListComponentEditorPage()
        {
        }

        protected virtual bool FilterIllegalName()
        {
            return true;
        }

        protected virtual string GetNewName()
        {
            int num = 1;
            while (this.NameExists(this.DefaultName + num.ToString()))
            {
                num++;
            }
            return (this.DefaultName + num.ToString());
        }

        protected virtual void InitForm()
        {
            this.TreeList = new EditableTreeList(true, this.CaseSensitive, this.Y);
            this.TreeList.TabIndex = 0;
            this.TreeList.LblTitle.Text = this.TreeViewTitle;
            this.TreeList.BtnAdd.Text = this.AddButtonTitle;
            this.TreeList.TvList.AfterLabelEdit += new NodeLabelEditEventHandler(this.OnAfterLabelEdit);
            this.TreeList.TvList.BeforeLabelEdit += new NodeLabelEditEventHandler(this.OnBeforeLabelEdit);
            this.TreeList.TvList.AfterSelect += new TreeViewEventHandler(this.OnNodeSelected);
            this.TreeList.BtnAdd.Click += new EventHandler(this.OnClickAddButton);
            this.TreeList.BtnRemove.Click += new EventHandler(this.OnClickRemoveButton);
            this.TreeList.BtnUp.Click += new EventHandler(this.OnClickUpButton);
            this.TreeList.BtnDown.Click += new EventHandler(this.OnClickDownButton);
            base.Controls.AddRange(new Control[] { this.TreeList });
        }

        protected virtual void InitPage()
        {
            this.TreeList.TvList.Nodes.Clear();
            this.TreeList.TvList.SelectedNode = null;
        }

        private void InitTree()
        {
            this.LoadItems();
            if (this.TreeList.TvList.Nodes.Count > 0)
            {
                this.CurrentNode = (ListTreeNode) this.TreeList.TvList.Nodes[0];
                this.TreeList.TvList.SelectedNode = this.CurrentNode;
                this.LoadItemProperties();
            }
        }

        protected sealed override void LoadComponent()
        {
            this.InitPage();
            this.InitTree();
            this.UpdateControlsEnabling();
        }

        protected abstract void LoadItemProperties();
        protected abstract void LoadItems();
        protected bool NameExists(string name)
        {
            foreach (ListTreeNode node in this.TreeList.TvList.Nodes)
            {
                if (string.Compare(node.Name, name, !this.CaseSensitive) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual void OnAfterLabelEdit(object source, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (this.FilterIllegalName())
                {
                    bool flag = true;
                    if (string.Empty == e.Label)
                    {
                        MessageBox.Show(this.EmptyNameMessage, this.MessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        flag = false;
                    }
                    if (flag)
                    {
                        e.CancelEdit = true;
                        return;
                    }
                }
                this.CurrentNode.Name = e.Label;
                this.CurrentNode.Dirty = true;
                this.SetDirty();
                this.OnNodeRenamed();
            }
        }

        private void OnBeforeLabelEdit(object source, NodeLabelEditEventArgs e)
        {
            this.SetDirty();
        }

        protected virtual void OnClickAddButton(object source, EventArgs e)
        {
            if (!base.IsLoading())
            {
                this.SetDirty();
            }
        }

        private void OnClickDownButton(object source, EventArgs e)
        {
            if (!base.IsLoading())
            {
                this.SetDirty();
            }
        }

        protected virtual void OnClickRemoveButton(object source, EventArgs e)
        {
            if (!base.IsLoading())
            {
                if (this.TreeList.TvList.Nodes.Count == 0)
                {
                    this.CurrentNode = null;
                    this.LoadItemProperties();
                }
                this.SetDirty();
                this.UpdateControlsEnabling();
            }
        }

        private void OnClickUpButton(object source, EventArgs e)
        {
            if (!base.IsLoading())
            {
                this.SetDirty();
            }
        }

        protected virtual void OnNodeRenamed()
        {
        }

        protected virtual void OnNodeSelected(object source, TreeViewEventArgs e)
        {
            if (!base.IsLoading())
            {
                this.CurrentNode = (ListTreeNode) this.TreeList.TvList.SelectedNode;
                this.LoadItemProperties();
                this.UpdateControlsEnabling();
            }
        }

        protected virtual void OnPropertyChanged(object source, EventArgs e)
        {
        }

        protected override void SaveComponent()
        {
            foreach (ListTreeNode node in this.TreeList.TvList.Nodes)
            {
                if (node.IsEditing)
                {
                    node.EndEdit(false);
                }
            }
        }

        public sealed override void SetComponent(IComponent component)
        {
            base.SetComponent(component);
            this.InitForm();
        }

        protected virtual void UpdateControlsEnabling()
        {
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        protected class ListTreeNode : TreeNode
        {
            private bool _dirty;
            private string _name;

            internal ListTreeNode(string text) : base(text)
            {
                this._name = text;
            }

            internal bool Dirty
            {
                get
                {
                    return this._dirty;
                }
                set
                {
                    this._dirty = value;
                }
            }

            internal string Name
            {
                get
                {
                    return this._name;
                }
                set
                {
                    this._name = value;
                }
            }
        }
    }
}

