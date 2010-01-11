namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Win32;
    using System;
    using System.Diagnostics;
    using System.DirectoryServices;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class StartWebAppDialog : TaskForm
    {
        private string _appDirectory;
        private MxTextBox _appNameTextBox;
        private MxTextBox _appRootTextBox;
        private string _appUrl;
        private MxButton _cancelButton;
        private MxRadioButton _cassiniRadioButton;
        private MxCheckBox _dirBrowseCheckBox;
        private MxRadioButton _iisRadioButton;
        private string _initialAppDirectory;
        private MxTextBox _portTextBox;
        private MxButton _startButton;
        private const int DefaultCassiniPort = 80;
        private const int DefaultCassiniPortWithIIS = 0x1f90;

        public StartWebAppDialog(IServiceProvider serviceProvider, string appDirectory) : base(serviceProvider)
        {
            this.InitializeUserInterface();
            this._initialAppDirectory = appDirectory.ToLower();
            this._appRootTextBox.Text = appDirectory;
        }

        private int GetDefaultPort(bool iisEnabled)
        {
            PreferencesStore store;
            int defaultValue = iisEnabled ? 0x1f90 : 80;
            IPreferencesService service = (IPreferencesService) this.GetService(typeof(IPreferencesService));
            if ((service != null) && service.GetPreferencesStore(typeof(StartWebAppDialog), out store))
            {
                defaultValue = store.GetValue("Port", defaultValue);
            }
            return defaultValue;
        }

        private void InitializeUserInterface()
        {
            MxLabel label = new MxLabel();
            GroupLabel label2 = new GroupLabel();
            MxLabel label3 = new MxLabel();
            MxLabel label4 = new MxLabel();
            this._appRootTextBox = new MxTextBox();
            this._cassiniRadioButton = new MxRadioButton();
            this._portTextBox = new MxTextBox();
            this._iisRadioButton = new MxRadioButton();
            this._startButton = new MxButton();
            this._cancelButton = new MxButton();
            this._appNameTextBox = new MxTextBox();
            this._dirBrowseCheckBox = new MxCheckBox();
            label.Location = new Point(12, 0x4c);
            label.Size = new Size(0x74, 0x10);
            label.TabIndex = 0;
            label.Text = "Application &Directory:";
            this._appRootTextBox.AlwaysShowFocusCues = true;
            this._appRootTextBox.FlatAppearance = true;
            this._appRootTextBox.Location = new Point(0x84, 0x48);
            this._appRootTextBox.Size = new Size(0x128, 20);
            this._appRootTextBox.TabIndex = 1;
            this._appRootTextBox.TextChanged += new EventHandler(this.OnAppRootTextBoxTextChanged);
            label2.Location = new Point(12, 0x62);
            label2.Size = new Size(0x1a0, 0x10);
            label2.TabIndex = 2;
            label2.Text = "";
            this._cassiniRadioButton.Checked = true;
            this._cassiniRadioButton.Location = new Point(20, 120);
            this._cassiniRadioButton.Size = new Size(220, 20);
            this._cassiniRadioButton.TabIndex = 3;
            this._cassiniRadioButton.TabStop = true;
            this._cassiniRadioButton.Text = "Use ASP.NET Web &Matrix Server";
            this._cassiniRadioButton.CheckedChanged += new EventHandler(this.OnCassiniRadioButtonCheckedChanged);
            label4.Location = new Point(0x24, 0x92);
            label4.Size = new Size(0x5c, 0x10);
            label4.TabIndex = 5;
            label4.Text = "Application &Port:";
            this._portTextBox.AlwaysShowFocusCues = true;
            this._portTextBox.FlatAppearance = true;
            this._portTextBox.Location = new Point(0x84, 0x8e);
            this._portTextBox.Numeric = true;
            this._portTextBox.Size = new Size(0x40, 20);
            this._portTextBox.TabIndex = 6;
            this._portTextBox.Text = "";
            this._iisRadioButton.Location = new Point(20, 0xac);
            this._iisRadioButton.Size = new Size(220, 20);
            this._iisRadioButton.TabIndex = 4;
            this._iisRadioButton.Text = "Use or create an &IIS Virtual Root";
            this._iisRadioButton.CheckedChanged += new EventHandler(this.OnIISRadioButtonCheckedChanged);
            label3.Location = new Point(0x24, 0xc4);
            label3.Size = new Size(0x68, 0x10);
            label3.TabIndex = 7;
            label3.Text = "Application &Name:";
            this._appNameTextBox.AlwaysShowFocusCues = true;
            this._appNameTextBox.FlatAppearance = true;
            this._appNameTextBox.Location = new Point(0x84, 0xc0);
            this._appNameTextBox.Size = new Size(0xcc, 20);
            this._appNameTextBox.TabIndex = 8;
            this._appNameTextBox.Text = "";
            this._dirBrowseCheckBox.Location = new Point(0x24, 0xd8);
            this._dirBrowseCheckBox.Name = "_dirBrowseCheckBox";
            this._dirBrowseCheckBox.Size = new Size(0xa4, 20);
            this._dirBrowseCheckBox.TabIndex = 9;
            this._dirBrowseCheckBox.Text = "Enable Directory &Browsing";
            this._startButton.Location = new Point(0x10c, 0xf4);
            this._startButton.TabIndex = 10;
            this._startButton.Text = "Start";
            this._startButton.Click += new EventHandler(this.OnStartButtonClick);
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.Location = new Point(0x160, 0xf4);
            this._cancelButton.TabIndex = 12;
            this._cancelButton.Text = "Cancel";
            base.AcceptButton = this._startButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            this.AutoScroll = true;
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(0x1ba, 0x116);
            base.Controls.AddRange(new Control[] { this._dirBrowseCheckBox, this._appNameTextBox, label3, this._cancelButton, this._startButton, this._iisRadioButton, this._portTextBox, label4, this._cassiniRadioButton, label2, this._appRootTextBox, label });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = null;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            base.TaskBorderStyle = BorderStyle.FixedSingle;
            base.TaskCaption = "Start Web Application";
            base.TaskDescription = "Start a Web application at the selected application directory.";
            base.TaskGlyph = new Bitmap(typeof(StartWebAppDialog), "StartWebAppGlyph.bmp");
            this.Text = "Start Web Application";
        }

        private bool IsIISAvailable()
        {
            bool flag = false;
            RegistryKey localMachine = Registry.LocalMachine;
            RegistryKey key2 = null;
            try
            {
                key2 = localMachine.OpenSubKey(@"Software\Microsoft\InetStp", false);
                if (key2 != null)
                {
                    object obj2 = key2.GetValue("MajorVersion");
                    object obj3 = key2.GetValue("MinorVersion");
                    if ((obj2 == null) || (obj3 == null))
                    {
                        return flag;
                    }
                    flag = true;
                }
            }
            catch
            {
            }
            finally
            {
                if (key2 != null)
                {
                    key2.Close();
                    key2 = null;
                }
            }
            return flag;
        }

        private bool IsPortInUse(int port)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            bool connected = false;
            Socket socket = null;
            try
            {
                socket = new Socket(remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(remoteEP);
                connected = socket.Connected;
            }
            catch
            {
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                }
            }
            return connected;
        }

        private void OnAppRootTextBoxTextChanged(object sender, EventArgs e)
        {
            this.UpdateEnabledState();
        }

        private void OnBaseURLTextBoxTextChanged(object sender, EventArgs e)
        {
            this.UpdateEnabledState();
        }

        private void OnCassiniRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEnabledState();
        }

        private void OnIISRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEnabledState();
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            bool iisEnabled = this.IsIISAvailable();
            this._portTextBox.Text = this.GetDefaultPort(iisEnabled).ToString();
            this._iisRadioButton.Enabled = iisEnabled;
            this.UpdateEnabledState();
        }

        private void OnStartButtonClick(object sender, EventArgs e)
        {
            IUIService service = (IUIService) this.GetService(typeof(IUIService));
            string path = this._appRootTextBox.Text.Trim();
            if (path.EndsWith(@"\"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            string message = null;
            if (!Directory.Exists(path))
            {
                message = "The specified application directory, '" + path + "' does not exist.";
            }
            else if (!this._initialAppDirectory.StartsWith(path.ToLower()))
            {
                message = "The specified application directory must either be '" + this._initialAppDirectory + "', or must contain it.";
            }
            if (message != null)
            {
                service.ShowMessage(message, this.Text);
            }
            else
            {
                string str3;
                if (this._cassiniRadioButton.Checked)
                {
                    str3 = this.StartCassini(path);
                }
                else
                {
                    str3 = this.StartIISVRoot(path);
                }
                if (str3.Length != 0)
                {
                    this._appDirectory = path;
                    this._appUrl = str3;
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
        }

        private string StartCassini(string appDir)
        {
            IUIService service = (IUIService) this.GetService(typeof(IUIService));
            string message = null;
            string s = this._portTextBox.Text.Trim();
            int port = 0;
            if (s.Length == 0)
            {
                message = "You must specify a port number before starting a Web application using the Matrix Web Server.\r\nMatrix Web Server runs Web applications using URLs of the form 'http://localhost:<port number>/<application name>'.";
            }
            if (message == null)
            {
                try
                {
                    port = int.Parse(s);
                    if ((port < 1) || (port > 0xffff))
                    {
                        message = "Invalid port number. The port number must be between 1 and 65535.";
                    }
                }
                catch
                {
                    message = "The specified port was not valid.";
                }
                if ((message == null) && this.IsPortInUse(port))
                {
                    message = "The specified port number is already in use. Please specify a different port number.";
                    if (port == 80)
                    {
                        message = message + "\r\nPort 80 might be in use by IIS, if you have it enabled.";
                    }
                }
            }
            if (message != null)
            {
                service.ShowMessage(message, this.Text);
                return string.Empty;
            }
            bool flag = false;
            string str3 = string.Concat(new object[] { "/silent:1 /port:", port, " /path:\"", appDir, "\"" });
            string applicationPath = string.Empty;
            IApplicationIdentity identity = (IApplicationIdentity) this.GetService(typeof(IApplicationIdentity));
            if (identity != null)
            {
                applicationPath = identity.ApplicationPath;
            }
            Process process = new Process();
            process.StartInfo.Arguments = str3;
            process.StartInfo.FileName = Path.Combine(applicationPath, "WebServer.exe");
            process.StartInfo.UseShellExecute = true;
            try
            {
                flag = process.Start();
                process.WaitForInputIdle();
            }
            catch (Exception)
            {
            }
            if (!flag)
            {
                service.ShowMessage(string.Concat(new object[] { "Unable to start ASP.NET Web Matrix Server for the directory '", appDir, "' on port ", port, "." }), this.Text);
                return string.Empty;
            }
            try
            {
                IPreferencesService service2 = (IPreferencesService) this.GetService(typeof(IPreferencesService));
                if (service2 != null)
                {
                    service2.GetPreferencesStore(typeof(StartWebAppDialog)).SetValue("Port", port, 80);
                }
            }
            catch (Exception)
            {
            }
            if (port != 80)
            {
                return ("http://localhost:" + port);
            }
            return "http://localhost";
        }

        private string StartIISVRoot(string appDir)
        {
            IUIService service = (IUIService) this.GetService(typeof(IUIService));
            string str = this._appNameTextBox.Text.Trim();
            if ((str.Length == 0) || (str.IndexOf('/') >= 0))
            {
                service.ShowMessage("You must specify a valid application name before starting a Web application using an IIS virtual root.\r\nIIS virtual roots are accessed using URLs of the form 'http://localhost/<application name>'.", this.Text);
                return string.Empty;
            }
            try
            {
                string str2 = "IIS://localhost/W3SVC/1/ROOT";
                string str3 = str2 + "/" + str;
                bool flag = false;
                try
                {
                    flag = DirectoryEntry.Exists(str3);
                }
                catch
                {
                }
                if (flag)
                {
                    DirectoryEntry entry = new DirectoryEntry(str3);
                    string strA = (string) entry.Properties["Path"][0];
                    if ((strA == null) || (string.Compare(strA, appDir, true, CultureInfo.InvariantCulture) != 0))
                    {
                        service.ShowMessage("An application with the name '" + str + "' already exists.\r\nSelect a different application.", this.Text);
                        return string.Empty;
                    }
                    return ("http://localhost/" + str);
                }
                DirectoryEntry entry2 = new DirectoryEntry(str2);
                entry2.Invoke("Create", new object[] { "IISWebVirtualDir", str });
                DirectoryEntry entry3 = (DirectoryEntry) entry2.Invoke("Create", new object[] { "IIsWebVirtualDir", str });
                entry3.CommitChanges();
                entry3.Properties["Path"][0] = appDir;
                entry3.CommitChanges();
                if (this._dirBrowseCheckBox.Checked)
                {
                    entry3.Properties["EnableDirBrowsing"][0] = true;
                    entry3.CommitChanges();
                }
                entry3.Invoke("AppCreate", new object[] { true });
            }
            catch (Exception)
            {
                service.ShowMessage("Unable to create and initialize a new virtual root with the name '" + str + "'.\r\nCheck that IIS is correctly installed and is enabled, and that you have admin priviledges needed to create IIS applications.", this.Text);
                return string.Empty;
            }
            return ("http://localhost/" + str);
        }

        private void UpdateEnabledState()
        {
            bool flag = this._appRootTextBox.Text.Trim().Length != 0;
            bool flag2 = this._iisRadioButton.Checked;
            this._startButton.Enabled = flag;
            this._portTextBox.Enabled = this._cassiniRadioButton.Checked;
            this._appNameTextBox.Enabled = flag2;
            this._dirBrowseCheckBox.Enabled = flag2;
        }

        public string AppDirectory
        {
            get
            {
                return this._appDirectory;
            }
        }

        public string AppUrl
        {
            get
            {
                return this._appUrl;
            }
        }
    }
}

