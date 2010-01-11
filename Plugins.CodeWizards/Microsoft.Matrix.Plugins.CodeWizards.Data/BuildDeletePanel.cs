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

    public class BuildDeletePanel : WizardPanel
    {
        private Label _previewLabel;
        private Microsoft.Matrix.Plugins.CodeWizards.Data.QueryBuilder _queryBuilder;
        private MxTextBox _sqlTextBox;
        private Label _tableLabel;
        private ListBox _tableListBox;
        private Panel _tablePanel;
        private WhereClauseBuilder _whereClauseBuilder;
        private Label _whereLabel;

        public BuildDeletePanel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
            this._whereClauseBuilder.ClauseAdded += new EventHandler(this.OnWhereClauseBuilderChanged);
            this._whereClauseBuilder.ClauseRemoved += new EventHandler(this.OnWhereClauseBuilderChanged);
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
            StringBuilder builder = new StringBuilder();
            builder.Append("DELETE FROM ");
            builder.Append('[');
            builder.Append(this._tableListBox.Text);
            builder.Append(']');
            string whereClause = this._whereClauseBuilder.WhereClause;
            if (whereClause.Length > 0)
            {
                builder.Append(" WHERE ");
                builder.Append(whereClause);
            }
            return builder.ToString();
        }

        private void InitializeComponent()
        {
            this._whereClauseBuilder = new WhereClauseBuilder();
            this._tableListBox = new ListBox();
            this._tablePanel = new Panel();
            this._tableLabel = new Label();
            this._whereLabel = new Label();
            this._sqlTextBox = new MxTextBox();
            this._previewLabel = new Label();
            this._tablePanel.SuspendLayout();
            base.SuspendLayout();
            this._whereClauseBuilder.Database = null;
            this._whereClauseBuilder.Location = new Point(8, 0x70);
            this._whereClauseBuilder.Name = "_whereClauseBuilder";
            this._whereClauseBuilder.ServiceProvider = null;
            this._whereClauseBuilder.Size = new Size(0x1e8, 100);
            this._whereClauseBuilder.TabIndex = 4;
            this._tableListBox.BorderStyle = BorderStyle.None;
            this._tableListBox.Dock = DockStyle.Fill;
            this._tableListBox.IntegralHeight = false;
            this._tableListBox.Location = new Point(1, 1);
            this._tableListBox.Name = "_tableListBox";
            this._tableListBox.Size = new Size(470, 0x56);
            this._tableListBox.TabIndex = 1;
            this._tableListBox.SelectedIndexChanged += new EventHandler(this.OnTableListBoxSelectedIndexChanged);
            this._tablePanel.BackColor = SystemColors.ControlDark;
            this._tablePanel.Controls.AddRange(new Control[] { this._tableListBox });
            this._tablePanel.DockPadding.All = 1;
            this._tablePanel.Location = new Point(0x10, 0x10);
            this._tablePanel.Name = "_tablePanel";
            this._tablePanel.Size = new Size(0x1d8, 0x54);
            this._tablePanel.TabIndex = 2;
            this._tableLabel.Location = new Point(0x10, 0);
            this._tableLabel.Name = "_tableLabel";
            this._tableLabel.Size = new Size(0x2c, 0x10);
            this._tableLabel.TabIndex = 1;
            this._tableLabel.Text = "&Table:";
            this._whereLabel.Location = new Point(0x10, 0x68);
            this._whereLabel.Name = "_whereLabel";
            this._whereLabel.Size = new Size(0x38, 0x10);
            this._whereLabel.TabIndex = 3;
            this._whereLabel.Text = "W&HERE:";
            this._sqlTextBox.Location = new Point(0x10, 0xe4);
            this._sqlTextBox.Multiline = true;
            this._sqlTextBox.Name = "_sqlTextBox";
            this._sqlTextBox.ReadOnly = true;
            this._sqlTextBox.ScrollBars = ScrollBars.Vertical;
            this._sqlTextBox.Size = new Size(0x1d4, 0x2c);
            this._sqlTextBox.TabIndex = 6;
            this._sqlTextBox.Text = "";
            this._previewLabel.Location = new Point(0x10, 0xd4);
            this._previewLabel.Name = "_previewLabel";
            this._previewLabel.Size = new Size(0x38, 0x10);
            this._previewLabel.TabIndex = 5;
            this._previewLabel.Text = "&Preview:";
            base.Controls.AddRange(new Control[] { this._previewLabel, this._sqlTextBox, this._whereLabel, this._tableLabel, this._tablePanel, this._whereClauseBuilder });
            base.Caption = "BUILD a DELECT Query";
            base.Description = "Construct a DELETE statement by selecting a table and specifiying a WHERE clause.";
            base.Name = "BuildDeletePanel";
            base.Size = new Size(0x1f8, 0x124);
            this._tablePanel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected override bool OnNext()
        {
            if (this._tableListBox.SelectedItem == null)
            {
                MessageBox.Show("No table has been selected");
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
            this.QueryBuilder.ReturnType = typeof(int);
            return true;
        }

        private void OnTableListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._tableListBox.SelectedItem != null)
            {
                this._whereClauseBuilder.DefaultTableName = ((TableListItem) this._tableListBox.SelectedItem).Table.Name;
            }
            this.UpdatePreview();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (base.Visible && (this._whereClauseBuilder.Database != this.Database))
            {
                this._whereClauseBuilder.Database = this.Database;
                this._whereClauseBuilder.ServiceProvider = base.ServiceProvider;
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
            this.UpdatePreview();
        }

        private void OnWhereClauseBuilderChanged(object sender, EventArgs args)
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
    }
}

