namespace Microsoft.Matrix
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;

    [SuppressUnmanagedCodeSecurity]
    internal sealed class Interop
    {
        private static readonly bool _isUnicode = (Marshal.SystemDefaultCharSize != 1);
        public const int ASM_CACHE_GAC = 2;
        public const int ASM_NAME_CODEBASE_URL = 13;
        public const int ASM_NAME_CULTURE = 8;
        public const int ASM_NAME_PUBLIC_KEY = 0;
        public const int BCM_SETIMAGELIST = 0x1602;
        public const int BrowseInfos_AllowUrls = 0x80;
        public const int BrowseInfos_BrowseForComputer = 0x1000;
        public const int BrowseInfos_BrowseForEverything = 0x4000;
        public const int BrowseInfos_BrowseForPrinter = 0x2000;
        public const int BrowseInfos_DontGoBelowDomain = 2;
        public const int BrowseInfos_EditBox = 0x10;
        public const int BrowseInfos_NewDialogStyle = 0x40;
        public const int BrowseInfos_ReturnFSAncestors = 8;
        public const int BrowseInfos_ReturnOnlyFSDirs = 1;
        public const int BrowseInfos_ShowShares = 0x8000;
        public const int BrowseInfos_StatusText = 4;
        public const int BrowseInfos_UseNewUI = 80;
        public const int BrowseInfos_Validate = 0x20;
        public const int BUTTON_IMAGELIST_ALIGN_BOTTOM = 3;
        public const int BUTTON_IMAGELIST_ALIGN_CENTER = 4;
        public const int BUTTON_IMAGELIST_ALIGN_LEFT = 0;
        public const int BUTTON_IMAGELIST_ALIGN_RIGHT = 1;
        public const int BUTTON_IMAGELIST_ALIGN_TOP = 2;
        public const int CBN_CLOSEUP = 8;
        public const int CBN_EDITUPDATE = 6;
        public const int CDDS_ITEMPOSTPAINT = 0x10002;
        public const int CDDS_ITEMPREPAINT = 0x10001;
        public const int CDDS_PREPAINT = 1;
        public const int CDIS_CHECKED = 8;
        public const int CDIS_DISABLED = 4;
        public const int CDIS_HOT = 0x40;
        public const int CDRF_DODEFAULT = 0;
        public const int CDRF_NOTIFYITEMDRAW = 0x20;
        public const int CDRF_NOTIFYPOSTPAINT = 0x10;
        public const int CDRF_SKIPDEFAULT = 4;
        public const int COLOR_BTNFACE = 15;
        public const int COLOR_BTNHIGHLIGHT = 20;
        public const int COLOR_BTNSHADOW = 0x10;
        public const int COLOR_BTNTEXT = 0x12;
        public const int COLOR_MENUBAR = 30;
        public const int COLOR_WINDOW = 5;
        private const string ComCtl32 = "comctl32.dll";
        private const string Crypt32 = "crypt32.dll";
        public const int CRYPTPROTECT_LOCAL_MACHINE = 4;
        public const int CRYPTPROTECT_UI_FORBIDDEN = 1;
        public const int CS_DROPSHADOW = 0x20000;
        public const int DC_ACTIVE = 1;
        public const int DC_GRADIENT = 0x20;
        public const int DC_SMALLCAP = 2;
        public const int DC_TEXT = 8;
        public const int DFC_SCROLL = 3;
        public const int DFCS_SCROLLSIZEGRIP = 8;
        public const int DRIVE_FIXED = 3;
        public const int DRIVE_REMOTE = 4;
        public const int DT_BOTTOM = 8;
        public const int DT_CALCRECT = 0x400;
        public const int DT_CENTER = 1;
        public const int DT_EXPANDTABS = 0x40;
        public const int DT_EXTERNALLEADING = 0x200;
        public const int DT_INTERNAL = 0x1000;
        public const int DT_LEFT = 0;
        public const int DT_NOCLIP = 0x100;
        public const int DT_NOPREFIX = 0x800;
        public const int DT_RIGHT = 2;
        public const int DT_SINGLELINE = 0x20;
        public const int DT_TABSTOP = 0x80;
        public const int DT_TOP = 0;
        public const int DT_VCENTER = 4;
        public const int DT_WORDBREAK = 0x10;
        public const int EM_LINEFROMCHAR = 0xc9;
        public const int EM_LINEINDEX = 0xbb;
        public const int ES_NUMBER = 0x2000;
        public const int ES_PASSWORD = 0x20;
        public const int ETO_OPAQUE = 2;
        public const int FSB_ENCARTA_MODE = 1;
        private const string Fusion = "fusion.dll";
        private const string Gdi32 = "gdi32.dll";
        public const int GW_CHILD = 5;
        public const int HTHELP = 0x15;
        public const int HWND_TOPMOST = -1;
        public const int ICC_TAB_CLASSES = 8;
        private const string Kernel32 = "kernel32.dll";
        public const int LOGPIXELSX = 0x58;
        public const int LOGPIXELSY = 90;
        public const int LVM_FIRST = 0x1000;
        public const int LVM_GETEDITCONTROL = 0x1018;
        public const int LVM_GETTOOLTIPS = 0x104e;
        public const int LVM_SETEXTENDEDLISTVIEWSTYLE = 0x1036;
        public const int LVS_EX_FLATSB = 0x100;
        public const int LVS_EX_INFOTIP = 0x400;
        public const int LVS_OWNERDRAWFIXED = 0x400;
        public const int LWA_ALPHA = 2;
        public const int LWA_COLORKEY = 1;
        public const int MAX_PATH = 0x100;
        public const int MF_BYCOMMAND = 0;
        public const int MF_BYPOSITION = 0x400;
        public const int MF_OWNERDRAW = 0x100;
        public const int MF_SEPARATOR = 0x800;
        public const int MF_SYSMENU = 0x2000;
        public const int NIF_ICON = 2;
        public const int NIF_INFO = 0x10;
        public const int NIF_MESSAGE = 1;
        public const int NIF_TIP = 4;
        public const int NIIF_ERROR = 3;
        public const int NIIF_INFO = 1;
        public const int NIIF_NONE = 0;
        public const int NIIF_WARNING = 2;
        public const int NIM_ADD = 0;
        public const int NIM_DELETE = 2;
        public const int NIM_MODIFY = 1;
        public const int NIM_SETVERSION = 4;
        public const int NM_CUSTOMDRAW = -12;
        public const int NOTIFYICON_VERSION = 3;
        public const int OPAQUE = 2;
        public const int S_OK = 0;
        public const int SB_BOTTOM = 7;
        public const int SB_GETRECT = 0x40a;
        public const int SB_HORZ = 0;
        public const int SB_LINEDOWN = 1;
        public const int SB_LINEUP = 0;
        public const int SB_PAGEDOWN = 3;
        public const int SB_PAGEUP = 2;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_THUMBTRACK = 5;
        public const int SB_TOP = 6;
        public const int SB_VERT = 1;
        public const int SC_CLOSE = 0xf060;
        public const int SC_MAXIMIZE = 0xf030;
        public const int SC_MINIMIZE = 0xf020;
        public const int SC_MOVE = 0xf010;
        public const int SC_NEXTWINDOW = 0xf040;
        public const int SC_PREVWINDOW = 0xf050;
        public const int SC_RESTORE = 0xf120;
        public const int SC_SIZE = 0xf000;
        public static readonly int SHARED_PATH = (_isUnicode ? 3 : 2);
        private const int SHARED_PATHA = 2;
        private const int SHARED_PATHW = 3;
        private const string Shell32 = "shell32.dll";
        public const int SIF_PAGE = 2;
        public const int SIF_RANGE = 1;
        public const int SW_SHOWNORMAL = 1;
        public const int SWP_NOACTIVATE = 0x10;
        public const int SWP_NOMOVE = 2;
        public const int SWP_NOSIZE = 1;
        public const int SWP_NOZORDER = 4;
        public const int SWP_SHOWWINDOW = 0x40;
        public const int TB_GETITEMRECT = 0x41d;
        public const int TB_GETTOOLTIPS = 0x423;
        public const int TMPF_FIXED_PITCH = 1;
        public const string TOOLTIPS_CLASS = "tooltips_class32";
        public const int TPM_VERTICAL = 0x40;
        public const int TRANSPARENT = 1;
        public const int TTF_IDISHWND = 1;
        public const int TTF_SUBCLASS = 0x10;
        public static readonly int TTM_ADDTOOL = (_isUnicode ? 0x432 : 0x404);
        public const int TTM_ADDTOOLA = 0x404;
        public const int TTM_ADDTOOLW = 0x432;
        public static readonly int TTM_DELTOOL = (_isUnicode ? 0x433 : 0x405);
        public const int TTM_DELTOOLA = 0x405;
        public const int TTM_DELTOOLW = 0x433;
        public const int TTM_SETMAXTIPWIDTH = 0x418;
        public const int TTN_NEEDTEXTA = -520;
        public const int TTN_NEEDTEXTW = -530;
        public const int TTS_ALWAYSTIP = 1;
        public const int TVGN_DROPHILITE = 8;
        public const int TVM_SELECTITEM = 0x110b;
        private const string User32 = "user32.dll";
        private const string UxTheme = "uxtheme.dll";
        public const int VK_CAPITAL = 20;
        private const string WinTrust = "wintrust.dll";
        public static Guid WINTRUST_ACTION_GENERIC_VERIFY_V2 = new Guid("00AAC56B-CD44-11d0-8CC2-00C04FC295EE");
        public const int WM_CHAR = 0x102;
        public const int WM_CLOSE = 0x10;
        public const int WM_COMMAND = 0x111;
        public const int WM_CONTEXTMENU = 0x7b;
        public const int WM_CREATE = 1;
        public const int WM_DESTROY = 2;
        public const int WM_DRAWITEM = 0x2b;
        public const int WM_ERASEBKGND = 20;
        public const int WM_HSCROLL = 0x114;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_KILLFOCUS = 8;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_MBUTTONDBLCLK = 0x209;
        public const int WM_MBUTTONDOWN = 0x207;
        public const int WM_MBUTTONUP = 520;
        public const int WM_MDIACTIVATE = 0x222;
        public const int WM_MEASUREITEM = 0x2c;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_NCLBUTTONDOWN = 0xa1;
        public const int WM_NCLBUTTONUP = 0xa2;
        public const int WM_NCPAINT = 0x85;
        public const int WM_NOTIFY = 0x4e;
        public const int WM_PAINT = 15;
        public const int WM_RBUTTONDBLCLK = 0x206;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_REFLECT = 0x2000;
        public const int WM_SETCURSOR = 0x20;
        public const int WM_SETFOCUS = 7;
        public const int WM_SETREDRAW = 11;
        public const int WM_TIMER = 0x113;
        public const int WM_TRAYMESSAGE = 0x800;
        public const int WM_USER = 0x400;
        public const int WM_VSCROLL = 0x115;
        public const int WS_EX_LAYERED = 0x80000;
        public const int WS_EX_TOOLWINDOW = 0x80;
        public const int WS_EX_TOPMOST = 8;
        public const int WS_HSCROLL = 0x100000;
        public const int WS_OVERLAPPEDWINDOW = 0xcf0000;
        public const int WS_POPUP = -2147483648;
        public const int WS_VISIBLE = 0x10000000;
        public const int WS_VSCROLL = 0x200000;
        public const int WSB_PROP_HSTYLE = 0x200;
        public const int WSB_PROP_VSTYLE = 0x100;
        public const int WTD_CHOICE_FILE = 1;
        public const int WTD_REVOKE_WHOLECHAIN = 1;
        public const int WTD_UI_ALL = 1;

        private Interop()
        {
        }

        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr BeginPaint(IntPtr hWnd, [In, Out] ref PAINTSTRUCT lpPaint);
        [DllImport("fusion.dll", ExactSpelling=true)]
        public static extern int CreateAssemblyCache(out IAssemblyCache ppAsmCache, uint dwReserved);
        [DllImport("fusion.dll", ExactSpelling=true)]
        public static extern int CreateAssemblyEnum(out IAssemblyEnum ppEnum, IntPtr pAppCtx, IAssemblyName pName, uint dwFlags, int pvReserved);
        [DllImport("user32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
        public static extern bool CreateCaret(IntPtr hWnd, IntPtr bitmap, int width, int height);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern IntPtr CreateWindowEx(int dwExStyle, string lpszClassName, string lpszWindowName, int style, int x, int y, int width, int height, IntPtr hWndParent, IntPtr hMenu, IntPtr hInst, [MarshalAs(UnmanagedType.AsAny)] object pvParam);
        [DllImport("crypt32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool CryptProtectData(ref DATA_BLOB dataIn, string szDataDescr, IntPtr pEntropy, IntPtr pvReserved, IntPtr pPromptStruct, int dwFlags, ref DATA_BLOB pDataOut);
        [DllImport("crypt32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool CryptUnprotectData(ref DATA_BLOB dataIn, StringBuilder ppszDataDescr, IntPtr pEntropy, IntPtr pvReserved, IntPtr pPromptStruct, int dwFlags, ref DATA_BLOB pDataOut);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("gdi32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("user32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
        public static extern bool DestroyCaret();
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool DestroyCursor(IntPtr hCursor);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool DestroyWindow(IntPtr hWnd);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int DispatchMessage(ref MSG msg);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool DrawCaption(IntPtr hwnd, IntPtr hdc, [In] ref RECT rect, int nFlags);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool DrawFocusRect(IntPtr hdc, ref RECT r);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool DrawFrameControl(IntPtr hdc, ref RECT r, int type, int state);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int DrawText(IntPtr hDC, string lpszString, int nCount, ref RECT lpRect, int nFormat);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);
        [DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        public static extern bool ExtTextOut(IntPtr hdc, int x, int y, int nOptions, ref RECT lpRect, string s, int nStrLength, int[] lpDx);
        [DllImport("gdi32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
        public static extern bool ExtTextOutW(IntPtr hdc, int x, int y, int nOptions, ref RECT lpRect, IntPtr text, int count, int[] lpDx);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int FillRect(IntPtr hdc, [In] ref RECT rect, IntPtr hbrush);
        [DllImport("comctl32.dll", ExactSpelling=true)]
        public static extern bool FlatSB_SetScrollProp(IntPtr hWnd, int index, IntPtr newValue, int fRedraw);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int FrameRect(IntPtr hdc, [In] ref RECT rect, IntPtr hbrush);
        [DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        public static extern bool GdiFlush();
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr GetActiveWindow();
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool GetClientRect(IntPtr hWnd, [In, Out] ref RECT rect);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool GetCursorPos([In, Out] ref POINT pt);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("gdi32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);
        [DllImport("kernel32.dll", CharSet=CharSet.Auto)]
        public static extern int GetDriveType(string lpRootPathName);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern short GetKeyState(int keyCode);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr GetLastActivePopup(IntPtr hWnd);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool GetMessage(ref MSG msg, IntPtr hwnd, int minFilter, int maxFilter);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int GetMessagePos();
        [DllImport("kernel32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string moduleName);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int GetSysColor(int nIndex);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr GetSysColorBrush(int nIndex);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("gdi32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
        public static extern bool GetTextExtentPoint32W(IntPtr hdc, string s, int count, ref SIZE size);
        [DllImport("gdi32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetTextMetrics(IntPtr hDC, TEXTMETRIC textMetric);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool GetWindowRect(IntPtr hWnd, [In, Out] ref RECT rect);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
        public static extern bool HideCaret(IntPtr hWnd);
        [DllImport("comctl32.dll", ExactSpelling=true)]
        public static extern bool InitCommonControlsEx(INITCOMMONCONTROLSEX icc);
        [DllImport("uxtheme.dll", CharSet=CharSet.Unicode)]
        public static extern int IsAppThemed();
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool IsWindow(IntPtr hWnd);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool KillTimer(IntPtr hwnd, int idEvent);
        [DllImport("kernel32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr LocalFree(IntPtr hMem);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool ModifyMenu(IntPtr hMenu, int uItem, int flags, int itemData, int newValue);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool PeekMessage([In, Out] ref MSG msg, IntPtr hwnd, int msgMin, int msgMax, int remove);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern void PostQuitMessage(int nExitCode);
        [DllImport("user32.dll", ExactSpelling=true)]
        public static extern bool PtInRect(ref RECT lprc, int x, int y);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern IntPtr RegisterClass(WNDCLASS wc);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int RegisterWindowMessage(string msg);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool ScrollWindow(IntPtr hWnd, int nXAmount, int nYAmount, ref RECT rectScrollRegion, ref RECT rectClip);
        [DllImport("gdi32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);
        [DllImport("gdi32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, [In, Out] ref RECT lParam);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, TOOLINFO lParam);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, BUTTON_IMAGELIST lParam);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("gdi32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int SetBkColor(IntPtr hdc, int color);
        [DllImport("gdi32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int SetBkMode(IntPtr hdc, int nBkMode);
        [DllImport("user32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
        public static extern bool SetCaretPos(int x, int y);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr SetCursor(IntPtr hcursor);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int SetLayeredWindowAttributes(IntPtr hwnd, int color, byte alpha, int flags);
        [DllImport("gdi32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int SetPixel(IntPtr hdc, int x, int y, int color);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int SetScrollInfo(IntPtr hWnd, int fnBar, SCROLLINFO si, bool redraw);
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true, ExactSpelling=true)]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
        [DllImport("gdi32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
        public static extern int SetTextColor(IntPtr hdc, int color);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int SetTimer(IntPtr hWnd, int nIDEvent, int uElapse, IntPtr lpTimerFunc);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern void SetWindowText(IntPtr hWnd, string lpString);
        [DllImport("shell32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool SHAddToRecentDocs(int nFlags, string path);
        [DllImport("shell32.dll", EntryPoint="SHBrowseForFolderW", ExactSpelling=true)]
        public static extern IntPtr SHBrowseForFolder([In] BROWSEINFO lpbi);
        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern int Shell_NotifyIcon(int message, NOTIFYICONDATA pnid);
        [DllImport("shell32.dll", ExactSpelling=true)]
        public static extern int SHGetFolderLocation(IntPtr hwnd, int csidl, IntPtr token, int reserved, out IntPtr ppidl);
        [DllImport("shell32.dll", ExactSpelling=true)]
        public static extern int SHGetMalloc([Out, MarshalAs(UnmanagedType.LPArray)] IMalloc[] ppMalloc);
        [DllImport("shell32.dll", EntryPoint="SHGetPathFromIDListW", ExactSpelling=true)]
        public static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);
        [DllImport("user32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
        public static extern bool ShowCaret(IntPtr hWnd);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, int fuFlags, int x, int y, IntPtr hwnd, IntPtr tpm);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool TranslateMessage(ref MSG msg);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool UpdateWindow(IntPtr hWnd);
        [DllImport("wintrust.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
        public static extern int WinVerifyTrust(IntPtr hWnd, ref Guid pgActionID, ref WINTRUST_DATA pWinTrustData);

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public class BROWSEINFO
        {
            public IntPtr hwndOwner = IntPtr.Zero;
            public IntPtr pidlRoot = IntPtr.Zero;
            public IntPtr pszDisplayName = IntPtr.Zero;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszTitle = null;
            public int ulFlags = 0;
            public IntPtr lpfn = IntPtr.Zero;
            public IntPtr lParam = IntPtr.Zero;
            public int iImage = 0;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public class BUTTON_IMAGELIST
        {
            public IntPtr hImageList = IntPtr.Zero;
            public int leftMargin = 0;
            public int topMargin = 0;
            public int rightMargin = 0;
            public int bottomMargin = 0;
            public int align = 0;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode), ComVisible(false)]
        public struct DATA_BLOB
        {
            public int cbData;
            public IntPtr pbData;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public class DRAWITEMSTRUCT
        {
            public int CtlType = 0;
            public int CtlID = 0;
            public int itemID = 0;
            public int itemAction = 0;
            public int itemState = 0;
            public IntPtr hwndItem = IntPtr.Zero;
            public IntPtr hDC = IntPtr.Zero;
            public Interop.RECT rcItem = new Interop.RECT(0, 0, 0, 0);
            public IntPtr itemData = IntPtr.Zero;
        }

        [ComImport, Guid("e707dcde-d1cd-11d2-bab9-00c04f8eceae"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IAssemblyCache
        {
            [PreserveSig]
            int UninstallAssembly(uint dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName, IntPtr pvReserved, out uint pulDisposition);
            [PreserveSig]
            int QueryAssemblyInfo(uint dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName, IntPtr pAsmInfo);
            [PreserveSig]
            int CreateAssemblyCacheItem(uint dwFlags, IntPtr pvReserved, out IntPtr ppAsmItem, [MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName);
            [PreserveSig]
            int CreateAssemblyScavenger(out object ppAsmScavenger);
            [PreserveSig]
            int InstallAssembly(uint dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string pszManifestFilePath, IntPtr pvReserved);
        }

        [ComImport, Guid("21b8916c-f28e-11d2-a473-00c04f8ef448"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IAssemblyEnum
        {
            [PreserveSig]
            int GetNextAssembly(IntPtr appCtx, out Interop.IAssemblyName ppName, uint dwFlags);
            [PreserveSig]
            int Reset();
            [PreserveSig]
            int Clone(out Interop.IAssemblyEnum ppEnum);
        }

        [ComImport, Guid("CD193BC0-B4BC-11d2-9833-00C04FC31D2E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IAssemblyName
        {
            [PreserveSig]
            int SetProperty(uint PropertyId, IntPtr pvProperty, uint cbProperty);
            [PreserveSig]
            int GetProperty(uint PropertyId, IntPtr pvProperty, ref uint pcbProperty);
            [PreserveSig]
            int Finalize();
            [PreserveSig]
            int GetDisplayName(IntPtr szDisplayName, ref uint pccDisplayName, uint dwDisplayFlags);
            [PreserveSig]
            int BindToObject(ref Guid refIID, IntPtr pAsmBindSink, IntPtr pApplicationContext, [MarshalAs(UnmanagedType.LPWStr)] string szCodeBase, long llFlags, int pvReserved, uint cbReserved, out int ppv);
            [PreserveSig]
            int GetName(out uint lpcwBuffer, out int pwzName);
            [PreserveSig]
            int GetVersion(out uint pdwVersionHi, out uint pdwVersionLow);
            [PreserveSig]
            int IsEqual(Interop.IAssemblyName pName, uint dwCmpFlags);
            [PreserveSig]
            int Clone(out Interop.IAssemblyName pName);
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000002-0000-0000-c000-000000000046")]
        public interface IMalloc
        {
            IntPtr Alloc(int cb);
            void Free(IntPtr pv);
            IntPtr Realloc(IntPtr pv, int cb);
            int GetSize(IntPtr pv);
            int DidAlloc(IntPtr pv);
            void HeapMinimize();
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public class INITCOMMONCONTROLSEX
        {
            public int dwSize = 0;
            public int dwICC = 0;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public class MEASUREITEMSTRUCT
        {
            public int CtlType = 0;
            public int CtlID = 0;
            public int itemID = 0;
            public int itemWidth = 0;
            public int itemHeight = 0;
            public IntPtr itemData = IntPtr.Zero;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public struct MSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int time;
            public int pt_x;
            public int pt_y;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public class NMCUSTOMDRAW
        {
            public Interop.NMHDR nmcd = null;
            public int dwDrawStage = 0;
            public IntPtr hdc = IntPtr.Zero;
            public Interop.RECT rc = new Interop.RECT(0, 0, 0, 0);
            public int dwItemSpec = 0;
            public int uItemState = 0;
            public IntPtr lItemlParam = IntPtr.Zero;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public class NMHDR
        {
            public IntPtr hwndFrom = IntPtr.Zero;
            public int idFrom = 0;
            public int code = 0;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public class NMTBCUSTOMDRAW
        {
            public Interop.NMCUSTOMDRAW nmcd = null;
            public IntPtr hbrMonoDither = IntPtr.Zero;
            public IntPtr hbrLines = IntPtr.Zero;
            public IntPtr hpenLines = IntPtr.Zero;
            public int clrText = 0;
            public int clrMark = 0;
            public int clrTextHighlight = 0;
            public int clrBtnFace = 0;
            public int clrBtnHighlight = 0;
            public int clrHighlightHotTrack = 0;
            public Interop.RECT rc = new Interop.RECT(0, 0, 0, 0);
            public int nStringBkMode = 0;
            public int nHLStringBkMode = 0;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto), ComVisible(false)]
        public class NOTIFYICONDATA
        {
            public int cbSize = Marshal.SizeOf(typeof(Interop.NOTIFYICONDATA));
            public IntPtr hWnd = IntPtr.Zero;
            public int uID = 0;
            public int uFlags = 0;
            public int uCallbackMessage = 0;
            public IntPtr hIcon = IntPtr.Zero;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x80)]
            public string szTip = null;
            public int dwState = 0;
            public int dwStateMask = 0;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x100)]
            public string szInfo = null;
            public int uTimeout = 0;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x40)]
            public string szTitle = null;
            public int dwInfoFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public int rcPaint_left;
            public int rcPaint_top;
            public int rcPaint_right;
            public int rcPaint_bottom;
            public bool fRestore;
            public bool fIncUpdate;
            public int reserved1;
            public int reserved2;
            public int reserved3;
            public int reserved4;
            public int reserved5;
            public int reserved6;
            public int reserved7;
            public int reserved8;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(true)]
        public struct POINT
        {
            public int x;
            public int y;
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(true)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public static Interop.RECT FromXYWH(int x, int y, int width, int height)
            {
                return new Interop.RECT(x, y, x + width, y + height);
            }

            public int Height
            {
                get
                {
                    return (this.bottom - this.top);
                }
            }
            public int Width
            {
                get
                {
                    return (this.right - this.left);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class SCROLLINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(Interop.SCROLLINFO));
            public int fMask = 0;
            public int nMin = 0;
            public int nMax = 0;
            public int nPage = 0;
            public int nPos = 0;
            public int nTrackPos = 0;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(true)]
        public struct SIZE
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto), ComVisible(false)]
        public class TEXTMETRIC
        {
            public int tmHeight = 0;
            public int tmAscent = 0;
            public int tmDescent = 0;
            public int tmInternalLeading = 0;
            public int tmExternalLeading = 0;
            public int tmAveCharWidth = 0;
            public int tmMaxCharWidth = 0;
            public int tmWeight = 0;
            public int tmOverhang = 0;
            public int tmDigitizedAspectX = 0;
            public int tmDigitizedAspectY = 0;
            public char tmFirstChar = '\0';
            public char tmLastChar = '\0';
            public char tmDefaultChar = '\0';
            public char tmBreakChar = '\0';
            public byte tmItalic = 0;
            public byte tmUnderlined = 0;
            public byte tmStruckOut = 0;
            public byte tmPitchAndFamily = 0;
            public byte tmCharSet = 0;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto), ComVisible(false)]
        public class TOOLINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(Interop.TOOLINFO));
            public int uFlags = 0;
            public IntPtr hwnd = IntPtr.Zero;
            public IntPtr uId = IntPtr.Zero;
            public Interop.RECT rect = new Interop.RECT(0, 0, 0, 0);
            public IntPtr hinst = IntPtr.Zero;
            public string lpszText = null;
            public IntPtr lParam = IntPtr.Zero;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        public struct WINTRUST_DATA
        {
            public int cbStruct;
            public IntPtr pPolicyCallbackData;
            public IntPtr pSIPClientData;
            public int dwUIChoice;
            public int fdwRevocationChecks;
            public int dwUnionChoice;
            public IntPtr pSomething;
            public int dwStateAction;
            public IntPtr hWVTStateData;
            public IntPtr pwszURLReference;
            public int dwProvFlags;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        public struct WINTRUST_FILE_INFO
        {
            public int cbStruct;
            public IntPtr pcwszFilePath;
            public IntPtr hFile;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto), ComVisible(false)]
        public class WNDCLASS
        {
            public int style = 0;
            public Interop.WNDPROC lpfnWndProc = null;
            public int cbClsExtra = 0;
            public int cbWndExtra = 0;
            public IntPtr hInstance = IntPtr.Zero;
            public IntPtr hIcon = IntPtr.Zero;
            public IntPtr hCursor = IntPtr.Zero;
            public IntPtr hbrBackground = IntPtr.Zero;
            public string lpszMenuName = null;
            public string lpszClassName = null;
        }

        public delegate int WNDPROC(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
    }
}

