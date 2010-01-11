namespace Microsoft.Matrix.Packages.Web.Html
{
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Html.Elements;
    using System;

    public sealed class EditorDocument
    {
        private HtmlEditor _editor;

        public EditorDocument(HtmlEditor editor)
        {
            this._editor = editor;
        }

        public void InsertButton()
        {
            this._editor.Exec(0x877);
        }

        public void InsertHtml(string html)
        {
            this.Selection.SynchronizeSelection();
            if (this.Selection.SelectionType == EditorSelectionType.ElementSelection)
            {
                Interop.IHtmlControlRange mSHTMLSelection = (Interop.IHtmlControlRange) this.Selection.MSHTMLSelection;
                if (mSHTMLSelection.GetLength() == 1)
                {
                    Interop.IHTMLElement element = mSHTMLSelection.Item(0);
                    if ((string.Compare(element.GetTagName(), "div", true) == 0) || (string.Compare(element.GetTagName(), "td", true) == 0))
                    {
                        element.InsertAdjacentHTML("beforeEnd", html);
                    }
                }
            }
            else
            {
                Interop.IHTMLTxtRange range2 = (Interop.IHTMLTxtRange) this.Selection.MSHTMLSelection;
                Interop.IHTMLElement parentElement = range2.ParentElement();
                Interop.IHTMLElement body = this._editor.MSHTMLDocument.GetBody();
                Interop.IHTMLElement peer = this._editor.GetContentElement(ElementWrapperTable.GetWrapper(body, this._editor)).Peer;
                if (this._editor.IsFullDocumentMode)
                {
                    range2.PasteHTML(html);
                }
                else
                {
                    bool flag = false;
                    while (parentElement != body)
                    {
                        if (parentElement == peer)
                        {
                            flag = true;
                            break;
                        }
                        parentElement = parentElement.GetParentElement();
                    }
                    if (!flag)
                    {
                        peer.InsertAdjacentHTML("beforeEnd", html);
                    }
                    else
                    {
                        range2.PasteHTML(html);
                    }
                }
            }
        }

        public void InsertHyperlink(string url, string description)
        {
            this.Selection.SynchronizeSelection();
            if (((this.Selection.SelectionType == EditorSelectionType.TextSelection) || (this.Selection.SelectionType == EditorSelectionType.Empty)) && (this.Selection.Length == 0))
            {
                this.InsertHtml("<a href=\"" + url + "\">" + description + "</a>");
            }
            else
            {
                this._editor.Exec(0x84c, url);
            }
        }

        public void InsertListBox()
        {
            this._editor.Exec(0x876);
        }

        public void InsertRadioButton()
        {
            this._editor.Exec(0x874);
        }

        public void InsertTextArea()
        {
            this._editor.Exec(0x872);
        }

        public void InsertTextBox()
        {
            this._editor.Exec(0x871);
        }

        public bool CanInsertButton
        {
            get
            {
                return this._editor.IsCommandEnabled(0x877);
            }
        }

        public bool CanInsertHtml
        {
            get
            {
                if (this.Selection.SelectionType != EditorSelectionType.ElementSelection)
                {
                    return true;
                }
                Interop.IHtmlControlRange mSHTMLSelection = (Interop.IHtmlControlRange) this.Selection.MSHTMLSelection;
                if (mSHTMLSelection.GetLength() == 1)
                {
                    Interop.IHTMLElement element = mSHTMLSelection.Item(0);
                    if ((string.Compare(element.GetTagName(), "div", true) == 0) || (string.Compare(element.GetTagName(), "td", true) == 0))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool CanInsertHyperlink
        {
            get
            {
                if (((this.Selection.SelectionType == EditorSelectionType.TextSelection) || (this.Selection.SelectionType == EditorSelectionType.Empty)) && (this.Selection.Length == 0))
                {
                    return this.CanInsertHtml;
                }
                return this._editor.IsCommandEnabled(0x84c);
            }
        }

        public bool CanInsertListBox
        {
            get
            {
                return this._editor.IsCommandEnabled(0x876);
            }
        }

        public bool CanInsertRadioButton
        {
            get
            {
                return this._editor.IsCommandEnabled(0x874);
            }
        }

        public bool CanInsertTextArea
        {
            get
            {
                return this._editor.IsCommandEnabled(0x872);
            }
        }

        public bool CanInsertTextBox
        {
            get
            {
                return this._editor.IsCommandEnabled(0x871);
            }
        }

        private EditorSelection Selection
        {
            get
            {
                return this._editor.Selection;
            }
        }
    }
}

