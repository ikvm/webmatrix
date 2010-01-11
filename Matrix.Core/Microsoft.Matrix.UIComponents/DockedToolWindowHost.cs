namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal sealed class DockedToolWindowHost : ToolWindowHost
    {
        private const int DC_ACTIVE = 0x0001;
        private const int DC_SMALLCAP = 0x0002;
        private const int DC_ICON = 0x0004;
        private const int DC_TEXT = 0x0008;
        private const int DC_INBUTTON = 0x0010;
        private const int DC_GRADIENT = 0x0020;

        public DockedToolWindowHost(IToolWindowManager toolWindowManager) : base(toolWindowManager)
        {
            base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.Opaque | ControlStyles.UserPaint, true);
            base.FormBorderStyle = FormBorderStyle.None;
            base.TopLevel = false;
            this.Dock = DockStyle.Fill;
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            base.Invalidate(this.CaptionRectangle);
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            base.Invalidate(this.CaptionRectangle);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            base.Focus();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Rectangle captionRectangle = this.CaptionRectangle;
            bool containsFocus = base.ContainsFocus;
            graphics.FillRectangle(SystemBrushes.Control, e.ClipRectangle);
            IntPtr hdc = graphics.GetHdc();
            try
            {
                // NOTE: 
                // 发现使用DC_SMALLCAP后, 在win7下的工具窗口标题栏不正常, 表现为
                // 标题栏文字不完整
                //int nFlags = DC_GRADIENT | DC_TEXT | DC_SMALLCAP; 
                int nFlags = DC_GRADIENT | DC_TEXT;
                if (containsFocus)
                {
                    nFlags |= DC_ACTIVE;
                }
                Interop.RECT rect = new Interop.RECT(captionRectangle.Left, captionRectangle.Top, captionRectangle.Right + 1, captionRectangle.Bottom);
                Interop.DrawCaption(base.Handle, hdc, ref rect, nFlags);
            }
            finally
            {
                if (hdc != IntPtr.Zero)
                {
                    graphics.ReleaseHdc(hdc);
                }
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            base.Invalidate(this.CaptionRectangle);
        }

        private Rectangle CaptionRectangle
        {
            get
            {
                return new Rectangle(0, 0, base.ClientRectangle.Width - 1, SystemInformation.ToolWindowCaptionHeight);
            }
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle clientRectangle = base.ClientRectangle;
                int toolWindowCaptionHeight = SystemInformation.ToolWindowCaptionHeight;
                return new Rectangle(0, toolWindowCaptionHeight + 1, clientRectangle.Width, (clientRectangle.Height - toolWindowCaptionHeight) - 1);
            }
        }
    }
}

