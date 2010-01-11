namespace Microsoft.Matrix.Framework.Mobile.UI.Design
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    public sealed class Interop
    {
        private Interop()
        {
        }

        [ComImport, Guid("3050F1D8-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true)]
        internal interface IHtmlBodyElement
        {
            void SetBackground([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBackground();
            void SetBgProperties([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetBgProperties();
            void SetLeftMargin([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetLeftMargin();
            void SetTopMargin([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetTopMargin();
            void SetRightMargin([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetRightMargin();
            void SetBottomMargin([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBottomMargin();
            void SetNoWrap([In, MarshalAs(UnmanagedType.Bool)] bool p);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetNoWrap();
            void SetBgColor([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetBgColor();
            void SetText([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetText();
            void SetLink([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetLink();
            void SetVLink([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetVLink();
            void SetALink([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetALink();
            void SetOnload([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnload();
            void SetOnunload([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnunload();
            void SetScroll([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetScroll();
            void SetOnselect([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnselect();
            void SetOnbeforeunload([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnbeforeunload();
            [return: MarshalAs(UnmanagedType.Interface)]
            object CreateTextRange();
        }

        [InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("332C4425-26CB-11D0-B483-00C04FD90119"), ComVisible(true)]
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
            object GetSelection();
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
            object GetParentWindow();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetStyleSheets();
            void SetOnbeforeupdate([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnbeforeupdate();
            void SetOnerrorupdate([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnerrorupdate();
            [return: MarshalAs(UnmanagedType.BStr)]
            string toString();
            [return: MarshalAs(UnmanagedType.Interface)]
            object CreateStyleSheet([In, MarshalAs(UnmanagedType.BStr)] string bstrHref, [In, MarshalAs(UnmanagedType.I4)] int lIndex);
        }

        [Guid("3050F1FF-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true)]
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
            object GetStyle();
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
            void InsertAdjacentHTML([In, MarshalAs(UnmanagedType.BStr)] string where, [In, MarshalAs(UnmanagedType.BStr)] string html);
            void InsertAdjacentText([In, MarshalAs(UnmanagedType.BStr)] string where, [In, MarshalAs(UnmanagedType.BStr)] string text);
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

        [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("3050F21F-98B5-11CF-BB82-00AA00BDCE0B")]
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
    }
}

