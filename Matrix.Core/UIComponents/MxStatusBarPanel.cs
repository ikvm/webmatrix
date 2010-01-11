namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Windows.Forms;

    public class MxStatusBarPanel : StatusBarPanel
    {
        private StatusBarPanelCommand _command;
        private const int TextPadding = 3;

        public MxStatusBarPanel()
        {
            base.Style = StatusBarPanelStyle.OwnerDraw;
            base.BorderStyle = StatusBarPanelBorderStyle.None;
        }

        public virtual void DrawPanel(DrawItemEventArgs e)
        {
            string text = base.Text;
            if ((text != null) && (text.Length != 0))
            {
                int num = 0;
                StringFormat format = new StringFormat(StringFormatFlags.NoWrap);
                format.LineAlignment = StringAlignment.Center;
                format.HotkeyPrefix = HotkeyPrefix.Hide;
                format.Trimming = StringTrimming.EllipsisCharacter;
                switch (base.Alignment)
                {
                    case HorizontalAlignment.Right:
                        format.Alignment = StringAlignment.Far;
                        break;

                    case HorizontalAlignment.Center:
                        format.Alignment = StringAlignment.Center;
                        break;

                    default:
                        format.Alignment = StringAlignment.Near;
                        num = 3;
                        break;
                }
                Rectangle layoutRectangle = new Rectangle((e.Bounds.X + 1) + num, e.Bounds.Y, (e.Bounds.Width - 2) - num, e.Bounds.Height);
                e.Graphics.DrawString(base.Text, base.Parent.Font, SystemBrushes.ControlText, layoutRectangle, format);
                format.Dispose();
            }
            this.DrawPanelBorder(e);
        }

        protected void DrawPanelBorder(DrawItemEventArgs e)
        {
            if (!MxTheme.IsAppThemed)
            {
                IntPtr hdc = e.Graphics.GetHdc();
                try
                {
                    Interop.SelectClipRgn(hdc, IntPtr.Zero);
                    int index = base.Parent.Panels.IndexOf(this);
                    Interop.RECT lParam = new Interop.RECT();
                    Interop.SendMessage(base.Parent.Handle, 0x40a, index, ref lParam);
                    Interop.FrameRect(hdc, ref lParam, Interop.GetSysColorBrush(0x10));
                }
                finally
                {
                    if (hdc != IntPtr.Zero)
                    {
                        e.Graphics.ReleaseHdc(hdc);
                    }
                }
            }
        }

        protected void Invalidate()
        {
            if (base.Parent.IsHandleCreated)
            {
                Interop.RECT lParam = new Interop.RECT();
                int index = base.Parent.Panels.IndexOf(this);
                if (Interop.SendMessage(base.Parent.Handle, 0x40a, index, ref lParam) != 0)
                {
                    base.Parent.Invalidate(Rectangle.FromLTRB(lParam.left, lParam.top, lParam.right, lParam.bottom));
                }
            }
        }

        internal StatusBarPanelCommand Command
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
    }
}

