namespace Microsoft.Matrix.Packages.DBAdmin.UserInterface.Sql
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine.Sql;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class SqlDatabaseLoginForm : TaskForm
    {
        private MxButton _cancelButton;
        private MxButton _connectButton;
        private bool _connecting;
        private SqlConnectionSettings _connectionSettings;
        private ConnectionTask _connectionTask;
        private LinkLabel _createDbLink;
        private MxComboBox _databaseCombo;
        private MxLabel _databaseLabel;
        private MxTextBox _passwordEdit;
        private MxLabel _passwordLabel;
        private string _savedDatabaseText;
        private MxTextBox _serverEdit;
        private MxLabel _serverLabel;
        private bool _skipDropDownEvent;
        private MxRadioButton _sqlRadio;
        private MxTextBox _userNameEdit;
        private MxLabel _userNameLabel;
        private MxRadioButton _windowsRadio;

        public SqlDatabaseLoginForm(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
            base.Icon = null;
            base.TaskGlyph = new Bitmap(typeof(SqlDatabaseLoginForm), "SqlProjectGlyph.bmp");
        }

        private void CollectConnectionSettings()
        {
            if (this._sqlRadio.Checked)
            {
                this._connectionSettings = new SqlConnectionSettings(this._serverEdit.Text, this._userNameEdit.Text, this._passwordEdit.Text, (string) this._databaseCombo.SelectedValue);
            }
            else
            {
                this._connectionSettings = new SqlConnectionSettings(this._serverEdit.Text, (string) this._databaseCombo.SelectedValue);
            }
        }

        private void InitializeComponent()
        {
            this._databaseLabel = new MxLabel();
            this._createDbLink = new LinkLabel();
            this._serverLabel = new MxLabel();
            this._serverEdit = new MxTextBox();
            this._passwordEdit = new MxTextBox();
            this._userNameEdit = new MxTextBox();
            this._passwordLabel = new MxLabel();
            this._connectButton = new MxButton();
            this._databaseCombo = new MxComboBox();
            this._userNameLabel = new MxLabel();
            this._cancelButton = new MxButton();
            this._windowsRadio = new MxRadioButton();
            this._sqlRadio = new MxRadioButton();
            base.SuspendLayout();
            this._databaseLabel.Location = new Point(0x19, 0xda);
            this._databaseLabel.Name = "_databaseLabel";
            this._databaseLabel.Size = new Size(0x38, 13);
            this._databaseLabel.TabIndex = 8;
            this._databaseLabel.Text = "&Database:";
            this._databaseLabel.TextAlign = ContentAlignment.MiddleLeft;
            this._createDbLink.Location = new Point(0x18, 260);
            this._createDbLink.Name = "_createDbLink";
            this._createDbLink.Size = new Size(0x90, 13);
            this._createDbLink.TabIndex = 10;
            this._createDbLink.TabStop = true;
            this._createDbLink.Text = "Create a new database";
            this._createDbLink.LinkClicked += new LinkLabelLinkClickedEventHandler(this.OnCreateDbLinkClicked);
            this._serverLabel.Location = new Point(0x18, 0x4c);
            this._serverLabel.Name = "_serverLabel";
            this._serverLabel.Size = new Size(0x29, 13);
            this._serverLabel.TabIndex = 0;
            this._serverLabel.Text = "&Server:";
            this._serverLabel.TextAlign = ContentAlignment.MiddleLeft;
            this._serverEdit.AlwaysShowFocusCues = true;
            this._serverEdit.FlatAppearance = true;
            this._serverEdit.Location = new Point(0x5d, 0x48);
            this._serverEdit.Name = "_serverEdit";
            this._serverEdit.Size = new Size(260, 20);
            this._serverEdit.TabIndex = 1;
            this._serverEdit.Text = "(local)";
            this._serverEdit.TextChanged += new EventHandler(this.OnLoginTextChanged);
            this._passwordEdit.AlwaysShowFocusCues = true;
            this._passwordEdit.FlatAppearance = true;
            this._passwordEdit.Location = new Point(120, 0xae);
            this._passwordEdit.Name = "_passwordEdit";
            this._passwordEdit.PasswordStyle = true;
            this._passwordEdit.Size = new Size(0xe8, 20);
            this._passwordEdit.TabIndex = 7;
            this._passwordEdit.Text = "";
            this._passwordEdit.TextChanged += new EventHandler(this.OnLoginTextChanged);
            this._userNameEdit.AlwaysShowFocusCues = true;
            this._userNameEdit.FlatAppearance = true;
            this._userNameEdit.Location = new Point(120, 150);
            this._userNameEdit.Name = "_userNameEdit";
            this._userNameEdit.Size = new Size(0xe8, 20);
            this._userNameEdit.TabIndex = 5;
            this._userNameEdit.Text = "";
            this._userNameEdit.TextChanged += new EventHandler(this.OnLoginTextChanged);
            this._passwordLabel.Location = new Point(0x2a, 0xb2);
            this._passwordLabel.Name = "_passwordLabel";
            this._passwordLabel.Size = new Size(70, 13);
            this._passwordLabel.TabIndex = 6;
            this._passwordLabel.Text = "&Password:";
            this._passwordLabel.TextAlign = ContentAlignment.MiddleLeft;
            this._connectButton.Location = new Point(0xc0, 0xfc);
            this._connectButton.Name = "_connectButton";
            this._connectButton.Size = new Size(0x4d, 0x17);
            this._connectButton.TabIndex = 11;
            this._connectButton.Text = "OK";
            this._connectButton.Click += new EventHandler(this.OnConnectButtonClicked);
            this._databaseCombo.AlwaysShowFocusCues = true;
            this._databaseCombo.DisplayMember = "Name";
            this._databaseCombo.DropDownWidth = 0xfb;
            this._databaseCombo.FlatAppearance = true;
            this._databaseCombo.Location = new Point(0x5e, 0xd6);
            this._databaseCombo.Name = "_databaseCombo";
            this._databaseCombo.Size = new Size(260, 0x15);
            this._databaseCombo.TabIndex = 9;
            this._databaseCombo.ValueMember = "Name";
            this._databaseCombo.KeyDown += new KeyEventHandler(this.OnDatabaseComboKeyDown);
            this._databaseCombo.DropDown += new EventHandler(this.OnDatabaseComboDropDown);
            this._databaseCombo.SelectedValueChanged += new EventHandler(this.OnDatabaseComboTextChanged);
            this._databaseCombo.TextChanged += new EventHandler(this.OnDatabaseComboTextChanged);
            this._userNameLabel.Location = new Point(0x2a, 0x9a);
            this._userNameLabel.Name = "_userNameLabel";
            this._userNameLabel.Size = new Size(0x44, 13);
            this._userNameLabel.TabIndex = 4;
            this._userNameLabel.Text = "&User name:";
            this._userNameLabel.TextAlign = ContentAlignment.MiddleLeft;
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.Location = new Point(0x114, 0xfc);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new Size(0x4d, 0x17);
            this._cancelButton.TabIndex = 12;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.Click += new EventHandler(this.OnCancelButtonClicked);
            this._windowsRadio.Checked = true;
            this._windowsRadio.Location = new Point(0x1c, 0x68);
            this._windowsRadio.Name = "_windowsRadio";
            this._windowsRadio.Size = new Size(0xa8, 0x18);
            this._windowsRadio.TabIndex = 2;
            this._windowsRadio.TabStop = true;
            this._windowsRadio.Text = "Windows authentication";
            this._windowsRadio.CheckedChanged += new EventHandler(this.OnRadioCheckedChanged);
            this._sqlRadio.Location = new Point(0x1c, 0x7e);
            this._sqlRadio.Name = "_sqlRadio";
            this._sqlRadio.Size = new Size(0xa8, 0x16);
            this._sqlRadio.TabIndex = 3;
            this._sqlRadio.Text = "S&QL Server authentication";
            base.AcceptButton = this._connectButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(0x17a, 0x120);
            base.Controls.AddRange(new Control[] { this._passwordEdit, this._userNameEdit, this._sqlRadio, this._windowsRadio, this._createDbLink, this._databaseLabel, this._passwordLabel, this._userNameLabel, this._serverLabel, this._serverEdit, this._databaseCombo, this._cancelButton, this._connectButton });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DatabaseLoginForm";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            base.TaskBorderStyle = BorderStyle.FixedSingle;
            base.TaskCaption = "Connect to SQL or MSDE Database";
            base.TaskDescription = "Enter the connection information and select a database.";
            this.Text = "Connect to Database";
            base.ResumeLayout(false);
        }

        private void OnAsyncConnectCompleted(object sender, AsyncTaskResultPostedEventArgs e)
        {
            this.Connecting = false;
            ConnectType data = (ConnectType) e.Data;
            ConnectionTask task = sender as ConnectionTask;
            this._databaseCombo.Text = this._savedDatabaseText;
            if (((task != this._connectionTask) || task.IsCanceled) && (task.Database != null))
            {
                try
                {
                    task.Database.Disconnect();
                }
                catch
                {
                }
                return;
            }
            if (task.Database == null)
            {
                this.ReportError(task.Exception, "Unable to connect to the database.");
                return;
            }
            this._databaseCombo.Items.Clear();
            switch (data)
            {
                case ConnectType.DropDown:
                    foreach (string str in task.Database.GetDatabaseNames())
                    {
                        this._databaseCombo.Items.Add(str);
                    }
                    try
                    {
                        this._skipDropDownEvent = true;
                        this._databaseCombo.DroppedDown = false;
                        Application.DoEvents();
                        this._databaseCombo.DroppedDown = true;
                        Application.DoEvents();
                        goto Label_0193;
                    }
                    finally
                    {
                        this._skipDropDownEvent = false;
                    }
                    break;

                case ConnectType.ConnectButton:
                    break;

                default:
                    goto Label_0193;
            }
            this.ConnectionSettings.Database = this._savedDatabaseText.Trim();
            SqlDatabase database = new SqlDatabase(this.ConnectionSettings);
            try
            {
                database.Connect();
                if (!database.DatabaseExists(this.ConnectionSettings.Database))
                {
                    throw new InvalidOperationException(string.Format("A database named '{0}' could not be found. Please check the connection settings and the spelling of the database name.", this.ConnectionSettings.Database));
                }
                base.DialogResult = DialogResult.OK;
                base.Close();
                return;
            }
            catch (Exception exception)
            {
                this.ReportError(exception.Message, "Unable to connect to the database.");
            }
            finally
            {
                database.Disconnect();
            }
        Label_0193:
            task.Database.Disconnect();
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            if (this.Connecting)
            {
                if (this._connectionTask != null)
                {
                    this._connectionTask.Cancel();
                    this._connectionTask = null;
                }
                this.Connecting = false;
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
                base.Close();
            }
        }

        private void OnConnectButtonClicked(object sender, EventArgs e)
        {
            this._databaseCombo.Items.Clear();
            this.Connecting = true;
            this.CollectConnectionSettings();
            this._connectionTask = new ConnectionTask(this.ConnectionSettings, ConnectType.ConnectButton);
            this._savedDatabaseText = this._databaseCombo.Text;
            this._connectionTask.StartSynchronous(new AsyncTaskResultPostedEventHandler(this.OnAsyncConnectCompleted));
        }

        private void OnCreateDbLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PromptDialog dialog = new PromptDialog(base.ServiceProvider);
            dialog.EntryText = "Enter a name for the new database:";
        Label_0017:
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string s = dialog.EntryValue.Trim();
                string message = string.Empty;
                if (s.Length == 0)
                {
                    message = "The name must not be empty.";
                }
                else if (s.Length > 0x7c)
                {
                    message = "The name cannot be longer than 124 characters.";
                }
                else if (!SqlHelper.IsValidIdentifier(s))
                {
                    message = "The name is not valid. Check that it contains valid characters and is not too long.";
                }
                if (message.Length > 0)
                {
                    this.ReportError(message, "Unable to create a database with that name.");
                    goto Label_0017;
                }
                this.CollectConnectionSettings();
                SqlDatabase database = new SqlDatabase(this.ConnectionSettings);
                try
                {
                    database.Connect();
                    database.CreateDatabase(SqlHelper.RemoveDelimiters(s));
                    this._connectionSettings.Database = s;
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
                catch (Exception exception)
                {
                    this.ReportError(exception, "Could not create a new database.");
                    goto Label_0017;
                }
                finally
                {
                    database.Disconnect();
                }
            }
        }

        private void OnDatabaseComboDropDown(object sender, EventArgs e)
        {
            if (!this.Connecting && !this._skipDropDownEvent)
            {
                try
                {
                    this._skipDropDownEvent = true;
                    this._databaseCombo.Items.Clear();
                    this.Connecting = true;
                    this.CollectConnectionSettings();
                    this._connectionTask = new ConnectionTask(this.ConnectionSettings, ConnectType.DropDown);
                    this._connectionTask.StartSynchronous(new AsyncTaskResultPostedEventHandler(this.OnAsyncConnectCompleted));
                    this._savedDatabaseText = this._databaseCombo.Text;
                    this._databaseCombo.Text = "Connecting (please wait)...";
                }
                finally
                {
                    this._skipDropDownEvent = false;
                }
            }
        }

        private void OnDatabaseComboKeyDown(object sender, KeyEventArgs e)
        {
            if (((e.KeyCode == Keys.Down) && (e.Modifiers == Keys.None)) && !this._databaseCombo.DroppedDown)
            {
                this._databaseCombo.DroppedDown = true;
            }
        }

        private void OnDatabaseComboTextChanged(object sender, EventArgs e)
        {
            this.UpdateEnabledStates();
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            this.UpdateEnabledStates();
            this._serverEdit.Focus();
        }

        private void OnLoginTextChanged(object sender, EventArgs e)
        {
            this.UpdateEnabledStates();
        }

        private void OnRadioCheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEnabledStates();
        }

        private void ReportError(Exception e, string caption)
        {
            this.ReportError(SqlHelper.RemoveBracketedStrings(e.Message), caption);
        }

        private void ReportError(string message, string caption)
        {
            IMxUIService service = (IMxUIService) this.GetService(typeof(IMxUIService));
            if (service != null)
            {
                service.ReportError(message, caption, false);
            }
        }

        private void UpdateEnabledStates()
        {
            bool flag = this._sqlRadio.Checked && !this._connecting;
            this._userNameLabel.Enabled = flag;
            this._userNameEdit.Enabled = flag;
            this._passwordLabel.Enabled = flag;
            this._passwordEdit.Enabled = flag;
            this._sqlRadio.Enabled = !this._connecting;
            this._windowsRadio.Enabled = !this._connecting;
            this._serverLabel.Enabled = !this._connecting;
            this._serverEdit.Enabled = !this._connecting;
            bool flag2 = this._sqlRadio.Checked && (this._userNameEdit.Text.Length == 0);
            bool flag3 = this._serverEdit.Text.Length == 0;
            bool flag4 = !flag2 && !flag3;
            this._createDbLink.Enabled = flag4 && !this._connecting;
            this._databaseCombo.Enabled = flag4;
            this._databaseLabel.Enabled = flag4 && !this._connecting;
            this._connectButton.Enabled = (flag4 && !this._connecting) && ((this._databaseCombo.Text.Trim().Length > 0) || (this._databaseCombo.SelectedItem != null));
        }

        private bool Connecting
        {
            get
            {
                return this._connecting;
            }
            set
            {
                this._connecting = value;
                this.UpdateEnabledStates();
            }
        }

        public SqlConnectionSettings ConnectionSettings
        {
            get
            {
                return this._connectionSettings;
            }
        }

        private sealed class ConnectionTask : AsyncTask
        {
            private Microsoft.Matrix.Packages.DBAdmin.UserInterface.Sql.SqlDatabaseLoginForm.ConnectType _connectType;
            private SqlDatabase _database;
            private System.Exception _exception;

            public ConnectionTask(SqlConnectionSettings connectionSettings, Microsoft.Matrix.Packages.DBAdmin.UserInterface.Sql.SqlDatabaseLoginForm.ConnectType connectType)
            {
                this._connectType = connectType;
                this._database = new SqlDatabase(connectionSettings);
            }

            protected override void PerformTask()
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    this._database.Connect();
                }
                catch (System.Exception exception)
                {
                    this._database = null;
                    this._exception = exception;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                    if ((this._database != null) && base.IsCanceled)
                    {
                        SqlDatabase database = this._database;
                        this._database = null;
                        try
                        {
                            database.Disconnect();
                        }
                        catch
                        {
                        }
                    }
                    base.PostResults(this._connectType, 100, true);
                }
            }

            public Microsoft.Matrix.Packages.DBAdmin.UserInterface.Sql.SqlDatabaseLoginForm.ConnectType ConnectType
            {
                get
                {
                    return this._connectType;
                }
            }

            public SqlDatabase Database
            {
                get
                {
                    return this._database;
                }
            }

            public System.Exception Exception
            {
                get
                {
                    return this._exception;
                }
            }
        }

        private enum ConnectType
        {
            DropDown,
            ConnectButton,
            CreateDatabaseLink
        }
    }
}

