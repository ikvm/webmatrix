namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class FontPicker : MxForm
    {
        private MxButton _addCustomFontButton;
        private MxButton _addGenericFontButton;
        private MxButton _addInstFontButton;
        private MxButton _cancelButton;
        private MxLabel _customFontLabel;
        private TextBox _customFontTextBox;
        private string _fontFamily;
        private MxLabel _installedFontLabel;
        private ListBox _installedFontList;
        private MxLabel _instructionLabel;
        private MxButton _okButton;
        private ImageButton _removeFontButton;
        private MxLabel _selectedFontLabel;
        private ListBox _selectedFontList;
        private ImageButton _sortDownButton;
        private ImageButton _sortUpButton;
        private ComboBox _standardFontsCombo;
        private MxLabel _standardFontsLabel;
        private static readonly string[] StandardFonts = new string[] { "Monospace", "Serif", "Sans-Serif", "Cursive", "Fantasy", "Caption", "SmallCaption", "Menu", "MessageBox", "Icon", "StatusBar" };

        public FontPicker(IServiceProvider serviceProvider, string fontFamily) : base(serviceProvider)
        {
            this.InitializeComponent();
            this._sortUpButton.Image = new Icon(typeof(FontPicker), "SortUp.ico").ToBitmap();
            this._sortDownButton.Image = new Icon(typeof(FontPicker), "SortDown.ico").ToBitmap();
            this._removeFontButton.Image = new Icon(typeof(FontPicker), "Delete.ico").ToBitmap();
            if (fontFamily != null)
            {
                this._fontFamily = fontFamily.Trim();
            }
        }

        private void InitializeComponent()
        {
            this._instructionLabel = new MxLabel();
            this._installedFontLabel = new MxLabel();
            this._standardFontsLabel = new MxLabel();
            this._customFontLabel = new MxLabel();
            this._selectedFontLabel = new MxLabel();
            this._installedFontList = new ListBox();
            this._standardFontsCombo = new ComboBox();
            this._customFontTextBox = new TextBox();
            this._addInstFontButton = new MxButton();
            this._addGenericFontButton = new MxButton();
            this._addCustomFontButton = new MxButton();
            this._selectedFontList = new ListBox();
            this._okButton = new MxButton();
            this._cancelButton = new MxButton();
            this._removeFontButton = new ImageButton();
            this._sortUpButton = new ImageButton();
            this._sortDownButton = new ImageButton();
            base.SuspendLayout();
            this._instructionLabel.Location = new Point(8, 8);
            this._instructionLabel.Name = "_instructionLabel";
            this._instructionLabel.Size = new Size(0xf4, 0x10);
            this._instructionLabel.TabIndex = 0;
            this._instructionLabel.Text = "Create a font sequence in order of preference:";
            this._installedFontLabel.Location = new Point(8, 0x20);
            this._installedFontLabel.Name = "_installedFontLabel";
            this._installedFontLabel.Size = new Size(0x7c, 0x10);
            this._installedFontLabel.TabIndex = 1;
            this._installedFontLabel.Text = "Installed Fonts:";
            this._standardFontsLabel.Location = new Point(8, 110);
            this._standardFontsLabel.Name = "_standardFontsLabel";
            this._standardFontsLabel.Size = new Size(0x7c, 0x10);
            this._standardFontsLabel.TabIndex = 4;
            this._standardFontsLabel.Text = "Standard Fonts:";
            this._customFontLabel.Location = new Point(8, 160);
            this._customFontLabel.Name = "_customFontLabel";
            this._customFontLabel.Size = new Size(0x7c, 0x10);
            this._customFontLabel.TabIndex = 7;
            this._customFontLabel.Text = "Custom Font";
            this._selectedFontLabel.Location = new Point(0xc0, 0x20);
            this._selectedFontLabel.Name = "_selectedFontLabel";
            this._selectedFontLabel.Size = new Size(0x80, 0x10);
            this._selectedFontLabel.TabIndex = 10;
            this._selectedFontLabel.Text = "Selected Font(s):";
            this._installedFontList.IntegralHeight = false;
            this._installedFontList.Location = new Point(8, 50);
            this._installedFontList.Name = "_installedFontList";
            this._installedFontList.Size = new Size(0x80, 0x38);
            this._installedFontList.Sorted = true;
            this._installedFontList.TabIndex = 2;
            this._installedFontList.DoubleClick += new EventHandler(this.OnClickAddInstalledFont);
            this._installedFontList.SelectedIndexChanged += new EventHandler(this.OnSelChangeInstalledFont);
            this._standardFontsCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            this._standardFontsCombo.Location = new Point(8, 0x80);
            this._standardFontsCombo.Name = "_standardFontsCombo";
            this._standardFontsCombo.Size = new Size(0x80, 0x15);
            this._standardFontsCombo.TabIndex = 5;
            this._standardFontsCombo.SelectedIndexChanged += new EventHandler(this.OnSelChangeGenericFont);
            this._customFontTextBox.Location = new Point(8, 0xb2);
            this._customFontTextBox.Name = "_customFontTextBox";
            this._customFontTextBox.Size = new Size(0x80, 20);
            this._customFontTextBox.TabIndex = 8;
            this._customFontTextBox.Text = "";
            this._customFontTextBox.TextChanged += new EventHandler(this.OnChangeCustomFont);
            this._addInstFontButton.Location = new Point(0x90, 0x34);
            this._addInstFontButton.Name = "_addInstFontButton";
            this._addInstFontButton.Size = new Size(0x24, 0x17);
            this._addInstFontButton.TabIndex = 3;
            this._addInstFontButton.Text = ">";
            this._addInstFontButton.Click += new EventHandler(this.OnClickAddInstalledFont);
            this._addGenericFontButton.Location = new Point(0x90, 0x80);
            this._addGenericFontButton.Name = "_addGenericFontButton";
            this._addGenericFontButton.Size = new Size(0x24, 0x17);
            this._addGenericFontButton.TabIndex = 6;
            this._addGenericFontButton.Text = ">";
            this._addGenericFontButton.Click += new EventHandler(this.OnClickAddGenericFont);
            this._addCustomFontButton.Location = new Point(0x90, 0xb0);
            this._addCustomFontButton.Name = "_addCustomFontButton";
            this._addCustomFontButton.Size = new Size(0x24, 0x17);
            this._addCustomFontButton.TabIndex = 9;
            this._addCustomFontButton.Text = ">";
            this._addCustomFontButton.Click += new EventHandler(this.OnClickAddCustomFont);
            this._selectedFontList.IntegralHeight = false;
            this._selectedFontList.Location = new Point(0xc0, 0x30);
            this._selectedFontList.Name = "_selectedFontList";
            this._selectedFontList.Size = new Size(0x80, 0x98);
            this._selectedFontList.TabIndex = 11;
            this._selectedFontList.SelectedIndexChanged += new EventHandler(this.OnSelChangeSelFont);
            this._okButton.Location = new Point(200, 0xd0);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 15;
            this._okButton.Text = "OK";
            this._okButton.Click += new EventHandler(this.OnClickOK);
            this._cancelButton.DialogResult = DialogResult.Cancel;
            this._cancelButton.Location = new Point(280, 0xd0);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 0x10;
            this._cancelButton.Text = "Cancel";
            this._removeFontButton.ImageAlign = ContentAlignment.MiddleCenter;
            this._removeFontButton.Location = new Point(0x144, 0x6a);
            this._removeFontButton.Name = "_removeFontButton";
            this._removeFontButton.Size = new Size(0x19, 0x19);
            this._removeFontButton.TabIndex = 14;
            this._removeFontButton.Click += new EventHandler(this.OnClickRemoveFont);
            this._sortUpButton.Location = new Point(0x144, 0x30);
            this._sortUpButton.Name = "_sortUpButton";
            this._sortUpButton.Size = new Size(0x19, 0x19);
            this._sortUpButton.TabIndex = 12;
            this._sortUpButton.Click += new EventHandler(this.OnClickSortUp);
            this._sortDownButton.Location = new Point(0x144, 0x4d);
            this._sortDownButton.Name = "_sortDownButton";
            this._sortDownButton.Size = new Size(0x19, 0x19);
            this._sortDownButton.TabIndex = 13;
            this._sortDownButton.Click += new EventHandler(this.OnClickSortDown);
            base.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this._cancelButton;
            base.ClientSize = new Size(0x170, 0xef);
            base.Controls.AddRange(new Control[] { 
                this._cancelButton, this._okButton, this._sortUpButton, this._sortDownButton, this._removeFontButton, this._selectedFontList, this._selectedFontLabel, this._addCustomFontButton, this._customFontTextBox, this._customFontLabel, this._addGenericFontButton, this._standardFontsCombo, this._standardFontsLabel, this._addInstFontButton, this._installedFontList, this._installedFontLabel, 
                this._instructionLabel
             });
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FontPicker";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Font Picker";
            base.ResumeLayout(false);
        }

        private void InitializeUserInterface()
        {
            int num2;
            System.Drawing.FontFamily[] families = System.Drawing.FontFamily.Families;
            for (int i = 0; i < families.Length; i++)
            {
                if (this._installedFontList.FindStringExact(families[i].Name) == -1)
                {
                    this._installedFontList.Items.Add(families[i].Name);
                }
            }
            this._standardFontsCombo.Items.Clear();
            this._standardFontsCombo.Items.AddRange(StandardFonts);
            this._installedFontList.SelectedIndex = 0;
            this._standardFontsCombo.SelectedIndex = 0;
            this._customFontTextBox.Clear();
            if ((this._fontFamily == null) || (this._fontFamily.Length == 0))
            {
                goto Label_0193;
            }
            string item = null;
            int index = 0;
        Label_00AB:
            num2 = index;
            index = this._fontFamily.IndexOf(',', num2);
            if (index < 0)
            {
                if (num2 < this._fontFamily.Length)
                {
                    item = this._fontFamily.Substring(num2);
                }
            }
            else if (index > num2)
            {
                item = this._fontFamily.Substring(num2, index - num2);
            }
            if (item != null)
            {
                item = item.Trim();
                if (((item[0] == '\'') || (item[0] == '"')) && (item.Length > 2))
                {
                    item = item.Substring(1, item.Length - 2);
                }
                if (((item.Length != 0) && !item.StartsWith("\"")) && !item.StartsWith("'"))
                {
                    this._selectedFontList.Items.Add(item);
                }
                item = null;
            }
            if (index >= 0)
            {
                index++;
                goto Label_00AB;
            }
            if (this._selectedFontList.Items.Count != 0)
            {
                this._selectedFontList.SelectedIndex = 0;
            }
        Label_0193:
            this.UpdateEnabledState(true, true);
        }

        private void OnChangeCustomFont(object source, EventArgs e)
        {
            this.UpdateEnabledState(true, false);
        }

        private void OnClickAddCustomFont(object source, EventArgs e)
        {
            string item = this._customFontTextBox.Text.Trim();
            int num = this._selectedFontList.Items.Add(item);
            this._selectedFontList.SelectedIndex = num;
            this._selectedFontList.Focus();
            this._customFontTextBox.Clear();
            this.UpdateEnabledState(true, true);
        }

        private void OnClickAddGenericFont(object source, EventArgs e)
        {
            int selectedIndex = this._standardFontsCombo.SelectedIndex;
            int num2 = this._selectedFontList.Items.Add(StandardFonts[selectedIndex]);
            this._selectedFontList.SelectedIndex = num2;
            this._selectedFontList.Focus();
            this._standardFontsCombo.SelectedIndex = -1;
            this.UpdateEnabledState(true, true);
        }

        private void OnClickAddInstalledFont(object source, EventArgs e)
        {
            string item = this._installedFontList.SelectedItem.ToString();
            int num = this._selectedFontList.Items.Add(item);
            this._selectedFontList.SelectedIndex = num;
            this._selectedFontList.Focus();
            this._installedFontList.SelectedIndex = -1;
            this.UpdateEnabledState(true, true);
        }

        private void OnClickOK(object source, EventArgs e)
        {
            this.SaveSelectedFonts();
            base.Close();
            base.DialogResult = DialogResult.OK;
        }

        private void OnClickRemoveFont(object source, EventArgs e)
        {
            int selectedIndex = this._selectedFontList.SelectedIndex;
            this._selectedFontList.Items.RemoveAt(selectedIndex);
            if (this._selectedFontList.Items.Count > 0)
            {
                if (selectedIndex > 0)
                {
                    selectedIndex--;
                }
                this._selectedFontList.SelectedIndex = selectedIndex;
            }
            else
            {
                this._selectedFontList.Focus();
            }
            this.UpdateEnabledState(false, true);
        }

        private void OnClickSortDown(object source, EventArgs e)
        {
            int selectedIndex = this._selectedFontList.SelectedIndex;
            string item = this._selectedFontList.Items[selectedIndex].ToString();
            this._selectedFontList.Items.RemoveAt(selectedIndex);
            this._selectedFontList.Items.Insert(selectedIndex + 1, item);
            this._selectedFontList.SelectedIndex = selectedIndex + 1;
            this.UpdateEnabledState(false, true);
        }

        private void OnClickSortUp(object source, EventArgs e)
        {
            int selectedIndex = this._selectedFontList.SelectedIndex;
            string item = this._selectedFontList.Items[selectedIndex].ToString();
            this._selectedFontList.Items.RemoveAt(selectedIndex);
            this._selectedFontList.Items.Insert(selectedIndex - 1, item);
            this._selectedFontList.SelectedIndex = selectedIndex - 1;
            this.UpdateEnabledState(false, true);
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
            this.InitializeUserInterface();
        }

        private void OnSelChangeGenericFont(object source, EventArgs e)
        {
            this.UpdateEnabledState(true, false);
        }

        private void OnSelChangeInstalledFont(object source, EventArgs e)
        {
            this.UpdateEnabledState(true, false);
        }

        private void OnSelChangeSelFont(object source, EventArgs e)
        {
            this.UpdateEnabledState(false, true);
        }

        private void SaveSelectedFonts()
        {
            int count = this._selectedFontList.Items.Count;
            this._fontFamily = string.Empty;
            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    string str = this._selectedFontList.Items[i].ToString();
                    if (str.IndexOf(' ') > 0)
                    {
                        str = "'" + str + "'";
                    }
                    if (i > 0)
                    {
                        this._fontFamily = this._fontFamily + ", ";
                    }
                    this._fontFamily = this._fontFamily + str;
                }
            }
        }

        private void UpdateEnabledState(bool addUI, bool removeSortUI)
        {
            if (addUI)
            {
                this._addInstFontButton.Enabled = this._installedFontList.SelectedIndex != -1;
                this._addGenericFontButton.Enabled = this._standardFontsCombo.SelectedIndex != -1;
                this._addCustomFontButton.Enabled = this._customFontTextBox.Text.Trim().Length != 0;
            }
            if (removeSortUI)
            {
                int count = this._selectedFontList.Items.Count;
                int selectedIndex = this._selectedFontList.SelectedIndex;
                this._removeFontButton.Enabled = (count > 0) && (selectedIndex != -1);
                this._sortUpButton.Enabled = (count > 0) && (selectedIndex > 0);
                this._sortDownButton.Enabled = (count > 0) && (selectedIndex < (count - 1));
            }
        }

        public string FontFamily
        {
            get
            {
                return this._fontFamily;
            }
        }
    }
}

