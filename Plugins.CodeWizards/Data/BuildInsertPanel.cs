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

    public class BuildInsertPanel : WizardPanel
    {
        private Label _columnLabel;
        private ListView _columnsListView;
        private Panel _columnsPanel;
        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database _database;
        private IDictionary _defaults;
        private IDictionary _ignoreDefaults;
        private Label _infoLabel2;
        private bool _internalChange;
        private Microsoft.Matrix.Plugins.CodeWizards.Data.QueryBuilder _queryBuilder;
        private Label _tableLabel;
        private ListBox _tableListBox;
        private Panel _tablePanel;

        public BuildInsertPanel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this._infoLabel2 = new Label();
            this._columnLabel = new Label();
            this._columnsPanel = new Panel();
            this._columnsListView = new ListView();
            this._tablePanel = new Panel();
            this._tableListBox = new ListBox();
            this._tableLabel = new Label();
            this._columnsPanel.SuspendLayout();
            this._tablePanel.SuspendLayout();
            base.SuspendLayout();
            this._columnsPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this._infoLabel2.Location = new Point(8, 8);
            this._infoLabel2.Name = "_infoLabel2";
            this._infoLabel2.Size = new Size(0x1e8, 40);
            this._infoLabel2.TabIndex = 0;
            this._infoLabel2.Text = "You can specify default values for columns by checking them in the list below.  Columns with default values in the database are already checked, but can be changed by unchecking and rechecking the column.";
            this._columnLabel.Location = new Point(160, 0x40);
            this._columnLabel.Name = "_columnLabel";
            this._columnLabel.Size = new Size(60, 0x10);
            this._columnLabel.TabIndex = 3;
            this._columnLabel.Text = "C&olumn:";
            this._columnsPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._columnsPanel.BackColor = SystemColors.ControlDark;
            this._columnsPanel.Controls.AddRange(new Control[] { this._columnsListView });
            this._columnsPanel.DockPadding.All = 1;
            this._columnsPanel.Location = new Point(160, 80);
            this._columnsPanel.Name = "_columnsPanel";
            this._columnsPanel.Size = new Size(0x14c, 0xd0);
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
            this._tablePanel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._tablePanel.BackColor = SystemColors.ControlDark;
            this._tablePanel.Controls.AddRange(new Control[] { this._tableListBox });
            this._tablePanel.DockPadding.All = 1;
            this._tablePanel.Location = new Point(8, 80);
            this._tablePanel.Name = "_tablePanel";
            this._tablePanel.Size = new Size(0x94, 0xd0);
            this._tablePanel.TabIndex = 2;
            this._tableListBox.BorderStyle = BorderStyle.None;
            this._tableListBox.Dock = DockStyle.Fill;
            this._tableListBox.IntegralHeight = false;
            this._tableListBox.Location = new Point(1, 1);
            this._tableListBox.Name = "_tableListBox";
            this._tableListBox.TabIndex = 0;
            this._tableListBox.SelectedIndexChanged += new EventHandler(this.OnTableListBoxSelectedIndexChanged);
            this._tableLabel.Location = new Point(8, 0x40);
            this._tableLabel.Name = "_tableLabel";
            this._tableLabel.Size = new Size(0x30, 0x10);
            this._tableLabel.TabIndex = 1;
            this._tableLabel.Text = "&Table:";
            base.Controls.AddRange(new Control[] { this._infoLabel2, this._columnLabel, this._columnsPanel, this._tablePanel, this._tableLabel });
            base.Caption = "Construct an INSERT Query";
            base.Description = "Check the columns for which you want to set values.";
            base.Name = "BuildInsertPanel";
            base.Size = new Size(0x1f8, 0x124);
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
                    form.ColumnName = item.Column.Name;
                    string defaultValue = item.Column.DefaultValue;
                    if ((defaultValue != null) && (defaultValue.Length > 0))
                    {
                        form.DefaultValue = defaultValue.Substring(1, defaultValue.Length - 2);
                    }
                    IUIService service = (IUIService) this.GetService(typeof(IUIService));
                    if (service.ShowDialog(form) == DialogResult.OK)
                    {
                        e.NewValue = CheckState.Checked;
                        this.Defaults[key] = form.DefaultValue;
                        this.IgnoreDefaults.Remove(key);
                    }
                    else
                    {
                        e.NewValue = CheckState.Unchecked;
                    }
                }
                else if (this.Defaults[key] is int)
                {
                    e.NewValue = CheckState.Checked;
                    ((IUIService) this.GetService(typeof(IUIService))).ShowMessage("You cannot set a default value for an identity column.", "Identity Column", MessageBoxButtons.OK);
                }
                else
                {
                    this.Defaults.Remove(key);
                    this.IgnoreDefaults[key] = string.Empty;
                }
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
            if (this._tableListBox.SelectedItem == null)
            {
                MessageBox.Show(this, "You must select a table!");
                return false;
            }
            string str = this._tableListBox.SelectedItem.ToString();
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            ArrayList parameters = this.QueryBuilder.Parameters;
            parameters.Clear();
            foreach (ColumnListViewItem item in this._columnsListView.Items)
            {
                string name = item.Column.Name;
                if (item.Checked)
                {
                    string str3 = str + "." + name;
                    object obj2 = this.Defaults[str3];
                    if (!(obj2 is int))
                    {
                        string str4 = (string) obj2;
                        if (item.Column.DefaultValue != str4)
                        {
                            if (builder.Length != 0)
                            {
                                builder.Append(", ");
                            }
                            if (builder2.Length != 0)
                            {
                                builder2.Append(", ");
                            }
                            builder.Append('[');
                            builder.Append(name);
                            builder.Append(']');
                            builder2.Append(str4);
                        }
                    }
                    continue;
                }
                if (builder.Length != 0)
                {
                    builder.Append(", ");
                }
                if (builder2.Length != 0)
                {
                    builder2.Append(", ");
                }
                builder.Append('[');
                builder.Append(name);
                builder.Append(']');
                string str5 = '@' + name;
                builder2.Append("@");
                builder2.Append(name);
                parameters.Add(new QueryParameter(str5, string.Empty, item.Column.DbType));
            }
            StringBuilder builder3 = new StringBuilder();
            builder3.Append("INSERT INTO [");
            builder3.Append(this._tableListBox.Text);
            builder3.Append("] (");
            builder3.Append(builder.ToString());
            builder3.Append(") VALUES (");
            builder3.Append(builder2.ToString());
            builder3.Append(')');
            this.QueryBuilder.Query = builder3.ToString();
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
                    this._columnsListView.Items.Clear();
                    TableListItem selectedItem = (TableListItem) this._tableListBox.SelectedItem;
                    foreach (Column column in selectedItem.Table.Columns)
                    {
                        string key = this._tableListBox.Text + "." + column.Name;
                        ColumnListViewItem item2 = new ColumnListViewItem(column);
                        string defaultValue = column.DefaultValue;
                        if (((defaultValue != null) && (defaultValue.Length != 0)) && !this.IgnoreDefaults.Contains(key))
                        {
                            this.Defaults[key] = defaultValue;
                        }
                        if (column.IsIdentity)
                        {
                            this.Defaults[key] = 0;
                        }
                        if (this.Defaults.Contains(key))
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
            }
            finally
            {
                this._internalChange = false;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (base.Visible && (this.Database != this._database))
            {
                this._database = this.Database;
                this._tableListBox.Items.Clear();
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

        private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database Database
        {
            get
            {
                return this.QueryBuilder.Database;
            }
        }

        private IDictionary Defaults
        {
            get
            {
                if (this._defaults == null)
                {
                    this._defaults = new HybridDictionary(true);
                }
                return this._defaults;
            }
        }

        private IDictionary IgnoreDefaults
        {
            get
            {
                if (this._ignoreDefaults == null)
                {
                    this._ignoreDefaults = new HybridDictionary(true);
                }
                return this._ignoreDefaults;
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

