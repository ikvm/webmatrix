namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    internal class CreateClauseDialog : MxForm
    {
        private Button _cancelButton;
        private Microsoft.Matrix.Plugins.CodeWizards.Data.ClauseNode _clauseNode;
        private Database _database;
        private string _defaultTableName;
        private RadioButton _filterRadioButton;
        private IDictionary _filters;
        private MxTextBox _filterValueTextBox;
        private ListBox _leftColumnListBox;
        private Label _leftColumnsLabel;
        private GroupBox _leftOperandGroupBox;
        private Panel _leftPanel;
        private MxComboBox _leftTableComboBox;
        private Label _leftTablesLabel;
        private Button _okButton;
        private MxComboBox _operatorComboBox;
        private Label _operatorLabel;
        private Label _rightColumnLabel;
        private ListBox _rightColumnListBox;
        private GroupBox _rightOperandGroupBox;
        private Panel _rightPanel;
        private MxComboBox _rightTableComboBox;
        private Label _rightTableLabel;
        private RadioButton radioButton1;

        public CreateClauseDialog(IServiceProvider provider, Database database, IDictionary filters, string defaultTableName) : base(provider)
        {
            this._filters = filters;
            this._defaultTableName = defaultTableName;
            this.InitializeComponent();
            this._database = database;
            foreach (Table table in (IEnumerable) this._database.Tables)
            {
                this._leftTableComboBox.Items.Add(new TableListItem(table));
                this._rightTableComboBox.Items.Add(new TableListItem(table));
            }
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void _filterRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (this._filterRadioButton.Checked)
            {
                this._rightTableComboBox.Enabled = false;
                this._rightColumnListBox.Enabled = false;
                this._filterValueTextBox.Enabled = true;
            }
            else
            {
                this._filterValueTextBox.Enabled = false;
                this._rightTableComboBox.Enabled = true;
                this._rightColumnListBox.Enabled = true;
            }
        }

        private void _leftColumnListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateRightColumnListBox();
            if (this._leftColumnListBox.SelectedItem != null)
            {
                this._filterValueTextBox.Text = '@' + this._leftColumnListBox.SelectedItem.ToString();
            }
            else
            {
                this._filterValueTextBox.Text = string.Empty;
            }
            ColumnListItem selectedItem = (ColumnListItem) this._leftColumnListBox.SelectedItem;
            switch (selectedItem.Column.DbType)
            {
                case DbType.String:
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.StringFixedLength:
                    if (!this._operatorComboBox.Items.Contains("like"))
                    {
                        this._operatorComboBox.Items.Add("like");
                        this._operatorComboBox.Items.Add("not like");
                        return;
                    }
                    break;

                default:
                    this._operatorComboBox.Items.Remove("like");
                    this._operatorComboBox.Items.Remove("not like");
                    break;
            }
        }

        private void _leftTableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._leftTableComboBox.SelectedItem != null)
            {
                this._leftColumnListBox.Items.Clear();
                TableListItem selectedItem = (TableListItem) this._leftTableComboBox.SelectedItem;
                foreach (Column column in selectedItem.Table.Columns)
                {
                    if (column.DbType != DbType.Binary)
                    {
                        this._leftColumnListBox.Items.Add(new ColumnListItem(column));
                    }
                }
                if (this._leftColumnListBox.Items.Count > 0)
                {
                    this._leftColumnListBox.SelectedIndex = 0;
                }
            }
        }

        private void _okButton_Click(object sender, EventArgs e)
        {
            if (this._filterRadioButton.Checked)
            {
                string key = this._filterValueTextBox.Text.Trim();
                if (((this._leftTableComboBox.Text.Length == 0) || (this._leftColumnListBox.Text.Length == 0)) || ((this._operatorComboBox.Text.Length == 0) || (key.Length == 0)))
                {
                    MessageBox.Show(this, "You must specify all fields!");
                    return;
                }
                DbType dbType = ((ColumnListItem) this._leftColumnListBox.SelectedItem).Column.DbType;
                if (key.StartsWith("@") && this._filters.Contains(key))
                {
                    FilterInfo info = (FilterInfo) this._filters[key];
                    if (info.Type != dbType)
                    {
                        MessageBox.Show(this, "The specified filter value already exists in the parameter collection, but does not have the same type as the existing one.\r\nPlease choose another name for your parameter.", "Query Builder");
                        return;
                    }
                    if (MessageBox.Show(this, "The specified filter value already exists in the parameter collection.  Are you sure you want to continue?", "Where Clause Builder", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                    {
                        return;
                    }
                }
                this._clauseNode = new FilterNode(this._leftTableComboBox.Text, this._leftColumnListBox.Text, this._operatorComboBox.Text, key, dbType);
            }
            else
            {
                if (((this._leftTableComboBox.Text.Length == 0) || (this._leftColumnListBox.Text.Length == 0)) || (((this._operatorComboBox.Text.Length == 0) || (this._rightTableComboBox.Text.Length == 0)) || (this._rightColumnListBox.Text.Length == 0)))
                {
                    MessageBox.Show(this, "You must specify all fields!", "Query Builder");
                    return;
                }
                this._clauseNode = new JoinNode(this._leftTableComboBox.Text, this._leftColumnListBox.Text, this._operatorComboBox.Text, this._rightTableComboBox.Text, this._rightColumnListBox.Text);
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void _rightTableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateRightColumnListBox();
        }

        private void InitializeComponent()
        {
            this._leftTableComboBox = new MxComboBox();
            this._leftColumnListBox = new ListBox();
            this._leftPanel = new Panel();
            this._operatorComboBox = new MxComboBox();
            this._filterRadioButton = new RadioButton();
            this.radioButton1 = new RadioButton();
            this._filterValueTextBox = new MxTextBox();
            this._rightTableComboBox = new MxComboBox();
            this._rightPanel = new Panel();
            this._rightColumnListBox = new ListBox();
            this._okButton = new Button();
            this._cancelButton = new Button();
            this._operatorLabel = new Label();
            this._leftOperandGroupBox = new GroupBox();
            this._leftTablesLabel = new Label();
            this._leftColumnsLabel = new Label();
            this._rightOperandGroupBox = new GroupBox();
            this._rightColumnLabel = new Label();
            this._rightTableLabel = new Label();
            this._leftPanel.SuspendLayout();
            this._rightPanel.SuspendLayout();
            this._leftOperandGroupBox.SuspendLayout();
            this._rightOperandGroupBox.SuspendLayout();
            base.SuspendLayout();
            this._leftTableComboBox.AlwaysShowFocusCues = true;
            this._leftTableComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this._leftTableComboBox.FlatAppearance = true;
            this._leftTableComboBox.InitialText = null;
            this._leftTableComboBox.Location = new Point(12, 40);
            this._leftTableComboBox.Name = "_leftTableComboBox";
            this._leftTableComboBox.Size = new Size(0xa4, 0x15);
            this._leftTableComboBox.TabIndex = 2;
            this._leftTableComboBox.SelectedIndexChanged += new EventHandler(this._leftTableComboBox_SelectedIndexChanged);
            this._leftColumnListBox.BorderStyle = BorderStyle.None;
            this._leftColumnListBox.Dock = DockStyle.Fill;
            this._leftColumnListBox.IntegralHeight = false;
            this._leftColumnListBox.Location = new Point(1, 1);
            this._leftColumnListBox.Name = "_leftColumnListBox";
            this._leftColumnListBox.Size = new Size(0xa2, 0xb2);
            this._leftColumnListBox.TabIndex = 1;
            this._leftColumnListBox.SelectedIndexChanged += new EventHandler(this._leftColumnListBox_SelectedIndexChanged);
            this._leftPanel.BackColor = SystemColors.ControlDark;
            this._leftPanel.Controls.AddRange(new Control[] { this._leftColumnListBox });
            this._leftPanel.DockPadding.All = 1;
            this._leftPanel.Location = new Point(12, 0x54);
            this._leftPanel.Name = "_leftPanel";
            this._leftPanel.Size = new Size(0xa4, 180);
            this._leftPanel.TabIndex = 4;
            this._operatorComboBox.AlwaysShowFocusCues = true;
            this._operatorComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this._operatorComboBox.FlatAppearance = true;
            this._operatorComboBox.InitialText = null;
            this._operatorComboBox.Items.AddRange(new object[] { "=", "<", ">", "<=", ">=", "<>", "is null", "is not null" });
            this._operatorComboBox.Location = new Point(0xd0, 0x80);
            this._operatorComboBox.Name = "_operatorComboBox";
            this._operatorComboBox.Size = new Size(0x5c, 0x15);
            this._operatorComboBox.TabIndex = 3;
            this._filterRadioButton.Checked = true;
            this._filterRadioButton.FlatStyle = FlatStyle.System;
            this._filterRadioButton.Location = new Point(12, 20);
            this._filterRadioButton.Name = "_filterRadioButton";
            this._filterRadioButton.Size = new Size(0x34, 0x10);
            this._filterRadioButton.TabIndex = 1;
            this._filterRadioButton.TabStop = true;
            this._filterRadioButton.Text = "&Filter";
            this._filterRadioButton.CheckedChanged += new EventHandler(this._filterRadioButton_CheckedChanged);
            this.radioButton1.FlatStyle = FlatStyle.System;
            this.radioButton1.Location = new Point(12, 0x40);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x34, 0x10);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.Text = "&Join";
            this._filterValueTextBox.AlwaysShowFocusCues = true;
            this._filterValueTextBox.FlatAppearance = true;
            this._filterValueTextBox.Location = new Point(0x1c, 40);
            this._filterValueTextBox.Name = "_filterValueTextBox";
            this._filterValueTextBox.Size = new Size(0xb0, 20);
            this._filterValueTextBox.TabIndex = 2;
            this._filterValueTextBox.Text = "";
            this._rightTableComboBox.AlwaysShowFocusCues = true;
            this._rightTableComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this._rightTableComboBox.Enabled = false;
            this._rightTableComboBox.FlatAppearance = true;
            this._rightTableComboBox.InitialText = null;
            this._rightTableComboBox.Location = new Point(0x1c, 0x68);
            this._rightTableComboBox.Name = "_rightTableComboBox";
            this._rightTableComboBox.Size = new Size(0xb0, 0x15);
            this._rightTableComboBox.TabIndex = 4;
            this._rightTableComboBox.SelectedIndexChanged += new EventHandler(this._rightTableComboBox_SelectedIndexChanged);
            this._rightPanel.BackColor = SystemColors.ControlDark;
            this._rightPanel.Controls.AddRange(new Control[] { this._rightColumnListBox });
            this._rightPanel.DockPadding.All = 1;
            this._rightPanel.Location = new Point(0x1c, 0x98);
            this._rightPanel.Name = "_rightPanel";
            this._rightPanel.Size = new Size(0xb0, 0x70);
            this._rightPanel.TabIndex = 6;
            this._rightColumnListBox.BorderStyle = BorderStyle.None;
            this._rightColumnListBox.Dock = DockStyle.Fill;
            this._rightColumnListBox.Enabled = false;
            this._rightColumnListBox.IntegralHeight = false;
            this._rightColumnListBox.Location = new Point(1, 1);
            this._rightColumnListBox.Name = "_rightColumnListBox";
            this._rightColumnListBox.Size = new Size(0xae, 110);
            this._rightColumnListBox.TabIndex = 1;
            this._okButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._okButton.FlatStyle = FlatStyle.System;
            this._okButton.Location = new Point(0x16c, 0x128);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 9;
            this._okButton.Text = "&OK";
            this._okButton.Click += new EventHandler(this._okButton_Click);
            this._cancelButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.FlatStyle = FlatStyle.System;
            this._cancelButton.Location = new Point(0x1bc, 0x128);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 10;
            this._cancelButton.Text = "Ca&ncel";
            this._cancelButton.Click += new EventHandler(this._cancelButton_Click);
            this._operatorLabel.Location = new Point(0xd0, 0x6c);
            this._operatorLabel.Name = "_operatorLabel";
            this._operatorLabel.Size = new Size(0x48, 0x10);
            this._operatorLabel.TabIndex = 2;
            this._operatorLabel.Text = "O&perator:";
            this._leftOperandGroupBox.Controls.AddRange(new Control[] { this._leftTablesLabel, this._leftColumnsLabel, this._leftTableComboBox, this._leftPanel });
            this._leftOperandGroupBox.FlatStyle = FlatStyle.System;
            this._leftOperandGroupBox.Location = new Point(8, 12);
            this._leftOperandGroupBox.Name = "_leftOperandGroupBox";
            this._leftOperandGroupBox.Size = new Size(0xc0, 0x114);
            this._leftOperandGroupBox.TabIndex = 1;
            this._leftOperandGroupBox.TabStop = false;
            this._leftOperandGroupBox.Text = "&Left Operand";
            this._leftTablesLabel.Location = new Point(8, 20);
            this._leftTablesLabel.Name = "_leftTablesLabel";
            this._leftTablesLabel.Size = new Size(0x48, 0x10);
            this._leftTablesLabel.TabIndex = 1;
            this._leftTablesLabel.Text = "&Table:";
            this._leftColumnsLabel.Location = new Point(8, 0x40);
            this._leftColumnsLabel.Name = "_leftColumnsLabel";
            this._leftColumnsLabel.Size = new Size(0x48, 0x10);
            this._leftColumnsLabel.TabIndex = 3;
            this._leftColumnsLabel.Text = "&Column:";
            this._rightOperandGroupBox.Controls.AddRange(new Control[] { this._rightColumnLabel, this._rightTableLabel, this._filterValueTextBox, this._rightPanel, this.radioButton1, this._rightTableComboBox, this._filterRadioButton });
            this._rightOperandGroupBox.FlatStyle = FlatStyle.System;
            this._rightOperandGroupBox.Location = new Point(0x130, 12);
            this._rightOperandGroupBox.Name = "_rightOperandGroupBox";
            this._rightOperandGroupBox.Size = new Size(0xd8, 0x114);
            this._rightOperandGroupBox.TabIndex = 4;
            this._rightOperandGroupBox.TabStop = false;
            this._rightOperandGroupBox.Text = "&Right Operand";
            this._rightColumnLabel.Location = new Point(0x18, 0x84);
            this._rightColumnLabel.Name = "_rightColumnLabel";
            this._rightColumnLabel.Size = new Size(0x48, 0x10);
            this._rightColumnLabel.TabIndex = 5;
            this._rightColumnLabel.Text = "Colu&mn:";
            this._rightTableLabel.Location = new Point(0x18, 0x54);
            this._rightTableLabel.Name = "_rightTableLabel";
            this._rightTableLabel.Size = new Size(0x48, 0x10);
            this._rightTableLabel.TabIndex = 4;
            this._rightTableLabel.Text = "T&able:";
            base.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(0x216, 0x144);
            base.Controls.AddRange(new Control[] { this._rightOperandGroupBox, this._leftOperandGroupBox, this._operatorLabel, this._cancelButton, this._okButton, this._operatorComboBox });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "CreateClauseDialog";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "WHERE Clause Builder";
            this._leftPanel.ResumeLayout(false);
            this._rightPanel.ResumeLayout(false);
            this._leftOperandGroupBox.ResumeLayout(false);
            this._rightOperandGroupBox.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            int num = 0;
            int num2 = 0;
            foreach (TableListItem item in this._leftTableComboBox.Items)
            {
                if (item.Table.Name.ToLower() == this._defaultTableName.ToLower())
                {
                    num2 = num;
                    break;
                }
                num++;
            }
            if (this._leftTableComboBox.Items.Count > 0)
            {
                this._leftTableComboBox.SelectedIndex = num2;
            }
            if (this._rightTableComboBox.Items.Count > 0)
            {
                this._rightTableComboBox.SelectedIndex = num2;
            }
            this._operatorComboBox.SelectedIndex = 0;
        }

        private void UpdateRightColumnListBox()
        {
            if (this._leftColumnListBox.SelectedItem != null)
            {
                ColumnListItem selectedItem = (ColumnListItem) this._leftColumnListBox.SelectedItem;
                DbType dbType = selectedItem.Column.DbType;
                this._rightColumnListBox.Items.Clear();
                TableListItem item2 = (TableListItem) this._rightTableComboBox.SelectedItem;
                if (item2 != null)
                {
                    foreach (Column column in item2.Table.Columns)
                    {
                        if (dbType == column.DbType)
                        {
                            this._rightColumnListBox.Items.Add(column.Name);
                        }
                    }
                }
                if (this._rightColumnListBox.Items.Count > 0)
                {
                    this._rightColumnListBox.SelectedIndex = 0;
                }
            }
        }

        public Microsoft.Matrix.Plugins.CodeWizards.Data.ClauseNode ClauseNode
        {
            get
            {
                return this._clauseNode;
            }
        }
    }
}

