namespace Microsoft.Matrix.Packages.Web.Html
{
    using Microsoft.Matrix.Packages.Web;
    using System;
    using System.Runtime.InteropServices;

    public class HtmlEditor : HtmlControl
    {
        private bool _absolutePositioningDesired;
        private bool _absolutePositioningEnabled;
        private bool _bordersDesired;
        private bool _bordersVisible;
        private Microsoft.Matrix.Packages.Web.Html.DataObjectConverter _dataObjectConverter;
        private bool _designModeDesired;
        private bool _designModeEnabled;
        private EditorDocument _editorDocument;
        private Interop.IHTMLEditServices _editServices;
        private bool _glyphsDesired;
        private bool _glyphsVisible;
        private EditorGlyphTable _glyphTable;
        private EditorGrid _grid;
        private int _gridBehaviorHandle;
        private bool _gridBehaviorInstalled;
        private bool _gridVisibleDesired;
        private bool _multipleSelectionDesired;
        private bool _multipleSelectionEnabled;
        private Interop.IPersistStreamInit _persistStream;
        private EditorSelection _selection;
        private EditorTextFormatting _textFormatting;
        private Interop.IOleUndoManager _undoManager;

        public HtmlEditor(IServiceProvider serviceProvider) : this(serviceProvider, true)
        {
        }

        internal HtmlEditor(IServiceProvider serviceProvider, bool fullDocumentMode) : base(serviceProvider, fullDocumentMode)
        {
        }

        public void ClearDirtyState()
        {
            if (base.IsReady)
            {
                base.Exec(0x926, false);
            }
        }

        protected override Interop.IElementBehavior CreateBehavior(string behavior, string behaviorUrl)
        {
            if (behavior.Equals(EditorGrid.Name))
            {
                return this.Grid;
            }
            return null;
        }

        protected virtual EditorSelection CreateSelection()
        {
            return new EditorSelection(this);
        }

        protected internal override object GetService(ref Guid sid)
        {
            if (sid == typeof(Interop.IHTMLEditHost).GUID)
            {
                return this.Grid;
            }
            return base.GetService(ref sid);
        }

        protected override void OnAfterSave()
        {
            if (!base.IsFullDocumentMode)
            {
                this.ClearDirtyState();
            }
        }

        protected override void OnBeforeLoad()
        {
            if (this._gridBehaviorInstalled)
            {
                ((Interop.IHTMLElement2) base.MSHTMLDocument.GetBody()).RemoveBehavior(this._gridBehaviorHandle);
                this._gridBehaviorInstalled = false;
            }
            if (this.BordersVisible)
            {
                this.BordersVisible = false;
                this._bordersDesired = true;
            }
        }

        protected override void OnCreated(EventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("You must specify a non-null EventArgs for OnCreated");
            }
            base.OnCreated(args);
            object[] pvaIn = new object[] { true };
            base.CommandTarget.Exec(ref Interop.Guid_MSHTML, 0x1bbc, 0, pvaIn, null);
            pvaIn[0] = true;
            base.CommandTarget.Exec(ref Interop.Guid_MSHTML, 0x1bbd, 0, pvaIn, null);
            pvaIn[0] = true;
            base.CommandTarget.Exec(ref Interop.Guid_MSHTML, 0x17a1, 0, pvaIn, null);
            pvaIn[0] = true;
            base.CommandTarget.Exec(ref Interop.Guid_MSHTML, 0x91c, 0, pvaIn, null);
            pvaIn[0] = true;
            base.CommandTarget.Exec(ref Interop.Guid_MSHTML, 0x91d, 0, pvaIn, null);
            pvaIn[0] = true;
            base.CommandTarget.Exec(ref Interop.Guid_MSHTML, 0x91e, 0, pvaIn, null);
            pvaIn[0] = true;
            base.CommandTarget.Exec(ref Interop.Guid_MSHTML, 0x91f, 0, pvaIn, null);
            if (this._designModeDesired)
            {
                this.DesignModeEnabled = this._designModeDesired;
                this._designModeDesired = false;
            }
        }

