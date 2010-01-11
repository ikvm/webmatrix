namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class WebOptionsPage : OptionsPage
    {
        private MxComboBox _defaultViewComboBox;
        private MxLabel _defaultViewLabel;
        private MxRadioButton _designRadioButton;
        private MxLabel _infoLabel;
        private bool _newDesignModeEnabled;
        private DocumentViewType _newWebDefaultView;
        private MxLabel _noteLabel;
        private bool _originalDesignModeEnabled;
        private DocumentViewType _originalWebDefaultView;
        private MxRadioButton _previewRadioButton;
        private WebPackage _webPackage;

        public WebOptionsPage(IServiceProvider provider, WebPackage package) : base(provider)
        {
            this.InitializeComponent();
            this._webPackage = package;
            this._defaultViewComboBox.Items.Add(Enum.GetName(typeof(DocumentViewType), DocumentViewType.Design));
            this._defaultViewComboBox.Items.Add(Enum.GetName(typeof(DocumentViewType), DocumentViewType.Source));
            this._originalWebDefaultView = this._webPackage.WebDefaultViewInternal;
            this._originalDesignModeEnabled = this._webPackage.DesignModeEnabled;
            this._newWebDefaultView = this._originalWebDefaultView;
            this._newDesignModeEnabled = this._originalDesignModeEnabled;
        }

        private void _defaultViewComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._defaultViewComboBox.SelectedItem != null)
            {
                this._newWebDefaultView = (DocumentViewType) Enum.Parse(typeof(DocumentViewType), (string) this._defaultViewComboBox.SelectedItem);
            }
        }

        private void _designRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateUIState();
        }

        public override void CommitChanges()
        {
            this._webPackage.DesignModeEnabled = this._newDesignModeEnabled;
            this._webPackage.WebDefaultView = this._newWebDefaultView;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._webPackage = null;
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._defaultViewLabel = new MxLabel();
            this._defaultViewComboBox = new MxComboBox();
            this._designRadioButton = new MxRadioButton();
            this._previewRadioButton = new MxRadioButton();
            this._infoLabel = new MxLabel();
            this._noteLabel = new MxLabel();
            base.SuspendLayout();
            this._defaultViewLabel.Location = new Point(0x20, 0x84);
            this._defaultViewLabel.Name = "_defaultViewLabel";
            this._defaultViewLabel.Size = new Size(0x48, 0x10);
            this._defaultViewLabel.TabIndex = 3;
            this._defaultViewLabel.Text = "Default &View:";
            this._defaultViewComboBox.AlwaysShowFocusCues = true;
            this._defaultViewComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this._defaultViewComboBox.FlatAppearance = true;
            this._defaultViewComboBox.InitialText = null;
            this._defaultViewComboBox.Location = new Point(0x20, 0x94);
            this._defaultViewComboBox.Name = "_defaultViewComboBox";
            this._defaultViewComboBox.Size = new Size(0x84, 0x15);
            this._defaultViewComboBox.TabIndex = 4;
            this._defaultViewComboBox.SelectedIndexChanged += new EventHandler(this._defaultViewComboBox_SelectedIndexChanged);
            this._designRadioButton.Location = new Point(12, 0x70);
            this._designRadioButton.Name = "_designRadioButton";
            this._designRadioButton.Size = new Size(0x84, 0x10);
            this._designRadioButton.TabIndex = 1;
            this._designRadioButton.Text = "&Design Mode";
            this._designRadioButton.CheckedChanged += new EventHandler(this._designRadioButton_CheckedChanged);
            this._previewRadioButton.Location = new Point(12, 0xb8);
            this._previewRadioButton.Name = "_previewRadioButton";
            this._previewRadioButton.Size = new Size(0x84, 0x10);
            this._previewRadioButton.TabIndex = 2;
            this._previewRadioButton.Text = "&Preview Mode";
            this._infoLabel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this._infoLabel.Location = new Point(12, 12);
            this._infoLabel.Name = "_infoLabel";
            this._infoLabel.Size = new Size(0x1ac, 0x5c);
            this._infoLabel.TabIndex = 0;
            this._infoLabel.Text = "Editing pages in Design view will cause them to be formatted automatically as XHTML.\r\n\r\nTo prevent this, edit your pages in the source views only (HTML, Code and All).  Alternatively you can switch the editor to use 'Preview' mode instead of 'Design' mode.  Preview mode permits you to preview the appearance of the page, but changes made in this view are not saved to the document.";
            this._noteLabel.Location = new Point(8, 0xec);
            this._noteLabel.Name = "_noteLabel";
            this._noteLabel.Size = new Size(0x1ac, 0x1c);
            this._noteLabel.TabIndex = 5;
            this._noteLabel.Text = "Note: If you switch modes, you'll have to close all currently open documents before the new behavior takes effect.";
            base.Controls.AddRange(new Control[] { this._noteLabel, this._infoLabel, this._previewRadioButton, this._designRadioButton, this._defaultViewComboBox, this._defaultViewLabel });
            base.Name = "WebOptionsPage";
            base.Size = new Size(0x1c0, 280);
            base.ResumeLayout(false);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (this._webPackage.DesignModeEnabled)
            {
                this._designRadioButton.Checked = true;
            }
            else
            {
                this._previewRadioButton.Checked = true;
            }
            this.UpdateUIState();
        }

        private void UpdateUIState()
        {
            if (this._designRadioButton.Checked)
            {
                this._newDesignModeEnabled = true;
                this._defaultViewComboBox.Enabled = true;
                if (this._originalWebDefaultView == DocumentViewType.Design)
                {
                    this._defaultViewComboBox.SelectedIndex = 0;
                }
                else
                {
                    this._defaultViewComboBox.SelectedIndex = 1;
                }
            }
            else
            {
                this._newDesignModeEnabled = false;
                this._defaultViewComboBox.Enabled = false;
            }
        }

        public override bool IsDirty
        {
            get
            {
                if (this._originalDesignModeEnabled == this._newDesignModeEnabled)
                {
                    return (this._originalWebDefaultView != this._newWebDefaultView);
                }
                return true;
            }
        }

        public override string OptionsPath
        {
            get
            {
                return @"Web Editing\(General)";
            }
        }
    }
}

