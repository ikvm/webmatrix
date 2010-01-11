namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [DefaultEvent("MouseDown"), DefaultProperty("Text")]
    public sealed class TrayIcon : Component
    {
        private bool _added;
        private bool _doubleClick;
        private System.Drawing.Icon _icon;
        private int _id;
        private Form _owner;
        private string _text;
        private bool _visible;
        private static readonly object EventClick = new object();
        private static readonly object EventDoubleClick = new object();
        private static readonly object EventMouseDown = new object();
        private static readonly object EventMouseMove = new object();
        private static readonly object EventMouseUp = new object();
        private static readonly object EventShowContextMenu = new object();
        private static int nextId = 0;
        private TrayIconNativeWindow window;
        private static int WM_TASKBARCREATED = Interop.RegisterWindowMessage("TaskbarCreated");

        [Category("Action")]
        public event EventHandler Click
        {
            add
            {
                base.Events.AddHandler(EventClick, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventClick, value);
            }
        }

        [Category("Action")]
        public event EventHandler DoubleClick
        {
            add
            {
                base.Events.AddHandler(EventDoubleClick, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventDoubleClick, value);
            }
        }

        [Category("Mouse")]
        public event MouseEventHandler MouseDown
        {
            add
            {
                base.Events.AddHandler(EventMouseDown, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventMouseDown, value);
            }
        }

        [Category("Mouse")]
        public event MouseEventHandler MouseMove
        {
            add
            {
                base.Events.AddHandler(EventMouseMove, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventMouseMove, value);
            }
        }

        [Category("Mouse")]
        public event MouseEventHandler MouseUp
        {
            add
            {
                base.Events.AddHandler(EventMouseUp, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventMouseUp, value);
            }
        }

        [Category("Behavior")]
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

        public TrayIcon()
        {
            this._doubleClick = false;
            this._visible = false;
            this._id = ++nextId;
            this._text = string.Empty;
            this.window = new TrayIconNativeWindow(this);
        }

        public TrayIcon(IContainer container) : this()
        {
            container.Add(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.window != null)
                {
                    this.UpdateIcon(false);
                    this.window.DestroyHandle();
                    this.window = null;
                    this._icon = null;
                    this._text = string.Empty;
                }
                this._owner = null;
            }
            else if ((this.window != null) && (this.window.Handle != IntPtr.Zero))
            {
                Interop.PostMessage(this.window.Handle, 0x10, IntPtr.Zero, IntPtr.Zero);
                this.window.ReleaseHandle();
            }
            base.Dispose(disposing);
        }

        private void OnClick(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[EventClick];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnDoubleClick(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[EventDoubleClick];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnMouseDown(MouseEventArgs e)
        {
            MouseEventHandler handler = (MouseEventHandler) base.Events[EventMouseDown];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnMouseMove(MouseEventArgs e)
        {
            MouseEventHandler handler = (MouseEventHandler) base.Events[EventMouseMove];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnMouseUp(MouseEventArgs e)
        {
            MouseEventHandler handler = (MouseEventHandler) base.Events[EventMouseUp];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnShowContextMenu()
        {
            if ((this._owner != null) && this._owner.IsHandleCreated)
            {
                ShowContextMenuEventHandler handler = (ShowContextMenuEventHandler) base.Events[EventShowContextMenu];
                if (handler != null)
                {
                    Interop.POINT pt = new Interop.POINT();
                    Interop.GetCursorPos(ref pt);
                    Point p = new Point(pt.x, pt.y);
                    p = this._owner.PointToClient(p);
                    Interop.SetForegroundWindow(this._owner.Handle);
                    ShowContextMenuEventArgs e = new ShowContextMenuEventArgs(p, false);
                    handler(this, e);
                    Interop.PostMessage(this._owner.Handle, 0, IntPtr.Zero, IntPtr.Zero);
                }
            }
        }

        public void ShowBalloon(string title, string message, int seconds)
        {
            if (!this._added)
            {
                throw new ApplicationException("Must set TrayIcon to be visible before showing a balloon");
            }
            if ((title == null) || (title.Length == 0))
            {
                throw new ArgumentNullException("title");
            }
            if (title.Length > 0x3f)
            {
                throw new ArgumentOutOfRangeException("Title is limited to 63 characters");
            }
            if (message == null)
            {
                message = string.Empty;
            }
            else if (title.Length > 0xff)
            {
                throw new ArgumentOutOfRangeException("Message is limited to 255 characters");
            }
            if ((seconds < 10) || (seconds > 30))
            {
                throw new ArgumentOutOfRangeException("Seconds must be between 10 and 30");
            }
            Interop.NOTIFYICONDATA pnid = new Interop.NOTIFYICONDATA();
            pnid.uFlags = 0x10;
            pnid.hWnd = this.window.Handle;
            pnid.uID = this._id;
            pnid.szTitle = title;
            pnid.szInfo = message;
            pnid.uTimeout = seconds * 0x3e8;
            pnid.dwInfoFlags = 1;
            Interop.Shell_NotifyIcon(1, pnid);
        }

        private void UpdateIcon(bool showIconInTray)
        {
            if (!base.DesignMode && (this._owner != null))
            {
                if (!this._added)
                {
                    if (!showIconInTray)
                    {
                        return;
                    }
                    if (this._icon == null)
                    {
                        return;
                    }
                }
                lock (this)
                {
                    this.window.LockReference(showIconInTray);
                    Interop.NOTIFYICONDATA pnid = new Interop.NOTIFYICONDATA();
                    pnid.uFlags = 7;
                    pnid.uCallbackMessage = 0x800;
                    if (showIconInTray && (this.window.Handle == IntPtr.Zero))
                    {
                        this.window.CreateHandle(new CreateParams());
                    }
                    pnid.hWnd = this.window.Handle;
                    pnid.uID = this._id;
                    pnid.hIcon = this._icon.Handle;
                    pnid.szTip = this._text;
                    pnid.uTimeout = 3;
                    if (showIconInTray && (this._icon != null))
                    {
                        if (!this._added)
                        {
                            Interop.Shell_NotifyIcon(0, pnid);
                            this._added = true;
                            Interop.Shell_NotifyIcon(4, pnid);
                        }
                        else
                        {
                            Interop.Shell_NotifyIcon(1, pnid);
                        }
                    }
                    else if (this._added)
                    {
                        Interop.Shell_NotifyIcon(2, pnid);
                        this._added = false;
                    }
                }
            }
        }

        private void WmMouseDown(ref Message m, MouseButtons button, int clicks)
        {
            if (clicks == 2)
            {
                this.OnDoubleClick(EventArgs.Empty);
                this._doubleClick = true;
            }
            this.OnMouseDown(new MouseEventArgs(button, clicks, 0, 0, 0));
        }

        private void WmMouseMove(ref Message m)
        {
            this.OnMouseMove(new MouseEventArgs(Control.MouseButtons, 0, 0, 0, 0));
        }

        private void WmMouseUp(ref Message m, MouseButtons button)
        {
            this.OnMouseUp(new MouseEventArgs(button, 0, 0, 0, 0));
            if (!this._doubleClick)
            {
                this.OnClick(EventArgs.Empty);
            }
            this._doubleClick = false;
        }

        private void WmTaskbarCreated(ref Message m)
        {
            this._added = false;
            this.UpdateIcon(this._visible);
        }

        private void WmTrayMessage(ref Message m)
        {
            switch (((int) m.LParam))
            {
                case 0x200:
                    this.WmMouseMove(ref m);
                    return;

                case 0x201:
                    this.WmMouseDown(ref m, MouseButtons.Left, 1);
                    return;

                case 0x202:
                    this.WmMouseUp(ref m, MouseButtons.Left);
                    return;

                case 0x203:
                    this.WmMouseDown(ref m, MouseButtons.Left, 2);
                    return;

                case 0x204:
                    this.WmMouseDown(ref m, MouseButtons.Right, 1);
                    return;

                case 0x205:
                    this.OnShowContextMenu();
                    this.WmMouseUp(ref m, MouseButtons.Right);
                    return;

                case 0x206:
                    this.WmMouseDown(ref m, MouseButtons.Right, 2);
                    return;

                case 0x207:
                    this.WmMouseDown(ref m, MouseButtons.Middle, 1);
                    return;

                case 520:
                    this.WmMouseUp(ref m, MouseButtons.Middle);
                    return;

                case 0x209:
                    this.WmMouseDown(ref m, MouseButtons.Middle, 2);
                    return;
            }
        }

        [DefaultValue((string) null), Category("Appearance")]
        public System.Drawing.Icon Icon
        {
            get
            {
                return this._icon;
            }
            set
            {
                if (this._icon != value)
                {
                    this._icon = value;
                    this.UpdateIcon(this._visible);
                }
            }
        }

        public Form Owner
        {
            get
            {
                return this._owner;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (this._owner != null)
                {
                    throw new ArgumentException("Cannot change owner form", "value");
                }
                this._owner = value;
            }
        }

        [Category("Appearance"), DefaultValue("")]
        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }
                if (!value.Equals(this._text))
                {
                    if (value.Length > 0x7f)
                    {
                        throw new ArgumentOutOfRangeException("Text is limited to 127 characters", "value");
                    }
                    this._text = value;
                    if (this._added)
                    {
                        this.UpdateIcon(true);
                    }
                }
            }
        }

        [DefaultValue(false), Category("Behavior")]
        public bool Visible
        {
            get
            {
                return this._visible;
            }
            set
            {
                if (this._visible != value)
                {
                    this.UpdateIcon(value);
                    this._visible = value;
                }
            }
        }

        private class TrayIconNativeWindow : NativeWindow
        {
            internal TrayIcon reference;
            private GCHandle rootRef;

            public TrayIconNativeWindow(TrayIcon trayIcon)
            {
                this.reference = trayIcon;
            }

            ~TrayIconNativeWindow()
            {
                if (base.Handle != IntPtr.Zero)
                {
                    Interop.PostMessage(base.Handle, 0x10, IntPtr.Zero, IntPtr.Zero);
                }
            }

            public void LockReference(bool locked)
            {
                if (locked)
                {
                    if (!this.rootRef.IsAllocated)
                    {
                        this.rootRef = GCHandle.Alloc(this.reference, GCHandleType.Normal);
                    }
                }
                else if (this.rootRef.IsAllocated)
                {
                    this.rootRef.Free();
                }
            }

            protected override void OnThreadException(Exception e)
            {
                Application.OnThreadException(e);
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 0x800)
                {
                    this.reference.WmTrayMessage(ref m);
                }
                else if (m.Msg == TrayIcon.WM_TASKBARCREATED)
                {
                    this.reference.WmTaskbarCreated(ref m);
                }
                else
                {
                    base.DefWndProc(ref m);
                }
            }
        }
    }
}

