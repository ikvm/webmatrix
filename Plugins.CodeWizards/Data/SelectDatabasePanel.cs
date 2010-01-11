namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.Services;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class SelectDatabasePanel : WizardPanel
    {
        private MxButton _createButton;
        private MxComboBox _databaseComboBox;
        private MxComboBox _databaseTypeComboBox;
        private MxLabel _databaseTypeLabel;
        private MxLabel _descriptionLabel;
        private MxInfoLabel _infoLabel;
        private bool _initialShow;
        private MxGroupBox _newDatabaseGroupBox;
        private Microsoft.Matrix.Plugins.CodeWizards.Data.QueryBuilder _queryBuilder;
        private MxLabel _selectDatabaseLabel;

        public SelectDatabasePanel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
            this._initialShow = true;
        }

        private void InitializeComponent()
        {
            this._selectDatabaseLabel = new MxLabel();
            this._databaseComboBox = new MxComboBox();
            this._newDatabaseGroupBox = new MxGroupBox();
            this._databaseTypeLabel = new MxLabel();
            this._databaseTypeComboBox = new MxComboBox();
            this._infoLabel = new MxInfoLabel();
            this._descriptionLabel = new MxLabel();
            this._createButton = new MxButton();
            this._newDatabaseGroupBox.SuspendLayout();
            base.SuspendLayout();
            this._selectDatabaseLabel.Location = new Point(12, 0x40);
            this._selectDatabaseLabel.Name = "_selectDatabaseLabel";
            this._selectDatabaseLabel.Size = new Size(100, 0x10);
            this._selectDatabaseLabel.TabIndex = 0;
            this._selectDatabaseLabel.Text = "&Select a database";
            this._databaseComboBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this._databaseComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this._databaseComboBox.Location = new Point(0x70, 0x3d);
            this._databaseComboBox.Name = "_databaseComboBox";
            this._databaseComboBox.Size = new Size(0x124, 0x15);
            this._databaseComboBox.TabIndex = 1;
            this._databaseComboBox.SelectedIndexChanged += new EventHandler(this.OnDatabaseComboBoxSelectedIndexChanged);
            this._newDatabaseGroupBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this._newDatabaseGroupBox.Controls.Add(this._createButton);
            this._newDatabaseGroupBox.Controls.Add(this._databaseTypeComboBox);
            this._newDatabaseGroupBox.Controls.Add(this._databaseTypeLabel);
            this._newDatabaseGroupBox.Enabled = false;
            this._newDatabaseGroupBox.Location = new Point(12, 0x60);
            this._newDatabaseGroupBox.Name = "_newDatabaseGroupBox";
            this._newDatabaseGroupBox.Size = new Size(0x188, 80);
            this._newDatabaseGroupBox.TabIndex = 2;
            this._newDatabaseGroupBox.TabStop = false;
            this._newDatabaseGroupBox.Text = "Create a &new database connection";
            this._databaseTypeLabel.Location = new Point(12, 0x18);
            this._databaseTypeLabel.Name = "_databaseTypeLabel";
            this._databaseTypeLabel.Size = new Size(0x7c, 0x10);
            this._databaseTypeLabel.TabIndex = 0;
            this._databaseTypeLabel.Text = "Select a database &type:";
            this._databaseTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this._databaseTypeComboBox.InitialText = "Select a database type";
            this._databaseTypeComboBox.Location = new Point(12, 40);
            this._databaseTypeComboBox.Name = "_databaseTypeComboBox";
            this._databaseTypeComboBox.Size = new Size(0xa4, 0x15);
            this._databaseTypeComboBox.TabIndex = 1;
            this._databaseTypeComboBox.SelectedIndexChanged += new EventHandler(this.OnDatabaseTypeComboBoxSelectedIndexChanged);
            this._infoLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this._infoLabel.Location = new Point(12, 0xe8);
            this._infoLabel.Name = "_infoLabel";
            this._infoLabel.Size = new Size(0x188, 60);
            this._infoLabel.TabIndex = 4;
            this._infoLabel.Text = "";
            this._infoLabel.Visible = false;
            this._descriptionLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this._descriptionLabel.Location = new Point(8, 8);
            this._descriptionLabel.Name = "_descriptionLabel";
            this._descriptionLabel.Size = new Size(0x184, 0x2c);
            this._descriptionLabel.TabStop = false;
            this._descriptionLabel.Text = "Use the drop-down to select an existing database connection for your query.  If you want to create a new database connection, select the <New Database Connection> option in the drop down, select a database type, and click the 'Create' button.";
            this._createButton.Enabled = false;
            this._createButton.Location = new Point(180, 40);
            this._createButton.Name = "_createButton";
            this._createButton.TabIndex = 2;
            this._createButton.Text = "&Create...";
            this._createButton.Click += new EventHandler(this.OnCreateButtonClick);
            base.Caption = "Select a database connection";
            base.Controls.Add(this._descriptionLabel);
            base.Controls.Add(this._infoLabel);
            base.Controls.Add(this._newDatabaseGroupBox);
            base.Controls.Add(this._databaseComboBox);
            base.Controls.Add(this._selectDatabaseLabel);
            base.Description = "Select an existing database connection or create a new database connection.";
            base.Name = "UserControl1";
            base.Size = new Size(0x1a0, 0x124);
            this._newDatabaseGroupBox.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnCreateButtonClick(object sender, EventArgs e)
        {
            if (this._databaseTypeComboBox.SelectedItem != null)
            {
                IProjectFactory factory = ((ProjectFactoryItem) this._databaseTypeComboBox.SelectedItem).Factory;
                IProjectManager service = (IProjectManager) base.ServiceProvider.GetService(typeof(IProjectManager));
                if (service != null)
                {
                    DatabaseProject project = service.CreateProject(factory.GetType(), null) as DatabaseProject;
                    if (project != null)
                    {
                        this._infoLabel.Visible = true;
                        this._infoLabel.Image = MxInfoLabel.InfoGlyph;
                        this._infoLabel.Text = "A new " + factory.Name + " has been created and added to the Data workspace.";
                        IDataProviderDatabase database = (IDataProviderDatabase) project.Database;
                        if (database != null)
                        {
                            this.RefreshDatabaseComboBox();
                            foreach (object obj2 in this._databaseComboBox.Items)
                            {
                                DatabaseItem item = obj2 as DatabaseItem;
                                if (item != null)
                                {
                                    IDataProviderDatabase database2 = (IDataProviderDatabase) item.Database;
                                    if ((string.Compare(database2.ProviderName, database.ProviderName, true) == 0) && (string.Compare(database2.ConnectionString, database.ConnectionString, true) == 0))
                                    {
                                        this._databaseComboBox.SelectedItem = item;
                                        this._infoLabel.Text = this._infoLabel.Text + "  The new connection has also been automatically selected in the database drop-down list.";
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnDatabaseComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._databaseComboBox.SelectedItem is string)
            {
                this._newDatabaseGroupBox.Enabled = true;
            }
            else
            {
                this._newDatabaseGroupBox.Enabled = false;
            }
            base.UpdateWizardState();
        }

        private void OnDatabaseTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._databaseTypeComboBox.SelectedItem != null)
            {
                this._createButton.Enabled = true;
            }
            else
            {
                this._createButton.Enabled = false;
            }
        }

        protected override bool OnNext()
        {
            DatabaseItem selectedItem = this._databaseComboBox.SelectedItem as DatabaseItem;
            this.QueryBuilder.Database = selectedItem.Database;
            return true;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (base.Visible && this._initialShow)
            {
                this._initialShow = false;
                this.RefreshDatabaseComboBox();
                if (this._databaseComboBox.Items.Count > 0)
                {
                    this._databaseComboBox.SelectedIndex = 0;
                }
                IProjectManager service = (IProjectManager) base.ServiceProvider.GetService(typeof(IProjectManager));
                if (service != null)
                {
                    this._databaseTypeComboBox.Items.Clear();
                    foreach (IProjectFactory factory in service.GetProjectFactories(typeof(DatabaseProject)))
                    {
                        if (typeof(IDataProviderDatabaseProject).IsAssignableFrom(factory.ProjectType))
                        {
                            this._databaseTypeComboBox.Items.Add(new ProjectFactoryItem(factory));
                        }
                    }
                    if (this._databaseTypeComboBox.Items.Count > 0)
                    {
                        this._databaseTypeComboBox.SelectedIndex = 0;
                    }
                }
            }
        }

        private void RefreshDatabaseComboBox()
        {
            IDatabaseManager service = (IDatabaseManager) base.ServiceProvider.GetService(typeof(IDatabaseManager));
            if (service != null)
            {
                this._databaseComboBox.Items.Clear();
                foreach (Database database in service.DatabaseConnections)
                {
                    if (database is IDataProviderDatabase)
                    {
                        this._databaseComboBox.Items.Add(new DatabaseItem(database));
                    }
                }
            }
            this._databaseComboBox.Items.Add("<New Database Connection>");
        }

        public override bool NextEnabled
        {
            get
            {
                return (this._databaseComboBox.SelectedItem is DatabaseItem);
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

        private sealed class DatabaseItem
        {
            private Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database _database;

            public DatabaseItem(Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database database)
            {
                this._database = database;
            }

            public override string ToString()
            {
                return this._database.DisplayName;
            }

            public Microsoft.Matrix.Packages.DBAdmin.DBEngine.Database Database
            {
                get
                {
                    return this._database;
                }
            }
        }

        private sealed class ProjectFactoryItem
        {
            private IProjectFactory _factory;

            public ProjectFactoryItem(IProjectFactory factory)
            {
                this._factory = factory;
            }

            public override string ToString()
            {
                return this._factory.Name;
            }

            public IProjectFactory Factory
            {
                get
                {
                    return this._factory;
                }
            }
        }
    }
}

