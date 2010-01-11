namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class MxListView : ListView
    {
        private bool _flatScrollBars;
        private bool _hasWatermark;
        private bool _multiLineToolTips;
        private bool _showToolTips;
        private string _watermarkText;
        private static readonly object EventShowContextMenu = new object();

        public event ShowContextMenuEventHandler ShowContextMenu
        {
            add
            {
                base.Events.AddHandler(EventShowContextMenu, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventShowContextMenu, value);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (this._flatScrollBars)
            {
                this.UpdateFlatScrollBars();
            }
            if (this._showToolTips)
            {
                this.UpdateShowToolTips();
            }
        }

        protected virtual void OnShowContextMenu(ShowContextMenuEventArgs e)
        {
            ShowContextMenuEventHandler handler = (ShowContextMenuEventHandler) base.Events[EventShowContextMenu];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void UpdateFlatScrollBars()
        {
            if (this._flatScrollBars)
            {
                Interop.SendMessage(base.Handle, 0x1036, 0x100, 0x100);
                Interop.FlatSB_SetScrollProp(base.Handle, 0x100, (IntPtr) 1, 1);
                Interop.FlatSB_SetScrollProp(base.Handle, 0x200, (IntPtr) 1, 1);
            }
            else
            {
                Interop.SendMessage(base.Handle, 0x1036, 0x100, 0);
            }
        }

        private void UpdateShowToolTips()
        {
            if (this._showToolTips)
            {
                Interop.SendMessage(base.Handle, 0x1036, 0x400, 0x400);
            }
            else
            {
                Interop.SendMessage(base.Handle, 0x1036, 0x400, 0);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x7b)
            {
                int lParam;
                int num2;
                bool keyboard = false;
                if (((int) m.LParam) == -1)
                {
                    keyboard = true;
                    int messagePos = Interop.GetMessagePos();
                    lParam = messagePos & 0xffff;
                    num2 = (messagePos >> 0x10) & 0xffff;
                }
                else
                {
                    lParam = (short) ((int) m.LParam);
                    num2 = ((int) m.LParam) >> 0x10;
                }
                ShowContextMenuEventArgs e = new ShowContextMenuEventArgs(base.PointToClient(new Point(lParam, num2)), keyboard);
                this.OnShowContextMenu(e);
            }
            else
            {
                base.WndProc(ref m);
                if (this._multiLineToolTips && (m.Msg == 0x4e))
                {
                    Interop.NMHDR nmhdr = (Interop.NMHDR) Marshal.PtrToStructure(m.LParam, typeof(Interop.NMHDR));
                    if ((nmhdr.code == -530) || (nmhdr.code == -520))
                    {
                        IntPtr hWnd = Interop.SendMessage(base.Handle, 0x104e, 0, 0);
                        if (hWnd != IntPtr.Zero)
                        {
                            Interop.SendMessage(hWnd, 0x418, 0, 600);
                        }
                    }
                }
                if ((this._hasWatermark && (m.Msg == 15)) && (base.Items.Count == 0))
                {
                    Graphics graphics = Graphics.FromHwndInternal(base.Handle);
                    Rectangle clientRectangle = base.ClientRectangle;
                    Brush brush = new SolidBrush(this.BackColor);
                    graphics.FillRectangle(brush, clientRectangle);
                    brush.Dispose();
                    int num4 = 8;
                    if ((base.View == View.Details) && (base.HeaderStyle != ColumnHeaderStyle.None))
                    {
                        num4 += 20;
                    }
                    clientRectangle.Inflate(-8, -num4);
                    StringFormat format = new StringFormat();
                    format.LineAlignment = StringAlignment.Near;
                    format.Alignment = StringAlignment.Center;
                    graphics.DrawString(this._watermarkText, this.Font, SystemBrushes.ControlDark, clientRectangle, format);
                    format.Dispose();
                    graphics.Dispose();
                }
            }
        }

        public bool FlatScrollBars
        {
            get
            {
                return this._flatScrollBars;
            }
            set
            {
                if (this._flatScrollBars != value)
                {
                    this._flatScrollBars = value;
                    if (base.IsHandleCreated)
                    {
                        this.UpdateFlatScrollBars();
                    }
                }
            }
        }

        public bool MultiLineToolTips
        {
            get
            {
                return this._multiLineToolTips;
            }
            set
            {
                this._multiLineToolTips = value;
            }
        }

        public bool ShowToolTips
        {
            get
            {
                return this._showToolTips;
            }
            set
            {
                if (this._showToolTips != value)
                {
                    this._showToolTips = value;
                    if (base.IsHandleCreated)
                    {
                        this.UpdateShowToolTips();
                    }
                }
            }
        }

        public string WatermarkText
        {
            get
            {
                if (this._watermarkText == null)
                {
                    return string.Empty;
                }
                return this._watermarkText;
            }
            set
            {
                this._watermarkText = value;
                this._hasWatermark = (this._watermarkText != null) && (this._watermarkText.Length != 0);
                if (base.IsHandleCreated)
                {
                    base.Invalidate();
                }
            }
        }
    }
}

