namespace Microsoft.Matrix.Plugins.CodeWizards.Web
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Web.Mail;
    using System.Windows.Forms;

    internal sealed class MailMessageWizardPanel : WizardPanel
    {
        private MxButton _cancelButton;
        private MxLabel _formatLabel;
        private MxLabel _fromLabel;
        private MxTextBox _fromTextBox;
        private MxRadioButton _htmlFormatRadioButton;
        private MxButton _okButton;
        private MxLabel _smtpServerLabel;
        private MxTextBox _smtpServerTextBox;
        private MxLabel _subjectLabel;
        private MxTextBox _subjectTextBox;
        private MxRadioButton _textFormatRadioButton;
        private MxLabel _toLabel;
        private MxTextBox _toTextBox;

        public MailMessageWizardPanel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this._toLabel = new MxLabel();
            this._fromLabel = new MxLabel();
            this._subjectLabel = new MxLabel();
            this._toTextBox = new MxTextBox();
            this._subjectTextBox = new MxTextBox();
            this._fromTextBox = new MxTextBox();
            this._okButton = new MxButton();
            this._cancelButton = new MxButton();
            this._smtpServerLabel = new MxLabel();
            this._smtpServerTextBox = new MxTextBox();
            this._formatLabel = new MxLabel();
            this._textFormatRadioButton = new MxRadioButton();
            this._htmlFormatRadioButton = new MxRadioButton();
            base.SuspendLayout();
            this._toLabel.FlatStyle = FlatStyle.System;
            this._toLabel.Location = new Point(8, 0x10);
            this._toLabel.Name = "_toLabel";
            this._toLabel.Size = new Size(0x1c, 0x10);
            this._toLabel.TabIndex = 0;
            this._toLabel.Text = "&To:";
            this._fromLabel.FlatStyle = FlatStyle.System;
            this._fromLabel.Location = new Point(8, 40);
            this._fromLabel.Name = "_fromLabel";
            this._fromLabel.Size = new Size(40, 0x10);
            this._fromLabel.TabIndex = 1;
            this._fromLabel.Text = "&From:";
            this._subjectLabel.FlatStyle = FlatStyle.System;
            this._subjectLabel.Location = new Point(8, 0x40);
            this._subjectLabel.Name = "_subjectLabel";
            this._subjectLabel.Size = new Size(0x30, 0x10);
            this._subjectLabel.TabIndex = 4;
            this._subjectLabel.Text = "&Subject:";
            this._toTextBox.AlwaysShowFocusCues = true;
            this._toTextBox.FlatAppearance = true;
            this._toTextBox.Location = new Point(0x60, 8);
            this._toTextBox.Name = "_toTextBox";
            this._toTextBox.Size = new Size(0x160, 20);
            this._toTextBox.TabIndex = 1;
            this._toTextBox.Text = "someone@example.com";
            this._subjectTextBox.AlwaysShowFocusCues = true;
            this._subjectTextBox.FlatAppearance = true;
            this._subjectTextBox.Location = new Point(0x60, 0x38);
            this._subjectTextBox.Name = "_subjectTextBox";
            this._subjectTextBox.Size = new Size(0x160, 20);
            this._subjectTextBox.TabIndex = 5;
            this._subjectTextBox.Text = "Email Subject";
            this._fromTextBox.AlwaysShowFocusCues = true;
            this._fromTextBox.FlatAppearance = true;
            this._fromTextBox.Location = new Point(0x60, 0x20);
            this._fromTextBox.Name = "_fromTextBox";
            this._fromTextBox.Size = new Size(0x160, 20);
            this._fromTextBox.TabIndex = 2;
            this._fromTextBox.Text = "someone@example.com";
            this._okButton.FlatStyle = FlatStyle.System;
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 0;
            this._cancelButton.FlatStyle = FlatStyle.System;
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 0;
            this._smtpServerLabel.FlatStyle = FlatStyle.System;
            this._smtpServerLabel.Location = new Point(8, 120);
            this._smtpServerLabel.Name = "_smtpServerLabel";
            this._smtpServerLabel.Size = new Size(80, 0x10);
            this._smtpServerLabel.TabIndex = 9;
            this._smtpServerLabel.Text = "S&MTP Server:";
            this._smtpServerTextBox.AlwaysShowFocusCues = true;
            this._smtpServerTextBox.FlatAppearance = true;
            this._smtpServerTextBox.Location = new Point(0x60, 120);
            this._smtpServerTextBox.Name = "_smtpServerTextBox";
            this._smtpServerTextBox.Size = new Size(0x160, 20);
            this._smtpServerTextBox.TabIndex = 10;
            this._smtpServerTextBox.Text = "localhost";
            this._formatLabel.FlatStyle = FlatStyle.System;
            this._formatLabel.Location = new Point(8, 0x58);
            this._formatLabel.Name = "_formatLabel";
            this._formatLabel.Size = new Size(0x4c, 0x10);
            this._formatLabel.TabIndex = 6;
            this._formatLabel.Text = "Mail Format:";
            this._textFormatRadioButton.Checked = true;
            this._textFormatRadioButton.FlatStyle = FlatStyle.System;
            this._textFormatRadioButton.Location = new Point(0x60, 0x58);
            this._textFormatRadioButton.Name = "_textFormatRadioButton";
            this._textFormatRadioButton.Size = new Size(0x48, 0x10);
            this._textFormatRadioButton.TabIndex = 7;
            this._textFormatRadioButton.TabStop = true;
            this._textFormatRadioButton.Text = "&Plain Text";
            this._htmlFormatRadioButton.FlatStyle = FlatStyle.System;
            this._htmlFormatRadioButton.Location = new Point(0xb0, 0x58);
            this._htmlFormatRadioButton.Name = "_htmlFormatRadioButton";
            this._htmlFormatRadioButton.Size = new Size(0x38, 0x10);
            this._htmlFormatRadioButton.TabIndex = 8;
            this._htmlFormatRadioButton.Text = "&HTML";
            base.Caption = "Send Email Message";
            base.Description = "Generate code to send email using the System.Web.Mail classes.";
            base.Controls.AddRange(new Control[] { this._htmlFormatRadioButton, this._textFormatRadioButton, this._formatLabel, this._smtpServerTextBox, this._smtpServerLabel, this._fromTextBox, this._subjectTextBox, this._toTextBox, this._subjectLabel, this._fromLabel, this._toLabel });
            base.Name = "MailMessageWizardPanel";
            base.Size = new Size(0x1c8, 0x98);
            base.ResumeLayout(false);
        }

        public override bool FinishEnabled
        {
            get
            {
                return true;
            }
        }

        public string FromText
        {
            get
            {
                return this._fromTextBox.Text.Trim();
            }
        }

        public System.Web.Mail.MailFormat MailFormat
        {
            get
            {
                if (!this._htmlFormatRadioButton.Checked)
                {
                    return System.Web.Mail.MailFormat.Text;
                }
                return System.Web.Mail.MailFormat.Html;
            }
        }

        public string SmtpServerText
        {
            get
            {
                return this._smtpServerTextBox.Text.Trim();
            }
        }

        public string SubjectText
        {
            get
            {
                return this._subjectTextBox.Text.Trim();
            }
        }

        public string ToText
        {
            get
            {
                return this._toTextBox.Text.Trim();
            }
        }
    }
}

