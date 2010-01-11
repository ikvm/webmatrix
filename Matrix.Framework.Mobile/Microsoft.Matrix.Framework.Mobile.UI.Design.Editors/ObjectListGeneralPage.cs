namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using System;
    using System.CodeDom;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Security.Permissions;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.Design.MobileControls;
    using System.Web.UI.MobileControls;
    using System.Windows.Forms;

    internal sealed class ObjectListGeneralPage : MobileComponentEditorPage
    {
        private UnsettableComboBox _cmbDataMember;
        private UnsettableComboBox _cmbDataSource;
        private UnsettableComboBox _cmbLabelField;
        private DataSourceItem _currentDataSource;
        private bool _dataSourceDirty;
        private TextBox _txtBackCommandText;
        private TextBox _txtDetailsCommandText;
        private TextBox _txtItemCount;
        private TextBox _txtItemsPerPage;
        private TextBox _txtMoreText;
        private InterchangeableLists _xLists;

        private void InitForm()
        {
            this._cmbDataSource = new UnsettableComboBox();
            this._cmbLabelField = new UnsettableComboBox();
            this._cmbDataMember = new UnsettableComboBox();
            this._xLists = new InterchangeableLists();
            GroupLabel label = new GroupLabel();
            label.SetBounds(4, 4, 0x188, 0x10);
            label.Text = MobileResource.GetString("ObjectListGeneralPage_DataGroupLabel");
            label.TabIndex = 0;
            label.TabStop = false;
            Label label2 = new Label();
            label2.SetBounds(12, 0x18, 0xae, 0x10);
            label2.Text = MobileResource.GetString("ObjectListGeneralPage_DataSourceCaption");
            label2.TabStop = false;
            label2.TabIndex = 1;
            this._cmbDataSource.SetBounds(12, 40, 0x9a, 0x40);
            this._cmbDataSource.DropDownStyle = ComboBoxStyle.DropDownList;
            this._cmbDataSource.Sorted = true;
            this._cmbDataSource.TabIndex = 2;
            this._cmbDataSource.NotSetText = MobileResource.GetString("ObjectListGeneralPage_UnboundComboEntry");
            this._cmbDataSource.SelectedIndexChanged += new EventHandler(this.OnSelChangedDataSource);
            Label label3 = new Label();
            label3.SetBounds(0xce, 0x18, 0xae, 0x10);
            label3.Text = MobileResource.GetString("ObjectListGeneralPage_DataMemberCaption");
            label3.TabStop = false;
            label3.TabIndex = 3;
            this._cmbDataMember.SetBounds(0xce, 40, 0x9a, 0x40);
            this._cmbDataMember.DropDownStyle = ComboBoxStyle.DropDownList;
            this._cmbDataMember.Sorted = true;
            this._cmbDataMember.TabIndex = 4;
            this._cmbDataMember.NotSetText = MobileResource.GetString("ObjectListGeneralPage_NoneComboEntry");
            this._cmbDataMember.SelectedIndexChanged += new EventHandler(this.OnSelChangedDataMember);
            Label label4 = new Label();
            label4.SetBounds(12, 0x43, 0xae, 0x10);
            label4.Text = MobileResource.GetString("ObjectListGeneralPage_LabelFieldCaption");
            label4.TabStop = false;
            label4.TabIndex = 5;
            this._cmbLabelField.SetBounds(12, 0x53, 0x9a, 0x40);
            this._cmbLabelField.DropDownStyle = ComboBoxStyle.DropDownList;
            this._cmbLabelField.Sorted = true;
            this._cmbLabelField.TabIndex = 6;
            this._cmbLabelField.NotSetText = MobileResource.GetString("ObjectListGeneralPage_NoneComboEntry");
            this._cmbLabelField.SelectedIndexChanged += new EventHandler(this.OnSetPageDirty);
            this._cmbLabelField.TextChanged += new EventHandler(this.OnSetPageDirty);
            GroupLabel label5 = new GroupLabel();
            label5.SetBounds(4, 0x76, 0x188, 0x10);
            label5.Text = MobileResource.GetString("ObjectListGeneralPage_TableFieldsGroupLabel");
            label5.TabIndex = 9;
            label5.TabStop = false;
            this._xLists.Location = new Point(4, 130);
            this._xLists.TabIndex = 10;
            this._xLists.OnComponentChanged = (EventHandler) Delegate.Combine(this._xLists.OnComponentChanged, new EventHandler(this.OnSetPageDirty));
            this._xLists.TabStop = true;
            this._xLists.SetTitles(MobileResource.GetString("ObjectListGeneralPage_TableFieldsAvailableListLabel"), MobileResource.GetString("ObjectListGeneralPage_TableFieldsSelectedListLabel"));
            GroupLabel label6 = new GroupLabel();
            label6.SetBounds(4, 0x101, 0x188, 0x10);
            label6.Text = MobileResource.GetString("ObjectListGeneralPage_AppearanceGroupLabel");
            label6.TabIndex = 11;
            label6.TabStop = false;
            Label label7 = new Label();
            label7.SetBounds(12, 0x115, 0xae, 0x10);
            label7.Text = MobileResource.GetString("ObjectListGeneralPage_BackCommandTextCaption");
            label7.TabStop = false;
            label7.TabIndex = 12;
            this._txtBackCommandText = new TextBox();
            this._txtBackCommandText.SetBounds(12, 0x125, 0x9a, 20);
            this._txtBackCommandText.TabIndex = 13;
            this._txtBackCommandText.TextChanged += new EventHandler(this.OnSetPageDirty);
            Label label8 = new Label();
            label8.SetBounds(0xce, 0x115, 0xae, 0x10);
            label8.Text = MobileResource.GetString("ObjectListGeneralPage_DetailsCommandTextCaption");
            label8.TabStop = false;
            label8.TabIndex = 14;
            this._txtDetailsCommandText = new TextBox();
            this._txtDetailsCommandText.SetBounds(0xce, 0x125, 0x9a, 20);
            this._txtDetailsCommandText.TabIndex = 15;
            this._txtDetailsCommandText.TextChanged += new EventHandler(this.OnSetPageDirty);
            Label label9 = new Label();
            label9.SetBounds(12, 320, 0xae, 0x10);
            label9.Text = MobileResource.GetString("ObjectListGeneralPage_MoreTextCaption");
            label9.TabStop = false;
            label9.TabIndex = 0x10;
            this._txtMoreText = new TextBox();
            this._txtMoreText.SetBounds(12, 0x150, 0x9a, 20);
            this._txtMoreText.TabIndex = 0x11;
            this._txtMoreText.TextChanged += new EventHandler(this.OnSetPageDirty);
            GroupLabel label10 = new GroupLabel();
            Label label11 = new Label();
            this._txtItemCount = new TextBox();
            Label label12 = new Label();
            this._txtItemsPerPage = new TextBox();
            label10.SetBounds(4, 0x173, 0x188, 0x10);
            label10.Text = MobileResource.GetString("ListGeneralPage_PagingGroupLabel");
            label10.TabIndex = 0x12;
            label10.TabStop = false;
            label11.SetBounds(12, 0x187, 0xae, 0x10);
            label11.Text = MobileResource.GetString("ListGeneralPage_ItemCountCaption");
            label11.TabStop = false;
            label11.TabIndex = 0x13;
            this._txtItemCount.SetBounds(12, 0x197, 0x9a, 20);
            this._txtItemCount.TextChanged += new EventHandler(this.OnSetPageDirty);
            this._txtItemCount.KeyPress += new KeyPressEventHandler(this.OnKeyPressNumberTextBox);
            this._txtItemCount.TabIndex = 20;
            label12.SetBounds(0xce, 0x187, 0xae, 0x10);
            label12.Text = MobileResource.GetString("ListGeneralPage_ItemsPerPageCaption");
            label12.TabStop = false;
            label12.TabIndex = 0x15;
            this._txtItemsPerPage.SetBounds(0xce, 0x197, 0x9a, 20);
            this._txtItemsPerPage.TextChanged += new EventHandler(this.OnSetPageDirty);
            this._txtItemsPerPage.KeyPress += new KeyPressEventHandler(this.OnKeyPressNumberTextBox);
            this._txtItemsPerPage.TabIndex = 0x16;
            this.Text = MobileResource.GetString("ObjectListGeneralPage_Title");
            base.Size = new Size(0x192, 0x1b4);
            base.CommitOnDeactivate = true;
            base.Icon = new Icon(Type.GetType("System.Web.UI.Design.MobileControls.MobileControlDesigner," + Constants.MobileAssemblyFullName), "General.ico");
            base.Controls.AddRange(new Control[] { 
                label, label2, this._cmbDataSource, label3, this._cmbDataMember, label4, this._cmbLabelField, label5, this._xLists, label6, label7, this._txtBackCommandText, label8, this._txtDetailsCommandText, label9, this._txtMoreText, 
                label10, label11, this._txtItemCount, label12, this._txtItemsPerPage
             });
        }

        private void InitPage()
        {
            this._cmbDataSource.SelectedIndex = -1;
            this._cmbDataSource.Items.Clear();
            this._currentDataSource = null;
            this._cmbDataMember.SelectedIndex = -1;
            this._cmbDataMember.Items.Clear();
            this._cmbLabelField.SelectedIndex = -1;
            this._cmbLabelField.Items.Clear();
            this._xLists.Clear();
            this._dataSourceDirty = false;
        }

        private bool IsBindableType(Type type)
        {
            if ((!type.IsPrimitive && (type != typeof(string))) && (type != typeof(DateTime)))
            {
                return (type == typeof(decimal));
            }
            return true;
        }

        protected override void LoadComponent()
        {
            this.InitPage();
            ObjectList baseControl = base.GetBaseControl();
            this.LoadDataSourceItems();
            if (this._cmbDataSource.Items.Count > 0)
            {
                ObjectListDesigner baseDesigner = (ObjectListDesigner) base.GetBaseDesigner();
                string dataSource = baseDesigner.DataSource;
                if (dataSource != null)
                {
                    int count = this._cmbDataSource.Items.Count;
                    for (int i = 1; i < count; i++)
                    {
                        DataSourceItem item = (DataSourceItem) this._cmbDataSource.Items[i];
                        if (string.Compare(item.Name, dataSource, true) == 0)
                        {
                            this._cmbDataSource.SelectedIndex = i;
                            this._currentDataSource = item;
                            this.LoadDataMembers();
                            if (this._currentDataSource is ListSourceDataSourceItem)
                            {
                                string dataMember = baseDesigner.DataMember;
                                this._cmbDataMember.SelectedIndex = this._cmbDataMember.FindStringExact(dataMember);
                                if (this._cmbDataMember.IsSet())
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
            string s = baseControl.get_LabelField();
            if (s.Length != 0)
            {
                int num3 = this._cmbLabelField.FindStringExact(s);
                this._cmbLabelField.SelectedIndex = num3;
            }
            this._txtItemCount.Text = baseControl.get_ItemCount().ToString();
            this._txtItemsPerPage.Text = baseControl.get_ItemsPerPage().ToString();
            this._txtBackCommandText.Text = baseControl.get_BackCommandText();
            this._txtDetailsCommandText.Text = baseControl.get_DetailsCommandText();
            this._txtMoreText.Text = baseControl.get_MoreText();
            this.UpdateEnabledVisibleState();
        }

        private void LoadDataMembers()
        {
            using (new MobileComponentEditorPage.LoadingModeResource(this))
            {
                this._cmbDataMember.SelectedIndex = -1;
                this._cmbDataMember.Items.Clear();
                this._cmbDataMember.EnsureNotSetItem();
                if ((this._currentDataSource != null) && (this._currentDataSource is ListSourceDataSourceItem))
                {
                    string[] dataMembers = ((ListSourceDataSourceItem) this._currentDataSource).DataMembers;
                    for (int i = 0; i < dataMembers.Length; i++)
                    {
                        this._cmbDataMember.AddItem(dataMembers[i]);
                    }
                }
            }
        }

        private void LoadDataSourceFields()
        {
            using (new MobileComponentEditorPage.LoadingModeResource(this))
            {
                ObjectList baseControl = base.GetBaseControl();
                this._cmbLabelField.SelectedIndex = -1;
                this._cmbLabelField.Items.Clear();
                this._cmbLabelField.EnsureNotSetItem();
                this._xLists.Clear();
                if (this._currentDataSource != null)
                {
                    StringCollection strings = new StringCollection();
                    foreach (ObjectListField field in baseControl.get_Fields())
                    {
                        this._cmbLabelField.AddItem(field.get_Name());
                        strings.Add(field.get_Name());
                    }
                    if (baseControl.get_AutoGenerateFields())
                    {
                        PropertyDescriptorCollection fields = this._currentDataSource.Fields;
                        if (fields != null)
                        {
                            IEnumerator enumerator = fields.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                PropertyDescriptor current = (PropertyDescriptor) enumerator.Current;
                                if (this.IsBindableType(current.PropertyType))
                                {
                                    this._cmbLabelField.AddItem(current.Name);
                                    strings.Add(current.Name);
                                }
                            }
                        }
                    }
                    if ((baseControl.get_TableFields() != string.Empty) && !this._dataSourceDirty)
                    {
                        char[] separator = new char[] { ';' };
                        foreach (string str in baseControl.get_TableFields().Split(separator))
                        {
                            for (int i = 0; i < strings.Count; i++)
                            {
                                string strB = strings[i];
                                if (string.Compare(str, strB, true) == 0)
                                {
                                    this._xLists.AddToSelectedList(strB);
                                    strings.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                    }
                    StringEnumerator enumerator3 = strings.GetEnumerator();
                    while (enumerator3.MoveNext())
                    {
                        string str3 = enumerator3.Current;
                        this._xLists.AddToAvailableList(str3);
                    }
                    this._xLists.Initialize();
                }
            }
        }

        private void LoadDataSourceItems()
        {
            this._cmbDataSource.EnsureNotSetItem();
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
                                this._cmbDataSource.AddItem(item);
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
                if (this._cmbDataMember.IsSet())
                {
                    selectedItem = (string) this._cmbDataMember.SelectedItem;
                }
                ListSourceDataSourceItem item = (ListSourceDataSourceItem) this._currentDataSource;
                item.CurrentDataMember = selectedItem;
                this._dataSourceDirty = true;
                this.LoadDataSourceFields();
                this.SetDirty();
                this.UpdateEnabledVisibleState();
            }
        }

        private void OnSelChangedDataSource(object source, EventArgs e)
        {
            if (!base.IsLoading())
            {
                DataSourceItem selectedItem = null;
                if (this._cmbDataSource.IsSet())
                {
                    selectedItem = (DataSourceItem) this._cmbDataSource.SelectedItem;
                }
                if ((selectedItem != null) && !selectedItem.IsSelectable())
                {
                    using (new MobileComponentEditorPage.LoadingModeResource(this))
                    {
                        if (this._currentDataSource == null)
                        {
                            this._cmbDataSource.SelectedIndex = -1;
                        }
                        else
                        {
                            this._cmbDataSource.SelectedItem = this._currentDataSource;
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
                    this._dataSourceDirty = true;
                    this.LoadDataMembers();
                    this.LoadDataSourceFields();
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
            ObjectList baseControl = base.GetBaseControl();
            ObjectListDesigner baseDesigner = (ObjectListDesigner) base.GetBaseDesigner();
            string selectedItem = string.Empty;
            if (this._cmbLabelField.IsSet())
            {
                selectedItem = (string) this._cmbLabelField.SelectedItem;
            }
            baseControl.set_LabelField(selectedItem);
            string str2 = string.Empty;
            foreach (string str3 in this._xLists.GetSelectedItems())
            {
                str2 = str2 + str3 + ";";
            }
            baseControl.set_TableFields(str2);
            if (this._dataSourceDirty)
            {
                DataBindingCollection dataBindings = baseDesigner.DataBindings;
                if (this._currentDataSource == null)
                {
                    baseDesigner.DataSource = string.Empty;
                    baseDesigner.DataMember = string.Empty;
                }
                else
                {
                    baseDesigner.DataSource = this._currentDataSource.ToString();
                    if (this._cmbDataMember.IsSet())
                    {
                        baseDesigner.DataMember = (string) this._cmbDataMember.SelectedItem;
                    }
                    else
                    {
                        baseDesigner.DataMember = string.Empty;
                    }
                }
                baseDesigner.OnDataSourceChanged();
            }
            try
            {
                int num = 0;
                if (this._txtItemCount.Text.Length != 0)
                {
                    num = int.Parse(this._txtItemCount.Text, CultureInfo.InvariantCulture);
                }
                baseControl.set_ItemCount(num);
            }
            catch (Exception)
            {
                this._txtItemCount.Text = baseControl.get_ItemCount().ToString();
            }
            try
            {
                int num2 = 0;
                if (this._txtItemsPerPage.Text.Length != 0)
                {
                    num2 = int.Parse(this._txtItemsPerPage.Text, CultureInfo.InvariantCulture);
                }
                baseControl.set_ItemsPerPage(num2);
            }
            catch (Exception)
            {
                this._txtItemsPerPage.Text = baseControl.get_ItemsPerPage().ToString();
            }
            baseControl.set_BackCommandText(this._txtBackCommandText.Text);
            baseControl.set_DetailsCommandText(this._txtDetailsCommandText.Text);
            baseControl.set_MoreText(this._txtMoreText.Text);
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
            this._cmbDataMember.Enabled = flag && (this._currentDataSource is ListSourceDataSourceItem);
            this._cmbLabelField.Enabled = this._xLists.Enabled = flag;
        }

        protected override string HelpKeyword
        {
            get
            {
                return "net.Mobile.ObjectListProperties.General";
            }
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        private class DataSourceItem
        {
            private PropertyDescriptorCollection _dataFields;
            private string _dataSourceName;
            private IEnumerable _runtimeDataSource;

            internal DataSourceItem(string dataSourceName, IEnumerable runtimeDataSource)
            {
                this._runtimeDataSource = runtimeDataSource;
                this._dataSourceName = dataSourceName;
            }

            protected void ClearFields()
            {
                this._dataFields = null;
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
                    string text = MobileResource.GetString("ObjectListGeneralPage_PrivateDataSourceMessage, _dataSourceName");
                    string caption = MobileResource.GetString("ObjectListGeneralPage_PrivateDataSourceTitle");
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
                    if (this._dataFields == null)
                    {
                        IEnumerable runtimeDataSource = this.RuntimeDataSource;
                        if (runtimeDataSource != null)
                        {
                            this._dataFields = DesignTimeData.GetDataFields(runtimeDataSource);
                        }
                    }
                    if (this._dataFields == null)
                    {
                        this._dataFields = new PropertyDescriptorCollection(null);
                    }
                    return this._dataFields;
                }
            }

            internal virtual bool HasDataMembers
            {
                get
                {
                    return false;
                }
            }

            internal string Name
            {
                get
                {
                    return this._dataSourceName;
                }
            }

            protected virtual object RuntimeComponent
            {
                get
                {
                    return this._runtimeDataSource;
                }
            }

            protected virtual IEnumerable RuntimeDataSource
            {
                get
                {
                    return this._runtimeDataSource;
                }
            }
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        private class ListSourceDataSourceItem : ObjectListGeneralPage.DataSourceItem
        {
            private string _dataFields;
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
                    return this._dataFields;
                }
                set
                {
                    this._dataFields = value;
                    base.ClearFields();
                }
            }

            internal string[] DataMembers
            {
                get
                {
                    if (this._dataMembers == null)
                    {
                        if (this.HasDataMembers)
                        {
                            this._dataMembers = DesignTimeData.GetDataMembers(this._runtimeListSource);
                        }
                        else
                        {
                            this._dataMembers = new string[0];
                        }
                    }
                    return this._dataMembers;
                }
            }

            internal override bool HasDataMembers
            {
                get
                {
                    return this._runtimeListSource.ContainsListCollection;
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
                    if (this.HasDataMembers)
                    {
                        return DesignTimeData.GetDataMember(this._runtimeListSource, this._dataFields);
                    }
                    return this._runtimeListSource.GetList();
                }
            }
        }
    }
}

