namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Security.Permissions;
    using System.Web.UI.Design.MobileControls;
    using System.Web.UI.MobileControls;
    using System.Windows.Forms;

    internal sealed class ListItemsPage : ListComponentEditorPage
    {
        private CheckBox _ckbSelected;
        private bool _isBaseControlList;
        private CheckBox _itemsAsLinksCheckBox;
        private IListDesigner _listDesigner;
        private TextBox _txtValue;

        public ListItemsPage()
        {
            base.TreeViewTitle = MobileResource.GetString("ListItemsPage_ItemCaption");
            base.AddButtonTitle = MobileResource.GetString("ListItemsPage_NewItemCaption");
            base.DefaultName = MobileResource.GetString("ListItemsPage_DefaultItemText");
        }

        protected override bool FilterIllegalName()
        {
            return false;
        }

        protected override string GetNewName()
        {
            return MobileResource.GetString("ListItemsPage_DefaultItemText");
        }

        protected override void InitForm()
        {
            this._isBaseControlList = base.GetBaseControl() is List;
            this._listDesigner = (IListDesigner) base.GetBaseDesigner();
            base.Y = this._isBaseControlList ? 0x34 : 0x18;
            base.InitForm();
            this.Text = MobileResource.GetString("ListItemsPage_Title");
            base.CommitOnDeactivate = true;
            base.Icon = new Icon(Type.GetType("System.Web.UI.Design.MobileControls.MobileControlDesigner," + Constants.MobileAssemblyFullName), "Items.ico");
            base.Size = new Size(0x17e, 220);
            if (this._isBaseControlList)
            {
                this._itemsAsLinksCheckBox = new CheckBox();
                this._itemsAsLinksCheckBox.SetBounds(4, 4, 370, 0x10);
                this._itemsAsLinksCheckBox.Text = MobileResource.GetString("ListItemsPage_ItemsAsLinksCaption");
                this._itemsAsLinksCheckBox.FlatStyle = FlatStyle.System;
                this._itemsAsLinksCheckBox.CheckedChanged += new EventHandler(this.OnSetPageDirty);
                this._itemsAsLinksCheckBox.TabIndex = 0;
            }
            GroupLabel label = new GroupLabel();
            label.SetBounds(4, this._isBaseControlList ? 0x20 : 4, 0x174, ListComponentEditorPage.LabelHeight);
            label.Text = MobileResource.GetString("ListItemsPage_ItemListGroupLabel");
            label.TabIndex = 1;
            label.TabStop = false;
            base.TreeList.TabIndex = 2;
            Label label2 = new Label();
            label2.SetBounds(ListComponentEditorPage.X, base.Y, 0x86, ListComponentEditorPage.LabelHeight);
            label2.Text = MobileResource.GetString("ListItemsPage_ItemValueCaption");
            label2.TabStop = false;
            label2.TabIndex = ListComponentEditorPage.Index;
            base.Y += ListComponentEditorPage.LabelHeight;
            this._txtValue = new TextBox();
            this._txtValue.SetBounds(ListComponentEditorPage.X, base.Y, 0x86, ListComponentEditorPage.CmbHeight);
            this._txtValue.TextChanged += new EventHandler(this.OnPropertyChanged);
            this._txtValue.TabIndex = ListComponentEditorPage.Index + 1;
            base.Controls.AddRange(new Control[] { label, label2, this._txtValue });
            if (this._isBaseControlList)
            {
                base.Controls.Add(this._itemsAsLinksCheckBox);
            }
            else
            {
                base.Y += ListComponentEditorPage.CellSpace;
                this._ckbSelected = new CheckBox();
                this._ckbSelected.SetBounds(ListComponentEditorPage.X, base.Y, 0x86, ListComponentEditorPage.LabelHeight);
                this._ckbSelected.FlatStyle = FlatStyle.System;
                this._ckbSelected.Text = MobileResource.GetString("ListItemsPage_ItemSelectedCaption");
                this._ckbSelected.CheckedChanged += new EventHandler(this.OnPropertyChanged);
                this._ckbSelected.TabIndex = ListComponentEditorPage.Index + 2;
                base.Controls.Add(this._ckbSelected);
            }
        }

        protected override void InitPage()
        {
            base.InitPage();
            if (this._isBaseControlList)
            {
                this._itemsAsLinksCheckBox.Checked = base.GetBaseControl().get_ItemsAsLinks();
            }
            else
            {
                this._ckbSelected.Checked = false;
            }
            this._txtValue.Text = string.Empty;
        }

        protected override void LoadItemProperties()
        {
            using (new MobileComponentEditorPage.LoadingModeResource(this))
            {
                if (base.CurrentNode != null)
                {
                    ItemTreeNode currentNode = (ItemTreeNode) base.CurrentNode;
                    this._txtValue.Text = currentNode.Value;
                    if (!this._isBaseControlList)
                    {
                        this._ckbSelected.Checked = currentNode.Selected;
                    }
                }
                else
                {
                    this._txtValue.Text = string.Empty;
                    if (!this._isBaseControlList)
                    {
                        this._ckbSelected.Checked = false;
                    }
                }
            }
        }

        protected override void LoadItems()
        {
            using (new MobileComponentEditorPage.LoadingModeResource(this))
            {
                foreach (MobileListItem item in this._listDesigner.Items)
                {
                    ItemTreeNode node = new ItemTreeNode(item);
                    base.TreeList.TvList.Nodes.Add(node);
                }
            }
        }

        protected override void OnAfterLabelEdit(object source, NodeLabelEditEventArgs e)
        {
            base.OnAfterLabelEdit(source, e);
            if (!((ItemTreeNode) base.CurrentNode).ValueSet)
            {
                this._txtValue.Text = ((ItemTreeNode) base.CurrentNode).Value = base.CurrentNode.Name;
            }
        }

        protected override void OnClickAddButton(object source, EventArgs e)
        {
            if (!base.IsLoading())
            {
                ItemTreeNode node = new ItemTreeNode(this.GetNewName());
                base.TreeList.TvList.Nodes.Add(node);
                base.TreeList.TvList.SelectedNode = node;
                base.CurrentNode = node;
                node.Dirty = true;
                node.BeginEdit();
                this.LoadItemProperties();
                this.SetDirty();
            }
        }

        protected override void OnPropertyChanged(object source, EventArgs e)
        {
            if ((base.CurrentNode != null) && !base.IsLoading())
            {
                if (source is TextBox)
                {
                    ((ItemTreeNode) base.CurrentNode).Value = this._txtValue.Text;
                }
                else
                {
                    ((ItemTreeNode) base.CurrentNode).Selected = this._ckbSelected.Checked;
                }
                this.SetDirty();
                base.CurrentNode.Dirty = true;
            }
        }

        private void OnSetPageDirty(object source, EventArgs e)
        {
            if (!base.IsLoading())
            {
                this.SetDirty();
            }
        }

        protected override void SaveComponent()
        {
            base.SaveComponent();
            this._listDesigner.Items.Clear();
            foreach (ItemTreeNode node in base.TreeList.TvList.Nodes)
            {
                if (node.Dirty)
                {
                    node.RuntimeItem.set_Text(node.Text);
                    node.RuntimeItem.set_Value(node.Value);
                    if (!this._isBaseControlList)
                    {
                        node.RuntimeItem.set_Selected(node.Selected);
                    }
                }
                this._listDesigner.Items.Add(node.RuntimeItem);
            }
            if (this._isBaseControlList)
            {
                List baseControl = base.GetBaseControl();
                baseControl.set_ItemsAsLinks(this._itemsAsLinksCheckBox.Checked);
                TypeDescriptor.Refresh(baseControl);
            }
            else
            {
                SelectionList component = base.GetBaseControl();
                TypeDescriptor.Refresh(component);
            }
        }

        protected override void UpdateControlsEnabling()
        {
            if (base.TreeList.TvList.SelectedNode == null)
            {
                base.TreeList.TvList.Enabled = this._txtValue.Enabled = false;
                this._txtValue.Text = string.Empty;
            }
            else
            {
                base.TreeList.TvList.Enabled = this._txtValue.Enabled = true;
            }
            if (!this._isBaseControlList)
            {
                SelectionList baseControl = base.GetBaseControl();
                if (base.TreeList.TvList.SelectedNode == null)
                {
                    this._ckbSelected.Enabled = false;
                    this._ckbSelected.Checked = false;
                }
                else
                {
                    this._ckbSelected.Enabled = true;
                }
            }
        }

        protected override string HelpKeyword
        {
            get
            {
                if (this._isBaseControlList)
                {
                    return "net.Mobile.ListProperties.Items";
                }
                return "net.Mobile.SelectionListProperties.Items";
            }
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        private class ItemTreeNode : ListComponentEditorPage.ListTreeNode
        {
            private MobileListItem _runtimeItem;
            private bool _selected;
            private string _value;
            private bool _valueSet;

            internal ItemTreeNode(string itemText) : base(itemText)
            {
                this._valueSet = false;
                this._runtimeItem = new MobileListItem();
                this._value = null;
                this._selected = false;
            }

            internal ItemTreeNode(MobileListItem runtimeItem) : base(runtimeItem.get_Text())
            {
                this._valueSet = false;
                this._valueSet = true;
                this._runtimeItem = runtimeItem;
                this._value = this._runtimeItem.get_Value();
                this._selected = this._runtimeItem.get_Selected();
            }

            internal MobileListItem RuntimeItem
            {
                get
                {
                    return this._runtimeItem;
                }
            }

            internal bool Selected
            {
                get
                {
                    return this._selected;
                }
                set
                {
                    this._selected = value;
                }
            }

            internal string Value
            {
                get
                {
                    return this._value;
                }
                set
                {
                    this._value = value;
                    this._valueSet = true;
                }
            }

            internal bool ValueSet
            {
                get
                {
                    return this._valueSet;
                }
            }
        }
    }
}

