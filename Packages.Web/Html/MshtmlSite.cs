namespace Microsoft.Matrix.Packages.Web.Html
{
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ClassInterface(ClassInterfaceType.None)]
    internal class MshtmlSite : Interop.IOleClientSite, Interop.IOleContainer, Interop.IOleDocumentSite, Interop.IOleInPlaceSite, Interop.IOleInPlaceFrame, Interop.IDocHostUIHandler, Interop.IPropertyNotifySink, Interop.IAdviseSink, Interop.IOleServiceProvider
    {
        private DropTarget _dropTarget;
        private Interop.IOleInPlaceActiveObject activeObject;
        private int adviseSinkCookie;
        private HtmlControl hostControl;
        private Interop.ConnectionPointCookie propNotifyCookie;
        private Interop.IOleCommandTarget tridentCmdTarget;
        private Interop.IHTMLDocument2 tridentDocument;
        private Interop.IOleObject tridentOleObject;
        private Interop.IOleDocumentView tridentView;

        public MshtmlSite(HtmlControl hostControl)
        {
            if ((hostControl == null) || !hostControl.IsHandleCreated)
            {
                throw new ArgumentException();
            }
            this.hostControl = hostControl;
            hostControl.Resize += new EventHandler(this.OnParentResize);
        }

        public int ActivateMe(Interop.IOleDocumentView pViewToActivate)
        {
            if (pViewToActivate == null)
            {
                return -2147024809;
            }
            Interop.COMRECT rect = new Interop.COMRECT();
            Interop.GetClientRect(this.hostControl.Handle, rect);
            this.tridentView = pViewToActivate;
            this.tridentView.SetInPlaceSite(this);
            this.tridentView.UIActivate(1);
            this.tridentView.SetRect(rect);
            this.tridentView.Show(1);
            return 0;
        }

        public void ActivateMSHTML()
        {
            try
            {
                Interop.COMRECT rect = new Interop.COMRECT();
                Interop.GetClientRect(this.hostControl.Handle, rect);
                this.tridentOleObject.DoVerb(-4, Interop.NullIntPtr, this, 0, this.hostControl.Handle, rect);
            }
            catch (Exception)
            {
            }
        }

        public int CanInPlaceActivate()
        {
            return 0;
        }

        public void CloseMSHTML()
        {
            this.hostControl.Resize -= new EventHandler(this.OnParentResize);
            try
            {
                if (this.propNotifyCookie != null)
                {
                    this.propNotifyCookie.Disconnect();
                    this.propNotifyCookie = null;
                }
                if (this.tridentDocument != null)
                {
                    this.tridentView = null;
                    this.tridentDocument = null;
                    this.tridentCmdTarget = null;
                    this.activeObject = null;
                    if (this.adviseSinkCookie != 0)
                    {
                        this.tridentOleObject.Unadvise(this.adviseSinkCookie);
                        this.adviseSinkCookie = 0;
                    }
                    this.tridentOleObject.Close(1);
                    this.tridentOleObject.SetClientSite(null);
                    this.tridentOleObject = null;
                }
            }
            catch (Exception)
            {
            }
        }

        public void ContextSensitiveHelp(int fEnterMode)
        {
            throw new COMException(string.Empty, -2147467263);
        }

        public void CreateMSHTML()
        {
            bool flag = false;
            try
            {
                this.tridentDocument = (Interop.IHTMLDocument2) new Interop.HTMLDocument();
                this.tridentOleObject = (Interop.IOleObject) this.tridentDocument;
                this.tridentOleObject.SetClientSite(this);
                flag = true;
                this.propNotifyCookie = new Interop.ConnectionPointCookie(this.tridentDocument, this, typeof(Interop.IPropertyNotifySink), false);
                this.tridentOleObject.Advise(this, out this.adviseSinkCookie);
                this.tridentCmdTarget = (Interop.IOleCommandTarget) this.tridentDocument;
            }
            finally
            {
                if (!flag)
                {
                    this.tridentDocument = null;
                    this.tridentOleObject = null;
                    this.tridentCmdTarget = null;
                }
            }
        }

        public void DeactivateAndUndo()
        {
        }

        public void DeactivateMSHTML()
        {
        }

        public void DiscardUndoState()
        {
            throw new COMException(string.Empty, -2147467263);
        }

        public int EnableModeless(bool fEnable)
        {
            return 0;
        }

        public void EnableModeless(int fEnable)
        {
        }

        public void EnumObjects(int grfFlags, object[] ppenum)
        {
            throw new COMException(string.Empty, -2147467263);
        }

        public int FilterDataObject(Interop.IOleDataObject pDO, out Interop.IOleDataObject ppDORet)
        {
            ppDORet = null;
            return -2147467263;
        }

        public void GetBorder(Interop.COMRECT lprectBorder)
        {
            throw new COMException(string.Empty, -2147467263);
        }

        public int GetContainer(out Interop.IOleContainer ppContainer)
        {
            ppContainer = this;
            return 0;
        }

        public int GetDropTarget(Interop.IOleDropTarget pDropTarget, out Interop.IOleDropTarget ppDropTarget)
        {
            if (this._dropTarget == null)
            {
                HtmlEditor hostControl = this.hostControl as HtmlEditor;
                if (hostControl != null)
                {
                    DataObjectConverter dataObjectConverter = hostControl.DataObjectConverter;
                    if (dataObjectConverter != null)
                    {
                        this._dropTarget = new DropTarget(hostControl, dataObjectConverter, pDropTarget);
                    }
                }
            }
            ppDropTarget = this._dropTarget;
            if (this._dropTarget != null)
            {
                return 0;
            }
            return -2147467263;
        }

        public int GetExternal(out object ppDispatch)
        {
            ppDispatch = this.hostControl.ScriptObject;
            if (ppDispatch != null)
            {
                return 0;
            }
            return -2147467263;
        }

        public int GetHostInfo(Interop.DOCHOSTUIINFO info)
        {
            info.dwDoubleClick = 0;
            int num = 0;
            if (this.hostControl.AllowInPlaceNavigation)
            {
                num |= 0x10000;
            }
            if (!this.hostControl.Border3d)
            {
                num |= 4;
            }
            if (!this.hostControl.ScriptEnabled)
            {
                num |= 0x10;
            }
            if (!this.hostControl.ScrollBarsEnabled)
            {
                num |= 8;
            }
            if (this.hostControl.FlatScrollBars)
            {
                num |= 0x80;
            }
            if (this.hostControl.EnableTheming)
            {
                num |= 0x40000;
            }
            else
            {
                num |= 0x80000;
            }
            info.dwFlags = num;
            return 0;
        }

        public int GetMoniker(int dwAssign, int dwWhichMoniker, out object ppmk)
        {
            ppmk = null;
            return -2147467263;
        }

        public int GetOptionKeyPath(string[] pbstrKey, int dw)
        {
            pbstrKey[0] = null;
            return 0;
        }

        public IntPtr GetWindow()
        {
            return this.hostControl.Handle;
        }

        public void GetWindowContext(out Interop.IOleInPlaceFrame ppFrame, out Interop.IOleInPlaceUIWindow ppDoc, Interop.COMRECT lprcPosRect, Interop.COMRECT lprcClipRect, Interop.tagOIFI lpFrameInfo)
        {
            ppFrame = this;
            ppDoc = null;
            Interop.GetClientRect(this.hostControl.Handle, lprcPosRect);
            Interop.GetClientRect(this.hostControl.Handle, lprcClipRect);
            lpFrameInfo.cb = Marshal.SizeOf(typeof(Interop.tagOIFI));
            lpFrameInfo.fMDIApp = 0;
            lpFrameInfo.hwndFrame = this.hostControl.Handle;
            lpFrameInfo.hAccel = Interop.NullIntPtr;
            lpFrameInfo.cAccelEntries = 0;
        }

        public int HideUI()
        {
            return 0;
        }

        public void InsertMenus(IntPtr hmenuShared, Interop.tagOleMenuGroupWidths lpMenuWidths)
        {
            throw new COMException(string.Empty, -2147467263);
        }

        public void LockContainer(int fLock)
        {
        }

        public void OnChanged(int dispID)
        {
            if (dispID == -525)
            {
                this.OnReadyStateChanged();
            }
        }

        public void OnClose()
        {
        }

        public void OnDataChange(Interop.FORMATETC pFormat, Interop.STGMEDIUM pStg)
        {
        }

        public int OnDocWindowActivate(bool fActivate)
        {
            return -2147467263;
        }

        public int OnFrameWindowActivate(bool fActivate)
        {
            return -2147467263;
        }

        public void OnInPlaceActivate()
        {
        }

        public void OnInPlaceDeactivate()
        {
        }

        private void OnParentResize(object src, EventArgs e)
        {
            if (this.tridentView != null)
            {
                Interop.COMRECT rect = new Interop.COMRECT();
                Interop.GetClientRect(this.hostControl.Handle, rect);
                this.tridentView.SetRect(rect);
            }
        }

        public int OnPosRectChange(Interop.COMRECT lprcPosRect)
        {
            return 0;
        }

        private void OnReadyStateChanged()
        {
            if (string.Compare(this.tridentDocument.GetReadyState(), "complete", true) == 0)
            {
                this.OnReadyStateComplete();
            }
        }

        private void OnReadyStateComplete()
        {
            this.hostControl.OnReadyStateComplete(EventArgs.Empty);
        }

        public void OnRename(object pmk)
        {
        }

        public void OnRequestEdit(int dispID)
        {
        }

        public void OnSave()
        {
        }

        public int OnShowWindow(int fShow)
        {
            return 0;
        }

        public void OnUIActivate()
        {
        }

        public void OnUIDeactivate(int fUndoable)
        {
        }

        public void OnViewChange(int dwAspect, int index)
        {
        }

        public void ParseDisplayName(object pbc, string pszDisplayName, int[] pchEaten, object[] ppmkOut)
        {
            throw new COMException(string.Empty, -2147467263);
        }

        public int QueryService(ref Guid sid, ref Guid iid, out IntPtr ppvObject)
        {
            int num = -2147467262;
            ppvObject = Interop.NullIntPtr;
            object service = this.hostControl.GetService(ref sid);
            if (service != null)
            {
                if (iid.Equals(Interop.IID_IUnknown))
                {
                    ppvObject = Marshal.GetIUnknownForObject(service);
                    return num;
                }
                IntPtr iUnknownForObject = Marshal.GetIUnknownForObject(service);
                num = Marshal.QueryInterface(iUnknownForObject, ref iid, out ppvObject);
                Marshal.Release(iUnknownForObject);
            }
            return num;
        }

        public void RemoveMenus(IntPtr hmenuShared)
        {
            throw new COMException(string.Empty, -2147467263);
        }

        public void RequestBorderSpace(Interop.COMRECT pborderwidths)
        {
            throw new COMException(string.Empty, -2147467263);
        }

        public int RequestNewObjectLayout()
        {
            return 0;
        }

        public int ResizeBorder(Interop.COMRECT rect, Interop.IOleInPlaceUIWindow doc, bool fFrameWindow)
        {
            return -2147467263;
        }

        public int SaveObject()
        {
            return 0;
        }

        public int Scroll(Interop.tagSIZE scrollExtant)
        {
            return -2147467263;
        }

        public void SetActiveObject(Interop.IOleInPlaceActiveObject pActiveObject, string pszObjName)
        {
            this.activeObject = pActiveObject;
        }

        public void SetBorderSpace(Interop.COMRECT pborderwidths)
        {
            throw new COMException(string.Empty, -2147467263);
        }

        internal void SetFocus()
        {
            if (this.activeObject != null)
            {
                IntPtr zero = IntPtr.Zero;
                if (this.activeObject.GetWindow(out zero) == 0)
                {
                    Interop.SetFocus(zero);
                }
            }
        }

        public void SetMenu(IntPtr hmenuShared, IntPtr holemenu, IntPtr hwndActiveObject)
        {
            throw new COMException(string.Empty, -2147467263);
        }

        public void SetStatusText(string pszStatusText)
        {
        }

        public int ShowContextMenu(int dwID, Interop.POINT pt, object pcmdtReserved, object pdispReserved)
        {
            ShowContextMenuEventArgs e = new ShowContextMenuEventArgs(this.hostControl.PointToClient(new Point(pt.x, pt.y)), false);
            try
            {
                this.hostControl.OnShowContextMenu(e);
            }
            catch
            {
            }
            return 0;
        }

        public int ShowObject()
        {
            return 0;
        }

        public int ShowUI(int dwID, Interop.IOleInPlaceActiveObject activeObject, Interop.IOleCommandTarget commandTarget, Interop.IOleInPlaceFrame frame, Interop.IOleInPlaceUIWindow doc)
        {
            return 0;
        }

        public bool TranslateAccelarator(Interop.COMMSG msg)
        {
            return ((this.activeObject != null) && (this.activeObject.TranslateAccelerator(msg) != 1));
        }

        public int TranslateAccelerator(Interop.COMMSG lpmsg, short wID)
        {
            return 1;
        }

        public int TranslateAccelerator(Interop.COMMSG msg, ref Guid group, int nCmdID)
        {
            return 1;
        }

        public int TranslateUrl(int dwTranslate, string strURLIn, out string pstrURLOut)
        {
            pstrURLOut = null;
            return -2147467263;
        }

        public int UpdateUI()
        {
            return 0;
        }

        public Interop.IOleCommandTarget MSHTMLCommandTarget
        {
            get
            {
                return this.tridentCmdTarget;
            }
        }

        public Interop.IHTMLDocument2 MSHTMLDocument
        {
            get
            {
                return this.tridentDocument;
            }
        }

        private sealed class DropTarget : Interop.IOleDropTarget
        {
            private DataObjectConverter _converter;
            private DataObjectConverterInfo _converterInfo;
            private DataObject _currentDataObj;
            private IntPtr _currentDataObjPtr;
            private Interop.IOleDropTarget _originalDropTarget;
            private HtmlControl _owner;

            public DropTarget(HtmlControl owner, DataObjectConverter converter, Interop.IOleDropTarget originalDropTarget)
            {
                this._owner = owner;
                this._converter = converter;
                this._originalDropTarget = originalDropTarget;
            }

            public int OleDragEnter(IntPtr pDataObj, int grfKeyState, long pt, ref int pdwEffect)
            {
                DataObject dataObject = new DataObject(Marshal.GetObjectForIUnknown(pDataObj));
                this._converterInfo = this._converter.CanConvertToHtml(dataObject);
                if (this._converterInfo == DataObjectConverterInfo.CanConvert)
                {
                    this._currentDataObj = new DataObject(DataFormats.Html, string.Empty);
                    IntPtr iUnknownForObject = Marshal.GetIUnknownForObject(this._currentDataObj);
                    Guid iid = new Guid("0000010E-0000-0000-C000-000000000046");
                    Marshal.QueryInterface(iUnknownForObject, ref iid, out this._currentDataObjPtr);
                    Marshal.Release(iUnknownForObject);
                    return this._originalDropTarget.OleDragEnter(this._currentDataObjPtr, grfKeyState, pt, ref pdwEffect);
                }
                if (this._converterInfo == DataObjectConverterInfo.Disabled)
                {
                    pdwEffect = 0;
                }
                else if (this._converterInfo == DataObjectConverterInfo.Unhandled)
                {
                    return this._originalDropTarget.OleDragEnter(pDataObj, grfKeyState, pt, ref pdwEffect);
                }
                return 0;
            }

            public int OleDragLeave()
            {
                this._converterInfo = DataObjectConverterInfo.Disabled;
                if (this._currentDataObj != null)
                {
                    this._currentDataObj = null;
                    Marshal.Release(this._currentDataObjPtr);
                    this._currentDataObjPtr = IntPtr.Zero;
                    return this._originalDropTarget.OleDragLeave();
                }
                return 0;
            }

            public int OleDragOver(int grfKeyState, long pt, ref int pdwEffect)
            {
                if (this._converterInfo != DataObjectConverterInfo.Disabled)
                {
                    return this._originalDropTarget.OleDragOver(grfKeyState, pt, ref pdwEffect);
                }
                pdwEffect = 0;
                return 0;
            }

            public int OleDrop(IntPtr pDataObj, int grfKeyState, long pt, ref int pdwEffect)
            {
                int num = 0;
                Control parent = this._owner;
                while (parent != null)
                {
                    parent = parent.Parent;
                    Form form = parent as Form;
                    if (form != null)
                    {
                        form.BringToFront();
                        break;
                    }
                }
                if (this._converterInfo == DataObjectConverterInfo.CanConvert)
                {
                    DataObject originalDataObject = new DataObject(Marshal.GetObjectForIUnknown(pDataObj));
                    if (this._converter.ConvertToHtml(originalDataObject, this._currentDataObj))
                    {
                        IntPtr iUnknownForObject = Marshal.GetIUnknownForObject(this._currentDataObj);
                        Guid iid = new Guid("0000010E-0000-0000-C000-000000000046");
                        Marshal.QueryInterface(iUnknownForObject, ref iid, out this._currentDataObjPtr);
                        num = this._originalDropTarget.OleDrop(this._currentDataObjPtr, grfKeyState, pt, ref pdwEffect);
                        Marshal.Release(this._currentDataObjPtr);
                        this._currentDataObj = null;
                        this._currentDataObjPtr = IntPtr.Zero;
                    }
                    else
                    {
                        pdwEffect = 0;
                    }
                }
                else if (this._converterInfo == DataObjectConverterInfo.Unhandled)
                {
                    num = this._originalDropTarget.OleDrop(pDataObj, grfKeyState, pt, ref pdwEffect);
                }
                this._converterInfo = DataObjectConverterInfo.Disabled;
                return num;
            }
        }
    }
}

