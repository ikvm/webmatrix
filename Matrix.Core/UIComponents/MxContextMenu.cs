namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class MxContextMenu : ContextMenu
    {
        private EventHandler _closeHandler;
        private static readonly object EventClose = new object();

        public event EventHandler Close
        {
            add
            {
                this._closeHandler = (EventHandler) Delegate.Combine(this._closeHandler, value);
            }
            remove
            {
                if (this._closeHandler != null)
                {
                    this._closeHandler = (EventHandler) Delegate.Remove(this._closeHandler, value);
                }
            }
        }

        public MxContextMenu() : this(null)
        {
        }

        public MxContextMenu(MenuItem[] menuItems) : base(menuItems)
        {
        }

        protected virtual void OnClose(EventArgs e)
        {
            if (this._closeHandler != null)
            {
                this._closeHandler(this, e);
            }
        }

        public void Show(Control control, int x, int y)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            if (!control.IsHandleCreated)
            {
                throw new ArgumentException();
            }
            ContextMenu contextMenu = control.ContextMenu;
            this.OnPopup(EventArgs.Empty);
            try
            {
                control.ContextMenu = this;
                Point point = control.PointToScreen(new Point(x, y));
                Interop.TrackPopupMenuEx(base.Handle, 0x40, point.X, point.Y, control.Handle, IntPtr.Zero);
            }
            finally
            {
                control.ContextMenu = contextMenu;
                this.OnClose(EventArgs.Empty);
            }
        }
    }
}

