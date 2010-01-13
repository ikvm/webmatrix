namespace Microsoft.Matrix.Packages.Web
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    public sealed class Interop
    {
        #region constants
        public const int BEHAVIOR_EVENT_APPLYSTYLE = 2;
        public const int BEHAVIOR_EVENT_CONTENTREADY = 0;
        public const int BEHAVIOR_EVENT_CONTENTSAVE = 4;
        public const int BEHAVIOR_EVENT_DOCUMENTCONTEXTCHANGE = 3;
        public const int BEHAVIOR_EVENT_DOCUMENTREADY = 1;
        public const int BITMAPINFO_MAX_COLORSIZE = 0x100;
        public const int DISPID_HTMLELEMENTEVENTS_ONDBLCLICK = -601;
        public const int DISPID_HTMLELEMENTEVENTS_ONDRAG = -2147418092;
        public const int DISPID_HTMLELEMENTEVENTS_ONDRAGSTART = -2147418101;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOUSEDOWN = -605;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOUSEMOVE = -606;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOUSEUP = -607;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOVE = 0x40b;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOVEEND = 0x40f;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOVESTART = 0x40e;
        public const int DISPID_HTMLELEMENTEVENTS_ONRESIZEEND = 0x411;
        public const int DISPID_HTMLELEMENTEVENTS_ONRESIZESTART = 0x410;
        public const int DISPID_READYSTATE = -525;
        public const int DISPID_XOBJ_BASE = -2147418112;
        public const int DISPID_XOBJ_MAX = -2147352577;
        public const int DISPID_XOBJ_MIN = -2147418112;
        public const int DOCHOSTUIDBLCLICK_DEFAULT = 0;
        public const int DOCHOSTUIDBLCLICK_SHOWCODE = 2;
        public const int DOCHOSTUIDBLCLICK_SHOWPROPERTIES = 1;
        public const int DOCHOSTUIFLAG_ACTIVATE_CLIENTHIT_ONLY = 0x200;
        public const int DOCHOSTUIFLAG_DIALOG = 1;
        public const int DOCHOSTUIFLAG_DISABLE_COOKIE = 0x400;
        public const int DOCHOSTUIFLAG_DISABLE_HELP_MENU = 2;
        public const int DOCHOSTUIFLAG_DISABLE_OFFSCREEN = 0x40;
        public const int DOCHOSTUIFLAG_DISABLE_SCRIPT_INACTIVE = 0x10;
        public const int DOCHOSTUIFLAG_DIV_BLOCKDEFAULT = 0x100;
        public const int DOCHOSTUIFLAG_ENABLE_INPLACE_NAVIGATION = 0x10000;
        public const int DOCHOSTUIFLAG_FLAT_SCROLLBAR = 0x80;
        public const int DOCHOSTUIFLAG_NO3DBORDER = 4;
        public const int DOCHOSTUIFLAG_NOTHEME = 0x80000;
        public const int DOCHOSTUIFLAG_OPENNEWWIN = 0x20;
        public const int DOCHOSTUIFLAG_SCROLL_NO = 8;
        public const int DOCHOSTUIFLAG_THEME = 0x40000;
        public const int DROPEFFECT_COPY = 1;
        public const int DROPEFFECT_LINK = 4;
        public const int DROPEFFECT_MOVE = 2;
        public const int DROPEFFECT_NONE = 0;
        public const int E_ABORT = -2147467260;
        public const int E_ACCESSDENIED = -2147024891;
        public const int E_FAIL = -2147467259;
        public const int E_HANDLE = -2147024890;
        public const int E_INVALIDARG = -2147024809;
        public const int E_NOINTERFACE = -2147467262;
        public const int E_NOTIMPL = -2147467263;
        public const int E_OUTOFMEMORY = -2147024882;
        public const int E_POINTER = -2147467261;
        public const int E_UNEXPECTED = -2147418113;
        public const int ELEMENT_CORNER_BOTTOM = 3;
        public const int ELEMENT_CORNER_BOTTOMLEFT = 7;
        public const int ELEMENT_CORNER_BOTTOMRIGHT = 8;
        public const int ELEMENT_CORNER_LEFT = 2;
        public const int ELEMENT_CORNER_NONE = 0;
        public const int ELEMENT_CORNER_RIGHT = 4;
        public const int ELEMENT_CORNER_TOP = 1;
        public const int ELEMENT_CORNER_TOPLEFT = 5;
        public const int ELEMENT_CORNER_TOPRIGHT = 6;
        public static Guid ElementBehaviorFactory = new Guid("3050f429-98b5-11cf-bb82-00aa00bdce0b");
        public const int ELEMENTDESCRIPTOR_FLAGS_LITERAL = 1;
        public const int ELEMENTDESCRIPTOR_FLAGS_NESTED_LITERAL = 2;
        public const int ELEMENTNAMESPACE_FLAGS_ALLOWANYTAG = 1;
        public const int ELEMENTNAMESPACE_FLAGS_QUERYFORUNKNOWNTAGS = 2;
        public static Guid Guid_MSHTML = new Guid("DE4BA900-59CA-11CF-9592-444553540000");
        public const int HTMLPAINT_ZORDER_ABOVE_CONTENT = 7;
        public const int HTMLPAINT_ZORDER_ABOVE_FLOW = 6;
        public const int HTMLPAINT_ZORDER_BELOW_CONTENT = 4;
        public const int HTMLPAINT_ZORDER_BELOW_FLOW = 5;
        public const int HTMLPAINT_ZORDER_NONE = 0;
        public const int HTMLPAINT_ZORDER_REPLACE_ALL = 1;
        public const int HTMLPAINT_ZORDER_REPLACE_BACKGROUND = 3;
        public const int HTMLPAINT_ZORDER_REPLACE_CONTENT = 2;
        public const int HTMLPAINT_ZORDER_WINDOW_TOP = 8;
        public const int HTMLPAINTER_3DSURFACE = 0x200;
        public const int HTMLPAINTER_ALPHA = 4;
        public const int HTMLPAINTER_COMPLEX = 8;
        public const int HTMLPAINTER_HITTEST = 0x20;
        public const int HTMLPAINTER_NOBAND = 0x400;
        public const int HTMLPAINTER_NODC = 0x1000;
        public const int HTMLPAINTER_NOPHYSICALCLIP = 0x2000;
        public const int HTMLPAINTER_NOSAVEDC = 0x4000;
        public const int HTMLPAINTER_OPAQUE = 1;
        public const int HTMLPAINTER_OVERLAY = 0x10;
        public const int HTMLPAINTER_SURFACE = 0x100;
        public const int HTMLPAINTER_TRANSPARENT = 2;
        public const int IDM_1D_ELEMENT = 0x95c;
        public const int IDM_2D_ELEMENT = 0x95b;
        public const int IDM_2D_POSITION = 0x95a;
        public const int IDM_ABSOLUTE_POSITION = 0x95d;
        public const int IDM_ADDTOGLYPHTABLE = 0x921;
        public const int IDM_ATOMICSELECTION = 0x95f;
        public const int IDM_BACKCOLOR = 0x33;
        public const int IDM_BLOCKFMT = 0x8ba;
        public const int IDM_BOLD = 0x34;
        public const int IDM_BUTTON = 0x877;
        public const int IDM_CHECKBOX = 0x873;
        public const int IDM_CLEARSELECTION = 0x7d7;
        public const int IDM_COPY = 15;
        public const int IDM_CSSEDITING_LEVEL = 0x966;
        public const int IDM_CUT = 0x10;
        public const int IDM_DELETE = 0x11;
        public const int IDM_DROPDOWNBOX = 0x875;
        public const int IDM_EMPTYGLYPHTABLE = 0x920;
        public const int IDM_FONTNAME = 0x12;
        public const int IDM_FONTSIZE = 0x13;
        public const int IDM_FORECOLOR = 0x37;
        public const int IDM_HYPERLINK = 0x84c;
        public const int IDM_IMAGE = 0x878;
        public const int IDM_INDENT = 0x88a;
        public const int IDM_ITALIC = 0x38;
        public const int IDM_JUSTIFYCENTER = 0x39;
        public const int IDM_JUSTIFYLEFT = 0x3b;
        public const int IDM_JUSTIFYRIGHT = 60;
        public const int IDM_LISTBOX = 0x876;
        public const int IDM_LIVERESIZE = 0x95e;
        public const int IDM_MULTIPLESELECTION = 0x959;
        public const int IDM_NOACTIVATEDESIGNTIMECONTROLS = 0x91d;
        public const int IDM_NOACTIVATEJAVAAPPLETS = 0x91e;
        public const int IDM_NOACTIVATENORMALOLECONTROLS = 0x91c;
        public const int IDM_NOFIXUPURLSONPASTE = 0x91f;
        public const int IDM_ORDERLIST = 0x888;
        public const int IDM_OUTDENT = 0x88b;
        public const int IDM_PASTE = 0x1a;
        public const int IDM_PERSISTDEFAULTVALUES = 0x1bbc;
        public const int IDM_PRESERVEUNDOALWAYS = 0x17a1;
        public const int IDM_PROTECTMETATAGS = 0x1bbd;
        public const int IDM_RADIOBUTTON = 0x874;
        public const int IDM_REDO = 0x1d;
        public const int IDM_REMOVEFROMGLYPHTABLE = 0x922;
        public const int IDM_REPLACEGLYPHCONTENTS = 0x923;
        public const int IDM_RESPECTVISIBILITY_INDESIGN = 0x965;
        public const int IDM_SELECTALL = 0x1f;
        public const int IDM_SETDIRTY = 0x926;
        public const int IDM_SHOWZEROBORDERATDESIGNTIME = 0x918;
        public const int IDM_STRIKETHROUGH = 0x5b;
        public const int IDM_SUBSCRIPT = 0x8c7;
        public const int IDM_SUPERSCRIPT = 0x8c8;
        public const int IDM_TEXTAREA = 0x872;
        public const int IDM_TEXTBOX = 0x871;
        public const int IDM_UNDERLINE = 0x3f;
        public const int IDM_UNDO = 0x2b;
        public const int IDM_UNLINK = 0x84d;
        public const int IDM_UNORDERLIST = 0x889;
        public static Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");
        public const int INET_E_DEFAULT_ACTION = -2146697199;
        public const int INET_E_USE_DEFAULT_PROTOCOLHANDLER = -2146697199;
        public static IntPtr NullIntPtr = IntPtr.Zero;
        public const int OLECLOSE_NOSAVE = 1;
        public const int OLEIVERB_DISCARDUNDOSTATE = -6;
        public const int OLEIVERB_HIDE = -3;
        public const int OLEIVERB_INPLACEACTIVATE = -5;
        public const int OLEIVERB_OPEN = -2;
        public const int OLEIVERB_PRIMARY = 0;
        public const int OLEIVERB_PROPERTIES = -7;
        public const int OLEIVERB_SHOW = -1;
        public const int OLEIVERB_UIACTIVATE = -4;
        public const int S_FALSE = 1;
        public const int S_OK = 0;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYFIRST = 0x100;
        public const int WM_KEYLAST = 0x108;
        public const int WM_KEYUP = 0x101;
        #endregion

        private Interop()
        {
        }

        [DllImport("urlmon.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
        internal static extern int CoInternetCombineUrl([In, MarshalAs(UnmanagedType.LPWStr)] string pwzBaseUrl, [In, MarshalAs(UnmanagedType.LPWStr)] string pwzRelativeUrl, [In] int dwCombineFlags, [Out] IntPtr pszResult, [In] int cchResult, out int pcchResult, [In] int dwReserved);
        [DllImport("urlmon.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
        internal static extern int CoInternetGetSession(int dwSessionMode, out IInternetSession session, int dwReserved);
        [DllImport("ole32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int CreateBindCtx(int dwReserved, out IBindCtx ppbc);

        //NOTE: 从MSDN上拷贝来的
        //[DllImport("ole32.dll", PreserveSig=false)]
        //public static extern void CreateStreamOnHGlobal(IntPtr hGlobal, bool fDeleteOnRelease, out IStream pStream);

        [SecurityCritical, DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int CreateStreamOnHGlobal(IntPtr hGlobal, bool fDeleteOnRelease, ref System.Runtime.InteropServices.ComTypes.IStream istream);

        [DllImport("urlmon.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
        internal static extern int CreateURLMoniker(IMoniker pmkContext, string szURL, out IMoniker ppmk);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool GetClientRect(IntPtr hWnd, [In, Out] COMRECT rect);
        [DllImport("ole32.dll", PreserveSig=false)]
        public static extern void GetHGlobalFromStream(System.Runtime.InteropServices.ComTypes.IStream pStream, out IntPtr pHGlobal);
        [DllImport("kernel32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr GlobalLock(IntPtr handle);
        [DllImport("kernel32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool GlobalUnlock(IntPtr handle);
        [DllImport("gdi32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool LineTo(IntPtr hdc, int x, int y);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);
        [DllImport("gdi32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int SetPixel(IntPtr hDC, int x, int y, int crColor);

        [Flags]
        public enum BSCFlags
        {
            BSCF_AVAILABLEDATASIZEUNKNOWN = 0x10,
            BSCF_DATAFULLYAVAILABLE = 8,
            BSCF_FIRSTDATANOTIFICATION = 1,
            BSCF_INTERMEDIATEDATANOTIFICATION = 2,
            BSCF_LASTDATANOTIFICATION = 4
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public class COMMSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int time;
            public int pt_x;
            public int pt_y;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(true)]
        public class COMRECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public COMRECT()
            {
            }

            public COMRECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public static Interop.COMRECT FromXYWH(int x, int y, int width, int height)
            {
                return new Interop.COMRECT(x, y, x + width, y + height);
            }
        }

        [ComVisible(false)]
        public class ConnectionPointCookie
        {
            private UCOMIConnectionPoint connectionPoint;
            private int cookie;

            public ConnectionPointCookie(object source, object sink, Type eventInterface) : this(source, sink, eventInterface, true)
            {
            }

            public ConnectionPointCookie(object source, object sink, Type eventInterface, bool throwException)
            {
                Exception exception = null;
                if (source is UCOMIConnectionPointContainer)
                {
                    UCOMIConnectionPointContainer container = (UCOMIConnectionPointContainer) source;
                    try
                    {
                        Guid gUID = eventInterface.GUID;
                        container.FindConnectionPoint(ref gUID, out this.connectionPoint);
                    }
                    catch (Exception)
                    {
                        this.connectionPoint = null;
                    }
                    if (this.connectionPoint == null)
                    {
                        exception = new ArgumentException("The source object does not expose the " + eventInterface.Name + " event inteface");
                        goto Label_00C1;
                    }
                    if (!eventInterface.IsInstanceOfType(sink))
                    {
                        exception = new InvalidCastException("The sink object does not implement the eventInterface");
                        goto Label_00C1;
                    }
                    try
                    {
                        this.connectionPoint.Advise(sink, out this.cookie);
                        goto Label_00C1;
                    }
                    catch
                    {
                        this.cookie = 0;
                        this.connectionPoint = null;
                        exception = new Exception("IConnectionPoint::Advise failed for event interface '" + eventInterface.Name + "'");
                        goto Label_00C1;
                    }
                }
                exception = new InvalidCastException("The source object does not expost IConnectionPointContainer");
            Label_00C1:
                if (!throwException || ((this.connectionPoint != null) && (this.cookie != 0)))
                {
                    return;
                }
                if (exception == null)
                {
                    throw new ArgumentException("Could not create connection point for event interface '" + eventInterface.Name + "'");
                }
                throw exception;
            }

            public void Disconnect()
            {
                if ((this.connectionPoint != null) && (this.cookie != 0))
                {
                    try
                    {
                        this.connectionPoint.Unadvise(this.cookie);
                        this.cookie = 0;
                        this.connectionPoint = null;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
            }

            ~ConnectionPointCookie()
            {
                this.Disconnect();
            }
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public sealed class DISPPARAMS
        {
            public IntPtr rgvarg;
            public IntPtr rgdispidNamedArgs;
            [MarshalAs(UnmanagedType.U4)]
            public int cArgs;
            [MarshalAs(UnmanagedType.U4)]
            public int cNamedArgs;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(true)]
        public class DOCHOSTUIINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.I4)]
            public int dwFlags;
            [MarshalAs(UnmanagedType.I4)]
            public int dwDoubleClick;
            [MarshalAs(UnmanagedType.I4)]
            public int dwReserved1;
            [MarshalAs(UnmanagedType.I4)]
            public int dwReserved2;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public sealed class EXCEPINFO
        {
            [MarshalAs(UnmanagedType.U2)]
            public short wCode;
            [MarshalAs(UnmanagedType.U2)]
            public short wReserved;
            [MarshalAs(UnmanagedType.BStr)]
            public string bstrSource;
            [MarshalAs(UnmanagedType.BStr)]
            public string bstrDescription;
            [MarshalAs(UnmanagedType.BStr)]
            public string bstrHelpFile;
            [MarshalAs(UnmanagedType.U4)]
            public int dwHelpContext;
            public IntPtr dwReserved;
            public IntPtr dwFillIn;
            [MarshalAs(UnmanagedType.I4)]
            public int scode;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public sealed class FORMATETC
        {
            [MarshalAs(UnmanagedType.I4)]
            public int cfFormat;
            public IntPtr ptd;
            [MarshalAs(UnmanagedType.I4)]
            public int dwAspect;
            [MarshalAs(UnmanagedType.I4)]
            public int lindex;
            [MarshalAs(UnmanagedType.I4)]
            public int tymed;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(true)]
        public class HTML_PAINTER_INFO
        {
            [MarshalAs(UnmanagedType.I4)]
            public int lFlags;
            [MarshalAs(UnmanagedType.I4)]
            public int lZOrder;
            [MarshalAs(UnmanagedType.Struct)]
            public Guid iidDrawObject;
            [MarshalAs(UnmanagedType.Struct)]
            public Interop.RECT rcBounds;
        }

        [ComImport, Guid("25336920-03F9-11CF-8FD0-00AA00686F13"), ComVisible(true)]
        public class HTMLDocument
        {
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("0000010F-0000-0000-C000-000000000046")]
        public interface IAdviseSink
        {
            void OnDataChange([In] Interop.FORMATETC pFormatetc, [In] Interop.STGMEDIUM pStgmed);
            void OnViewChange([In, MarshalAs(UnmanagedType.U4)] int dwAspect, [In, MarshalAs(UnmanagedType.I4)] int lindex);
            void OnRename([In, MarshalAs(UnmanagedType.Interface)] object pmk);
            void OnSave();
            void OnClose();
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000000e-0000-0000-C000-000000000046"), ComVisible(true)]
        public interface IBindCtx
        {
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("00000001-0000-0000-C000-000000000046")]
        public interface IClassFactory
        {
            [PreserveSig]
            int CreateInstance([In, MarshalAs(UnmanagedType.Interface)] object pUnkOuter, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object obj);
            [PreserveSig]
            int LockServer([In] bool fLock);
        }

        [ComImport, Guid("BD3F23C0-D43E-11CF-893B-00AA00BDCE1A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true)]
        public interface IDocHostUIHandler
        {
            [PreserveSig]
            int ShowContextMenu([In, MarshalAs(UnmanagedType.U4)] int dwID, [In] Interop.POINT pt, [In, MarshalAs(UnmanagedType.Interface)] object pcmdtReserved, [In, MarshalAs(UnmanagedType.Interface)] object pdispReserved);
            [PreserveSig]
            int GetHostInfo([In, Out] Interop.DOCHOSTUIINFO info);
            [PreserveSig]
            int ShowUI([In, MarshalAs(UnmanagedType.I4)] int dwID, [In, MarshalAs(UnmanagedType.Interface)] Interop.IOleInPlaceActiveObject activeObject, [In, MarshalAs(UnmanagedType.Interface)] Interop.IOleCommandTarget commandTarget, [In, MarshalAs(UnmanagedType.Interface)] Interop.IOleInPlaceFrame frame, [In, MarshalAs(UnmanagedType.Interface)] Interop.IOleInPlaceUIWindow doc);
            [PreserveSig]
            int HideUI();
            [PreserveSig]
            int UpdateUI();
            [PreserveSig]
            int EnableModeless([In, MarshalAs(UnmanagedType.Bool)] bool fEnable);
            [PreserveSig]
            int OnDocWindowActivate([In, MarshalAs(UnmanagedType.Bool)] bool fActivate);
            [PreserveSig]
            int OnFrameWindowActivate([In, MarshalAs(UnmanagedType.Bool)] bool fActivate);
            [PreserveSig]
            int ResizeBorder([In] Interop.COMRECT rect, [In] Interop.IOleInPlaceUIWindow doc, [In] bool fFrameWindow);
            [PreserveSig]
            int TranslateAccelerator([In] Interop.COMMSG msg, [In] ref Guid group, [In, MarshalAs(UnmanagedType.I4)] int nCmdID);
            [PreserveSig]
            int GetOptionKeyPath([Out, MarshalAs(UnmanagedType.LPArray)] string[] pbstrKey, [In, MarshalAs(UnmanagedType.U4)] int dw);
            [PreserveSig]
            int GetDropTarget([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleDropTarget pDropTarget, [MarshalAs(UnmanagedType.Interface)] out Interop.IOleDropTarget ppDropTarget);
            [PreserveSig]
            int GetExternal([MarshalAs(UnmanagedType.Interface)] out object ppDispatch);
            [PreserveSig]
            int TranslateUrl([In, MarshalAs(UnmanagedType.U4)] int dwTranslate, [In, MarshalAs(UnmanagedType.LPWStr)] string strURLIn, [MarshalAs(UnmanagedType.LPWStr)] out string pstrURLOut);
            [PreserveSig]
            int FilterDataObject([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleDataObject pDO, [MarshalAs(UnmanagedType.Interface)] out Interop.IOleDataObject ppDORet);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("3050F425-98B5-11CF-BB82-00AA00BDCE0B")]
        public interface IElementBehavior
        {
            void Init([In, MarshalAs(UnmanagedType.Interface)] Interop.IElementBehaviorSite pBehaviorSite);
            void Notify([In, MarshalAs(UnmanagedType.U4)] int dwEvent, [In] IntPtr pVar);
            void Detach();
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("3050F429-98B5-11CF-BB82-00AA00BDCE0B"), ComVisible(true)]
        public interface IElementBehaviorFactory
        {
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IElementBehavior FindBehavior([In, MarshalAs(UnmanagedType.BStr)] string bstrBehavior, [In, MarshalAs(UnmanagedType.BStr)] string bstrBehaviorUrl, [In, MarshalAs(UnmanagedType.Interface)] Interop.IElementBehaviorSite pSite);
        }

        [Guid("3050F427-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true)]
        public interface IElementBehaviorSite
        {
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement GetElement();
            void RegisterNotification([In, MarshalAs(UnmanagedType.I4)] int lEvent);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("3050F659-98B5-11CF-BB82-00AA00BDCE0B")]
        public interface IElementBehaviorSiteOM2
        {
            [return: MarshalAs(UnmanagedType.I4)]
            int RegisterEvent([In, MarshalAs(UnmanagedType.BStr)] string pchEvent, [In, MarshalAs(UnmanagedType.I4)] int lFlags);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetEventCookie([In, MarshalAs(UnmanagedType.BStr)] string pchEvent);
            void FireEvent([In, MarshalAs(UnmanagedType.I4)] int lCookie, [In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLEventObj pEventObject);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLEventObj CreateEventObject();
            void RegisterName([In, MarshalAs(UnmanagedType.BStr)] string pchName);
            void RegisterUrn([In, MarshalAs(UnmanagedType.BStr)] string pchUrn);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElementDefaults GetDefaults();
        }

        [ComVisible(true), Guid("3050F671-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IElementNamespace
        {
            void AddTag([In, MarshalAs(UnmanagedType.BStr)] string tagName, [In, MarshalAs(UnmanagedType.I4)] int lFlags);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("3050F672-98B5-11CF-BB82-00AA00BDCE0B")]
        public interface IElementNamespaceFactory
        {
            void Create([In, MarshalAs(UnmanagedType.Interface)] Interop.IElementNamespace pNamespace);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("3050F7FD-98B5-11CF-BB82-00AA00BDCE0B"), ComVisible(true)]
        public interface IElementNamespaceFactoryCallback
        {
            void Resolve([In, MarshalAs(UnmanagedType.BStr)] string nameSpace, [In, MarshalAs(UnmanagedType.BStr)] string tagName, [In, MarshalAs(UnmanagedType.BStr)] string attributes, [In, MarshalAs(UnmanagedType.Interface)] Interop.IElementNamespace pNamespace);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("3050F670-98B5-11CF-BB82-00AA00BDCE0B")]
        public interface IElementNamespaceTable
        {
            void AddNamespace([In, MarshalAs(UnmanagedType.BStr)] string nameSpace, [In, MarshalAs(UnmanagedType.BStr)] string urn, [In, MarshalAs(UnmanagedType.I4)] int lFlags, [In] ref object factory);
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("00000103-0000-0000-C000-000000000046")]
        public interface IEnumFORMATETC
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Next([In, MarshalAs(UnmanagedType.U4)] int celt, [Out] Interop.FORMATETC rgelt, [In, Out, MarshalAs(UnmanagedType.LPArray)] int[] pceltFetched);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Skip([In, MarshalAs(UnmanagedType.U4)] int celt);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Reset();
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Clone([Out, MarshalAs(UnmanagedType.LPArray)] Interop.IEnumFORMATETC[] ppenum);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("B3E7C340-EF97-11CE-9BC9-00AA00608E01"), ComVisible(true)]
        public interface IEnumOleUndoUnits
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Next([In, MarshalAs(UnmanagedType.U4)] int numDesired, out IntPtr unit, [MarshalAs(UnmanagedType.U4)] out int numReceived);
            void Bogus();
            [PreserveSig]
            int Skip([In, MarshalAs(UnmanagedType.I4)] int numToSkip);
            [PreserveSig]
            int Reset();
            [PreserveSig]
            int Clone([Out, MarshalAs(UnmanagedType.Interface)] Interop.IEnumOleUndoUnits enumerator);
        }

        [ComImport, ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000104-0000-0000-C000-000000000046")]
        public interface IEnumOLEVERB
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Next([MarshalAs(UnmanagedType.U4)] int celt, [Out] Interop.tagOLEVERB rgelt, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pceltFetched);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Skip([In, MarshalAs(UnmanagedType.U4)] int celt);
            void Reset();
            void Clone(out Interop.IEnumOLEVERB ppenum);
        }

        [ComVisible(true), Guid("00000105-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IEnumSTATDATA
        {
            void Next([In, MarshalAs(UnmanagedType.U4)] int celt, [Out] Interop.STATDATA rgelt, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pceltFetched);
            void Skip([In, MarshalAs(UnmanagedType.U4)] int celt);
            void Reset();
            void Clone([Out, MarshalAs(UnmanagedType.LPArray)] Interop.IEnumSTATDATA[] ppenum);
        }

        [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIDispatch), Guid("3050f1d8-98b5-11cf-bb82-00aa00bdce0b")]
        public interface IHtmlBodyElement
        {
            void put_background([In, MarshalAs(UnmanagedType.BStr)] string v);
            [return: MarshalAs(UnmanagedType.BStr)]
            string get_background();
            void put_bgProperties([In, MarshalAs(UnmanagedType.BStr)] string v);
            [return: MarshalAs(UnmanagedType.BStr)]
            string get_bgProperties();
            void put_leftMargin([In, MarshalAs(UnmanagedType.Interface)] object o);
            [return: MarshalAs(UnmanagedType.Interface)]
            object get_leftMargin();
            void put_topMargin([In, MarshalAs(UnmanagedType.Interface)] object o);
            [return: MarshalAs(UnmanagedType.Interface)]
            object get_topMargin();
            void put_rightMargin([In, MarshalAs(UnmanagedType.Interface)] object o);
            [return: MarshalAs(UnmanagedType.Interface)]
            object get_rightMargin();
            void put_bottomMargin([In, MarshalAs(UnmanagedType.Interface)] object o);
            [return: MarshalAs(UnmanagedType.Interface)]
            object get_bottomMargin();
            void put_noWrap([In, MarshalAs(UnmanagedType.Interface)] object o);
            [return: MarshalAs(UnmanagedType.Interface)]
            object get_noWrap();
            void put_bgColor([In, MarshalAs(UnmanagedType.Interface)] object o);
            [return: MarshalAs(UnmanagedType.Interface)]
            object get_bgColor();
            void put_text([In, MarshalAs(UnmanagedType.Interface)] object o);
            [return: MarshalAs(UnmanagedType.Interface)]
            object get_text();
            void put_link([In, MarshalAs(UnmanagedType.Interface)] object o);
            [return: MarshalAs(UnmanagedType.Interface)]
            object get_link();
            void put_vLink([In, MarshalAs(UnmanagedType.Interface)] object o);
            [return: MarshalAs(UnmanagedType.Interface)]
            object get_vLink();
            void put_aLink([In, MarshalAs(UnmanagedType.Interface)] object o);
            [return: MarshalAs(UnmanagedType.Interface)]
            object get_aLink();
            void put_onload([In, MarshalAs(UnmanagedType.Interface)] object o);
            [return: MarshalAs(UnmanagedType.Interface)]
            object get_onload();
            void put_onunload([In, MarshalAs(UnmanagedType.Interface)] object o);
            [return: MarshalAs(UnmanagedType.Interface)]
            object get_onunload();
            void put_scroll([In, MarshalAs(UnmanagedType.BStr)] string s);
            [return: MarshalAs(UnmanagedType.BStr)]
            string get_scroll();
            void put_onselect([In, MarshalAs(UnmanagedType.Interface)] object o);
            [return: MarshalAs(UnmanagedType.Interface)]
            object get_onselect();
            void put_onbeforeunload([In, MarshalAs(UnmanagedType.Interface)] object o);
            [return: MarshalAs(UnmanagedType.Interface)]
            object get_onbeforeunload();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLTxtRange createTextRange();
        }

        [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("3050F4E9-98B5-11CF-BB82-00AA00BDCE0B")]
        public interface IHtmlControlElement
        {
            void SetTabIndex([In, MarshalAs(UnmanagedType.I2)] short p);
            [return: MarshalAs(UnmanagedType.I2)]
            short GetTabIndex();
            void Focus();
            void SetAccessKey([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetAccessKey();
            void SetOnblur([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnblur();
            void SetOnfocus([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnfocus();
            void SetOnresize([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnresize();
            void Blur();
            void AddFilter([In, MarshalAs(UnmanagedType.Interface)] object pUnk);
            void RemoveFilter([In, MarshalAs(UnmanagedType.Interface)] object pUnk);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetClientHeight();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetClientWidth();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetClientTop();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetClientLeft();
        }

        [Guid("3050F29C-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true)]
        public interface IHtmlControlRange
        {
            void Select();
            void Add([In, MarshalAs(UnmanagedType.Interface)] Interop.IHtmlControlElement item);
            void Remove([In, MarshalAs(UnmanagedType.I4)] int index);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement Item([In, MarshalAs(UnmanagedType.I4)] int index);
            void ScrollIntoView([In, MarshalAs(UnmanagedType.Struct)] object varargStart);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool QueryCommandSupported([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool QueryCommandEnabled([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool QueryCommandState([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool QueryCommandIndeterm([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.BStr)]
            string QueryCommandText([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Struct)]
            object QueryCommandValue([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool ExecCommand([In, MarshalAs(UnmanagedType.BStr)] string cmdID, [In, MarshalAs(UnmanagedType.Bool)] bool showUI, [In, MarshalAs(UnmanagedType.Struct)] object value);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool ExecCommandShowHelp([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement CommonParentElement();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetLength();
        }

        [InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("3050f65e-98b5-11cf-bb82-00aa00bdce0b"), ComVisible(true)]
        public interface IHtmlControlRange2
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int addElement([In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLElement element);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("3050F3DB-98B5-11CF-BB82-00AA00BDCE0B"), ComVisible(true)]
        public interface IHTMLCurrentStyle
        {
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetPosition();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetStyleFloat();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetColor();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBackgroundColor();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFontFamily();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFontStyle();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFontObject();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetFontWeight();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetFontSize();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBackgroundImage();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBackgroundPositionX();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBackgroundPositionY();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBackgroundRepeat();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderLeftColor();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderTopColor();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderRightColor();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderBottomColor();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderTopStyle();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderRightStyle();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderBottomStyle();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderLeftStyle();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderTopWidth();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderRightWidth();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderBottomWidth();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderLeftWidth();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetLeft();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetTop();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetWidth();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetHeight();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetPaddingLeft();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetPaddingTop();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetPaddingRight();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetPaddingBottom();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTextAlign();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTextDecoration();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetDisplay();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetVisibility();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetZIndex();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetLetterSpacing();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetLineHeight();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetTextIndent();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetVerticalAlign();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBackgroundAttachment();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetMarginTop();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetMarginRight();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetMarginBottom();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetMarginLeft();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetClear();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetListStyleType();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetListStylePosition();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetListStyleImage();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetClipTop();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetClipRight();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetClipBottom();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetClipLeft();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetOverflow();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetPageBreakBefore();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetPageBreakAfter();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetCursor();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTableLayout();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderCollapse();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetDirection();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBehavior();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In, MarshalAs(UnmanagedType.I4)] int lFlags);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetUnicodeBidi();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetRight();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBottom();
        }

        [InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("626FC520-A41E-11CF-A731-00A0C9082637")]
        public interface IHTMLDocument
        {
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetScript();
        }

        [Guid("332C4425-26CB-11D0-B483-00C04FD90119"), InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true)]
        public interface IHTMLDocument2
        {
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetScript();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElementCollection GetAll();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement GetBody();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement GetActiveElement();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElementCollection GetImages();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElementCollection GetApplets();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElementCollection GetLinks();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElementCollection GetForms();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElementCollection GetAnchors();
            void SetTitle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTitle();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElementCollection GetScripts();
            void SetDesignMode([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetDesignMode();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLSelectionObject GetSelection();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetReadyState();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetFrames();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElementCollection GetEmbeds();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElementCollection GetPlugins();
            void SetAlinkColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetAlinkColor();
            void SetBgColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBgColor();
            void SetFgColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetFgColor();
            void SetLinkColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetLinkColor();
            void SetVlinkColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetVlinkColor();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetReferrer();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetLocation();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetLastModified();
            void SetURL([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetURL();
            void SetDomain([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetDomain();
            void SetCookie([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetCookie();
            void SetExpando([In, MarshalAs(UnmanagedType.Bool)] bool p);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetExpando();
            void SetCharset([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetCharset();
            void SetDefaultCharset([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetDefaultCharset();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetMimeType();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFileSize();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFileCreatedDate();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFileModifiedDate();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFileUpdatedDate();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetSecurity();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetProtocol();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetNameProp();
            void DummyWrite([In, MarshalAs(UnmanagedType.I4)] int psarray);
            void DummyWriteln([In, MarshalAs(UnmanagedType.I4)] int psarray);
            [return: MarshalAs(UnmanagedType.Interface)]
            object Open([In, MarshalAs(UnmanagedType.BStr)] string URL, [In, MarshalAs(UnmanagedType.Struct)] object name, [In, MarshalAs(UnmanagedType.Struct)] object features, [In, MarshalAs(UnmanagedType.Struct)] object replace);
            void Close();
            void Clear();
            [return: MarshalAs(UnmanagedType.Bool)]
            bool QueryCommandSupported([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool QueryCommandEnabled([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool QueryCommandState([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool QueryCommandIndeterm([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.BStr)]
            string QueryCommandText([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Struct)]
            object QueryCommandValue([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool ExecCommand([In, MarshalAs(UnmanagedType.BStr)] string cmdID, [In, MarshalAs(UnmanagedType.Bool)] bool showUI, [In, MarshalAs(UnmanagedType.Struct)] object value);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool ExecCommandShowHelp([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement CreateElement([In, MarshalAs(UnmanagedType.BStr)] string eTag);
            void SetOnhelp([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnhelp();
            void SetOnclick([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnclick();
            void SetOndblclick([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndblclick();
            void SetOnkeyup([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnkeyup();
            void SetOnkeydown([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnkeydown();
            void SetOnkeypress([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnkeypress();
            void SetOnmouseup([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnmouseup();
            void SetOnmousedown([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnmousedown();
            void SetOnmousemove([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnmousemove();
            void SetOnmouseout([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnmouseout();
            void SetOnmouseover([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnmouseover();
            void SetOnreadystatechange([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnreadystatechange();
            void SetOnafterupdate([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnafterupdate();
            void SetOnrowexit([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnrowexit();
            void SetOnrowenter([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnrowenter();
            int SetOndragstart([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndragstart();
            void SetOnselectstart([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnselectstart();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement ElementFromPoint([In, MarshalAs(UnmanagedType.I4)] int x, [In, MarshalAs(UnmanagedType.I4)] int y);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLWindow2 GetParentWindow();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLStyleSheetsCollection GetStyleSheets();
            void SetOnbeforeupdate([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnbeforeupdate();
            void SetOnerrorupdate([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnerrorupdate();
            [return: MarshalAs(UnmanagedType.BStr)]
            string toString();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLStyleSheet CreateStyleSheet([In, MarshalAs(UnmanagedType.BStr)] string bstrHref, [In, MarshalAs(UnmanagedType.I4)] int lIndex);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("3050f662-98b5-11cf-bb82-00aa00bdce0b")]
        public interface IHTMLEditDesigner
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int PreHandleEvent([In] int dispId, [In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLEventObj eventObj);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int PostHandleEvent([In] int dispId, [In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLEventObj eventObj);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int TranslateAccelerator([In] int dispId, [In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLEventObj eventObj);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int PostEditorEventNotify([In] int dispId, [In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLEventObj eventObj);
        }

        [Guid("3050f6a0-98b5-11cf-bb82-00aa00bdce0b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true)]
        public interface IHTMLEditHost
        {
            void SnapRect([In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLElement pElement, [In, Out] Interop.COMRECT rcNew, [In, MarshalAs(UnmanagedType.I4)] int nHandle);
        }

        [Guid("3050f663-98b5-11cf-bb82-00aa00bdce0b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true)]
        public interface IHTMLEditServices
        {
            [return: MarshalAs(UnmanagedType.I4)]
            int AddDesigner([In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLEditDesigner designer);
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetSelectionServices([In, MarshalAs(UnmanagedType.Interface)] object markupContainer);
            [return: MarshalAs(UnmanagedType.I4)]
            int MoveToSelectionAnchor([In, MarshalAs(UnmanagedType.Interface)] object markupPointer);
            [return: MarshalAs(UnmanagedType.I4)]
            int MoveToSelectionEnd([In, MarshalAs(UnmanagedType.Interface)] object markupPointer);
            [return: MarshalAs(UnmanagedType.I4)]
            int RemoveDesigner([In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLEditDesigner designer);
        }

        [ComVisible(true), Guid("3050F1FF-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IHTMLElement
        {
            void SetAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In, MarshalAs(UnmanagedType.Struct)] object AttributeValue, [In, MarshalAs(UnmanagedType.I4)] int lFlags);
            void GetAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In, MarshalAs(UnmanagedType.I4)] int lFlags, [Out, MarshalAs(UnmanagedType.LPArray)] object[] pvars);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool RemoveAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In, MarshalAs(UnmanagedType.I4)] int lFlags);
            void SetClassName([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetClassName();
            void SetId([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetId();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTagName();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement GetParentElement();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLStyle GetStyle();
            void SetOnhelp([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnhelp();
            void SetOnclick([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnclick();
            void SetOndblclick([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndblclick();
            void SetOnkeydown([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnkeydown();
            void SetOnkeyup([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnkeyup();
            void SetOnkeypress([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnkeypress();
            void SetOnmouseout([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnmouseout();
            void SetOnmouseover([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnmouseover();
            void SetOnmousemove([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnmousemove();
            void SetOnmousedown([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnmousedown();
            void SetOnmouseup([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnmouseup();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetDocument();
            void SetTitle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTitle();
            void SetLanguage([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetLanguage();
            void SetOnselectstart([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnselectstart();
            void ScrollIntoView([In, MarshalAs(UnmanagedType.Struct)] object varargStart);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool Contains([In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLElement pChild);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetSourceIndex();
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetRecordNumber();
            void SetLang([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetLang();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetOffsetLeft();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetOffsetTop();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetOffsetWidth();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetOffsetHeight();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement GetOffsetParent();
            void SetInnerHTML([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetInnerHTML();
            void SetInnerText([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetInnerText();
            void SetOuterHTML([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetOuterHTML();
            void SetOuterText([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetOuterText();
            void InsertAdjacentHTML([In, MarshalAs(UnmanagedType.BStr)] string whereLocation, [In, MarshalAs(UnmanagedType.BStr)] string html);
            void InsertAdjacentText([In, MarshalAs(UnmanagedType.BStr)] string whereLocation, [In, MarshalAs(UnmanagedType.BStr)] string text);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement GetParentTextEdit();
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetIsTextEdit();
            void Click();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetFilters();
            void SetOndragstart([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndragstart();
            [return: MarshalAs(UnmanagedType.BStr)]
            string toString();
            void SetOnbeforeupdate([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnbeforeupdate();
            void SetOnafterupdate([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnafterupdate();
            void SetOnerrorupdate([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnerrorupdate();
            void SetOnrowexit([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnrowexit();
            void SetOnrowenter([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnrowenter();
            void SetOndatasetchanged([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndatasetchanged();
            void SetOndataavailable([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndataavailable();
            void SetOndatasetcomplete([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndatasetcomplete();
            void SetOnfilterchange([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnfilterchange();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetChildren();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetAll();
        }

        [Guid("3050F434-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true)]
        public interface IHTMLElement2
        {
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetScopeName();
            void SetCapture([In, MarshalAs(UnmanagedType.Bool)] bool containerCapture);
            void ReleaseCapture();
            void SetOnlosecapture([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnlosecapture();
            [return: MarshalAs(UnmanagedType.BStr)]
            string ComponentFromPoint([In, MarshalAs(UnmanagedType.I4)] int x, [In, MarshalAs(UnmanagedType.I4)] int y);
            void DoScroll([In, MarshalAs(UnmanagedType.Struct)] object component);
            void SetOnscroll([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnscroll();
            void SetOndrag([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndrag();
            void SetOndragend([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndragend();
            void SetOndragenter([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndragenter();
            void SetOndragover([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndragover();
            void SetOndragleave([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndragleave();
            void SetOndrop([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndrop();
            void SetOnbeforecut([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnbeforecut();
            void SetOncut([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOncut();
            void SetOnbeforecopy([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnbeforecopy();
            void SetOncopy([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOncopy();
            void SetOnbeforepaste([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnbeforepaste();
            void SetOnpaste([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnpaste();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLCurrentStyle GetCurrentStyle();
            void SetOnpropertychange([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnpropertychange();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetClientRects();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetBoundingClientRect();
            void SetExpression([In, MarshalAs(UnmanagedType.BStr)] string propname, [In, MarshalAs(UnmanagedType.BStr)] string expression, [In, MarshalAs(UnmanagedType.BStr)] string language);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetExpression([In, MarshalAs(UnmanagedType.BStr)] object propname);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool RemoveExpression([In, MarshalAs(UnmanagedType.BStr)] string propname);
            void SetTabIndex([In, MarshalAs(UnmanagedType.I2)] short p);
            [return: MarshalAs(UnmanagedType.I2)]
            short GetTabIndex();
            void Focus();
            void SetAccessKey([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetAccessKey();
            void SetOnblur([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnblur();
            void SetOnfocus([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnfocus();
            void SetOnresize([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnresize();
            void Blur();
            void AddFilter([In, MarshalAs(UnmanagedType.Interface)] object pUnk);
            void RemoveFilter([In, MarshalAs(UnmanagedType.Interface)] object pUnk);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetClientHeight();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetClientWidth();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetClientTop();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetClientLeft();
            [return: MarshalAs(UnmanagedType.Bool)]
            bool AttachEvent([In, MarshalAs(UnmanagedType.BStr)] string ev, [In, MarshalAs(UnmanagedType.Interface)] object pdisp);
            void DetachEvent([In, MarshalAs(UnmanagedType.BStr)] string ev, [In, MarshalAs(UnmanagedType.Interface)] object pdisp);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetReadyState();
            void SetOnreadystatechange([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnreadystatechange();
            void SetOnrowsdelete([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnrowsdelete();
            void SetOnrowsinserted([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnrowsinserted();
            void SetOncellchange([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOncellchange();
            void SetDir([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetDir();
            [return: MarshalAs(UnmanagedType.Interface)]
            object CreateControlRange();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetScrollHeight();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetScrollWidth();
            void SetScrollTop([In, MarshalAs(UnmanagedType.I4)] int p);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetScrollTop();
            void SetScrollLeft([In, MarshalAs(UnmanagedType.I4)] int p);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetScrollLeft();
            void ClearAttributes();
            void MergeAttributes([In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLElement mergeThis);
            void SetOncontextmenu([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOncontextmenu();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement InsertAdjacentElement([In, MarshalAs(UnmanagedType.BStr)] string whereLocation, [In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLElement insertedElement);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement ApplyElement([In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLElement apply, [In, MarshalAs(UnmanagedType.BStr)] string whereLocation);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetAdjacentText([In, MarshalAs(UnmanagedType.BStr)] string whereLocation);
            [return: MarshalAs(UnmanagedType.BStr)]
            string ReplaceAdjacentText([In, MarshalAs(UnmanagedType.BStr)] string whereLocation, [In, MarshalAs(UnmanagedType.BStr)] string newText);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetCanHaveChildren();
            [return: MarshalAs(UnmanagedType.I4)]
            int AddBehavior([In, MarshalAs(UnmanagedType.BStr)] string bstrUrl, [In] ref object pvarFactory);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool RemoveBehavior([In, MarshalAs(UnmanagedType.I4)] int cookie);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLStyle GetRuntimeStyle();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetBehaviorUrns();
            void SetTagUrn([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTagUrn();
            void SetOnbeforeeditfocus([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnbeforeeditfocus();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetReadyStateValue();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElementCollection GetElementsByTagName([In, MarshalAs(UnmanagedType.BStr)] string v);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLStyle GetBaseStyle();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLCurrentStyle GetBaseCurrentStyle();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLStyle GetBaseRuntimeStyle();
            void SetOnmousehover([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnmousehover();
            void SetOnkeydownpreview([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnkeydownpreview();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetBehavior([In, MarshalAs(UnmanagedType.BStr)] string bstrName, [In, MarshalAs(UnmanagedType.BStr)] string bstrUrn);
        }

        [ComVisible(true), Guid("3050F21F-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IHTMLElementCollection
        {
            [return: MarshalAs(UnmanagedType.BStr)]
            string toString();
            void SetLength([In, MarshalAs(UnmanagedType.I4)] int p);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetLength();
            [return: MarshalAs(UnmanagedType.Interface)]
            object Get_newEnum();
            [return: MarshalAs(UnmanagedType.Interface)]
            object Item([In, MarshalAs(UnmanagedType.Struct)] object name, [In, MarshalAs(UnmanagedType.Struct)] object index);
            [return: MarshalAs(UnmanagedType.Interface)]
            object Tags([In, MarshalAs(UnmanagedType.Struct)] object tagName);
        }

        [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("3050F6C9-98B5-11CF-BB82-00AA00BDCE0B")]
        public interface IHTMLElementDefaults
        {
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLStyle GetStyle();
            void SetTabStop([In, MarshalAs(UnmanagedType.Bool)] bool v);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetTabStop();
            void SetViewInheritStyle([In, MarshalAs(UnmanagedType.Bool)] bool v);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetViewInheritStyle();
            void SetViewMasterTab([In, MarshalAs(UnmanagedType.Bool)] bool v);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetViewMasterTab();
            void SetScrollSegmentX([In, MarshalAs(UnmanagedType.I4)] int v);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetScrollSegmentX();
            void SetScrollSegmentY([In, MarshalAs(UnmanagedType.I4)] object p);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetScrollSegmentY();
            void SetIsMultiLine([In, MarshalAs(UnmanagedType.Bool)] bool v);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetIsMultiLine();
            void SetContentEditable([In, MarshalAs(UnmanagedType.BStr)] string v);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetContentEditable();
            void SetCanHaveHTML([In, MarshalAs(UnmanagedType.Bool)] bool v);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetCanHaveHTML();
            void SetViewLink([In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLDocument viewLink);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLDocument GetViewLink();
            void SetFrozen([In, MarshalAs(UnmanagedType.Bool)] bool v);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetFrozen();
        }

        [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("3050F33C-98B5-11CF-BB82-00AA00BDCE0B")]
        public interface IHTMLElementEvents
        {
            void Bogus1();
            void Bogus2();
            void Bogus3();
            void Invoke([In, MarshalAs(UnmanagedType.U4)] int id, [In] ref Guid g, [In, MarshalAs(UnmanagedType.U4)] int lcid, [In, MarshalAs(UnmanagedType.U4)] int dwFlags, [In] Interop.DISPPARAMS pdp, [Out, MarshalAs(UnmanagedType.LPArray)] object[] pvarRes, [Out] Interop.EXCEPINFO pei, [Out, MarshalAs(UnmanagedType.LPArray)] int[] nArgError);
        }

        [ComVisible(true), Guid("3050F32D-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IHTMLEventObj
        {
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement GetSrcElement();
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetAltKey();
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetCtrlKey();
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetShiftKey();
            void SetReturnValue([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetReturnValue();
            void SetCancelBubble([In, MarshalAs(UnmanagedType.Bool)] bool p);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetCancelBubble();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement GetFromElement();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement GetToElement();
            void SetKeyCode([In, MarshalAs(UnmanagedType.I4)] int p);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetKeyCode();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetButton();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetEventType();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetQualifier();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetReason();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetX();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetY();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetClientX();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetClientY();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetOffsetX();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetOffsetY();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetScreenX();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetScreenY();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetSrcFilter();
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("3050F6A6-98B5-11CF-BB82-00AA00BDCE0B"), ComVisible(true)]
        public interface IHTMLPainter
        {
            void Draw([In, MarshalAs(UnmanagedType.I4)] int leftBounds, [In, MarshalAs(UnmanagedType.I4)] int topBounds, [In, MarshalAs(UnmanagedType.I4)] int rightBounds, [In, MarshalAs(UnmanagedType.I4)] int bottomBounds, [In, MarshalAs(UnmanagedType.I4)] int leftUpdate, [In, MarshalAs(UnmanagedType.I4)] int topUpdate, [In, MarshalAs(UnmanagedType.I4)] int rightUpdate, [In, MarshalAs(UnmanagedType.I4)] int bottomUpdate, [In, MarshalAs(UnmanagedType.U4)] int lDrawFlags, [In] IntPtr hdc, [In] IntPtr pvDrawObject);
            void OnResize([In, MarshalAs(UnmanagedType.I4)] int cx, [In, MarshalAs(UnmanagedType.I4)] int cy);
            void GetPainterInfo([Out] Interop.HTML_PAINTER_INFO htmlPainterInfo);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool HitTestPoint([In, MarshalAs(UnmanagedType.I4)] int ptx, [In, MarshalAs(UnmanagedType.I4)] int pty, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pbHit, [Out, MarshalAs(UnmanagedType.LPArray)] int[] plPartID);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("3050f6a7-98b5-11cf-bb82-00aa00bdce0b")]
        public interface IHTMLPaintSite
        {
            void InvalidatePainterInfo();
            void InvalidateRect([In] IntPtr pRect);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("3050F3CF-98B5-11CF-BB82-00AA00BDCE0B"), ComVisible(true)]
        public interface IHTMLRuleStyle
        {
            void SetFontFamily([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFontFamily();
            void SetFontStyle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFontStyle();
            void SetFontObject([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFontObject();
            void SetFontWeight([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFontWeight();
            void SetFontSize([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetFontSize();
            void SetFont([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFont();
            void SetColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetColor();
            void SetBackground([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBackground();
            void SetBackgroundColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBackgroundColor();
            void SetBackgroundImage([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBackgroundImage();
            void SetBackgroundRepeat([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBackgroundRepeat();
            void SetBackgroundAttachment([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBackgroundAttachment();
            void SetBackgroundPosition([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBackgroundPosition();
            void SetBackgroundPositionX([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBackgroundPositionX();
            void SetBackgroundPositionY([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBackgroundPositionY();
            void SetWordSpacing([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetWordSpacing();
            void SetLetterSpacing([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetLetterSpacing();
            void SetTextDecoration([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTextDecoration();
            void SetTextDecorationNone([In, MarshalAs(UnmanagedType.Bool)] bool p);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetTextDecorationNone();
            void SetTextDecorationUnderline([In, MarshalAs(UnmanagedType.Bool)] bool p);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetTextDecorationUnderline();
            void SetTextDecorationOverline([In, MarshalAs(UnmanagedType.Bool)] bool p);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetTextDecorationOverline();
            void SetTextDecorationLineThrough([In, MarshalAs(UnmanagedType.Bool)] bool p);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetTextDecorationLineThrough();
            void SetTextDecorationBlink([In, MarshalAs(UnmanagedType.Bool)] bool p);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetTextDecorationBlink();
            void SetVerticalAlign([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetVerticalAlign();
            void SetTextTransform([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTextTransform();
            void SetTextAlign([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTextAlign();
            void SetTextIndent([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetTextIndent();
            void SetLineHeight([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetLineHeight();
            void SetMarginTop([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetMarginTop();
            void SetMarginRight([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetMarginRight();
            void SetMarginBottom([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetMarginBottom();
            void SetMarginLeft([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetMarginLeft();
            void SetMargin([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetMargin();
            void SetPaddingTop([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetPaddingTop();
            void SetPaddingRight([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetPaddingRight();
            void SetPaddingBottom([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetPaddingBottom();
            void SetPaddingLeft([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetPaddingLeft();
            void SetPadding([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetPadding();
            void SetBorder([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorder();
            void SetBorderTop([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderTop();
            void SetBorderRight([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderRight();
            void SetBorderBottom([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderBottom();
            void SetBorderLeft([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderLeft();
            void SetBorderColor([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderColor();
            void SetBorderTopColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderTopColor();
            void SetBorderRightColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderRightColor();
            void SetBorderBottomColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderBottomColor();
            void SetBorderLeftColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderLeftColor();
            void SetBorderWidth([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderWidth();
            void SetBorderTopWidth([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderTopWidth();
            void SetBorderRightWidth([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderRightWidth();
            void SetBorderBottomWidth([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderBottomWidth();
            void SetBorderLeftWidth([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderLeftWidth();
            void SetBorderStyle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderStyle();
            void SetBorderTopStyle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderTopStyle();
            void SetBorderRightStyle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderRightStyle();
            void SetBorderBottomStyle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderBottomStyle();
            void SetBorderLeftStyle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderLeftStyle();
            void SetWidth([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetWidth();
            void SetHeight([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetHeight();
            void SetStyleFloat([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetStyleFloat();
            void SetClear([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetClear();
            void SetDisplay([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetDisplay();
            void SetVisibility([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetVisibility();
            void SetListStyleType([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetListStyleType();
            void SetListStylePosition([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetListStylePosition();
            void SetListStyleImage([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetListStyleImage();
            void SetListStyle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetListStyle();
            void SetWhiteSpace([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetWhiteSpace();
            void SetTop([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetTop();
            void SetLeft([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetLeft();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetPosition();
            void SetZIndex([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetZIndex();
            void SetOverflow([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetOverflow();
            void SetPageBreakBefore([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetPageBreakBefore();
            void SetPageBreakAfter([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetPageBreakAfter();
            void SetCssText([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetCssText();
            void SetCursor([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetCursor();
            void SetClip([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetClip();
            void SetFilter([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFilter();
            void SetAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In, MarshalAs(UnmanagedType.Struct)] object AttributeValue, [In, MarshalAs(UnmanagedType.I4)] int lFlags);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In, MarshalAs(UnmanagedType.I4)] int lFlags);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool RemoveAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In, MarshalAs(UnmanagedType.I4)] int lFlags);
        }

        [Guid("3050F25A-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true)]
        public interface IHTMLSelectionObject
        {
            [return: MarshalAs(UnmanagedType.Interface)]
            object CreateRange();
            void Empty();
            void Clear();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetSelectionType();
        }

        [ComVisible(true), Guid("3050F25E-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IHTMLStyle
        {
            void SetFontFamily([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFontFamily();
            void SetFontStyle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFontStyle();
            void SetFontObject([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFontObject();
            void SetFontWeight([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFontWeight();
            void SetFontSize([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetFontSize();
            void SetFont([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFont();
            void SetColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetColor();
            void SetBackground([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBackground();
            void SetBackgroundColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBackgroundColor();
            void SetBackgroundImage([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBackgroundImage();
            void SetBackgroundRepeat([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBackgroundRepeat();
            void SetBackgroundAttachment([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBackgroundAttachment();
            void SetBackgroundPosition([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBackgroundPosition();
            void SetBackgroundPositionX([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBackgroundPositionX();
            void SetBackgroundPositionY([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBackgroundPositionY();
            void SetWordSpacing([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetWordSpacing();
            void SetLetterSpacing([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetLetterSpacing();
            void SetTextDecoration([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTextDecoration();
            void SetTextDecorationNone([In, MarshalAs(UnmanagedType.Bool)] bool p);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetTextDecorationNone();
            void SetTextDecorationUnderline([In, MarshalAs(UnmanagedType.Bool)] bool p);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetTextDecorationUnderline();
            void SetTextDecorationOverline([In, MarshalAs(UnmanagedType.Bool)] bool p);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetTextDecorationOverline();
            void SetTextDecorationLineThrough([In, MarshalAs(UnmanagedType.Bool)] bool p);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetTextDecorationLineThrough();
            void SetTextDecorationBlink([In, MarshalAs(UnmanagedType.Bool)] bool p);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetTextDecorationBlink();
            void SetVerticalAlign([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetVerticalAlign();
            void SetTextTransform([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTextTransform();
            void SetTextAlign([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTextAlign();
            void SetTextIndent([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetTextIndent();
            void SetLineHeight([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetLineHeight();
            void SetMarginTop([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetMarginTop();
            void SetMarginRight([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetMarginRight();
            void SetMarginBottom([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetMarginBottom();
            void SetMarginLeft([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetMarginLeft();
            void SetMargin([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetMargin();
            void SetPaddingTop([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetPaddingTop();
            void SetPaddingRight([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetPaddingRight();
            void SetPaddingBottom([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetPaddingBottom();
            void SetPaddingLeft([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetPaddingLeft();
            void SetPadding([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetPadding();
            void SetBorder([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorder();
            void SetBorderTop([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderTop();
            void SetBorderRight([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderRight();
            void SetBorderBottom([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderBottom();
            void SetBorderLeft([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderLeft();
            void SetBorderColor([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderColor();
            void SetBorderTopColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderTopColor();
            void SetBorderRightColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderRightColor();
            void SetBorderBottomColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderBottomColor();
            void SetBorderLeftColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderLeftColor();
            void SetBorderWidth([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderWidth();
            void SetBorderTopWidth([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderTopWidth();
            void SetBorderRightWidth([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderRightWidth();
            void SetBorderBottomWidth([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderBottomWidth();
            void SetBorderLeftWidth([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBorderLeftWidth();
            void SetBorderStyle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderStyle();
            void SetBorderTopStyle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderTopStyle();
            void SetBorderRightStyle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderRightStyle();
            void SetBorderBottomStyle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderBottomStyle();
            void SetBorderLeftStyle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBorderLeftStyle();
            void SetWidth([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetWidth();
            void SetHeight([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetHeight();
            void SetStyleFloat([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetStyleFloat();
            void SetClear([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetClear();
            void SetDisplay([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetDisplay();
            void SetVisibility([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetVisibility();
            void SetListStyleType([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetListStyleType();
            void SetListStylePosition([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetListStylePosition();
            void SetListStyleImage([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetListStyleImage();
            void SetListStyle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetListStyle();
            void SetWhiteSpace([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetWhiteSpace();
            void SetTop([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetTop();
            void SetLeft([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetLeft();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetPosition();
            void SetZIndex([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetZIndex();
            void SetOverflow([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetOverflow();
            void SetPageBreakBefore([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetPageBreakBefore();
            void SetPageBreakAfter([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetPageBreakAfter();
            void SetCssText([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetCssText();
            void SetPixelTop([In, MarshalAs(UnmanagedType.I4)] int p);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetPixelTop();
            void SetPixelLeft([In, MarshalAs(UnmanagedType.I4)] int p);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetPixelLeft();
            void SetPixelWidth([In, MarshalAs(UnmanagedType.I4)] int p);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetPixelWidth();
            void SetPixelHeight([In, MarshalAs(UnmanagedType.I4)] int p);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetPixelHeight();
            void SetPosTop([In, MarshalAs(UnmanagedType.R4)] float p);
            [return: MarshalAs(UnmanagedType.R4)]
            float GetPosTop();
            void SetPosLeft([In, MarshalAs(UnmanagedType.R4)] float p);
            [return: MarshalAs(UnmanagedType.R4)]
            float GetPosLeft();
            void SetPosWidth([In, MarshalAs(UnmanagedType.R4)] float p);
            [return: MarshalAs(UnmanagedType.R4)]
            float GetPosWidth();
            void SetPosHeight([In, MarshalAs(UnmanagedType.R4)] float p);
            [return: MarshalAs(UnmanagedType.R4)]
            float GetPosHeight();
            void SetCursor([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetCursor();
            void SetClip([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetClip();
            void SetFilter([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetFilter();
            void SetAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In, MarshalAs(UnmanagedType.Struct)] object AttributeValue, [In, MarshalAs(UnmanagedType.I4)] int lFlags);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In, MarshalAs(UnmanagedType.I4)] int lFlags);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool RemoveAttribute([In, MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In, MarshalAs(UnmanagedType.I4)] int lFlags);
        }

        [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("3050F2E3-98B5-11CF-BB82-00AA00BDCE0B")]
        public interface IHTMLStyleSheet
        {
            void SetTitle([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTitle();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLStyleSheet GetParentStyleSheet();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement GetOwningElement();
            void SetDisabled([In, MarshalAs(UnmanagedType.Bool)] bool p);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetDisabled();
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetReadOnly();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLStyleSheetsCollection GetImports();
            void SetHref([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetHref();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetStyleSheetType();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetId();
            [return: MarshalAs(UnmanagedType.I4)]
            int AddImport([In, MarshalAs(UnmanagedType.BStr)] string bstrURL, [In, MarshalAs(UnmanagedType.I4)] int lIndex);
            [return: MarshalAs(UnmanagedType.I4)]
            int AddRule([In, MarshalAs(UnmanagedType.BStr)] string bstrSelector, [In, MarshalAs(UnmanagedType.BStr)] string bstrStyle, [In, MarshalAs(UnmanagedType.I4)] int lIndex);
            void RemoveImport([In, MarshalAs(UnmanagedType.I4)] int lIndex);
            void RemoveRule([In, MarshalAs(UnmanagedType.I4)] int lIndex);
            void SetMedia([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetMedia();
            void SetCssText([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetCssText();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLStyleSheetRulesCollection GetRules();
        }

        [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("3050F357-98B5-11CF-BB82-00AA00BDCE0B")]
        public interface IHTMLStyleSheetRule
        {
            void SetSelectorText([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetSelectorText();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLRuleStyle GetStyle();
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetReadOnly();
        }

        [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("3050F2E5-98B5-11CF-BB82-00AA00BDCE0B")]
        public interface IHTMLStyleSheetRulesCollection
        {
            [return: MarshalAs(UnmanagedType.I4)]
            int GetLength();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLStyleSheetRule Item([In, MarshalAs(UnmanagedType.I4)] int index);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("3050F37E-98B5-11CF-BB82-00AA00BDCE0B")]
        public interface IHTMLStyleSheetsCollection
        {
            [return: MarshalAs(UnmanagedType.I4)]
            int GetLength();
            [return: MarshalAs(UnmanagedType.Interface)]
            object Get_newEnum();
            [return: MarshalAs(UnmanagedType.Struct)]
            object Item([In] ref object pvarIndex);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("3050f21e-98b5-11cf-bb82-00aa00bdce0b")]
        public interface IHTMLTable
        {
            void Bogus1();
            void Bogus2();
            void Bogus3();
            void Bogus4();
            void Bogus5();
            void Bogus6();
            void Bogus7();
            void Bogus8();
            void Bogus9();
            void Bogus10();
            void Bogus11();
            void Bogus12();
            void Bogus13();
            void Bogus14();
            void Bogus15();
            void Bogus16();
            void Bogus17();
            void Bogus18();
            void Bogus19();
            void Bogus20();
            void Bogus21();
            void Bogus22();
            void Bogus23();
            void Bogus24();
            void Bogus25();
            Interop.IHTMLElementCollection GetRows();
            void Bogus26();
            void Bogus27();
            void Bogus28();
            void Bogus29();
            void Bogus30();
            void Bogus31();
            void Bogus32();
            void Bogus33();
            void Bogus34();
            void Bogus35();
            void Bogus36();
            void Bogus37();
            void Bogus38();
            void Bogus39();
            void Bogus40();
            void Bogus41();
            void Bogus42();
            void Bogus43();
            Interop.IHTMLTableRow InsertRow(int index);
            void DeleteRow(int index);
            void Bogus46();
            void Bogus47();
            void Bogus48();
        }

        [InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("3050f23d-98b5-11cf-bb82-00aa00bdce0b"), ComVisible(true)]
        public interface IHTMLTableCell
        {
            void SetRowSpan(int rowSpan);
            int GetRowSpan();
            void SetColSpan(int colSpan);
            int GetColSpan();
            void Bogus1();
            void Bogus2();
            void Bogus3();
            void Bogus4();
            void Bogus5();
            void Bogus6();
            void Bogus7();
            void Bogus8();
            void Bogus9();
            void Bogus10();
            void Bogus11();
            void Bogus12();
            void Bogus13();
            void Bogus14();
            void Bogus15();
            void Bogus16();
            void Bogus17();
            void Bogus18();
            void Bogus19();
            void Bogus20();
            int GetCellIndex();
        }

        [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("3050f23c-98b5-11cf-bb82-00aa00bdce0b")]
        public interface IHTMLTableRow
        {
            void Bogus1();
            void Bogus2();
            void Bogus3();
            void Bogus4();
            void Bogus5();
            void Bogus6();
            void Bogus7();
            void Bogus8();
            void Bogus9();
            void Bogus10();
            void Bogus11();
            void Bogus12();
            int GetRowIndex();
            void Bogus14();
            Interop.IHTMLElementCollection GetCells();
            Interop.IHTMLTableCell InsertCell(int index);
            void DeleteCell(int index);
        }

        [Guid("3050f230-98b5-11cf-bb82-00aa00bdce0b"), ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
        public interface IHTMLTextContainer
        {
            [return: MarshalAs(UnmanagedType.IDispatch)]
            object createControlRange();
            int get_ScrollHeight();
            int get_ScrollWidth();
            int get_ScrollTop();
            int get_ScrollLeft();
            void put_ScrollHeight(int i);
            void put_ScrollWidth(int i);
            void put_ScrollTop(int i);
            void put_ScrollLeft(int i);
        }

        [Guid("3050F220-98B5-11CF-BB82-00AA00BDCE0B"), ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IHTMLTxtRange
        {
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetHtmlText();
            void SetText([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetText();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLElement ParentElement();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLTxtRange Duplicate();
            [return: MarshalAs(UnmanagedType.Bool)]
            bool InRange([In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLTxtRange range);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool IsEqual([In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLTxtRange range);
            void ScrollIntoView([In, MarshalAs(UnmanagedType.Bool)] bool fStart);
            void Collapse([In, MarshalAs(UnmanagedType.Bool)] bool Start);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool Expand([In, MarshalAs(UnmanagedType.BStr)] string Unit);
            [return: MarshalAs(UnmanagedType.I4)]
            int Move([In, MarshalAs(UnmanagedType.BStr)] string Unit, [In, MarshalAs(UnmanagedType.I4)] int Count);
            [return: MarshalAs(UnmanagedType.I4)]
            int MoveStart([In, MarshalAs(UnmanagedType.BStr)] string Unit, [In, MarshalAs(UnmanagedType.I4)] int Count);
            [return: MarshalAs(UnmanagedType.I4)]
            int MoveEnd([In, MarshalAs(UnmanagedType.BStr)] string Unit, [In, MarshalAs(UnmanagedType.I4)] int Count);
            void Select();
            void PasteHTML([In, MarshalAs(UnmanagedType.BStr)] string html);
            void MoveToElementText([In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLElement element);
            void SetEndPoint([In, MarshalAs(UnmanagedType.BStr)] string how, [In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLTxtRange SourceRange);
            [return: MarshalAs(UnmanagedType.I4)]
            int CompareEndPoints([In, MarshalAs(UnmanagedType.BStr)] string how, [In, MarshalAs(UnmanagedType.Interface)] Interop.IHTMLTxtRange SourceRange);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool FindText([In, MarshalAs(UnmanagedType.BStr)] string String, [In, MarshalAs(UnmanagedType.I4)] int Count, [In, MarshalAs(UnmanagedType.I4)] int Flags);
            void MoveToPoint([In, MarshalAs(UnmanagedType.I4)] int x, [In, MarshalAs(UnmanagedType.I4)] int y);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBookmark();
            [return: MarshalAs(UnmanagedType.Bool)]
            bool MoveToBookmark([In, MarshalAs(UnmanagedType.BStr)] string Bookmark);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool QueryCommandSupported([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool QueryCommandEnabled([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool QueryCommandState([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool QueryCommandIndeterm([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.BStr)]
            string QueryCommandText([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Struct)]
            object QueryCommandValue([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool ExecCommand([In, MarshalAs(UnmanagedType.BStr)] string cmdID, [In, MarshalAs(UnmanagedType.Bool)] bool showUI, [In, MarshalAs(UnmanagedType.Struct)] object value);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool ExecCommandShowHelp([In, MarshalAs(UnmanagedType.BStr)] string cmdID);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true), Guid("332C4427-26CB-11D0-B483-00C04FD90119")]
        public interface IHTMLWindow2
        {
            [return: MarshalAs(UnmanagedType.Struct)]
            object Item([In] ref object pvarIndex);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetLength();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetFrames();
            void SetDefaultStatus([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetDefaultStatus();
            void SetStatus([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetStatus();
            [return: MarshalAs(UnmanagedType.I4)]
            int SetTimeout([In, MarshalAs(UnmanagedType.BStr)] string expression, [In, MarshalAs(UnmanagedType.I4)] int msec, [In] ref object language);
            void ClearTimeout([In, MarshalAs(UnmanagedType.I4)] int timerID);
            void Alert([In, MarshalAs(UnmanagedType.BStr)] string message);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool Confirm([In, MarshalAs(UnmanagedType.BStr)] string message);
            [return: MarshalAs(UnmanagedType.Struct)]
            object Prompt([In, MarshalAs(UnmanagedType.BStr)] string message, [In, MarshalAs(UnmanagedType.BStr)] string defstr);
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetImage();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetLocation();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetHistory();
            void Close();
            void SetOpener([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOpener();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetNavigator();
            void SetName([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetName();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLWindow2 GetParent();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLWindow2 Open([In, MarshalAs(UnmanagedType.BStr)] string URL, [In, MarshalAs(UnmanagedType.BStr)] string name, [In, MarshalAs(UnmanagedType.BStr)] string features, [In, MarshalAs(UnmanagedType.Bool)] bool replace);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLWindow2 GetSelf();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLWindow2 GetTop();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLWindow2 GetWindow();
            void Navigate([In, MarshalAs(UnmanagedType.BStr)] string URL);
            void SetOnfocus([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnfocus();
            void SetOnblur([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnblur();
            void SetOnload([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnload();
            void SetOnbeforeunload([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnbeforeunload();
            void SetOnunload([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnunload();
            void SetOnhelp([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnhelp();
            void SetOnerror([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnerror();
            void SetOnresize([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnresize();
            void SetOnscroll([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnscroll();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLDocument2 GetDocument();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IHTMLEventObj GetEvent();
            [return: MarshalAs(UnmanagedType.Interface)]
            object Get_newEnum();
            [return: MarshalAs(UnmanagedType.Struct)]
            object ShowModalDialog([In, MarshalAs(UnmanagedType.BStr)] string dialog, [In] ref object varArgIn, [In] ref object varOptions);
            void ShowHelp([In, MarshalAs(UnmanagedType.BStr)] string helpURL, [In, MarshalAs(UnmanagedType.Struct)] object helpArg, [In, MarshalAs(UnmanagedType.BStr)] string features);
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetScreen();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetOption();
            void Focus();
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetClosed();
            void Blur();
            void Scroll([In, MarshalAs(UnmanagedType.I4)] int x, [In, MarshalAs(UnmanagedType.I4)] int y);
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetClientInformation();
            [return: MarshalAs(UnmanagedType.I4)]
            int SetInterval([In, MarshalAs(UnmanagedType.BStr)] string expression, [In, MarshalAs(UnmanagedType.I4)] int msec, [In] ref object language);
            void ClearInterval([In, MarshalAs(UnmanagedType.I4)] int timerID);
            void SetOffscreenBuffering([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOffscreenBuffering();
            [return: MarshalAs(UnmanagedType.Struct)]
            object ExecScript([In, MarshalAs(UnmanagedType.BStr)] string code, [In, MarshalAs(UnmanagedType.BStr)] string language);
            void ScrollBy([In, MarshalAs(UnmanagedType.I4)] int x, [In, MarshalAs(UnmanagedType.I4)] int y);
            void ScrollTo([In, MarshalAs(UnmanagedType.I4)] int x, [In, MarshalAs(UnmanagedType.I4)] int y);
            void MoveTo([In, MarshalAs(UnmanagedType.I4)] int x, [In, MarshalAs(UnmanagedType.I4)] int y);
            void MoveBy([In, MarshalAs(UnmanagedType.I4)] int x, [In, MarshalAs(UnmanagedType.I4)] int y);
            void ResizeTo([In, MarshalAs(UnmanagedType.I4)] int x, [In, MarshalAs(UnmanagedType.I4)] int y);
            void ResizeBy([In, MarshalAs(UnmanagedType.I4)] int x, [In, MarshalAs(UnmanagedType.I4)] int y);
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetExternal();
        }

        [Guid("79eac9e4-baf9-11ce-8c82-00aa004ba90b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true)]
        public interface IInternetProtocol
        {
            [PreserveSig]
            int Start([In, MarshalAs(UnmanagedType.LPWStr)] string szUrl, [In] Interop.IInternetProtocolSink protocolSink, [In] IntPtr bindInfo, [In] int grfPI, [In] int dwReserved);
            [PreserveSig]
            int Continue([In] IntPtr protocolData);
            [PreserveSig]
            int Abort([In] int hrReason, [In] int dwOptions);
            [PreserveSig]
            int Terminate([In] int dwOptions);
            [PreserveSig]
            int Suspend();
            [PreserveSig]
            int Resume();
            [PreserveSig]
            int Read([In] IntPtr buffer, [In] int bufferSize, out int numRead);
            [PreserveSig]
            int Seek([In] long moveAmount, [In] int dwOrigin, [In] IntPtr newPosition);
            [PreserveSig]
            int LockRequest([In] int dwOptions);
            [PreserveSig]
            int UnlockRequest();
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("79eac9ec-baf9-11ce-8c82-00aa004ba90b"), ComVisible(true)]
        public interface IInternetProtocolInfo
        {
            [PreserveSig]
            int ParseUrl([In, MarshalAs(UnmanagedType.LPWStr)] string pwzUrl, [In] int parseAction, [In] int dwParseFlags, IntPtr result, [In] int cchResult, [Out] IntPtr pcchResult, [In] int dwReserved);
            [PreserveSig]
            int CombineUrl([In, MarshalAs(UnmanagedType.LPWStr)] string pwzBaseUrl, [In, MarshalAs(UnmanagedType.LPWStr)] string pwzRelativeUrl, [In] int dwCombineFlags, [Out] IntPtr pwzResult, [In] int cchResult, out int pcchResult, [In] int dwReserved);
            [PreserveSig]
            int CompareUrl([In, MarshalAs(UnmanagedType.LPWStr)] string pwzUrl1, [In, MarshalAs(UnmanagedType.LPWStr)] string pwzUrl2, [In] int dwCompareFlags);
            [PreserveSig]
            int QueryInfo([In, MarshalAs(UnmanagedType.LPWStr)] string pwzUrl, [In] int queryOption, [In] int dwQueryFlags, [In] IntPtr pBuffer, [In] int cbBuffer, [In] IntPtr pcbBuf, [In] int dwReserved);
        }

        [Guid("79eac9e5-baf9-11ce-8c82-00aa004ba90b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true)]
        public interface IInternetProtocolSink
        {
            [PreserveSig]
            int Switch([In] IntPtr protocolData);
            [PreserveSig]
            int ReportProgress([In] uint ulStatusCode, [In, MarshalAs(UnmanagedType.LPWStr)] string szStatusText);
            [PreserveSig]
            int ReportData([In] int grfBSCF, [In] uint ulProgress, [In] uint ulProgressMax);
            [PreserveSig]
            int ReportResult([In] int hrResult, [In] int dwError, [In, MarshalAs(UnmanagedType.LPWStr)] string szResult);
        }

        [ComVisible(true), Guid("79eac9e7-baf9-11ce-8c82-00aa004ba90b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IInternetSession
        {
            [PreserveSig]
            int RegisterNameSpace([In] Interop.IClassFactory classFactory, [In] ref Guid rclsid, [In, MarshalAs(UnmanagedType.LPWStr)] string pwzProtocol, [In] int cPatterns, [In, MarshalAs(UnmanagedType.LPWStr)] string ppwzPatterns, [In] int dwReserved);
            [PreserveSig]
            int UnregisterNameSpace([In] Interop.IClassFactory classFactory, [In, MarshalAs(UnmanagedType.LPWStr)] string pszProtocol);
            int Bogus1();
            int Bogus2();
            int Bogus3();
            int Bogus4();
            int Bogus5();
        }

        [Guid("0000000F-0000-0000-C000-000000000046"), ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IMoniker
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int IsDirty();
            void Load([In, MarshalAs(UnmanagedType.Interface)] Interop.IStream pstm);
            void Save([In, MarshalAs(UnmanagedType.Interface)] Interop.IStream pstm, [In, MarshalAs(UnmanagedType.Bool)] bool fClearDirty);
            [return: MarshalAs(UnmanagedType.I8)]
            long GetSizeMax();
            [return: MarshalAs(UnmanagedType.Interface)]
            object BindToObject([In, MarshalAs(UnmanagedType.Interface)] object pbc, [In, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pMkToLeft, [In] ref Guid riidResult);
            [return: MarshalAs(UnmanagedType.Interface)]
            object BindToStorage([In, MarshalAs(UnmanagedType.Interface)] object pbc, [In, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pMkToLeft, [In] ref Guid riidResult);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IMoniker Reduce([In, MarshalAs(UnmanagedType.Interface)] object pbc, [In, MarshalAs(UnmanagedType.I4)] int dwReduceHowFar, [In, Out, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pMkToLeft);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IMoniker Reduce([In, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pMkRight, [In, MarshalAs(UnmanagedType.Bool)] bool fOnlyIfNotGeneric);
            [return: MarshalAs(UnmanagedType.Interface)]
            object Reduce([In, MarshalAs(UnmanagedType.Bool)] bool fForward);
            void IsEqual([In, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pOtherMoniker);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Hash();
            void IsRunning([In, MarshalAs(UnmanagedType.Interface)] object pbc, [In, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pMkToLeft, [In, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pMkNewlyRunning);
            [return: MarshalAs(UnmanagedType.LPStruct)]
            FILETIME GetTimeOfLastChange([In, MarshalAs(UnmanagedType.Interface)] object pbc, [In, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pMkToLeft);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IMoniker Inverse();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IMoniker CommonPrefixWith([In, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pMkOther);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IMoniker RelativePathTo([In, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pMkOther);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetDisplayName([In, MarshalAs(UnmanagedType.Interface)] object pbc, [In, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pMkOther);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IMoniker ParseDisplayName([In, MarshalAs(UnmanagedType.Interface)] object pbc, [In, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pMkToLeft, [In, MarshalAs(UnmanagedType.BStr)] string pszDisplayName, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pchEaten);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int IsSystemMoniker();
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("00000118-0000-0000-C000-000000000046")]
        public interface IOleClientSite
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SaveObject();
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetMoniker([In, MarshalAs(UnmanagedType.U4)] int dwAssign, [In, MarshalAs(UnmanagedType.U4)] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] out object ppmk);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetContainer(out Interop.IOleContainer ppContainer);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int ShowObject();
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int OnShowWindow([In, MarshalAs(UnmanagedType.I4)] int fShow);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int RequestNewObjectLayout();
        }

        [ComImport, ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("B722BCCB-4E68-101B-A2BC-00AA00404770")]
        public interface IOleCommandTarget
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int QueryStatus(ref Guid pguidCmdGroup, int cCmds, [In, Out] Interop.tagOLECMD prgCmds, [In, Out] int pCmdText);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Exec(ref Guid pguidCmdGroup, int nCmdID, int nCmdexecopt, [In, MarshalAs(UnmanagedType.LPArray)] object[] pvaIn, [Out, MarshalAs(UnmanagedType.LPArray)] object[] pvaOut);
        }

        [Guid("0000011B-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true)]
        public interface IOleContainer
        {
            void ParseDisplayName([In, MarshalAs(UnmanagedType.Interface)] object pbc, [In, MarshalAs(UnmanagedType.BStr)] string pszDisplayName, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pchEaten, [Out, MarshalAs(UnmanagedType.LPArray)] object[] ppmkOut);
            void EnumObjects([In, MarshalAs(UnmanagedType.U4)] int grfFlags, [Out, MarshalAs(UnmanagedType.LPArray)] object[] ppenum);
            void LockContainer([In, MarshalAs(UnmanagedType.I4)] int fLock);
        }

        [ComImport, ComVisible(true), Guid("0000010E-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleDataObject
        {
            int OleGetData(Interop.FORMATETC pFormatetc, [Out] Interop.STGMEDIUM pMedium);
            int OleGetDataHere(Interop.FORMATETC pFormatetc, [In, Out] Interop.STGMEDIUM pMedium);
            int OleQueryGetData(Interop.FORMATETC pFormatetc);
            int OleGetCanonicalFormatEtc(Interop.FORMATETC pformatectIn, [Out] Interop.FORMATETC pformatetcOut);
            int OleSetData(Interop.FORMATETC pFormatectIn, Interop.STGMEDIUM pmedium, int fRelease);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IEnumFORMATETC OleEnumFormatEtc([In, MarshalAs(UnmanagedType.U4)] int dwDirection);
            int OleDAdvise(Interop.FORMATETC pFormatetc, [In, MarshalAs(UnmanagedType.U4)] int advf, [In, MarshalAs(UnmanagedType.Interface)] object pAdvSink, [Out, MarshalAs(UnmanagedType.LPArray)] int[] pdwConnection);
            int OleDUnadvise([In, MarshalAs(UnmanagedType.U4)] int dwConnection);
            int OleEnumDAdvise([Out, MarshalAs(UnmanagedType.LPArray)] object[] ppenumAdvise);
        }

        [ComImport, ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("B722BCC7-4E68-101B-A2BC-00AA00404770")]
        public interface IOleDocumentSite
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int ActivateMe([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleDocumentView pViewToActivate);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("B722BCC6-4E68-101B-A2BC-00AA00404770")]
        public interface IOleDocumentView
        {
            void SetInPlaceSite([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleInPlaceSite pIPSite);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IOleInPlaceSite GetInPlaceSite();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetDocument();
            void SetRect([In] Interop.COMRECT prcView);
            void GetRect([Out] Interop.COMRECT prcView);
            void SetRectComplex([In] Interop.COMRECT prcView, [In] Interop.COMRECT prcHScroll, [In] Interop.COMRECT prcVScroll, [In] Interop.COMRECT prcSizeBox);
            void Show([In, MarshalAs(UnmanagedType.I4)] int fShow);
            void UIActivate([In, MarshalAs(UnmanagedType.I4)] int fUIActivate);
            void Open();
            void CloseView([In, MarshalAs(UnmanagedType.U4)] int dwReserved);
            void SaveViewState([In, MarshalAs(UnmanagedType.Interface)] Interop.IStream pstm);
            void ApplyViewState([In, MarshalAs(UnmanagedType.Interface)] Interop.IStream pstm);
            void Clone([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleInPlaceSite pIPSiteNew, [Out, MarshalAs(UnmanagedType.LPArray)] Interop.IOleDocumentView[] ppViewNew);
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("00000121-0000-0000-C000-000000000046")]
        public interface IOleDropSource
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int QueryContinueDrag([In, MarshalAs(UnmanagedType.I4)] int fEscapePressed, [In, MarshalAs(UnmanagedType.U4)] int grfKeyState);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GiveFeedback([In, MarshalAs(UnmanagedType.U4)] int dwEffect);
        }

        [ComImport, Guid("00000122-0000-0000-C000-000000000046"), ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleDropTarget
        {
            [PreserveSig]
            int OleDragEnter(IntPtr pDataObj, [In, MarshalAs(UnmanagedType.U4)] int grfKeyState, [In, MarshalAs(UnmanagedType.U8)] long pt, [In, Out] ref int pdwEffect);
            [PreserveSig]
            int OleDragOver([In, MarshalAs(UnmanagedType.U4)] int grfKeyState, [In, MarshalAs(UnmanagedType.U8)] long pt, [In, Out] ref int pdwEffect);
            [PreserveSig]
            int OleDragLeave();
            [PreserveSig]
            int OleDrop(IntPtr pDataObj, [In, MarshalAs(UnmanagedType.U4)] int grfKeyState, [In, MarshalAs(UnmanagedType.U8)] long pt, [In, Out] ref int pdwEffect);
        }

        [ComImport, Guid("00000117-0000-0000-C000-000000000046"), ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleInPlaceActiveObject
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetWindow(out IntPtr hwnd);
            void ContextSensitiveHelp([In, MarshalAs(UnmanagedType.I4)] int fEnterMode);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int TranslateAccelerator([In, MarshalAs(UnmanagedType.LPStruct)] Interop.COMMSG lpmsg);
            void OnFrameWindowActivate([In, MarshalAs(UnmanagedType.I4)] int fActivate);
            void OnDocWindowActivate([In, MarshalAs(UnmanagedType.I4)] int fActivate);
            void ResizeBorder([In] Interop.COMRECT prcBorder, [In] Interop.IOleInPlaceUIWindow pUIWindow, [In, MarshalAs(UnmanagedType.I4)] int fFrameWindow);
            void EnableModeless([In, MarshalAs(UnmanagedType.I4)] int fEnable);
        }

        [ComImport, ComVisible(true), Guid("00000116-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleInPlaceFrame
        {
            IntPtr GetWindow();
            void ContextSensitiveHelp([In, MarshalAs(UnmanagedType.I4)] int fEnterMode);
            void GetBorder([Out] Interop.COMRECT lprectBorder);
            void RequestBorderSpace([In] Interop.COMRECT pborderwidths);
            void SetBorderSpace([In] Interop.COMRECT pborderwidths);
            void SetActiveObject([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleInPlaceActiveObject pActiveObject, [In, MarshalAs(UnmanagedType.LPWStr)] string pszObjName);
            void InsertMenus([In] IntPtr hmenuShared, [In, Out] Interop.tagOleMenuGroupWidths lpMenuWidths);
            void SetMenu([In] IntPtr hmenuShared, [In] IntPtr holemenu, [In] IntPtr hwndActiveObject);
            void RemoveMenus([In] IntPtr hmenuShared);
            void SetStatusText([In, MarshalAs(UnmanagedType.BStr)] string pszStatusText);
            void EnableModeless([In, MarshalAs(UnmanagedType.I4)] int fEnable);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int TranslateAccelerator([In, MarshalAs(UnmanagedType.LPStruct)] Interop.COMMSG lpmsg, [In, MarshalAs(UnmanagedType.U2)] short wID);
        }

        [ComImport, Guid("00000113-0000-0000-C000-000000000046"), ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleInPlaceObject
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetWindow(out IntPtr hwnd);
            void ContextSensitiveHelp([In, MarshalAs(UnmanagedType.I4)] int fEnterMode);
            void InPlaceDeactivate();
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int UIDeactivate();
            void SetObjectRects([In] Interop.COMRECT lprcPosRect, [In] Interop.COMRECT lprcClipRect);
            void ReactivateAndUndo();
        }

        [ComImport, Guid("00000119-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true)]
        public interface IOleInPlaceSite
        {
            IntPtr GetWindow();
            void ContextSensitiveHelp([In, MarshalAs(UnmanagedType.I4)] int fEnterMode);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int CanInPlaceActivate();
            void OnInPlaceActivate();
            void OnUIActivate();
            void GetWindowContext(out Interop.IOleInPlaceFrame ppFrame, out Interop.IOleInPlaceUIWindow ppDoc, [Out] Interop.COMRECT lprcPosRect, [Out] Interop.COMRECT lprcClipRect, [In, Out] Interop.tagOIFI lpFrameInfo);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Scroll([In, MarshalAs(UnmanagedType.U4)] Interop.tagSIZE scrollExtent);
            void OnUIDeactivate([In, MarshalAs(UnmanagedType.I4)] int fUndoable);
            void OnInPlaceDeactivate();
            void DiscardUndoState();
            void DeactivateAndUndo();
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int OnPosRectChange([In] Interop.COMRECT lprcPosRect);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("00000115-0000-0000-C000-000000000046")]
        public interface IOleInPlaceUIWindow
        {
            IntPtr GetWindow();
            void ContextSensitiveHelp([In, MarshalAs(UnmanagedType.I4)] int fEnterMode);
            void GetBorder([Out] Interop.COMRECT lprectBorder);
            void RequestBorderSpace([In] Interop.COMRECT pborderwidths);
            void SetBorderSpace([In] Interop.COMRECT pborderwidths);
            void SetActiveObject([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleInPlaceActiveObject pActiveObject, [In, MarshalAs(UnmanagedType.LPWStr)] string pszObjName);
        }

        [ComImport, Guid("00000112-0000-0000-C000-000000000046"), ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleObject
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SetClientSite([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleClientSite pClientSite);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetClientSite(out Interop.IOleClientSite site);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SetHostNames([In, MarshalAs(UnmanagedType.LPWStr)] string szContainerApp, [In, MarshalAs(UnmanagedType.LPWStr)] string szContainerObj);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Close([In, MarshalAs(UnmanagedType.I4)] int dwSaveOption);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SetMoniker([In, MarshalAs(UnmanagedType.U4)] int dwWhichMoniker, [In, MarshalAs(UnmanagedType.Interface)] object pmk);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetMoniker([In, MarshalAs(UnmanagedType.U4)] int dwAssign, [In, MarshalAs(UnmanagedType.U4)] int dwWhichMoniker, out object moniker);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int InitFromData([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleDataObject pDataObject, [In, MarshalAs(UnmanagedType.I4)] int fCreation, [In, MarshalAs(UnmanagedType.U4)] int dwReserved);
            int GetClipboardData([In, MarshalAs(UnmanagedType.U4)] int dwReserved, out Interop.IOleDataObject data);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int DoVerb([In, MarshalAs(UnmanagedType.I4)] int iVerb, [In] IntPtr lpmsg, [In, MarshalAs(UnmanagedType.Interface)] Interop.IOleClientSite pActiveSite, [In, MarshalAs(UnmanagedType.I4)] int lindex, [In] IntPtr hwndParent, [In] Interop.COMRECT lprcPosRect);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int EnumVerbs(out Interop.IEnumOLEVERB e);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int OleUpdate();
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int IsUpToDate();
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetUserClassID([In, Out] ref Guid pClsid);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetUserType([In, MarshalAs(UnmanagedType.U4)] int dwFormOfType, [MarshalAs(UnmanagedType.LPWStr)] out string userType);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SetExtent([In, MarshalAs(UnmanagedType.U4)] int dwDrawAspect, [In] Interop.tagSIZEL pSizel);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetExtent([In, MarshalAs(UnmanagedType.U4)] int dwDrawAspect, [Out] Interop.tagSIZEL pSizel);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Advise([In, MarshalAs(UnmanagedType.Interface)] Interop.IAdviseSink pAdvSink, out int cookie);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Unadvise([In, MarshalAs(UnmanagedType.U4)] int dwConnection);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int EnumAdvise(out Interop.IEnumSTATDATA e);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetMiscStatus([In, MarshalAs(UnmanagedType.U4)] int dwAspect, out int misc);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SetColorScheme([In] Interop.tagLOGPALETTE pLogpal);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("A1FAF330-EF97-11CE-9BC9-00AA00608E01")]
        public interface IOleParentUndoUnit : Interop.IOleUndoUnit
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Open([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleParentUndoUnit parentUnit);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Close([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleParentUndoUnit parentUnit, [In, MarshalAs(UnmanagedType.Bool)] bool fCommit);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Add([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleUndoUnit undoUnit);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int FindUnit([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleUndoUnit undoUnit);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetParentState([MarshalAs(UnmanagedType.I8)] out long state);
        }

        [ComImport, ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
        public interface IOleServiceProvider
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int QueryService([In] ref Guid guidService, [In] ref Guid riid, out IntPtr ppvObject);
        }

        [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("D001F200-EF97-11CE-9BC9-00AA00608E01")]
        public interface IOleUndoManager
        {
            void Open([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleParentUndoUnit parentUndo);
            void Close([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleParentUndoUnit parentUndo, [In, MarshalAs(UnmanagedType.Bool)] bool fCommit);
            void Add([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleUndoUnit undoUnit);
            [return: MarshalAs(UnmanagedType.I8)]
            long GetOpenParentState();
            void DiscardFrom([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleUndoUnit undoUnit);
            void UndoTo([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleUndoUnit undoUnit);
            void RedoTo([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleUndoUnit undoUnit);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IEnumOleUndoUnits EnumUndoable();
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IEnumOleUndoUnits EnumRedoable();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetLastUndoDescription();
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetLastRedoDescription();
            void Enable([In, MarshalAs(UnmanagedType.Bool)] bool fEnable);
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("894AD3B0-EF97-11CE-9BC9-00AA00608E01"), ComVisible(true)]
        public interface IOleUndoUnit
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Do([In, MarshalAs(UnmanagedType.Interface)] Interop.IOleUndoManager undoManager);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetDescription([MarshalAs(UnmanagedType.BStr)] out string bStr);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetUnitType([MarshalAs(UnmanagedType.I4)] out int clsid, [MarshalAs(UnmanagedType.I4)] out int plID);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int OnNextAdd();
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true), Guid("79eac9c9-baf9-11ce-8c82-00aa004ba90b")]
        public interface IPersistMoniker
        {
            void GetClassID([In, Out] ref Guid pClassID);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int IsDirty();
            void Load([In] int fFullyAvailable, [In, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pmk, [In, MarshalAs(UnmanagedType.Interface)] object pbc, [In] int grfMode);
            void Save([In, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pimkName, [In, MarshalAs(UnmanagedType.Interface)] object pbc, [In, MarshalAs(UnmanagedType.Bool)] bool fRemember);
            void SaveCompleted([In, MarshalAs(UnmanagedType.Interface)] Interop.IMoniker pmk, [In, MarshalAs(UnmanagedType.Interface)] object pbc);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IMoniker GetCurMoniker();
        }

        [ComImport, Guid("7FD52380-4E07-101B-AE2D-08002B2EC713"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(true)]
        public interface IPersistStreamInit
        {
            void GetClassID([In, Out] ref Guid pClassID);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int IsDirty();
            void Load([In, MarshalAs(UnmanagedType.Interface)] System.Runtime.InteropServices.ComTypes.IStream pstm);
            void Save([In, MarshalAs(UnmanagedType.Interface)] System.Runtime.InteropServices.ComTypes.IStream pstm, [In, MarshalAs(UnmanagedType.I4)] int fClearDirty);
            void GetSizeMax([Out, MarshalAs(UnmanagedType.LPArray)] long pcbSize);
            void InitNew();
        }

        [ComVisible(true), Guid("9BFBBC02-EFF1-101A-84ED-00AA00341D07"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPropertyNotifySink
        {
            void OnChanged(int dispID);
            void OnRequestEdit(int dispID);
        }

        [ComVisible(true), Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IServiceProvider
        {
            [return: MarshalAs(UnmanagedType.I4)]
            int QueryService([In] ref Guid sid, [In] ref Guid iid, out IntPtr service);
        }

        [ComVisible(true), Guid("0000000C-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IStream
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Read([In] IntPtr buf, [In, MarshalAs(UnmanagedType.I4)] int len);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int Write([In] IntPtr buf, [In, MarshalAs(UnmanagedType.I4)] int len);
            [return: MarshalAs(UnmanagedType.I8)]
            long Seek([In, MarshalAs(UnmanagedType.I8)] long dlibMove, [In, MarshalAs(UnmanagedType.I4)] int dwOrigin);
            void SetSize([In, MarshalAs(UnmanagedType.I8)] long libNewSize);
            [return: MarshalAs(UnmanagedType.I8)]
            long CopyTo([In, MarshalAs(UnmanagedType.Interface)] Interop.IStream pstm, [In, MarshalAs(UnmanagedType.I8)] long cb, [Out, MarshalAs(UnmanagedType.LPArray)] long[] pcbRead);
            void Commit([In, MarshalAs(UnmanagedType.I4)] int grfCommitFlags);
            void Revert();
            void LockRegion([In, MarshalAs(UnmanagedType.I8)] long libOffset, [In, MarshalAs(UnmanagedType.I8)] long cb, [In, MarshalAs(UnmanagedType.I4)] int dwLockType);
            void UnlockRegion([In, MarshalAs(UnmanagedType.I8)] long libOffset, [In, MarshalAs(UnmanagedType.I8)] long cb, [In, MarshalAs(UnmanagedType.I4)] int dwLockType);
            void Stat([Out] Interop.STATSTG pstatstg, [In, MarshalAs(UnmanagedType.I4)] int grfStatFlag);
            [return: MarshalAs(UnmanagedType.Interface)]
            Interop.IStream Clone();
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public class NMCUSTOMDRAW
        {
            public Interop.NMHDR nmcd;
            public int dwDrawStage;
            public IntPtr hdc;
            public Interop.RECT rc;
            public int dwItemSpec;
            public int uItemState;
            public IntPtr lItemlParam;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public class NMHDR
        {
            public IntPtr hwndFrom;
            public int idFrom;
            public int code;
        }

        [ComVisible(false)]
        public sealed class OLECMDEXECOPT
        {
            public const int OLECMDEXECOPT_DODEFAULT = 0;
            public const int OLECMDEXECOPT_DONTPROMPTUSER = 2;
            public const int OLECMDEXECOPT_PROMPTUSER = 1;
            public const int OLECMDEXECOPT_SHOWHELP = 3;
        }

        [ComVisible(false)]
        public sealed class OLECMDF
        {
            public const int OLECMDF_ENABLED = 2;
            public const int OLECMDF_LATCHED = 4;
            public const int OLECMDF_NINCHED = 8;
            public const int OLECMDF_SUPPORTED = 1;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(true)]
        public class POINT
        {
            public int x;
            public int y;
            public POINT()
            {
            }

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
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public sealed class STATDATA
        {
            [MarshalAs(UnmanagedType.U4)]
            public int advf;
            [MarshalAs(UnmanagedType.U4)]
            public int dwConnection;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public class STATSTG
        {
            [MarshalAs(UnmanagedType.I4)]
            public int pwcsName;
            [MarshalAs(UnmanagedType.I4)]
            public int type;
            [MarshalAs(UnmanagedType.I8)]
            public long cbSize;
            [MarshalAs(UnmanagedType.I8)]
            public long mtime;
            [MarshalAs(UnmanagedType.I8)]
            public long ctime;
            [MarshalAs(UnmanagedType.I8)]
            public long atime;
            [MarshalAs(UnmanagedType.I8)]
            public long grfMode;
            [MarshalAs(UnmanagedType.I8)]
            public long grfLocksSupported;
            [MarshalAs(UnmanagedType.I4)]
            public int clsid_data1;
            [MarshalAs(UnmanagedType.I2)]
            public short clsid_data2;
            [MarshalAs(UnmanagedType.I2)]
            public short clsid_data3;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b0;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b1;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b2;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b3;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b4;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b5;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b6;
            [MarshalAs(UnmanagedType.U1)]
            public byte clsid_b7;
            [MarshalAs(UnmanagedType.I8)]
            public long grfStateBits;
            [MarshalAs(UnmanagedType.I8)]
            public long reserved;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public class STGMEDIUM
        {
            [MarshalAs(UnmanagedType.I4)]
            public int tymed;
            public IntPtr unionmember;
            public IntPtr pUnkForRelease;
        }

        [ComVisible(false)]
        public sealed class StreamConsts
        {
            public const int LOCK_EXCLUSIVE = 2;
            public const int LOCK_ONLYONCE = 4;
            public const int LOCK_WRITE = 1;
            public const int STATFLAG_DEFAULT = 0;
            public const int STATFLAG_NONAME = 1;
            public const int STATFLAG_NOOPEN = 2;
            public const int STGC_DANGEROUSLYCOMMITMERELYTODISKCACHE = 4;
            public const int STGC_DEFAULT = 0;
            public const int STGC_ONLYIFCURRENT = 2;
            public const int STGC_OVERWRITE = 1;
            public const int STREAM_SEEK_CUR = 1;
            public const int STREAM_SEEK_END = 2;
            public const int STREAM_SEEK_SET = 0;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public sealed class tagLOGPALETTE
        {
            [MarshalAs(UnmanagedType.U2)]
            public short palVersion;
            [MarshalAs(UnmanagedType.U2)]
            public short palNumEntries;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public sealed class tagOIFI
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.I4)]
            public int fMDIApp;
            public IntPtr hwndFrame;
            public IntPtr hAccel;
            [MarshalAs(UnmanagedType.U4)]
            public int cAccelEntries;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public sealed class tagOLECMD
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cmdID;
            [MarshalAs(UnmanagedType.U4)]
            public int cmdf;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public sealed class tagOleMenuGroupWidths
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=6)]
            public int[] widths = new int[6];
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public sealed class tagOLEVERB
        {
            [MarshalAs(UnmanagedType.I4)]
            public int lVerb;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszVerbName;
            [MarshalAs(UnmanagedType.U4)]
            public int fuFlags;
            [MarshalAs(UnmanagedType.U4)]
            public int grfAttribs;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(false)]
        public sealed class tagSIZE
        {
            [MarshalAs(UnmanagedType.I4)]
            public int cx;
            [MarshalAs(UnmanagedType.I4)]
            public int cy;
        }

        [StructLayout(LayoutKind.Sequential), ComVisible(true)]
        public sealed class tagSIZEL
        {
            [MarshalAs(UnmanagedType.I4)]
            public int cx;
            [MarshalAs(UnmanagedType.I4)]
            public int cy;
        }
    }
}

