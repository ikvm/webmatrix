namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Windows.Forms;

    public sealed class WindowHandle : IWin32Window
    {
        private IntPtr _handle;

        public WindowHandle(IntPtr handle)
        {
            this._handle = handle;
        }

        public IntPtr Handle
        {
            get
            {
                return this._handle;
            }
        }
    }
}

