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
    using System.Windows.Forms.Design;

    public class BuildUpdatePanel : WizardPanel
    {
        private Label _columnLabel;
        private ListView _columnsListView;
        private Panel _columnsPanel;
        private bool _internalChange;
        private Label _previewLabel;
        private Microsoft.Matrix.Plugins.CodeWizards.Data.QueryBuilder _queryBuilder;
        private MxTextBox _sqlTextBox;
        private Label _tableLabel;
        private ListBox _tableListBox;
        private Panel _tablePanel;
        private IDictionary _updateValues;
        private WhereClauseBuilder _whereClauseBuilder;
        private Label _whereLabel;

        public BuildUpdatePanel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
        }

        private string GetPreviewSql()
        {
            StringBuilder builder = new StringBuilder();
            string text = this._tableListBox.Text;
            builder.Append("SELECT ");
            builder.Append(text);
            builder.Append(".* FROM ");
            IDictionary dictionary = new HybridDictionary(true);
            dictionary[text] = string.Empty;
            foreach (string str2 in this._whereClauseBuilder.TablesUsed)
            {
                dictionary[str2] = string.Empty;
            }
            int num = 0;
            int count = dictionary.Count;
            foreach (string str3 in dictionary.Keys)
            {
                builder.Append(str3);
                num++;
                if (num < count)
                {
                    builder.Append(", ");
                }
            }
            string whereClause = this._whereClauseBuilder.WhereClause;
            if (whereClause.Length > 0)
            {
                builder.Append(" WHERE ");
                builder.Append(whereClause);
            }
            return builder.ToString();
        }

        private string GetSql()
        {
            string str = this._tableListBox.SelectedItem.ToString();
            StringBuilder builder = new StringBuilder();
            new ArrayList();
            IDictionaryEnumerator enumerator = this.UpdateValues.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                string[] strArray = ((string) current.Key).Split(new char[] { '.' });
                string str2 = strArray[0];
                string str3 = strArray[1];
                if (str2 == str)
                {
                    if (builder.Length != 0)
                    {
                        builder.Append(", ");
                    }
                    string str4 = (string) current.Value;
                    builder.Append('[');
                    builder.Append(str3);
                    builder.Append(']');
                    builder.Append('=');
                    builder.Append(str4);
                }
            }
            StringBuilder builder2 = new StringBuilder();
            builder2.Append("UPDATE [");
            builder2.Append(this._tableListBox.Text);
            builder2.Append("] SET ");
            builder2.Append(builder.ToString());
            if (this._whereClauseBuilder.WhereClause.Length > 0)
            {
                builder2.Append(" WHERE ");
                builder2.Append(this._whereClauseBuilder.WhereClause);
            }
            return builder2.ToString();
        }

        private void InitializeComponent()
        {
            this._columnLabel = new Label();
            this._columnsPanel = new Panel();
            this._columnsListView = new ListView();
            this._tablePanel = new Panel();
            this._tableListBox = new ListBox();
            this._tableLabel = new Label();
            this._whereClauseBuilder = new WhereClauseBuilder();
            this._previewLabel = new Label();
            this._sqlTextBox = new MxTextBox();
            this._whereLabel = new Label();
            this._columnsPanel.SuspendLayout();
            this._tablePanel.SuspendLayout();
            base.SuspendLayout();
            this._columnLabel.Location = new Point(0x7c, 0);
            this._columnLabel.Name = "_columnLabel";
            this._columnLabel.Size = new Size(60, 0x10);
            this._columnLabel.TabIndex = 3;
            this._columnLabel.Text = "C&olumn:";
            this._columnsPanel.BackColor = SystemColors.ControlDark;
            this._columnsPanel.Controls.AddRange(new Control[] { this._columnsListView });
            this._columnsPanel.DockPadding.All = 1;
            this._columnsPanel.Location = new Point(0x7c, 0x10);
            this._columnsPanel.Name = "_columnsPanel";
            this._columnsPanel.Size = new Size(0x160, 0x54);
            this._columnsPanel.TabIndex = 4;
            this._columnsListView.BorderStyle = BorderStyle.None;
            this._columnsListView.CheckBoxes = true;
            this._columnsListView.Dock = DockStyle.Fill;
            this._columnsListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this._columnsListView.HideSelection = false;
            this._columnsListView.Location = new Point(1, 1);
            this._columnsListView.MultiSelect = false;
            this._columnsListView.Name = "_columnsListView";
            this._columnsListView.TabIndex = 0;
            this._columnsListView.View = View.List;
            this._columnsListView.SelectedIndexChanged += new EventHandler(this.OnColumnsListViewSelectedIndexChanged);
            this._columnsListView.ItemCheck += new ItemCheckEventHandler(this.OnColumnsListViewItemCheck);
            this._tablePanel.BackColor = SystemColors.ControlDark;
            this._tablePanel.Controls.AddRange(new Control[] { this._tableListBox });
            this._tablePanel.DockPadding.All = 1;
            this._tablePanel.Location = new Point(8, 0x10);
            this._tablePanel.Name = "_tablePanel";
            this._tablePanel.Size = new Size(0x70, 0x54);
            this._tablePanel.TabIndex = 2;
            this._tableListBox.BorderStyle = BorderStyle.None;
            this._tableListBox.Dock = DockStyle.Fill;
            this._tableListBox.IntegralHeight = false;
            this._tableListBox.Location = new Point(1, 1);
            this._tableListBox.Name = "_tableListBox";
            this._tableListBox.TabIndex = 0;
            this._tableListBox.SelectedIndexChanged += new EventHandler(this.OnTableListBoxSelectedIndexChanged);
            this._tableLabel.Location = new Point(8, 0);
            this._tableLabel.Name = "_tableLabel";
            this._tableLabel.Size = new Size(0x30, 0x10);
            this._tableLabel.TabIndex = 1;
            this._tableLabel.Text = "&Table:";
            this._whereClauseBuilder.Database = null;
            this._whereClauseBuilder.Location = new Point(0, 0x70);
            this._whereClauseBuilder.Name = "_whereClauseBuilder";
            this._whereClauseBuilder.ServiceProvider = null;
            this._whereClauseBuilder.Size = new Size(0x1e8, 100);
            this._whereClauseBuilder.TabIndex = 6;
            this._whereClauseBuilder.ClauseRemoved += new EventHandler(this.OnWhereClauseBuilderChanged);
            this._whereClauseBuilder.ClauseAdded += new EventHandler(this.OnWhereClauseBuilderChanged);
            this._previewLabel.Location = new Point(12, 0xd4);
            this._previewLabel.Name = "_previewLabel";
            this._previewLabel.Size = new Size(0x38, 0x10);
            this._previewLabel.TabIndex = 7;
            this._previewLabel.Text = "&Preview:";
            this._sqlTextBox.Location = new Point(12, 0xe4);
            this._sqlTextBox.Multiline = true;
            this._sqlTextBox.Name = "_sqlTextBox";
            this._sqlTextBox.ReadOnly = true;
            this._sqlTextBox.ScrollBars = ScrollBars.Vertical;
            this._sqlTextBox.Size = new Size(0x1d4, 0x2c);
            this._sqlTextBox.TabIndex = 8;
            this._sqlTextBox.Text = "";
            this._whereLabel.Location = new Point(8, 0x68);
            this._whereLabel.Name = "_whereLabel";
            this._whereLabel.Size = new Size(0x38, 0x10);
            this._whereLabel.TabIndex = 5;
            this._whereLabel.Text = "&WHERE:";
            base.Controls.AddRange(new Control[] { this._whereLabel, this._previewLabel, this._sqlTextBox, this._whereClauseBuilder, this._columnLabel, this._columnsPanel, this._tablePanel, this._tableLabel });
            base.Caption = "Construct an UPDATE query";
            base.Description = "Select the columns you want to update and construct a WHERE clause.";
            base.Name = "BuildUpdatePanel";
            base.Size = new Size(0x1ec, 0x124);
            this._columnsPanel.ResumeLayout(false);
            this._tablePanel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnColumnsListViewItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!this._internalChange)
            {
                ColumnListViewItem item = (ColumnListViewItem) this._columnsListView.Items[e.Index];
                string key = this._tableListBox.Text + "." + item.Column.Name;
                if (e.CurrentValue == CheckState.Unchecked)
                {
                    SetDefaultDialog form = new SetDefaultDialog(base.ServiceProvider);
                    string name = item.Column.Name;
                    form.ColumnName = name;
                    form.DefaultValue = '@' + name;
                    IUIService service = (IUIService) this.GetService(typeof(IUIService));
                    if (service.ShowDialog(form) == DialogResult.OK)
                    {
                        e.NewValue = CheckState.Checked;
                        this.UpdateValues[key] = form.DefaultValue;
                    }
                    else
                    {
                        e.NewValue = CheckState.Unchecked;
                    }
                }
                else
                {
                    this.UpdateValues.Remove(key);
                }
                this.UpdatePreview();
            }
        }

        private void OnColumnsListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._columnsListView.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in this._columnsListView.SelectedItems)
                {
                    item.Selected = false;
                }
            }
        }

        protected override bool OnNext()
        {
            if (this.UpdateValues.Count == 0)
            {
                MessageBox.Show("No columns have been selected to update!");
                return false;
            }
            this.QueryBuilder.Parameters.Clear();
            this.QueryBuilder.PreviewParameters.Clear();
            this.QueryBuilder.Query = this.GetSql();
            this.QueryBuilder.PreviewQuery = this.GetPreviewSql();
            foreach (QueryParameter parameter in this._whereClauseBuilder.Parameters)
            {
                this.QueryBuilder.Parameters.Add(parameter);
                this.QueryBuilder.PreviewParameters.Add(parameter);
            }
            foreach (ColumnListViewItem item in this._columnsListView.Items)
            {
                string str = this._tableListBox.Text + "." + item.Column.Name;
                if (item.Checked)
                {
                    string name = (string) this.UpdateValues[str];
                    if (name.StartsWith("@"))
                    {
                        this.QueryBuilder.Parameters.Add(new QueryParameter(name, string.Empty, item.Column.DbType));
                    }
                }
            }
            this.QueryBuilder.ReturnType = typeof(int);
            return true;
        }

        private void OnTableListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            this._internalChange = true;
            try
            {
                if (this._tableListBox.SelectedItem != null)
                {
                    this._whereClauseBuilder.DefaultTableName = ((TableListItem) this._tableListBox.SelectedItem).Table.Name;
                    this._columnsListView.Items.Clear();
                    TableListItem selectedItem = (TableListItem) this._tableListBox.SelectedItem;
                    foreach (Column column in selectedItem.Table.Columns)
                    {
                        string key = this._tableListBox.Text + "." + column.Name;
                        ColumnListViewItem item2 = new ColumnListViewItem(column);
                        if (this.UpdateValues.Contains(key))
                        {
                            item2.Checked = true;
                        }
                        this._columnsListView.Items.Add(item2);
                    }
                    if (this._columnsListView.Items.Count > 0)
                    {
                        this._columnsListView.Items[0].Selected = true;
                    }
                }
                this.UpdatePreview();
            }
            finally
            {
                this._internalChange = false;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (base.Visible && (this._whereClauseBuilder.Database != this.Database))
            {
                this._whereClauseBuilder.Database = this.Database;
                this._whereClauseBuilder.ServiceProvider = base.ServiceProvider;
                foreach (Table table in (IEnumerable) this.Database.Tables)
                {
                    this._tableListBox.Items.Add(new TableListItem(table));
                }
                if (this._tableListBox.Items.Count > 0)
                {
                    this._tableListBox.SelectedIndex = 0;
                }
            }
        }

        private void OnWhereClauseBuilderChanged(object sender, EventArgs e)
        {
            this.UpdatePreview();
        }

        private void UpdatePreview()
        {
            this._sqlTextBox.Text = this.GetSql();
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

        private IDictionary UpdateValues
        {
            get
            {
                if (this._updateValues == null)
                {
                    this._updateValues = new HybridDictionary(true);
                }
                return this._updateValues;
            }
        }

        private class ColumnListViewItem : ListViewItem
        {
            private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column _column;

            public ColumnListViewItem(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column column) : base(column.Name)
            {
                this._column = column;
            }

            public override string ToString()
            {
                return this._column.Name;
            }

            public Microsoft.Matrix.Packages.DBAdmin.DBEngine.Column Column
            {
                get
                {
                    return this._column;
                }
            }
        }
    }
}

