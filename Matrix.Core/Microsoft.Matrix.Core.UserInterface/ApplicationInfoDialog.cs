namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;

    public sealed class ApplicationInfoDialog : MxForm
    {
        private ApplicationInformation _appInfo;
        private ArrayList _assemblies;
        private MxListView _assemblyListView;
        private Exception _exception;
        private string _feedbackMessage;
        private PropertyGrid _infoGrid;
        private MxTextBox _messageSubjectText;
        private MxTextBox _messageText;
        private bool _sendFeedbackMode;
        private MxTextBox _userEmailText;

        public ApplicationInfoDialog(IServiceProvider serviceProvider) : this(serviceProvider, false)
        {
        }

        public ApplicationInfoDialog(IServiceProvider serviceProvider, bool sendFeedbackMode) : base(serviceProvider)
        {
            this._sendFeedbackMode = sendFeedbackMode;
            if (this._sendFeedbackMode)
            {
                this._feedbackMessage = "[Enter your feedback and comments here]";
            }
            this.InitializeComponent();
        }

        public ApplicationInfoDialog(IServiceProvider serviceProvider, Exception e) : base(serviceProvider)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            this._sendFeedbackMode = true;
            this._exception = e;
            this._feedbackMessage = "[Enter any information relevant to reproducing the error]";
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Microsoft.Matrix.UIComponents.TabControl control = new Microsoft.Matrix.UIComponents.TabControl();
            Microsoft.Matrix.UIComponents.TabPage page = new Microsoft.Matrix.UIComponents.TabPage();
            Microsoft.Matrix.UIComponents.TabPage page2 = new Microsoft.Matrix.UIComponents.TabPage();
            Microsoft.Matrix.UIComponents.TabPage page3 = null;
            MxButton button = null;
            MxButton button2 = new MxButton();
            this._infoGrid = new PropertyGrid();
            this._assemblyListView = new MxListView();
            control.SetBounds(4, 4, 0x1ec, 0x116);
            control.Mode = TabControlMode.TextOnly;
            control.TabPlacement = TabPlacement.Top;
            control.TabIndex = 0;
            control.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            if (this._sendFeedbackMode)
            {
                MxLabel label = new MxLabel();
                MxLabel label2 = new MxLabel();
                MxLabel label3 = new MxLabel();
                MxLabel label4 = new MxLabel();
                label.Text = "&Subject:";
                label.TabStop = false;
                label.TabIndex = 0;
                label.SetBounds(4, 6, 100, 0x10);
                this._messageSubjectText = new MxTextBox();
                this._messageSubjectText.TabIndex = 1;
                this._messageSubjectText.FlatAppearance = true;
                this._messageSubjectText.AlwaysShowFocusCues = true;
                this._messageSubjectText.SetBounds(0x6c, 4, 260, 0x15);
                label3.TabIndex = 2;
                label3.Text = "(Optional)";
                label3.SetBounds(0x174, 6, 100, 0x10);
                if (this._exception == null)
                {
                    label3.Visible = false;
                }
                label2.Text = "&Email Address:";
                label2.TabStop = false;
                label2.TabIndex = 3;
                label2.SetBounds(4, 0x1f, 100, 0x10);
                this._userEmailText = new MxTextBox();
                this._userEmailText.TabIndex = 4;
                this._userEmailText.FlatAppearance = true;
                this._userEmailText.AlwaysShowFocusCues = true;
                this._userEmailText.SetBounds(0x6c, 0x1d, 260, 0x15);
                label4.TabIndex = 2;
                label4.Text = "(Optional)";
                label4.SetBounds(0x174, 0x1f, 100, 0x10);
                Panel panel = new Panel();
                panel.TabIndex = 0;
                panel.Height = 0x36;
                panel.Dock = DockStyle.Top;
                panel.BackColor = SystemColors.Control;
                panel.Controls.AddRange(new Control[] { label, this._messageSubjectText, label3, label2, this._userEmailText, label4 });
                this._messageText = new MxTextBox();
                this._messageText.Dock = DockStyle.Fill;
                this._messageText.Multiline = true;
                this._messageText.AcceptsReturn = true;
                this._messageText.AcceptsTab = true;
                this._messageText.WordWrap = true;
                this._messageText.TabIndex = 0;
                this._messageText.Text = this._feedbackMessage;
                this._messageText.BorderStyle = BorderStyle.None;
                Panel panel2 = new Panel();
                panel2.TabIndex = 1;
                panel2.DockPadding.All = 1;
                panel2.BackColor = SystemColors.ControlDark;
                panel2.Dock = DockStyle.Fill;
                panel2.Controls.Add(this._messageText);
                page3 = new Microsoft.Matrix.UIComponents.TabPage();
                page3.TabIndex = 4;
                page3.Text = "Feedback";
                page3.Controls.Add(panel2);
                page3.Controls.Add(panel);
                button = new MxButton();
                button.SetBounds(340, 0x120, 0x4b, 0x17);
                button.TabIndex = 1;
                button.Text = "Send";
                button.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                button.Click += new EventHandler(this.OnClickSendFeedbackButton);
            }
            page.TabIndex = 1;
            page.Text = "Information";
            page2.TabIndex = 2;
            page2.Text = "Loaded Assemblies";
            page2.BackColor = SystemColors.ControlDark;
            page2.DockPadding.All = 1;
            if (this._sendFeedbackMode)
            {
                control.Controls.Add(page3);
            }
            control.Controls.Add(page);
            control.Controls.Add(page2);
            button2.DialogResult = DialogResult.OK;
            button2.SetBounds(0x1a5, 0x120, 0x4b, 0x17);
            button2.TabIndex = 2;
            button2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            if (this._sendFeedbackMode)
            {
                button2.Text = "Close";
            }
            else
            {
                button2.Text = "OK";
            }
            this._infoGrid.ToolbarVisible = false;
            this._infoGrid.Dock = DockStyle.Fill;
            this._infoGrid.TabIndex = 0;
            this._assemblyListView.BorderStyle = BorderStyle.None;
            this._assemblyListView.FullRowSelect = true;
            this._assemblyListView.View = View.Details;
            this._assemblyListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this._assemblyListView.FlatScrollBars = true;
            this._assemblyListView.ShowToolTips = true;
            this._assemblyListView.Dock = DockStyle.Fill;
            this._assemblyListView.TabIndex = 0;
            ColumnHeader header = new ColumnHeader();
            header.Text = "Name";
            header.TextAlign = HorizontalAlignment.Left;
            header.Width = 150;
            ColumnHeader header2 = new ColumnHeader();
            header2.Text = "Version";
            header2.TextAlign = HorizontalAlignment.Left;
            header2.Width = 0x4b;
            ColumnHeader header3 = new ColumnHeader();
            header3.Text = "Location";
            header3.TextAlign = HorizontalAlignment.Left;
            header3.Width = 250;
            this._assemblyListView.Columns.AddRange(new ColumnHeader[] { header, header2, header3 });
            if (this._sendFeedbackMode)
            {
                this.Text = "Send Feedback";
                base.Icon = new Icon(typeof(ApplicationInfoDialog), "Feedback.ico");
            }
            else
            {
                this.Text = "Application Information";
                base.Icon = new Icon(typeof(ApplicationInfoDialog), "ApplicationInfoDialog.ico");
            }
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            base.AcceptButton = button2;
            base.CancelButton = button2;
            base.ClientSize = new Size(500, 0x13d);
            base.MinimumSize = new Size(500, 340);
            page.Controls.Add(this._infoGrid);
            page2.Controls.Add(this._assemblyListView);
            base.Controls.Add(control);
            if (this._sendFeedbackMode)
            {
                base.Controls.Add(button);
            }
            base.Controls.Add(button2);
        }

        private void OnClickSendFeedbackButton(object sender, EventArgs e)
        {
            IApplicationIdentity identity = (IApplicationIdentity) this.GetService(typeof(IApplicationIdentity));
            if (identity != null)
            {
                string url = ConfigurationSettings.AppSettings["IDE.SendFeedbackServiceUrl"];
                if ((url != null) && (url.Length != 0))
                {
                    IMxUIService service = (IMxUIService) this.GetService(typeof(IMxUIService));
                    FeedbackData feedbackData = new FeedbackData();
                    feedbackData.appVersion = this._appInfo.ProductVersion;
                    feedbackData.osInfo = this._appInfo.OperatingSystemVersion;
                    feedbackData.title = this._messageSubjectText.Text.Trim();
                    feedbackData.Text = this._messageText.Text;
                    if (this._exception == null)
                    {
                        if ((feedbackData.title.Length == 0) || (feedbackData.Text.Length == 0))
                        {
                            service.ShowMessage("Please enter in a subject and some details about your feedback.", this.Text, MessageBoxIcon.Exclamation, MessageBoxButtons.OK);
                            return;
                        }
                    }
                    else
                    {
                        feedbackData.isExceptionReport = true;
                        feedbackData.exceptionData = this._exception.Message + "\r\n" + this._exception.StackTrace;
                        if (feedbackData.title.Length == 0)
                        {
                            feedbackData.title = this._exception.Message;
                        }
                    }
                    feedbackData.isExceptionReportSpecified = true;
                    StringBuilder builder = new StringBuilder(this._assemblies.Count * 0x200);
                    foreach (AssemblyName name in this._assemblies)
                    {
                        builder.Append(name.FullName);
                        builder.Append("\r\n");
                    }
                    feedbackData.AssemblyInfo = builder.ToString();
                    new SendFeedbackService(url).BeginSubmitFeedback(identity.Name, this._userEmailText.Text.Trim(), feedbackData, null, null);
                    if (service != null)
                    {
                        service.ShowMessage("Thank you for sending your feedback and comments.\r\nYou will receive confirmation email, if you provided your email address.", this.Text, MessageBoxIcon.Asterisk, MessageBoxButtons.OK);
                    }
                }
            }
            base.Close();
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            this._appInfo = new ApplicationInformation();
            this._infoGrid.SelectedObject = this._appInfo;
            if (this._sendFeedbackMode)
            {
                this._assemblies = new ArrayList();
            }
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string directoryName;
                AssemblyName name = assembly.GetName();
                if (this._assemblies != null)
                {
                    this._assemblies.Add(name);
                }
                Version version = name.Version;
                if (version == null)
                {
                    version = new Version(0, 0, 0, 0);
                }
                if (assembly.GlobalAssemblyCache)
                {
                    directoryName = "Global Assembly Cache";
                }
                else if ((name != null) && (name.CodeBase.Length != 0))
                {
                    directoryName = Path.GetDirectoryName(new Uri(name.CodeBase, true).LocalPath);
                }
                else
                {
                    directoryName = string.Empty;
                }
                ListViewItem item = new ListViewItem(new string[] { name.Name, version.ToString(), directoryName });
                this._assemblyListView.Items.Add(item);
            }
            this._assemblyListView.ListViewItemSorter = new AssemblyListSorter();
            if (this._sendFeedbackMode)
            {
                this._messageText.Text = this._feedbackMessage;
                this._messageText.Select(0, 0);
            }
        }

        private sealed class AssemblyListSorter : IComparer
        {
            public int Compare(object obj1, object obj2)
            {
                ListViewItem item = (ListViewItem) obj1;
                ListViewItem item2 = (ListViewItem) obj2;
                return string.Compare(item.Text, item2.Text, false);
            }
        }
    }
}

