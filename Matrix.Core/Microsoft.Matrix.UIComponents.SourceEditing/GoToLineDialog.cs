namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class GoToLineDialog : MxForm
    {
        private Button cancelButton;
        private MxTextBox lineText;
        private Button okButton;

        public GoToLineDialog(IServiceProvider provider, int initialLine) : base(provider)
        {
            this.InitForm();
            this.LineNumber = initialLine;
        }

        private void InitForm()
        {
            this.lineText = new MxTextBox();
            this.okButton = new Button();
            this.cancelButton = new Button();
            this.lineText.SetBounds(6, 6, 80, 0x15);
            this.lineText.TabIndex = 0;
            this.lineText.MaxLength = 5;
            this.lineText.Numeric = true;
            this.okButton.SetBounds(0x66, 5, 60, 0x16);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.DialogResult = DialogResult.OK;
            this.cancelButton.SetBounds(0x66, 0x1f, 60, 0x16);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.DialogResult = DialogResult.Cancel;
            base.ClientSize = new Size(0xa8, 0x3b);
            base.ShowInTaskbar = false;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            this.Text = "Go To Line";
            base.AcceptButton = this.okButton;
            base.CancelButton = this.cancelButton;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Font = new Font("Tahoma", 8f);
            base.Icon = null;
            base.StartPosition = FormStartPosition.CenterParent;
            base.Controls.Add(this.lineText);
            base.Controls.Add(this.okButton);
            base.Controls.Add(this.cancelButton);
        }

        public int LineNumber
        {
            get
            {
                try
                {
                    return int.Parse(this.lineText.Text.Trim());
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            set
            {
                this.lineText.Text = value.ToString();
            }
        }
    }
}

