namespace Microsoft.Matrix.Packages.Web.Html
{
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Html.Elements;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public class EditorSelection
    {
        private Interop.IHTMLDocument2 _document;
        private HtmlEditor _editor;
        private ArrayList _elements;
        private ArrayList _items;
        private int _maxZIndex;
        private int _minZIndex;
        private object _mshtmlSelection;
        private bool _sameParentValid;
        private EventHandler _selectionChangedHandler;
        private int _selectionLength;
        private EditorTable _tableEditor;
        private string _text;
        private EditorSelectionType _type;
        public static readonly string DesignTimeLockAttribute = "Design_Time_Lock";

        public event EventHandler SelectionChanged
        {
            add
            {
                this._selectionChangedHandler = (EventHandler) Delegate.Combine(this._selectionChangedHandler, value);
            }
            remove
            {
                if (this._selectionChangedHandler != null)
                {
                    this._selectionChangedHandler = (EventHandler) Delegate.Remove(this._selectionChangedHandler, value);
                }
            }
        }

        public EditorSelection(HtmlEditor editor)
        {
            this._editor = editor;
            this._minZIndex = 100;
            this._maxZIndex = 0x63;
        }

        public void AlignBottom()
        {
            this.SynchronizeSelection();
            if (this.SelectionType == EditorSelectionType.ElementSelection)
            {
                int count = this._items.Count;
                Interop.IHTMLElement element = (Interop.IHTMLElement) this._items[count - 1];
                int num2 = element.GetStyle().GetPixelTop() + element.GetOffsetHeight();
                BatchedUndoUnit unit = this._editor.OpenBatchUndo("Align Left");
                try
                {
                    for (int i = 0; i < (count - 1); i++)
                    {
                        element = (Interop.IHTMLElement) this._items[i];
                        element.GetStyle().SetTop(num2 - element.GetOffsetHeight());
                    }
                }
                catch
                {
                }
                finally
                {
                    unit.Close();
                }
            }
        }

        public void AlignHorizontalCenter()
        {
            this.SynchronizeSelection();
            if (this.SelectionType == EditorSelectionType.ElementSelection)
            {
                int count = this._items.Count;
                Interop.IHTMLElement element = (Interop.IHTMLElement) this._items[count - 1];
                int num2 = element.GetStyle().GetPixelLeft() + (element.GetOffsetWidth() / 2);
                BatchedUndoUnit unit = this._editor.OpenBatchUndo("Align Left");
                try
                {
                    for (int i = 0; i < (count - 1); i++)
                    {
                        element = (Interop.IHTMLElement) this._items[i];
                        element.GetStyle().SetLeft(num2 - (element.GetOffsetWidth() / 2));
                    }
                }
                catch
                {
                }
                finally
                {
                    unit.Close();
                }
            }
        }

        public void AlignLeft()
        {
            this.SynchronizeSelection();
            if (this.SelectionType == EditorSelectionType.ElementSelection)
            {
                int count = this._items.Count;
                Interop.IHTMLElement element = (Interop.IHTMLElement) this._items[count - 1];
                int pixelLeft = element.GetStyle().GetPixelLeft();
                BatchedUndoUnit unit = this._editor.OpenBatchUndo("Align Left");
                try
                {
                    for (int i = 0; i < (count - 1); i++)
                    {
                        element = (Interop.IHTMLElement) this._items[i];
                        element.GetStyle().SetLeft(pixelLeft);
                    }
                }
                catch
                {
                }
                finally
                {
                    unit.Close();
                }
            }
        }

        public void AlignRight()
        {
            this.SynchronizeSelection();
            if (this.SelectionType == EditorSelectionType.ElementSelection)
            {
                int count = this._items.Count;
                Interop.IHTMLElement element = (Interop.IHTMLElement) this._items[count - 1];
                int num2 = element.GetStyle().GetPixelLeft() + element.GetOffsetWidth();
                BatchedUndoUnit unit = this._editor.OpenBatchUndo("Align Left");
                try
                {
                    for (int i = 0; i < (count - 1); i++)
                    {
                        element = (Interop.IHTMLElement) this._items[i];
                        element.GetStyle().SetLeft(num2 - element.GetOffsetWidth());
                    }
                }
                catch
                {
                }
                finally
                {
                    unit.Close();
                }
            }
        }

        public void AlignTop()
        {
            this.SynchronizeSelection();
            if (this.SelectionType == EditorSelectionType.ElementSelection)
            {
                int count = this._items.Count;
                Interop.IHTMLElement element = (Interop.IHTMLElement) this._items[count - 1];
                int pixelTop = element.GetStyle().GetPixelTop();
                BatchedUndoUnit unit = this._editor.OpenBatchUndo("Align Left");
                try
                {
                    for (int i = 0; i < (count - 1); i++)
                    {
                        element = (Interop.IHTMLElement) this._items[i];
                        element.GetStyle().SetTop(pixelTop);
                    }
                }
                catch
                {
                }
                finally
                {
                    unit.Close();
                }
            }
        }

        public void AlignVerticalCenter()
        {
            this.SynchronizeSelection();
            if (this.SelectionType == EditorSelectionType.ElementSelection)
            {
                int count = this._items.Count;
                Interop.IHTMLElement element = (Interop.IHTMLElement) this._items[count - 1];
                int num2 = element.GetStyle().GetPixelTop() + (element.GetOffsetHeight() / 2);
                BatchedUndoUnit unit = this._editor.OpenBatchUndo("Align Left");
                try
                {
                    for (int i = 0; i < (count - 1); i++)
                    {
                        element = (Interop.IHTMLElement) this._items[i];
                        element.GetStyle().SetTop(num2 - (element.GetOffsetHeight() / 2));
                    }
                }
                catch
                {
                }
                finally
                {
                    unit.Close();
                }
            }
        }

        public void BringToFront()
        {
            this.SynchronizeSelection();
            if (this.SelectionType == EditorSelectionType.ElementSelection)
            {
                if (this._items.Count > 1)
                {
                    int num = this._maxZIndex;
                    int count = this._items.Count;
                    Interop.IHTMLStyle[] styleArray = new Interop.IHTMLStyle[count];
                    int[] numArray = new int[count];
                    for (int i = 0; i < count; i++)
                    {
                        styleArray[i] = ((Interop.IHTMLElement) this._items[i]).GetStyle();
                        numArray[i] = (int) styleArray[i].GetZIndex();
                        if (numArray[i] < num)
                        {
                            num = numArray[i];
                        }
                    }
                    int num4 = (this._maxZIndex + 1) - num;
                    BatchedUndoUnit unit = this._editor.OpenBatchUndo("Align Left");
                    try
                    {
                        for (int j = 0; j < count; j++)
                        {
                            int p = num4 + numArray[j];
                            if (numArray[j] == this._minZIndex)
                            {
                                this._minZIndex++;
                            }
                            styleArray[j].SetZIndex(p);
                            if (p > this._maxZIndex)
                            {
                                this._maxZIndex = p;
                            }
                        }
                        return;
                    }
                    catch
                    {
                        return;
                    }
                    finally
                    {
                        unit.Close();
                    }
                }
                Interop.IHTMLElement element2 = (Interop.IHTMLElement) this._items[0];
                object zIndex = element2.GetStyle().GetZIndex();
                if ((zIndex != null) && !(zIndex is DBNull))
                {
                    if (((int) zIndex) == this._maxZIndex)
                    {
                        return;
                    }
                    if (((int) zIndex) == this._minZIndex)
                    {
                        this._minZIndex++;
                    }
                }
                element2.GetStyle().SetZIndex(++this._maxZIndex);
            }
        }

        public void ClearSelection()
        {
            this._editor.Exec(0x7d7);
        }

        protected virtual object CreateElementWrapper(Interop.IHTMLElement element)
        {
            return ElementWrapperTable.GetWrapper(element, this._editor);
        }

        public CommandInfo GetAbsolutePositionInfo()
        {
            return this._editor.GetCommandInfo(0x95d);
        }

        protected virtual Interop.IHTMLElement GetIHtmlElement(object o)
        {
            if (o is Interop.IHTMLElement)
            {
                return (Interop.IHTMLElement) o;
            }
            if (o is Element)
            {
                return ((Element) o).Peer;
            }
            return null;
        }

        public CommandInfo GetLockInfo()
        {
            if (this.SelectionType == EditorSelectionType.ElementSelection)
            {
                foreach (Interop.IHTMLElement element in this._items)
                {
                    if (!this.IsElement2DPositioned(element))
                    {
                        return 0;
                    }
                    if (this.IsElementLocked(element))
                    {
                        return (CommandInfo.Checked | CommandInfo.Enabled);
                    }
                    return CommandInfo.Enabled;
                }
            }
            return 0;
        }

        public string GetOuterHtml()
        {
            string outerHTML = string.Empty;
            try
            {
                outerHTML = ((Interop.IHTMLElement) this._items[0]).GetOuterHTML();
                outerHTML = ((Interop.IHTMLElement) this._items[0]).GetOuterHTML();
            }
            catch
            {
            }
            return outerHTML;
        }

        public ArrayList GetParentHierarchy(object o)
        {
            Interop.IHTMLElement iHtmlElement = this.GetIHtmlElement(o);
            if (iHtmlElement == null)
            {
                return null;
            }
            if (iHtmlElement.GetTagName().ToLower().Equals("body"))
            {
                return null;
            }
            ArrayList list = new ArrayList();
            iHtmlElement = iHtmlElement.GetParentElement();
            while ((iHtmlElement != null) && !iHtmlElement.GetTagName().ToLower().Equals("body"))
            {
                Element wrapper = ElementWrapperTable.GetWrapper(iHtmlElement, this._editor);
                if (this.IsSelectableElement(wrapper))
                {
                    list.Add(wrapper);
                }
                iHtmlElement = iHtmlElement.GetParentElement();
            }
            if (iHtmlElement != null)
            {
                Element element = ElementWrapperTable.GetWrapper(iHtmlElement, this._editor);
                if (this.IsSelectableElement(element))
                {
                    list.Add(element);
                }
            }
            return list;
        }

        private bool IsElement2DPositioned(Interop.IHTMLElement elem)
        {
            Interop.IHTMLElement2 element = (Interop.IHTMLElement2) elem;
            string position = element.GetCurrentStyle().GetPosition();
            return ((position != null) && (string.Compare(position, "absolute", true) == 0));
        }

        private bool IsElementLocked(Interop.IHTMLElement elem)
        {
            object[] pvars = new object[1];
            elem.GetAttribute(DesignTimeLockAttribute, 0, pvars);
            if (pvars[0] == null)
            {
                pvars[0] = elem.GetStyle().GetAttribute(DesignTimeLockAttribute, 0);
            }
            return ((pvars[0] != null) && (pvars[0] is string));
        }

        protected virtual bool IsSelectableElement(Element element)
        {
            if (!this._editor.IsFullDocumentMode)
            {
                return (element.TagName.ToLower() != "body");
            }
            return true;
        }

        public void MatchHeight()
        {
            this.SynchronizeSelection();
            if (this.SelectionType == EditorSelectionType.ElementSelection)
            {
                int count = this._items.Count;
                Interop.IHTMLElement element = (Interop.IHTMLElement) this._items[count - 1];
                int offsetHeight = element.GetOffsetHeight();
                BatchedUndoUnit unit = this._editor.OpenBatchUndo("Align Left");
                try
                {
                    for (int i = 0; i < (count - 1); i++)
                    {
                        element = (Interop.IHTMLElement) this._items[i];
                        element.GetStyle().SetPixelHeight(offsetHeight);
                    }
                }
                catch
                {
                }
                finally
                {
                    unit.Close();
                }
            }
        }

        public void MatchSize()
        {
            this.SynchronizeSelection();
            if (this.SelectionType == EditorSelectionType.ElementSelection)
            {
                int count = this._items.Count;
                Interop.IHTMLElement element = (Interop.IHTMLElement) this._items[count - 1];
                int offsetWidth = element.GetOffsetWidth();
                int offsetHeight = element.GetOffsetHeight();
                BatchedUndoUnit unit = this._editor.OpenBatchUndo("Align Left");
                try
                {
                    for (int i = 0; i < (count - 1); i++)
                    {
                        Interop.IHTMLStyle style = ((Interop.IHTMLElement) this._items[i]).GetStyle();
                        style.SetPixelHeight(offsetHeight);
                        style.SetPixelWidth(offsetWidth);
                    }
                }
                catch
                {
                }
                finally
                {
                    unit.Close();
                }
            }
        }

        public void MatchWidth()
        {
            this.SynchronizeSelection();
            if (this.SelectionType == EditorSelectionType.ElementSelection)
            {
                int count = this._items.Count;
                Interop.IHTMLElement element = (Interop.IHTMLElement) this._items[count - 1];
                int offsetWidth = element.GetOffsetWidth();
                BatchedUndoUnit unit = this._editor.OpenBatchUndo("Align Left");
                try
                {
                    for (int i = 0; i < (count - 1); i++)
                    {
                        element = (Interop.IHTMLElement) this._items[i];
                        element.GetStyle().SetPixelWidth(offsetWidth);
                    }
                }
                catch
                {
                }
                finally
                {
                    unit.Close();
                }
            }
        }

        protected virtual void OnSelectionChanged()
        {
            if (this._selectionChangedHandler != null)
            {
                this._selectionChangedHandler(this, EventArgs.Empty);
            }
        }

        public void RemoveHyperlink()
        {
            this._editor.Exec(0x84d);
        }

        public bool SelectElement(object o)
        {
            ArrayList elements = new ArrayList(1);
            elements.Add(o);
            return this.SelectElements(elements);
        }

        public bool SelectElements(ICollection elements)
        {
            Interop.IHTMLElement body = this._editor.MSHTMLDocument.GetBody();
            object obj2 = (body as Interop.IHTMLTextContainer).createControlRange();
            Interop.IHtmlControlRange range = obj2 as Interop.IHtmlControlRange;
            if (range == null)
            {
                return false;
            }
            Interop.IHtmlControlRange2 range2 = obj2 as Interop.IHtmlControlRange2;
            if (range2 == null)
            {
                return false;
            }
            int num = 0;
            foreach (object obj3 in elements)
            {
                Interop.IHTMLElement iHtmlElement = this.GetIHtmlElement(obj3);
                if (iHtmlElement == null)
                {
                    return false;
                }
                num = range2.addElement(iHtmlElement);
                if (num != 0)
                {
                    break;
                }
            }
            if (num == 0)
            {
                range.Select();
            }
            else
            {
                Interop.IHTMLTxtRange range3 = ((Interop.IHtmlBodyElement) body).createTextRange();
                if (range3 != null)
                {
                    foreach (object obj4 in elements)
                    {
                        try
                        {
                            Interop.IHTMLElement element = this.GetIHtmlElement(obj4);
                            if (element == null)
                            {
                                return false;
                            }
                            range3.MoveToElementText(element);
                            continue;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    range3.Select();
                }
            }
            return true;
        }

        public void SendToBack()
        {
            this.SynchronizeSelection();
            if (this.SelectionType == EditorSelectionType.ElementSelection)
            {
                if (this._items.Count > 1)
                {
                    int num = this._minZIndex;
                    int count = this._items.Count;
                    Interop.IHTMLStyle[] styleArray = new Interop.IHTMLStyle[count];
                    int[] numArray = new int[count];
                    for (int i = 0; i < count; i++)
                    {
                        styleArray[i] = ((Interop.IHTMLElement) this._items[i]).GetStyle();
                        numArray[i] = (int) styleArray[i].GetZIndex();
                        if (numArray[i] > num)
                        {
                            num = numArray[i];
                        }
                    }
                    int num4 = num - (this._minZIndex - 1);
                    BatchedUndoUnit unit = this._editor.OpenBatchUndo("Align Left");
                    try
                    {
                        for (int j = 0; j < count; j++)
                        {
                            int p = numArray[j] - num4;
                            if (numArray[j] == this._maxZIndex)
                            {
                                this._maxZIndex--;
                            }
                            styleArray[j].SetZIndex(p);
                            if (p < this._minZIndex)
                            {
                                this._minZIndex = p;
                            }
                        }
                        return;
                    }
                    catch
                    {
                        return;
                    }
                    finally
                    {
                        unit.Close();
                    }
                }
                Interop.IHTMLElement element2 = (Interop.IHTMLElement) this._items[0];
                object zIndex = element2.GetStyle().GetZIndex();
                if ((zIndex != null) && !(zIndex is DBNull))
                {
                    if (((int) zIndex) == this._minZIndex)
                    {
                        return;
                    }
                    if (((int) zIndex) == this._maxZIndex)
                    {
                        this._maxZIndex--;
                    }
                }
                element2.GetStyle().SetZIndex(--this._minZIndex);
            }
        }

        public void SetOuterHtml(string outerHtml)
        {
            ((Interop.IHTMLElement) this._items[0]).SetOuterHTML(outerHtml);
        }

        public bool SynchronizeSelection()
        {
            if (this._document == null)
            {
                this._document = this._editor.MSHTMLDocument;
            }
            Interop.IHTMLSelectionObject selection = this._document.GetSelection();
            object obj3 = null;
            try
            {
                obj3 = selection.CreateRange();
            }
            catch
            {
            }
            ArrayList list = this._items;
            EditorSelectionType type = this._type;
            int num = this._selectionLength;
            this._type = EditorSelectionType.Empty;
            this._selectionLength = 0;
            if (obj3 != null)
            {
                this._mshtmlSelection = obj3;
                this._items = new ArrayList();
                if (obj3 is Interop.IHTMLTxtRange)
                {
                    Interop.IHTMLTxtRange range = (Interop.IHTMLTxtRange) obj3;
                    Interop.IHTMLElement element = range.ParentElement();
                    if (element != null)
                    {
                        this._text = range.GetText();
                        if (this._text != null)
                        {
                            this._selectionLength = this._text.Length;
                        }
                        else
                        {
                            this._selectionLength = 0;
                        }
                        this._type = EditorSelectionType.TextSelection;
                        if (this.IsSelectableElement(ElementWrapperTable.GetWrapper(element, this._editor)))
                        {
                            this._items.Add(element);
                        }
                    }
                }
                else if (obj3 is Interop.IHtmlControlRange)
                {
                    Interop.IHtmlControlRange range2 = (Interop.IHtmlControlRange) obj3;
                    int length = range2.GetLength();
                    if (length > 0)
                    {
                        this._type = EditorSelectionType.ElementSelection;
                        for (int i = 0; i < length; i++)
                        {
                            Interop.IHTMLElement element2 = range2.Item(i);
                            this._items.Add(element2);
                        }
                        this._selectionLength = length;
                    }
                }
            }
            this._sameParentValid = false;
            bool flag = false;
            if (this._type != type)
            {
                flag = true;
            }
            else if (this._selectionLength != num)
            {
                flag = true;
            }
            else
            {
                int count = 0;
                int num5 = 0;
                if (this._items != null)
                {
                    count = this._items.Count;
                }
                if (list != null)
                {
                    num5 = list.Count;
                }
                if (count != num5)
                {
                    flag = true;
                }
                else if (this._items != null)
                {
                    for (int j = 0; j < this._items.Count; j++)
                    {
                        if (this._items[j] != list[j])
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }
            if (flag)
            {
                this._elements = null;
                this._tableEditor = null;
                this.OnSelectionChanged();
                return true;
            }
            return false;
        }

        public void ToggleAbsolutePosition()
        {
            this._editor.Exec(0x95d, (this.GetAbsolutePositionInfo() & CommandInfo.Checked) == 0);
            this.SynchronizeSelection();
            if (this._type == EditorSelectionType.ElementSelection)
            {
                foreach (Interop.IHTMLElement element in this._items)
                {
                    element.GetStyle().SetZIndex(++this._maxZIndex);
                }
            }
        }

        public void ToggleLock()
        {
            foreach (Interop.IHTMLElement element in this._items)
            {
                Interop.IHTMLStyle style = element.GetStyle();
                if (this.IsElementLocked(element))
                {
                    element.RemoveAttribute(DesignTimeLockAttribute, 0);
                    style.RemoveAttribute(DesignTimeLockAttribute, 0);
                }
                else
                {
                    element.SetAttribute(DesignTimeLockAttribute, "true", 0);
                    style.SetAttribute(DesignTimeLockAttribute, "true", 0);
                }
            }
        }

        public void WrapSelection(string tag)
        {
            this.WrapSelection(tag, null);
        }

        public void WrapSelection(string tag, IDictionary attributes)
        {
            string str = string.Empty;
            if (attributes != null)
            {
                foreach (string str2 in attributes.Keys)
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, str2, "=\"", attributes[str2], "\" " });
                }
            }
            this.SynchronizeSelection();
            if (this.SelectionType == EditorSelectionType.TextSelection)
            {
                Interop.IHTMLTxtRange mSHTMLSelection = (Interop.IHTMLTxtRange) this.MSHTMLSelection;
                string htmlText = mSHTMLSelection.GetHtmlText();
                if (htmlText == null)
                {
                    htmlText = string.Empty;
                }
                string html = "<" + tag + " " + str + ">" + htmlText + "</" + tag + ">";
                mSHTMLSelection.PasteHTML(html);
            }
        }

        public void WrapSelectionInBlockQuote()
        {
            this.WrapSelection("blockquote");
        }

        public void WrapSelectionInDiv()
        {
            this.WrapSelection("div");
        }

        public void WrapSelectionInHyperlink(string url)
        {
            this._editor.Exec(0x84c, url);
        }

        public void WrapSelectionInSpan()
        {
            this.WrapSelection("span");
        }

        public bool CanAlign
        {
            get
            {
                if (this._items.Count < 2)
                {
                    return false;
                }
                if (this._type != EditorSelectionType.ElementSelection)
                {
                    return false;
                }
                foreach (Interop.IHTMLElement element in this._items)
                {
                    if (!this.IsElement2DPositioned(element))
                    {
                        return false;
                    }
                    if (this.IsElementLocked(element))
                    {
                        return false;
                    }
                }
                if (!this.SameParent)
                {
                    return false;
                }
                return true;
            }
        }

        public bool CanChangeZIndex
        {
            get
            {
                if (this._items.Count == 0)
                {
                    return false;
                }
                if (this._type != EditorSelectionType.ElementSelection)
                {
                    return false;
                }
                foreach (Interop.IHTMLElement element in this._items)
                {
                    if (!this.IsElement2DPositioned(element))
                    {
                        return false;
                    }
                }
                if (!this.SameParent)
                {
                    return false;
                }
                return true;
            }
        }

        public bool CanMatchSize
        {
            get
            {
                if (this._items.Count < 2)
                {
                    return false;
                }
                if (this._type != EditorSelectionType.ElementSelection)
                {
                    return false;
                }
                foreach (Interop.IHTMLElement element in this._items)
                {
                    if (this.IsElementLocked(element))
                    {
                        return false;
                    }
                }
                if (!this.SameParent)
                {
                    return false;
                }
                return true;
            }
        }

        public bool CanRemoveHyperlink
        {
            get
            {
                return this._editor.IsCommandEnabled(0x84d);
            }
        }

        public bool CanWrapSelection
        {
            get
            {
                return ((this._selectionLength != 0) && (this.SelectionType == EditorSelectionType.TextSelection));
            }
        }

        protected HtmlEditor Editor
        {
            get
            {
                return this._editor;
            }
        }

        public IList Elements
        {
            get
            {
                if (this._elements == null)
                {
                    this._elements = new ArrayList();
                    foreach (Interop.IHTMLElement element in this._items)
                    {
                        object obj2 = this.CreateElementWrapper(element);
                        if (obj2 != null)
                        {
                            this._elements.Add(obj2);
                        }
                    }
                }
                return ArrayList.ReadOnly(this._elements);
            }
        }

        internal ICollection Items
        {
            get
            {
                return this._items;
            }
        }

        public int Length
        {
            get
            {
                return this._selectionLength;
            }
        }

        protected internal object MSHTMLSelection
        {
            get
            {
                return this._mshtmlSelection;
            }
        }

        private bool SameParent
        {
            get
            {
                if (!this._sameParentValid)
                {
                    IntPtr nullIntPtr = Interop.NullIntPtr;
                    foreach (Interop.IHTMLElement element in this._items)
                    {
                        IntPtr iUnknownForObject = Marshal.GetIUnknownForObject(element.GetParentElement());
                        if (nullIntPtr == Interop.NullIntPtr)
                        {
                            nullIntPtr = iUnknownForObject;
                            continue;
                        }
                        if (nullIntPtr != iUnknownForObject)
                        {
                            Marshal.Release(iUnknownForObject);
                            if (nullIntPtr != Interop.NullIntPtr)
                            {
                                Marshal.Release(nullIntPtr);
                            }
                            this._sameParentValid = false;
                            return this._sameParentValid;
                        }
                        Marshal.Release(iUnknownForObject);
                    }
                    if (nullIntPtr != Interop.NullIntPtr)
                    {
                        Marshal.Release(nullIntPtr);
                    }
                    this._sameParentValid = true;
                }
                return this._sameParentValid;
            }
        }

        public EditorSelectionType SelectionType
        {
            get
            {
                return this._type;
            }
        }

        public EditorTable TableEditor
        {
            get
            {
                if ((this._tableEditor == null) && EditorTable.IsTableSelection(this))
                {
                    this._tableEditor = new EditorTable(this._editor, this);
                }
                return this._tableEditor;
            }
        }

        public string Text
        {
            get
            {
                if (this._type == EditorSelectionType.TextSelection)
                {
                    return this._text;
                }
                return null;
            }
        }
    }
}

