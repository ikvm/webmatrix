namespace Microsoft.Matrix.Packages.Web.Html.WebForms
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.ComponentModel.Design.Serialization;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;

    internal sealed class Behavior : Interop.IElementBehavior, Interop.IHTMLPainter, IHtmlControlDesignerBehavior, IControlDesignerBehavior
    {
        private Interop.IElementBehaviorSite _behaviorSite;
        private System.Web.UI.Control _control;
        private bool _controlDown;
        private static Bitmap _controlGlyph = new Bitmap(typeof(Behavior), "Behavior.bmp");
        private ControlDesigner _designer;
        private IDesignerHost _designerHost;
        private bool _dragged;
        private bool _dragging;
        private WebFormsEditor _editor;
        private Interop.IHTMLElement _element;
        private EventSink _eventSink;
        private Interop.IHTMLPaintSite _paintSite;
        private Interop.IHTMLElement _parent;
        private bool _savingContents;
        private Interop.IHTMLElement _viewElement;

        static Behavior()
        {
            _controlGlyph.MakeTransparent(Color.Black);
        }

        public Behavior(WebFormsEditor editor)
        {
            this._editor = editor;
            this._designerHost = (IDesignerHost) editor.ServiceProvider.GetService(typeof(IDesignerHost));
        }

        private void ConnectToControlAndDesigner()
        {
            ((IHtmlControlDesignerBehavior) this).SetAttribute("RuntimeComponent", this._control, false);
            this._eventSink.Connect(this._element);
            if (this._control is ErrorControl)
            {
                this.DesignTimeHtml = ((ErrorControl) this._control).GetDesignTimeHtml();
            }
            else if (this._designer != null)
            {
                this._designer.Behavior = this;
                this.Designer = this._designer;
                if (this._designer.ReadOnly)
                {
                    if (this._designer.DesignTimeHtmlRequiresLoadComplete && this._designerHost.Loading)
                    {
                        this._designerHost.LoadComplete += new EventHandler(this.OnDesignerHostLoadComplete);
                    }
                    else
                    {
                        this.DesignTimeHtml = this._designer.GetDesignTimeHtml();
                    }
                }
            }
        }

        private void CreateControlAndDesigner()
        {
            bool flag;
            this._control = this.ParseControl(out flag);
            if (!flag)
            {
                this.EnsureControlID();
                this._designerHost.Container.Add(this._control, this._control.ID);
                this._designer = (ControlDesigner) this._designerHost.GetDesigner(this._control);
            }
            else
            {
                this._eventSink.AllowResize = false;
            }
        }

        private void CreateControlView()
        {
            if ((this._designer == null) || this._designer.ReadOnly)
            {
                Interop.IHTMLDocument2 document = (Interop.IHTMLDocument2) this._element.GetDocument();
                Interop.IHTMLElementDefaults defaults = ((Interop.IElementBehaviorSiteOM2) this._behaviorSite).GetDefaults();
                Interop.IHTMLElement element = document.CreateElement("HTML");
                Interop.IHTMLElement insertedElement = document.CreateElement("HEAD");
                Interop.IHTMLElement element3 = document.CreateElement("BODY");
                Interop.IHTMLElement element4 = document.CreateElement("SPAN");
                ((Interop.IHTMLElement2) element).InsertAdjacentElement("beforeBegin", insertedElement);
                ((Interop.IHTMLElement2) element).InsertAdjacentElement("afterBegin", element3);
                ((Interop.IHTMLElement2) element3).InsertAdjacentElement("afterBegin", element4);
                this._viewElement = element4;
                Interop.IHTMLDocument viewLink = (Interop.IHTMLDocument) element.GetDocument();
                defaults.SetViewLink(viewLink);
                defaults.SetFrozen(true);
                try
                {
                    Interop.IHTMLDocument2 document3 = (Interop.IHTMLDocument2) viewLink;
                    int length = 0;
                    Interop.IHTMLStyleSheetsCollection styleSheets = document.GetStyleSheets();
                    if (styleSheets != null)
                    {
                        length = styleSheets.GetLength();
                    }
                    for (int i = 0; i < length; i++)
                    {
                        object pvarIndex = i;
                        Interop.IHTMLStyleSheet sheet = (Interop.IHTMLStyleSheet) styleSheets.Item(ref pvarIndex);
                        if (sheet != null)
                        {
                            int num3 = 0;
                            Interop.IHTMLStyleSheetRulesCollection rules = sheet.GetRules();
                            if (rules != null)
                            {
                                num3 = rules.GetLength();
                            }
                            if (num3 != 0)
                            {
                                Interop.IHTMLStyleSheet sheet2 = document3.CreateStyleSheet(string.Empty, 0);
                                for (int j = 0; j < num3; j++)
                                {
                                    Interop.IHTMLStyleSheetRule rule = rules.Item(j);
                                    if (rule != null)
                                    {
                                        string selectorText = rule.GetSelectorText();
                                        string cssText = rule.GetStyle().GetCssText();
                                        sheet2.AddRule(selectorText, cssText, j);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            else
            {
                this._viewElement = this._element;
            }
        }

        private bool EnsureControlID()
        {
            string iD = this._control.ID;
            bool flag = iD == null;
            INameCreationService service = null;
            if (!flag)
            {
                if (this._designerHost.Container.Components[iD] != null)
                {
                    flag = true;
                }
                else
                {
                    service = (INameCreationService) this.ServiceProvider.GetService(typeof(INameCreationService));
                    if ((service != null) && !service.IsValidName(iD))
                    {
                        flag = true;
                    }
                }
            }
            if (flag)
            {
                if (service == null)
                {
                    service = (INameCreationService) this.ServiceProvider.GetService(typeof(INameCreationService));
                }
                if (service != null)
                {
                    iD = service.CreateName(this._designerHost.Container, this._control.GetType());
                    this._control.ID = iD;
                    this._element.SetAttribute("id", iD, 0);
                    return true;
                }
            }
            return false;
        }

        internal bool IsControlDown()
        {
            return this._controlDown;
        }

        internal bool IsDragging()
        {
            return this._dragging;
        }

        void Interop.IElementBehavior.Detach()
        {
            this.OnBehaviorDetach();
        }

        void Interop.IElementBehavior.Init(Interop.IElementBehaviorSite behaviorSite)
        {
            this.OnBehaviorInit(behaviorSite);
        }

        void Interop.IElementBehavior.Notify(int eventId, IntPtr pVar)
        {
            this.OnBehaviorNotify(eventId);
        }

        void Interop.IHTMLPainter.Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, int leftUpdate, int topUpdate, int rightUpdate, int bottomUpdate, int lDrawFlags, IntPtr hdc, IntPtr pvDrawObject)
        {
            Graphics graphics = Graphics.FromHdc(hdc);
            graphics.DrawImage(_controlGlyph, leftBounds - 2, topBounds - 2, 10, 10);
            graphics.Dispose();
        }

        void Interop.IHTMLPainter.GetPainterInfo(Interop.HTML_PAINTER_INFO htmlPainterInfo)
        {
            htmlPainterInfo.lFlags = 0x2002;
            htmlPainterInfo.lZOrder = 8;
            htmlPainterInfo.iidDrawObject = Guid.Empty;
            htmlPainterInfo.rcBounds.left = 0;
            htmlPainterInfo.rcBounds.top = 0;
            htmlPainterInfo.rcBounds.right = 0;
            htmlPainterInfo.rcBounds.bottom = 0;
        }

        bool Interop.IHTMLPainter.HitTestPoint(int ptx, int pty, int[] pbHit, int[] plPartID)
        {
            return false;
        }

        void Interop.IHTMLPainter.OnResize(int cx, int cy)
        {
        }

        private void OnBehaviorDetach()
        {
            if (this._designer != null)
            {
                this._designer.Behavior = null;
                this._designer = null;
            }
            if (this._eventSink != null)
            {
                this._eventSink.Disconnect();
                this._eventSink = null;
            }
            this._element = null;
            this._viewElement = null;
            this._paintSite = null;
            this._behaviorSite = null;
            this._editor = null;
        }

        private void OnBehaviorInit(Interop.IElementBehaviorSite behaviorSite)
        {
            this._behaviorSite = behaviorSite;
            this._paintSite = (Interop.IHTMLPaintSite) this._behaviorSite;
            this._element = this._behaviorSite.GetElement();
            behaviorSite.RegisterNotification(0);
            behaviorSite.RegisterNotification(3);
            behaviorSite.RegisterNotification(4);
            this._eventSink = new EventSink(this);
        }

        private void OnBehaviorNotify(int eventId)
        {
            switch (eventId)
            {
                case 0:
                    this.OnContentReady();
                    return;

                case 1:
                case 2:
                    return;

                case 3:
                    this.OnDocumentContextChanged();
                    return;

                case 4:
                    this.OnContentSave();
                    return;
            }
        }

        private void OnContentReady()
        {
            IDesignerHost service = (IDesignerHost) this.ServiceProvider.GetService(typeof(IDesignerHost));
            if (service.Loading)
            {
                this.CreateControlAndDesigner();
                this.CreateControlView();
                this.SetControlParent(this._element.GetParentElement());
                this.ConnectToControlAndDesigner();
            }
            else
            {
                Interop.IHTMLElement parentElement = this._element.GetParentElement();
                if ((this._parent == null) && (parentElement != null))
                {
                    bool flag = false;
                    string attribute = (string) ((IHtmlControlDesignerBehavior) this).GetAttribute("id", true);
                    if (attribute != null)
                    {
                        IComponent component = this._designerHost.Container.Components[attribute];
                        if ((component != null) && (component is System.Web.UI.Control))
                        {
                            System.Web.UI.Control control = (System.Web.UI.Control) component;
                            ControlDesigner designer = (ControlDesigner) this._designerHost.GetDesigner(control);
                            if (designer != null)
                            {
                                Behavior behavior = (Behavior) designer.Behavior;
                                if (behavior.IsDragging() && !behavior.IsControlDown())
                                {
                                    ((Behavior) designer.Behavior).StopDrag();
                                    flag = true;
                                    this._control = control;
                                    this._designer = designer;
                                    this.CreateControlView();
                                    this.SetControlParent(parentElement);
                                    this.ConnectToControlAndDesigner();
                                }
                            }
                        }
                    }
                    if (!flag)
                    {
                        this.CreateControlAndDesigner();
                        this.CreateControlView();
                        if (this._eventSink == null)
                        {
                            this._eventSink = new EventSink(this);
                        }
                        this.SetControlParent(parentElement);
                        this.ConnectToControlAndDesigner();
                    }
                }
            }
            Interop.IHTMLStyle runtimeStyle = ((Interop.IHTMLElement2) this._element).GetRuntimeStyle();
            if (runtimeStyle != null)
            {
                runtimeStyle.SetDisplay("inline-block");
            }
        }

        private void OnContentSave()
        {
            if (!this._savingContents)
            {
                try
                {
                    this._savingContents = true;
                    if ((this._designer != null) && this._designer.ReadOnly)
                    {
                        string persistInnerHtml = this._designer.GetPersistInnerHtml();
                        if (persistInnerHtml != null)
                        {
                            this._element.SetInnerHTML(persistInnerHtml);
                        }
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    this._savingContents = false;
                }
            }
        }

        private void OnDesignerHostLoadComplete(object sender, EventArgs e)
        {
            this.DesignTimeHtml = this._designer.GetDesignTimeHtml();
            this._designerHost.LoadComplete -= new EventHandler(this.OnDesignerHostLoadComplete);
        }

        private void OnDocumentContextChanged()
        {
            Interop.IHTMLElement parentElement = this._element.GetParentElement();
            if (this._parent == null)
            {
                if ((parentElement != null) && (this._control == null))
                {
                    this.CreateControlAndDesigner();
                    this.CreateControlView();
                    if (this._eventSink == null)
                    {
                        this._eventSink = new EventSink(this);
                    }
                    this.SetControlParent(parentElement);
                    this.ConnectToControlAndDesigner();
                }
            }
            else if (parentElement == null)
            {
                if (this._eventSink != null)
                {
                    this._eventSink.Disconnect();
                    this._eventSink = null;
                }
                if (!this._dragged)
                {
                    this._designerHost.Container.Remove(this._control);
                    if (this._control.Parent != null)
                    {
                        this._control.Parent.Controls.Remove(this._control);
                    }
                }
                this._control = null;
                this._designer = null;
                this._viewElement = null;
                this._parent = null;
            }
            else if (this._control == null)
            {
                this.CreateControlAndDesigner();
                this.CreateControlView();
                this.SetControlParent(parentElement);
                this.ConnectToControlAndDesigner();
            }
            else
            {
                this.SetControlParent(parentElement);
            }
        }

        private System.Web.UI.Control ParseControl(out bool parseError)
        {
            string outerHTML = this._element.GetOuterHTML();
            System.Web.UI.Control control = null;
            parseError = false;
            try
            {
                object[] pvars = new object[1];
                this._element.GetAttribute("runat", 0, pvars);
                if (string.Compare(pvars[0].ToString(), 0, "server", 0, 6, true, CultureInfo.InvariantCulture) != 0)
                {
                    throw new Exception(string.Format("{0} is missing the runat=\"server\" attribute.", this._element.GetTagName()));
                }
                control = ControlParser.ParseControl(this._designerHost, outerHTML);
            }
            catch (Exception exception)
            {
                control = new ErrorControl(((Interop.IHTMLElement2) this._element).GetScopeName() + ":" + this._element.GetTagName(), exception);
                parseError = true;
            }
            return control;
        }

        private void SetContainedControlsParent(Interop.IHTMLElement element)
        {
            Interop.IHTMLElementCollection children = (Interop.IHTMLElementCollection) element.GetChildren();
            if (children != null)
            {
                int length = children.GetLength();
                for (int i = 0; i < length; i++)
                {
                    Interop.IHTMLElement element2 = (Interop.IHTMLElement) children.Item(i, i);
                    if (element2 != null)
                    {
                        object[] pvars = new object[1];
                        element2.GetAttribute("runat", 0, pvars);
                        string strA = pvars[0] as string;
                        if (string.Compare(strA, "server", true) == 0)
                        {
                            element2.GetAttribute("id", 0, pvars);
                            string str2 = pvars[0] as string;
                            System.Web.UI.Control child = this._designerHost.Container.Components[str2] as System.Web.UI.Control;
                            if ((child != null) && (child.Parent != this._control))
                            {
                                this._control.Controls.Add(child);
                                ControlDesigner designer = this._designerHost.GetDesigner(child) as ControlDesigner;
                                if (designer != null)
                                {
                                    designer.OnSetParent();
                                }
                            }
                        }
                        else
                        {
                            this.SetContainedControlsParent(element2);
                        }
                    }
                }
            }
        }

        internal void SetControlDown(bool val)
        {
            this._controlDown = val;
        }

        private void SetControlParent(Interop.IHTMLElement newParent)
        {
            try
            {
                this._parent = newParent;
                Interop.IHTMLElement parentElement = newParent;
                System.Web.UI.Control defaultControlParent = null;
                while ((defaultControlParent == null) && (parentElement != null))
                {
                    object[] pvars = new object[1];
                    parentElement.GetAttribute("runat", 0, pvars);
                    string strB = pvars[0] as string;
                    if (string.Compare("server", strB, true) == 0)
                    {
                        parentElement.GetAttribute("id", 0, pvars);
                        string str2 = pvars[0] as string;
                        defaultControlParent = this._designerHost.Container.Components[str2] as System.Web.UI.Control;
                    }
                    parentElement = parentElement.GetParentElement();
                }
                if (defaultControlParent == null)
                {
                    defaultControlParent = this._editor.DefaultControlParent;
                }
                if (defaultControlParent == null)
                {
                    defaultControlParent = (System.Web.UI.Control) this._designerHost.RootComponent;
                }
                if (this._control.Parent != defaultControlParent)
                {
                    defaultControlParent.Controls.Add(this._control);
                    if (this._designer != null)
                    {
                        this._designer.OnSetParent();
                        if (!this._designer.ReadOnly)
                        {
                            this._control.Controls.Clear();
                            this.SetContainedControlsParent(this._element);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        internal void StartDrag()
        {
            this._dragging = true;
        }

        internal void StopDrag()
        {
            this._dragging = false;
            this._dragged = true;
        }

        void IControlDesignerBehavior.OnTemplateModeChanged()
        {
        }

        object IHtmlControlDesignerBehavior.GetAttribute(string attribute, bool ignoreCase)
        {
            if (this._element != null)
            {
                object[] pvars = new object[1];
                try
                {
                    this._element.GetAttribute(attribute, ignoreCase ? 0 : 1, pvars);
                    return pvars[0];
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        object IHtmlControlDesignerBehavior.GetStyleAttribute(string attribute, bool designTimeOnly, bool ignoreCase)
        {
            if (this._element != null)
            {
                Interop.IHTMLStyle runtimeStyle = null;
                if (designTimeOnly)
                {
                    runtimeStyle = ((Interop.IHTMLElement2) this._element).GetRuntimeStyle();
                }
                else
                {
                    runtimeStyle = this._element.GetStyle();
                }
                if (runtimeStyle != null)
                {
                    try
                    {
                        return runtimeStyle.GetAttribute(attribute, ignoreCase ? 0 : 1);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return null;
        }

        void IHtmlControlDesignerBehavior.RemoveAttribute(string attribute, bool ignoreCase)
        {
            if (this._element != null)
            {
                try
                {
                    this._element.RemoveAttribute(attribute, ignoreCase ? 0 : 1);
                }
                catch (Exception)
                {
                }
            }
        }

        void IHtmlControlDesignerBehavior.RemoveStyleAttribute(string attribute, bool designTimeOnly, bool ignoreCase)
        {
            if (this._element != null)
            {
                Interop.IHTMLStyle runtimeStyle = null;
                if (designTimeOnly)
                {
                    runtimeStyle = ((Interop.IHTMLElement2) this._element).GetRuntimeStyle();
                }
                else
                {
                    runtimeStyle = this._element.GetStyle();
                }
                if (runtimeStyle != null)
                {
                    try
                    {
                        runtimeStyle.RemoveAttribute(attribute, ignoreCase ? 0 : 1);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        void IHtmlControlDesignerBehavior.SetAttribute(string attribute, object val, bool ignoreCase)
        {
            if (this._element != null)
            {
                try
                {
                    this._element.SetAttribute(attribute, val, ignoreCase ? 0 : 1);
                }
                catch (Exception)
                {
                }
            }
        }

        void IHtmlControlDesignerBehavior.SetStyleAttribute(string attribute, bool designTimeOnly, object val, bool ignoreCase)
        {
            if (this._element != null)
            {
                Interop.IHTMLStyle runtimeStyle = null;
                if (designTimeOnly)
                {
                    runtimeStyle = ((Interop.IHTMLElement2) this._element).GetRuntimeStyle();
                }
                else
                {
                    runtimeStyle = this._element.GetStyle();
                }
                if (runtimeStyle != null)
                {
                    try
                    {
                        runtimeStyle.SetAttribute(attribute, val, ignoreCase ? 0 : 1);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public System.Web.UI.Control Control
        {
            get
            {
                return this._control;
            }
        }

        public WebFormsEditor Editor
        {
            get
            {
                return this._editor;
            }
        }

        public Interop.IHTMLElement Element
        {
            get
            {
                return this._element;
            }
        }

        public Interop.IHTMLElement Parent
        {
            get
            {
                return this._parent;
            }
        }

        private IServiceProvider ServiceProvider
        {
            get
            {
                return this._editor.ServiceProvider;
            }
        }

        object IControlDesignerBehavior.DesignTimeElementView
        {
            get
            {
                return this._viewElement;
            }
        }

        //string IControlDesignerBehavior.DesignTimeHtml //NOTE: changed
        public string DesignTimeHtml
        {
            get
            {
                if ((this._designer != null) && !this._designer.ReadOnly)
                {
                    return string.Empty;
                }
                string innerHTML = string.Empty;
                if (this._viewElement != null)
                {
                    innerHTML = this._viewElement.GetInnerHTML();
                }
                return innerHTML;
            }
            set
            {
                if ((this._designer != null) && !this._designer.ReadOnly)
                {
                    throw new NotSupportedException();
                }
                if ((value != null) && (this._viewElement != null))
                {
                    this._viewElement.SetInnerHTML(value);
                }
            }
        }

        //public HtmlControlDesigner IHtmlControlDesignerBehavior.Designer //NOTE: changed
        public HtmlControlDesigner Designer
        {
            get
            {
                return this._designer;
            }
            set
            {
                this._designer = (ControlDesigner) value;
            }
        }

        object IHtmlControlDesignerBehavior.DesignTimeElement
        {
            get
            {
                return this._element;
            }
        }

        private sealed class ErrorControl : Control
        {
            private string _controlTag;
            private Exception _errorException;
            private static string errorGlyph = null;
            private static readonly string RenderTemplate = "\r\n              <table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow\">\r\n                <tr><td><img src=\"{1}\" width=\"16\" height=\"16\" align=absmiddle>&nbsp;&nbsp;&lt;{2}&gt;</td></tr>\r\n                <tr><td>{0}</td></tr>\r\n              </table>";

            public ErrorControl(string controlTag, Exception e)
            {
                this._controlTag = controlTag;
                this._errorException = e;
            }

            public string GetDesignTimeHtml()
            {
                return string.Format(RenderTemplate, this._errorException.Message, ErrorGlyph, this._controlTag);
            }

            public string Error
            {
                get
                {
                    return this._errorException.ToString();
                }
            }

            private static string ErrorGlyph
            {
                get
                {
                    if (errorGlyph == null)
                    {
                        errorGlyph = "res://" + typeof(Behavior.ErrorControl).Module.FullyQualifiedName + "//ERROR_GLYPH";
                    }
                    return errorGlyph;
                }
            }
        }

        private sealed class EventSink : Interop.IHTMLElementEvents
        {
            private bool _allowResize;
            private Behavior _behavior;
            private ControlDesigner _designer;
            private Interop.IHTMLElement _element;
            private bool _elementLocked;
            private object _elementLockedLeft;
            private object _elementLockedTop;
            private bool _elementMoving;
            private Interop.ConnectionPointCookie _eventSinkCookie;
            private int _oldHeight;
            private int _oldWidth;
            public static readonly string DesignTimeLockAttribute = "Design_Time_Lock";

            public EventSink(Behavior behavior)
            {
                this._behavior = behavior;
                this._allowResize = true;
            }

            public void Connect(Interop.IHTMLElement element)
            {
                this._designer = (ControlDesigner) this._behavior.Designer;
                try
                {
                    this._element = element;
                    this._eventSinkCookie = new Interop.ConnectionPointCookie(this._element, this, typeof(Interop.IHTMLElementEvents));
                }
                catch (Exception)
                {
                }
            }

            public void Disconnect()
            {
                if (this._eventSinkCookie != null)
                {
                    this._eventSinkCookie.Disconnect();
                    this._eventSinkCookie = null;
                }
                this._element = null;
                this._designer = null;
                this._behavior = null;
            }

            private Interop.IHTMLEventObj GetEventObject()
            {
                Interop.IHTMLDocument2 document = (Interop.IHTMLDocument2) this._element.GetDocument();
                return document.GetParentWindow().GetEvent();
            }

            void Interop.IHTMLElementEvents.Bogus1()
            {
            }

            void Interop.IHTMLElementEvents.Bogus2()
            {
            }

            void Interop.IHTMLElementEvents.Bogus3()
            {
            }

            void Interop.IHTMLElementEvents.Invoke(int dispid, ref Guid g, int lcid, int dwFlags, Interop.DISPPARAMS pdp, object[] pvarRes, Interop.EXCEPINFO pei, int[] nArgError)
            {
                switch (dispid)
                {
                    case -2147418101:
                        this.OnDragStart();
                        return;

                    case -2147418092:
                        this._behavior.SetControlDown(this.GetEventObject().GetCtrlKey());
                        break;

                    case -607:
                        this.OnMouseUp();
                        return;

                    case -606:
                        this.OnMouseMove();
                        return;

                    case -605:
                        this.OnMouseDown();
                        return;

                    case -604:
                    case -603:
                    case -602:
                    case 0x40c:
                    case 0x40d:
                        return;

                    case -601:
                        this.OnDoubleClick();
                        return;

                    case 0x40b:
                        this.OnMove();
                        return;

                    case 0x40e:
                        this.OnMoveStart();
                        return;

                    case 0x40f:
                        this.OnMoveEnd();
                        return;

                    case 0x410:
                    {
                        bool flag = this.OnResizeStart();
                        pvarRes[0] = flag;
                        return;
                    }
                    case 0x411:
                        this.OnResize();
                        return;
                }
            }

            private void OnDoubleClick()
            {
                if (this._behavior.Editor.EditorMode != WebFormsEditorMode.Template)
                {
                    Interop.IHTMLEventObj eventObject = this.GetEventObject();
                    if (eventObject != null)
                    {
                        eventObject.SetCancelBubble(true);
                    }
                    Control component = this._behavior.Control;
                    EventDescriptor defaultEvent = TypeDescriptor.GetDefaultEvent(component);
                    IEventBindingService service = (IEventBindingService) this._behavior.ServiceProvider.GetService(typeof(IEventBindingService));
                    if (defaultEvent != null)
                    {
                        string strAttributeName = "On" + defaultEvent.DisplayName;
                        string attributeValue = null;
                        object[] pvars = new object[1];
                        this._element.GetAttribute(strAttributeName, 0, pvars);
                        if (pvars[0] != null)
                        {
                            attributeValue = pvars[0] as string;
                        }
                        if (attributeValue == null)
                        {
                            attributeValue = service.CreateUniqueMethodName(component, defaultEvent);
                            this._element.SetAttribute(strAttributeName, attributeValue, 0);
                            this._behavior.Editor.SetDirty();
                        }
                        service.ShowCode(component, defaultEvent);
                    }
                }
            }

            private void OnDragStart()
            {
                this._behavior.StartDrag();
            }

            private void OnMouseDown()
            {
            }

            private void OnMouseMove()
            {
                if (this._elementMoving)
                {
                    this._element.GetStyle();
                }
            }

            private void OnMouseUp()
            {
            }

            private void OnMove()
            {
            }

            private void OnMoveEnd()
            {
                if (this._elementMoving)
                {
                    this._elementMoving = false;
                    if (this._elementLocked)
                    {
                        Interop.IHTMLStyle style = this._element.GetStyle();
                        if (style != null)
                        {
                            style.SetTop(this._elementLockedTop);
                            style.SetLeft(this._elementLockedLeft);
                        }
                        this._elementLocked = false;
                    }
                }
            }

            private void OnMoveStart()
            {
                Interop.IHTMLCurrentStyle currentStyle = ((Interop.IHTMLElement2) this._element).GetCurrentStyle();
                string position = currentStyle.GetPosition();
                if ((position != null) && (string.Compare(position, "absolute", true) == 0))
                {
                    this._elementMoving = true;
                }
                if (this._elementMoving)
                {
                    object[] pvars = new object[1];
                    this._element.GetAttribute(DesignTimeLockAttribute, 0, pvars);
                    if (pvars[0] == null)
                    {
                        pvars[0] = currentStyle.GetAttribute(DesignTimeLockAttribute, 0);
                    }
                    if ((pvars[0] != null) && (pvars[0] is string))
                    {
                        this._elementLocked = true;
                        this._elementLockedTop = currentStyle.GetTop();
                        this._elementLockedLeft = currentStyle.GetLeft();
                    }
                }
            }

            private void OnResize()
            {
                Interop.IHTMLStyle runtimeStyle = this._element.GetStyle();
                int pixelWidth = runtimeStyle.GetPixelWidth();
                int pixelHeight = runtimeStyle.GetPixelHeight();
                if ((!this._designer.ReadOnly && (pixelHeight == 0)) && (pixelWidth == 0))
                {
                    pixelWidth = this._element.GetOffsetWidth();
                    pixelHeight = this._element.GetOffsetHeight();
                }
                if ((pixelHeight != 0) || (pixelWidth != 0))
                {
                    runtimeStyle.RemoveAttribute("width", 0);
                    runtimeStyle.RemoveAttribute("height", 0);
                    WebControl control = this._behavior.Control as WebControl;
                    if (control != null)
                    {
                        if (pixelHeight != this._oldHeight)
                        {
                            control.Height = pixelHeight;
                        }
                        if (pixelWidth != this._oldWidth)
                        {
                            control.Width = pixelWidth;
                        }
                    }
                    IComponentChangeService service = (IComponentChangeService) this._behavior.ServiceProvider.GetService(typeof(IComponentChangeService));
                    if (service != null)
                    {
                        service.OnComponentChanged(this._behavior.Control, null, null, null);
                    }
                    if (!this._designer.ReadOnly)
                    {
                        runtimeStyle = ((Interop.IHTMLElement2) this._element).GetRuntimeStyle();
                        runtimeStyle.SetAttribute("Width", pixelWidth, 0);
                        runtimeStyle.SetAttribute("Height", pixelHeight, 0);
                    }
                }
            }

            private bool OnResizeStart()
            {
                bool allowResize = this.AllowResize;
                if (!allowResize)
                {
                    this.GetEventObject().SetCancelBubble(true);
                }
                this._oldWidth = ((Interop.IHTMLElement2) this._element).GetClientWidth();
                this._oldHeight = ((Interop.IHTMLElement2) this._element).GetClientHeight();
                return allowResize;
            }

            internal bool AllowResize
            {
                get
                {
                    bool allowResize = true;
                    if (this._designer != null)
                    {
                        allowResize = this._designer.AllowResize;
                    }
                    return (this._allowResize && allowResize);
                }
                set
                {
                    this._allowResize = value;
                }
            }
        }
    }
}

