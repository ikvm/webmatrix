namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Drawing;

    internal sealed class TextLanguageOptionsPage : OptionsPage
    {
        private MxCheckBox _convertTabsCheckBox;
        private bool _dirty;
        private TextDocumentLanguage _language;
        private MxCheckBox _showLineNumbersCheckBox;
        private MxCheckBox _showWhitespaceCheckBox;
        private MxLabel _tabSizeLabel;
        private MxTextBox _tabSizeTextBox;
        private MxCheckBox _trimCheckBox;

        public TextLanguageOptionsPage(TextDocumentLanguage language, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.InitializeComponent();
            this._language = language;
            TextOptions textOptions = language.GetTextOptions(serviceProvider);
            this._tabSizeTextBox.Text = textOptions.TabSize.ToString();
            this._convertTabsCheckBox.Checked = textOptions.ConvertTabsToSpaces;
            this._trimCheckBox.Checked = textOptions.TrimTrailingWhitespace;
            this._showWhitespaceCheckBox.Checked = textOptions.ShowWhitespace;
            this._showLineNumbersCheckBox.Checked = textOptions.ShowLineNumbers;
        }

        public override void CommitChanges()
        {
            TextOptions textOptions = this._language.GetTextOptions(base.ServiceProvider);
            textOptions.TabSize = int.Parse(this._tabSizeTextBox.Text);
            textOptions.ConvertTabsToSpaces = this._convertTabsCheckBox.Checked;
            textOptions.TrimTrailingWhitespace = this._trimCheckBox.Checked;
            textOptions.ShowLineNumbers = this._showLineNumbersCheckBox.Checked;
            textOptions.ShowWhitespace = this._showWhitespaceCheckBox.Checked;
            MxTextManager service = this.GetService(typeof(TextManager)) as MxTextManager;
            if (service != null)
            {
                service.UpdateLanguageSettings();
            }
        }

        private void InitializeComponent()
        {
            this._tabSizeLabel = new MxLabel();
            this._tabSizeTextBox = new MxTextBox();
            this._convertTabsCheckBox = new MxCheckBox();
            this._trimCheckBox = new MxCheckBox();
            this._showWhitespaceCheckBox = new MxCheckBox();
            this._showLineNumbersCheckBox = new MxCheckBox();
            base.SuspendLayout();
            this._tabSizeLabel.Location = new Point(8, 0x10);
            this._tabSizeLabel.Name = "_tabSizeLabel";
            this._tabSizeLabel.Size = new Size(0x34, 0x10);
            this._tabSizeLabel.TabIndex = 0;
            this._tabSizeLabel.Text = "Tab Size:";
            this._tabSizeTextBox.Location = new Point(0x44, 12);
            this._tabSizeTextBox.Name = "_tabSizeTextBox";
            this._tabSizeTextBox.Numeric = true;
            this._tabSizeTextBox.Size = new Size(0x34, 20);
            this._tabSizeTextBox.TabIndex = 1;
            this._tabSizeTextBox.Text = "";
            this._tabSizeTextBox.TextChanged += new EventHandler(this.OnValueChanged);
            this._convertTabsCheckBox.Location = new Point(8, 0x38);
            this._convertTabsCheckBox.Name = "_convertTabsCheckBox";
            this._convertTabsCheckBox.Size = new Size(140, 20);
            this._convertTabsCheckBox.TabIndex = 2;
            this._convertTabsCheckBox.Text = "Convert tabs to spaces";
            this._convertTabsCheckBox.CheckedChanged += new EventHandler(this.OnValueChanged);
            this._trimCheckBox.Location = new Point(8, 80);
            this._trimCheckBox.Name = "_trimCheckBox";
            this._trimCheckBox.Size = new Size(160, 20);
            this._trimCheckBox.TabIndex = 3;
            this._trimCheckBox.Text = "Trim trailing whitespace";
            this._trimCheckBox.CheckedChanged += new EventHandler(this.OnValueChanged);
            this._showWhitespaceCheckBox.Location = new Point(8, 0x68);
            this._showWhitespaceCheckBox.Name = "_showWhitespaceCheckBox";
            this._showWhitespaceCheckBox.Size = new Size(140, 20);
            this._showWhitespaceCheckBox.TabIndex = 4;
            this._showWhitespaceCheckBox.Text = "Show whitespace";
            this._showWhitespaceCheckBox.CheckedChanged += new EventHandler(this.OnValueChanged);
            this._showLineNumbersCheckBox.Location = new Point(8, 0x98);
            this._showLineNumbersCheckBox.Name = "_showLineNumbersCheckBox";
            this._showLineNumbersCheckBox.Size = new Size(140, 20);
            this._showLineNumbersCheckBox.TabIndex = 4;
            this._showLineNumbersCheckBox.Text = "Show line numbers";
            this._showLineNumbersCheckBox.CheckedChanged += new EventHandler(this.OnValueChanged);
            base.Controls.Add(this._showLineNumbersCheckBox);
            base.Controls.Add(this._showWhitespaceCheckBox);
            base.Controls.Add(this._trimCheckBox);
            base.Controls.Add(this._convertTabsCheckBox);
            base.Controls.Add(this._tabSizeTextBox);
            base.Controls.Add(this._tabSizeLabel);
            base.Name = "TextLanguageOptionsPage";
            base.Size = new Size(0x174, 0xb0);
            base.ResumeLayout(false);
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            this._dirty = true;
        }

        public override bool IsDirty
        {
            get
            {
                return this._dirty;
            }
        }

        public override string OptionsPath
        {
            get
            {
                return (@"Text Editor\Languages\" + this._language.DisplayName + @"\(General)");
            }
        }
    }
}

