namespace Microsoft.Matrix.WebServer
{
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.WebHost;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using System.ComponentModel.Design;

    internal sealed class WebServerForm : TaskForm
    {
        private MxLabel _appRootLabel;
        private MxTextBox _appRootTextBox;
        private bool _exitMode;
        private MxLabel _hyperlinkLabel;
        private LinkLabel _hyperlinkLinkLabel;
        private MxLabel _physicalPathLabel;
        private MxTextBox _physicalPathTextBox;
        private MxLabel _portLabel;
        private MxTextBox _portTextBox;
        private MxButton _restartButton;
        private Server _server;
        private MxButton _stopButton;
        private TrayIcon _trayIcon;
        private MxContextMenu _trayMenu;
        private IContainer components;
        private const int WM_QUERYENDSESSION = 0x11;

        public WebServerForm(Server server) : base(new ServiceContainer())
        {
            this.InitializeComponent();
            base.Icon = new Icon(typeof(WebServerForm), "WebServerForm.ico");
            base.TaskGlyph = new Bitmap(typeof(WebServerForm), "WebServerForm.bmp");
            this._server = server;
            this._physicalPathTextBox.Text = this._server.PhysicalPath;
            this._appRootTextBox.Text = this._server.VirtualPath;
            this._portTextBox.Text = this._server.Port.ToString();
            this._hyperlinkLinkLabel.Text = this._server.RootUrl;
            this.Text = "Microsoft ASP.NET Web Matrix Server - Port " + this._server.Port;
            this._trayIcon.Icon = new Icon(typeof(WebServerForm), "WebServerTray.ico");
            this._trayIcon.Text = this.Text;
            this._trayIcon.Visible = true;
            this._trayIcon.ShowBalloon("Microsoft ASP.NET Web Matrix Server", this._server.RootUrl, 15);
        }

        private void DoLaunch()
        {
            Process.Start(this._server.RootUrl);
        }

        private void DoRestart()
        {
            this._server.Restart();
        }

        private void DoShow()
        {
            base.Show();
            base.Focus();
            base.WindowState = FormWindowState.Normal;
        }

