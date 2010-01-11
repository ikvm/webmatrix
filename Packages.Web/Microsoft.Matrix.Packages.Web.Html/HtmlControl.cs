namespace Microsoft.Matrix.Packages.Web.Html
{
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Html.Elements;
    using Microsoft.Matrix.Packages.Web.Utility;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;

    public class HtmlControl : Control, Interop.IElementBehaviorFactory
    {
        private bool _allowInPlaceNavigation;
        private bool _border3d;
        private string _desiredContent;
        private string _desiredUrl;
        private bool _enableTheming;
        private bool _firstActivation;
        private bool _flatScrollBars;
        private bool _focusDesired;
        private bool _fullDocumentMode;
        private bool _isCreated;
        private bool _isReady;
        private bool _loadDesired;
        private static bool _namespaceRegistered;
        private static readonly object _readyStateCompleteEvent = new object();
        private bool _scriptEnabled;
        private object _scriptObject;
        private bool _scrollBarsEnabled;
        private IServiceProvider _serviceProvider;
        private MshtmlSite _site;
        private string _url;
        private static IDictionary _urlMap;
        private static readonly object EventShowContextMenu = new object();

        public event EventHandler ReadyStateComplete
        {
            add
            {
                base.Events.AddHandler(_readyStateCompleteEvent, value);
            }
            remove
            {
                base.Events.RemoveHandler(_readyStateCompleteEvent, value);
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

        public HtmlControl(IServiceProvider serviceProvider) : this(serviceProvider, true)
        {
        }

        public HtmlControl(IServiceProvider serviceProvider, bool fullDocumentMode)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            this._serviceProvider = serviceProvider;
            this._firstActivation = true;
            this._fullDocumentMode = fullDocumentMode;
            this._scrollBarsEnabled = true;
            this._enableTheming = true;
        }

        public void Copy()
        {
            if (!this.CanCopy)
            {
                throw new Exception("HtmlControl.Copy : Not in able to copy the current selection!");
            }
            this.Exec(15);
        }

        protected virtual Interop.IElementBehavior CreateBehavior(string behavior, string behaviorUrl)
        {
            return null;
        }

        protected virtual string CreateHtmlContent(string content, string style)
        {
            return ("<html><head>" + style + "</head><body>" + content + "</body></html>");
        }

        public void Cut()
        {
            if (!this.CanCut)
            {
                throw new Exception("HtmlControl.Cut : Not in able to cut the current selection!");
            }
            this.Exec(0x10);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this._url != null))
            {
                UrlMap[this._url] = null;
            }
            base.Dispose(disposing);
        }

        protected internal void Exec(int command)
        {
            this.Exec(command, null);
        }

        protected internal void Exec(int command, object argument)
        {
            object[] pvaIn = new object[] { argument };
            if (this.CommandTarget.Exec(ref Interop.Guid_MSHTML, command, 2, pvaIn, null) != 0)
            {
                throw new Exception("MSHTMLHelper.Exec : Command " + command + " did not return S_OK");
            }
        }

        protected internal object ExecResult(int command)
        {
            object[] pvaOut = new object[1];
            if (this.CommandTarget.Exec(ref Interop.Guid_MSHTML, command, 2, null, pvaOut) != 0)
            {
                throw new Exception("MSHTMLHelper.ExecResult : Command " + command + " did not return S_OK");
            }
            return pvaOut[0];
        }

        public bool Find(string searchString, bool matchCase, bool wholeWord, bool searchUp)
        {
            Interop.IHTMLSelectionObject selection = this.MSHTMLDocument.GetSelection();
            bool flag = false;
            if (selection != null)
            {
                flag = selection.GetSelectionType().Equals("Text");
            }
            Interop.IHTMLTxtRange range = null;
            if (flag)
            {
                range = selection.CreateRange() as Interop.IHTMLTxtRange;
            }
            if (range == null)
            {
                Interop.IHtmlBodyElement body = this.MSHTMLDocument.GetBody() as Interop.IHtmlBodyElement;
                flag = false;
                range = body.createTextRange();
            }
            if (searchUp)
            {
                if (flag)
                {
                    range.MoveEnd("character", -1);
                }
                for (int i = 1; i == 1; i = range.MoveStart("textedit", -1))
                {
                }
            }
            else
            {
                if (flag)
                {
                    range.MoveStart("character", 1);
                }
                for (int j = 1; j == 1; j = range.MoveEnd("textedit", 1))
                {
                }
            }
            int flags = (matchCase ? 4 : 0) | (wholeWord ? 2 : 0);
            int count = searchUp ? -10000000 : 0x989680;
            if (range.FindText(searchString, count, flags))
            {
                range.Select();
                range.ScrollIntoView(true);
                return true;
            }
            if (flag)
            {
                range = selection.CreateRange() as Interop.IHTMLTxtRange;
                if (searchUp)
                {
                    range.MoveStart("character", 1);
                    for (int k = 1; k == 1; k = range.MoveEnd("textedit", 1))
                    {
                    }
                }
                else
                {
                    range.MoveEnd("character", -1);
                    for (int m = 1; m == 1; m = range.MoveStart("textedit", -1))
                    {
                    }
                }
                if (range.FindText(searchString, count, flags))
                {
                    range.Select();
                    range.ScrollIntoView(true);
                    return true;
                }
            }
            return false;
        }

        protected internal CommandInfo GetCommandInfo(int command)
        {
            Interop.tagOLECMD prgCmds = new Interop.tagOLECMD();
            prgCmds.cmdID = command;
            int num2 = this.CommandTarget.QueryStatus(ref Interop.Guid_MSHTML, 1, prgCmds, 0);
            return (((CommandInfo) (prgCmds.cmdf >> 1)) & (CommandInfo.Checked | CommandInfo.Enabled));
        }

        protected internal virtual Element GetContentElement(Element bodyElement)
        {
            return bodyElement;
        }

        public Element GetElementByID(string id)
        {
            Interop.IHTMLElementCollection all = (Interop.IHTMLElementCollection) this.MSHTMLDocument.GetBody().GetAll();
            Interop.IHTMLElement element = (Interop.IHTMLElement) all.Item(id, 0);
            if (element == null)
            {
                return null;
            }
            return ElementWrapperTable.GetWrapper(element, this);
        }

        protected internal virtual object GetService(ref Guid sid)
        {
            if (sid == Interop.ElementBehaviorFactory)
            {
                return this;
            }
            return null;
        }

        protected internal bool IsCommandChecked(int command)
        {
            return ((this.GetCommandInfo(command) & CommandInfo.Checked) != 0);
        }

        protected internal bool IsCommandEnabled(int command)
        {
            return ((this.GetCommandInfo(command) & CommandInfo.Enabled) != 0);
        }

        protected internal bool IsCommandEnabledAndChecked(int command)
        {
            CommandInfo commandInfo = this.GetCommandInfo(command);
            return (((commandInfo & CommandInfo.Enabled) != 0) && ((commandInfo & CommandInfo.Checked) != 0));
        }

        public void LoadHtml(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("LoadHtml : You must specify a non-null stream for content");
            }
            this.LoadHtml(new StreamReader(stream).ReadToEnd());
        }

        public void LoadHtml(string content)
        {
            this.LoadHtml(content, null, null);
        }

        public void LoadHtml(string content, string url)
        {
            this.LoadHtml(content, url, null);
        }

        public void LoadHtml(string content, string url, string style)
        {
            if (content == null)
            {
                throw new ArgumentNullException("LoadHtml : You must specify a non-null string for content");
            }
            if (!this._isCreated)
            {
                this._desiredContent = content;
                this._desiredUrl = url;
                this._loadDesired = true;
            }
            else
            {
                if (!this._fullDocumentMode)
                {
                    content = this.CreateHtmlContent(content, style);
                }
                this.OnBeforeLoad();
                System.Runtime.InteropServices.ComTypes.IStream pStream = null;
                Interop.CreateStreamOnHGlobal(Marshal.StringToHGlobalUni(content), true, ref pStream);

                //NOTE: 改为使用MSDN写法
                //Interop.IStream pStream = null;
                //Interop.CreateStreamOnHGlobal(Marshal.StringToHGlobalUni(content), true, out pStream);

                if (pStream == null)
                {
                    Interop.IPersistStreamInit mSHTMLDocument = (Interop.IPersistStreamInit) this._site.MSHTMLDocument;
                    mSHTMLDocument.InitNew();
                    mSHTMLDocument = null;
                }
                else
                {
                    Interop.IHTMLDocument2 document = this._site.MSHTMLDocument;
                    if (url == null)
                    {
                        Interop.IPersistStreamInit init2 = (Interop.IPersistStreamInit) document;
                        init2.Load(pStream);
                        init2 = null;
                    }
                    else
                    {
                        if (!_namespaceRegistered)
                        {
                            ProtocolHandler.Register();
                            _namespaceRegistered = true;
                        }
                        ProtocolHandler.SetContentEncoding(document.GetCharset());
                        UrlMap[url] = content;
                        Interop.IPersistMoniker moniker = (Interop.IPersistMoniker) document;
                        Interop.IMoniker ppmk = null;
                        Interop.IBindCtx ppbc = null;
                        Interop.CreateURLMoniker(null, "maxtrix:" + url, out ppmk);
                        Interop.CreateBindCtx(0, out ppbc);
                        moniker.Load(1, ppmk, ppbc, 0);
                        UrlMap[url] = string.Empty;
                        moniker = null;
                        ppbc = null;
                        ppmk = null;
                    }
                }
                this._url = url;
                this.OnAfterLoad();
            }
        }

        Interop.IElementBehavior Interop.IElementBehaviorFactory.FindBehavior(string behavior, string behaviorUrl, Interop.IElementBehaviorSite pSite)
        {
            return this.CreateBehavior(behavior, behaviorUrl);
        }

        protected virtual void OnAfterLoad()
        {
        }

        protected virtual void OnAfterSave()
        {
        }

        protected virtual void OnBeforeLoad()
        {
        }

        protected virtual void OnBeforeSave()
        {
        }

        protected virtual void OnCreated(EventArgs args)
        {
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (this.IsReady)
            {
                this._site.SetFocus();
            }
            else
            {
                this._focusDesired = true;
            }
        }

        protected override void OnHandleCreated(EventArgs args)
        {
            base.OnHandleCreated(args);
            if (this._firstActivation)
            {
                this._site = new MshtmlSite(this);
                this._site.CreateMSHTML();
                this._isCreated = true;
                this.OnCreated(new EventArgs());
                this._site.ActivateMSHTML();
                this._firstActivation = false;
                if (this._loadDesired)
                {
                    this.LoadHtml(this._desiredContent, this._desiredUrl);
                    this._loadDesired = false;
                }
            }
        }

        protected internal virtual void OnReadyStateComplete(EventArgs e)
        {
            this._isReady = true;
            EventHandler handler = (EventHandler) base.Events[_readyStateCompleteEvent];
            if (handler != null)
            {
                handler(this, e);
            }
            if (this._focusDesired)
            {
                this._focusDesired = false;
                this._site.ActivateMSHTML();
                this._site.SetFocus();
            }
        }

        protected internal virtual void OnShowContextMenu(ShowContextMenuEventArgs e)
        {
            ShowContextMenuEventHandler handler = (ShowContextMenuEventHandler) base.Events[EventShowContextMenu];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Paste()
        {
            if (!this.CanPaste)
            {
                throw new Exception("HtmlControl.Paste : Not in able to paste the current selection!");
            }
            this.Exec(0x1a);
        }

        public override bool PreProcessMessage(ref Message m)
        {
            bool flag = false;
            if ((m.Msg >= 0x100) && (m.Msg <= 0x108))
            {
                if (m.Msg == 0x100)
                {
                    flag = this.ProcessCmdKey(ref m, ((Keys) ((int) m.WParam)) | Control.ModifierKeys);
                }
                if (!flag)
                {
                    int wParam = (int) m.WParam;
                    if (((wParam != 0x21) && (wParam != 0x22)) || ((Control.ModifierKeys & Keys.Control) == Keys.None))
                    {
                        Interop.COMMSG msg = new Interop.COMMSG();
                        msg.hwnd = m.HWnd;
                        msg.message = m.Msg;
                        msg.wParam = m.WParam;
                        msg.lParam = m.LParam;
                        flag = this._site.TranslateAccelarator(msg);
                    }
                    else
                    {
                        this.WndProc(ref m);
                        flag = true;
                    }
                }
            }
            if (!flag)
            {
                flag = base.PreProcessMessage(ref m);
            }
            return flag;
        }

        public void Redo()
        {
            if (!this.CanRedo)
            {
                throw new Exception("HtmlControl.Redo : Not in able to redo!");
            }
            this.Exec(0x1d);
        }

        public string SaveHtml()
        {
            if (!this.IsCreated)
            {
                throw new Exception("HtmlControl.SaveHtml : No HTML to save!");
            }
            string input = string.Empty;
            try
            {
                this.OnBeforeSave();

                Interop.IHTMLDocument2 mSHTMLDocument = this._site.MSHTMLDocument;
                if (this._fullDocumentMode)
                {
                    //NOTE: 切换到设计模式后出错的问题出在这里, 使用System.Runtime.InteropServices.ComTypes下的类和结构后正常了
                    // 替换Interop.IStream为System.Runtime.InteropServices.ComTypes.IStream
                    IntPtr ptr;
                    Interop.IPersistStreamInit init = (Interop.IPersistStreamInit)mSHTMLDocument;
                    System.Runtime.InteropServices.ComTypes.IStream pStream = null;
                    Interop.CreateStreamOnHGlobal(Interop.NullIntPtr, true, ref pStream);
                    init.Save(pStream, 1);
                    //Interop.STATSTG pstatstg = new Interop.STATSTG();
                    System.Runtime.InteropServices.ComTypes.STATSTG pStat = new System.Runtime.InteropServices.ComTypes.STATSTG();
                    pStream.Stat(out pStat, 1);
                    int cbSize = (int)pStat.cbSize;
                    byte[] destination = new byte[cbSize];
                    Interop.GetHGlobalFromStream(pStream, out ptr);
                    IntPtr source = Interop.GlobalLock(ptr);
                    if (!(source != Interop.NullIntPtr))
                    {
                        goto Label_010B;
                    }
                    Marshal.Copy(source, destination, 0, cbSize);
                    Interop.GlobalUnlock(ptr);
                    StreamReader reader = null;
                    try
                    {
                        reader = new StreamReader(new MemoryStream(destination), Encoding.Default);
                        input = reader.ReadToEnd();
                        goto Label_010B;
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }
                }
                Interop.IHTMLElement body = mSHTMLDocument.GetBody();
                if (body != null)
                {
                    input = this.GetContentElement(ElementWrapperTable.GetWrapper(body, this)).InnerHtml;
                }
            }
            catch (Exception)
            {
                input = string.Empty;
            }
            finally
            {
                this.OnAfterSave();
            }
        Label_010B:
            if (input == null)
            {
                input = string.Empty;
            }
            HtmlFormatter formatter = new HtmlFormatter();
            StringWriter output = new StringWriter();
            formatter.Format(input, output, new HtmlFormatterOptions(' ', 4, 80, true));
            return output.ToString();
        }

        /*
        public string SaveHtml()
        {
            if (!this.IsCreated)
            {
                throw new Exception("HtmlControl.SaveHtml : No HTML to save!");
            }
            string input = string.Empty;
            try
            {
                this.OnBeforeSave();
                Interop.IHTMLDocument2 mSHTMLDocument = this._site.MSHTMLDocument;
                if (this._fullDocumentMode)
                {
                    IntPtr ptr;
                    Interop.IPersistStreamInit init = (Interop.IPersistStreamInit)mSHTMLDocument;
                    Interop.IStream pStream = null;
                    Interop.CreateStreamOnHGlobal(Interop.NullIntPtr, true, out pStream);
                    init.Save(pStream, 1);
                    Interop.STATSTG pstatstg = new Interop.STATSTG();
                    pStream.Stat(pstatstg, 1);
                    int cbSize = (int)pstatstg.cbSize;
                    byte[] destination = new byte[cbSize];
                    Interop.GetHGlobalFromStream(pStream, out ptr);
                    IntPtr source = Interop.GlobalLock(ptr);
                    if (!(source != Interop.NullIntPtr))
                    {
                        goto Label_010B;
                    }
                    Marshal.Copy(source, destination, 0, cbSize);
                    Interop.GlobalUnlock(ptr);
                    StreamReader reader = null;
                    try
                    {
                        reader = new StreamReader(new MemoryStream(destination), Encoding.Default);
                        input = reader.ReadToEnd();
                        goto Label_010B;
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }
                }
                Interop.IHTMLElement body = mSHTMLDocument.GetBody();
                if (body != null)
                {
                    input = this.GetContentElement(ElementWrapperTable.GetWrapper(body, this)).InnerHtml;
                }
            }
            catch (Exception)
            {
                input = string.Empty;
            }
            finally
            {
                this.OnAfterSave();
            }
        Label_010B:
            if (input == null)
            {
                input = string.Empty;
            }
            HtmlFormatter formatter = new HtmlFormatter();
            StringWriter output = new StringWriter();
            formatter.Format(input, output, new HtmlFormatterOptions(' ', 4, 80, true));
            return output.ToString();
        }
        */

        public void SaveHtml(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("SaveHtml : Must specify a non-null stream to which to save");
            }
            string str = this.SaveHtml();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(str);
            writer.Flush();
        }

        public void SelectAll()
        {
            Interop.IHTMLTxtRange range = null;
            range = (this.MSHTMLDocument.GetBody() as Interop.IHtmlBodyElement).createTextRange();
            while (range.MoveStart("character", -1) > 0)
            {
            }
            while (range.MoveEnd("character", 1) > 0)
            {
            }
            range.Select();
        }

        public void Undo()
        {
            if (!this.CanUndo)
            {
                throw new Exception("HtmlControl.Undo : Not in able to undo!");
            }
            this.Exec(0x2b);
        }

        public bool AllowInPlaceNavigation
        {
            get
            {
                return this._allowInPlaceNavigation;
            }
            set
            {
                this._allowInPlaceNavigation = value;
            }
        }

        public bool Border3d
        {
            get
            {
                return this._border3d;
            }
            set
            {
                this._border3d = value;
            }
        }

        public bool CanCopy
        {
            get
            {
                return this.IsCommandEnabled(15);
            }
        }

        public bool CanCut
        {
            get
            {
                return this.IsCommandEnabled(0x10);
            }
        }

        public bool CanPaste
        {
            get
            {
                return this.IsCommandEnabled(0x1a);
            }
        }

        public bool CanRedo
        {
            get
            {
                return this.IsCommandEnabled(0x1d);
            }
        }

        public bool CanUndo
        {
            get
            {
                return this.IsCommandEnabled(0x2b);
            }
        }

        protected internal Interop.IOleCommandTarget CommandTarget
        {
            get
            {
                return this._site.MSHTMLCommandTarget;
            }
        }

        public bool EnableTheming
        {
            get
            {
                return this._enableTheming;
            }
            set
            {
                this._enableTheming = value;
            }
        }

        public bool FlatScrollBars
        {
            get
            {
                return this._flatScrollBars;
            }
            set
            {
                this._flatScrollBars = value;
            }
        }

        protected bool IsCreated
        {
            get
            {
                return this._isCreated;
            }
        }

        internal bool IsFullDocumentMode
        {
            get
            {
                return this._fullDocumentMode;
            }
        }

        public bool IsReady
        {
            get
            {
                return this._isReady;
            }
        }

        protected internal Interop.IHTMLDocument2 MSHTMLDocument
        {
            get
            {
                return this._site.MSHTMLDocument;
            }
        }

        public bool ScriptEnabled
        {
            get
            {
                return this._scriptEnabled;
            }
            set
            {
                this._scriptEnabled = value;
            }
        }

        public object ScriptObject
        {
            get
            {
                return this._scriptObject;
            }
            set
            {
                this._scriptObject = value;
            }
        }

        public bool ScrollBarsEnabled
        {
            get
            {
                return this._scrollBarsEnabled;
            }
            set
            {
                this._scrollBarsEnabled = value;
            }
        }

        protected internal IServiceProvider ServiceProvider
        {
            get
            {
                return this._serviceProvider;
            }
        }

        public virtual string Url
        {
            get
            {
                return this._url;
            }
        }

        internal static IDictionary UrlMap
        {
            get
            {
                if (_urlMap == null)
                {
                    _urlMap = new HybridDictionary(true);
                }
                return _urlMap;
            }
        }
    }
}

