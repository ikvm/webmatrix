namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Packages.Web.Utility;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    internal sealed class QuickTagEditDialog : MxForm
    {
        private MxButton _cancelButton;
        private Panel _containerPanel;
        private MxButton _okButton;
        private TextControl _textControl;

        public QuickTagEditDialog(IServiceProvider provider) : base(provider)
        {
            this.InitializeComponent();
            this._textControl = new TextControl();
            ((ISupportInitialize) this._textControl).BeginInit();
            this._textControl.MarginWidth = 0;
            this._textControl.LineNumbersVisible = false;
            this._textControl.Dock = DockStyle.Fill;
            IDocumentDesignerHost service = (IDocumentDesignerHost) base.ServiceProvider.GetService(typeof(IDocumentDesignerHost));
            Document document = service.Document;
            this._textControl.Site = new TextControlSite(base.ServiceProvider, document.Language as ITextLanguage);
            ((ISupportInitialize) this._textControl).EndInit();
            this._containerPanel.Controls.Add(this._textControl);
        }

        private void InitializeComponent()
        {
            this._containerPanel = new Panel();
            this._okButton = new MxButton();
            this._cancelButton = new MxButton();
            base.SuspendLayout();
            this._containerPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this._containerPanel.BackColor = SystemColors.ControlDark;
            this._containerPanel.DockPadding.All = 1;
            this._containerPanel.Location = new Point(8, 8);
            this._containerPanel.Name = "_containerPanel";
            this._containerPanel.Size = new Size(0x23c, 0xf8);
            this._containerPanel.TabIndex = 0;
            this._okButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._okButton.Location = new Point(0x1ac, 260);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 1;
            this._okButton.Text = "&OK";
            this._okButton.Click += new EventHandler(this.OnOkButtonClicked);
            this._cancelButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.Location = new Point(0x1fc, 260);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 2;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.Click += new EventHandler(this.OnCancelButtonClicked);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(0x24c, 290);
            base.Controls.AddRange(new Control[] { this._cancelButton, this._okButton, this._containerPanel });
            base.FormBorderStyle = FormBorderStyle.Sizable;
            base.Icon = new Icon(typeof(QuickTagEditDialog), "QuickTagEditDialog.ico");
            base.MaximizeBox = false;
            base.MinimumSize = new Size(400, 200);
            base.MinimizeBox = false;
            base.Name = "QuickTagEditDialog";
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Quick Tag Edit";
            base.ResumeLayout(false);
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void OnOkButtonClicked(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        public string TagText
        {
            get
            {
                return this._textControl.Text;
            }
            set
            {
                HtmlFormatter formatter = new HtmlFormatter();
                StringWriter output = new StringWriter();
                formatter.Format(value, output, new HtmlFormatterOptions(' ', 2, this._textControl.ActiveView.VisibleColumns, HtmlFormatterCase.LowerCase, HtmlFormatterCase.PreserveCase, true));
                this._textControl.Text = output.ToString();
            }
        }
    }
}