        private void DoStop()
        {
            this._exitMode = true;
            base.Close();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._appRootTextBox = new Microsoft.Matrix.UIComponents.MxTextBox();
            this._portTextBox = new Microsoft.Matrix.UIComponents.MxTextBox();
            this._portLabel = new Microsoft.Matrix.UIComponents.MxLabel();
            this._restartButton = new Microsoft.Matrix.UIComponents.MxButton();
            this._hyperlinkLabel = new Microsoft.Matrix.UIComponents.MxLabel();
            this._hyperlinkLinkLabel = new System.Windows.Forms.LinkLabel();
            this._physicalPathLabel = new Microsoft.Matrix.UIComponents.MxLabel();
            this._appRootLabel = new Microsoft.Matrix.UIComponents.MxLabel();
            this._stopButton = new Microsoft.Matrix.UIComponents.MxButton();
            this._physicalPathTextBox = new Microsoft.Matrix.UIComponents.MxTextBox();
            this._trayIcon = new Microsoft.Matrix.UIComponents.TrayIcon(this.components);
            this.SuspendLayout();
            // 
            // _appRootTextBox
            // 
            this._appRootTextBox.AlwaysShowFocusCues = true;
            this._appRootTextBox.FlatAppearance = true;
            this._appRootTextBox.Location = new System.Drawing.Point(116, 92);
            this._appRootTextBox.Name = "_appRootTextBox";
            this._appRootTextBox.ReadOnly = true;
            this._appRootTextBox.Size = new System.Drawing.Size(296, 20);
            this._appRootTextBox.TabIndex = 3;
            this._appRootTextBox.TabStop = false;
            // 
            // _portTextBox
            // 
            this._portTextBox.AlwaysShowFocusCues = true;
            this._portTextBox.FlatAppearance = true;
            this._portTextBox.Location = new System.Drawing.Point(116, 116);
            this._portTextBox.Name = "_portTextBox";
            this._portTextBox.ReadOnly = true;
            this._portTextBox.Size = new System.Drawing.Size(60, 20);
            this._portTextBox.TabIndex = 5;
            this._portTextBox.TabStop = false;
            // 
            // _portLabel
            // 
            this._portLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._portLabel.Location = new System.Drawing.Point(12, 118);
            this._portLabel.Name = "_portLabel";
            this._portLabel.Size = new System.Drawing.Size(100, 16);
            this._portLabel.TabIndex = 4;
            this._portLabel.Text = "Port:";
            // 
            // _restartButton
            // 
            this._restartButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._restartButton.Location = new System.Drawing.Point(252, 168);
            this._restartButton.Name = "_restartButton";
            this._restartButton.Size = new System.Drawing.Size(75, 23);
            this._restartButton.TabIndex = 8;
            this._restartButton.Text = "Restart";
            this._restartButton.Click += new System.EventHandler(this.OnClickRestartButton);
            // 
            // _hyperlinkLabel
            // 
            this._hyperlinkLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._hyperlinkLabel.Location = new System.Drawing.Point(12, 144);
            this._hyperlinkLabel.Name = "_hyperlinkLabel";
            this._hyperlinkLabel.Size = new System.Drawing.Size(100, 16);
            this._hyperlinkLabel.TabIndex = 6;
            this._hyperlinkLabel.Text = "Root &URL:";
            // 
            // _hyperlinkLinkLabel
            // 
            this._hyperlinkLinkLabel.Location = new System.Drawing.Point(116, 144);
            this._hyperlinkLinkLabel.Name = "_hyperlinkLinkLabel";
            this._hyperlinkLinkLabel.Size = new System.Drawing.Size(296, 16);
            this._hyperlinkLinkLabel.TabIndex = 7;
            this._hyperlinkLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClickedHyperlinkLinkLabel);
            // 
            // _physicalPathLabel
            // 
            this._physicalPathLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._physicalPathLabel.Location = new System.Drawing.Point(12, 70);
            this._physicalPathLabel.Name = "_physicalPathLabel";
            this._physicalPathLabel.Size = new System.Drawing.Size(100, 16);
            this._physicalPathLabel.TabIndex = 0;
            this._physicalPathLabel.Text = "Physical Path:";
            // 
            // _appRootLabel
            // 
            this._appRootLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._appRootLabel.Location = new System.Drawing.Point(12, 94);
            this._appRootLabel.Name = "_appRootLabel";
            this._appRootLabel.Size = new System.Drawing.Size(100, 16);
            this._appRootLabel.TabIndex = 2;
            this._appRootLabel.Text = "Virtual Path:";
            // 
            // _stopButton
            // 
            this._stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._stopButton.Location = new System.Drawing.Point(336, 168);
            this._stopButton.Name = "_stopButton";
            this._stopButton.Size = new System.Drawing.Size(75, 23);
            this._stopButton.TabIndex = 9;
            this._stopButton.Text = "Stop";
            this._stopButton.Click += new System.EventHandler(this.OnClickStopButton);
            // 
            // _physicalPathTextBox
            // 
            this._physicalPathTextBox.AlwaysShowFocusCues = true;
            this._physicalPathTextBox.FlatAppearance = true;
            this._physicalPathTextBox.Location = new System.Drawing.Point(116, 68);
            this._physicalPathTextBox.Name = "_physicalPathTextBox";
            this._physicalPathTextBox.ReadOnly = true;
            this._physicalPathTextBox.Size = new System.Drawing.Size(296, 20);
            this._physicalPathTextBox.TabIndex = 1;
            this._physicalPathTextBox.TabStop = false;
            // 
            // _trayIcon
            // 
            this._trayIcon.Owner = this;
            this._trayIcon.DoubleClick += new System.EventHandler(this.OnDoubleClickTrayIcon);
            this._trayIcon.ShowContextMenu += new Microsoft.Matrix.UIComponents.ShowContextMenuEventHandler(this.OnTrayIconShowContextMenu);
            // 
            // WebServerForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(422, 199);
            this.Controls.Add(this._restartButton);
            this.Controls.Add(this._stopButton);
            this.Controls.Add(this._hyperlinkLinkLabel);
            this.Controls.Add(this._portTextBox);
            this.Controls.Add(this._appRootTextBox);
            this.Controls.Add(this._physicalPathTextBox);
            this.Controls.Add(this._hyperlinkLabel);
            this.Controls.Add(this._portLabel);
            this.Controls.Add(this._appRootLabel);
            this.Controls.Add(this._physicalPathLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WebServerForm";
            this.ShowInTaskbar = false;
            this.TaskBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TaskCaption = "Microsoft ASP.NET Web Matrix Server";
            this.TaskDescription = "Run ASP.NET applications locally.";
            this.Text = "Microsoft ASP.NET Web Matrix Server";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void OnClickRestartButton(object sender, EventArgs e)
        {
            this.DoRestart();
        }

        private void OnClickStopButton(object sender, EventArgs e)
        {
            this.DoStop();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!this._exitMode)
            {
                e.Cancel = true;
                base.WindowState = FormWindowState.Minimized;
                base.Hide();
                this._trayIcon.ShowBalloon("Microsoft ASP.NET Web Matrix Server", this._server.RootUrl, 10);
            }
            else
            {
                base.OnClosing(e);
                if (!e.Cancel)
                {
                    this._trayIcon.Dispose();
                    this._server.Stop();
                }
            }
        }

        private void OnCommandLaunch(object sender, EventArgs e)
        {
            this.DoLaunch();
        }

        private void OnCommandRestart(object sender, EventArgs e)
        {
            this.DoRestart();
        }

        private void OnCommandShow(object sender, EventArgs e)
        {
            this.DoShow();
        }

        private void OnCommandStop(object sender, EventArgs e)
        {
            this.DoStop();
        }

        private void OnDoubleClickTrayIcon(object sender, EventArgs e)
        {
            this.DoShow();
        }

        private void OnLinkClickedHyperlinkLinkLabel(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.DoLaunch();
        }

        private void OnTrayIconShowContextMenu(object sender, ShowContextMenuEventArgs e)
        {
            if (this._trayMenu == null)
            {
                MxMenuItem item = new MxMenuItem("Show Details", string.Empty, new EventHandler(this.OnCommandShow));
                MxMenuItem item2 = new MxMenuItem("Restart", string.Empty, new EventHandler(this.OnCommandRestart));
                MxMenuItem item3 = new MxMenuItem("Stop", string.Empty, new EventHandler(this.OnCommandStop));
                MxMenuItem item4 = new MxMenuItem("Open in Web Browser", string.Empty, new EventHandler(this.OnCommandLaunch));
                this._trayMenu = new MxContextMenu(new MenuItem[] { item4, new MxMenuItem("-"), item2, item3, new MxMenuItem("-"), item });
            }
            this._trayMenu.Show(this, e.Location.X, e.Location.Y);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x11)
            {
                this._exitMode = true;
            }
            base.WndProc(ref m);
        }
    }
}

