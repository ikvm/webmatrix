namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class InsertHyperlinkDialog : MxForm
    {
        private MxButton _cancelButton;
        private MxLabel _descriptionLabel;
        private MxTextBox _descriptionTextBox;
        private MxButton _okButton;
        private MxLabel _urlLabel;
        private MxTextBox _urlTextBox;

        public InsertHyperlinkDialog(IServiceProvider provider) : this(null, provider)
        {
        }

        public InsertHyperlinkDialog(string initialDescription, IServiceProvider provider) : base(provider)
        {
            this.InitializeComponent();
            if (initialDescription != null)
            {
                this._descriptionTextBox.Text = initialDescription;
                this._descriptionTextBox.ReadOnly = true;
            }
            this._urlTextBox.Focus();
            base.Icon = null;
        }

        private void InitializeComponent()
        {
            this._okButton = new MxButton();
            this._urlLabel = new MxLabel();
            this._urlTextBox = new MxTextBox();
            this._cancelButton = new MxButton();
            this._descriptionTextBox = new MxTextBox();
            this._descriptionLabel = new MxLabel();
            base.SuspendLayout();
            this._okButton.Location = new Point(0xf4, 60);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 4;
            this._okButton.Text = "OK";
            this._okButton.Click += new EventHandler(this.OnOKButtonClick);
            this._urlLabel.Location = new Point(8, 0x24);
            this._urlLabel.Name = "_urlLabel";
            this._urlLabel.Size = new Size(0x24, 15);
            this._urlLabel.TabIndex = 2;
            this._urlLabel.Text = "&URL:";
            this._urlTextBox.AlwaysShowFocusCues = true;
            this._urlTextBox.FlatAppearance = true;
            this._urlTextBox.Location = new Point(0x54, 0x20);
            this._urlTextBox.Name = "_urlTextBox";
            this._urlTextBox.Size = new Size(0x13c, 20);
            this._urlTextBox.TabIndex = 3;
            this._urlTextBox.Text = "";
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.Location = new Point(0x144, 60);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 5;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.Click += new EventHandler(this.OnCancelButtonClick);
            this._descriptionTextBox.AlwaysShowFocusCues = true;
            this._descriptionTextBox.FlatAppearance = true;
            this._descriptionTextBox.Location = new Point(0x54, 8);
            this._descriptionTextBox.Name = "_descriptionTextBox";
            this._descriptionTextBox.Size = new Size(0x13c, 20);
            this._descriptionTextBox.TabIndex = 1;
            this._descriptionTextBox.Text = "";
            this._descriptionLabel.Location = new Point(8, 12);
            this._descriptionLabel.Name = "_descriptionLabel";
            this._descriptionLabel.Size = new Size(0x41, 0x10);
            this._descriptionLabel.TabIndex = 0;
            this._descriptionLabel.Text = "&Description:";
            base.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(0x194, 0x58);
            base.Controls.AddRange(new Control[] { this._cancelButton, this._okButton, this._descriptionTextBox, this._urlTextBox, this._descriptionLabel, this._urlLabel });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "InsertHyperlinkDialog";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Insert Hyperlink";
            base.ResumeLayout(false);
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void OnOKButtonClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        public string Description
        {
            get
            {
                return this._descriptionTextBox.Text;
            }
        }

        public string Url
        {
            get
            {
                return this._urlTextBox.Text;
            }
        }
    }
}

