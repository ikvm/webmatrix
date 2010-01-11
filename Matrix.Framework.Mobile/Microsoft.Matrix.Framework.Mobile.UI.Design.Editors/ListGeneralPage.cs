namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using System;
    using System.CodeDom;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Security.Permissions;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.Design.MobileControls;
    using System.Web.UI.MobileControls;
    using System.Web.UI.WebControls;
    using System.Windows.Forms;

    internal class ListGeneralPage : MobileComponentEditorPage
    {
        private DataSourceItem _currentDataSource;
        private UnsettableComboBox _dataMemberCombo;
        private UnsettableComboBox _dataSourceCombo;
        private bool _dataSourceDirty;
        private UnsettableComboBox _dataTextFieldCombo;
        private UnsettableComboBox _dataValueFieldCombo;
        private ComboBox _decorationCombo;
        private bool _isBaseControlList;
        private TextBox _itemCountTextBox;
        private TextBox _itemsPerPageTextBox;
        private TextBox _rowsTextBox;
        private ComboBox _selectTypeCombo;
        private const int IDX_DECORATION_BULLETED = 1;
        private const int IDX_DECORATION_NONE = 0;
        private const int IDX_DECORATION_NUMBERED = 2;
        private const int IDX_SELECTTYPE_CHECKBOX = 4;
        private const int IDX_SELECTTYPE_DROPDOWN = 0;
        private const int IDX_SELECTTYPE_LISTBOX = 1;
        private const int IDX_SELECTTYPE_MULTISELECTLISTBOX = 3;
        private const int IDX_SELECTTYPE_RADIO = 2;

        private void InitForm()
        {
            this._isBaseControlList = base.GetBaseControl() is List;
            GroupLabel label = new GroupLabel();
            Label label2 = new Label();
            this._dataSourceCombo = new UnsettableComboBox();
            Label label3 = new Label();
            this._dataMemberCombo = new UnsettableComboBox();
            Label label4 = new Label();
            this._dataTextFieldCombo = new UnsettableComboBox();
            Label label5 = new Label();
            this._dataValueFieldCombo = new UnsettableComboBox();
            GroupLabel label6 = new GroupLabel();
            GroupLabel label7 = null;
            Label label8 = null;
            Label label9 = null;
            Label label10 = null;
            Label label11 = null;
            Label label12 = null;
            if (this._isBaseControlList)
            {
                label7 = new GroupLabel();
                label8 = new Label();
                this._itemCountTextBox = new TextBox();
                label9 = new Label();
                this._itemsPerPageTextBox = new TextBox();
                label11 = new Label();
                this._decorationCombo = new ComboBox();
            }
            else
            {
                label10 = new Label();
                this._rowsTextBox = new TextBox();
                label12 = new Label();
                this._selectTypeCombo = new ComboBox();
            }
            label.SetBounds(4, 4, 0x174, 0x10);
            label.Text = MobileResource.GetString("ListGeneralPage_DataGroupLabel");
            label.TabIndex = 0;
            label.TabStop = false;
            label2.SetBounds(8, 0x18, 0xa1, 0x10);
            label2.Text = MobileResource.GetString("ListGeneralPage_DataSourceCaption");
            label2.TabStop = false;
            label2.TabIndex = 1;
            this._dataSourceCombo.SetBounds(8, 40, 0xa1, 0x15);
            this._dataSourceCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            this._dataSourceCombo.Sorted = true;
            this._dataSourceCombo.TabIndex = 2;
            this._dataSourceCombo.NotSetText = MobileResource.GetString("ListGeneralPage_UnboundComboEntry");
            this._dataSourceCombo.SelectedIndexChanged += new EventHandler(this.OnSelChangedDataSource);
            label3.SetBounds(0xd3, 0x18, 0xa1, 0x10);
            label3.Text = MobileResource.GetString("ListGeneralPage_DataMemberCaption");
            label3.TabStop = false;
            label3.TabIndex = 3;
            this._dataMemberCombo.SetBounds(0xd3, 40, 0xa1, 0x15);
            this._dataMemberCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            this._dataMemberCombo.Sorted = true;
            this._dataMemberCombo.TabIndex = 4;
            this._dataMemberCombo.NotSetText = MobileResource.GetString("ListGeneralPage_NoneComboEntry");
            this._dataMemberCombo.SelectedIndexChanged += new EventHandler(this.OnSelChangedDataMember);
            label4.SetBounds(8, 0x43, 0xa1, 0x10);
            label4.Text = MobileResource.GetString("ListGeneralPage_DataTextFieldCaption");
            label4.TabStop = false;
            label4.TabIndex = 5;
            this._dataTextFieldCombo.SetBounds(8, 0x53, 0xa1, 0x15);
            this._dataTextFieldCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            this._dataTextFieldCombo.Sorted = true;
            this._dataTextFieldCombo.TabIndex = 6;
            this._dataTextFieldCombo.NotSetText = MobileResource.GetString("ListGeneralPage_NoneComboEntry");
            this._dataTextFieldCombo.SelectedIndexChanged += new EventHandler(this.OnSetPageDirty);
            label5.SetBounds(0xd3, 0x43, 0xa1, 0x10);
            label5.Text = MobileResource.GetString("ListGeneralPage_DataValueFieldCaption");
            label5.TabStop = false;
            label5.TabIndex = 7;
            this._dataValueFieldCombo.SetBounds(0xd3, 0x53, 0xa1, 0x15);
            this._dataValueFieldCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            this._dataValueFieldCombo.Sorted = true;
            this._dataValueFieldCombo.TabIndex = 8;
            this._dataValueFieldCombo.NotSetText = MobileResource.GetString("ListGeneralPage_NoneComboEntry");
            this._dataValueFieldCombo.SelectedIndexChanged += new EventHandler(this.OnSetPageDirty);
            label6.SetBounds(4, 120, 0x174, 0x10);
            label6.Text = MobileResource.GetString("ListGeneralPage_AppearanceGroupLabel");
            label6.TabIndex = 9;
            label6.TabStop = false;
            if (this._isBaseControlList)
            {
                label11.SetBounds(8, 140, 200, 0x10);
                label11.Text = MobileResource.GetString("ListGeneralPage_DecorationCaption");
                label11.TabStop = false;
                label11.TabIndex = 10;
                this._decorationCombo.SetBounds(8, 0x9c, 0xa1, 0x15);
                this._decorationCombo.DropDownStyle = ComboBoxStyle.DropDownList;
                this._decorationCombo.SelectedIndexChanged += new EventHandler(this.OnSetPageDirty);
                this._decorationCombo.Items.AddRange(new object[] { MobileResource.GetString("ListGeneralPage_DecorationNone"), MobileResource.GetString("ListGeneralPage_DecorationBulleted"), MobileResource.GetString("ListGeneralPage_DecorationNumbered") });
                this._decorationCombo.TabIndex = 11;
                label7.SetBounds(4, 0xc1, 0x174, 0x10);
                label7.Text = MobileResource.GetString("ListGeneralPage_PagingGroupLabel");
                label7.TabIndex = 12;
                label7.TabStop = false;
                label8.SetBounds(8, 0xd5, 0xa1, 0x10);
                label8.Text = MobileResource.GetString("ListGeneralPage_ItemCountCaption");
                label8.TabStop = false;
                label8.TabIndex = 13;
                this._itemCountTextBox.SetBounds(8, 0xe5, 0xa1, 20);
                this._itemCountTextBox.TextChanged += new EventHandler(this.OnSetPageDirty);
                this._itemCountTextBox.KeyPress += new KeyPressEventHandler(this.OnKeyPressNumberTextBox);
                this._itemCountTextBox.TabIndex = 14;
                label9.SetBounds(0xd3, 0xd5, 0xa1, 0x10);
                label9.Text = MobileResource.GetString("ListGeneralPage_ItemsPerPageCaption");
                label9.TabStop = false;
                label9.TabIndex = 15;
                this._itemsPerPageTextBox.SetBounds(0xd3, 0xe5, 0xa1, 20);
                this._itemsPerPageTextBox.TextChanged += new EventHandler(this.OnSetPageDirty);
                this._itemsPerPageTextBox.KeyPress += new KeyPressEventHandler(this.OnKeyPressNumberTextBox);
                this._itemsPerPageTextBox.TabIndex = 0x10;
            }
            else
            {
                label12.SetBounds(8, 140, 0xa1, 0x10);
                label12.Text = MobileResource.GetString("ListGeneralPage_SelectTypeCaption");
                label12.TabStop = false;
                label12.TabIndex = 10;
                this._selectTypeCombo.SetBounds(8, 0x9c, 0xa1, 0x15);
                this._selectTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
                this._selectTypeCombo.SelectedIndexChanged += new EventHandler(this.OnSetPageDirty);
                this._selectTypeCombo.Items.AddRange(new object[] { MobileResource.GetString("ListGeneralPage_SelectTypeDropDown"), MobileResource.GetString("ListGeneralPage_SelectTypeListBox"), MobileResource.GetString("ListGeneralPage_SelectTypeRadio"), MobileResource.GetString("ListGeneralPage_SelectTypeMultiSelectListBox"), MobileResource.GetString("ListGeneralPage_SelectTypeCheckBox") });
                this._selectTypeCombo.TabIndex = 11;
                label10.SetBounds(0xd3, 140, 0xa1, 0x10);
                label10.Text = MobileResource.GetString("ListGeneralPage_RowsCaption");
                label10.TabStop = false;
                label10.TabIndex = 12;
                this._rowsTextBox.SetBounds(0xd3, 0x9c, 0xa1, 20);
                this._rowsTextBox.TextChanged += new EventHandler(this.OnSetPageDirty);
                this._rowsTextBox.KeyPress += new KeyPressEventHandler(this.OnKeyPressNumberTextBox);
                this._rowsTextBox.TabIndex = 13;
            }
            this.Text = MobileResource.GetString("ListGeneralPage_Title");
            base.Size = new Size(0x17e, 270);
            base.CommitOnDeactivate = true;
            base.Icon = new Icon(Type.GetType("System.Web.UI.Design.MobileControls.MobileControlDesigner," + Constants.MobileAssemblyFullName), "General.ico");
            base.Controls.AddRange(new Control[] { this._dataTextFieldCombo, label4, this._dataValueFieldCombo, label5, this._dataMemberCombo, label3, this._dataSourceCombo, label2, label, label6 });
            if (this._isBaseControlList)
            {
                base.Controls.AddRange(new Control[] { this._itemsPerPageTextBox, label9, this._itemCountTextBox, label8, label7, label11, this._decorationCombo });
            }
            else
            {
                base.Controls.AddRange(new Control[] { this._rowsTextBox, label10, label12, this._selectTypeCombo });
            }
        }

        private void InitPage()
        {
            this._dataSourceCombo.SelectedIndex = -1;
            this._dataSourceCombo.Items.Clear();
            this._currentDataSource = null;
            this._dataMemberCombo.SelectedIndex = -1;
            this._dataMemberCombo.Items.Clear();
            this._dataTextFieldCombo.SelectedIndex = -1;
            this._dataTextFieldCombo.Items.Clear();
            this._dataValueFieldCombo.SelectedIndex = -1;
            this._dataValueFieldCombo.Items.Clear();
            this._dataSourceDirty = false;
        }

        protected override void LoadComponent()
        {
            this.InitPage();
            this.LoadDataSourceItems();
            IListDesigner baseDesigner = (IListDesigner) base.GetBaseDesigner();
            if (this._dataSourceCombo.Items.Count > 0)
            {
                string dataSource = baseDesigner.DataSource;
                if (dataSource != null)
                {
                    int count = this._dataSourceCombo.Items.Count;
                    for (int i = 1; i < count; i++)
                    {
                        DataSourceItem item = (DataSourceItem) this._dataSourceCombo.Items[i];
                        if (string.Compare(item.Name, dataSource, true) == 0)
                        {
                            this._dataSourceCombo.SelectedIndex = i;
                            this._currentDataSource = item;
                            this.LoadDataMembers();
                            if (this._currentDataSource is ListSourceDataSourceItem)
                            {
                                string dataMember = baseDesigner.DataMember;
                                this._dataMemberCombo.SelectedIndex = this._dataMemberCombo.FindStringExact(dataMember);
                                if (this._dataMemberCombo.IsSet())
                                {
                                    ((ListSourceDataSourceItem) this._currentDataSource).CurrentDataMember = dataMember;
                                }
                            }
                            this.LoadDataSourceFields();
                            break;
                        }
                    }
                }
            }
            string dataTextField = baseDesigner.DataTextField;
            string dataValueField = baseDesigner.DataValueField;
            if (dataTextField.Length != 0)
            {
                int num3 = this._dataTextFieldCombo.FindStringExact(dataTextField);
                this._dataTextFieldCombo.SelectedIndex = num3;
            }
            if (dataValueField.Length != 0)
            {
                int num4 = this._dataValueFieldCombo.FindStringExact(dataValueField);
                this._dataValueFieldCombo.SelectedIndex = num4;
            }
            if (this._isBaseControlList)
            {
                List baseControl = base.GetBaseControl();
                this._itemCountTextBox.Text = baseControl.get_ItemCount().ToString();
                this._itemsPerPageTextBox.Text = baseControl.get_ItemsPerPage().ToString();
                switch (baseControl.get_Decoration())
                {
                    case 0:
                        this._decorationCombo.SelectedIndex = 0;
                        goto Label_0282;

                    case 1:
                        this._decorationCombo.SelectedIndex = 1;
                        goto Label_0282;

                    case 2:
                        this._decorationCombo.SelectedIndex = 2;
                        goto Label_0282;
                }
            }
            else
            {
                SelectionList list2 = base.GetBaseControl();
                switch (list2.get_SelectType())
                {
                    case 0:
                        this._selectTypeCombo.SelectedIndex = 0;
                        break;

                    case 1:
                        this._selectTypeCombo.SelectedIndex = 1;
                        break;

                    case 2:
                        this._selectTypeCombo.SelectedIndex = 2;
                        break;

                    case 3:
                        this._selectTypeCombo.SelectedIndex = 3;
                        break;

                    case 4:
                        this._selectTypeCombo.SelectedIndex = 4;
                        break;
                }
                this._rowsTextBox.Text = list2.get_Rows().ToString();
            }
        Label_0282:
            this.UpdateEnabledVisibleState();
        }

        private void LoadDataMembers()
        {
            using (new MobileComponentEditorPage.LoadingModeResource(this))
            {
                this._dataMemberCombo.SelectedIndex = -1;
                this._dataMemberCombo.Items.Clear();
                this._dataMemberCombo.EnsureNotSetItem();
                if ((this._currentDataSource != null) && (this._currentDataSource is ListSourceDataSourceItem))
                {
                    string[] dataMembers = ((ListSourceDataSourceItem) this._currentDataSource).DataMembers;
                    for (int i = 0; i < dataMembers.Length; i++)
                    {
                        this._dataMemberCombo.AddItem(dataMembers[i]);
                    }
                }
            }
        }

        private void LoadDataSourceFields()
        {
            using (new MobileComponentEditorPage.LoadingModeResource(this))
            {
                this._dataTextFieldCombo.SelectedIndex = -1;
                this._dataTextFieldCombo.Items.Clear();
                this._dataTextFieldCombo.EnsureNotSetItem();
                this._dataValueFieldCombo.SelectedIndex = -1;
                this._dataValueFieldCombo.Items.Clear();
                this._dataValueFieldCombo.EnsureNotSetItem();
                if (this._currentDataSource != null)
                {
                    PropertyDescriptorCollection fields = this._currentDataSource.Fields;
                    if (fields != null)
                    {
                        IEnumerator enumerator = fields.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            PropertyDescriptor current = (PropertyDescriptor) enumerator.Current;
                            if (BaseDataList.IsBindableType(current.PropertyType))
                            {
                                this._dataTextFieldCombo.AddItem(current.Name);
                                this._dataValueFieldCombo.AddItem(current.Name);
                            }
                        }
                    }
                }
            }
        }

        private void LoadDataSourceItems()
        {
            this._dataSourceCombo.EnsureNotSetItem();
            ISite site = base.GetSelectedComponent().Site;
            if (site != null)
            {
                IContainer service = (IContainer) site.GetService(typeof(IContainer));
                if (service != null)
                {
                    ComponentCollection components = service.Components;
                    if (components != null)
                    {
                        foreach (IComponent component in components)
                        {
                            if (!(component is IEnumerable) && !(component is IListSource))
                            {
                                continue;
                            }
                            ISite site2 = component.Site;
                            if (((site2 != null) && (site2.Name != null)) && (site2.Name.Length != 0))
                            {
                                DataSourceItem item;
                                if (component is IListSource)
                                {
                                    IListSource runtimeListSource = (IListSource) component;
                                    item = new ListSourceDataSourceItem(site2.Name, runtimeListSource);
                                }
                                else
                                {
                                    IEnumerable runtimeDataSource = (IEnumerable) component;
                                    item = new DataSourceItem(site2.Name, runtimeDataSource);
                                }
                                this._dataSourceCombo.AddItem(item);
                            }
                        }
                    }
                }
            }
        }

        private void OnKeyPressNumberTextBox(object source, KeyPressEventArgs e)
        {
            if (((e.KeyChar < '0') || (e.KeyChar > '9')) && (e.KeyChar != '\b'))
            {
                e.Handled = true;
            }
        }

        private void OnSelChangedDataMember(object source, EventArgs e)
        {
            if (!base.IsLoading())
            {
                string selectedItem = null;
                if (this._dataMemberCombo.IsSet())
                {
                    selectedItem = (string) this._dataMemberCombo.SelectedItem;
                }
                ((ListSourceDataSourceItem) this._currentDataSource).CurrentDataMember = selectedItem;
                this.LoadDataSourceFields();
                this._dataSourceDirty = true;
                this.SetDirty();
                this.UpdateEnabledVisibleState();
            }
        }

        private void OnSelChangedDataSource(object source, EventArgs e)
        {
            if (!base.IsLoading())
            {
                DataSourceItem selectedItem = null;
                if (this._dataSourceCombo.IsSet())
                {
                    selectedItem = (DataSourceItem) this._dataSourceCombo.SelectedItem;
                }
                if ((selectedItem != null) && !selectedItem.IsSelectable())
                {
                    using (new MobileComponentEditorPage.LoadingModeResource(this))
                    {
                        if (this._currentDataSource == null)
                        {
                            this._dataSourceCombo.SelectedIndex = -1;
                        }
                        else
                        {
                            this._dataSourceCombo.SelectedItem = this._currentDataSource;
                        }
                    }
                }
                else
                {
                    this._currentDataSource = selectedItem;
                    if (this._currentDataSource is ListSourceDataSourceItem)
                    {
                        ((ListSourceDataSourceItem) this._currentDataSource).CurrentDataMember = null;
                    }
                    this.LoadDataMembers();
                    this.LoadDataSourceFields();
                    this._dataSourceDirty = true;
                    this.SetDirty();
                    this.UpdateEnabledVisibleState();
                }
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
            SelectionList baseControl;
            string selectedItem = string.Empty;
            string str2 = string.Empty;
            IListDesigner baseDesigner = (IListDesigner) base.GetBaseDesigner();
            if (this._dataTextFieldCombo.IsSet())
            {
                selectedItem = (string) this._dataTextFieldCombo.SelectedItem;
            }
            if (this._dataValueFieldCombo.IsSet())
            {
                str2 = (string) this._dataValueFieldCombo.SelectedItem;
            }
            baseDesigner.DataTextField = selectedItem;
            baseDesigner.DataValueField = str2;
            if (this._dataSourceDirty)
            {
                DataBindingCollection dataBindings = ((HtmlControlDesigner) baseDesigner).DataBindings;
                if (this._currentDataSource == null)
                {
                    baseDesigner.DataSource = string.Empty;
                    baseDesigner.DataMember = string.Empty;
                }
                else
                {
                    baseDesigner.DataSource = this._currentDataSource.ToString();
                    if (this._dataMemberCombo.IsSet())
                    {
                        baseDesigner.DataMember = (string) this._dataMemberCombo.SelectedItem;
                    }
                    else
                    {
                        baseDesigner.DataMember = string.Empty;
                    }
                }
                baseDesigner.OnDataSourceChanged();
            }
            if (!this._isBaseControlList)
            {
                baseControl = base.GetBaseControl();
                switch (this._selectTypeCombo.SelectedIndex)
                {
                    case 0:
                        baseControl.set_SelectType(0);
                        goto Label_0243;

                    case 1:
                        baseControl.set_SelectType(1);
                        goto Label_0243;

                    case 2:
                        baseControl.set_SelectType(2);
                        goto Label_0243;

                    case 3:
                        baseControl.set_SelectType(3);
                        goto Label_0243;

                    case 4:
                        baseControl.set_SelectType(4);
                        goto Label_0243;
                }
            }
            else
            {
                List component = base.GetBaseControl();
                switch (this._decorationCombo.SelectedIndex)
                {
                    case 0:
                        component.set_Decoration(0);
                        break;

                    case 1:
                        component.set_Decoration(1);
                        break;

                    case 2:
                        component.set_Decoration(2);
                        break;
                }
                try
                {
                    int num = 0;
                    if (this._itemCountTextBox.Text.Length != 0)
                    {
                        num = int.Parse(this._itemCountTextBox.Text, CultureInfo.InvariantCulture);
                    }
                    component.set_ItemCount(num);
                }
                catch (Exception)
                {
                    this._itemCountTextBox.Text = component.get_ItemCount().ToString();
                }
                try
                {
                    int num2 = 0;
                    if (this._itemsPerPageTextBox.Text.Length != 0)
                    {
                        num2 = int.Parse(this._itemsPerPageTextBox.Text, CultureInfo.InvariantCulture);
                    }
                    component.set_ItemsPerPage(num2);
                }
                catch (Exception)
                {
                    this._itemsPerPageTextBox.Text = component.get_ItemsPerPage().ToString();
                }
                TypeDescriptor.Refresh(component);
                return;
            }
        Label_0243:
            try
            {
                int num3 = 4;
                if (this._rowsTextBox.Text.Length != 0)
                {
                    num3 = int.Parse(this._rowsTextBox.Text, CultureInfo.InvariantCulture);
                }
                baseControl.set_Rows(num3);
            }
            catch (Exception)
            {
                this._rowsTextBox.Text = baseControl.get_Rows().ToString();
            }
            TypeDescriptor.Refresh(baseControl);
        }

        public override void SetComponent(IComponent component)
        {
            base.SetComponent(component);
            this.InitForm();
        }

        private void UpdateEnabledVisibleState()
        {
            bool flag = this._currentDataSource != null;
            this._dataMemberCombo.Enabled = flag && (this._currentDataSource is ListSourceDataSourceItem);
            this._dataTextFieldCombo.Enabled = flag;
            this._dataValueFieldCombo.Enabled = flag;
        }

        protected override string HelpKeyword
        {
            get
            {
                if (this._isBaseControlList)
                {
                    return "net.Mobile.ListProperties.General";
                }
                return "net.Mobile.SelectionListProperties.General";
            }
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        private class DataSourceItem
        {
            private PropertyDescriptorCollection dataFields;
            private string dataSourceName;
            private IEnumerable runtimeDataSource;

            internal DataSourceItem(string dataSourceName, IEnumerable runtimeDataSource)
            {
                this.runtimeDataSource = runtimeDataSource;
                this.dataSourceName = dataSourceName;
            }

            protected void ClearFields()
            {
                this.dataFields = null;
            }

            internal bool IsSelectable()
            {
                object runtimeComponent = this.RuntimeComponent;
                MemberAttributes attributes = (MemberAttributes) 0;
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(runtimeComponent)["Modifiers"];
                if (descriptor != null)
                {
                    attributes = (MemberAttributes) descriptor.GetValue(runtimeComponent);
                }
                if (attributes == MemberAttributes.Private)
                {
                    string text = string.Format(MobileResource.GetString("ListGeneralPage_PrivateMemberMessage"), this.dataSourceName);
                    string caption = MobileResource.GetString("ListGeneralPage_PrivateMemberCaption");
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                return true;
            }

            public override string ToString()
            {
                return this.Name;
            }

            internal PropertyDescriptorCollection Fields
            {
                get
                {
                    if (this.dataFields == null)
                    {
                        IEnumerable runtimeDataSource = this.RuntimeDataSource;
                        if (runtimeDataSource != null)
                        {
                            this.dataFields = DesignTimeData.GetDataFields(runtimeDataSource);
                        }
                    }
                    if (this.dataFields == null)
                    {
                        this.dataFields = new PropertyDescriptorCollection(null);
                    }
                    return this.dataFields;
                }
            }

            internal string Name
            {
                get
                {
                    return this.dataSourceName;
                }
            }

            protected virtual object RuntimeComponent
            {
                get
                {
                    return this.runtimeDataSource;
                }
            }

            protected virtual IEnumerable RuntimeDataSource
            {
                get
                {
                    return this.runtimeDataSource;
                }
            }
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        private class ListSourceDataSourceItem : ListGeneralPage.DataSourceItem
        {
            private string _currentDataMember;
            private string[] _dataMembers;
            private IListSource _runtimeListSource;

            internal ListSourceDataSourceItem(string dataSourceName, IListSource runtimeListSource) : base(dataSourceName, null)
            {
                this._runtimeListSource = runtimeListSource;
            }

            internal string CurrentDataMember
            {
                get
                {
                    return this._currentDataMember;
                }
                set
                {
                    this._currentDataMember = value;
                    base.ClearFields();
                }
            }

            internal string[] DataMembers
            {
                get
                {
                    if (this._dataMembers == null)
                    {
                        this._dataMembers = DesignTimeData.GetDataMembers(this._runtimeListSource);
                    }
                    return this._dataMembers;
                }
            }

            protected override object RuntimeComponent
            {
                get
                {
                    return this._runtimeListSource;
                }
            }

            protected override IEnumerable RuntimeDataSource
            {
                get
                {
                    return DesignTimeData.GetDataMember(this._runtimeListSource, this._currentDataMember);
                }
            }
        }
    }
}

