namespace Microsoft.Matrix.Packages.DBAdmin.UserInterface.Access
{
    using Microsoft.Matrix.Packages.DBAdmin.DBEngine.Access;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    internal sealed class AccessDatabaseLoginForm : TaskForm
    {
        private MxButton _cancelButton;
        private MxButton _chooseFileButton;
        private MxButton _connectButton;
        private bool _connecting;
        private AccessConnectionSettings _connectionSettings;
        private ConnectionTask _connectionTask;
        private LinkLabel _createDbLink;
        private MxLabel _exampleLabel;
        private MxTextBox _filenameEdit;
        private MxLabel _filenameLabel;

        public AccessDatabaseLoginForm(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
            base.Icon = null;
            base.TaskGlyph = new Bitmap(typeof(AccessDatabaseLoginForm), "AccessProjectGlyph.bmp");
        }

        private void CollectConnectionSettings()
        {
            this._connectionSettings = new AccessConnectionSettings(this._filenameEdit.Text);
        }

        private void InitializeComponent()
        {
            this._createDbLink = new LinkLabel();
            this._filenameLabel = new MxLabel();
            this._filenameEdit = new MxTextBox();
            this._connectButton = new MxButton();
            this._cancelButton = new MxButton();
            this._chooseFileButton = new MxButton();
            this._exampleLabel = new MxLabel();
            base.SuspendLayout();
            this._createDbLink.Location = new Point(0x18, 260);
            this._createDbLink.Name = "_createDbLink";
            this._createDbLink.Size = new Size(0x90, 13);
            this._createDbLink.TabIndex = 10;
            this._createDbLink.TabStop = true;
            this._createDbLink.Text = "Create a new database";
            this._createDbLink.LinkClicked += new LinkLabelLinkClickedEventHandler(this.OnCreateDbLinkClicked);
            this._filenameLabel.Location = new Point(0x18, 0x4c);
            this._filenameLabel.Name = "_filenameLabel";
            this._filenameLabel.Size = new Size(60, 13);
            this._filenameLabel.TabIndex = 0;
            this._filenameLabel.Text = "Data &File:";
            this._filenameLabel.TextAlign = ContentAlignment.MiddleLeft;
            this._filenameEdit.AlwaysShowFocusCues = true;
            this._filenameEdit.FlatAppearance = true;
            this._filenameEdit.Location = new Point(0x5d, 0x48);
            this._filenameEdit.Name = "_filenameEdit";
            this._filenameEdit.Size = new Size(220, 20);
            this._filenameEdit.TabIndex = 1;
            this._filenameEdit.Text = "";
            this._filenameEdit.TextChanged += new EventHandler(this.OnLoginTextChanged);
            this._connectButton.Location = new Point(0xc0, 0xfc);
            this._connectButton.Name = "_connectButton";
            this._connectButton.Size = new Size(0x4d, 0x17);
            this._connectButton.TabIndex = 11;
            this._connectButton.Text = "OK";
            this._connectButton.Click += new EventHandler(this.OnConnectButtonClicked);
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.Location = new Point(0x114, 0xfc);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new Size(0x4d, 0x17);
            this._cancelButton.TabIndex = 12;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.Click += new EventHandler(this.OnCancelButtonClicked);
            this._chooseFileButton.Location = new Point(0x13b, 70);
            this._chooseFileButton.Name = "_chooseFileButton";
            this._chooseFileButton.Size = new Size(0x18, 0x17);
            this._chooseFileButton.TabIndex = 2;
            this._chooseFileButton.Text = "...";
            this._chooseFileButton.Click += new EventHandler(this.OnChooseFileButtonClicked);
            this._exampleLabel.Location = new Point(0x60, 0x60);
            this._exampleLabel.Name = "_exampleLabel";
            this._exampleLabel.Size = new Size(0xd8, 13);
            this._exampleLabel.TabIndex = 0;
            this._exampleLabel.Text = @"Example: c:\example.mdb";
            this._exampleLabel.TextAlign = ContentAlignment.MiddleLeft;
            base.AcceptButton = this._connectButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(0x17a, 0x120);
            base.Controls.AddRange(new Control[] { this._createDbLink, this._filenameLabel, this._filenameEdit, this._chooseFileButton, this._cancelButton, this._connectButton, this._exampleLabel });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AccessDatabaseLoginForm";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            base.TaskBorderStyle = BorderStyle.FixedSingle;
            base.TaskCaption = "Connect to a Microsoft Access Database";
            base.TaskDescription = "Enter the filename of the database.";
            this.Text = "Connect to Database";
            base.ResumeLayout(false);
        }

        private void OnAsyncConnectCompleted(object sender, AsyncTaskResultPostedEventArgs e)
        {
            this.Connecting = false;
            ConnectType data = (ConnectType) e.Data;
            ConnectionTask task = sender as ConnectionTask;
            if (((task != this._connectionTask) || task.IsCanceled) && (task.Database != null))
            {
                try
                {
                    task.Database.Disconnect();
                }
                catch
                {
                }
            }
            else if (task.Database == null)
            {
                this.ReportError(task.Exception, "Unable to connect to the database.");
            }
            else
            {
                switch (data)
                {
                    case ConnectType.ConnectButton:
                    {
                        this.ConnectionSettings.Filename = this._filenameEdit.Text;
                        AccessDatabase database = new AccessDatabase(this.ConnectionSettings);
                        try
                        {
                            database.Connect();
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
                        break;
                    }
                }
                task.Database.Disconnect();
            }
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

        private void OnChooseFileButtonClicked(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select Database";
            dialog.CheckFileExists = true;
            dialog.DereferenceLinks = true;
            dialog.Multiselect = false;
            dialog.Filter = "Microsoft Access Databases (*.mdb)|*.mdb";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this._filenameEdit.Text = dialog.FileName;
            }
        }

        private void OnConnectButtonClicked(object sender, EventArgs e)
        {
            this.Connecting = true;
            this.CollectConnectionSettings();
            this._connectionTask = new ConnectionTask(this.ConnectionSettings, ConnectType.ConnectButton);
            this._connectionTask.StartSynchronous(new AsyncTaskResultPostedEventHandler(this.OnAsyncConnectCompleted));
        }

        private void OnCreateDbLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Microsoft Access Databases (*.mdb)|*.mdb";
            dialog.AddExtension = true;
            dialog.CheckPathExists = true;
            dialog.DefaultExt = "mdb";
            dialog.OverwritePrompt = true;
            dialog.Title = "Create New Access Database";
        Label_003C:
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = dialog.FileName;
                string message = string.Empty;
                if (fileName.Length == 0)
                {
                    message = "The name must not be empty.";
                }
                else if (fileName.IndexOfAny(Path.InvalidPathChars) != -1)
                {
                    message = "The filename contains invalid path characters.";
                }
                else if (!Path.IsPathRooted(fileName))
                {
                    message = "The filename must contain a full path.\r\nExample: c:\\example.mdb";
                }
                else if (!fileName.ToLower().EndsWith(".mdb"))
                {
                    message = "The filename must have the '.mdb' file extension.\r\nExample: c:\\example.mdb";
                }
                if (message.Length > 0)
                {
                    this.ReportError(message, "Unable to create a database with that name.");
                    goto Label_003C;
                }
                this.CollectConnectionSettings();
                try
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                    AccessDatabase.CreateDatabase(fileName);
                    this._connectionSettings.Filename = fileName;
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
                catch (Exception exception)
                {
                    this.ReportError(exception, "Could not create a new database.");
                    goto Label_003C;
                }
            }
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            this.UpdateEnabledStates();
            this._filenameEdit.Focus();
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
            this.ReportError(e.Message, caption);
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
            this._filenameLabel.Enabled = !this._connecting;
            this._filenameEdit.Enabled = !this._connecting;
            this._createDbLink.Enabled = !this._connecting;
            this._connectButton.Enabled = (this._filenameEdit.Text.Length > 0) && !this._connecting;
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

        public AccessConnectionSettings ConnectionSettings
        {
            get
            {
                return this._connectionSettings;
            }
        }

        private sealed class ConnectionTask : AsyncTask
        {
            private Microsoft.Matrix.Packages.DBAdmin.UserInterface.Access.AccessDatabaseLoginForm.ConnectType _connectType;
            private AccessDatabase _database;
            private System.Exception _exception;

            public ConnectionTask(AccessConnectionSettings connectionSettings, Microsoft.Matrix.Packages.DBAdmin.UserInterface.Access.AccessDatabaseLoginForm.ConnectType connectType)
            {
                this._connectType = connectType;
                this._database = new AccessDatabase(connectionSettings);
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
                        AccessDatabase database = this._database;
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

            public Microsoft.Matrix.Packages.DBAdmin.UserInterface.Access.AccessDatabaseLoginForm.ConnectType ConnectType
            {
                get
                {
                    return this._connectType;
                }
            }

            public AccessDatabase Database
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
            ConnectButton,
            CreateDatabaseLink
        }
    }
}

