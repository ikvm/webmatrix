namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Collections.Specialized;

    public class TextManager : IDisposable
    {
        private static IDictionary _colorTables = new HybridDictionary();
        private static ColorInfoTable _defaultColorTable;
        private Font _printFont;
        private IServiceProvider _serviceProvider;
        private Font _sourceFont;
        private ArrayList _textControls;
        private static ColorInfo[] defaultColors = new ColorInfo[] { new ColorInfo(SystemColors.WindowText, SystemColors.Window, false, false), new ColorInfo(Color.Blue, SystemColors.Window, false, false), new ColorInfo(Color.Navy, SystemColors.Window, false, false), new ColorInfo(Color.Green, SystemColors.Window, false, true), new ColorInfo(SystemColors.ControlDark, SystemColors.Window, false, false), new ColorInfo(SystemColors.HighlightText, SystemColors.Highlight, false, false), new ColorInfo(SystemColors.ControlDark, SystemColors.Control, false, false), new ColorInfo(SystemColors.Control, SystemColors.Control, false, false), new ColorInfo(SystemColors.ControlDark, SystemColors.ControlDark, false, false) };
        protected const string DefaultPrintFontFace = "Lucida Console";
        protected const int DefaultPrintFontSize = 8;
        protected const string DefaultSourceFontFace = "Lucida Console";
        protected const int DefaultSourceFontSize = 8;

        public TextManager(IServiceProvider provider)
        {
            this._serviceProvider = provider;
            SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(this.OnSystemColorsChanged);
        }

        protected virtual void Dispose()
        {
            SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(this.OnSystemColorsChanged);
            if (this._printFont != null)
            {
                this._printFont.Dispose();
                this._printFont = null;
            }
        }

        private void FillColorTableWithStandardColors(ColorInfoTable table)
        {
            table[0] = this.PlainTextColor;
            table[1] = this.KeywordColor;
            table[2] = this.StringColor;
            table[3] = this.CommentColor;
            table[4] = this.LineNumbersColor;
            table[5] = this.SelectedTextColor;
            table[6] = this.InactiveSelectedTextColor;
            table[7] = this.ControlColor;
            table[8] = this.ControlDarkColor;
        }

        public ColorInfoTable GetColorTable(ITextLanguage language)
        {
            if (language == null)
            {
                return this.DefaultColorTable;
            }
            ColorInfoTable table = (ColorInfoTable) _colorTables[language];
            if (table == null)
            {
                ITextColorizer colorizer = language.GetColorizer(this._serviceProvider);
                ColorInfo[] infoArray = (colorizer != null) ? colorizer.ColorTable : null;
                table = new ColorInfoTable(0x40 + ((infoArray != null) ? infoArray.Length : 0));
                this.FillColorTableWithStandardColors(table);
                if (infoArray != null)
                {
                    for (int i = 0; i < infoArray.Length; i++)
                    {
                        table[i + 0x40] = infoArray[i];
                    }
                }
                _colorTables[language] = table;
            }
            return table;
        }

        private void OnSystemColorsChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.Color)
            {
                this.PlainTextColor.RefreshWin32Colors();
                this.KeywordColor.RefreshWin32Colors();
                this.StringColor.RefreshWin32Colors();
                this.CommentColor.RefreshWin32Colors();
                this.LineNumbersColor.RefreshWin32Colors();
                this.SelectedTextColor.RefreshWin32Colors();
                this.InactiveSelectedTextColor.RefreshWin32Colors();
                this.ControlColor.RefreshWin32Colors();
                this.ControlDarkColor.RefreshWin32Colors();
            }
        }

        internal void RegisterTextControl(TextControl textControl)
        {
            if (!this.TextControlsInternal.Contains(textControl))
            {
                textControl.Font = this.SourceFont;
                this.TextControlsInternal.Add(textControl);
            }
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        internal void UnregisterTextControl(TextControl textControl)
        {
            this.TextControlsInternal.Remove(textControl);
        }

        protected virtual ColorInfo CommentColor
        {
            get
            {
                return defaultColors[3];
            }
        }

        protected virtual ColorInfo ControlColor
        {
            get
            {
                return defaultColors[7];
            }
        }

        protected virtual ColorInfo ControlDarkColor
        {
            get
            {
                return defaultColors[8];
            }
        }

        private ColorInfoTable DefaultColorTable
        {
            get
            {
                if (_defaultColorTable == null)
                {
                    _defaultColorTable = new ColorInfoTable(0x40);
                    this.FillColorTableWithStandardColors(_defaultColorTable);
                }
                return _defaultColorTable;
            }
        }

        protected virtual ColorInfo InactiveSelectedTextColor
        {
            get
            {
                return defaultColors[6];
            }
        }

        protected virtual ColorInfo KeywordColor
        {
            get
            {
                return defaultColors[1];
            }
        }

        protected virtual ColorInfo LineNumbersColor
        {
            get
            {
                return defaultColors[4];
            }
        }

        protected virtual ColorInfo PlainTextColor
        {
            get
            {
                return defaultColors[0];
            }
        }

        public Font PrintFont
        {
            get
            {
                if (this._printFont == null)
                {
                    this._printFont = new Font("Lucida Console", 8f);
                }
                return this._printFont;
            }
            set
            {
                if (this._printFont != value)
                {
                    this._printFont = value;
                }
            }
        }

        protected virtual ColorInfo SelectedTextColor
        {
            get
            {
                return defaultColors[5];
            }
        }

        protected IServiceProvider ServiceProvider
        {
            get
            {
                return this._serviceProvider;
            }
        }

        public Font SourceFont
        {
            get
            {
                if (this._sourceFont == null)
                {
                    return new Font("Lucida Console", 8f);
                }
                return this._sourceFont;
            }
            set
            {
                this._sourceFont = value;
                foreach (TextControl control in this.TextControls)
                {
                    control.Font = this._sourceFont;
                }
            }
        }

        protected virtual ColorInfo StringColor
        {
            get
            {
                return defaultColors[2];
            }
        }

        public ICollection TextControls
        {
            get
            {
                return this.TextControlsInternal;
            }
        }

        internal ArrayList TextControlsInternal
        {
            get
            {
                if (this._textControls == null)
                {
                    this._textControls = new ArrayList();
                }
                return this._textControls;
            }
        }
    }
}

