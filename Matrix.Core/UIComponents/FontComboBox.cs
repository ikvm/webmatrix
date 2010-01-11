namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    public class FontComboBox : ToolBarComboBox
    {
        private bool _deferFontEnumeration;
        private bool _showFixedFontsOnly;
        private bool _showFontPreview = true;

        public FontComboBox()
        {
            base.DrawMode = DrawMode.OwnerDrawFixed;
            base.MaxDropDownItems = 10;
        }

        public void EnumerateFonts()
        {
            this.InitializeFontList();
        }

        private void InitializeFontList()
        {
            string text = this.Text;
            base.Items.Clear();
            object[] array = null;
            Hashtable hashtable = new Hashtable();
            FontFamily[] families = FontFamily.Families;
            if (this._showFixedFontsOnly)
            {
                Graphics graphics = base.CreateGraphics();
                IntPtr hdc = graphics.GetHdc();
                try
                {
                    for (int i = 0; i < families.Length; i++)
                    {
                        try
                        {
                            string name = families[i].Name;
                            if (!hashtable.Contains(name.ToLower()))
                            {
                                IntPtr hObject = new Font(name, 8f).ToHfont();
                                IntPtr ptr3 = Interop.SelectObject(hdc, hObject);
                                try
                                {
                                    Interop.TEXTMETRIC textMetric = new Interop.TEXTMETRIC();
                                    Interop.GetTextMetrics(hdc, textMetric);
                                    if ((textMetric.tmPitchAndFamily & 1) == 0)
                                    {
                                        hashtable[name.ToLower()] = families[i].Name;
                                    }
                                }
                                finally
                                {
                                    Interop.SelectObject(hdc, ptr3);
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                finally
                {
                    graphics.ReleaseHdc(hdc);
                    graphics.Dispose();
                }
            }
            else
            {
                for (int j = 0; j < families.Length; j++)
                {
                    hashtable[families[j].Name.ToLower()] = families[j].Name;
                }
            }
            array = new object[hashtable.Count];
            hashtable.Values.CopyTo(array, 0);
            Array.Sort(array);
            base.Items.AddRange(array);
            if (text.Length != 0)
            {
                this.Text = text;
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (!base.Enabled)
            {
                e.Graphics.FillRectangle(SystemBrushes.Control, e.Bounds);
            }
            else
            {
                e.DrawBackground();
                string familyName = null;
                if (e.Index == -1)
                {
                    familyName = this.Text;
                }
                else
                {
                    familyName = (string) base.Items[e.Index];
                }
                if (this._showFontPreview)
                {
                    Rectangle rect = new Rectangle(e.Bounds.Left + 1, e.Bounds.Top, 0x12, e.Bounds.Height);
                    Font font = null;
                    if ((familyName != null) && (familyName.Length != 0))
                    {
                        try
                        {
                            font = new Font(familyName, (float) (((double) (rect.Height - 2)) / 1.2), FontStyle.Regular, GraphicsUnit.Pixel);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    Brush menu = SystemBrushes.Menu;
                    Pen controlDark = SystemPens.ControlDark;
                    Brush controlText = SystemBrushes.ControlText;
                    e.Graphics.FillRectangle(menu, rect);
                    StringFormat format = new StringFormat(StringFormatFlags.NoWrap);
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    format.Trimming = StringTrimming.Character;
                    if (font != null)
                    {
                        e.Graphics.DrawString("ab", font, controlText, rect, format);
                    }
                    e.Graphics.DrawRectangle(controlDark, new Rectangle(rect.Left, rect.Top, rect.Width, rect.Height - 2));
                    Rectangle layoutRectangle = new Rectangle(e.Bounds.Left + 0x15, e.Bounds.Top, e.Bounds.Width - 0x15, e.Bounds.Height);
                    Brush brush = new SolidBrush(e.ForeColor);
                    format.Alignment = StringAlignment.Near;
                    format.Trimming = StringTrimming.None;
                    e.Graphics.DrawString(familyName, e.Font, brush, layoutRectangle, format);
                    format.Dispose();
                    brush.Dispose();
                    if (font != null)
                    {
                        font.Dispose();
                    }
                }
                else
                {
                    StringFormat format2 = new StringFormat(StringFormatFlags.NoWrap);
                    format2.Alignment = StringAlignment.Near;
                    format2.LineAlignment = StringAlignment.Center;
                    format2.Trimming = StringTrimming.None;
                    Brush brush4 = new SolidBrush(e.ForeColor);
                    e.Graphics.DrawString(familyName, this.Font, brush4, e.Bounds, format2);
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!this._deferFontEnumeration)
            {
                this.InitializeFontList();
            }
            SystemEvents.InstalledFontsChanged += new EventHandler(this.OnInstalledFontsChanged);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            SystemEvents.InstalledFontsChanged -= new EventHandler(this.OnInstalledFontsChanged);
            base.OnHandleDestroyed(e);
        }

        private void OnInstalledFontsChanged(object sender, EventArgs e)
        {
            this.InitializeFontList();
        }

        public bool DeferFontEnumeration
        {
            get
            {
                return this._deferFontEnumeration;
            }
            set
            {
                this._deferFontEnumeration = value;
            }
        }

        public bool ShowFixedFontsOnly
        {
            get
            {
                return this._showFixedFontsOnly;
            }
            set
            {
                if (this._showFixedFontsOnly != value)
                {
                    this._showFixedFontsOnly = value;
                    if (base.IsHandleCreated)
                    {
                        this.InitializeFontList();
                    }
                }
            }
        }

        public bool ShowFontPreview
        {
            get
            {
                return this._showFontPreview;
            }
            set
            {
                if (this._showFontPreview != value)
                {
                    this._showFontPreview = value;
                    if (base.IsHandleCreated)
                    {
                        base.Invalidate();
                    }
                }
            }
        }
    }
}

