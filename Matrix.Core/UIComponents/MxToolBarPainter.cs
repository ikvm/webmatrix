namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal sealed class MxToolBarPainter
    {
        private bool _mouseDown;
        private ToolBar _toolBar;
        private const int HotTrackOpacity = 50;
        private const int MouseDownOpacity = 150;

        public MxToolBarPainter(ToolBar toolBar)
        {
            this._toolBar = toolBar;
        }

        public void OnCustomDraw(ref Message m)
        {
            Interop.NMTBCUSTOMDRAW nmtbcustomdraw = (Interop.NMTBCUSTOMDRAW) Marshal.PtrToStructure(m.LParam, typeof(Interop.NMTBCUSTOMDRAW));
            switch (nmtbcustomdraw.nmcd.dwDrawStage)
            {
                case 0x10001:
                {
                    Interop.RECT rc = nmtbcustomdraw.nmcd.rc;
                    Graphics g = Graphics.FromHdc(nmtbcustomdraw.nmcd.hdc);
                    Rectangle r = new Rectangle(rc.left + 1, rc.top + 1, rc.Width - 2, rc.Height - 2);
                    this.OnDrawItem(g, r, nmtbcustomdraw.nmcd.dwItemSpec, nmtbcustomdraw.nmcd.uItemState);
                    g.Dispose();
                    m.Result = (IntPtr) 4;
                    return;
                }
                case 0x10002:
                    m.Result = (IntPtr) 4;
                    return;

                case 1:
                    m.Result = (IntPtr) 0x30;
                    return;
            }
            m.Result = IntPtr.Zero;
        }

        private void OnDrawItem(Graphics g, Rectangle r, int index, int itemState)
        {
            ToolBarButton button = this._toolBar.Buttons[index];
            bool flag = (itemState & 8) != 0;
            bool flag2 = (itemState & 0x40) != 0;
            bool flag3 = (itemState & 4) != 0;
            if (!flag3)
            {
                if (this._mouseDown && flag2)
                {
                    Brush brush = new SolidBrush(Color.FromArgb(150, SystemColors.Highlight));
                    g.FillRectangle(brush, r);
                    brush.Dispose();
                }
                else if (flag2 || flag)
                {
                    Brush brush2 = new SolidBrush(Color.FromArgb(50, SystemColors.Highlight));
                    g.FillRectangle(brush2, r);
                    brush2.Dispose();
                    Brush brush3 = new SolidBrush(Color.FromArgb(50, Color.White));
                    g.FillRectangle(brush3, r);
                    brush3.Dispose();
                }
            }
            int imageIndex = button.ImageIndex;
            ImageList imageList = this._toolBar.ImageList;
            if (((imageIndex != -1) && (imageList != null)) && (imageList.Images.Count > imageIndex))
            {
                if (!flag3)
                {
                    imageList.Draw(g, r.Left + 2, 1 + ((r.Height - 0x10) / 2), imageIndex);
                }
                else if (this._toolBar is MxToolBar)
                {
                    ((MxToolBar) this._toolBar).DisabledImageList.Draw(g, r.Left + 2, 1 + ((r.Height - 0x10) / 2), imageIndex);
                }
                else
                {
                    Image image = ImageUtility.CreateDisabledImage(imageList.Images[imageIndex]);
                    g.DrawImageUnscaled(image, r.Left + 2, 1 + ((r.Height - 0x10) / 2));
                    image.Dispose();
                }
            }
            Brush controlText = SystemBrushes.ControlText;
            if (flag3)
            {
                controlText = SystemBrushes.ControlDark;
            }
            else if (this._mouseDown)
            {
                controlText = SystemBrushes.HighlightText;
            }
            else if ((flag2 || flag) || this._mouseDown)
            {
                controlText = SystemBrushes.Highlight;
            }
            if (button.Text.Length != 0)
            {
                Rectangle layoutRectangle = new Rectangle(r.Left + 20, r.Top, r.Width - 20, r.Height);
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(button.Text, this._toolBar.Font, controlText, layoutRectangle, format);
                format.Dispose();
            }
            if (button.Style == ToolBarButtonStyle.DropDownButton)
            {
                Point[] points = new Point[] { new Point((r.Left + r.Width) - 9, (r.Top + ((r.Height - 3) / 2)) + 1), new Point((r.Left + r.Width) - 4, (r.Top + ((r.Height - 3) / 2)) + 1), new Point((r.Left + r.Width) - 7, (r.Top + ((r.Height - 3) / 2)) + 4) };
                g.FillPolygon(controlText, points);
            }
            if (!flag3 && (flag2 || flag))
            {
                Pen pen = new Pen(SystemColors.Highlight);
                g.DrawRectangle(pen, r);
                if (button.Style == ToolBarButtonStyle.DropDownButton)
                {
                    g.DrawLine(pen, (r.Left + r.Width) - 14, r.Top, (r.Left + r.Width) - 14, (r.Top + r.Height) - 1);
                }
                pen.Dispose();
            }
        }

        public bool MouseDown
        {
            get
            {
                return this._mouseDown;
            }
            set
            {
                this._mouseDown = value;
            }
        }
    }
}

