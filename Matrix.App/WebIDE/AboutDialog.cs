namespace Microsoft.Matrix.WebIDE
{
    using Microsoft.Matrix.Core.UserInterface;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class AboutDialog : MxAboutDialog
    {
        private Label _appVersionLabel;
        private Label _clrVersionLabel;

        public AboutDialog()
        {
            this.InitializeComponent();
            this._appVersionLabel.Text = base.ApplicationVersion + " (Technology Preview)";
            this._clrVersionLabel.Text = base.FrameworkVersion;
        }

        private void InitializeComponent()
        {
            this._appVersionLabel = new Label();
            this._clrVersionLabel = new Label();
            Label label = new Label();
            AboutDialogButton button = new AboutDialogButton();
            this._appVersionLabel.SetBounds(0x75, 0xd8, 240, 0x10);
            this._appVersionLabel.TabIndex = 1;
            this._appVersionLabel.TabStop = false;
            this._appVersionLabel.BackColor = Color.Transparent;
            this._clrVersionLabel.SetBounds(0x75, 0xe8, 240, 0x10);
            this._clrVersionLabel.TabIndex = 2;
            this._clrVersionLabel.TabStop = false;
            this._clrVersionLabel.BackColor = Color.Transparent;
            label = new Label();
            label.Text = "This program is licensed under the terms of the installed End-User License Agreement.";
            label.SetBounds(0x75, 0x100, 240, 0x20);
            label.TabIndex = 3;
            label.TabStop = false;
            label.BackColor = Color.Transparent;
            button.SetBounds(0x18b, 0x10c, 0x4b, 0x17);
            button.DialogResult = DialogResult.OK;
            button.TabIndex = 4;
            button.Text = "OK";
            this.Text = "About Microsoft ASP.NET Web Matrix";
            base.ClientSize = new Size(0x1dd, 0x143);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.CancelButton = button;
            base.AcceptButton = button;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Font = new Font("Tahoma", 8f);
            base.Icon = null;
            base.StartPosition = FormStartPosition.CenterParent;
            base.ShowInTaskbar = false;
            this.BackgroundImage = new Bitmap(typeof(AboutDialog), "WebIDE.gif");
            base.Controls.Add(this._appVersionLabel);
            base.Controls.Add(this._clrVersionLabel);
            base.Controls.Add(label);
            base.Controls.Add(button);
        }
    }
}

