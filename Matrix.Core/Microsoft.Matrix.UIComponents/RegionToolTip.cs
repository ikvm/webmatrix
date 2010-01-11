namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal sealed class RegionToolTip : IDisposable
    {
        private Control _owner;
        private NativeWindow _tipWindow;

        public RegionToolTip(Control owner)
        {
            this._owner = owner;
            if (this._owner.IsHandleCreated)
            {
                this.CreateNativeToolTip();
            }
            else
            {
                this._owner.HandleCreated += new EventHandler(this.OnOwnerHandleCreated);
            }
            this._owner.HandleDestroyed += new EventHandler(this.OnOwnerHandleDestroyed);
        }

        public void AddToolTipRegion(string toolTipText, int regionID, Rectangle regionBounds)
        {
            if ((((toolTipText != null) && (toolTipText.Length != 0)) && !regionBounds.IsEmpty) && (this._tipWindow != null))
            {
                Interop.TOOLINFO lParam = new Interop.TOOLINFO();
                lParam.hwnd = this._owner.Handle;
                lParam.uId = (IntPtr) regionID;
                lParam.lpszText = toolTipText;
                lParam.rect = Interop.RECT.FromXYWH(regionBounds.X, regionBounds.Y, regionBounds.Width, regionBounds.Height);
                lParam.uFlags = 0x10;
                Interop.SendMessage(this._tipWindow.Handle, Interop.TTM_ADDTOOL, 0, lParam);
            }
        }

        private void CreateNativeToolTip()
        {
            if (this._tipWindow == null)
            {
                try
                {
                    Interop.INITCOMMONCONTROLSEX structure = new Interop.INITCOMMONCONTROLSEX();
                    structure.dwICC = 8;
                    structure.dwSize = Marshal.SizeOf(structure);
                    Interop.InitCommonControlsEx(structure);
                    CreateParams cp = new CreateParams();
                    cp.Parent = this._owner.Handle;
                    cp.ClassName = "tooltips_class32";
                    cp.Style = 1;
                    this._tipWindow = new NativeWindow();
                    this._tipWindow.CreateHandle(cp);
                }
                catch (Exception)
                {
                }
                if (this._tipWindow.Handle != IntPtr.Zero)
                {
                    Interop.SetWindowPos(this._tipWindow.Handle, (IntPtr) (-1), 0, 0, 0, 0, 0x13);
                    Interop.SendMessage(this._tipWindow.Handle, 0x418, 0, 0x400);
                }
                else
                {
                    this._tipWindow = null;
                }
            }
        }

        private void DestroyNativeToolTip()
        {
            this._tipWindow.DestroyHandle();
            this._tipWindow = null;
        }

        public void Dispose()
        {
            if (this._tipWindow != null)
            {
                this.DestroyNativeToolTip();
            }
            this._owner.HandleCreated -= new EventHandler(this.OnOwnerHandleCreated);
            this._owner.HandleDestroyed -= new EventHandler(this.OnOwnerHandleDestroyed);
            this._owner = null;
        }

        private void OnOwnerHandleCreated(object sender, EventArgs e)
        {
            this.CreateNativeToolTip();
        }

        private void OnOwnerHandleDestroyed(object sender, EventArgs e)
        {
            if (this._tipWindow != null)
            {
                this.DestroyNativeToolTip();
            }
        }

        public void RemoveToolTipRegion(int regionID)
        {
            if (this._tipWindow != null)
            {
                Interop.TOOLINFO lParam = new Interop.TOOLINFO();
                lParam.hwnd = this._owner.Handle;
                lParam.uId = (IntPtr) regionID;
                Interop.SendMessage(this._tipWindow.Handle, Interop.TTM_DELTOOL, 0, lParam);
            }
        }

        public bool IsToolTipCreated
        {
            get
            {
                return (this._tipWindow != null);
            }
        }
    }
}

