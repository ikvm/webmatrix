namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class ThreadExceptionDialog : MxForm
    {
        private MxButton _detailsButton;
        private MxTextBox _detailsTextBox;
        private bool _detailsVisible;
        private Exception _exception;
        private MxLabel _exceptionLabel;
        private MxButton _sendFeedbackButton;

        public ThreadExceptionDialog(IServiceProvider serviceProvider, Exception e) : base(serviceProvider)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            this.InitializeComponent();
            IApplicationIdentity service = null;
            if (serviceProvider != null)
            {
                service = (IApplicationIdentity) this.GetService(typeof(IApplicationIdentity));
            }
            if (service != null)
            {
                this.Text = service.Title;
            }
            else
            {
                this._sendFeedbackButton.Visible = false;
            }
            this._exception = e;
            string message = e.Message;
            if ((message == null) || (message.Length == 0))
            {
                message = "No exception message available";
            }
            this._exceptionLabel.Text = message;
        }

        private void InitializeComponent()
        {
            PictureBox box = new PictureBox();
            MxLabel label = new MxLabel();
            MxButton button = new MxButton();
            MxButton button2 = new MxButton();
            this._exceptionLabel = new MxLabel();
            this._sendFeedbackButton = new MxButton();
            this._detailsButton = new MxButton();
            this._detailsTextBox = new MxTextBox();
            base.SuspendLayout();
            box.Location = new Point(4, 8);
            box.Size = new Size(0x40, 0x40);
            box.SizeMode = PictureBoxSizeMode.CenterImage;
            box.TabIndex = 0;
            box.TabStop = false;
            box.Image = SystemIcons.Error.ToBitmap();
            label.Location = new Point(0x48, 8);
            label.Size = new Size(0x170, 0x60);
            label.TabIndex = 1;
            label.Text = "An unhandled exception has occurred in the application.\nClick \"Send Feedback\" to submit a bug report with exception details.\nClick \"Continue\" to ignore the error and attempt to continue running the application.\nClick \"Quit\" to end the application immediately.";
            this._exceptionLabel.Location = new Point(0x48, 0x70);
            this._exceptionLabel.Size = new Size(0x170, 0x2c);
            this._exceptionLabel.TabIndex = 2;
            this._exceptionLabel.Text = "[Exception Message]";
            this._sendFeedbackButton.Location = new Point(8, 0xa8);
            this._sendFeedbackButton.Size = new Size(0x68, 0x17);
            this._sendFeedbackButton.TabIndex = 3;
            this._sendFeedbackButton.Text = "Send Feedback...";
            this._sendFeedbackButton.Click += new EventHandler(this.OnClickSendFeedbackButton);
            button.DialogResult = DialogResult.OK;
            button.Location = new Point(0xd0, 0xa8);
            button.TabIndex = 4;
            button.Text = "Continue";
            button.Click += new EventHandler(this.OnClickContinueButton);
            button2.Location = new Point(0x120, 0xa8);
            button2.TabIndex = 5;
            button2.Text = "Quit";
            button2.Click += new EventHandler(this.OnClickQuitButton);
            this._detailsButton.Location = new Point(0x170, 0xa8);
            this._detailsButton.TabIndex = 6;
            this._detailsButton.Text = "Details >>";
            this._detailsButton.Click += new EventHandler(this.OnClickDetailsButton);
            this._detailsTextBox.Location = new Point(4, 200);
            this._detailsTextBox.Multiline = true;
            this._detailsTextBox.ReadOnly = true;
            this._detailsTextBox.ScrollBars = ScrollBars.Both;
            this._detailsTextBox.Size = new Size(440, 0xd0);
            this._detailsTextBox.TabIndex = 7;
            this._detailsTextBox.TabStop = false;
            this._detailsTextBox.Text = "";
            this._detailsTextBox.WordWrap = false;
            base.AcceptButton = button;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1c0, 0xc7);
            base.Controls.AddRange(new Control[] { this._detailsTextBox, this._detailsButton, button2, button, this._sendFeedbackButton, this._exceptionLabel, label, box });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Unhandled Exception";
            base.Icon = null;
            base.ResumeLayout(false);
        }

        private void OnClickContinueButton(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Ignore;
            base.Close();
        }

        private void OnClickDetailsButton(object sender, EventArgs e)
        {
            if (this._detailsVisible)
            {
                base.ClientSize = new Size(base.ClientSize.Width, 0xc7);
                this._detailsButton.Text = "Details >>";
            }
            else
            {
                base.ClientSize = new Size(base.ClientSize.Width, 0x19d);
                this._detailsButton.Text = "Details <<";
                if (this._detailsTextBox.Text.Length == 0)
                {
                    this._detailsTextBox.Text = this._exception.ToString();
                }
            }
            this._detailsVisible = !this._detailsVisible;
        }

        private void OnClickQuitButton(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Abort;
            base.Close();
        }

        private void OnClickSendFeedbackButton(object sender, EventArgs e)
        {
            new ApplicationInfoDialog(base.ServiceProvider, this._exception).ShowDialog(this);
        }
    }
}

