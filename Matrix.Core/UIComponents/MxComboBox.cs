namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MxComboBox : ComboBox
    {
        private bool _alwaysShowFocusCues;
        private bool _flatAppearance;
        private string _initialText;
        private bool _mouseHover;
        private bool _showInitialText;
        private ChildTextBoxWindow _textBoxWindow;
        private static readonly object EventCloseDropDown = new object();
        private static readonly object EventTextBoxKeyPress = new object();
        private static readonly object EventTextUpdated = new object();

        public event EventHandler CloseDropDown
        {
            add
            {
                base.Events.AddHandler(EventCloseDropDown, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventCloseDropDown, value);
            }
        }

        public event KeyPressEventHandler TextBoxKeyPress
        {
            add
            {
                base.Events.AddHandler(EventTextBoxKeyPress, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventTextBoxKeyPress, value);
            }
        }

        public event EventHandler TextUpdated
        {
            add
            {
                base.Events.AddHandler(EventTextUpdated, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventTextUpdated, value);
            }
        }

        private void DrawFlatCombo()
        {
            if (!MxTheme.IsAppThemed || !this._alwaysShowFocusCues)
            {
                Interop.RECT rect = new Interop.RECT();
                Interop.GetClientRect(base.Handle, ref rect);
                IntPtr dC = Interop.GetDC(base.Handle);
                try
                {
                    int num5;
                    int num6;
                    bool flag = ((this._alwaysShowFocusCues || this._mouseHover) || base.ContainsFocus) && base.Enabled;
                    int horizontalScrollBarArrowWidth = SystemInformation.HorizontalScrollBarArrowWidth;
                    int sysColor = Interop.GetSysColor(15);
                    int bottomRightColor = Interop.GetSysColor(0x10);
                    int topLeftColor = Interop.GetSysColor(20);
                    if (flag)
                    {
                        num5 = bottomRightColor;
                        num6 = topLeftColor;
                    }
                    else
                    {
                        num5 = num6 = sysColor;
                    }
                    this.DrawRectangle(dC, rect.left, rect.top, rect.Width, rect.Height, num5, num6);
                    if ((MxTheme.IsAppThemed && !base.Enabled) && (base.DropDownStyle == ComboBoxStyle.DropDown))
                    {
                        this.DrawRectangle(dC, rect.left + 2, rect.top + 2, (rect.Width - horizontalScrollBarArrowWidth) - 4, rect.Height - 4, sysColor, sysColor);
                    }
                    rect.left++;
                    rect.top++;
                    rect.right--;
                    rect.bottom--;
                    if (base.Enabled)
                    {
                        num5 = num6 = sysColor;
                    }
                    else
                    {
                        num5 = num6 = topLeftColor;
                    }
                    this.DrawRectangle(dC, rect.left, rect.top, rect.Width, rect.Height, num5, num6);
                    rect.left++;
                    rect.top++;
                    rect.right--;
                    rect.bottom--;
                    rect.left = rect.right - horizontalScrollBarArrowWidth;
                    this.DrawRectangle(dC, rect.left, rect.top, rect.Width, rect.Height, sysColor, sysColor);
                    rect.left++;
                    rect.top++;
                    rect.right--;
                    rect.bottom--;
                    this.DrawRectangle(dC, rect.left, rect.top, rect.Width, rect.Height, sysColor, sysColor);
                    Interop.FillRect(dC, ref rect, (IntPtr) 0x10);
                    this.DrawFlatComboDropDown(dC, ref rect);
                    if (base.Enabled)
                    {
                        if (flag)
                        {
                            rect.top--;
                            rect.bottom++;
                            rect.right++;
                            this.DrawRectangle(dC, rect.left, rect.top, rect.Width, rect.Height, topLeftColor, bottomRightColor);
                        }
                        else
                        {
                            rect.top--;
                            rect.bottom++;
                            this.DrawRectangle(dC, rect.left, rect.top, rect.Width, rect.Height, topLeftColor, topLeftColor);
                            rect.left--;
                            this.DrawRectangle(dC, rect.left, rect.top, rect.Width, rect.Height, topLeftColor, topLeftColor);
                        }
                    }
                }
                finally
                {
                    Interop.ReleaseDC(base.Handle, dC);
                }
            }
        }

        private void DrawFlatComboDropDown(IntPtr hdc, ref Interop.RECT dropDownRect)
        {
            int num4;
            int color = base.Enabled ? Interop.GetSysColor(0x12) : Interop.GetSysColor(0x10);
            int x = (((dropDownRect.Width - 5) / 2) + dropDownRect.left) + 1;
            int y = ((dropDownRect.Height - 3) / 2) + dropDownRect.top;
            for (num4 = 0; num4 < 5; num4++)
            {
                Interop.SetPixel(hdc, x + num4, y, color);
            }
            x++;
            y++;
            for (num4 = 0; num4 < 3; num4++)
            {
                Interop.SetPixel(hdc, x + num4, y, color);
            }
            x++;
            y++;
            Interop.SetPixel(hdc, x, y, color);
        }

        private void DrawRectangle(IntPtr hdc, int x, int y, int cx, int cy, int topLeftColor, int bottomRightColor)
        {
            Interop.RECT lpRect = new Interop.RECT();
            Interop.SetBkColor(hdc, topLeftColor);
            lpRect.left = x;
            lpRect.top = y;
            lpRect.right = (cx - 1) + x;
            lpRect.bottom = 1 + y;
            Interop.ExtTextOut(hdc, 0, 0, 2, ref lpRect, null, 0, null);
            lpRect.left = x;
            lpRect.top = y;
            lpRect.right = 1 + x;
            lpRect.bottom = (cy - 1) + y;
            Interop.ExtTextOut(hdc, 0, 0, 2, ref lpRect, null, 0, null);
            Interop.SetBkColor(hdc, bottomRightColor);
            lpRect.left = (x + cx) - 1;
            lpRect.top = y;
            lpRect.right = x + cx;
            lpRect.bottom = cy + y;
            Interop.ExtTextOut(hdc, 0, 0, 2, ref lpRect, null, 0, null);
            lpRect.left = x;
            lpRect.top = (y + cy) - 1;
            lpRect.right = x + cx;
            lpRect.bottom = cy + y;
            Interop.ExtTextOut(hdc, 0, 0, 2, ref lpRect, null, 0, null);
        }

        protected virtual void OnCloseDropDown(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[EventCloseDropDown];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (this._flatAppearance)
            {
                if (this._mouseHover && !base.Enabled)
                {
                    this._mouseHover = false;
                }
                this.DrawFlatCombo();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (this._flatAppearance && !this._alwaysShowFocusCues)
            {
                this.DrawFlatCombo();
            }
            this.UpdateShowInitialText(false);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (base.DropDownStyle == ComboBoxStyle.DropDown)
            {
                IntPtr window = Interop.GetWindow(base.Handle, 5);
                if (window != IntPtr.Zero)
                {
                    this._textBoxWindow = new ChildTextBoxWindow(this);
                    this._textBoxWindow.AssignHandle(window);
                }
            }
            this.UpdateShowInitialText();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (this._textBoxWindow != null)
            {
                this._textBoxWindow.ReleaseHandle();
                this._textBoxWindow = null;
            }
            base.OnHandleDestroyed(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (this._flatAppearance && !this._alwaysShowFocusCues)
            {
                this.DrawFlatCombo();
            }
            this.UpdateShowInitialText();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (this._flatAppearance)
            {
                this._mouseHover = true;
                if (!this._alwaysShowFocusCues)
                {
                    this.DrawFlatCombo();
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (this._flatAppearance)
            {
                this._mouseHover = false;
                if (!this._alwaysShowFocusCues)
                {
                    this.DrawFlatCombo();
                }
            }
        }

        protected virtual void OnTextBoxKeyPress(KeyPressEventArgs e)
        {
            KeyPressEventHandler handler = (KeyPressEventHandler) base.Events[EventTextBoxKeyPress];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnTextUpdated(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[EventTextUpdated];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void UpdateShowInitialText()
        {
            bool show = (this.InitialText.Length != 0) && (base.Text.Length == 0);
            this.UpdateShowInitialText(show);
        }

        private void UpdateShowInitialText(bool show)
        {
            if ((base.DropDownStyle == ComboBoxStyle.DropDown) && (show != this._showInitialText))
            {
                if (show)
                {
                    base.Text = this.InitialText;
                    this.ForeColor = SystemColors.ControlDark;
                }
                else
                {
                    if (base.Text.Equals(this.InitialText))
                    {
                        base.Text = string.Empty;
                        Interop.SetWindowText(this._textBoxWindow.Handle, string.Empty);
                    }
                    this.ForeColor = SystemColors.ControlText;
                }
                this._showInitialText = show;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if ((!MxTheme.IsAppThemed || !this._alwaysShowFocusCues) && this._flatAppearance)
            {
                if (m.Msg == 15)
                {
                    this.DefWndProc(ref m);
                    this.DrawFlatCombo();
                    return;
                }
                if ((m.Msg == 0x20) && (base.DropDownStyle != ComboBoxStyle.DropDownList))
                {
                    Interop.RECT rect = new Interop.RECT();
                    Interop.GetWindowRect(base.Handle, ref rect);
                    int messagePos = Interop.GetMessagePos();
                    if (Interop.PtInRect(ref rect, messagePos & 0xffff, messagePos >> 0x10))
                    {
                        this._mouseHover = true;
                        this.DrawFlatCombo();
                    }
                }
            }
            if (m.Msg == 0x2111)
            {
                switch ((((int) m.WParam) >> 0x10))
                {
                    case 6:
                        this.OnTextUpdated(EventArgs.Empty);
                        break;

                    case 8:
                        this.OnCloseDropDown(EventArgs.Empty);
                        break;
                }
            }
            base.WndProc(ref m);
        }

        public bool AlwaysShowFocusCues
        {
            get
            {
                return this._alwaysShowFocusCues;
            }
            set
            {
                if (value != this._alwaysShowFocusCues)
                {
                    this._alwaysShowFocusCues = value;
                    if (base.IsHandleCreated)
                    {
                        base.Invalidate();
                    }
                }
            }
        }

        public bool FlatAppearance
        {
            get
            {
                return this._flatAppearance;
            }
            set
            {
                if (value != this._flatAppearance)
                {
                    this._flatAppearance = value;
                    if (base.IsHandleCreated)
                    {
                        base.Invalidate();
                    }
                }
            }
        }

        [DefaultValue(""), Category("Appearance")]
        public string InitialText
        {
            get
            {
                if (this._initialText == null)
                {
                    return string.Empty;
                }
                return this._initialText;
            }
            set
            {
                this._initialText = value;
            }
        }

        public override string Text
        {
            get
            {
                if (this._showInitialText)
                {
                    return string.Empty;
                }
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.UpdateShowInitialText();
            }
        }

        private class ChildTextBoxWindow : NativeWindow
        {
            private MxComboBox _owner;

            public ChildTextBoxWindow(MxComboBox owner)
            {
                this._owner = owner;
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 0x102)
                {
                    KeyPressEventArgs e = new KeyPressEventArgs((char) ((int) m.WParam));
                    this._owner.OnTextBoxKeyPress(e);
                    if (e.Handled)
                    {
                        return;
                    }
                }
                base.WndProc(ref m);
            }
        }
    }
}

