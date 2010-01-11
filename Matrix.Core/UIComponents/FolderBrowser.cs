namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public sealed class FolderBrowser : Component
    {
        private string _descriptionText = string.Empty;
        private string _directoryPath = string.Empty;
        private int _privateOptions = 0x40;
        private FolderBrowserStyles _publicOptions = FolderBrowserStyles.RestrictToFileSystem;
        private FolderBrowserLocation _startLocation = FolderBrowserLocation.Desktop;

        private static Interop.IMalloc GetSHMalloc()
        {
            Interop.IMalloc[] ppMalloc = new Interop.IMalloc[1];
            Interop.SHGetMalloc(ppMalloc);
            return ppMalloc[0];
        }

        public DialogResult ShowDialog()
        {
            return this.ShowDialog(null);
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            IntPtr zero = IntPtr.Zero;
            IntPtr hwnd = IntPtr.Zero;
            if (owner != null)
            {
                hwnd = owner.Handle;
            }
            Interop.SHGetFolderLocation(hwnd, (int) this._startLocation, IntPtr.Zero, 0, out zero);
            if (zero == IntPtr.Zero)
            {
                return DialogResult.Cancel;
            }
            int num = ((int) this._publicOptions) | this._privateOptions;
            if ((num & 0x40) != 0)
            {
                Application.OleRequired();
            }
            IntPtr pidl = IntPtr.Zero;
            try
            {
                Interop.BROWSEINFO lpbi = new Interop.BROWSEINFO();
                IntPtr pszPath = Marshal.AllocHGlobal(0x100);
                lpbi.pidlRoot = zero;
                lpbi.hwndOwner = hwnd;
                lpbi.pszDisplayName = pszPath;
                lpbi.lpszTitle = this._descriptionText;
                lpbi.ulFlags = num;
                lpbi.lpfn = IntPtr.Zero;
                lpbi.lParam = IntPtr.Zero;
                lpbi.iImage = 0;
                pidl = Interop.SHBrowseForFolder(lpbi);
                if (pidl == IntPtr.Zero)
                {
                    return DialogResult.Cancel;
                }
                Interop.SHGetPathFromIDList(pidl, pszPath);
                this._directoryPath = Marshal.PtrToStringAuto(pszPath);
                Marshal.FreeHGlobal(pszPath);
            }
            finally
            {
                Interop.IMalloc sHMalloc = GetSHMalloc();
                sHMalloc.Free(zero);
                if (pidl != IntPtr.Zero)
                {
                    sHMalloc.Free(pidl);
                }
            }
            return DialogResult.OK;
        }

        public string Description
        {
            get
            {
                return this._descriptionText;
            }
            set
            {
                this._descriptionText = (value == null) ? string.Empty : value;
            }
        }

        public string DirectoryPath
        {
            get
            {
                return this._directoryPath;
            }
        }

        public FolderBrowserLocation StartLocation
        {
            get
            {
                return this._startLocation;
            }
            set
            {
                this._startLocation = value;
            }
        }

        public FolderBrowserStyles Style
        {
            get
            {
                return this._publicOptions;
            }
            set
            {
                this._publicOptions = value;
            }
        }
    }
}

