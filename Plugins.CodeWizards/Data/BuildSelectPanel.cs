namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class BuildSelectPanel : WizardPanel
    {
        private IDictionary _checkedItems;
        private Label _columnsLabel;
        private ListView _columnsListView;
        private Panel _columnsPanel;
        private Label _previewLabel;
        private Microsoft.Matrix.Plugins.CodeWizards.Data.QueryBuilder _queryBuilder;
        private Button _selectAllButton;
        private Button _selectNoneButton;
        private MxTextBox _sqlTextBox;
        private ListBox _tableListBox;
        private Panel _tablePanel;
        private Label _tablesLabel;
        private WhereClauseBuilder _whereClauseBuilder;
        private Label _whereLabel;

        public BuildSelectPanel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
            this._whereClauseBuilder.ClauseAdded += new EventHandler(this.OnWhereClauseChanged);
            this._whereClauseBuilder.ClauseRemoved += new EventHandler(this.OnWhereClauseChanged);
        }

        private void _selectAllButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this._columnsListView.Items)
            {
                item.Checked = true;
            }
        }

        private void _selectNoneButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this._columnsListView.Items)
            {
                item.Checked = false;
            }
        }

        private void _tableListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._tableListBox.SelectedItem != null)
            {
                this._whereClauseBuilder.DefaultTableName = ((TableListItem) this._tableListBox.SelectedItem).Table.Name;
                this._columnsListView.Items.Clear();
                ListViewItem item = new ListViewItem("*");
                string key = this._tableListBox.Text + ".*";
                if (this.CheckedItems.Contains(key))
                {
                    item.Checked = true;
                }
                this._columnsListView.Items.Add(item);
                TableListItem selectedItem = (TableListItem) this._tableListBox.SelectedItem;
                foreach (Column column in selectedItem.Table.Columns)
                {
                    key = this._tableListBox.Text + "." + column.Name;
                    item = new ListViewItem(column.Name);
                    if (this.CheckedItems.Contains(key))
                    {
                        item.Checked = true;
                    }
                    this._columnsListView.Items.Add(item);
                }
                if (this._columnsListView.Items.Count > 0)
                {
                    this._columnsListView.Items[0].Selected = true;
                }
            }
        }

        private string GetSql()
        {
            IDictionary dictionary = new HybridDictionary(true);
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ");
            int num = 0;
            int count = this.CheckedItems.Count;
            foreach (TableColumn column in this.CheckedItems.Values)
            {
                builder.Append('[');
                builder.Append(column.Table);
                builder.Append(']');
                if (!dictionary.Contains(column.Table))
                {
                    dictionary[column.Table] = string.Empty;
                }
                builder.Append('.');
                if (column.Column != "*")
                {
                    builder.Append('[');
                    builder.Append(column.Column);
                    builder.Append(']');
                }
                else
                {
                    builder.Append('*');
                }
                num++;
                if (num < count)
                {
                    builder.Append(", ");
                }
            }
            builder.Append(" FROM ");
            string whereClause = this._whereClauseBuilder.WhereClause;
            foreach (string str2 in this._whereClauseBuilder.TablesUsed)
            {
                if (!dictionary.Contains(str2))
                {
                    dictionary[str2] = string.Empty;
                }
            }
            num = 0;
            count = dictionary.Count;
            foreach (string str3 in dictionary.Keys)
            {
                builder.Append('[');
                builder.Append(str3);
                builder.Append(']');
                num++;
                if (num < count)
                {
                    builder.Append(", ");
                }
            }
            if (whereClause.Length > 0)
            {
                builder.Append(" WHERE ");
                builder.Append(whereClause);
            }
            return builder.ToString();
        }

        private void InitializeComponent()
        {
            this._tableListBox = new ListBox();
            this._tablePanel = new Panel();
            this._columnsPanel = new Panel();
            this._columnsListView = new ListView();
            this._sqlTextBox = new MxTextBox();
            this._selectAllButton = new Button();
            this._selectNoneButton = new Button();
            this._tablesLabel = new Label();
            this._columnsLabel = new Label();
            this._previewLabel = new Label();
            this._whereClauseBuilder = new WhereClauseBuilder();
            this._whereLabel = new Label();
            this._tablePanel.SuspendLayout();
            this._columnsPanel.SuspendLayout();
            base.SuspendLayout();
            this._tableListBox.BorderStyle = BorderStyle.None;
            this._tableListBox.Dock = DockStyle.Fill;
            this._tableListBox.IntegralHeight = false;
            this._tableListBox.Location = new Point(1, 1);
            this._tableListBox.Name = "_tableListBox";
            this._tableListBox.TabIndex = 0;
            this._tableListBox.SelectedIndexChanged += new EventHandler(this._tableListBox_SelectedIndexChanged);
            this._tablePanel.BackColor = SystemColors.ControlDark;
            this._tablePanel.Controls.AddRange(new Control[] { this._tableListBox });
            this._tablePanel.DockPadding.All = 1;
            this._tablePanel.Location = new Point(12, 0x10);
            this._tablePanel.Name = "_tablePanel";
            this._tablePanel.Size = new Size(120, 0x54);
            this._tablePanel.TabIndex = 2;
            this._columnsPanel.BackColor = SystemColors.ControlDark;
            this._columnsPanel.Controls.AddRange(new Control[] { this._columnsListView });
            this._columnsPanel.DockPadding.All = 1;
            this._columnsPanel.Location = new Point(140, 0x10);
            this._columnsPanel.Name = "_columnsPanel";
            this._columnsPanel.Size = new Size(0x100, 0x54);
            this._columnsPanel.TabIndex = 4;
            this._columnsListView.BorderStyle = BorderStyle.None;
            this._columnsListView.CheckBoxes = true;
            this._columnsListView.Dock = DockStyle.Fill;
            this._columnsListView.FullRowSelect = true;
            this._columnsListView.Location = new Point(1, 1);
            this._columnsListView.MultiSelect = false;
            this._columnsListView.Name = "_columnsListView";
            this._columnsListView.TabIndex = 0;
            this._columnsListView.View = View.List;
            this._columnsListView.ItemCheck += new ItemCheckEventHandler(this.OnColumnsListViewItemCheck);
            this._sqlTextBox.AlwaysShowFocusCues = true;
            this._sqlTextBox.FlatAppearance = true;
            this._sqlTextBox.Location = new Point(12, 0xe4);
            this._sqlTextBox.Multiline = true;
            this._sqlTextBox.Name = "_sqlTextBox";
            this._sqlTextBox.ReadOnly = true;
            this._sqlTextBox.Size = new Size(0x1d8, 0x2c);
            this._sqlTextBox.ScrollBars = ScrollBars.Vertical;
            this._sqlTextBox.TabIndex = 10;
            this._sqlTextBox.Text = "";
            this._selectAllButton.FlatStyle = FlatStyle.System;
            this._selectAllButton.Location = new Point(0x198, 0x10);
            this._selectAllButton.Name = "_selectAllButton";
            this._selectAllButton.TabIndex = 5;
            this._selectAllButton.Text = "Select &All";
            this._selectAllButton.Click += new EventHandler(this._selectAllButton_Click);
            this._selectNoneButton.FlatStyle = FlatStyle.System;
            this._selectNoneButton.Location = new Point(0x198, 0x2c);
            this._selectNoneButton.Name = "_selectNoneButton";
            this._selectNoneButton.TabIndex = 6;
            this._selectNoneButton.Text = "Select Non&e";
            this._selectNoneButton.Click += new EventHandler(this._selectNoneButton_Click);
            this._tablesLabel.Location = new Point(12, 0);
            this._tablesLabel.Name = "_tablesLabel";
            this._tablesLabel.Size = new Size(0x38, 0x10);
            this._tablesLabel.TabIndex = 1;
            this._tablesLabel.Text = "&Tables:";
            this._columnsLabel.Location = new Point(140, 0);
            this._columnsLabel.Name = "_columnsLabel";
            this._columnsLabel.Size = new Size(0x38, 0x10);
            this._columnsLabel.TabIndex = 3;
            this._columnsLabel.Text = "C&olumns:";
            this._previewLabel.Location = new Point(12, 0xd4);
            this._previewLabel.Name = "_previewLabel";
            this._previewLabel.Size = new Size(0x58, 0x10);
            this._previewLabel.TabIndex = 9;
            this._previewLabel.Text = "&Preview:";
            this._whereClauseBuilder.Location = new Point(4, 0x70);
            this._whereClauseBuilder.Name = "_whereClauseBuilder";
            this._whereClauseBuilder.Size = new Size(0x1e8, 100);
            this._whereClauseBuilder.TabIndex = 8;
            this._whereLabel.Location = new Point(12, 0x68);
            this._whereLabel.Name = "_whereLabel";
            this._whereLabel.Size = new Size(90, 0x10);
            this._whereLabel.TabIndex = 7;
            this._whereLabel.Text = "&WHERE clause:";
            base.Controls.AddRange(new Control[] { this._whereLabel, this._whereClauseBuilder, this._previewLabel, this._columnsLabel, this._tablesLabel, this._selectNoneButton, this._selectAllButton, this._sqlTextBox, this._columnsPanel, this._tablePanel });
            base.Caption = "Construct a SELECT query";
            base.Description = "Check the columns you want returned and build the WHERE clause.";
            base.Name = "BuildSelectPanel";
            base.Size = new Size(500, 0x124);
            this._tablePanel.ResumeLayout(false);
            this._columnsPanel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnColumnsListViewItemCheck(object sender, ItemCheckEventArgs e)
        {
            string text = this._columnsListView.Items[e.Index].Text;
            string key = this._tableListBox.Text + "." + text;
            if (e.CurrentValue == CheckState.Unchecked)
            {
                TableColumn column = new TableColumn(this._tableListBox.Text, this._columnsListView.Items[e.Index].Text);
                this.CheckedItems[key] = column;
                if (text == "*")
                {
                    for (int i = 1; i < this._columnsListView.Items.Count; i++)
                    {
                        if (this._columnsListView.Items[i].Checked)
                        {
                            this._columnsListView.Items[i].Checked = false;
                        }
                    }
                }
                else if (this._columnsListView.Items[0].Checked)
                {
                    this._columnsListView.Items[0].Checked = false;
                }
            }
            else
            {
                this.CheckedItems.Remove(key);
            }
            this.UpdatePreview();
        }

        protected override bool OnNext()
        {
            if (this.CheckedItems.Count == 0)
            {
                MessageBox.Show("No columns have been selected to be displayed!");
                return false;
            }
            string sql = this.GetSql();
            this.QueryBuilder.Parameters.Clear();
            this.QueryBuilder.Query = sql;
            this.QueryBuilder.PreviewQuery = sql;
            foreach (QueryParameter parameter in this._whereClauseBuilder.Parameters)
            {
                this.QueryBuilder.Parameters.Add(parameter);
            }
            this.QueryBuilder.PreviewParameters.Clear();
            foreach (QueryParameter parameter2 in this.QueryBuilder.Parameters)
            {
                this.QueryBuilder.PreviewParameters.Add(parameter2);
            }
            return true;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (base.Visible && (this.Database != this._whereClauseBuilder.Database))
            {
                this._whereClauseBuilder.Database = this.Database;
                this._whereClauseBuilder.ServiceProvider = base.ServiceProvider;
                this._tableListBox.Items.Clear();
                this.CheckedItems.Clear();
                foreach (Table table in (IEnumerable) this.Database.Tables)
                {
                    this._tableListBox.Items.Add(new TableListItem(table));
                }
                if (this._tableListBox.Items.Count > 0)
                {
                    this._tableListBox.SelectedIndex = 0;
                }
            }
            this.UpdatePreview();
        }

        private void OnWhereClauseChanged(object sender, EventArgs e)
        {
            this.UpdatePreview();
        }

        private void UpdatePreview()
        {
            this._sqlTextBox.Text = this.GetSql();
        }

        private IDictionary CheckedItems
        {
            get
            {
                if (this._checkedItems == null)
                {
                    this._checkedItems = new HybridDictionary(true);
                }
                return this._checkedItems;
            }
        }

        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database Database
        {
            get
            {
                return this.QueryBuilder.Database;
            }
        }

        private Microsoft.Matrix.Plugins.CodeWizards.Data.QueryBuilder QueryBuilder
        {
            get
            {
                if (this._queryBuilder == null)
                {
                    this._queryBuilder = (Microsoft.Matrix.Plugins.CodeWizards.Data.QueryBuilder) base.WizardForm;
                }
                return this._queryBuilder;
            }
        }
    }
}