        protected internal override void OnReadyStateComplete(EventArgs args)
        {
            base.OnReadyStateComplete(args);
            this._persistStream = (Interop.IPersistStreamInit) base.MSHTMLDocument;
            this._selection.SynchronizeSelection();
            if (this._multipleSelectionDesired)
            {
                this.MultipleSelectionEnabled = this._multipleSelectionDesired;
            }
            if (this._absolutePositioningDesired)
            {
                this.AbsolutePositioningEnabled = this._absolutePositioningDesired;
            }
            if (this._gridVisibleDesired)
            {
                this.GridVisible = this._gridVisibleDesired;
            }
            if (this._glyphsDesired)
            {
                this.GlyphsVisible = this._glyphsDesired;
            }
            if (this._bordersDesired)
            {
                this.BordersVisible = this._bordersDesired;
            }
        }

        public BatchedUndoUnit OpenBatchUndo(string description)
        {
            BatchedUndoUnit unit = new BatchedUndoUnit(description, this.UndoManager, BatchedUndoType.Undo);
            unit.Open();
            return unit;
        }

        public bool Replace(string searchString, string replaceString, bool matchCase, bool wholeWord, bool searchUp)
        {
            this.Selection.SynchronizeSelection();
            if ((this.Selection.SelectionType == EditorSelectionType.TextSelection) && (this.Selection.Length > 0))
            {
                Interop.IHTMLTxtRange mSHTMLSelection = this.Selection.MSHTMLSelection as Interop.IHTMLTxtRange;
                int flags = (matchCase ? 4 : 0) | (wholeWord ? 2 : 0);
                int count = searchUp ? -10000000 : 0x989680;
                if (mSHTMLSelection.FindText(searchString, count, flags))
                {
                    mSHTMLSelection.SetText(replaceString);
                }
            }
            return base.Find(searchString, matchCase, wholeWord, searchUp);
        }

        public bool AbsolutePositioningEnabled
        {
            get
            {
                return this._absolutePositioningEnabled;
            }
            set
            {
                this._absolutePositioningDesired = value;
                if (base.IsCreated)
                {
                    this._absolutePositioningEnabled = value;
                    object[] argument = new object[] { this._absolutePositioningEnabled };
                    base.Exec(0x95a, argument);
                }
            }
        }

        public bool BordersVisible
        {
            get
            {
                return this._bordersVisible;
            }
            set
            {
                this._bordersDesired = value;
                if (base.IsReady && (this._bordersVisible != this._bordersDesired))
                {
                    this._bordersVisible = value;
                    object[] argument = new object[] { this._bordersVisible };
                    base.Exec(0x918, argument);
                }
            }
        }

        public Microsoft.Matrix.Packages.Web.Html.DataObjectConverter DataObjectConverter
        {
            get
            {
                return this._dataObjectConverter;
            }
            set
            {
                this._dataObjectConverter = value;
            }
        }

        public bool DesignModeEnabled
        {
            get
            {
                return this._designModeEnabled;
            }
            set
            {
                if (this._designModeEnabled != value)
                {
                    if (!base.IsCreated)
                    {
                        this._designModeDesired = value;
                    }
                    else
                    {
                        this._designModeEnabled = value;
                        base.MSHTMLDocument.SetDesignMode(this._designModeEnabled ? "on" : "off");
                    }
                }
            }
        }

        public EditorDocument Document
        {
            get
            {
                if (!base.IsReady)
                {
                    throw new Exception("HtmlEditor not ready yet!");
                }
                if (this._editorDocument == null)
                {
                    this._editorDocument = new EditorDocument(this);
                }
                return this._editorDocument;
            }
        }

        public EditorGlyphTable Glyphs
        {
            get
            {
                if (this._glyphTable == null)
                {
                    this._glyphTable = new EditorGlyphTable();
                }
                return this._glyphTable;
            }
        }

        public bool GlyphsVisible
        {
            get
            {
                return this._glyphsVisible;
            }
            set
            {
                this._glyphsDesired = value;
                if (base.IsReady)
                {
                    this._glyphsVisible = value;
                    if (this._glyphsVisible)
                    {
                        base.Exec(0x921, this._glyphTable.DefinitonString);
                    }
                    else
                    {
                        base.Exec(0x920);
                    }
                }
            }
        }

        private EditorGrid Grid
        {
            get
            {
                if (this._grid == null)
                {
                    this._grid = new EditorGrid();
                }
                return this._grid;
            }
        }

