namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Packages.Web.Projects.Ftp;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class FtpConnectionDialog : TaskForm
    {
        private MxButton _cancelButton;
        private FtpConnection _connection;
        private ConnectionAsyncTask _connectionTask;
        private GradientBand _gradientBand;
        private GroupBox _httpGroup;
        private Label _httpInfoLabel;
        private string _httpRoot;
        private MxLabel _httpRootLabel;
        private MxTextBox _httpRootTextBox;
        private string _httpUrl;
        private MxLabel _httpUrlLabel;
        private MxTextBox _httpUrlTextBox;
        private MxButton _okButton;
        private MxLabel _passwordLabel;
        private MxTextBox _passwordTextBox;
        private MxLabel _portLabel;
        private MxTextBox _portTextBox;
        private MxLabel _siteLabel;
        private MxTextBox _siteTextBox;
        private MxLabel _userNameLabel;
        private MxTextBox _userNameTextBox;

        public FtpConnectionDialog(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
            base.TaskGlyph = new Bitmap(typeof(FtpConnectionDialog), "FtpConnectionGlyph.bmp");
        }

        private void InitializeComponent()
        {
            this._gradientBand = new GradientBand();
            this._siteTextBox = new MxTextBox();
            this._portTextBox = new MxTextBox();
            this._userNameTextBox = new MxTextBox();
            this._passwordTextBox = new MxTextBox();
            this._httpRootTextBox = new MxTextBox();
            this._httpUrlTextBox = new MxTextBox();
            this._okButton = new MxButton();
            this._cancelButton = new MxButton();
            this._siteLabel = new MxLabel();
            this._portLabel = new MxLabel();
            this._userNameLabel = new MxLabel();
            this._passwordLabel = new MxLabel();
            this._httpRootLabel = new MxLabel();
            this._httpUrlLabel = new MxLabel();
            this._httpGroup = new GroupBox();
            this._httpInfoLabel = new Label();
            this._httpGroup.SuspendLayout();
            base.SuspendLayout();
            this._gradientBand.EndColor = Color.FromArgb(0xcc, 0xec, 0xff);
            this._gradientBand.GradientSpeed = 1;
            this._gradientBand.Location = new Point(0, 0x38);
            this._gradientBand.Name = "_gradientBand";
            this._gradientBand.ScrollSpeed = 6;
            this._gradientBand.Size = new Size(0x1b0, 4);
            this._gradientBand.StartColor = Color.FromArgb(0, 0x33, 0x87);
            this._gradientBand.TabIndex = 0;
            this._gradientBand.Visible = false;
            this._siteTextBox.AlwaysShowFocusCues = true;
            this._siteTextBox.FlatAppearance = true;
            this._siteTextBox.Location = new Point(0x6c, 0x48);
            this._siteTextBox.Name = "_siteTextBox";
            this._siteTextBox.Size = new Size(240, 20);
            this._siteTextBox.TabIndex = 2;
            this._siteTextBox.Text = "";
            this._siteTextBox.TextChanged += new EventHandler(this.OnTextChangedEntry);
            this._portTextBox.AlwaysShowFocusCues = true;
            this._portTextBox.FlatAppearance = true;
            this._portTextBox.Location = new Point(0x6c, 0x60);
            this._portTextBox.Name = "_portTextBox";
            this._portTextBox.Numeric = true;
            this._portTextBox.Size = new Size(60, 20);
            this._portTextBox.TabIndex = 4;
            this._portTextBox.Text = "21";
            this._userNameTextBox.AlwaysShowFocusCues = true;
            this._userNameTextBox.FlatAppearance = true;
            this._userNameTextBox.Location = new Point(0x6c, 0x80);
            this._userNameTextBox.Name = "_userNameTextBox";
            this._userNameTextBox.Size = new Size(240, 20);
            this._userNameTextBox.TabIndex = 6;
            this._userNameTextBox.Text = "";
            this._userNameTextBox.TextChanged += new EventHandler(this.OnTextChangedEntry);
            this._passwordTextBox.AlwaysShowFocusCues = true;
            this._passwordTextBox.FlatAppearance = true;
            this._passwordTextBox.Location = new Point(0x6c, 0x98);
            this._passwordTextBox.Name = "_passwordTextBox";
            this._passwordTextBox.PasswordStyle = true;
            this._passwordTextBox.Size = new Size(240, 20);
            this._passwordTextBox.TabIndex = 8;
            this._passwordTextBox.Text = "";
            this._passwordTextBox.TextChanged += new EventHandler(this.OnTextChangedEntry);
            this._httpRootTextBox.AlwaysShowFocusCues = true;
            this._httpRootTextBox.FlatAppearance = true;
            this._httpRootTextBox.Location = new Point(0x60, 0x38);
            this._httpRootTextBox.Name = "_httpRootTextBox";
            this._httpRootTextBox.Size = new Size(0x128, 20);
            this._httpRootTextBox.TabIndex = 2;
            this._httpRootTextBox.Text = "";
            this._httpUrlTextBox.AlwaysShowFocusCues = true;
            this._httpUrlTextBox.FlatAppearance = true;
            this._httpUrlTextBox.Location = new Point(0x60, 80);
            this._httpUrlTextBox.Name = "_httpUrlTextBox";
            this._httpUrlTextBox.Size = new Size(0x128, 20);
            this._httpUrlTextBox.TabIndex = 4;
            this._httpUrlTextBox.Text = "";
            this._okButton.FlatStyle = FlatStyle.System;
            this._okButton.Location = new Point(260, 0x130);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 10;
            this._okButton.Text = "OK";
            this._okButton.Click += new EventHandler(this.OnClickOKButton);
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.FlatStyle = FlatStyle.System;
            this._cancelButton.Location = new Point(0x158, 0x130);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 11;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.Click += new EventHandler(this.OnClickCancelButton);
            this._siteLabel.FlatStyle = FlatStyle.System;
            this._siteLabel.Location = new Point(0x10, 0x4a);
            this._siteLabel.Name = "_siteLabel";
            this._siteLabel.Size = new Size(80, 0x10);
            this._siteLabel.TabIndex = 1;
            this._siteLabel.Text = "&FTP Site:";
            this._portLabel.FlatStyle = FlatStyle.System;
            this._portLabel.Location = new Point(0x10, 100);
            this._portLabel.Name = "_portLabel";
            this._portLabel.Size = new Size(80, 0x10);
            this._portLabel.TabIndex = 3;
            this._portLabel.Text = "P&ort:";
            this._userNameLabel.FlatStyle = FlatStyle.System;
            this._userNameLabel.Location = new Point(0x10, 0x84);
            this._userNameLabel.Name = "_userNameLabel";
            this._userNameLabel.Size = new Size(80, 0x10);
            this._userNameLabel.TabIndex = 5;
            this._userNameLabel.Text = "&User Name:";
            this._passwordLabel.FlatStyle = FlatStyle.System;
            this._passwordLabel.Location = new Point(0x10, 0x9c);
            this._passwordLabel.Name = "_passwordLabel";
            this._passwordLabel.Size = new Size(80, 0x10);
            this._passwordLabel.TabIndex = 7;
            this._passwordLabel.Text = "&Password:";
            this._httpRootLabel.FlatStyle = FlatStyle.System;
            this._httpRootLabel.Location = new Point(8, 60);
            this._httpRootLabel.Name = "_httpRootLabel";
            this._httpRootLabel.Size = new Size(80, 0x10);
            this._httpRootLabel.TabIndex = 1;
            this._httpRootLabel.Text = "&Directory:";
            this._httpUrlLabel.FlatStyle = FlatStyle.System;
            this._httpUrlLabel.Location = new Point(8, 0x54);
            this._httpUrlLabel.Name = "_httpUrlLabel";
            this._httpUrlLabel.Size = new Size(80, 0x10);
            this._httpUrlLabel.TabIndex = 3;
            this._httpUrlLabel.Text = "&Web URL:";
            this._httpGroup.Controls.AddRange(new Control[] { this._httpInfoLabel, this._httpRootTextBox, this._httpUrlTextBox, this._httpUrlLabel, this._httpRootLabel });
            this._httpGroup.FlatStyle = FlatStyle.System;
            this._httpGroup.Location = new Point(12, 0xb8);
            this._httpGroup.Name = "_httpGroup";
            this._httpGroup.Size = new Size(0x198, 0x70);
            this._httpGroup.TabIndex = 9;
            this._httpGroup.TabStop = false;
            this._httpGroup.Text = "Web Application Information";
            this._httpInfoLabel.Location = new Point(8, 20);
            this._httpInfoLabel.Name = "_httpInfoLabel";
            this._httpInfoLabel.Size = new Size(0x188, 0x1c);
            this._httpInfoLabel.TabIndex = 0;
            this._httpInfoLabel.Text = "To run a Web application from this FTP connection, specify the path to the directory containing the application, and its associated HTTP URL.";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(430, 0x151);
            base.Controls.AddRange(new Control[] { this._httpGroup, this._cancelButton, this._okButton, this._passwordTextBox, this._passwordLabel, this._userNameTextBox, this._userNameLabel, this._portTextBox, this._portLabel, this._siteTextBox, this._siteLabel, this._gradientBand });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FtpConnectionDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            base.TaskBorderStyle = BorderStyle.FixedSingle;
            base.TaskCaption = "FTP Connection";
            base.TaskDescription = "Enter information about the FTP site and your user identity.";
            this.Text = "New FTP Connection";
            this._httpGroup.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnClickCancelButton(object sender, EventArgs e)
        {
            if (this._connectionTask != null)
            {
                this._gradientBand.Stop();
                this._gradientBand.Visible = false;
                this._okButton.Enabled = true;
                this._connectionTask = null;
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
                base.Close();
            }
        }

        private void OnClickOKButton(object sender, EventArgs e)
        {
            string userName = this._userNameTextBox.Text.Trim();
            string password = this._passwordTextBox.Text.Trim();
            string uriString = this._siteTextBox.Text.Trim();
            string s = this._portTextBox.Text.Trim();
            int port = 0x15;
            try
            {
                Uri uri = new Uri(uriString);
                uriString = uri.Host;
            }
            catch
            {
            }
            if (s.Length != 0)
            {
                try
                {
                    port = int.Parse(s);
                    if (port <= 0)
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    ((IMxUIService) this.GetService(typeof(IUIService))).ReportError("The specified port is not a valid port number.", this.Text, false);
                    return;
                }
            }
            this._httpRoot = this._httpRootTextBox.Text.Trim();
            if ((this._httpRoot.Length == 0) || (!this._httpRoot.StartsWith("/") && !this._httpRoot.EndsWith("/")))
            {
                this._okButton.Enabled = false;
                this._gradientBand.Visible = true;
                this._gradientBand.Start();
                this._connectionTask = new ConnectionAsyncTask(uriString, port, userName, password);
                this._connectionTask.Start(new AsyncTaskResultPostedEventHandler(this.OnConnectionComplete));
            }
            else
            {
                ((IMxUIService) this.GetService(typeof(IUIService))).ReportError("The specified Web directory is invalid.\r\nYou should not having leading or trailing slashes in your path.", this.Text, false);
            }
        }

        private void OnConnectionComplete(object sender, AsyncTaskResultPostedEventArgs e)
        {
            this._gradientBand.Stop();
            this._gradientBand.Visible = false;
            this._okButton.Enabled = true;
            this._connectionTask = null;
            this._connection = (FtpConnection) e.Data;
            if (this._connection != null)
            {
                this._httpUrl = this._httpUrlTextBox.Text.Trim();
                if (this._httpUrl.EndsWith("/"))
                {
                    this._httpUrl = this._httpUrl.Substring(0, this._httpUrl.Length - 1);
                }
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
            else
            {
                ((IMxUIService) this.GetService(typeof(IMxUIService))).ReportError("Could not connect to the specified FTP site.\r\nCheck the site address and user information before trying again.", this.Text, false);
            }
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            this.UpdateEnabledState();
        }

        private void OnTextChangedEntry(object sender, EventArgs e)
        {
            this.UpdateEnabledState();
        }

        private void UpdateEnabledState()
        {
            this._okButton.Enabled = ((this._siteTextBox.Text.Trim().Length != 0) && (this._userNameTextBox.Text.Trim().Length != 0)) && (this._passwordTextBox.Text.Trim().Length != 0);
        }

        public FtpConnection Connection
        {
            get
            {
                return this._connection;
            }
        }

        public string HttpRoot
        {
            get
            {
                return this._httpRoot;
            }
        }

        public string HttpUrl
        {
            get
            {
                return this._httpUrl;
            }
        }

        private sealed class ConnectionAsyncTask : AsyncTask
        {
            private FtpConnection _connection;
            private string _error;

            public ConnectionAsyncTask(string remoteHost, int port, string userName, string password)
            {
                this._connection = new FtpConnection(remoteHost, port, userName, password);
            }

            protected override void PerformTask()
            {
                try
                {
                    this._connection.Open();
                }
                catch (Exception exception)
                {
                    this._connection = null;
                    this._error = exception.Message;
                }
                base.PostResults(this._connection, 100, true);
            }

            public string Error
            {
                get
                {
                    return this._error;
                }
            }
        }
    }
}

