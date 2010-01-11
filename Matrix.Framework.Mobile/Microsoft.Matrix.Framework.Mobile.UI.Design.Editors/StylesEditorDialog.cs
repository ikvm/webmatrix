namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using Microsoft.Matrix.Framework.Mobile.UI.Design;
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Security.Permissions;
    using System.Web.UI.Design.MobileControls;
    using System.Web.UI.MobileControls;
    using System.Web.UI.MobileControls.Adapters;
    using System.Windows.Forms;

    internal sealed class StylesEditorDialog : Form
    {
        private Button _btnAdd;
        private Button _btnCancel;
        private Button _btnDown;
        private Button _btnHelp;
        private Button _btnOK;
        private Button _btnRemove;
        private Button _btnUp;
        private ContextMenu _cntxtMenu;
        private MenuItem _cntxtMenuItem;
        private Type _currentNewStyleType;
        private Timer _delayTimer;
        private TreeNode _editCandidateNode = null;
        private bool _firstActivate = true;
        private ListView _lvAvailableStyles;
        private Style _previewStyle;
        private PropertyGrid _propertyBrowser;
        private Control _samplePreview;
        private StyleSheet _styleSheet;
        private StyleSheetDesigner _styleSheetDesigner;
        private StyleSheet _tempStyleSheet;
        private TreeView _tvDefinedStyles;
        private TextBox _txtType;

        internal event StyleDeletedEventHandler StyleDeleted;

        internal event StyleRenamedEventHandler StyleRenamed;

        internal StylesEditorDialog(StyleSheet stylesheet, StyleSheetDesigner styleSheetDesigner, string initialStyleName)
        {
            if (Utils.GetDuplicateStyles(stylesheet).Count > 0)
            {
                MessageBox.Show(MobileResource.GetString("StylesEditorDialog_DuplicateStyleNames"), MobileResource.GetString("StylesEditorDialog_Title"));
                throw new ArgumentException(MobileResource.GetString("StylesEditorDialog_DuplicateStyleException"));
            }
            this._tempStyleSheet = new StyleSheet();
            this._previewStyle = new Style();
            this._styleSheet = stylesheet;
            this._styleSheetDesigner = styleSheetDesigner;
            this._tempStyleSheet.Site = this._styleSheet.Site;
            this.InitializeComponent();
            this.InitAvailableStyles();
            this.LoadStyleItems();
            if (this._tvDefinedStyles.Nodes.Count > 0)
            {
                int num = 0;
                if (initialStyleName != null)
                {
                    num = this.StyleIndex(initialStyleName);
                }
                this.SelectedStyle = (StyleNode) this._tvDefinedStyles.Nodes[num];
                this._tvDefinedStyles.Enabled = true;
                this.UpdateTypeText();
                this.UpdatePropertyGrid();
            }
            this.UpdateButtonsEnabling();
            this.UpdateFieldsEnabling();
        }

        private void ApplyStyle()
        {
            Style runtimeStyle = this.SelectedStyle.RuntimeStyle;
            Color color = runtimeStyle.get_ForeColor();
            Color color2 = runtimeStyle.get_BackColor();
            BooleanOption option = runtimeStyle.get_Font().get_Bold();
            BooleanOption option2 = runtimeStyle.get_Font().get_Italic();
            FontSize size = runtimeStyle.get_Font().get_Size();
            string str = runtimeStyle.get_Font().get_Name();
            string strB = runtimeStyle.get_StyleReference();
            bool flag = true;
            while (((strB != null) && (strB.Length > 0)) && flag)
            {
                flag = false;
                foreach (StyleNode node2 in this._tvDefinedStyles.Nodes)
                {
                    Style style2 = node2.RuntimeStyle;
                    if (string.Compare(style2.get_Name(), strB, true) == 0)
                    {
                        if (color == Color.Empty)
                        {
                            color = style2.get_ForeColor();
                        }
                        if (color2 == Color.Empty)
                        {
                            color2 = style2.get_BackColor();
                        }
                        if (option == -1)
                        {
                            option = style2.get_Font().get_Bold();
                        }
                        if (option2 == -1)
                        {
                            option2 = style2.get_Font().get_Italic();
                        }
                        if (size == null)
                        {
                            size = style2.get_Font().get_Size();
                        }
                        if (str == string.Empty)
                        {
                            str = style2.get_Font().get_Name();
                        }
                        strB = style2.get_StyleReference();
                        flag = true;
                        break;
                    }
                }
                if (!flag && (StyleSheet.get_Default().get_Item(strB) != null))
                {
                    Style style3 = StyleSheet.get_Default().get_Item(strB);
                    if (color == Color.Empty)
                    {
                        color = style3.get_ForeColor();
                    }
                    if (color2 == Color.Empty)
                    {
                        color2 = style3.get_BackColor();
                    }
                    if (option == -1)
                    {
                        option = style3.get_Font().get_Bold();
                    }
                    if (option2 == -1)
                    {
                        option2 = style3.get_Font().get_Italic();
                    }
                    if (size == null)
                    {
                        size = style3.get_Font().get_Size();
                    }
                    if (str == string.Empty)
                    {
                        str = style3.get_Font().get_Name();
                    }
                    strB = null;
                    flag = true;
                    break;
                }
            }
            this._previewStyle.set_ForeColor(color);
            this._previewStyle.set_BackColor(color2);
            this._previewStyle.get_Font().set_Name(str);
            this._previewStyle.get_Font().set_Size(size);
            this._previewStyle.get_Font().set_Bold(option);
            this._previewStyle.get_Font().set_Italic(option2);
        }

        private string AutoIDStyle()
        {
            string name = this._currentNewStyleType.Name;
            int num = 1;
            while (this.StyleIndex(name + num.ToString()) >= 0)
            {
                num++;
            }
            return (name + num.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this._tvDefinedStyles != null))
            {
                foreach (StyleNode node in this._tvDefinedStyles.Nodes)
                {
                    node.Dispose();
                }
                this._tvDefinedStyles = null;
            }
            base.Dispose(disposing);
        }

        private void InitAvailableStyles()
        {
            int[] numArray = new int[] { 0x44, 0xca };
            int[] numArray2 = new int[2];
            StringCollection strings = new StringCollection();
            strings.AddRange(new string[] { "System.Web.UI.MobileControls.PagerStyle," + Constants.MobileAssemblyFullName, "System.Web.UI.MobileControls.Style," + Constants.MobileAssemblyFullName });
            StringEnumerator enumerator = strings.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Type type = Type.GetType(enumerator.Current, true);
                string[] items = new string[] { type.Name, type.Namespace };
                ListViewItem item = new ListViewItem(items);
                this._lvAvailableStyles.Items.Add(item);
            }
            foreach (string str2 in this._styleSheet.get_Styles())
            {
                Type type2 = this._styleSheet.get_Item(str2).GetType();
                if (!strings.Contains(type2.FullName + "," + type2.Assembly.FullName))
                {
                    string[] strArray2 = new string[] { type2.Name, type2.Namespace };
                    ListViewItem item2 = new ListViewItem(strArray2);
                    this._lvAvailableStyles.Items.Add(item2);
                    numArray2[0] = 0x44;
                    if (numArray2[0] > numArray[0])
                    {
                        numArray[0] = numArray2[0];
                    }
                    numArray2[1] = 0xca;
                    if (numArray2[1] > numArray[1])
                    {
                        numArray[1] = numArray2[1];
                    }
                }
            }
            this._lvAvailableStyles.Columns[0].Width = numArray[0] + 4;
            this._lvAvailableStyles.Columns[1].Width = numArray[1] + 4;
            this._lvAvailableStyles.Sort();
            this._lvAvailableStyles.Items[0].Selected = true;
            this._currentNewStyleType = Type.GetType(this._lvAvailableStyles.Items[0].SubItems[1].Text + "." + this._lvAvailableStyles.Items[0].Text + ", " + Constants.MobileAssemblyFullName, true);
        }

        private void InitializeComponent()
        {
            this._btnOK = new Button();
            this._btnCancel = new Button();
            this._btnHelp = new Button();
            this._btnUp = new Button();
            this._btnDown = new Button();
            this._btnAdd = new Button();
            this._btnRemove = new Button();
            this._txtType = new TextBox();
            this._tvDefinedStyles = new TreeView();
            this._lvAvailableStyles = new ListView();
            this._samplePreview = Utils.CreateMSHTMLHost();
            this._propertyBrowser = new PropertyGrid();
            this._cntxtMenuItem = new MenuItem();
            this._cntxtMenu = new ContextMenu();
            GroupLabel label = new GroupLabel();
            label.SetBounds(6, 5, 0x1b0, 0x10);
            label.Text = MobileResource.GetString("StylesEditorDialog_StyleListGroupLabel");
            label.TabStop = false;
            label.TabIndex = 0;
            Label label2 = new Label();
            label2.SetBounds(14, 0x19, 180, 0x10);
            label2.Text = MobileResource.GetString("StylesEditorDialog_AvailableStylesCaption");
            label2.TabStop = false;
            label2.TabIndex = 1;
            ColumnHeader header = new ColumnHeader();
            ColumnHeader header2 = new ColumnHeader();
            header.Width = 0x10;
            header.TextAlign = HorizontalAlignment.Left;
            header2.Width = 0x10;
            header2.TextAlign = HorizontalAlignment.Left;
            this._lvAvailableStyles.SetBounds(14, 0x29, 180, 0x5f);
            this._lvAvailableStyles.HeaderStyle = ColumnHeaderStyle.None;
            this._lvAvailableStyles.MultiSelect = false;
            this._lvAvailableStyles.HideSelection = false;
            this._lvAvailableStyles.FullRowSelect = true;
            this._lvAvailableStyles.View = View.Details;
            this._lvAvailableStyles.Columns.AddRange(new ColumnHeader[] { header, header2 });
            this._lvAvailableStyles.SelectedIndexChanged += new EventHandler(this.OnNewStyleTypeChanged);
            this._lvAvailableStyles.DoubleClick += new EventHandler(this.OnDoubleClick);
            this._lvAvailableStyles.Sorting = SortOrder.Ascending;
            this._lvAvailableStyles.TabIndex = 2;
            this._lvAvailableStyles.TabStop = true;
            this._btnAdd.SetBounds(0xc6, 0x4d, 0x20, 0x19);
            this._btnAdd.Text = MobileResource.GetString("StylesEditorDialog_AddBtnCation");
            this._btnAdd.Click += new EventHandler(this.OnClickAddButton);
            this._btnAdd.TabIndex = 3;
            this._btnAdd.TabStop = true;
            Label label3 = new Label();
            label3.SetBounds(0xea, 0x19, 0xa6, 0x10);
            label3.Text = MobileResource.GetString("StylesEditorDialog_DefinedStylesCaption");
            label3.TabStop = false;
            label3.TabIndex = 4;
            this._tvDefinedStyles.SetBounds(0xea, 0x29, 0xa6, 0x5f);
            this._tvDefinedStyles.AfterSelect += new TreeViewEventHandler(this.OnStylesSelected);
            this._tvDefinedStyles.AfterLabelEdit += new NodeLabelEditEventHandler(this.OnAfterLabelEdit);
            this._tvDefinedStyles.LabelEdit = true;
            this._tvDefinedStyles.ShowPlusMinus = false;
            this._tvDefinedStyles.HideSelection = false;
            this._tvDefinedStyles.Indent = 15;
            this._tvDefinedStyles.ShowRootLines = false;
            this._tvDefinedStyles.ShowLines = false;
            this._tvDefinedStyles.ContextMenu = this._cntxtMenu;
            this._tvDefinedStyles.TabIndex = 5;
            this._tvDefinedStyles.TabStop = true;
            this._tvDefinedStyles.KeyDown += new KeyEventHandler(this.OnKeyDown);
            this._tvDefinedStyles.MouseUp += new MouseEventHandler(this.OnListMouseUp);
            this._tvDefinedStyles.MouseDown += new MouseEventHandler(this.OnListMouseDown);
            this._btnUp.SetBounds(0x194, 0x29, 0x1c, 0x1b);
            this._btnUp.Click += new EventHandler(this.OnClickUpButton);
            this._btnUp.Image = GenericUI.SortUpIcon;
            this._btnUp.TabIndex = 6;
            this._btnUp.TabStop = true;
            this._btnDown.SetBounds(0x194, 0x48, 0x1c, 0x1b);
            this._btnDown.Click += new EventHandler(this.OnClickDownButton);
            this._btnDown.Image = GenericUI.SortDownIcon;
            this._btnDown.TabIndex = 7;
            this._btnDown.TabStop = true;
            this._btnRemove.SetBounds(0x194, 0x6d, 0x1c, 0x1b);
            this._btnRemove.Click += new EventHandler(this.OnClickRemoveButton);
            this._btnRemove.Image = GenericUI.DeleteIcon;
            this._btnRemove.TabIndex = 8;
            this._btnRemove.TabStop = true;
            GroupLabel label4 = new GroupLabel();
            label4.SetBounds(6, 0x91, 0x1b0, 0x10);
            label4.Text = MobileResource.GetString("StylesEditorDialog_StylePropertiesGroupLabel");
            label4.TabStop = false;
            label4.TabIndex = 9;
            Label label5 = new Label();
            label5.SetBounds(14, 0xa5, 180, 0x10);
            label5.Text = MobileResource.GetString("StylesEditorDialog_TypeCaption");
            label5.TabIndex = 10;
            label5.TabStop = false;
            this._txtType.SetBounds(14, 0xb5, 180, 0x10);
            this._txtType.ReadOnly = true;
            this._txtType.TabIndex = 11;
            this._txtType.TabStop = true;
            Label label6 = new Label();
            label6.SetBounds(14, 0xd5, 180, 0x10);
            label6.Text = MobileResource.GetString("StylesEditorDialog_SampleCaption");
            label6.TabStop = false;
            label6.TabIndex = 12;
            this._samplePreview.SetBounds(14, 0xe5, 180, 0x4c);
            this._samplePreview.TabStop = false;
            this._samplePreview.TabIndex = 13;
            Label label7 = new Label();
            label7.SetBounds(0xea, 0xa5, 0xc6, 0x10);
            label7.Text = MobileResource.GetString("StylesEditorDialog_PropertiesCaption");
            label7.TabIndex = 14;
            label7.TabStop = false;
            this._propertyBrowser.SetBounds(0xea, 0xb5, 0xc6, 0xb2);
            this._propertyBrowser.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._propertyBrowser.ToolbarVisible = false;
            this._propertyBrowser.HelpVisible = false;
            this._propertyBrowser.TabIndex = 15;
            this._propertyBrowser.TabStop = true;
            this._propertyBrowser.PropertySort = PropertySort.Alphabetical;
            this._propertyBrowser.PropertyValueChanged += new PropertyValueChangedEventHandler(this.OnPropertyValueChanged);
            this._btnOK.DialogResult = DialogResult.OK;
            this._btnOK.Location = new Point(0xc9, 370);
            this._btnOK.Size = new Size(0x4b, 0x17);
            this._btnOK.TabIndex = 0x10;
            this._btnOK.Text = MobileResource.GetString("GenericDialog_OKBtnCaption");
            this._btnOK.Click += new EventHandler(this.OnClickOKButton);
            this._btnCancel.DialogResult = DialogResult.Cancel;
            this._btnCancel.Location = new Point(0x11a, 370);
            this._btnCancel.Size = new Size(0x4b, 0x17);
            this._btnCancel.TabIndex = 0x11;
            this._btnCancel.Text = MobileResource.GetString("GenericDialog_CancelBtnCaption");
            this._btnHelp.Click += new EventHandler(this.OnClickHelpButton);
            this._btnHelp.Location = new Point(0x16b, 370);
            this._btnHelp.Size = new Size(0x4b, 0x17);
            this._btnHelp.TabIndex = 0x12;
            this._btnHelp.Text = MobileResource.GetString("GenericDialog_HelpBtnCaption");
            this._cntxtMenuItem.Text = MobileResource.GetString("EditableTreeList_Rename");
            this._cntxtMenu.MenuItems.Add(this._cntxtMenuItem);
            this._cntxtMenu.Popup += new EventHandler(this.OnPopup);
            this._cntxtMenuItem.Click += new EventHandler(this.OnContextMenuItemClick);
            this.Text = this._styleSheet.ID + " - " + MobileResource.GetString("StylesEditorDialog_Title");
            base.ClientSize = new Size(0x1bc, 0x191);
            base.AcceptButton = this._btnOK;
            base.CancelButton = this._btnCancel;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Activated += new EventHandler(this.StylesEditorDialog_Activated);
            base.HelpRequested += new HelpEventHandler(this.OnHelpRequested);
            base.Controls.AddRange(new Control[] { 
                label, label2, this._lvAvailableStyles, this._btnAdd, label3, this._tvDefinedStyles, this._btnUp, this._btnDown, this._btnRemove, label4, label5, this._txtType, label6, this._samplePreview, label7, this._propertyBrowser, 
                this._btnOK, this._btnCancel, this._btnHelp
             });
        }

        private void LoadStyleItems()
        {
            foreach (string str in this._styleSheet.get_Styles())
            {
                Style component = this._styleSheet.get_Item(str);
                Style style2 = (Style) Activator.CreateInstance(component.GetType());
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(component);
                for (int i = 0; i < properties.Count; i++)
                {
                    if (properties[i].Name.Equals("Font"))
                    {
                        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(component.get_Font()))
                        {
                            descriptor.SetValue(style2.get_Font(), descriptor.GetValue(component.get_Font()));
                        }
                    }
                    else if (!properties[i].IsReadOnly)
                    {
                        properties[i].SetValue(style2, properties[i].GetValue(component));
                    }
                }
                this._tempStyleSheet.set_Item(style2.get_Name(), style2);
                Utils.InvokeSetControl(style2, this._tempStyleSheet);
                StyleNode node = new StyleNode(style2);
                this._tvDefinedStyles.Nodes.Add(node);
            }
        }

        private void MoveSelectedNode(int direction)
        {
            StyleNode selectedStyle = this.SelectedStyle;
            int index = selectedStyle.Index;
            this._tvDefinedStyles.Nodes.RemoveAt(index);
            this._tvDefinedStyles.Nodes.Insert(index + direction, selectedStyle);
            this.SelectedStyle = selectedStyle;
        }

        private void OnActivateDefinedStyles(object sender, EventArgs e)
        {
            this._delayTimer.Stop();
            this._delayTimer.Tick -= new EventHandler(this.OnActivateDefinedStyles);
            this._lvAvailableStyles.Focus();
        }

        private void OnAfterLabelEdit(object source, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                string text = e.Node.Text;
                string label = e.Label;
                string caption = MobileResource.GetString("Style_ErrorMessageTitle");
                if ((string.Compare(text, label, true) != 0) && (this.StyleIndex(label) >= 0))
                {
                    MessageBox.Show(MobileResource.GetString("Style_DuplicateName"), caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.CancelEdit = true;
                }
                else if (label == string.Empty)
                {
                    MessageBox.Show(MobileResource.GetString("StylesEditorDialog_EmptyName"), caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.CancelEdit = true;
                }
                else
                {
                    this.SelectedStyle.RuntimeStyle.set_Name(label);
                    this._tempStyleSheet.Remove(text);
                    this._tempStyleSheet.set_Item(label, this.SelectedStyle.RuntimeStyle);
                    if (this.ReferencesContainCycle(this.SelectedStyle))
                    {
                        this.SelectedStyle.RuntimeStyle.set_Name(text);
                        this._tempStyleSheet.Remove(label);
                        this._tempStyleSheet.set_Item(text, this.SelectedStyle.RuntimeStyle);
                        MessageBox.Show(MobileResource.GetString("Style_NameChangeCauseCircularLoop"), caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        e.CancelEdit = true;
                    }
                    else
                    {
                        this.OnStyleRenamed(new StyleRenamedEventArgs(text, label));
                    }
                }
            }
        }

        private void OnClickAddButton(object sender, EventArgs e)
        {
            this.OnCreateNewStyle();
        }

        private void OnClickDownButton(object source, EventArgs e)
        {
            this.MoveSelectedNode(1);
            this.UpdateButtonsEnabling();
        }

        private void OnClickHelpButton(object sender, EventArgs e)
        {
            this.ShowHelpTopic();
        }

        private void OnClickOKButton(object sender, EventArgs e)
        {
            this.SaveComponent();
            base.Close();
            base.DialogResult = DialogResult.OK;
        }

        private void OnClickRemoveButton(object source, EventArgs e)
        {
            string text = MobileResource.GetString("StylesEditorDialog_DeleteStyleMessage");
            string caption = MobileResource.GetString("StylesEditorDialog_DeleteStyleCaption");
            if (MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No)
            {
                string name = this.SelectedStyle.RuntimeStyle.get_Name();
                this._tempStyleSheet.Remove(name);
                this.SelectedStyle.Dispose();
                int index = this.SelectedStyle.Index;
                int count = this._tvDefinedStyles.Nodes.Count;
                this._tvDefinedStyles.Nodes.RemoveAt(index);
                this.OnStyleDeleted(new StyleDeletedEventArgs(name));
                if (index < (count - 1))
                {
                    this.SelectedStyle = (StyleNode) this._tvDefinedStyles.Nodes[index];
                }
                else if (index >= 1)
                {
                    this.SelectedStyle = (StyleNode) this._tvDefinedStyles.Nodes[index - 1];
                }
                else if (count == 1)
                {
                    this.SelectedStyle = null;
                    this.UpdateTypeText();
                    this.UpdatePropertyGrid();
                    this.UpdateSamplePreview();
                    this.UpdateButtonsEnabling();
                    this.UpdateFieldsEnabling();
                }
            }
        }

        private void OnClickUpButton(object source, EventArgs e)
        {
            this.MoveSelectedNode(-1);
            this.UpdateButtonsEnabling();
        }

        private void OnContextMenuItemClick(object sender, EventArgs e)
        {
            if (this._editCandidateNode == null)
            {
                if (this._tvDefinedStyles.SelectedNode != null)
                {
                    this._tvDefinedStyles.SelectedNode.BeginEdit();
                }
            }
            else
            {
                this._editCandidateNode.BeginEdit();
            }
            this._editCandidateNode = null;
        }

        private void OnCreateNewStyle()
        {
            string str = this.AutoIDStyle();
            Style style = (Style) Activator.CreateInstance(this._currentNewStyleType);
            style.set_Name(str);
            this._tempStyleSheet.set_Item(style.get_Name(), style);
            Utils.InvokeSetControl(style, this._tempStyleSheet);
            StyleNode node = new StyleNode(style);
            this._tvDefinedStyles.Enabled = true;
            this._propertyBrowser.Enabled = true;
            this._tvDefinedStyles.Nodes.Add(node);
            this.SelectedStyle = node;
            this.UpdateSamplePreview();
            this.UpdateButtonsEnabling();
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            this.OnCreateNewStyle();
        }

        private void OnHelpRequested(object Control, HelpEventArgs hevent)
        {
            this.ShowHelpTopic();
            hevent.Handled = true;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case (Keys.Control | Keys.End):
                {
                    int count = this._tvDefinedStyles.Nodes.Count;
                    if (count <= 0)
                    {
                        break;
                    }
                    this.SelectedStyle = (StyleNode) this._tvDefinedStyles.Nodes[count - 1];
                    return;
                }
                case (Keys.Control | Keys.Home):
                    if (this._tvDefinedStyles.Nodes.Count <= 0)
                    {
                        break;
                    }
                    this.SelectedStyle = (StyleNode) this._tvDefinedStyles.Nodes[0];
                    return;

                case Keys.F2:
                    if (this.SelectedStyle != null)
                    {
                        this.SelectedStyle.BeginEdit();
                        return;
                    }
                    break;

                default:
                    return;
            }
        }

        private void OnListMouseDown(object sender, MouseEventArgs e)
        {
            this._editCandidateNode = null;
            if (e.Button == MouseButtons.Right)
            {
                this._editCandidateNode = this._tvDefinedStyles.GetNodeAt(e.X, e.Y);
            }
        }

        private void OnListMouseUp(object sender, MouseEventArgs e)
        {
            this._editCandidateNode = null;
            if (e.Button == MouseButtons.Right)
            {
                this._editCandidateNode = this._tvDefinedStyles.GetNodeAt(e.X, e.Y);
            }
        }

        private void OnNewStyleTypeChanged(object sender, EventArgs e)
        {
            if (this._lvAvailableStyles.SelectedItems.Count != 0)
            {
                this._currentNewStyleType = Type.GetType(this._lvAvailableStyles.SelectedItems[0].SubItems[1].Text + "." + this._lvAvailableStyles.SelectedItems[0].Text + ", " + Constants.MobileAssemblyFullName, true);
            }
        }

        private void OnPopup(object sender, EventArgs e)
        {
            this._cntxtMenuItem.Enabled = (this._editCandidateNode != null) || (this._tvDefinedStyles.SelectedNode != null);
        }

        private void OnPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (this.SelectedStyle != null)
            {
                this.UpdateSamplePreview();
            }
        }

        private void OnStyleDeleted(StyleDeletedEventArgs e)
        {
            if (this.StyleDeleted != null)
            {
                this.StyleDeleted(this, e);
            }
        }

        private void OnStyleRenamed(StyleRenamedEventArgs e)
        {
            if (this.StyleRenamed != null)
            {
                this.StyleRenamed(this, e);
            }
        }

        private void OnStylesSelected(object sender, TreeViewEventArgs e)
        {
            this.UpdateTypeText();
            this.UpdatePropertyGrid();
            this.UpdateSamplePreview();
            this.UpdateButtonsEnabling();
            this.UpdateFieldsEnabling();
        }

        private bool ReferencesContainCycle(StyleNode startingStyleItem)
        {
            StyleNode node = startingStyleItem;
            string strB = node.RuntimeStyle.get_StyleReference();
            bool flag = true;
            bool flag2 = false;
            foreach (StyleNode node2 in this._tvDefinedStyles.Nodes)
            {
                node2.Referenced = false;
            }
            node.Referenced = true;
        Label_00E7:
            while (((strB != null) && (strB.Length > 0)) && (flag && !flag2))
            {
                flag = false;
                foreach (StyleNode node3 in this._tvDefinedStyles.Nodes)
                {
                    Style runtimeStyle = node3.RuntimeStyle;
                    if (string.Compare(runtimeStyle.get_Name(), strB, true) == 0)
                    {
                        strB = runtimeStyle.get_StyleReference();
                        flag = true;
                        if (node3.Referenced)
                        {
                            flag2 = true;
                        }
                        else
                        {
                            node3.Referenced = true;
                        }
                        goto Label_00E7;
                    }
                }
            }
            return flag2;
        }

        private void SaveComponent()
        {
            this._styleSheet.Clear();
            foreach (StyleNode node in this._tvDefinedStyles.Nodes)
            {
                this._styleSheet.set_Item(node.RuntimeStyle.get_Name(), node.RuntimeStyle);
                Utils.InvokeSetControl(node.RuntimeStyle, this._styleSheet);
            }
            if ((this._styleSheetDesigner.CurrentStyle != null) && (this._styleSheet.get_Item(this._styleSheetDesigner.CurrentStyle.get_Name()) == null))
            {
                this._styleSheetDesigner.CurrentStyle = null;
            }
        }

        private void ShowHelpTopic()
        {
            IHelpService service = (IHelpService) this._styleSheet.Site.GetService(typeof(IHelpService));
            if (service != null)
            {
                service.ShowHelpFromKeyword("net.Mobile.StylesEditorDialog");
            }
        }

        private int StyleIndex(string name)
        {
            int num = 0;
            foreach (StyleNode node in this._tvDefinedStyles.Nodes)
            {
                if (string.Compare(name, node.RuntimeStyle.get_Name(), true) == 0)
                {
                    return num;
                }
                num++;
            }
            return -1;
        }

        private void StylesEditorDialog_Activated(object sender, EventArgs e)
        {
            if (this._firstActivate)
            {
                this._firstActivate = false;
                Utils.InvokeCreateTrident(this._samplePreview);
                Utils.InvokeActivateTrident(this._samplePreview);
                this.UpdateSamplePreview();
                this._delayTimer = new Timer();
                this._delayTimer.Interval = 100;
                this._delayTimer.Tick += new EventHandler(this.OnActivateDefinedStyles);
                this._delayTimer.Start();
            }
        }

        private void UpdateButtonsEnabling()
        {
            if (this.SelectedStyle == null)
            {
                this._btnUp.Enabled = false;
                this._btnDown.Enabled = false;
            }
            else
            {
                this._btnUp.Enabled = this.SelectedStyle.Index > 0;
                this._btnDown.Enabled = this.SelectedStyle.Index < (this._tvDefinedStyles.Nodes.Count - 1);
            }
            this._btnRemove.Enabled = this.SelectedStyle != null;
        }

        private void UpdateFieldsEnabling()
        {
            this._propertyBrowser.Enabled = this._tvDefinedStyles.Enabled = this.SelectedStyle != null;
        }

        private void UpdatePropertyGrid()
        {
            this._propertyBrowser.SelectedObject = (this.SelectedStyle == null) ? null : this.SelectedStyle.RuntimeStyle;
        }

        private void UpdateSamplePreview()
        {
            if (!this._firstActivate)
            {
                Interop.IHTMLDocument2 document = (Interop.IHTMLDocument2) Utils.InvokeGetDocument(this._samplePreview);
                Interop.IHTMLElement body = document.GetBody();
                ((Interop.IHtmlBodyElement) body).SetScroll("no");
                if (this.SelectedStyle == null)
                {
                    body.SetInnerHTML(string.Empty);
                    document.SetBgColor("buttonface");
                }
                else
                {
                    document.SetBgColor(string.Empty);
                    if (this.ReferencesContainCycle(this.SelectedStyle))
                    {
                        body.SetInnerHTML(string.Empty);
                    }
                    else
                    {
                        this.ApplyStyle();
                        HtmlMobileTextWriter designerTextWriter = Utils.GetDesignerTextWriter();
                        designerTextWriter.AddAttribute("title", this.SelectedStyle.RuntimeStyle.get_Name());
                        Color c = this._previewStyle.get_ForeColor();
                        if (!c.Equals(Color.Empty))
                        {
                            designerTextWriter.AddStyleAttribute("color", ColorTranslator.ToHtml(c));
                        }
                        c = this._previewStyle.get_BackColor();
                        if (!c.Equals(Color.Empty))
                        {
                            designerTextWriter.AddStyleAttribute("background-color", ColorTranslator.ToHtml(c));
                        }
                        string str = this._previewStyle.get_Font().get_Name();
                        if (!str.Equals(string.Empty))
                        {
                            designerTextWriter.AddStyleAttribute("font-family", str);
                        }
                        switch (this._previewStyle.get_Font().get_Size())
                        {
                            case 2:
                                designerTextWriter.AddStyleAttribute("font-size", "X-Small");
                                break;

                            case 3:
                                designerTextWriter.AddStyleAttribute("font-size", "Medium");
                                break;

                            default:
                                designerTextWriter.AddStyleAttribute("font-size", "Small");
                                break;
                        }
                        if (this._previewStyle.get_Font().get_Bold() == 1)
                        {
                            designerTextWriter.AddStyleAttribute("font-weight", "bold");
                        }
                        if (this._previewStyle.get_Font().get_Italic() == 1)
                        {
                            designerTextWriter.AddStyleAttribute("font-style", "italic");
                        }
                        designerTextWriter.RenderBeginTag("span");
                        designerTextWriter.Write(MobileResource.GetString("StylesEditorDialog_PreviewText"));
                        designerTextWriter.RenderEndTag();
                        string p = "<div align='center'><table width='100%' height='100%'><tr><td><p align='center'>" + designerTextWriter.ToString() + "</p></td></tr></table></div>";
                        body.SetInnerHTML(p);
                    }
                }
            }
        }

        private void UpdateTypeText()
        {
            if (this.SelectedStyle == null)
            {
                this._txtType.Text = string.Empty;
            }
            else
            {
                this._txtType.Text = this.SelectedStyle.FullName;
            }
        }

        private StyleNode SelectedStyle
        {
            get
            {
                return (this._tvDefinedStyles.SelectedNode as StyleNode);
            }
            set
            {
                this._tvDefinedStyles.SelectedNode = value;
            }
        }

        internal delegate void StyleDeletedEventHandler(object source, StyleDeletedEventArgs e);

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        private class StyleNode : TreeNode
        {
            private string _fullName;
            private bool _referenced;
            private Style _runtimeStyle;
            private string _styleReference;
            private EventHandler _styleReferenceChanged;

            internal StyleNode(Style style)
            {
                this._runtimeStyle = style;
                this._fullName = style.GetType().FullName;
                this._styleReference = this.RuntimeStyle.get_StyleReference();
                this._styleReferenceChanged = new EventHandler(this.OnStyleReferenceChanged);
                base.Text = this.RuntimeStyle.get_Name();
                TypeDescriptor.GetProperties(typeof(Style))["StyleReference"].AddValueChanged(this.RuntimeStyle, this._styleReferenceChanged);
            }

            private void CacheStyleReference()
            {
                this._styleReference = this.RuntimeStyle.get_StyleReference();
            }

            internal void Dispose()
            {
                TypeDescriptor.GetProperties(typeof(Style))["StyleReference"].RemoveValueChanged(this.RuntimeStyle, this._styleReferenceChanged);
            }

            private bool InCircularLoop()
            {
                StyleSheet sheet = this.RuntimeStyle.get_Control();
                string strB = this.RuntimeStyle.get_StyleReference();
                int num = sheet.get_Styles().Count + 1;
                while (((strB != null) && (strB.Length > 0)) && (num > 0))
                {
                    if (string.Compare(this.RuntimeStyle.get_Name(), strB, true) == 0)
                    {
                        return true;
                    }
                    Style style = sheet.get_Item(strB);
                    if (style != null)
                    {
                        strB = style.get_StyleReference();
                        num--;
                    }
                    else
                    {
                        strB = null;
                    }
                }
                return false;
            }

            private void OnStyleReferenceChanged(object sender, EventArgs e)
            {
                if (this.InCircularLoop())
                {
                    this.RestoreStyleReference();
                    throw new Exception(MobileResource.GetString("Style_ReferenceCauseCircularLoop"));
                }
                this.CacheStyleReference();
            }

            private void RestoreStyleReference()
            {
                this.RuntimeStyle.set_StyleReference(this._styleReference);
            }

            internal string FullName
            {
                get
                {
                    return this._fullName;
                }
            }

            internal bool Referenced
            {
                get
                {
                    return this._referenced;
                }
                set
                {
                    this._referenced = value;
                }
            }

            internal Style RuntimeStyle
            {
                get
                {
                    return this._runtimeStyle;
                }
            }
        }

        internal delegate void StyleRenamedEventHandler(object source, StyleRenamedEventArgs e);
    }
}

