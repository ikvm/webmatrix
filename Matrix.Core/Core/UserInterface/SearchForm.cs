namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class SearchForm : MxForm
    {
        private MxButton _closeButton;
        private MxGroupBox _directionGroupBox;
        private MxRadioButton _downRadioButton;
        private MxButton _findButton;
        private MxComboBox _findComboBox;
        private MruList _findMru;
        private Microsoft.Matrix.Core.UserInterface.FindReplaceOptions _findReplaceOptions;
        private MxLabel _findWhatLabel;
        private MxCheckBox _matchCaseCheckBox;
        private MxRadioButton _upRadioButton;
        private ISearchableDocumentView _view;
        private MxCheckBox _wholeWordCheckBox;

        public SearchForm(IServiceProvider serviceProvider, ISearchableDocumentView view, Microsoft.Matrix.Core.UserInterface.FindReplaceOptions FindReplaceOptions, string initialSearchString, MruList findMru) : base(serviceProvider)
        {
            this.InitializeComponent();
            this._view = view;
            this._findMru = findMru;
            this._findComboBox.FlatAppearance = true;
            this._findComboBox.AlwaysShowFocusCues = true;
            this._findComboBox.TextBoxKeyPress += new KeyPressEventHandler(this.OnTextBoxKeyPress);
            this._findComboBox.TextChanged += new EventHandler(this.OnFindComboBoxTextChanged);
            if ((initialSearchString != null) && (initialSearchString.Length > 0))
            {
                this._findComboBox.Text = initialSearchString;
            }
            else if (findMru.Count != 0)
            {
                this._findComboBox.Items.AddRange(findMru.Save());
                this._findComboBox.SelectedIndex = 0;
            }
            this._findComboBox.Select();
            this.SetFindReplaceOptions(FindReplaceOptions);
        }

        private void _closeButton_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void _findButton_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private Microsoft.Matrix.Core.UserInterface.FindReplaceOptions GetFindReplaceOptions()
        {
            Microsoft.Matrix.Core.UserInterface.FindReplaceOptions none = Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.None;
            if (this._matchCaseCheckBox.Checked)
            {
                none |= Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.MatchCase;
            }
            if (this._wholeWordCheckBox.Checked)
            {
                none |= Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.WholeWord;
            }
            if (this._upRadioButton.Checked)
            {
                none |= Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.SearchUp;
            }
            return none;
        }

        private void InitializeComponent()
        {
            this._findWhatLabel = new MxLabel();
            this._findComboBox = new MxComboBox();
            this._findButton = new MxButton();
            this._matchCaseCheckBox = new MxCheckBox();
            this._wholeWordCheckBox = new MxCheckBox();
            this._closeButton = new MxButton();
            this._directionGroupBox = new MxGroupBox();
            this._upRadioButton = new MxRadioButton();
            this._downRadioButton = new MxRadioButton();
            this._directionGroupBox.SuspendLayout();
            base.SuspendLayout();
            this._findWhatLabel.Location = new Point(8, 12);
            this._findWhatLabel.Name = "_findWhatLabel";
            this._findWhatLabel.Size = new Size(0x44, 0x10);
            this._findWhatLabel.TabIndex = 0;
            this._findWhatLabel.Text = "Fi&nd What :";
            this._findComboBox.AlwaysShowFocusCues = false;
            this._findComboBox.FlatAppearance = false;
            this._findComboBox.InitialText = null;
            this._findComboBox.Location = new Point(0x4c, 10);
            this._findComboBox.Name = "_findComboBox";
            this._findComboBox.Size = new Size(0x110, 0x15);
            this._findComboBox.TabIndex = 1;
            this._findButton.Enabled = false;
            this._findButton.Location = new Point(360, 8);
            this._findButton.Name = "_findButton";
            this._findButton.TabIndex = 6;
            this._findButton.Text = "&Find";
            this._findButton.Click += new EventHandler(this._findButton_Click);
            this._matchCaseCheckBox.Location = new Point(8, 0x2c);
            this._matchCaseCheckBox.Name = "_matchCaseCheckBox";
            this._matchCaseCheckBox.Size = new Size(0x68, 0x10);
            this._matchCaseCheckBox.TabIndex = 2;
            this._matchCaseCheckBox.Text = "Match &case";
            this._wholeWordCheckBox.Location = new Point(8, 60);
            this._wholeWordCheckBox.Name = "_wholeWordCheckBox";
            this._wholeWordCheckBox.Size = new Size(0x68, 0x10);
            this._wholeWordCheckBox.TabIndex = 3;
            this._wholeWordCheckBox.Text = "Whole &word";
            this._closeButton.DialogResult = DialogResult.OK;
            this._closeButton.Location = new Point(360, 0x24);
            this._closeButton.Name = "_closeButton";
            this._closeButton.TabIndex = 7;
            this._closeButton.Text = "Close";
            this._closeButton.Click += new EventHandler(this._closeButton_Click);
            this._directionGroupBox.Controls.AddRange(new Control[] { this._upRadioButton, this._downRadioButton });
            this._directionGroupBox.Location = new Point(0xd8, 40);
            this._directionGroupBox.Name = "_directionGroupBox";
            this._directionGroupBox.Size = new Size(0x84, 0x38);
            this._directionGroupBox.TabIndex = 0;
            this._directionGroupBox.TabStop = false;
            this._directionGroupBox.Text = "Direction";
            this._upRadioButton.Location = new Point(8, 0x18);
            this._upRadioButton.Name = "_upRadioButton";
            this._upRadioButton.Size = new Size(0x34, 20);
            this._upRadioButton.TabIndex = 4;
            this._upRadioButton.Text = "&Up";
            this._downRadioButton.Checked = true;
            this._downRadioButton.Location = new Point(0x48, 0x18);
            this._downRadioButton.Name = "_downRadioButton";
            this._downRadioButton.Size = new Size(50, 20);
            this._downRadioButton.TabIndex = 5;
            this._downRadioButton.TabStop = true;
            this._downRadioButton.Text = "&Down";
            base.AcceptButton = this._findButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._closeButton;
            base.ClientSize = new Size(0x1bc, 0x66);
            base.Controls.AddRange(new Control[] { this._directionGroupBox, this._closeButton, this._wholeWordCheckBox, this._matchCaseCheckBox, this._findButton, this._findComboBox, this._findWhatLabel });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = null;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SearchForm";
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Search";
            this._directionGroupBox.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnFindComboBoxTextChanged(object sender, EventArgs e)
        {
            if (this._findComboBox.Text.Length == 0)
            {
                this._findButton.Enabled = false;
            }
            else
            {
                this._findButton.Enabled = true;
            }
        }

        private void OnTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.Search();
            }
        }

        private void Search()
        {
            string text = this._findComboBox.Text;
            this._findMru.AddEntry(text);
            this._findComboBox.Items.Clear();
            this._findComboBox.Items.AddRange(this._findMru.Save());
            this._findComboBox.SelectedIndex = 0;
            this._findReplaceOptions = this.GetFindReplaceOptions();
            if (!this._view.PerformFind(text, this._findReplaceOptions))
            {
                IUIService service = (IUIService) base.ServiceProvider.GetService(typeof(IUIService));
                if (service != null)
                {
                    service.ShowMessage("Couldn't find '" + text + "'", string.Empty, MessageBoxButtons.OK);
                }
            }
        }

        private void SetFindReplaceOptions(Microsoft.Matrix.Core.UserInterface.FindReplaceOptions FindReplaceOptions)
        {
            this._findReplaceOptions = FindReplaceOptions;
            if ((FindReplaceOptions & Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.MatchCase) != Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.None)
            {
                this._matchCaseCheckBox.Checked = true;
            }
            if ((FindReplaceOptions & Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.WholeWord) != Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.None)
            {
                this._wholeWordCheckBox.Checked = true;
            }
            if ((FindReplaceOptions & Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.SearchUp) != Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.None)
            {
                this._upRadioButton.Checked = true;
            }
        }

        public Microsoft.Matrix.Core.UserInterface.FindReplaceOptions FindReplaceOptions
        {
            get
            {
                return this._findReplaceOptions;
            }
        }
    }
}

