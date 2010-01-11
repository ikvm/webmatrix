namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.Utility;
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class ReplaceDialog : MxForm
    {
        private MxButton _closeButton;
        private MxGroupBox _directionGroupBox;
        private ISearchableDocumentView _documentView;
        private MxRadioButton _downRadioButton;
        private MxLabel _findLabel;
        private MruList _findMru;
        private MxButton _findNextButton;
        private Microsoft.Matrix.Core.UserInterface.FindReplaceOptions _findReplaceOptions;
        private MxCheckBox _inSelectionCheckBox;
        private MxCheckBox _matchCaseCheckBox;
        private MxButton _replaceAllButton;
        private MxButton _replaceButton;
        private MxLabel _replaceLabel;
        private MruList _replaceMru;
        private MxComboBox _replaceStringComboBox;
        private MxComboBox _searchStringComboBox;
        private MxRadioButton _upRadioButton;
        private MxCheckBox _wholeWordCheckBox;

        public ReplaceDialog(IServiceProvider provider, ISearchableDocumentView view, Microsoft.Matrix.Core.UserInterface.FindReplaceOptions FindReplaceOptions, string initialSearchString, MruList findMru, MruList replaceMru) : base(provider)
        {
            this.InitializeComponent();
            this._documentView = view;
            if ((this._documentView.ReplaceSupport & Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.InSelection) == Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.None)
            {
                this._inSelectionCheckBox.Enabled = false;
            }
            if ((this._documentView.ReplaceSupport & Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.All) == Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.None)
            {
                this._replaceAllButton.Enabled = false;
            }
            this._findMru = findMru;
            this._replaceMru = replaceMru;
            if ((initialSearchString != null) && (initialSearchString.Length > 0))
            {
                this._searchStringComboBox.Text = initialSearchString;
            }
            else
            {
                if (findMru.Count != 0)
                {
                    this._searchStringComboBox.Items.AddRange(findMru.Save());
                    this._searchStringComboBox.SelectedIndex = 0;
                }
                if (replaceMru.Count != 0)
                {
                    this._replaceStringComboBox.Items.AddRange(replaceMru.Save());
                    this._replaceStringComboBox.SelectedIndex = 0;
                }
            }
            this.SetFindReplaceOptions(FindReplaceOptions);
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
            if (this._inSelectionCheckBox.Checked)
            {
                none |= Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.InSelection;
            }
            return none;
        }

        private void InitializeComponent()
        {
            this._searchStringComboBox = new MxComboBox();
            this._replaceStringComboBox = new MxComboBox();
            this._replaceButton = new MxButton();
            this._replaceAllButton = new MxButton();
            this._closeButton = new MxButton();
            this._findLabel = new MxLabel();
            this._replaceLabel = new MxLabel();
            this._findNextButton = new MxButton();
            this._matchCaseCheckBox = new MxCheckBox();
            this._wholeWordCheckBox = new MxCheckBox();
            this._directionGroupBox = new MxGroupBox();
            this._downRadioButton = new MxRadioButton();
            this._upRadioButton = new MxRadioButton();
            this._inSelectionCheckBox = new MxCheckBox();
            this._directionGroupBox.SuspendLayout();
            base.SuspendLayout();
            this._searchStringComboBox.AlwaysShowFocusCues = true;
            this._searchStringComboBox.FlatAppearance = true;
            this._searchStringComboBox.InitialText = null;
            this._searchStringComboBox.Location = new Point(0x5c, 9);
            this._searchStringComboBox.Name = "_searchStringComboBox";
            this._searchStringComboBox.Size = new Size(280, 0x15);
            this._searchStringComboBox.TabIndex = 1;
            this._searchStringComboBox.TextChanged += new EventHandler(this.OnSearchStringComboBoxTextChanged);
            this._replaceStringComboBox.AlwaysShowFocusCues = true;
            this._replaceStringComboBox.FlatAppearance = true;
            this._replaceStringComboBox.InitialText = null;
            this._replaceStringComboBox.Location = new Point(0x5c, 0x25);
            this._replaceStringComboBox.Name = "_replaceStringComboBox";
            this._replaceStringComboBox.Size = new Size(280, 0x15);
            this._replaceStringComboBox.TabIndex = 2;
            this._replaceButton.Enabled = false;
            this._replaceButton.Location = new Point(0x180, 0x24);
            this._replaceButton.Name = "_replaceButton";
            this._replaceButton.TabIndex = 4;
            this._replaceButton.Text = "&Replace";
            this._replaceButton.Click += new EventHandler(this.OnReplaceButtonClick);
            this._replaceAllButton.Enabled = false;
            this._replaceAllButton.Location = new Point(0x180, 0x40);
            this._replaceAllButton.Name = "_replaceAllButton";
            this._replaceAllButton.TabIndex = 6;
            this._replaceAllButton.Text = "Replace &All";
            this._replaceAllButton.Click += new EventHandler(this.OnReplaceAllClick);
            this._closeButton.DialogResult = DialogResult.OK;
            this._closeButton.Location = new Point(0x180, 0x5c);
            this._closeButton.Name = "_closeButton";
            this._closeButton.TabIndex = 10;
            this._closeButton.Text = "&Close";
            this._findLabel.Location = new Point(12, 12);
            this._findLabel.Name = "_findLabel";
            this._findLabel.Size = new Size(60, 0x10);
            this._findLabel.TabIndex = 5;
            this._findLabel.Text = "Fi&nd what :";
            this._replaceLabel.Location = new Point(12, 40);
            this._replaceLabel.Name = "_replaceLabel";
            this._replaceLabel.Size = new Size(0x4c, 0x10);
            this._replaceLabel.TabIndex = 6;
            this._replaceLabel.Text = "Re&place with :";
            this._findNextButton.Enabled = false;
            this._findNextButton.Location = new Point(0x180, 8);
            this._findNextButton.Name = "_findNextButton";
            this._findNextButton.TabIndex = 3;
            this._findNextButton.Text = "&Find Next";
            this._findNextButton.Click += new EventHandler(this.OnFindButtonClick);
            this._matchCaseCheckBox.Location = new Point(12, 0x44);
            this._matchCaseCheckBox.Name = "_matchCaseCheckBox";
            this._matchCaseCheckBox.Size = new Size(0x58, 0x10);
            this._matchCaseCheckBox.TabIndex = 7;
            this._matchCaseCheckBox.Text = "&Match case";
            this._wholeWordCheckBox.Location = new Point(12, 0x54);
            this._wholeWordCheckBox.Name = "_wholeWordCheckBox";
            this._wholeWordCheckBox.Size = new Size(0x58, 0x10);
            this._wholeWordCheckBox.TabIndex = 8;
            this._wholeWordCheckBox.Text = "&Whole word";
            this._directionGroupBox.Controls.AddRange(new Control[] { this._downRadioButton, this._upRadioButton });
            this._directionGroupBox.Location = new Point(0xec, 0x40);
            this._directionGroupBox.Name = "_directionGroupBox";
            this._directionGroupBox.Size = new Size(0x88, 0x34);
            this._directionGroupBox.TabIndex = 10;
            this._directionGroupBox.TabStop = false;
            this._directionGroupBox.Text = "Direction";
            this._downRadioButton.Checked = true;
            this._downRadioButton.Location = new Point(0x48, 0x18);
            this._downRadioButton.Name = "_downRadioButton";
            this._downRadioButton.Size = new Size(0x34, 0x10);
            this._downRadioButton.TabIndex = 11;
            this._downRadioButton.TabStop = true;
            this._downRadioButton.Text = "&Down";
            this._upRadioButton.Location = new Point(0x10, 0x18);
            this._upRadioButton.Name = "_upRadioButton";
            this._upRadioButton.Size = new Size(40, 0x10);
            this._upRadioButton.TabIndex = 10;
            this._upRadioButton.Text = "&Up";
            this._inSelectionCheckBox.Location = new Point(12, 100);
            this._inSelectionCheckBox.Name = "_inSelectionCheckBox";
            this._inSelectionCheckBox.Size = new Size(0x58, 0x10);
            this._inSelectionCheckBox.TabIndex = 11;
            this._inSelectionCheckBox.Text = "&In selection";
            this._inSelectionCheckBox.CheckedChanged += new EventHandler(this.OnInSelectionCheckBoxCheckedChanged);
            base.AcceptButton = this._findNextButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._closeButton;
            base.ClientSize = new Size(0x1d8, 0x7a);
            base.Controls.AddRange(new Control[] { this._inSelectionCheckBox, this._directionGroupBox, this._wholeWordCheckBox, this._matchCaseCheckBox, this._findNextButton, this._replaceLabel, this._findLabel, this._closeButton, this._replaceAllButton, this._replaceButton, this._replaceStringComboBox, this._searchStringComboBox });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ReplaceDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Replace";
            this._directionGroupBox.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void OnFindButtonClick(object sender, EventArgs e)
        {
            string text = this._searchStringComboBox.Text;
            if (text.Length != 0)
            {
                this._findMru.AddEntry(text);
                this._searchStringComboBox.Items.Clear();
                this._searchStringComboBox.Items.AddRange(this._findMru.Save());
                this._searchStringComboBox.SelectedIndex = 0;
                this._findReplaceOptions = this.GetFindReplaceOptions();
                if (!this._documentView.PerformFind(text, this._findReplaceOptions))
                {
                    this.ShowNotFoundMessage(text);
                }
            }
        }

        private void OnInSelectionCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEnabled();
        }

        private void OnReplaceAllClick(object sender, EventArgs e)
        {
            string text = this._searchStringComboBox.Text;
            if (text.Length != 0)
            {
                string entry = this._replaceStringComboBox.Text;
                this._findMru.AddEntry(text);
                this._searchStringComboBox.Items.Clear();
                this._searchStringComboBox.Items.AddRange(this._findMru.Save());
                this._searchStringComboBox.SelectedIndex = 0;
                this._replaceMru.AddEntry(entry);
                this._replaceStringComboBox.Items.Clear();
                this._replaceStringComboBox.Items.AddRange(this._replaceMru.Save());
                this._replaceStringComboBox.SelectedIndex = 0;
                this._findReplaceOptions = this.GetFindReplaceOptions();
                if (!this._documentView.PerformReplace(text, entry, this._findReplaceOptions | Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.All))
                {
                    this.ShowNotFoundMessage(text);
                }
            }
        }

        private void OnReplaceButtonClick(object sender, EventArgs e)
        {
            string text = this._searchStringComboBox.Text;
            if (text.Length != 0)
            {
                string entry = this._replaceStringComboBox.Text;
                this._findMru.AddEntry(text);
                this._searchStringComboBox.Items.Clear();
                this._searchStringComboBox.Items.AddRange(this._findMru.Save());
                this._searchStringComboBox.SelectedIndex = 0;
                this._replaceMru.AddEntry(entry);
                this._replaceStringComboBox.Items.Clear();
                this._replaceStringComboBox.Items.AddRange(this._replaceMru.Save());
                this._replaceStringComboBox.SelectedIndex = 0;
                this._findReplaceOptions = this.GetFindReplaceOptions();
                if (!this._documentView.PerformReplace(text, entry, this._findReplaceOptions))
                {
                    this.ShowNotFoundMessage(text);
                }
            }
        }

        private void OnSearchStringComboBoxTextChanged(object sender, EventArgs e)
        {
            this.UpdateEnabled();
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
            if ((FindReplaceOptions & Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.InSelection) != Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.None)
            {
                this._inSelectionCheckBox.Checked = true;
            }
        }

        private void ShowNotFoundMessage(string searchString)
        {
            IUIService service = (IUIService) base.ServiceProvider.GetService(typeof(IUIService));
            if (service != null)
            {
                service.ShowMessage("Couldn't find '" + searchString + "'", string.Empty, MessageBoxButtons.OK);
            }
        }

        private void UpdateEnabled()
        {
            if (this._searchStringComboBox.Text.Length == 0)
            {
                this._findNextButton.Enabled = false;
                this._replaceButton.Enabled = false;
                this._replaceAllButton.Enabled = false;
            }
            else
            {
                if (!this._inSelectionCheckBox.Checked)
                {
                    this._findNextButton.Enabled = true;
                    this._replaceButton.Enabled = true;
                }
                else
                {
                    this._findNextButton.Enabled = false;
                    this._replaceButton.Enabled = false;
                }
                if ((this._documentView.ReplaceSupport & Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.All) != Microsoft.Matrix.Core.UserInterface.FindReplaceOptions.None)
                {
                    this._replaceAllButton.Enabled = true;
                }
            }
            if (this._inSelectionCheckBox.Checked)
            {
                this._upRadioButton.Enabled = false;
                this._downRadioButton.Enabled = false;
                base.AcceptButton = this._replaceAllButton;
            }
            else
            {
                this._upRadioButton.Enabled = true;
                this._downRadioButton.Enabled = true;
                base.AcceptButton = this._findNextButton;
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