        public int GridSize
        {
            get
            {
                return this.Grid.GridSize;
            }
            set
            {
                this.Grid.GridSize = value;
            }
        }

        public bool GridVisible
        {
            get
            {
                return this.Grid.GridVisible;
            }
            set
            {
                this._gridVisibleDesired = value;
                if (base.IsReady)
                {
                    if (value && !this._gridBehaviorInstalled)
                    {
                        Interop.IHTMLElement2 body = (Interop.IHTMLElement2) base.MSHTMLDocument.GetBody();
                        string bstrUrl = EditorGrid.URL + "#" + EditorGrid.Name;
                        object pvarFactory = this;
                        this._gridBehaviorHandle = body.AddBehavior(bstrUrl, ref pvarFactory);
                        this._gridBehaviorInstalled = true;
                    }
                    this.Grid.GridVisible = value;
                }
            }
        }

        public virtual bool IsDirty
        {
            get
            {
                return ((this.DesignModeEnabled && base.IsReady) && ((this._persistStream != null) && (this._persistStream.IsDirty() == 0)));
            }
        }

        private Interop.IHTMLEditServices MSHTMLEditServices
        {
            get
            {
                if (this._editServices == null)
                {
                    Interop.IServiceProvider mSHTMLDocument = base.MSHTMLDocument as Interop.IServiceProvider;
                    Guid sid = new Guid(0x3050f7f9, (ushort) 0x98b5, (ushort) 0x11cf, 0xbb, 130, 0, 170, 0, 0xbd, 0xce, 11);
                    Guid gUID = typeof(Interop.IHTMLEditServices).GUID;
                    IntPtr nullIntPtr = Interop.NullIntPtr;
                    if ((mSHTMLDocument.QueryService(ref sid, ref gUID, out nullIntPtr) == 0) && (nullIntPtr != Interop.NullIntPtr))
                    {
                        this._editServices = (Interop.IHTMLEditServices) Marshal.GetObjectForIUnknown(nullIntPtr);
                        Marshal.Release(nullIntPtr);
                    }
                }
                return this._editServices;
            }
        }

        public bool MultipleSelectionEnabled
        {
            get
            {
                return this._multipleSelectionEnabled;
            }
            set
            {
                this._multipleSelectionDesired = value;
                if (base.IsReady)
                {
                    this._multipleSelectionEnabled = value;
                    object[] pvaIn = new object[] { this._multipleSelectionEnabled };
                    int num = base.CommandTarget.Exec(ref Interop.Guid_MSHTML, 0x959, 0, pvaIn, null);
                }
            }
        }

        public EditorSelection Selection
        {
            get
            {
                if (this._selection == null)
                {
                    this._selection = this.CreateSelection();
                }
                return this._selection;
            }
        }

        public bool SnapEnabled
        {
            get
            {
                return this.Grid.SnapEnabled;
            }
            set
            {
                this.Grid.SnapEnabled = value;
            }
        }

        public EditorTextFormatting TextFormatting
        {
            get
            {
                if (!base.IsReady)
                {
                    throw new Exception("HtmlDocument not ready yet!");
                }
                if (this._textFormatting == null)
                {
                    this._textFormatting = new EditorTextFormatting(this);
                }
                return this._textFormatting;
            }
        }

        private Interop.IOleUndoManager UndoManager
        {
            get
            {
                if (this._undoManager == null)
                {
                    Interop.IServiceProvider mSHTMLDocument = base.MSHTMLDocument as Interop.IServiceProvider;
                    Guid gUID = typeof(Interop.IOleUndoManager).GUID;
                    Guid sid = typeof(Interop.IOleUndoManager).GUID;
                    IntPtr nullIntPtr = Interop.NullIntPtr;
                    if ((mSHTMLDocument.QueryService(ref sid, ref gUID, out nullIntPtr) == 0) && (nullIntPtr != Interop.NullIntPtr))
                    {
                        this._undoManager = (Interop.IOleUndoManager) Marshal.GetObjectForIUnknown(nullIntPtr);
                        Marshal.Release(nullIntPtr);
                    }
                }
                return this._undoManager;
            }
        }
    }
}

