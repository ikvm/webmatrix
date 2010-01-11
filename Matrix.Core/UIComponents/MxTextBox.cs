namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class MxTextBox : TextBox
    {
        private bool _alwaysShowFocusCues;
        private bool _flatAppearance;
        private bool _mouseHover;
        private bool _numeric;
        private bool _passwordStyle;

        private void DrawFlatTextBox()
        {
            if (!MxTheme.IsAppThemed)
            {
                Interop.RECT rect = new Interop.RECT();
                Interop.GetWindowRect(base.Handle, ref rect);
                IntPtr windowDC = Interop.GetWindowDC(base.Handle);
                try
                {
                    int num4;
                    int num5;
                    bool flag = ((this._alwaysShowFocusCues || this._mouseHover) || base.ContainsFocus) && base.Enabled;
                    int sysColor = Interop.GetSysColor(15);
                    int num2 = Interop.GetSysColor(0x10);
                    int num3 = Interop.GetSysColor(20);
                    rect.bottom -= rect.top;
                    rect.right -= rect.left;
                    rect.left = 0;
                    rect.top = 0;
                    if (flag)
                    {
                        num4 = num2;
                        num5 = num3;
                    }
                    else
                    {
                        num4 = num5 = sysColor;
                    }
                    this.DrawRectangle(windowDC, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, num4, num5);
                    rect.left++;
                    rect.top++;
                    rect.right--;
                    rect.bottom--;
                    if (base.Enabled && (flag || !base.ReadOnly))
                    {
                        num4 = num5 = sysColor;
                    }
                    else
                    {
                        num4 = num5 = num3;
                    }
                    this.DrawRectangle(windowDC, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, num4, num5);
                }
                finally
                {
                    Interop.ReleaseDC(base.Handle, windowDC);
                }
            }
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

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (this._flatAppearance)
            {
                if (this._mouseHover && !base.Enabled)
                {
                    this._mouseHover = false;
                }
                this.DrawFlatTextBox();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (this._flatAppearance && !this._alwaysShowFocusCues)
            {
                this.DrawFlatTextBox();
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (this._flatAppearance && !this._alwaysShowFocusCues)
            {
                this.DrawFlatTextBox();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (this._flatAppearance)
            {
                this._mouseHover = true;
                if (!this._alwaysShowFocusCues)
                {
                    this.DrawFlatTextBox();
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
                    this.DrawFlatTextBox();
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            if ((!MxTheme.IsAppThemed && this._flatAppearance) && ((m.Msg == 15) || (m.Msg == 0x85)))
            {
                this.DefWndProc(ref m);
                this.DrawFlatTextBox();
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        [DefaultValue(false)]
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

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                if (this._numeric)
                {
                    createParams.Style |= 0x2000;
                }
                if (this._passwordStyle)
                {
                    createParams.Style |= 0x20;
                }
                return createParams;
            }
        }

        [DefaultValue(false)]
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

        [DefaultValue(false)]
        public bool Numeric
        {
            get
            {
                return this._numeric;
            }
            set
            {
                if (value != this._numeric)
                {
                    this._numeric = value;
                    if (base.IsHandleCreated)
                    {
                        base.RecreateHandle();
                    }
                }
            }
        }

        [DefaultValue(false)]
        public bool PasswordStyle
        {
            get
            {
                return this._passwordStyle;
            }
            set
            {
                if (value != this._passwordStyle)
                {
                    this._passwordStyle = value;
                    if (base.IsHandleCreated)
                    {
                        base.RecreateHandle();
                    }
                }
            }
        }
    }
}

