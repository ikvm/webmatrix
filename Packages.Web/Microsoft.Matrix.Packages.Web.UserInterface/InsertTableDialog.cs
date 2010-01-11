namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    internal sealed class InsertTableDialog : MxForm
    {
        private MxButton _cancelButton;
        private MxLabel _columnsLabel;
        private MxTextBox _columnsTextBox;
        private MxLabel _heightLabel;
        private MxTextBox _heightTextBox;
        private string _htmlTableString;
        private MxButton _okButton;
        private MxLabel _rowsLabel;
        private MxTextBox _rowTextBox;
        private MxLabel _widthLabel;
        private MxTextBox _widthTextBox;

        public InsertTableDialog(IServiceProvider provider) : base(provider)
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this._okButton = new MxButton();
            this._cancelButton = new MxButton();
            this._rowsLabel = new MxLabel();
            this._columnsLabel = new MxLabel();
            this._widthLabel = new MxLabel();
            this._heightLabel = new MxLabel();
            this._rowTextBox = new MxTextBox();
            this._columnsTextBox = new MxTextBox();
            this._widthTextBox = new MxTextBox();
            this._heightTextBox = new MxTextBox();
            base.SuspendLayout();
            this._okButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._okButton.Location = new Point(0x80, 0x44);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 8;
            this._okButton.Text = "OK";
            this._okButton.Click += new EventHandler(this.OnOKButtonClick);
            this._cancelButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.Location = new Point(0xd0, 0x44);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 9;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.Click += new EventHandler(this.OnCancelButtonClick);
            this._rowsLabel.Location = new Point(8, 0x10);
            this._rowsLabel.Name = "_rowsLabel";
            this._rowsLabel.Size = new Size(0x34, 0x10);
            this._rowsLabel.TabIndex = 0;
            this._rowsLabel.Text = "&Rows:";
            this._columnsLabel.Location = new Point(8, 40);
            this._columnsLabel.Name = "_columnsLabel";
            this._columnsLabel.Size = new Size(0x34, 0x10);
            this._columnsLabel.TabIndex = 4;
            this._columnsLabel.Text = "&Columns:";
            this._widthLabel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._widthLabel.Location = new Point(0xac, 0x10);
            this._widthLabel.Name = "_widthLabel";
            this._widthLabel.Size = new Size(0x24, 0x10);
            this._widthLabel.TabIndex = 2;
            this._widthLabel.Text = "&Width:";
            this._heightLabel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._heightLabel.Location = new Point(0xac, 40);
            this._heightLabel.Name = "_heightLabel";
            this._heightLabel.Size = new Size(0x2f, 0x10);
            this._heightLabel.TabIndex = 6;
            this._heightLabel.Text = "&Height:";
            this._rowTextBox.AlwaysShowFocusCues = true;
            this._rowTextBox.FlatAppearance = true;
            this._rowTextBox.Location = new Point(0x44, 12);
            this._rowTextBox.Name = "_rowTextBox";
            this._rowTextBox.Numeric = true;
            this._rowTextBox.Size = new Size(60, 20);
            this._rowTextBox.TabIndex = 1;
            this._rowTextBox.Text = "3";
            this._columnsTextBox.AlwaysShowFocusCues = true;
            this._columnsTextBox.FlatAppearance = true;
            this._columnsTextBox.Location = new Point(0x44, 0x24);
            this._columnsTextBox.Name = "_columnsTextBox";
            this._columnsTextBox.Numeric = true;
            this._columnsTextBox.Size = new Size(60, 20);
            this._columnsTextBox.TabIndex = 5;
            this._columnsTextBox.Text = "3";
            this._widthTextBox.AlwaysShowFocusCues = true;
            this._widthTextBox.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._widthTextBox.FlatAppearance = true;
            this._widthTextBox.Location = new Point(0xe0, 12);
            this._widthTextBox.Name = "_widthTextBox";
            this._widthTextBox.Numeric = true;
            this._widthTextBox.Size = new Size(60, 20);
            this._widthTextBox.TabIndex = 3;
            this._widthTextBox.Text = "300";
            this._heightTextBox.AlwaysShowFocusCues = true;
            this._heightTextBox.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this._heightTextBox.FlatAppearance = true;
            this._heightTextBox.Location = new Point(0xe0, 0x24);
            this._heightTextBox.Name = "_heightTextBox";
            this._heightTextBox.Numeric = true;
            this._heightTextBox.Size = new Size(60, 20);
            this._heightTextBox.TabIndex = 7;
            this._heightTextBox.Text = "150";
            base.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(290, 100);
            base.Controls.AddRange(new Control[] { this._heightTextBox, this._widthTextBox, this._columnsTextBox, this._rowTextBox, this._heightLabel, this._widthLabel, this._columnsLabel, this._rowsLabel, this._cancelButton, this._okButton });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "InsertTableDialog";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Insert Table";
            base.ResumeLayout(false);
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void OnOKButtonClick(object sender, EventArgs e)
        {
            if ((this._rowTextBox.Text.Length == 0) || (this._columnsTextBox.Text.Length == 0))
            {
                MessageBox.Show(this, "You must specify the number of rows and columns", "Insert Table", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
            else
            {
                int num = -1;
                int num2 = -1;
                try
                {
                    num = int.Parse(this._rowTextBox.Text);
                    num2 = int.Parse(this._columnsTextBox.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show(this, "Invalid number.", "Insert Table", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    return;
                }
                catch (OverflowException)
                {
                    MessageBox.Show(this, "The number is too large.", "Insert Table", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    return;
                }
                if (((num >= 1) && (num <= 0x63)) && ((num2 >= 1) && (num2 <= 0x63)))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("<table width=\"");
                    builder.Append(this._widthTextBox.Text);
                    builder.Append("\" height=\"");
                    builder.Append(this._heightTextBox.Text);
                    builder.Append("\">");
                    for (int i = 0; i < num; i++)
                    {
                        builder.Append("<tr>");
                        for (int j = 0; j < num2; j++)
                        {
                            builder.Append("<td></td>");
                        }
                        builder.Append("</tr>");
                    }
                    builder.Append("</table>");
                    this._htmlTableString = builder.ToString();
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
                else
                {
                    MessageBox.Show(this, "Rows and columns must be between 0 and 100.", "Insert Table", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                }
            }
        }

        public string HtmlTableString
        {
            get
            {
                return this._htmlTableString;
            }
        }
    }
}

