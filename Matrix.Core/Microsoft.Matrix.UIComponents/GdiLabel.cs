namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class GdiLabel : Label
    {
        public GdiLabel()
        {
            base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int nFormat = 0x40;
            switch (this.TextAlign)
            {
                case ContentAlignment.TopLeft:
                    nFormat = nFormat;
                    goto Label_009A;

                case ContentAlignment.TopCenter:
                    nFormat |= 1;
                    goto Label_009A;

                case (ContentAlignment.TopCenter | ContentAlignment.TopLeft):
                    goto Label_009A;

                case ContentAlignment.TopRight:
                    nFormat |= 2;
                    goto Label_009A;

                case ContentAlignment.MiddleLeft:
                    nFormat |= 4;
                    goto Label_009A;

                case ContentAlignment.MiddleCenter:
                    nFormat |= 5;
                    goto Label_009A;

                case ContentAlignment.MiddleRight:
                    nFormat |= 6;
                    goto Label_009A;

                case ContentAlignment.BottomLeft:
                    nFormat |= 8;
                    goto Label_009A;

                case ContentAlignment.BottomCenter:
                    nFormat |= 9;
                    break;

                case ContentAlignment.BottomRight:
                    nFormat |= 10;
                    goto Label_009A;
            }
        Label_009A:
            using (Brush brush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillRectangle(brush, base.ClientRectangle);
            }
            IntPtr hdc = e.Graphics.GetHdc();
            IntPtr hObject = this.Font.ToHfont();
            try
            {
                IntPtr ptr3 = Interop.SelectObject(hdc, hObject);
                Interop.RECT lpRect = new Interop.RECT(0, 0, base.Width, base.Height);
                Interop.DrawText(hdc, this.Text, this.Text.Length, ref lpRect, 0x400);
                int num2 = lpRect.bottom - lpRect.top;
                int top = (base.Height - num2) / 2;
                lpRect = new Interop.RECT(0, top, base.Width, base.Height);
                Interop.SetTextColor(hdc, ColorTranslator.ToWin32(this.ForeColor));
                Interop.DrawText(hdc, this.Text, this.Text.Length, ref lpRect, nFormat);
                Interop.SelectObject(hdc, ptr3);
            }
            finally
            {
                e.Graphics.ReleaseHdc(hdc);
            }
        }
    }
}

