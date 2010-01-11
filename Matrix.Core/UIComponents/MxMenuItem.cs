namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Windows.Forms;

    public class MxMenuItem : MenuItem
    {
        private MenuItemCommand _command;
        private Image _disabledGlyph;
        private Image _glyph;
        private string _helpText;
        private string _shortcutText;
        internal static readonly MxMenuItem CloseMenuItem;
        private const int DisabledOpacity = 120;
        private const int GlyphHeight = 0x12;
        private const int GlyphWidth = 0x12;
        private const int HighlightOpacity1 = 120;
        private const int HighlightOpacity2 = 0x4b;
        private const int HorizontalPadding = 2;
        private const int Image_CheckGlyph = 0;
        private const int Image_CloseGlyph = 4;
        private const int Image_MaximizeGlyph = 3;
        private const int Image_MinimizeGlyph = 2;
        private const int Image_RestoreGlyph = 1;
        internal static readonly MxMenuItem MaximizeMenuItem;
        internal static readonly MxMenuItem MinimizeMenuItem;
        internal static readonly MxMenuItem MoveMenuItem;
        internal static readonly MxMenuItem RestoreMenuItem;
        private const int SeparatorHeight = 8;
        internal static readonly MxMenuItem SeparatorMenuItem;
        private static int sharedMaxMenuShortcutWidth;
        private static int sharedMaxMenuTextWidth;
        internal static readonly MxMenuItem SizeMenuItem;
        private static ImageList stockGlyphs = new ImageList();
        private const int VerticalPadding = 1;

        static MxMenuItem()
        {
            stockGlyphs.ImageSize = new Size(0x10, 0x10);
            stockGlyphs.TransparentColor = Color.Fuchsia;
            ImageList.ImageCollection images = stockGlyphs.Images;
            images.AddStrip(new Bitmap(typeof(MxMenuItem), "StockMenuItemGlyphs.bmp"));
            RestoreMenuItem = new MxMenuItem("&Restore", string.Empty, null, images[1]);
            MoveMenuItem = new MxMenuItem("&Move");
            SizeMenuItem = new MxMenuItem("&Size");
            MinimizeMenuItem = new MxMenuItem("Mi&nimize", string.Empty, null, stockGlyphs.Images[2]);
            MaximizeMenuItem = new MxMenuItem("Ma&ximize", string.Empty, null, stockGlyphs.Images[3]);
            CloseMenuItem = new MxMenuItem("&Close", string.Empty, null, stockGlyphs.Images[4]);
            SeparatorMenuItem = new MxMenuItem("-");
        }

        public MxMenuItem() : this(string.Empty)
        {
        }

        public MxMenuItem(string text) : base(MenuMerge.Add, 0, Shortcut.None, text, null, null, null, null)
        {
            this._helpText = string.Empty;
            base.OwnerDraw = true;
        }

        public MxMenuItem(string text, MenuItem[] items) : this(text, items, true)
        {
        }

        public MxMenuItem(string text, MenuItem[] items, bool ownerDraw) : base(MenuMerge.Add, 0, Shortcut.None, text, null, null, null, items)
        {
            this._helpText = string.Empty;
            base.OwnerDraw = ownerDraw;
        }

        public MxMenuItem(string text, string helpText, Image glyph) : base(MenuMerge.Add, 0, Shortcut.None, text, null, null, null, null)
        {
            this._helpText = helpText;
            this._glyph = glyph;
            base.OwnerDraw = true;
        }

        public MxMenuItem(string text, string helpText, EventHandler clickHandler) : base(MenuMerge.Add, 0, Shortcut.None, text, clickHandler, null, null, null)
        {
            this._helpText = helpText;
            base.OwnerDraw = true;
        }

        public MxMenuItem(string text, string helpText, Shortcut shortcut) : base(MenuMerge.Add, 0, shortcut, text, null, null, null, null)
        {
            this._helpText = helpText;
            if (shortcut != Shortcut.None)
            {
                this._shortcutText = TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString((Keys) shortcut);
            }
            base.OwnerDraw = true;
        }

        public MxMenuItem(string text, string helpText, EventHandler clickHandler, Image glyph) : base(MenuMerge.Add, 0, Shortcut.None, text, clickHandler, null, null, null)
        {
            this._helpText = helpText;
            this._glyph = glyph;
            base.OwnerDraw = true;
        }

        public MxMenuItem(string text, string helpText, EventHandler clickHandler, Shortcut shortcut) : base(MenuMerge.Add, 0, shortcut, text, clickHandler, null, null, null)
        {
            this._helpText = helpText;
            if (shortcut != Shortcut.None)
            {
                this._shortcutText = TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString((Keys) shortcut);
            }
            base.OwnerDraw = true;
        }

        public MxMenuItem(string text, string helpText, Shortcut shortcut, Image glyph) : base(MenuMerge.Add, 0, shortcut, text, null, null, null, null)
        {
            this._helpText = helpText;
            if (shortcut != Shortcut.None)
            {
                this._shortcutText = TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString((Keys) shortcut);
            }
            this._glyph = glyph;
            base.OwnerDraw = true;
        }

        public MxMenuItem(string text, string helpText, EventHandler clickHandler, Shortcut shortcut, Image glyph) : base(MenuMerge.Add, 0, shortcut, text, clickHandler, null, null, null)
        {
            this._helpText = helpText;
            if (shortcut != Shortcut.None)
            {
                this._shortcutText = TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString((Keys) shortcut);
            }
            this._glyph = glyph;
            base.OwnerDraw = true;
        }

        private void DrawCheckBackground(Graphics g, Rectangle bounds)
        {
            Pen controlDark = SystemPens.ControlDark;
            Pen controlLightLight = SystemPens.ControlLightLight;
            Bitmap bitmap = new Bitmap(0x10, 0x10);
            for (int i = 0; i < 0x10; i += 2)
            {
                for (int j = 0; j < 0x10; j += 2)
                {
                    bitmap.SetPixel(i, j, SystemColors.ControlLightLight);
                }
            }
            Brush brush = new TextureBrush(bitmap);
            g.FillRectangle(SystemBrushes.Menu, bounds);
            g.FillRectangle(brush, bounds);
            g.DrawLine(controlDark, bounds.X, bounds.Y, (bounds.X + bounds.Width) - 1, bounds.Y);
            g.DrawLine(controlDark, bounds.X, bounds.Y, bounds.X, (bounds.Y + bounds.Height) - 1);
            g.DrawLine(controlLightLight, bounds.X, (bounds.Y + bounds.Height) - 1, (bounds.X + bounds.Width) - 1, (bounds.Y + bounds.Height) - 1);
            g.DrawLine(controlLightLight, (int) ((bounds.X + bounds.Width) - 1), (int) ((bounds.Y + bounds.Height) - 1), (int) ((bounds.X + bounds.Width) - 1), (int) (bounds.Y + 1));
            bitmap.Dispose();
            brush.Dispose();
        }

        protected virtual void DrawGlyph(Graphics g, Rectangle glyphBounds, bool enabled)
        {
            Image image = this._glyph;
            if ((image == null) && base.Checked)
            {
                image = stockGlyphs.Images[0];
            }
            if (image != null)
            {
                if (enabled)
                {
                    g.DrawImageUnscaled(image, glyphBounds.X, glyphBounds.Y, 0x10, 0x10);
                }
                else
                {
                    if (this._disabledGlyph == null)
                    {
                        this._disabledGlyph = ImageUtility.CreateDisabledImage(image);
                    }
                    g.DrawImageUnscaled(this._disabledGlyph, glyphBounds.X, glyphBounds.Y, 0x10, 0x10);
                }
            }
        }

        internal void OnDrawItem(Interop.DRAWITEMSTRUCT dis)
        {
            Graphics graphics = Graphics.FromHdc(dis.hDC);
            DrawItemEventArgs e = new DrawItemEventArgs(graphics, Control.DefaultFont, Rectangle.FromLTRB(dis.rcItem.left, dis.rcItem.top, dis.rcItem.right, dis.rcItem.bottom), dis.itemID, (DrawItemState) dis.itemState);
            this.OnDrawItem(e);
            graphics.Dispose();
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            bool isTopLevel = this.IsTopLevel;
            if (isTopLevel)
            {
                g.FillRectangle(MxTheme.MenuBarBrush, e.Bounds);
            }
            else
            {
                g.FillRectangle(SystemBrushes.Menu, e.Bounds);
            }
            if (this.IsSeparator)
            {
                Pen pen = new Pen(Color.FromArgb(120, SystemColors.MenuText));
                int num = (e.Bounds.X + 2) + 0x12;
                int num2 = (e.Bounds.X + e.Bounds.Width) - 2;
                int num3 = e.Bounds.Y + (e.Bounds.Height / 2);
                g.DrawLine(pen, num, num3, num2, num3);
                pen.Dispose();
            }
            else
            {
                Brush brush3;
                bool flag2 = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                bool flag3 = (e.State & DrawItemState.HotLight) == DrawItemState.HotLight;
                bool enabled = (e.State & (DrawItemState.Inactive | DrawItemState.Disabled | DrawItemState.Grayed)) == DrawItemState.None;
                Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
                if (enabled)
                {
                    if (flag2 || (isTopLevel && flag3))
                    {
                        Brush brush = new SolidBrush(Color.FromArgb(120, SystemColors.Highlight));
                        Brush brush2 = new SolidBrush(Color.FromArgb(0x4b, Color.White));
                        g.FillRectangle(brush, rect);
                        g.FillRectangle(brush2, rect);
                        g.DrawRectangle(SystemPens.Highlight, rect);
                        brush.Dispose();
                        brush2.Dispose();
                    }
                }
                else if (flag2)
                {
                    g.DrawRectangle(SystemPens.Highlight, rect);
                }
                Font menuFont = this.MenuFont;
                Rectangle bounds = e.Bounds;
                StringFormat format = new StringFormat(StringFormatFlags.NoWrap);
                if (enabled)
                {
                    brush3 = new SolidBrush(SystemColors.MenuText);
                }
                else
                {
                    brush3 = new SolidBrush(Color.FromArgb(120, SystemColors.MenuText));
                }
                if (!isTopLevel)
                {
                    int num4 = 0x16;
                    bounds = new Rectangle(bounds.X + num4, bounds.Y + 1, (bounds.Width - num4) - 4, bounds.Height - 2);
                }
                else
                {
                    format.Alignment = StringAlignment.Center;
                }
                if ((e.State & DrawItemState.NoAccelerator) == DrawItemState.NoAccelerator)
                {
                    format.HotkeyPrefix = HotkeyPrefix.Hide;
                }
                else
                {
                    format.HotkeyPrefix = HotkeyPrefix.Show;
                }
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(base.Text, menuFont, brush3, bounds, format);
                if (!isTopLevel)
                {
                    if (this._shortcutText != null)
                    {
                        format.Alignment = StringAlignment.Far;
                        format.HotkeyPrefix = HotkeyPrefix.Hide;
                        g.DrawString(this._shortcutText, menuFont, brush3, bounds, format);
                    }
                    Rectangle rectangle3 = new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 1, 0x12, 0x12);
                    if (base.Checked && enabled)
                    {
                        this.DrawCheckBackground(g, rectangle3);
                    }
                    rectangle3.Inflate(-1, -1);
                    this.DrawGlyph(g, rectangle3, enabled);
                }
                format.Dispose();
                brush3.Dispose();
            }
        }

        internal void OnMeasureItem(Interop.MEASUREITEMSTRUCT mis)
        {
            IntPtr dC = Interop.GetDC(IntPtr.Zero);
            Graphics graphics = Graphics.FromHdc(dC);
            MeasureItemEventArgs e = new MeasureItemEventArgs(graphics, 0);
            this.OnMeasureItem(e);
            graphics.Dispose();
            Interop.ReleaseDC(IntPtr.Zero, dC);
            mis.itemWidth = e.ItemWidth;
            mis.itemHeight = e.ItemHeight;
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            if (this.IsSeparator)
            {
                e.ItemWidth = 4;
                e.ItemHeight = 8;
            }
            else
            {
                if (e.Index == 0)
                {
                    sharedMaxMenuTextWidth = 0;
                    sharedMaxMenuShortcutWidth = 0;
                }
                Font menuFont = this.MenuFont;
                StringFormat stringFormat = new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
                Size size = e.Graphics.MeasureString(base.Text, menuFont, new PointF(0f, 0f), stringFormat).ToSize();
                e.ItemHeight = Math.Max(0x12, size.Height) + 2;
                if (!this.IsTopLevel)
                {
                    sharedMaxMenuTextWidth = Math.Max(sharedMaxMenuTextWidth, size.Width);
                    if (this._shortcutText != null)
                    {
                        size = e.Graphics.MeasureString(this._shortcutText, menuFont, new PointF(0f, 0f), stringFormat).ToSize();
                        sharedMaxMenuShortcutWidth = Math.Max(sharedMaxMenuShortcutWidth, size.Width);
                    }
                    e.ItemWidth = ((sharedMaxMenuTextWidth + sharedMaxMenuShortcutWidth) + 0x12) + 10;
                }
                else
                {
                    e.ItemWidth = size.Width - 10;
                }
                stringFormat.Dispose();
            }
        }

        internal MenuItemCommand Command
        {
            get
            {
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }

        public Image Glyph
        {
            get
            {
                return this._glyph;
            }
            set
            {
                this._glyph = value;
                this._disabledGlyph = null;
            }
        }

        public string HelpText
        {
            get
            {
                return this._helpText;
            }
            set
            {
                this._helpText = value;
            }
        }

        private bool IsSeparator
        {
            get
            {
                return (base.Text == "-");
            }
        }

        private bool IsTopLevel
        {
            get
            {
                return ((base.Parent == base.GetMainMenu()) && (base.Parent != null));
            }
        }

        private Font MenuFont
        {
            get
            {
                Font menuFont = null;
                MainMenu mainMenu = base.GetMainMenu();
                if (mainMenu != null)
                {
                    return mainMenu.GetForm().Font;
                }
                menuFont = SystemInformation.MenuFont;
                if (menuFont == null)
                {
                    menuFont = new Font("Tahoma", 8f);
                }
                return menuFont;
            }
        }
    }
}

