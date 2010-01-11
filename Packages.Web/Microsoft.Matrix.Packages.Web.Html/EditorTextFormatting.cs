namespace Microsoft.Matrix.Packages.Web.Html
{
    using System;
    using System.Drawing;

    public sealed class EditorTextFormatting
    {
        private HtmlEditor _editor;
        private static readonly string[] formats = new string[] { "Normal", "Formatted", "Heading 1", "Heading 2", "Heading 3", "Heading 4", "Heading 5", "Heading 6", "Paragraph", "Numbered List", "Bulleted List" };

        public EditorTextFormatting(HtmlEditor editor)
        {
            this._editor = editor;
        }

        public bool CanAlign(Alignment alignment)
        {
            return this._editor.IsCommandEnabled(this.MapAlignment(alignment));
        }

        private Color ConvertMSHTMLColor(object colorValue)
        {
            if (colorValue != null)
            {
                Type type = colorValue.GetType();
                if (type == typeof(int))
                {
                    return ColorTranslator.FromWin32((int) colorValue);
                }
                if (type == typeof(string))
                {
                    return ColorTranslator.FromHtml((string) colorValue);
                }
            }
            return Color.Empty;
        }

        public Alignment GetAlignment()
        {
            if (this._editor.IsCommandChecked(this.MapAlignment(Alignment.Center)))
            {
                return Alignment.Center;
            }
            if (!this._editor.IsCommandChecked(this.MapAlignment(Alignment.Left)) && this._editor.IsCommandChecked(this.MapAlignment(Alignment.Right)))
            {
                return Alignment.Right;
            }
            return Alignment.Left;
        }

        public CommandInfo GetBoldInfo()
        {
            return this._editor.GetCommandInfo(0x34);
        }

        public CommandInfo GetItalicsInfo()
        {
            return this._editor.GetCommandInfo(0x38);
        }

        public CommandInfo GetStrikethroughInfo()
        {
            return this._editor.GetCommandInfo(0x5b);
        }

        public CommandInfo GetSubscriptInfo()
        {
            return this._editor.GetCommandInfo(0x8c7);
        }

        public CommandInfo GetSuperscriptInfo()
        {
            return this._editor.GetCommandInfo(0x8c8);
        }

        public TextFormat GetTextFormat()
        {
            string str = this._editor.ExecResult(0x8ba) as string;
            if (str != null)
            {
                for (int i = 0; i < formats.Length; i++)
                {
                    if (str.Equals(formats[i]))
                    {
                        return (TextFormat) i;
                    }
                }
            }
            return TextFormat.Normal;
        }

        public CommandInfo GetUnderlineInfo()
        {
            return this._editor.GetCommandInfo(0x3f);
        }

        public void Indent()
        {
            this._editor.Exec(0x88a);
        }

        private int MapAlignment(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Left:
                    return 0x3b;

                case Alignment.Center:
                    return 0x39;

                case Alignment.Right:
                    return 60;
            }
            return -1;
        }

        public void SetAlignment(Alignment alignment)
        {
            this._editor.Exec(this.MapAlignment(alignment));
        }

        public void SetTextFormat(TextFormat format)
        {
            this._editor.Exec(0x8ba, formats[(int) format]);
        }

        public void ToggleBold()
        {
            this._editor.Exec(0x34);
        }

        public void ToggleItalics()
        {
            this._editor.Exec(0x38);
        }

        public void ToggleStrikethrough()
        {
            this._editor.Exec(0x5b);
        }

        public void ToggleSubscript()
        {
            this._editor.Exec(0x8c7);
        }

        public void ToggleSuperscript()
        {
            this._editor.Exec(0x8c8);
        }

        public void ToggleUnderline()
        {
            this._editor.Exec(0x3f);
        }

        public void Unindent()
        {
            this._editor.Exec(0x88b);
        }

        public Color BackColor
        {
            get
            {
                return this.ConvertMSHTMLColor(this._editor.ExecResult(0x33));
            }
            set
            {
                string argument = ColorTranslator.ToHtml(value);
                this._editor.Exec(0x33, argument);
            }
        }

        public bool CanIndent
        {
            get
            {
                return this._editor.IsCommandEnabled(0x88a);
            }
        }

        public bool CanSetBackColor
        {
            get
            {
                return this._editor.IsCommandEnabled(0x33);
            }
        }

        public bool CanSetFontName
        {
            get
            {
                return this._editor.IsCommandEnabled(0x12);
            }
        }

        public bool CanSetFontSize
        {
            get
            {
                return this._editor.IsCommandEnabled(0x13);
            }
        }

        public bool CanSetForeColor
        {
            get
            {
                return this._editor.IsCommandEnabled(0x37);
            }
        }

        public bool CanSetHtmlFormat
        {
            get
            {
                return this._editor.IsCommandEnabled(0x8ba);
            }
        }

        public bool CanUnindent
        {
            get
            {
                return this._editor.IsCommandEnabled(0x88b);
            }
        }

        public string FontName
        {
            get
            {
                return (this._editor.ExecResult(0x12) as string);
            }
            set
            {
                this._editor.Exec(0x12, value);
            }
        }

        public Microsoft.Matrix.Packages.Web.Html.FontSize FontSize
        {
            get
            {
                object obj2 = this._editor.ExecResult(0x13);
                if (obj2 == null)
                {
                    return Microsoft.Matrix.Packages.Web.Html.FontSize.Medium;
                }
                return (Microsoft.Matrix.Packages.Web.Html.FontSize) obj2;
            }
            set
            {
                this._editor.Exec(0x13, (int) value);
            }
        }

        public Color ForeColor
        {
            get
            {
                return this.ConvertMSHTMLColor(this._editor.ExecResult(0x37));
            }
            set
            {
                string argument = ColorTranslator.ToHtml(value);
                this._editor.Exec(0x37, argument);
            }
        }
    }
}

