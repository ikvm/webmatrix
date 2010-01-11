namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class SetDefaultDialog : MxForm
    {
        private Button _cancelButton;
        private string _columnName;
        private string _defaultValue;
        private MxTextBox _defaultValueTextBox;
        private Label _infoLabel;
        private Label _nameLabel;
        private Button _okButton;

        public SetDefaultDialog(IServiceProvider provider) : base(provider)
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this._nameLabel = new Label();
            this._cancelButton = new Button();
            this._okButton = new Button();
            this._defaultValueTextBox = new MxTextBox();
            this._infoLabel = new Label();
            base.SuspendLayout();
            this._nameLabel.Location = new Point(8, 8);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Size = new Size(0x128, 0x10);
            this._nameLabel.TabIndex = 0;
            this._cancelButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.FlatStyle = FlatStyle.System;
            this._cancelButton.Location = new Point(0xe8, 80);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.Click += new EventHandler(this.OnCancelButtonClick);
            this._okButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._okButton.FlatStyle = FlatStyle.System;
            this._okButton.Location = new Point(0x98, 80);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 2;
            this._okButton.Text = "OK";
            this._okButton.Click += new EventHandler(this.OnOkButtonClick);
            this._defaultValueTextBox.AlwaysShowFocusCues = true;
            this._defaultValueTextBox.FlatAppearance = true;
            this._defaultValueTextBox.Location = new Point(8, 0x1c);
            this._defaultValueTextBox.Name = "_defaultValueTextBox";
            this._defaultValueTextBox.Size = new Size(0x128, 20);
            this._defaultValueTextBox.TabIndex = 1;
            this._defaultValueTextBox.Text = "";
            this._infoLabel.Location = new Point(8, 0x34);
            this._infoLabel.Name = "_infoLabel";
            this._infoLabel.Size = new Size(0x128, 0x10);
            this._infoLabel.TabIndex = 4;
            this._infoLabel.Text = "Note: You must put single quotes around string values.";
            base.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(0x13a, 0x6c);
            base.Controls.AddRange(new Control[] { this._infoLabel, this._defaultValueTextBox, this._cancelButton, this._okButton, this._nameLabel });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SetDefaultDialog";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Set Value";
            base.ResumeLayout(false);
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            if (this.DefaultValue != null)
            {
                this._defaultValueTextBox.Text = this.DefaultValue;
            }
            this._nameLabel.Text = "Value for " + this.ColumnName + ":";
        }

        private void OnOkButtonClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            this.DefaultValue = this._defaultValueTextBox.Text;
            base.Close();
        }

        public string ColumnName
        {
            get
            {
                return this._columnName;
            }
            set
            {
                this._columnName = value;
            }
        }

        public string DefaultValue
        {
            get
            {
                return this._defaultValue;
            }
            set
            {
                this._defaultValue = value;
            }
        }
    }
}

