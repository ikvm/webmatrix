namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public sealed class SplashScreen : MarshalByRefObject
    {
        private SplashScreenCustomizeCallback _customizer;
        private int _height;
        private IntPtr _hwnd;
        private System.Drawing.Image _image;
        private int _minimumDuration;
        private bool _minimumDurationComplete;
        private bool _showShadow;
        private int _timer;
        private Color _transparencyKey;
        private bool _waitingForTimer;
        private int _width;
        private IWin32Window _windowToActivate;
        private static SplashScreen current;
        private static Interop.WNDPROC splashWindowProcedure;
        private const string ThreadName = "SplashThread";
        private const string WindowClassName = "SplashWindow";

        private SplashScreen()
        {
        }

        private bool CreateNativeWindow()
        {
            bool flag = false;
            int style = -1879048192;
            int dwExStyle = 0x88;
            if (!this._transparencyKey.IsEmpty && this.IsLayeringSupported())
            {
                dwExStyle |= 0x80000;
            }
            Rectangle workingArea = Screen.FromPoint(Control.MousePosition).WorkingArea;
            int x = Math.Max(workingArea.X, workingArea.X + ((workingArea.Width - this._width) / 2));
            int y = Math.Max(workingArea.Y, workingArea.Y + ((workingArea.Height - this._height) / 2));
            this._hwnd = Interop.CreateWindowEx(dwExStyle, "SplashWindow", "", style, x, y, this._width, this._height, IntPtr.Zero, IntPtr.Zero, Interop.GetModuleHandle(null), null);
            if (this._hwnd != IntPtr.Zero)
            {
                Interop.ShowWindow(this._hwnd, 1);
                Interop.UpdateWindow(this._hwnd);
                flag = true;
            }
            return flag;
        }

        public void Hide(IWin32Window windowToActivate)
        {
            this._windowToActivate = windowToActivate;
            if (this._minimumDuration > 0)
            {
                this._waitingForTimer = true;
                if (!this._minimumDurationComplete)
                {
                    return;
                }
            }
            if (this._hwnd != IntPtr.Zero)
            {
                Interop.PostMessage(this._hwnd, 0x10, IntPtr.Zero, IntPtr.Zero);
            }
        }

        private bool IsDropShadowSupported()
        {
            return (Environment.OSVersion.Version.CompareTo(new Version(5, 1, 0, 0)) >= 0);
        }

        private bool IsLayeringSupported()
        {
            return (Environment.OSVersion.Version.CompareTo(new Version(5, 0, 0, 0)) >= 0);
        }

        private bool RegisterWindowClass()
        {
            splashWindowProcedure = new Interop.WNDPROC(this.SplashWindowProcedure);
            bool flag = false;
            Interop.WNDCLASS wc = new Interop.WNDCLASS();
            wc.style = 0;
            wc.lpfnWndProc = splashWindowProcedure;
            wc.hInstance = Interop.GetModuleHandle(null);
            wc.hbrBackground = (IntPtr) 6;
            wc.lpszClassName = "SplashWindow";
            wc.cbClsExtra = 0;
            wc.cbWndExtra = 0;
            wc.hIcon = IntPtr.Zero;
            wc.hCursor = IntPtr.Zero;
            wc.lpszMenuName = null;
            if (this._showShadow && this.IsDropShadowSupported())
            {
                wc.style |= 0x20000;
            }
            if (Interop.RegisterClass(wc) != IntPtr.Zero)
            {
                flag = true;
            }
            return flag;
        }

        public void Show()
        {
            if (this._hwnd == IntPtr.Zero)
            {
                if (this._image == null)
                {
                    throw new InvalidOperationException("Image must be set first");
                }
                Thread thread = new Thread(new ThreadStart(SplashScreen.SplashThreadProcedure));
                thread.Name = "SplashThread";
                thread.ApartmentState = ApartmentState.STA;
                thread.Start();
                thread.IsBackground = true;
            }
        }

        private static void SplashThreadProcedure()
        {
            bool flag = true;
            if (splashWindowProcedure == null)
            {
                flag = current.RegisterWindowClass();
            }
            if (flag)
            {
                flag = current.CreateNativeWindow();
            }
            if (flag)
            {
                Interop.MSG msg = new Interop.MSG();
                while (Interop.GetMessage(ref msg, IntPtr.Zero, 0, 0))
                {
                    Interop.TranslateMessage(ref msg);
                    Interop.DispatchMessage(ref msg);
                }
                current._hwnd = IntPtr.Zero;
                if (current._windowToActivate != null)
                {
                    IntPtr handle = current._windowToActivate.Handle;
                    current._windowToActivate = null;
                    Interop.SetForegroundWindow(Interop.GetLastActivePopup(handle));
                }
            }
        }

        private int SplashWindowProcedure(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case 20:
                    return 1;

                case 0x113:
                    Interop.KillTimer(hwnd, this._timer);
                    this._timer = 0;
                    this._minimumDurationComplete = true;
                    if (this._waitingForTimer)
                    {
                        Interop.PostMessage(hwnd, 0x10, IntPtr.Zero, IntPtr.Zero);
                    }
                    return 0;

                case 1:
                    if (!this._transparencyKey.IsEmpty && this.IsLayeringSupported())
                    {
                        Interop.SetLayeredWindowAttributes(hwnd, ColorTranslator.ToWin32(this._transparencyKey), 0, 1);
                    }
                    if (this._minimumDuration > 0)
                    {
                        this._timer = Interop.SetTimer(hwnd, 1, this._minimumDuration, IntPtr.Zero);
                    }
                    break;

                case 2:
                    Interop.PostQuitMessage(0);
                    break;

                case 15:
                {
                    Interop.PAINTSTRUCT lpPaint = new Interop.PAINTSTRUCT();
                    IntPtr hdc = Interop.BeginPaint(hwnd, ref lpPaint);
                    if (hdc != IntPtr.Zero)
                    {
                        Graphics g = Graphics.FromHdcInternal(hdc);
                        g.DrawImage(this._image, 0, 0, this._width, this._height);
                        if (this._customizer != null)
                        {
                            this._customizer(new SplashScreenSurface(g, new Rectangle(0, 0, this._width, this._height)));
                        }
                        g.Dispose();
                    }
                    Interop.EndPaint(hwnd, ref lpPaint);
                    return 0;
                }
            }
            return Interop.DefWindowProc(hwnd, msg, wParam, lParam);
        }

        public static SplashScreen Current
        {
            get
            {
                if (current == null)
                {
                    current = new SplashScreen();
                }
                return current;
            }
        }

        public SplashScreenCustomizeCallback Customizer
        {
            get
            {
                return this._customizer;
            }
            set
            {
                if (this._hwnd != IntPtr.Zero)
                {
                    throw new InvalidOperationException();
                }
                this._customizer = value;
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this._image;
            }
            set
            {
                if (this._hwnd != IntPtr.Zero)
                {
                    throw new InvalidOperationException();
                }
                this._image = value;
                if (this._image != null)
                {
                    this._width = this._image.Width;
                    this._height = this._image.Height;
                }
            }
        }

        public int MinimumDuration
        {
            get
            {
                return this._minimumDuration;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (this._hwnd != IntPtr.Zero)
                {
                    throw new InvalidOperationException();
                }
                this._minimumDuration = value;
            }
        }

        public bool ShowShadow
        {
            get
            {
                return this._showShadow;
            }
            set
            {
                if (this._hwnd != IntPtr.Zero)
                {
                    throw new InvalidOperationException();
                }
                this._showShadow = value;
            }
        }

        public Color TransparencyKey
        {
            get
            {
                return this._transparencyKey;
            }
            set
            {
                if (this._hwnd != IntPtr.Zero)
                {
                    throw new InvalidOperationException();
                }
                this._transparencyKey = value;
            }
        }
    }
}

