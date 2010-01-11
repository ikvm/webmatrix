namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class FontOptionsPage : OptionsPage
    {
        private bool _dirty;
        private MxComboBox _fontComboBox;
        private FontComboBox _fontFaceComboBox;
        private Label _fontFaceLabel;
        private Label _fontLabel;
        private MxComboBox _fontSizeComboBox;
        private bool _internalChange;
        private GdiLabel _previewLabel;
        private Panel _previewPanel;
        private GroupLabel _sampleLabel;
        private Label _sizeLabel;
        private Microsoft.Matrix.UIComponents.SourceEditing.TextManager _textManager;
        private static readonly string[] fontSizes = new string[] { "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "36" };

        public FontOptionsPage(Microsoft.Matrix.UIComponents.SourceEditing.TextManager textManager, IServiceProvider provider) : base(provider)
        {
            this.InitializeComponent();
            this._textManager = textManager;
            this._internalChange = true;
            try
            {
                this._fontSizeComboBox.Items.AddRange(fontSizes);
                this._fontComboBox.Items.Add(new SourceFontInfo(this));
                this._fontComboBox.Items.Add(new PrintFontInfo(this));
            }
            finally
            {
                this._internalChange = false;
            }
        }

        public override void CommitChanges()
        {
            foreach (FontInfo info in this._fontComboBox.Items)
            {
                if (info.HasChanged)
                {
                    info.Save();
                }
            }
        }

        private void InitializeComponent()
        {
            this._fontLabel = new Label();
            this._fontFaceLabel = new Label();
            this._fontFaceComboBox = new FontComboBox();
            this._sizeLabel = new Label();
            this._fontSizeComboBox = new MxComboBox();
            this._fontComboBox = new MxComboBox();
            this._previewLabel = new GdiLabel();
            this._sampleLabel = new GroupLabel();
            this._previewPanel = new Panel();
            base.SuspendLayout();
            this._fontLabel.Location = new Point(8, 8);
            this._fontLabel.Name = "_fontLabel";
            this._fontLabel.Size = new Size(0x80, 0x10);
            this._fontLabel.TabIndex = 0;
            this._fontLabel.Text = "Show S&ettings For:";
            this._fontFaceLabel.Location = new Point(8, 0x38);
            this._fontFaceLabel.Name = "_fontFaceLabel";
            this._fontFaceLabel.Size = new Size(0x4c, 0x10);
            this._fontFaceLabel.TabIndex = 2;
            this._fontFaceLabel.Text = "&Font Face:";
            this._fontFaceComboBox.AlwaysShowFocusCues = true;
            this._fontFaceComboBox.DeferFontEnumeration = true;
            this._fontFaceComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            this._fontFaceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this._fontFaceComboBox.FlatAppearance = true;
            this._fontFaceComboBox.Location = new Point(8, 0x48);
            this._fontFaceComboBox.MaxDropDownItems = 10;
            this._fontFaceComboBox.Name = "_fontFaceComboBox";
            this._fontFaceComboBox.ShowFixedFontsOnly = true;
            this._fontFaceComboBox.ShowFontPreview = false;
            this._fontFaceComboBox.Size = new Size(0xe0, 20);
            this._fontFaceComboBox.TabIndex = 3;
            this._fontFaceComboBox.SelectedIndexChanged += new EventHandler(this.OnFontFaceSelectedIndexChanged);
            this._sizeLabel.Location = new Point(0xfc, 0x38);
            this._sizeLabel.Name = "_sizeLabel";
            this._sizeLabel.Size = new Size(0x24, 0x10);
            this._sizeLabel.TabIndex = 4;
            this._sizeLabel.Text = "&Size:";
            this._fontSizeComboBox.AlwaysShowFocusCues = true;
            this._fontSizeComboBox.FlatAppearance = true;
            this._fontSizeComboBox.Location = new Point(0xfc, 0x48);
            this._fontSizeComboBox.Name = "_fontSizeComboBox";
            this._fontSizeComboBox.Size = new Size(0x44, 0x15);
            this._fontSizeComboBox.TabIndex = 5;
            this._fontSizeComboBox.SelectedIndexChanged += new EventHandler(this.OnFontSizeComboBoxSelectedIndexChanged);
            this._fontSizeComboBox.TextChanged += new EventHandler(this.OnFontSizeComboBoxTextChanged);
            this._fontComboBox.AlwaysShowFocusCues = true;
            this._fontComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this._fontComboBox.FlatAppearance = true;
            this._fontComboBox.Location = new Point(8, 0x18);
            this._fontComboBox.Name = "_fontComboBox";
            this._fontComboBox.Size = new Size(0xe0, 0x15);
            this._fontComboBox.TabIndex = 1;
            this._fontComboBox.SelectedIndexChanged += new EventHandler(this.OnFontComboBoxSelectedIndexChanged);
            this._previewPanel.BackColor = SystemColors.ControlDark;
            this._previewPanel.DockPadding.All = 1;
            this._previewPanel.Location = new Point(8, 0x80);
            this._previewPanel.Size = new Size(0x1b4, 60);
            this._previewPanel.TabIndex = 7;
            this._previewLabel.BackColor = Color.White;
            this._previewLabel.Dock = DockStyle.Fill;
            this._previewLabel.Name = "_previewLabel";
            this._previewLabel.TabIndex = 0;
            this._previewLabel.Text = "Sample Text\n1234567890";
            this._previewLabel.TextAlign = ContentAlignment.MiddleCenter;
            this._sampleLabel.Location = new Point(8, 0x6c);
            this._sampleLabel.Name = "_sampleLabel";
            this._sampleLabel.Size = new Size(0x1b4, 0x10);
            this._sampleLabel.TabIndex = 6;
            this._sampleLabel.Text = "Sample";
            this._previewPanel.Controls.AddRange(new Control[] { this._previewLabel });
            base.Controls.AddRange(new Control[] { this._sampleLabel, this._previewPanel, this._fontComboBox, this._fontSizeComboBox, this._sizeLabel, this._fontFaceComboBox, this._fontFaceLabel, this._fontLabel });
            base.Name = "FontOptionsPage";
            base.Size = new Size(0x1c4, 0x10c);
            base.ResumeLayout(false);
        }

        private void OnFontComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._internalChange && (this._fontComboBox.SelectedItem != null))
            {
                FontInfo selectedItem = (FontInfo) this._fontComboBox.SelectedItem;
                this._internalChange = true;
                try
                {
                    this._fontFaceComboBox.ShowFixedFontsOnly = selectedItem.OnlyFixed;
                    this._fontFaceComboBox.SelectedItem = selectedItem.FontFace;
                    this._fontSizeComboBox.Text = ((int) selectedItem.FontSize).ToString();
                }
                finally
                {
                    this._internalChange = false;
                }
                this.UpdatePreview();
            }
        }

        private void OnFontFaceSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._internalChange && (this._fontComboBox.SelectedItem != null))
            {
                FontInfo selectedItem = (FontInfo) this._fontComboBox.SelectedItem;
                selectedItem.FontFace = this._fontFaceComboBox.Text;
                this.UpdatePreview();
                this._dirty = true;
            }
        }

        private void OnFontSizeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._internalChange && (this._fontComboBox.SelectedItem != null))
            {
                FontInfo selectedItem = (FontInfo) this._fontComboBox.SelectedItem;
                selectedItem.FontSize = int.Parse((string) this._fontSizeComboBox.SelectedItem);
                this.UpdatePreview();
                this._dirty = true;
            }
        }

        private void OnFontSizeComboBoxTextChanged(object sender, EventArgs e)
        {
            if (!this._internalChange && (this._fontComboBox.SelectedItem != null))
            {
                FontInfo selectedItem = (FontInfo) this._fontComboBox.SelectedItem;
                string s = this._fontSizeComboBox.Text.Trim();
                if (s.Length != 0)
                {
                    int num = 0;
                    try
                    {
                        num = int.Parse(s);
                    }
                    catch
                    {
                    }
                    if (num > 0)
                    {
                        selectedItem.FontSize = num;
                        this.UpdatePreview();
                        this._dirty = true;
                    }
                    else
                    {
                        this._internalChange = true;
                        try
                        {
                            this._fontSizeComboBox.Text = ((int) selectedItem.FontSize).ToString();
                        }
                        finally
                        {
                            this._internalChange = false;
                        }
                    }
                }
            }
        }

        protected override void OnInitialVisibleChanged(EventArgs e)
        {
            base.OnInitialVisibleChanged(e);
            Application.DoEvents();
            this._fontFaceComboBox.EnumerateFonts();
            this._fontComboBox.SelectedIndex = 0;
        }

        private void UpdatePreview()
        {
            if (this._fontComboBox.SelectedItem != null)
            {
                FontInfo selectedItem = (FontInfo) this._fontComboBox.SelectedItem;
                try
                {
                    this._previewLabel.Font = new Font(selectedItem.FontFace, selectedItem.FontSize);
                    this._previewLabel.Refresh();
                }
                catch (Exception)
                {
                }
            }
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
                return @"Text Editor\Fonts";
            }
        }

        private Microsoft.Matrix.UIComponents.SourceEditing.TextManager TextManager
        {
            get
            {
                return this._textManager;
            }
        }

        private abstract class FontInfo
        {
            public string _displayName;
            private string _newFace;
            private float _newSize;
            private bool _onlyFixed;
            private string _originalFace;
            private float _originalSize;
            private FontOptionsPage _owner;

            public FontInfo(Font originalFont, string displayName, bool onlyFixed, FontOptionsPage owner)
            {
                this._displayName = displayName;
                this._onlyFixed = onlyFixed;
                this._owner = owner;
                this._originalFace = originalFont.Name;
                this._originalSize = originalFont.Size;
                this._newFace = this._originalFace;
                this._newSize = this._originalSize;
            }

            public abstract void Save();
            public override string ToString()
            {
                return this._displayName;
            }

            public string DisplayName
            {
                get
                {
                    return this._displayName;
                }
            }

            public string FontFace
            {
                get
                {
                    return this._newFace;
                }
                set
                {
                    this._newFace = value;
                }
            }

            public float FontSize
            {
                get
                {
                    return this._newSize;
                }
                set
                {
                    this._newSize = value;
                }
            }

            public bool HasChanged
            {
                get
                {
                    if (this._originalSize == this._newSize)
                    {
                        return (this._originalFace != this._newFace);
                    }
                    return true;
                }
            }

            public bool OnlyFixed
            {
                get
                {
                    return this._onlyFixed;
                }
            }

            public FontOptionsPage Owner
            {
                get
                {
                    return this._owner;
                }
            }
        }

        private class PrintFontInfo : FontOptionsPage.FontInfo
        {
            public PrintFontInfo(FontOptionsPage owner) : base(owner.TextManager.PrintFont, "Print", true, owner)
            {
            }

            public override void Save()
            {
                base.Owner.TextManager.PrintFont = new Font(base.FontFace, base.FontSize);
            }
        }

        private class SourceFontInfo : FontOptionsPage.FontInfo
        {
            public SourceFontInfo(FontOptionsPage owner) : base(owner.TextManager.SourceFont, "Source code", true, owner)
            {
            }

            public override void Save()
            {
                base.Owner.TextManager.SourceFont = new Font(base.FontFace, base.FontSize);
            }
        }
    }
}

