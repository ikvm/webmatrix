namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class MxTreeView : TreeView
    {
        private bool _hasWatermark;
        private string _watermarkText;
        private static readonly object EventNodeDoubleClick = new object();
        private static readonly object EventShowContextMenu = new object();

        public event TreeViewEventHandler NodeDoubleClick
        {
            add
            {
                base.Events.AddHandler(EventNodeDoubleClick, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventNodeDoubleClick, value);
            }
        }

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

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            Interop.POINT pt = new Interop.POINT(0, 0);
            Interop.GetCursorPos(ref pt);
            Point point2 = base.PointToClient(new Point(pt.x, pt.y));
            TreeNode nodeAt = base.GetNodeAt(point2);
            if (nodeAt == base.SelectedNode)
            {
                this.OnNodeDoubleClick(new TreeViewEventArgs(nodeAt, TreeViewAction.ByMouse));
            }
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            Interop.SendMessage(base.Handle, 0x110b, (IntPtr) 8, IntPtr.Zero);
            base.OnDragDrop(e);
        }

        protected override void OnDragLeave(EventArgs e)
        {
            Interop.SendMessage(base.Handle, 0x110b, (IntPtr) 8, IntPtr.Zero);
            base.OnDragLeave(e);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            p = base.PointToClient(p);
            TreeNode nodeAt = base.GetNodeAt(p);
            if (nodeAt != null)
            {
                Interop.SendMessage(base.Handle, 0x110b, (IntPtr) 8, nodeAt.Handle);
                TreeNode prevVisibleNode = nodeAt.PrevVisibleNode;
                TreeNode nextVisibleNode = nodeAt.NextVisibleNode;
                if (prevVisibleNode != null)
                {
                    prevVisibleNode.EnsureVisible();
                }
                if (nextVisibleNode != null)
                {
                    nextVisibleNode.EnsureVisible();
                }
            }
            base.OnDragOver(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
            {
                this.OnNodeDoubleClick(new TreeViewEventArgs(base.SelectedNode, TreeViewAction.ByKeyboard));
            }
        }

        protected virtual void OnNodeDoubleClick(TreeViewEventArgs e)
        {
            TreeViewEventHandler handler = (TreeViewEventHandler) base.Events[EventNodeDoubleClick];
            if (handler != null)
            {
                handler(this, e);
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

        protected override void WndProc(ref Message m)
        {
            if ((m.Msg == 0x7b) && (this.ContextMenu == null))
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
                if ((this._hasWatermark && (m.Msg == 15)) && (base.Nodes.Count == 0))
                {
                    Graphics graphics = Graphics.FromHwndInternal(base.Handle);
                    Rectangle clientRectangle = base.ClientRectangle;
                    Brush brush = new SolidBrush(this.BackColor);
                    graphics.FillRectangle(brush, clientRectangle);
                    brush.Dispose();
                    clientRectangle.Inflate(-8, -8);
                    StringFormat format = new StringFormat();
                    format.LineAlignment = StringAlignment.Near;
                    format.Alignment = StringAlignment.Center;
                    graphics.DrawString(this._watermarkText, this.Font, SystemBrushes.ControlDark, clientRectangle, format);
                    format.Dispose();
                    graphics.Dispose();
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

