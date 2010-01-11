namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Security.Permissions;
    using System.Web.UI.Design;
    using System.Web.UI.Design.MobileControls;
    using System.Web.UI.MobileControls;
    using System.Windows.Forms;

    internal sealed class ObjectListFieldsPage : ListComponentEditorPage
    {
        private CheckBox _ckbAutoGenerateFields;
        private CheckBox _ckbVisible;
        private UnsettableComboBox _cmbDataField;
        private ObjectList _objectList;
        private TextBox _txtDataFormatString;
        private TextBox _txtTitle;

        public ObjectListFieldsPage()
        {
            base.Y = 0x34;
            base.CaseSensitive = false;
            base.TreeViewTitle = MobileResource.GetString("ObjectListFieldsPage_FieldNameCaption");
            base.AddButtonTitle = MobileResource.GetString("ObjectListFieldsPage_NewFieldBtnCaption");
            base.DefaultName = MobileResource.GetString("ObjectListFieldsPage_DefaultFieldName");
            base.MessageTitle = MobileResource.GetString("ObjectListFieldsPage_ErrorMessageTitle");
            base.EmptyNameMessage = MobileResource.GetString("ObjectListFieldsPage_EmptyNameError");
        }

        protected override void InitForm()
        {
            base.InitForm();
            this._objectList = (ObjectList) base.Component;
            base.CommitOnDeactivate = true;
            base.Icon = new Icon(Type.GetType("System.Web.UI.Design.MobileControls.MobileControlDesigner," + Constants.MobileAssemblyFullName), "Fields.ico");
            base.Size = new Size(0x192, 300);
            this.Text = MobileResource.GetString("ObjectListFieldsPage_Title");
            this._ckbAutoGenerateFields = new CheckBox();
            this._cmbDataField = new UnsettableComboBox();
            this._ckbVisible = new CheckBox();
            this._txtDataFormatString = new TextBox();
            this._txtTitle = new TextBox();
            this._ckbAutoGenerateFields.SetBounds(4, 4, 0x18c, ListComponentEditorPage.LabelHeight);
            this._ckbAutoGenerateFields.Text = MobileResource.GetString("ObjectListFieldsPage_AutoGenerateFieldsCaption");
            this._ckbAutoGenerateFields.FlatStyle = FlatStyle.System;
            this._ckbAutoGenerateFields.CheckedChanged += new EventHandler(this.OnSetPageDirty);
            this._ckbAutoGenerateFields.TabIndex = 0;
            GroupLabel label = new GroupLabel();
            label.SetBounds(4, 0x20, 0x188, ListComponentEditorPage.LabelHeight);
            label.Text = MobileResource.GetString("ObjectListFieldsPage_FieldListGroupLabel");
            label.TabIndex = 1;
            label.TabStop = false;
            base.TreeList.TabIndex = 2;
            Label label2 = new Label();
            label2.SetBounds(ListComponentEditorPage.X, base.Y, ListComponentEditorPage.ControlWidth, ListComponentEditorPage.LabelHeight);
            label2.Text = MobileResource.GetString("ObjectListFieldsPage_DataFieldCaption");
            label2.TabStop = false;
            label2.TabIndex = ListComponentEditorPage.Index;
            base.Y += ListComponentEditorPage.LabelHeight;
            this._cmbDataField.SetBounds(ListComponentEditorPage.X, base.Y, ListComponentEditorPage.ControlWidth, ListComponentEditorPage.CmbHeight);
            this._cmbDataField.DropDownStyle = ComboBoxStyle.DropDown;
            this._cmbDataField.Sorted = true;
            this._cmbDataField.NotSetText = MobileResource.GetString("ObjectListFieldsPage_NoneComboEntry");
            this._cmbDataField.TextChanged += new EventHandler(this.OnPropertyChanged);
            this._cmbDataField.SelectedIndexChanged += new EventHandler(this.OnPropertyChanged);
            this._cmbDataField.TabIndex = ListComponentEditorPage.Index + 1;
            base.Y += ListComponentEditorPage.CellSpace;
            Label label3 = new Label();
            label3.SetBounds(ListComponentEditorPage.X, base.Y, ListComponentEditorPage.ControlWidth, ListComponentEditorPage.LabelHeight);
            label3.Text = MobileResource.GetString("ObjectListFieldsPage_DataFormatStringCaption");
            label3.TabStop = false;
            label3.TabIndex = ListComponentEditorPage.Index + 2;
            base.Y += ListComponentEditorPage.LabelHeight;
            this._txtDataFormatString.SetBounds(ListComponentEditorPage.X, base.Y, ListComponentEditorPage.ControlWidth, ListComponentEditorPage.CmbHeight);
            this._txtDataFormatString.TextChanged += new EventHandler(this.OnPropertyChanged);
            this._txtDataFormatString.TabIndex = ListComponentEditorPage.Index + 3;
            base.Y += ListComponentEditorPage.CellSpace;
            Label label4 = new Label();
            label4.SetBounds(ListComponentEditorPage.X, base.Y, ListComponentEditorPage.ControlWidth, ListComponentEditorPage.LabelHeight);
            label4.Text = MobileResource.GetString("ObjectListFieldsPage_TitleCaption");
            label4.TabStop = false;
            label4.TabIndex = ListComponentEditorPage.Index + 4;
            base.Y += ListComponentEditorPage.LabelHeight;
            this._txtTitle.SetBounds(ListComponentEditorPage.X, base.Y, ListComponentEditorPage.ControlWidth, ListComponentEditorPage.CmbHeight);
            this._txtTitle.TextChanged += new EventHandler(this.OnPropertyChanged);
            this._txtTitle.TabIndex = ListComponentEditorPage.Index + 5;
            base.Y += ListComponentEditorPage.CellSpace;
            this._ckbVisible.SetBounds(ListComponentEditorPage.X, base.Y, ListComponentEditorPage.ControlWidth, ListComponentEditorPage.CmbHeight);
            this._ckbVisible.FlatStyle = FlatStyle.System;
            this._ckbVisible.Text = MobileResource.GetString("ObjectListFieldsPage_VisibleCaption");
            this._ckbVisible.CheckedChanged += new EventHandler(this.OnPropertyChanged);
            this._ckbVisible.TabIndex = ListComponentEditorPage.Index + 6;
            base.Controls.AddRange(new Control[] { this._ckbAutoGenerateFields, label, label2, this._cmbDataField, label3, this._txtDataFormatString, label4, this._txtTitle, this._ckbVisible });
        }

        protected override void InitPage()
        {
            base.InitPage();
            this._cmbDataField.Items.Clear();
            this._cmbDataField.SelectedIndex = -1;
            this._cmbDataField.EnsureNotSetItem();
            this._txtDataFormatString.Text = string.Empty;
            this._txtTitle.Text = string.Empty;
            this._ckbVisible.Checked = true;
            this._ckbAutoGenerateFields.Checked = this._objectList.get_AutoGenerateFields();
            this.LoadDataSourceFields();
        }

        private void LoadDataSourceFields()
        {
            using (new MobileComponentEditorPage.LoadingModeResource(this))
            {
                PropertyDescriptorCollection dataFields = null;
                IEnumerable resolvedSelectedDataSource = ((ObjectListDesigner) base.GetBaseDesigner()).GetResolvedSelectedDataSource();
                if (resolvedSelectedDataSource != null)
                {
                    dataFields = DesignTimeData.GetDataFields(resolvedSelectedDataSource);
                }
                if (dataFields != null)
                {
                    foreach (PropertyDescriptor descriptor in dataFields)
                    {
                        this._cmbDataField.Items.Add(descriptor.Name);
                    }
                }
            }
        }

        protected override void LoadItemProperties()
        {
            using (new MobileComponentEditorPage.LoadingModeResource(this))
            {
                if (base.CurrentNode != null)
                {
                    FieldTreeNode currentNode = (FieldTreeNode) base.CurrentNode;
                    this._cmbDataField.Text = currentNode.DataField;
                    this._txtDataFormatString.Text = currentNode.DataFormatString;
                    this._txtTitle.Text = currentNode.Title;
                    this._ckbVisible.Checked = currentNode.Visible;
                }
                else
                {
                    this._cmbDataField.Text = string.Empty;
                    this._txtDataFormatString.Text = string.Empty;
                    this._txtTitle.Text = string.Empty;
                    this._ckbVisible.Checked = false;
                }
            }
        }

        protected override void LoadItems()
        {
            using (new MobileComponentEditorPage.LoadingModeResource(this))
            {
                foreach (ObjectListField field in this._objectList.get_Fields())
                {
                    FieldTreeNode node = new FieldTreeNode(field.get_Name(), field);
                    base.TreeList.TvList.Nodes.Add(node);
                }
            }
        }

        protected override void OnClickAddButton(object source, EventArgs e)
        {
            if (!base.IsLoading())
            {
                FieldTreeNode node = new FieldTreeNode(this.GetNewName());
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
            if (!base.IsLoading() && (base.CurrentNode != null))
            {
                FieldTreeNode currentNode = (FieldTreeNode) base.CurrentNode;
                if (source == this._cmbDataField)
                {
                    currentNode.DataField = this._cmbDataField.Text;
                }
                else if (source == this._txtDataFormatString)
                {
                    currentNode.DataFormatString = this._txtDataFormatString.Text;
                }
                else if (source == this._txtTitle)
                {
                    currentNode.Title = this._txtTitle.Text;
                }
                else if (source == this._ckbVisible)
                {
                    currentNode.Visible = this._ckbVisible.Checked;
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
            this._objectList.get_Fields().Clear();
            foreach (FieldTreeNode node in base.TreeList.TvList.Nodes)
            {
                if (node.Dirty)
                {
                    node.RuntimeField.set_Name(node.Name);
                    node.RuntimeField.set_DataField(node.DataField);
                    node.RuntimeField.set_DataFormatString(node.DataFormatString);
                    node.RuntimeField.set_Title(node.Title);
                    node.RuntimeField.set_Visible(node.Visible);
                }
                this._objectList.get_Fields().AddAt(-1, node.RuntimeField);
            }
            this._objectList.set_AutoGenerateFields(this._ckbAutoGenerateFields.Checked);
            TypeDescriptor.Refresh(this._objectList);
        }

        protected override void UpdateControlsEnabling()
        {
            base.TreeList.TvList.Enabled = this._cmbDataField.Enabled = this._txtDataFormatString.Enabled = this._txtTitle.Enabled = this._ckbVisible.Enabled = base.TreeList.TvList.SelectedNode != null;
        }

        protected override string HelpKeyword
        {
            get
            {
                return "net.Mobile.ObjectListProperties.Fields";
            }
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        private class FieldTreeNode : ListComponentEditorPage.ListTreeNode
        {
            private string _dataField;
            private string _dataFormatString;
            private ObjectListField _runtimeField;
            private string _title;
            private bool _visible;

            internal FieldTreeNode(string fieldID) : this(fieldID, new ObjectListField())
            {
            }

            internal FieldTreeNode(string fieldID, ObjectListField runtimeField) : base(fieldID)
            {
                this._runtimeField = runtimeField;
                this.LoadAttributes();
            }

            private void LoadAttributes()
            {
                this.DataField = this.RuntimeField.get_DataField();
                this.DataFormatString = this.RuntimeField.get_DataFormatString();
                this.Title = this.RuntimeField.get_Title();
                this.Visible = this.RuntimeField.get_Visible();
            }

            internal string DataField
            {
                get
                {
                    return this._dataField;
                }
                set
                {
                    this._dataField = value;
                }
            }

            internal string DataFormatString
            {
                get
                {
                    return this._dataFormatString;
                }
                set
                {
                    this._dataFormatString = value;
                }
            }

            internal ObjectListField RuntimeField
            {
                get
                {
                    return this._runtimeField;
                }
            }

            internal string Title
            {
                get
                {
                    return this._title;
                }
                set
                {
                    this._title = value;
                }
            }

            internal bool Visible
            {
                get
                {
                    return this._visible;
                }
                set
                {
                    this._visible = value;
                }
            }
        }
    }
}

