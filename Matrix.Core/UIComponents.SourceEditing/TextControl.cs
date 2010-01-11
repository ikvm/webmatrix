namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using Microsoft.Matrix;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class TextControl : Control, ISupportInitialize, ICommandHandler
    {
        private TextView _activeView;
        private Microsoft.Matrix.UIComponents.SourceEditing.TextBuffer _buffer;
        private ColorInfoTable _colorTable;
        private bool _convertTabs;
        private bool _draggingInternal;
        private bool _helpEnabled;
        private bool _initializing;
        private bool _lineNumbersVisible;
        private int _marginPadding;
        private int _marginWidth;
        private bool _readOnly;
        private bool _resizeGripVisible;
        private bool _showWhitespace;
        private int _tabSize;
        private ITextColorizer _textColorizer;
        private ITextControlHost _textHost;
        private ITextLanguage _textLanguage;
        private TextManager _textManager;
        private bool _trimTrailingWhitespace;
        private ArrayList _views;
        private readonly object EventActiveTextViewChanged;
        private readonly object EventLocationChanged;
        private readonly object EventSelectionChanged;
        private readonly object EventShowContextMenu;
        private readonly object EventTextBufferChanged;

        public event TextViewEventHandler ActiveTextViewChanged
        {
            add
            {
                base.Events.AddHandler(this.EventActiveTextViewChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(this.EventActiveTextViewChanged, value);
            }
        }

        public event TextViewEventHandler CaretLocationChanged
        {
            add
            {
                base.Events.AddHandler(this.EventLocationChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(this.EventLocationChanged, value);
            }
        }

        public event EventHandler SelectionChanged
        {
            add
            {
                base.Events.AddHandler(this.EventSelectionChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(this.EventSelectionChanged, value);
            }
        }

        public event ShowContextMenuEventHandler ShowContextMenu
        {
            add
            {
                base.Events.AddHandler(this.EventShowContextMenu, value);
            }
            remove
            {
                base.Events.RemoveHandler(this.EventShowContextMenu, value);
            }
        }

        public event TextBufferEventHandler TextBufferChanged
        {
            add
            {
                base.Events.AddHandler(this.EventTextBufferChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(this.EventTextBufferChanged, value);
            }
        }

        public TextControl()
        {
            this.EventShowContextMenu = new object();
            this.EventActiveTextViewChanged = new object();
            this.EventSelectionChanged = new object();
            this.EventLocationChanged = new object();
            this.EventTextBufferChanged = new object();
            this._buffer = new Microsoft.Matrix.UIComponents.SourceEditing.TextBuffer();
        }

        public TextControl(Stream stream)
        {
            this.EventShowContextMenu = new object();
            this.EventActiveTextViewChanged = new object();
            this.EventSelectionChanged = new object();
            this.EventLocationChanged = new object();
            this.EventTextBufferChanged = new object();
            this._buffer = new Microsoft.Matrix.UIComponents.SourceEditing.TextBuffer(new StreamReader(stream));
        }

        public TextControl(string text)
        {
            this.EventShowContextMenu = new object();
            this.EventActiveTextViewChanged = new object();
            this.EventSelectionChanged = new object();
            this.EventLocationChanged = new object();
            this.EventTextBufferChanged = new object();
            this._buffer = new Microsoft.Matrix.UIComponents.SourceEditing.TextBuffer(new StringReader(text));
        }

        internal void AddTextView(TextView view)
        {
            if (view.Font != this.Font)
            {
                view.Font = this.Font;
            }
            else
            {
                view.UpdateFont();
            }
            if (!this._views.Contains(view))
            {
                this._views.Add(view);
                base.Controls.Add(view);
                if (this._textColorizer != null)
                {
                    view.UpdateTextLanguage();
                }
                if (this._textHost != null)
                {
                    this._textHost.OnTextViewCreated(view);
                }
            }
        }

        protected virtual void BeginInit()
        {
            this._initializing = true;
            this._views = new ArrayList();
            this.LineNumbersVisible = true;
            this.MarginWidth = 12;
            this.ConvertTabsToSpaces = true;
            this.MarginPadding = 10;
            this.TabSize = 4;
            this.BackColor = SystemColors.Window;
            this.ShowWhitespace = false;
            this.TrimTrailingWhitespace = true;
        }

        public void CenterCaret()
        {
            if (this.ActiveView != null)
            {
                this.ActiveView.CenterCaret();
            }
        }

        public void ClearSelection()
        {
            if (this.ActiveView != null)
            {
                this.ActiveView.ResetSelection();
            }
        }

        public void Copy()
        {
            if (this.ActiveView != null)
            {
                this.ActiveView.Copy();
            }
        }

        protected TextView CreateTextView()
        {
            return new TextView(this, this._buffer, this._textManager);
        }

        public void Cut()
        {
            if (this.ActiveView != null)
            {
                this.ActiveView.Cut();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._textManager.UnregisterTextControl(this);
                this._textManager = null;
                this._textLanguage = null;
                this._textColorizer = null;
                this._textHost = null;
                this._buffer = null;
            }
            base.Dispose(disposing);
        }

        protected virtual void EndInit()
        {
            this._initializing = false;
            this.Initialize();
        }

        public TextBufferSpan Find(string searchString, bool matchCase, bool wholeWord, bool searchUp, bool inSelection)
        {
            if (this.ActiveView != null)
            {
                return this.ActiveView.Find(searchString, matchCase, wholeWord, searchUp, inSelection);
            }
            return null;
        }

        internal void FireDragEnter(DragEventArgs e)
        {
            this.OnDragEnter(e);
        }

        protected virtual bool HandleCommand(Command command)
        {
            return ((this.ActiveView != null) && ((ICommandHandler) this.ActiveView).HandleCommand(command));
        }

        protected void Initialize()
        {
            this._buffer.TextBufferChanged += new TextBufferEventHandler(this.OnTextBufferChanged);
            if (this.Site != null)
            {
                this._textManager = this.Site.GetService(typeof(TextManager)) as TextManager;
            }
            if (this._textManager == null)
            {
                this._textManager = new TextManager(this.Site);
            }
            this._textManager.RegisterTextControl(this);
            this.QuerySiteForLanguage();
            TextView view = this.CreateTextView();
            view.Dock = DockStyle.Fill;
            this.AddTextView(view);
            this.ActiveView = view;
        }

        public void Load(Stream stream)
        {
            this.QuerySiteForLanguage();
            this._buffer.Load(new StreamReader(stream));
        }

        public void Load(char[] chars)
        {
            this.Load(chars, 0, chars.Length);
        }

        public void Load(char[] chars, int startIndex, int length)
        {
            this.Load(new MemoryStream(Encoding.UTF8.GetBytes(chars, startIndex, length)));
        }

        bool ICommandHandler.HandleCommand(Command command)
        {
            return this.HandleCommand(command);
        }

        bool ICommandHandler.UpdateCommand(Command command)
        {
            return this.UpdateCommand(command);
        }

        protected void OnActiveTextViewChanged()
        {
            TextViewEventHandler handler = (TextViewEventHandler) base.Events[this.EventActiveTextViewChanged];
            if (handler != null)
            {
                handler(this, new TextViewEventArgs(this.ActiveView));
            }
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            foreach (TextView view in this._views)
            {
                view.BackColor = this.BackColor;
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            foreach (TextView view in this._views)
            {
                view.Font = this.Font;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (this.ActiveView != null)
            {
                this.ActiveView.ShowCaret();
            }
            foreach (TextView view in this._views)
            {
                view.RepaintSelection();
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (this.ActiveView != null)
            {
                this.ActiveView.HideCaret();
            }
            foreach (TextView view in this._views)
            {
                view.RepaintSelection();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (this.ActiveView != null)
            {
                this.ActiveView.OnParentMouseWheel(e.Delta);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        protected virtual void OnShowContextMenu(ShowContextMenuEventArgs e)
        {
            ShowContextMenuEventHandler handler = (ShowContextMenuEventHandler) base.Events[this.EventShowContextMenu];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnTextBufferChanged(object sender, TextBufferEventArgs e)
        {
            TextBufferEventHandler handler = (TextBufferEventHandler) base.Events[this.EventTextBufferChanged];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected internal virtual void OnViewLocationChanged(TextView view)
        {
            if (view == this.ActiveView)
            {
                EventHandler handler = (EventHandler) base.Events[this.EventLocationChanged];
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        protected internal virtual void OnViewSelectionChanged(TextView view)
        {
            if (view == this.ActiveView)
            {
                EventHandler handler = (EventHandler) base.Events[this.EventSelectionChanged];
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        public void Paste()
        {
            if (this.ActiveView != null)
            {
                this.ActiveView.Paste();
            }
        }

        public void Paste(IDataObject dataObj)
        {
            if (dataObj == null)
            {
                throw new ArgumentNullException("dataObj");
            }
            this.ActiveView.Paste(dataObj);
        }

        public override bool PreProcessMessage(ref Message msg)
        {
            switch (msg.Msg)
            {
                case 0x100:
                    return this.ProcessCmdKey(ref msg, ((Keys) ((int) msg.WParam)) | Control.ModifierKeys);

                case 0x102:
                    return false;
            }
            return base.PreProcessMessage(ref msg);
        }

        protected bool QuerySiteForLanguage()
        {
            if (this.Site != null)
            {
                ITextLanguage service = this.Site.GetService(typeof(ITextLanguage)) as ITextLanguage;
                if (service != this._textLanguage)
                {
                    this.SetTextLanguage(service);
                    return true;
                }
            }
            return false;
        }

        public void Redo()
        {
            if (this.ActiveView != null)
            {
                this.ActiveView.Redo();
            }
        }

        public bool Replace(string searchString, string replaceString, bool matchCase, bool wholeWord, bool searchUp, bool replaceAll, bool inSelection)
        {
            return ((this.ActiveView != null) && this.ActiveView.Replace(searchString, replaceString, matchCase, wholeWord, searchUp, replaceAll, inSelection));
        }

        public void Save(Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(this._buffer.Text);
            writer.Flush();
        }

        public void Select(TextBufferSpan span)
        {
            if (span == null)
            {
                throw new ArgumentNullException("span");
            }
            if (this.ActiveView != null)
            {
                this.ActiveView.Select(span);
            }
        }

        public void SelectAll()
        {
            if (this.ActiveView != null)
            {
                this.ActiveView.SelectAll();
            }
        }

        public void SetText(string text, bool preserveUndo)
        {
            if (preserveUndo)
            {
                foreach (TextView view in this._views)
                {
                    view.FreezePaint();
                }
                try
                {
                    TextBufferLocation startLocation = this._buffer.CreateFirstCharacterLocation();
                    TextBufferLocation endLocation = this._buffer.CreateLastCharacterLocation();
                    try
                    {
                        this._buffer.BeginBatchUndo();
                        try
                        {
                            this.HandleCommand(new TextBufferCommand(3, startLocation, endLocation));
                            this.HandleCommand(new TextBufferCommand(4, startLocation, text));
                        }
                        finally
                        {
                            this._buffer.EndBatchUndo();
                        }
                    }
                    finally
                    {
                        startLocation.Dispose();
                        endLocation.Dispose();
                    }
                }
                finally
                {
                    foreach (TextView view2 in this._views)
                    {
                        view2.UnfreezePaint();
                    }
                    base.Invalidate();
                }
            }
            else
            {
                this.Text = text;
            }
        }

        private void SetTextLanguage(ITextLanguage language)
        {
            this._textLanguage = language;
            if (this._textHost != null)
            {
                this._textHost.Dispose();
                this._textHost = null;
            }
            if (this._textColorizer != null)
            {
                this._textColorizer.Dispose();
                this._textColorizer = null;
            }
            if (this._textLanguage != null)
            {
                this._textHost = this._textLanguage.GetTextControlHost(this, this.Site);
                this._textColorizer = this._textLanguage.GetColorizer(this.Site);
            }
            foreach (TextView view in this._views)
            {
                view.UpdateTextLanguage();
                if (this._textHost != null)
                {
                    this._textHost.OnTextViewCreated(view);
                }
            }
        }

        void ISupportInitialize.BeginInit()
        {
            this.BeginInit();
        }

        void ISupportInitialize.EndInit()
        {
            this.EndInit();
        }

        public void Undo()
        {
            if (this.ActiveView != null)
            {
                this.ActiveView.Undo();
            }
        }

        protected virtual bool UpdateCommand(Command command)
        {
            return ((this.ActiveView != null) && ((ICommandHandler) this.ActiveView).UpdateCommand(command));
        }

        protected override void WndProc(ref Message msg)
        {
            bool flag = false;
            switch (msg.Msg)
            {
                case 0x100:
                {
                    int wParam = (int) msg.WParam;
                    flag = ((ICommandHandler) this._activeView).HandleCommand(new TextBufferCommand(80, wParam));
                    Keys key = (Keys)((int)msg.WParam);
                    if (!flag)
                    {
                        flag = this.ProcessDialogKey(((Keys) ((int) msg.WParam)) | Control.ModifierKeys);
                    }
                    break;
                }
                case 0x102:
                {
                    int commandVal = (int) msg.WParam;
                    flag = ((ICommandHandler) this._activeView).HandleCommand(new TextBufferCommand(0x51, commandVal));
                    break;
                }
                case 0x7b:
                {
                    int lParam;
                    int num4;
                    bool keyboard = false;
                    if (((int) msg.LParam) == -1)
                    {
                        keyboard = true;
                        int messagePos = Interop.GetMessagePos();
                        lParam = messagePos & 0xffff;
                        num4 = (messagePos >> 0x10) & 0xffff;
                    }
                    else
                    {
                        lParam = (short) ((int) msg.LParam);
                        num4 = ((int) msg.LParam) >> 0x10;
                    }
                    ShowContextMenuEventArgs e = new ShowContextMenuEventArgs(base.PointToClient(new Point(lParam, num4)), keyboard);
                    this.OnShowContextMenu(e);
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                base.WndProc(ref msg);
            }
        }

        public TextView ActiveView
        {
            get
            {
                return this._activeView;
            }
            set
            {
                if ((this._activeView != value) && this._views.Contains(value))
                {
                    TextView view = this._activeView;
                    if (view != null)
                    {
                        view.HideCaret();
                    }
                    this._activeView = value;
                    this._activeView.ShowCaret();
                    if (view != null)
                    {
                        view.RepaintSelection();
                    }
                    this._activeView.RepaintSelection();
                    this.OnActiveTextViewChanged();
                }
            }
        }

        public Microsoft.Matrix.UIComponents.SourceEditing.TextBuffer Buffer
        {
            get
            {
                return this._buffer;
            }
        }

        public bool CanCopy
        {
            get
            {
                TextBufferCommand command = new TextBufferCommand(0x40);
                ((ICommandHandler) this.ActiveView).UpdateCommand(command);
                return command.Enabled;
            }
        }

        public bool CanCut
        {
            get
            {
                TextBufferCommand command = new TextBufferCommand(0x3f);
                ((ICommandHandler) this.ActiveView).UpdateCommand(command);
                return command.Enabled;
            }
        }

        public bool CanPaste
        {
            get
            {
                TextBufferCommand command = new TextBufferCommand(0x41);
                ((ICommandHandler) this.ActiveView).UpdateCommand(command);
                return command.Enabled;
            }
        }

        public bool CanRedo
        {
            get
            {
                TextBufferCommand command = new TextBufferCommand(6);
                ((ICommandHandler) this.ActiveView).UpdateCommand(command);
                return command.Enabled;
            }
        }

        public bool CanUndo
        {
            get
            {
                TextBufferCommand command = new TextBufferCommand(5);
                ((ICommandHandler) this.ActiveView).UpdateCommand(command);
                return command.Enabled;
            }
        }

        public int CaretColumnIndex
        {
            get
            {
                if (this.ActiveView != null)
                {
                    return this.ActiveView.ColumnIndex;
                }
                return -1;
            }
            set
            {
                if (this.ActiveView != null)
                {
                    this.ActiveView.ColumnIndex = value;
                }
            }
        }

        public int CaretLineIndex
        {
            get
            {
                if (this.ActiveView != null)
                {
                    return this.ActiveView.LineIndex;
                }
                return -1;
            }
            set
            {
                if (this.ActiveView != null)
                {
                    this.ActiveView.LineIndex = value;
                }
            }
        }

        public int CaretLineLength
        {
            get
            {
                return this.ActiveView.LineLength;
            }
        }

        internal ITextColorizer Colorizer
        {
            get
            {
                return this._textColorizer;
            }
        }

        internal ColorInfoTable ColorTable
        {
            get
            {
                if (this._colorTable == null)
                {
                    this._colorTable = this._textManager.GetColorTable(this._textLanguage);
                }
                return this._colorTable;
            }
        }

        public bool ConvertTabsToSpaces
        {
            get
            {
                return this._convertTabs;
            }
            set
            {
                if (this._convertTabs != value)
                {
                    this._convertTabs = value;
                }
            }
        }

        internal bool DraggingInternal
        {
            get
            {
                return this._draggingInternal;
            }
            set
            {
                this._draggingInternal = value;
            }
        }

        public bool HelpEnabled
        {
            get
            {
                return this._helpEnabled;
            }
            set
            {
                this._helpEnabled = value;
            }
        }

        public bool InsertMode
        {
            get
            {
                return this.ActiveView.InsertMode;
            }
            set
            {
                this.ActiveView.InsertMode = value;
            }
        }

        public bool IsInitializing
        {
            get
            {
                return this._initializing;
            }
        }

        public int LineCount
        {
            get
            {
                return this._buffer.LineCount;
            }
        }

        public virtual bool LineNumbersVisible
        {
            get
            {
                return this._lineNumbersVisible;
            }
            set
            {
                if (this._lineNumbersVisible != value)
                {
                    this._lineNumbersVisible = value;
                    foreach (TextView view in this._views)
                    {
                        view.UpdateLayout();
                    }
                }
            }
        }

        public virtual int MarginPadding
        {
            get
            {
                return this._marginPadding;
            }
            set
            {
                if (this._marginPadding != value)
                {
                    this._marginPadding = value;
                    foreach (TextView view in this._views)
                    {
                        view.UpdateLayout();
                    }
                }
            }
        }

        public virtual int MarginWidth
        {
            get
            {
                return this._marginWidth;
            }
            set
            {
                if (this._marginWidth != value)
                {
                    this._marginWidth = value;
                    foreach (TextView view in this._views)
                    {
                        view.UpdateLayout();
                    }
                }
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this._readOnly;
            }
            set
            {
                if (this._readOnly != value)
                {
                    this._readOnly = value;
                }
            }
        }

        public virtual bool ResizeGripVisible
        {
            get
            {
                return this._resizeGripVisible;
            }
            set
            {
                if (this._resizeGripVisible != value)
                {
                    this._resizeGripVisible = value;
                }
            }
        }

        public string SelectedText
        {
            get
            {
                return this.ActiveView.SelectedText;
            }
        }

        public TextBufferSpan Selection
        {
            get
            {
                return this.ActiveView.Selection;
            }
        }

        public bool SelectionExists
        {
            get
            {
                return this.ActiveView.SelectionExists;
            }
        }

        public virtual bool ShowWhitespace
        {
            get
            {
                return this._showWhitespace;
            }
            set
            {
                this._showWhitespace = value;
                foreach (TextView view in this._views)
                {
                    view.UpdateLayout();
                }
            }
        }

        public override ISite Site
        {
            set
            {
                if (!this._initializing)
                {
                    throw new ApplicationException("Can't set site when not initializing");
                }
                base.Site = value;
            }
        }

        public int TabbedColumnIndex
        {
            get
            {
                if (this.ActiveView != null)
                {
                    return this.ActiveView.TabbedColumnIndex;
                }
                return -1;
            }
        }

        public virtual int TabSize
        {
            get
            {
                return this._tabSize;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException();
                }
                if (this._tabSize != value)
                {
                    this._tabSize = value;
                    foreach (TextView view in this._views)
                    {
                        view.UpdateLayout();
                    }
                }
            }
        }

        public override string Text
        {
            get
            {
                if (this._trimTrailingWhitespace)
                {
                    this.ActiveView.TrimTrailingWhitespace();
                    foreach (TextView view in this._views)
                    {
                        view.UpdateLayout();
                    }
                }
                return this._buffer.Text;
            }
            set
            {
                this.QuerySiteForLanguage();
                this.ClearSelection();
                this._buffer.Text = value;
            }
        }

        public Microsoft.Matrix.UIComponents.SourceEditing.TextBuffer TextBuffer
        {
            get
            {
                return this._buffer;
            }
        }

        protected internal ITextLanguage TextLanguage
        {
            get
            {
                if (this._textLanguage == null)
                {
                    return Microsoft.Matrix.UIComponents.SourceEditing.TextLanguage.Instance;
                }
                return this._textLanguage;
            }
        }

        public virtual bool TrimTrailingWhitespace
        {
            get
            {
                return this._trimTrailingWhitespace;
            }
            set
            {
                this._trimTrailingWhitespace = value;
            }
        }
    }
}

