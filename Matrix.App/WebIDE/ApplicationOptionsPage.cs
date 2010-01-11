namespace Microsoft.Matrix.WebIDE
{
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.Projects.FileSystem;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class ApplicationOptionsPage : OptionsPage
    {
        private bool _mruListLengthChanged;
        private Label _mruListLengthLabel;
        private Label _mruListLengthLabel2;
        private MxTextBox _mruListLengthTextBox;
        private bool _startupActionChanged;
        private MxComboBox _startupComboBox;
        private Label _startupLabel;

        internal ApplicationOptionsPage(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
        }

        public override void CommitChanges()
        {
            if (this._mruListLengthChanged)
            {
                bool flag = false;
                int num = -1;
                try
                {
                    num = int.Parse(this._mruListLengthTextBox.Text);
                }
                catch
                {
                    flag = true;
                }
                if (!flag)
                {
                    IProjectManager service = (IProjectManager) this.GetService(typeof(IProjectManager));
                    ILocalFileSystemProject localFileSystemProject = service.LocalFileSystemProject as ILocalFileSystemProject;
                    localFileSystemProject.MruDocumentsLength = num;
                    if (localFileSystemProject.MruDocumentsLength != num)
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    IProjectManager manager2 = (IProjectManager) this.GetService(typeof(IProjectManager));
                    ILocalFileSystemProject project2 = manager2.LocalFileSystemProject as ILocalFileSystemProject;
                    ((IMxUIService) this.GetService(typeof(IMxUIService))).ShowMessage("Recent Files List length specified must be between 1 and 10.  The list length preference has been updated to " + project2.MruDocumentsLength + ".", "Note:", MessageBoxIcon.Asterisk, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                }
            }
            if (this._startupActionChanged)
            {
                IApplicationIdentity identity = (IApplicationIdentity) this.GetService(typeof(IApplicationIdentity));
                int selectedIndex = this._startupComboBox.SelectedIndex;
                if (selectedIndex != -1)
                {
                    identity.SetSetting("startup", Enum.GetName(typeof(StartupAction), (StartupAction) selectedIndex));
                }
            }
        }

        private void InitializeComponent()
        {
            this._startupComboBox = new MxComboBox();
            this._mruListLengthTextBox = new MxTextBox();
            this._startupLabel = new Label();
            this._mruListLengthLabel = new Label();
            this._mruListLengthLabel2 = new Label();
            base.SuspendLayout();
            this._startupComboBox.AlwaysShowFocusCues = true;
            this._startupComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this._startupComboBox.FlatAppearance = true;
            this._startupComboBox.Items.AddRange(new object[] { "Create a new file", "Open an existing file", "Show empty environment" });
            this._startupComboBox.Location = new Point(0x4c, 8);
            this._startupComboBox.Name = "_startupComboBox";
            this._startupComboBox.Size = new Size(0xd4, 0x15);
            this._startupComboBox.TabIndex = 1;
            this._startupComboBox.SelectedIndexChanged += new EventHandler(this.OnStartupComboBoxSelectedIndexChanged);
            this._mruListLengthTextBox.AlwaysShowFocusCues = true;
            this._mruListLengthTextBox.FlatAppearance = true;
            this._mruListLengthTextBox.Location = new Point(0x4c, 40);
            this._mruListLengthTextBox.Name = "_mruListLengthTextBox";
            this._mruListLengthTextBox.Numeric = true;
            this._mruListLengthTextBox.Size = new Size(0x24, 20);
            this._mruListLengthTextBox.TabIndex = 3;
            this._mruListLengthTextBox.Text = "";
            this._mruListLengthTextBox.TextChanged += new EventHandler(this.OnMruListLengthTextBoxTextChanged);
            this._startupLabel.Location = new Point(8, 12);
            this._startupLabel.Name = "_startupLabel";
            this._startupLabel.Size = new Size(60, 0x10);
            this._startupLabel.TabIndex = 0;
            this._startupLabel.Text = "At &Startup:";
            this._mruListLengthLabel.Location = new Point(8, 0x2c);
            this._mruListLengthLabel.Name = "_mruListLengthLabel";
            this._mruListLengthLabel.Size = new Size(0x40, 0x10);
            this._mruListLengthLabel.TabIndex = 2;
            this._mruListLengthLabel.Text = "&Display";
            this._mruListLengthLabel2.Location = new Point(120, 0x2c);
            this._mruListLengthLabel2.Name = "_mruListLengthLabel2";
            this._mruListLengthLabel2.Size = new Size(0xa8, 0x10);
            this._mruListLengthLabel2.TabIndex = 4;
            this._mruListLengthLabel2.Text = "items in the Recent Files menu.";
            base.Controls.AddRange(new Control[] { this._mruListLengthLabel2, this._mruListLengthLabel, this._startupLabel, this._mruListLengthTextBox, this._startupComboBox });
            base.Size = new Size(400, 0xe8);
            base.ResumeLayout(false);
        }

        protected override void OnInitialVisibleChanged(EventArgs e)
        {
            base.OnInitialVisibleChanged(e);
            IProjectManager service = (IProjectManager) this.GetService(typeof(IProjectManager));
            ILocalFileSystemProject localFileSystemProject = service.LocalFileSystemProject as ILocalFileSystemProject;
            this._mruListLengthTextBox.Text = localFileSystemProject.MruDocumentsLength.ToString();
            string setting = ((IApplicationIdentity) this.GetService(typeof(IApplicationIdentity))).GetSetting("startup", false);
            if (((setting == null) || (setting.Length == 0)) || setting.Equals(Enum.GetName(typeof(StartupAction), StartupAction.NewFile)))
            {
                this._startupComboBox.SelectedIndex = 0;
            }
            else if (setting.Equals(Enum.GetName(typeof(StartupAction), StartupAction.OpenFile)))
            {
                this._startupComboBox.SelectedIndex = 1;
            }
            else
            {
                this._startupComboBox.SelectedIndex = 2;
            }
        }

        private void OnMruListLengthTextBoxTextChanged(object sender, EventArgs e)
        {
            this._mruListLengthChanged = true;
        }

        private void OnStartupComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            this._startupActionChanged = true;
        }

        public override bool IsDirty
        {
            get
            {
                if (!this._startupActionChanged)
                {
                    return this._mruListLengthChanged;
                }
                return true;
            }
        }

        public override string OptionsPath
        {
            get
            {
                return @"(General)\Application";
            }
        }
    }
}

