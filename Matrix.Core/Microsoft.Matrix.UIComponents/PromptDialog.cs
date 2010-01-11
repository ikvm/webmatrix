namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class PromptDialog : MxForm
    {
        private MxButton _cancelButton;
        private MxLabel _entryLabel;
        private MxTextBox _entryTextBox;
        private PictureBox _iconPictureBox;
        private string _initialValue;
        private MxButton _okButton;
        private bool _requiresNewValue;

        public PromptDialog(IServiceProvider provider) : base(provider)
        {
            this.InitializeUserInterface();
        }

        private void InitializeUserInterface()
        {
            this._iconPictureBox = new PictureBox();
            this._entryLabel = new MxLabel();
            this._entryTextBox = new MxTextBox();
            this._okButton = new MxButton();
            this._cancelButton = new MxButton();
            this._iconPictureBox.Location = new Point(12, 12);
            this._iconPictureBox.Name = "iconPictureBox";
            this._iconPictureBox.Size = new Size(0x20, 0x20);
            this._iconPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            this._iconPictureBox.Image = SystemIcons.Question.ToBitmap();
            this._iconPictureBox.TabIndex = 0;
            this._iconPictureBox.TabStop = false;
            this._entryLabel.Location = new Point(0x38, 8);
            this._entryLabel.Name = "_entryLabel";
            this._entryLabel.Size = new Size(0x160, 0x20);
            this._entryLabel.TabIndex = 1;
            this._entryLabel.Text = "[Entry Name]:";
            this._entryTextBox.AlwaysShowFocusCues = true;
            this._entryTextBox.FlatAppearance = true;
            this._entryTextBox.Location = new Point(0x38, 0x2c);
            this._entryTextBox.Name = "_entryTextBox";
            this._entryTextBox.Size = new Size(0x160, 20);
            this._entryTextBox.TabIndex = 1;
            this._entryTextBox.Text = "";
            this._entryTextBox.TextChanged += new EventHandler(this.OnTextChangedEntryText);
            this._okButton.Enabled = false;
            this._okButton.Location = new Point(0xf8, 0x4c);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 2;
            this._okButton.Text = "OK";
            this._okButton.Click += new EventHandler(this.OnClickOKButton);
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.Location = new Point(0x14c, 0x4c);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 2;
            this._cancelButton.Text = "Cancel";
            base.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(420, 0x6d);
            base.Controls.AddRange(new Control[] { this._cancelButton, this._okButton, this._entryTextBox, this._entryLabel, this._iconPictureBox });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = null;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Enter Information";
        }

        private void OnClickOKButton(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void OnTextChangedEntryText(object sender, EventArgs e)
        {
            string strB = this._entryTextBox.Text.Trim();
            bool flag = strB.Length != 0;
            if ((flag && this._requiresNewValue) && (string.Compare(this._initialValue, strB, true) == 0))
            {
                flag = false;
            }
            this._okButton.Enabled = flag;
        }

        public string EntryText
        {
            get
            {
                return this._entryLabel.Text;
            }
            set
            {
                this._initialValue = value;
                this._entryLabel.Text = value;
            }
        }

        public string EntryValue
        {
            get
            {
                return this._entryTextBox.Text;
            }
            set
            {
                this._entryTextBox.Text = value;
            }
        }

        public bool RequiresNewValue
        {
            get
            {
                return this._requiresNewValue;
            }
            set
            {
                this._requiresNewValue = value;
            }
        }
    }
}

