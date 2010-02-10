namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using Microsoft.Matrix;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Diagnostics;

    public class TextView : Control, ICommandHandler
    {
        #region private members
        private AutoCompleteForm _autoCompleteForm;
        private AutoCompleteLabel _autoCompleteHint;
        private int _bottomPaintLineNumber;
        private TextBuffer _buffer;
        private bool _caretHiding;
        private ICommandHandler _firstHandler;
        private int _fontHeight;
        private IntPtr _fontPtr;
        private int _fontWidth;
        private int _horizontalScrollAmount;
        private ScrollBar _horizontalScrollBar;
        private int _horizontalSelectionScrollAmount;
        private bool _ignoreMouseMove;
        private bool _ignoreMouseUp;
        private bool _insertMode;
        private int _lineNumbersWidth;
        private TextBufferLocation _location;
        private bool _mouseDown;
        private bool _mouseDownInSelection;
        private TextControl _owner;
        private int _paintFrozen;
        private TextBufferLocation _preDragLocation;
        private TextBufferSpan _selection;
        private TextBufferLocation _selectionEnd;
        private bool _selectionExists;
        private Timer _selectionScrollTimer;
        private TextBufferLocation _selectionStart;
        private int _smartCursorIndex;
        private IntPtr[] _styledFontPtrs;
        private TextManager _textManager;
        private int _topPaintLineNumber;
        private bool _trimmingWhitespace;
        private bool _updateCaretOnPaint;
        private bool _updateLayoutRequired;
        private ScrollBar _verticalScrollBar;
        private int _verticalSelectionScrollAmount;
        private int _viewLeftIndex;
        private int _viewMaxLineLength;
        private int _viewTopLineNumber;
        private int _visibleColumns;
        private int _visibleLines;
        private const int CtrlAKeyCode = 1;
        private const int CtrlBackKeyCode = 0x7f;
        private const int CtrlCKeyCode = 3;
        private const int CtrlEnterKeyCode = 10;
        private const int CtrlVKeyCode = 0x16;
        private const int CtrlXKeyCode = 0x18;
        private const int CtrlYKeyCode = 0x19;
        private const int CtrlZKeyCode = 0x1a;
        private const int LineNumbersPadding = 3;
        private const int VerticalScrollAmount = 3;

        private double _viewMaxLineWidth = 0;
        private int _viewMaxLineWidthLineIndex = 0;
        #endregion

        internal TextView(TextControl owner, TextBuffer buffer, TextManager manager)
        {
            this._textManager = manager;
            this._owner = owner;
            this._buffer = buffer;
            this._location = this._buffer.CreateTextBufferLocation();
            this._location.LocationChanged += new EventHandler(this.OnLocationChanged);
            this._selectionStart = this._buffer.CreateTextBufferLocation();
            this._selectionEnd = this._buffer.CreateTextBufferLocation();
            this._buffer.TextBufferChanged += new TextBufferEventHandler(this.OnTextBufferChanged);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.Opaque, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            this._horizontalScrollBar = new HScrollBar();
            this._horizontalScrollBar.Cursor = Cursors.Arrow;
            this._horizontalScrollBar.Scroll += new ScrollEventHandler(this.OnHorizontalScrollBarScroll);
            if (this._buffer.MaxLine != null)
            {
                this._viewMaxLineLength = this.GetViewIndex(this._buffer.MaxLine, this._buffer.MaxLineLength);
            }
            else
            {
                this._viewMaxLineLength = 0;
            }
            using (TextBufferLocation location = this._buffer.CreateTextBufferLocation())
            {
                location.GotoLine(0);
                while (location.LineIndex <= this._buffer.LineCount)
                {
                    double num = this.MeasureString(location.Line.Data, 0, location.Line.Length);
                    if (num > this._viewMaxLineWidth)
                    {
                        this._viewMaxLineWidth = num;
                        this._viewMaxLineWidthLineIndex = location.LineIndex;
                    }
                    if (location.MoveDown(1) == 0)
                    {
                        goto Label_018C;
                    }
                }
            }
        Label_018C:
            if (this._fontWidth != 0)
            {
                this._horizontalScrollBar.Maximum = (int)((this._viewMaxLineWidth / ((double)this._fontWidth)) + 0.5);
            }
            else
            {
                this._horizontalScrollBar.Maximum = (int)this._viewMaxLineWidth;
            }
            this._verticalScrollBar = new VScrollBar();
            this._verticalScrollBar.Cursor = Cursors.Arrow;
            this._verticalScrollBar.Scroll += new ScrollEventHandler(this.OnVerticalScrollBarScroll);
            this._verticalScrollBar.Maximum = this._buffer.LineCount;
            this.Cursor = Cursors.IBeam;
            this._viewTopLineNumber = 0;
            this._insertMode = true;
            base.Controls.Add(this._verticalScrollBar);
            base.Controls.Add(this._horizontalScrollBar);
            this.AddCommandHandler(new PlainTextViewCommandHandler(this, this._buffer));
            this.AllowDrop = true;
        }

        public ICommandHandler AddCommandHandler(ICommandHandler handler)
        {
            ICommandHandler handler2 = this._firstHandler;
            this._firstHandler = handler;
            return handler2;
        }

        private void AddDirtyLines(int top, int bottom)
        {
            if (top > bottom)
            {
                int num = top;
                top = bottom;
                bottom = num;
            }
            this._topPaintLineNumber = Math.Min(this._topPaintLineNumber, top);
            this._bottomPaintLineNumber = Math.Max(this._bottomPaintLineNumber, bottom);
            if (this._paintFrozen == 0)
            {
                this.PaintDirtyLines();
            }
        }

        public void CenterCaret()
        {
            this.HandleCommand(new TextBufferCommand(0x34));
        }

        private Point ColorizeLine(TextBufferLocation colorLocation)
        {
            int lineIndex = colorLocation.LineIndex;
            int y = colorLocation.LineIndex;
            if (this.Colorizer != null)
            {
                TextLine line = colorLocation.Line;
                lineIndex = colorLocation.LineIndex;
                int num3 = 0;
                int initialColorState = 0;
                TextLine previous = line;
                while ((previous.Previous != null) && previous.Previous.AttributesDirty)
                {
                    previous = previous.Previous;
                    lineIndex--;
                }
                initialColorState = (previous.Previous == null) ? 0 : previous.Previous.ColorState;
                while (previous != line)
                {
                    char[] data = previous.Data;
                    if (data != null)
                    {
                        previous.SetColorState(this.Colorizer.Colorize(data, previous.Attributes, 0, previous.Length, initialColorState));
                        previous.SetAttributesDirty(false);
                        initialColorState = previous.ColorState;
                    }
                    else
                    {
                        initialColorState = 0;
                    }
                    previous = previous.Next;
                    num3++;
                }
                int colorState = line.ColorState;
                bool flag = true;
                while ((previous != null) && flag)
                {
                    char[] text = previous.Data;
                    colorState = previous.ColorState;
                    previous.SetColorState(this.Colorizer.Colorize(text, previous.Attributes, 0, previous.Length, initialColorState));
                    previous.SetAttributesDirty(false);
                    flag = colorState != previous.ColorState;
                    initialColorState = previous.ColorState;
                    previous = previous.Next;
                    num3++;
                }
                y = lineIndex + num3;
            }
            return new Point(lineIndex, y);
        }

        internal void Copy()
        {
            this.HandleCommand(new TextBufferCommand(0x40));
        }

        private void CopyInternal()
        {
            if (this._selectionExists)
            {
                IDataObject dataObjectFromText = this.TextLanguage.GetDataObjectFromText(this._selection.Text);
                if (dataObjectFromText == null)
                {
                    dataObjectFromText = new DataObject(DataFormats.Text, this._selection.Text);
                }
                Clipboard.SetDataObject(dataObjectFromText, true);
            }
        }

        internal void Cut()
        {
            this.HandleCommand(new TextBufferCommand(0x3f));
        }

        private void CutInternal()
        {
            if (this._selectionExists)
            {
                IDataObject dataObjectFromText = this.TextLanguage.GetDataObjectFromText(this._selection.Text);
                if (dataObjectFromText == null)
                {
                    dataObjectFromText = new DataObject(DataFormats.Text, this._selection.Text);
                }
                Clipboard.SetDataObject(dataObjectFromText, true);
                this.DeleteSelection();
            }
        }

        private void DeleteSelection()
        {
            if (this._selection != null)
            {
                this.HandleCommand(new TextBufferCommand(3, this._selection.Start, this._selection.End));
                this._location.MoveTo(this._selection.Start);
                this.ResetSelection();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void DoSmartCursorMovement()
        {
            if (this._location.ColumnIndex != this._smartCursorIndex)
            {
                if (this._smartCursorIndex < this._location.Line.Length)
                {
                    this._location.ColumnIndex = this._smartCursorIndex;
                }
                else
                {
                    this._location.ColumnIndex = this._location.Line.Length;
                }
            }
        }

        public TextBufferSpan Find(string searchString, bool matchCase, bool wholeWord, bool searchUp, bool inSelection)
        {
            TextBufferLocation startLocation = null;
            TextBufferLocation endLocation = null;
            TextBufferSpan span2;
            try
            {
                if (inSelection)
                {
                    if (!this._selectionExists)
                    {
                        return null;
                    }
                    startLocation = this._selection.Start.Clone();
                    endLocation = this._selection.End.Clone();
                }
                else if (searchUp)
                {
                    startLocation = this._buffer.First.Clone();
                    if (this._selectionExists)
                    {
                        endLocation = this._selection.Start.Clone();
                    }
                    else
                    {
                        endLocation = this._location.Clone();
                    }
                }
                else
                {
                    if (this._selectionExists)
                    {
                        startLocation = this._selection.End.Clone();
                    }
                    else
                    {
                        startLocation = this._location.Clone();
                    }
                    endLocation = this._buffer.Last.Clone();
                }
                TextBufferSpan span = this._buffer.Find(startLocation, endLocation, searchString, matchCase, wholeWord, searchUp);
                if ((span == null) && !inSelection)
                {
                    if (searchUp)
                    {
                        endLocation.MoveTo(this._buffer.Last);
                    }
                    else
                    {
                        endLocation.MoveTo(startLocation);
                        startLocation.MoveTo(this._buffer.First);
                    }
                    span = this._buffer.Find(startLocation, endLocation, searchString, matchCase, wholeWord, searchUp);
                }
                span2 = span;
            }
            finally
            {
                if (startLocation != null)
                {
                    startLocation.Dispose();
                }
                if (endLocation != null)
                {
                    endLocation.Dispose();
                }
            }
            return span2;
        }

        public void FreezePaint()
        {
            this._paintFrozen++;
        }

        private int GetActualIndex(TextLine line, int xPos)
        {
            int index = 0;
            int end = 0;
            int length = line.Length;
            char[] data = line.Data;
            int num4 = (this.MarginPadding + this._lineNumbersWidth) + this.MarginWidth;
            int viewLeftIndex = this.ViewLeftIndex;
            while (index < length)
            {
                if (data[index] == '\t')
                {
                    end += this.TabSize - (end % this.TabSize);
                }
                else
                {
                    end++;
                }
                if (this.ViewLeftIndex < end)
                {
                    num4 += (int)this.MeasureString(data, viewLeftIndex, end - viewLeftIndex);
                    viewLeftIndex = end;
                }
                if (xPos < num4)
                {
                    return index;
                }
                index++;
            }
            return index;
        }

        private Point GetLineColFromXY(int x, int y)
        {
            int curLineIndex = y / this._fontHeight;
            int lineIndex = Math.Max(0, Math.Min((int)(this._buffer.LineCount - 1), (int)(this.ViewTopLineNumber + curLineIndex)));
            int actualIndex = 0;
            using (TextBufferLocation location = this._buffer.CreateTextBufferLocation(this._location))
            {
                location.GotoLine(lineIndex);
                actualIndex = this.GetActualIndex(location.Line, x);
            }
            return new Point(lineIndex, actualIndex);
        }

        private IntPtr GetStyledFont(bool bold, bool italic)
        {
            if (this._styledFontPtrs == null)
            {
                this._styledFontPtrs = new IntPtr[3];
            }
            int index = -1;
            FontStyle regular = FontStyle.Regular;
            if (bold)
            {
                regular |= FontStyle.Bold;
                index++;
            }
            if (italic)
            {
                regular |= FontStyle.Italic;
                index += 2;
            }
            if (this._styledFontPtrs[index] == IntPtr.Zero)
            {
                try
                {
                    this._styledFontPtrs[index] = new Font(this.Font, regular).ToHfont();
                }
                catch
                {
                    this._styledFontPtrs[index] = this._fontPtr;
                }
            }
            return this._styledFontPtrs[index];
        }

        private int GetViewIndex(TextBufferLocation location)
        {
            return this.GetViewIndex(location.Line, location.ColumnIndex);
        }

        private int GetViewIndex(TextLine line, int index)
        {
            int num = 0;
            int num2 = 0;
            int length = line.Length;
            char[] data = line.Data;
            if (data != null)
            {
                while (num < index)
                {
                    if (data[num] == '\t')
                    {
                        num2 += this.TabSize - (num2 % this.TabSize);
                    }
                    else
                    {
                        num2++;
                    }
                    num++;
                }
            }
            return num2;
        }

        public TextBufferSpan GetWordSpan(TextBufferLocation location)
        {
            return this.TextLanguage.GetWordSpan(location, WordType.Current);
        }

        private bool HandleCommand(Command command)
        {
            bool flag = false;
            if (this._firstHandler != null)
            {
                flag = this._firstHandler.HandleCommand(command);
            }
            return flag;
        }

        internal bool HandleViewCommand(Command command)
        {
            bool flag = false;
            bool flag2 = false;
            bool isSelecting = false;
            bool flag4 = false;
            if (command.CommandGroup == typeof(TextBufferCommands))
            {
                int num3;
                int lineIndex;
                TextBufferLocation location;
                int num5;
                switch (command.CommandID)
                {
                    case 30:
                        if (this._location.ColumnIndex >= this._location.Line.Length)
                        {
                            if (this._location.LineIndex < (this._buffer.LineCount - 1))
                            {
                                this._location.MoveDown(1);
                                this._location.ColumnIndex = 0;
                                flag = true;
                                isSelecting = ((TextBufferCommand)command).IsSelecting;
                                this.SetSmartCursorIndex();
                            }
                            else if (!((TextBufferCommand)command).IsSelecting)
                            {
                                flag2 = true;
                            }
                        }
                        else
                        {
                            this._location.ColumnIndex++;
                            flag = true;
                            isSelecting = ((TextBufferCommand)command).IsSelecting;
                            this.SetSmartCursorIndex();
                        }
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x1f:
                        if (this._location.ColumnIndex <= 0)
                        {
                            if (this._location.LineIndex > 0)
                            {
                                this._location.MoveUp(1);
                                this._location.ColumnIndex = this._location.Line.Length;
                                flag = true;
                                isSelecting = ((TextBufferCommand)command).IsSelecting;
                                this.SetSmartCursorIndex();
                            }
                            else if (!((TextBufferCommand)command).IsSelecting)
                            {
                                flag2 = true;
                            }
                        }
                        else
                        {
                            this._location.ColumnIndex--;
                            flag = true;
                            isSelecting = ((TextBufferCommand)command).IsSelecting;
                            this.SetSmartCursorIndex();
                        }
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x20:
                        {
                            isSelecting = ((TextBufferCommand)command).IsSelecting;
                            TextBufferLocation location2 = this._location.Clone();
                            TextBufferSpan wordSpan = null;
                            try
                            {
                                wordSpan = this.TextLanguage.GetWordSpan(location2, WordType.Next);
                                if (wordSpan != null)
                                {
                                    this._location.MoveTo(wordSpan.Start);
                                    flag = true;
                                    this.SetSmartCursorIndex();
                                }
                            }
                            finally
                            {
                                location2.Dispose();
                                if (wordSpan != null)
                                {
                                    wordSpan.Dispose();
                                }
                            }
                            flag4 = true;
                            goto Label_0C3E;
                        }
                    case 0x21:
                        {
                            isSelecting = ((TextBufferCommand)command).IsSelecting;
                            TextBufferLocation location3 = this._location.Clone();
                            TextBufferSpan span2 = null;
                            try
                            {
                                span2 = this.TextLanguage.GetWordSpan(location3, WordType.Previous);
                                if (span2 != null)
                                {
                                    this._location.MoveTo(span2.Start);
                                    flag = true;
                                    this.SetSmartCursorIndex();
                                }
                            }
                            finally
                            {
                                location3.Dispose();
                                if (span2 != null)
                                {
                                    span2.Dispose();
                                }
                            }
                            flag4 = true;
                            goto Label_0C3E;
                        }
                    case 0x22:
                        {
                            int columnIndex = this._location.ColumnIndex;
                            if (this.MoveToFirstCharIndex(this._location) == columnIndex)
                            {
                                this._location.ColumnIndex = 0;
                            }
                            if (columnIndex != this._location.ColumnIndex)
                            {
                                flag = true;
                                isSelecting = ((TextBufferCommand)command).IsSelecting;
                                this.SetSmartCursorIndex();
                            }
                            else
                            {
                                if (!((TextBufferCommand)command).IsSelecting)
                                {
                                    flag2 = true;
                                }
                                this.SetSmartCursorIndex();
                            }
                            flag4 = true;
                            goto Label_0C3E;
                        }
                    case 0x23:
                        {
                            int length = this._location.Line.Length;
                            if (this._location.ColumnIndex == length)
                            {
                                if (!((TextBufferCommand)command).IsSelecting)
                                {
                                    flag2 = true;
                                }
                                this.SetSmartCursorIndex();
                            }
                            else
                            {
                                this._location.ColumnIndex = this._location.Line.Length;
                                flag = true;
                                isSelecting = ((TextBufferCommand)command).IsSelecting;
                                this.SetSmartCursorIndex();
                            }
                            flag4 = true;
                            goto Label_0C3E;
                        }
                    case 0x24:
                        if (this._location.LineIndex >= this._buffer.LineCount)
                        {
                            if (!((TextBufferCommand)command).IsSelecting)
                            {
                                flag2 = true;
                            }
                        }
                        else
                        {
                            this._location.MoveDown(1);
                            flag = true;
                            isSelecting = ((TextBufferCommand)command).IsSelecting;
                            this.DoSmartCursorMovement();
                        }
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x25:
                        if (this._location.LineIndex <= 0)
                        {
                            if (!((TextBufferCommand)command).IsSelecting)
                            {
                                flag2 = true;
                            }
                        }
                        else
                        {
                            this._location.MoveUp(1);
                            flag = true;
                            isSelecting = ((TextBufferCommand)command).IsSelecting;
                            this.DoSmartCursorMovement();
                        }
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x26:
                        num3 = 0;
                        lineIndex = this._location.LineIndex;
                        if ((lineIndex < this.ViewTopLineNumber) || (lineIndex > (this.ViewTopLineNumber + this.VisibleLines)))
                        {
                            num3 = this.VisibleLines / 2;
                        }
                        else
                        {
                            num3 = this._location.LineIndex - this.ViewTopLineNumber;
                        }
                        if (this._location.MoveDown(this._visibleLines) != 0)
                        {
                            flag = true;
                            isSelecting = ((TextBufferCommand)command).IsSelecting;
                            this.DoSmartCursorMovement();
                            this.ViewTopLineNumber = Math.Max(Math.Min((int)(this._location.LineIndex - num3), (int)(this._buffer.LineCount - this._visibleLines)), 0);
                        }
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x27:
                        lineIndex = this._location.LineIndex;
                        if ((lineIndex < this.ViewTopLineNumber) || (lineIndex > (this.ViewTopLineNumber + this.VisibleLines)))
                        {
                            num3 = this.VisibleLines / 2;
                        }
                        else
                        {
                            num3 = Math.Min((int)(this.VisibleLines - 1), (int)(this._location.LineIndex - this.ViewTopLineNumber));
                        }
                        if (this._location.MoveUp(this._visibleLines) != 0)
                        {
                            flag = true;
                            isSelecting = ((TextBufferCommand)command).IsSelecting;
                            this.DoSmartCursorMovement();
                            this.ViewTopLineNumber = Math.Max(this._location.LineIndex - num3, 0);
                        }
                        flag4 = true;
                        goto Label_0C3E;

                    case 40:
                        this._location.MoveTo(this._buffer.Last);
                        flag = true;
                        isSelecting = ((TextBufferCommand)command).IsSelecting;
                        this.SetSmartCursorIndex();
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x29:
                        this._location.MoveTo(this._buffer.First);
                        flag = true;
                        isSelecting = ((TextBufferCommand)command).IsSelecting;
                        this.SetSmartCursorIndex();
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x2a:
                        {
                            isSelecting = ((TextBufferCommand)command).IsSelecting;
                            Point commandPosition = ((TextBufferCommand)command).CommandPosition;
                            this._location.GotoLineColumn(commandPosition.X, commandPosition.Y);
                            flag = true;
                            this.SetSmartCursorIndex();
                            flag4 = true;
                            goto Label_0C3E;
                        }
                    case 0x2b:
                        this._location.GotoLine((int)((TextBufferCommand)command).Data);
                        this.ViewTopLineNumber = Math.Min(this._buffer.LineCount - this.VisibleLines, Math.Max(0, this._location.LineIndex - (this.VisibleLines / 2)));
                        flag = true;
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x2c:
                    case 0x2d:
                    case 0x2e:
                    case 0x2f:
                    case 0x30:
                    case 0x31:
                    case 0x35:
                    case 0x36:
                    case 0x37:
                    case 0x38:
                    case 0x39:
                    case 0x3a:
                    case 0x3b:
                    case 0x44:
                    case 0x45:
                    case 70:
                    case 0x47:
                    case 0x4a:
                    case 0x4b:
                    case 0x4c:
                    case 0x4d:
                    case 0x4e:
                    case 0x4f:
                    case 0x5c:
                    case 0x5d:
                        goto Label_0C3E;

                    case 50:
                        this.ViewTopLineNumber = ((TextBufferCommand)command).CommandValue;
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x33:
                        this.ViewLeftIndex = ((TextBufferCommand)command).CommandValue;
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x34:
                        this.ViewTopLineNumber = Math.Min(this._buffer.LineCount - this.VisibleLines, Math.Max(0, this._location.LineIndex - (this.VisibleLines / 2)));
                        flag4 = true;
                        goto Label_0C3E;

                    case 60:
                        this.ResetSelection();
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x3d:
                        this.UpdateSelection();
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x3e:
                        this.DeleteSelection();
                        this.SetSmartCursorIndex();
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x3f:
                        this.CutInternal();
                        this.SetSmartCursorIndex();
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x40:
                        this.CopyInternal();
                        this.SetSmartCursorIndex();
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x41:
                        this.PasteInternal();
                        this.SetSmartCursorIndex();
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x42:
                        this.FreezePaint();
                        try
                        {
                            this._location.MoveTo(this._buffer.First);
                            this.ResetSelection();
                            this._location.MoveTo(this._buffer.Last);
                            this.UpdateSelection();
                        }
                        finally
                        {
                            this.UnfreezePaint();
                        }
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x43:
                        {
                            TextBufferLocation location4 = this._location.Clone();
                            TextBufferSpan span3 = null;
                            try
                            {
                                span3 = this.TextLanguage.GetWordSpan(location4, WordType.Current);
                                if (span3 != null)
                                {
                                    isSelecting = true;
                                    this._location.MoveTo(span3.Start);
                                    this.ResetSelection();
                                    this._location.MoveTo(span3.End);
                                    flag = true;
                                }
                            }
                            finally
                            {
                                location4.Dispose();
                                if (span3 != null)
                                {
                                    span3.Dispose();
                                }
                            }
                            flag4 = true;
                            goto Label_0C3E;
                        }
                    case 0x48:
                        location = this._buffer.CreateTextBufferLocation(this._selection.Start);
                        num5 = this._selection.End.LineIndex;
                        if (this._selection.End.ColumnIndex == 0)
                        {
                            num5--;
                        }
                        this._buffer.BeginBatchUndo();
                        while (location.LineIndex <= num5)
                        {
                            if (!location.Line.IsEmpty)
                            {
                                this.MoveToFirstCharIndex(location);
                                TextBufferCommand command2 = new TextBufferCommand(70, location);
                                this.HandleCommand(command2);
                            }
                            if (location.MoveDown(1) == 0)
                            {
                                break;
                            }
                        }
                        location.Dispose();
                        this._buffer.EndBatchUndo();
                        this._selection.Start.ColumnIndex = 0;
                        if (this._selection.End.ColumnIndex != 0)
                        {
                            this._selection.End.ColumnIndex = this._selection.End.Line.Length;
                        }
                        this.SetSmartCursorIndex();
                        flag4 = true;
                        goto Label_0C3E;

                    case 0x49:
                        location = this._buffer.CreateTextBufferLocation(this._selection.Start);
                        num5 = this._selection.End.LineIndex;
                        if (this._selection.End.ColumnIndex == 0)
                        {
                            num5--;
                        }
                        this._buffer.BeginBatchUndo();
                        while (location.LineIndex <= num5)
                        {
                            this.MoveToFirstCharIndex(location);
                            TextBufferCommand command3 = new TextBufferCommand(0x47, location);
                            this.HandleCommand(command3);
                            if (location.MoveDown(1) == 0)
                            {
                                break;
                            }
                        }
                        location.Dispose();
                        this._buffer.EndBatchUndo();
                        this._selection.Start.ColumnIndex = 0;
                        if (this._selection.End.ColumnIndex != 0)
                        {
                            this._selection.End.ColumnIndex = this._selection.End.Line.Length;
                        }
                        this.SetSmartCursorIndex();
                        flag4 = true;
                        goto Label_0C3E;

                    case 80:
                        flag4 = this.OnKeyDownCommand(((TextBufferCommand)command).CommandValue);
                        goto Label_0C3E;

                    case 0x51:
                        this.OnKeyPressedCommand(((TextBufferCommand)command).CommandValue);
                        flag4 = true;
                        goto Label_0C3E;

                    case 90:
                        {
                            using (TextBufferLocation location6 = this._location.Clone())
                            {
                                TextBufferSpan span5 = null;
                                try
                                {
                                    //NOTE: 手动修改
                                    span5 = this.TextLanguage.GetWordSpan(location6, WordType.Current);
                                    if (span5 != null)
                                    {
                                        span5.End.MoveTo(this._location);
                                    }
                                    if ((span5 == null) || span5.IsEmpty)
                                    {
                                        span5 = this.TextLanguage.GetWordSpan(location6, WordType.Previous);
                                    }
                                    if ((span5 != null) && !span5.IsEmpty)
                                    {
                                        span5.End.MoveTo(this._location);
                                        this.HandleCommand(new TextBufferCommand(3, span5.Start, span5.End));
                                        this._location.MoveTo(span5.Start);
                                        this.SetSmartCursorIndex();
                                    }

                                }
                                catch (Exception ex)
                                {
                                }
                                finally
                                {
                                    span5.Dispose();
                                    span5 = null;
                                }
                                //using (TextBufferSpan span5 = null)
                                //{
                                //    span5 = this.TextLanguage.GetWordSpan(location6, WordType.Current);
                                //    if (span5 != null)
                                //    {
                                //        span5.End.MoveTo(this._location);
                                //    }
                                //    if ((span5 == null) || span5.IsEmpty)
                                //    {
                                //        span5 = this.TextLanguage.GetWordSpan(location6, WordType.Previous);
                                //    }
                                //    if ((span5 != null) && !span5.IsEmpty)
                                //    {
                                //        span5.End.MoveTo(this._location);
                                //        this.HandleCommand(new TextBufferCommand(3, span5.Start, span5.End));
                                //        this._location.MoveTo(span5.Start);
                                //        this.SetSmartCursorIndex();
                                //    }
                                //}
                                goto Label_0C3E;
                            }
                        }
                    case 0x5b:
                        {
                            using (TextBufferLocation location5 = this._location.Clone())
                            {
                                //NOTE: 手动修改
                                //using (TextBufferSpan span4 = null)
                                //{
                                //    span4 = this.TextLanguage.GetWordSpan(location5, WordType.Current);
                                //    if (span4 != null)
                                //    {
                                //        span4.Start.MoveTo(this._location);
                                //    }
                                //    if ((span4 == null) || span4.IsEmpty)
                                //    {
                                //        span4 = this.TextLanguage.GetWordSpan(location5, WordType.Next);
                                //    }
                                //    if ((span4 != null) && !span4.IsEmpty)
                                //    {
                                //        span4.Start.MoveTo(this._location);
                                //        this.HandleCommand(new TextBufferCommand(3, span4.Start, span4.End));
                                //        this.SetSmartCursorIndex();
                                //    }
                                //}
                                TextBufferSpan span4 = null;
                                try
                                {
                                    span4 = this.TextLanguage.GetWordSpan(location5, WordType.Current);
                                    if (span4 != null)
                                    {
                                        span4.Start.MoveTo(this._location);
                                    }
                                    if ((span4 == null) || span4.IsEmpty)
                                    {
                                        span4 = this.TextLanguage.GetWordSpan(location5, WordType.Next);
                                    }
                                    if ((span4 != null) && !span4.IsEmpty)
                                    {
                                        span4.Start.MoveTo(this._location);
                                        this.HandleCommand(new TextBufferCommand(3, span4.Start, span4.End));
                                        this.SetSmartCursorIndex();
                                    }

                                }
                                catch (Exception ex)
                                {
                                }
                                finally
                                {
                                    span4.Dispose();
                                    span4 = null;
                                }

                                goto Label_0C3E;
                            }
                        }
                    case 0x5e:
                        if (this._owner.HelpEnabled)
                        {
                            if (this._selectionExists)
                            {
                                this.TextLanguage.ShowHelp(this.ServiceProvider, this._selection.Start);
                            }
                            else
                            {
                                this.TextLanguage.ShowHelp(this.ServiceProvider, this._location);
                            }
                        }
                        goto Label_0C3E;
                }
            }
        Label_0C3E:
            if (flag)
            {
                if (isSelecting)
                {
                    this.UpdateSelection();
                    return flag4;
                }
                this.ResetSelection();
                return flag4;
            }
            if (flag2 && !isSelecting)
            {
                this.ResetSelection();
            }
            return flag4;
        }

        internal void HideCaret()
        {
            if ((this._owner.ActiveView == this) && !this._caretHiding)
            {
                Interop.HideCaret(base.Handle);
                Interop.DestroyCaret();
                this._caretHiding = true;
            }
        }

        public void HideHint()
        {
            if (this._autoCompleteHint != null)
            {
                this._autoCompleteHint.Visible = false;
                base.Controls.Remove(this._autoCompleteHint);
                this._autoCompleteHint.Dispose();
                this._autoCompleteHint = null;
            }
        }

        private bool IsInSelection(int lineIndex, int columnIndex, int mouseX, int mouseY)
        {
            if ((mouseX > ((this.MarginPadding + this._lineNumbersWidth) + this.MarginWidth)) && (this._selectionExists && this._selection.Contains(lineIndex, columnIndex + 1)))
            {
                using (TextBufferLocation location = this._location.Clone())
                {
                    location.GotoLine(lineIndex);
                    return ((((mouseX - this.MarginPadding) - this._lineNumbersWidth) - this.MarginWidth) <= ((int)this.MeasureString(location.Line.Data, this.ViewLeftIndex, location.Line.Length - this.ViewLeftIndex)));
                }
            }
            return false;
        }

        bool ICommandHandler.HandleCommand(Command command)
        {
            return this.HandleCommand(command);
        }

        bool ICommandHandler.UpdateCommand(Command command)
        {
            bool flag = false;
            if (this._firstHandler != null)
            {
                flag = this._firstHandler.UpdateCommand(command);
            }
            return flag;
        }

        private int MoveToFirstCharIndex(TextBufferLocation location)
        {
            int index = 0;
            char[] data = location.Line.Data;
            if (data != null)
            {
                while (index < location.Line.Length)
                {
                    if (!char.IsWhiteSpace(data[index]))
                    {
                        break;
                    }
                    index++;
                }
            }
            location.ColumnIndex = index;
            return index;
        }

        private void OnAutoCompleteFormClosed(object sender, EventArgs e)
        {
            this._autoCompleteForm.Closed -= new EventHandler(this.OnAutoCompleteFormClosed);
            if (this._autoCompleteForm.DialogResult == DialogResult.OK)
            {
                this.HandleCommand(new TextBufferCommand(4, this._location, this._autoCompleteForm.PickedItemSuffix));
            }
            ((IDisposable)this._autoCompleteForm).Dispose();
            this._autoCompleteForm = null;
            this._owner.Focus();
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
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
            this.HideCaret();
            string textFromDataObject = this.TextLanguage.GetTextFromDataObject(e.Data, this.ServiceProvider);
            if (textFromDataObject != null)
            {
                if (this._owner.DraggingInternal)
                {
                    if (this._selection.Contains(this._location))
                    {
                        goto Label_02AB;
                    }
                    this.HandleCommand(new TextBufferCommand(7, this._location));
                    TextBufferLocation location = null;
                    TextBufferLocation location2 = null;
                    try
                    {
                        if ((this._location.LineIndex < this._selection.Start.LineIndex) || ((this._location.LineIndex == this._selection.Start.LineIndex) && (this._location.ColumnIndex < this._selection.Start.ColumnIndex)))
                        {
                            if ((e.KeyState & 8) == 0)
                            {
                                this._firstHandler.HandleCommand(new TextBufferCommand(3, this._selection.Start, this._selection.End));
                            }
                            location = this._location.Clone();
                            this._firstHandler.HandleCommand(new TextBufferCommand(4, this._location, textFromDataObject));
                            location2 = this._location.Clone();
                        }
                        else
                        {
                            int num = 0;
                            num = this._selection.Start.ColumnIndex + (this._location.ColumnIndex - this._selection.End.ColumnIndex);
                            location = this._location.Clone();
                            this._firstHandler.HandleCommand(new TextBufferCommand(4, this._location, textFromDataObject));
                            location2 = this._location.Clone();
                            if ((e.KeyState & 8) == 0)
                            {
                                this._firstHandler.HandleCommand(new TextBufferCommand(3, this._selection.Start, this._selection.End));
                                if (this._selection.End.LineIndex == location.LineIndex)
                                {
                                    if (this._selection.IsSingleLine)
                                    {
                                        location2.ColumnIndex += num - location.ColumnIndex;
                                    }
                                    location.ColumnIndex = num;
                                }
                            }
                        }
                        this._location.MoveTo(location);
                        this.ResetSelection();
                        this._location.MoveTo(location2);
                        this.UpdateSelection();
                        goto Label_02AB;
                    }
                    finally
                    {
                        location.Dispose();
                        location2.Dispose();
                        this.HandleCommand(new TextBufferCommand(8, this._location));
                    }
                }
                this._firstHandler.HandleCommand(new TextBufferCommand(60));
                this._firstHandler.HandleCommand(new TextBufferCommand(4, this._location, textFromDataObject));
                this._firstHandler.HandleCommand(new TextBufferCommand(0x3d));
            }
        Label_02AB:
            this._owner.DraggingInternal = false;
            this._owner.Focus();
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            if (this.TextLanguage.SupportsDataObject(this.ServiceProvider, e.Data))
            {
                this.ShowCaret(false);
                if (this._owner.DraggingInternal & ((e.KeyState & 8) == 0))
                {
                    e.Effect = DragDropEffects.Move;
                }
                else
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
            this._preDragLocation = this._buffer.CreateTextBufferLocation(this._location);
            this._owner.FireDragEnter(e);
        }

        protected override void OnDragLeave(EventArgs e)
        {
            this._location.MoveTo(this._preDragLocation);
            this.HideCaret();
            this._owner.DraggingInternal = false;
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            if (this.TextLanguage.SupportsDataObject(this.ServiceProvider, e.Data))
            {
                if (this._owner.DraggingInternal & ((e.KeyState & 8) == 0))
                {
                    e.Effect = DragDropEffects.Move;
                }
                else
                {
                    e.Effect = DragDropEffects.Copy;
                }
                Point lineColFromXY = base.PointToClient(new Point(e.X, e.Y));
                lineColFromXY = this.GetLineColFromXY(lineColFromXY.X, lineColFromXY.Y);
                this._location.GotoLineColumn(lineColFromXY.X, lineColFromXY.Y);
                this.UpdateCaretPosition(false);
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.UpdateFont();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this._caretHiding = true;
            if (this._updateLayoutRequired)
            {
                this.UpdateLayout();
            }
        }

        protected void OnHorizontalScrollBarScroll(object sender, ScrollEventArgs e)
        {
            if (this._fontWidth != 0)
            {
                this.ViewLeftIndex = Math.Min(e.NewValue, Math.Max(0, ((int)(((this._viewMaxLineWidth / ((double)this._fontWidth)) * 2.0) + 0.5)) - this._visibleColumns));
            }
            else
            {
                this.ViewLeftIndex = Math.Min(e.NewValue, Math.Max(0, this._viewMaxLineLength - this._visibleColumns));
            }
        }

        internal bool OnKeyDownCommand(int keyCode)
        {
            bool flag = false;
            switch (keyCode)
            {
                case 0x1b:
                    if (this._selectionExists)
                    {
                        flag = this.HandleCommand(new TextBufferCommand(60));
                    }
                    return flag;

                case 0x1c:
                case 0x1d:
                case 30:
                case 0x1f:
                case 0x20:
                case 0x29:
                case 0x2a:
                case 0x2b:
                case 0x2c:
                    return flag;

                case 0x21:
                    if ((Control.ModifierKeys & Keys.Control) == Keys.None)
                    {
                        flag = this.HandleCommand(new TextBufferCommand(0x27, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None));
                    }
                    return flag;

                case 0x22:
                    if ((Control.ModifierKeys & Keys.Control) == Keys.None)
                    {
                        flag = this.HandleCommand(new TextBufferCommand(0x26, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None));
                    }
                    return flag;

                case 0x23:
                    if ((Control.ModifierKeys & Keys.Control) == Keys.None)
                    {
                        return this.HandleCommand(new TextBufferCommand(0x23, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None));
                    }
                    return this.HandleCommand(new TextBufferCommand(40, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None));

                case 0x24:
                    if ((Control.ModifierKeys & Keys.Control) == Keys.None)
                    {
                        return this.HandleCommand(new TextBufferCommand(0x22, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None));
                    }
                    return this.HandleCommand(new TextBufferCommand(0x29, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None));

                case 0x25:
                    if ((Control.ModifierKeys & Keys.Control) == Keys.None)
                    {
                        return this.HandleCommand(new TextBufferCommand(0x1f, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None));
                    }
                    return this.HandleCommand(new TextBufferCommand(0x21, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None));

                case 0x26:
                    if ((Control.ModifierKeys & Keys.Control) == Keys.None)
                    {
                        return this.HandleCommand(new TextBufferCommand(0x25, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None));
                    }
                    return this.HandleCommand(new TextBufferCommand(50, this.ViewTopLineNumber - 1));

                case 0x27:
                    if ((Control.ModifierKeys & Keys.Control) == Keys.None)
                    {
                        return this.HandleCommand(new TextBufferCommand(30, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None));
                    }
                    return this.HandleCommand(new TextBufferCommand(0x20, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None));

                case 40:
                    if ((Control.ModifierKeys & Keys.Control) == Keys.None)
                    {
                        return this.HandleCommand(new TextBufferCommand(0x24, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None));
                    }
                    return this.HandleCommand(new TextBufferCommand(50, this.ViewTopLineNumber + 1));

                case 0x2d:
                    if ((Control.ModifierKeys & Keys.Control) == Keys.None)
                    {
                        if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
                        {
                            this.Paste();
                        }
                        else
                        {
                            this.InsertMode = !this.InsertMode;
                        }
                        break;
                    }
                    if (this._selectionExists)
                    {
                        this.Copy();
                    }
                    break;

                case 0x2e:
                    {
                        bool flag2 = (Control.ModifierKeys & Keys.Control) != Keys.None;
                        if (!this._selectionExists)
                        {
                            if (flag2)
                            {
                                return this.HandleCommand(new TextBufferCommand(0x5b));
                            }
                            int columnIndex = this._location.ColumnIndex;
                            int length = this._location.Line.Length;
                            if (columnIndex < length)
                            {
                                this.HandleCommand(new TextBufferCommand(2, this._location, (char)keyCode));
                                return flag;
                            }
                            if (this._location.LineIndex < (this._buffer.LineCount - 1))
                            {
                                TextBufferLocation startLocation = this._buffer.CreateTextBufferLocation(this._location);
                                startLocation.MoveDown(1);
                                startLocation.ColumnIndex = 0;
                                flag = this.HandleCommand(new TextBufferCommand(3, startLocation, this._location));
                                startLocation.Dispose();
                            }
                            return flag;
                        }
                        if ((Control.ModifierKeys & Keys.Shift) == Keys.None)
                        {
                            if (flag2)
                            {
                                this._buffer.BeginBatchUndo();
                                try
                                {
                                    flag = this.HandleCommand(new TextBufferCommand(0x3e, this._location));
                                    return this.HandleCommand(new TextBufferCommand(0x5b));
                                }
                                finally
                                {
                                    this._buffer.EndBatchUndo();
                                }
                            }
                            return this.HandleCommand(new TextBufferCommand(0x3e, this._location));
                        }
                        this.Cut();
                        return true;
                    }
                case 9:
                    return true;

                case 0x70:
                    return this.HandleCommand(new TextBufferCommand(0x5e));

                case 0x71:
                    return flag;

                case 0x72:
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.None)
                        {
                            return flag;
                        }
                        string searchString = string.Empty;
                        if (!this._selectionExists)
                        {
                            using (TextBufferSpan span = this.TextLanguage.GetWordSpan(this._location, WordType.Current))
                            {
                                if (span == null)
                                {
                                    return flag;
                                }
                                searchString = span.Text;
                            }
                        }
                        else
                        {
                            searchString = this.SelectedText;
                        }
                        using (TextBufferSpan span2 = this.Find(searchString, true, false, false, false))
                        {
                            this._location.MoveTo(span2.Start);
                            this.Select(span2);
                        }
                        return true;
                    }
                default:
                    return flag;
            }
            return true;
        }

        internal void OnKeyPressedCommand(int keyCode)
        {
            bool flag = false;
            switch (keyCode)
            {
                case 0x16:
                    this.HandleCommand(new TextBufferCommand(0x41, this._location));
                    flag = true;
                    goto Label_0364;

                case 0x18:
                    this.HandleCommand(new TextBufferCommand(0x3f, this._location));
                    flag = true;
                    goto Label_0364;

                case 0x19:
                    this.Redo();
                    flag = true;
                    goto Label_0364;

                case 0x1a:
                    this.Undo();
                    flag = true;
                    goto Label_0364;

                case 0x1b:
                    flag = true;
                    goto Label_0364;

                case 0x7f:
                    if (this._selectionExists)
                    {
                        this._buffer.BeginBatchUndo();
                        try
                        {
                            this.HandleCommand(new TextBufferCommand(0x3e, this._location));
                            this.HandleCommand(new TextBufferCommand(90));
                        }
                        finally
                        {
                            this._buffer.EndBatchUndo();
                        }
                    }
                    else
                    {
                        this.HandleCommand(new TextBufferCommand(90));
                    }
                    flag = true;
                    goto Label_0364;

                case 1:
                    this.HandleCommand(new TextBufferCommand(0x42));
                    flag = true;
                    goto Label_0364;

                case 3:
                    this.HandleCommand(new TextBufferCommand(0x40, this._location));
                    flag = true;
                    goto Label_0364;

                case 8:
                    if (!this._selectionExists)
                    {
                        int columnIndex = this._location.ColumnIndex;
                        int length = this._location.Line.Length;
                        if (columnIndex > 0)
                        {
                            this._location.ColumnIndex--;
                            this.HandleCommand(new TextBufferCommand(2, this._location, (char)keyCode));
                        }
                        else if (this._location.LineIndex > 0)
                        {
                            TextBufferLocation startLocation = this._buffer.CreateTextBufferLocation(this._location);
                            startLocation.MoveUp(1);
                            startLocation.ColumnIndex = startLocation.Line.Length;
                            this.HandleCommand(new TextBufferCommand(3, startLocation, this._location));
                            startLocation.Dispose();
                        }
                        this.ResetSelection();
                        break;
                    }
                    this.HandleCommand(new TextBufferCommand(0x3e, this._location));
                    break;

                case 9:
                    if ((Control.ModifierKeys & Keys.Shift) == Keys.None)
                    {
                        if (this._selectionExists && !this._selection.IsSingleLine)
                        {
                            this.HandleCommand(new TextBufferCommand(0x48, this._location, false));
                        }
                        else
                        {
                            if (this._selectionExists)
                            {
                                this._buffer.BeginBatchUndo();
                                try
                                {
                                    this.HandleCommand(new TextBufferCommand(0x3e, this._location));
                                    this.HandleCommand(new TextBufferCommand(70, this._location, false));
                                    goto Label_02F0;
                                }
                                finally
                                {
                                    this._buffer.EndBatchUndo();
                                }
                            }
                            this.HandleCommand(new TextBufferCommand(70, this._location, false));
                            this.ResetSelection();
                        }
                    }
                    else if (!this._selectionExists || this._selection.IsSingleLine)
                    {
                        this.HandleCommand(new TextBufferCommand(0x47, this._location, false));
                        this.ResetSelection();
                    }
                    else
                    {
                        this.HandleCommand(new TextBufferCommand(0x49, this._location, false));
                    }
                    goto Label_02F0;

                case 10:
                    this._location.ColumnIndex = 0;
                    this.HandleCommand(new TextBufferCommand(20, this._location));
                    this.ResetSelection();
                    flag = true;
                    goto Label_0364;

                case 13:
                    if (this._selectionExists)
                    {
                        this.HandleCommand(new TextBufferCommand(0x3e, this._location));
                    }
                    this.HandleCommand(new TextBufferCommand(20, this._location));
                    this.ResetSelection();
                    flag = true;
                    goto Label_0364;

                default:
                    goto Label_0364;
            }
            flag = true;
            goto Label_0364;
        Label_02F0:
            flag = true;
        Label_0364:
            if (!flag)
            {
                if (this._selectionExists)
                {
                    this.HandleCommand(new TextBufferCommand(7, this._location));
                    this.HandleCommand(new TextBufferCommand(0x3e, this._location));
                    this.HandleCommand(new TextBufferCommand(1, this._location, (char)keyCode));
                    this.HandleCommand(new TextBufferCommand(8, this._location));
                }
                else
                {
                    if (this.InsertMode)
                    {
                        this.HandleCommand(new TextBufferCommand(1, this._location, (char)keyCode));
                    }
                    else
                    {
                        this.HandleCommand(new TextBufferCommand(9, this._location, (char)keyCode));
                    }
                    this.ResetSelection();
                }
            }
            this.SetSmartCursorIndex();
        }

        protected void OnLocationChanged(object sender, EventArgs e)
        {
            if (this._location.LineIndex < this.ViewTopLineNumber)
            {
                this.ViewTopLineNumber = this._location.LineIndex;
            }
            else if (this._location.LineIndex > ((this.ViewTopLineNumber + this._visibleLines) - 1))
            {
                this.ViewTopLineNumber = (this._location.LineIndex - this._visibleLines) + 1;
            }
            int viewIndex = this.GetViewIndex(this._location);
            if (viewIndex < this.ViewLeftIndex)
            {
                this.ViewLeftIndex = Math.Max(0, viewIndex - this._horizontalScrollAmount);
            }
            else if ((this.MeasureString(this._location.Line.Data, this.ViewLeftIndex, viewIndex - this.ViewLeftIndex) + 2.0) > (base.Width - ((this.MarginPadding + this.MarginWidth) + this._lineNumbersWidth)))
            {
                int start = this.ViewLeftIndex + this._horizontalScrollAmount;
                double num2 = this.MeasureString(this._location.Line.Data, start, viewIndex - start);
                if ((num2 + 2.0) > (base.Width - ((this.MarginPadding + this.MarginWidth) + this._lineNumbersWidth)))
                {
                    start += (int)(((num2 + 2.0) - (base.Width - ((this.MarginPadding + this.MarginWidth) + this._lineNumbersWidth))) / ((double)(this._fontWidth * 2)));
                    for (num2 = this.MeasureString(this._location.Line.Data, start, viewIndex - start); (num2 + 2.0) > (base.Width - ((this.MarginPadding + this.MarginWidth) + this._lineNumbersWidth)); num2 = this.MeasureString(this._location.Line.Data, ++start, viewIndex - start))
                    {
                    }
                }
                this.ViewLeftIndex = start;
            }
            this.UpdateCaret();
            this._owner.OnViewLocationChanged(this);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this._owner.ActiveView != this)
            {
                this._owner.ActiveView = this;
            }
            this._owner.Focus();
            Point lineColFromXY = this.GetLineColFromXY(e.X, e.Y);
            if (e.Clicks == 2)
            {
                this._ignoreMouseUp = true;
                this._ignoreMouseMove = true;
                this.HandleCommand(new TextBufferCommand(0x2a, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None, lineColFromXY));
                this._firstHandler.HandleCommand(new TextBufferCommand(0x43));
            }
            else if (e.Clicks == 1)
            {
                if (this.IsInSelection(lineColFromXY.X, lineColFromXY.Y, e.X, e.Y))
                {
                    this._mouseDownInSelection = true;
                }
                else
                {
                    this.HandleCommand(new TextBufferCommand(0x2a, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None, lineColFromXY));
                    this._mouseDownInSelection = false;
                    this._ignoreMouseUp = true;
                }
            }
            this._mouseDown = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if ((this._mouseDown && (e.Button == MouseButtons.Left)) && !this._ignoreMouseMove)
            {
                this._ignoreMouseUp = true;
                if (this._mouseDownInSelection)
                {
                    this._owner.DraggingInternal = true;
                    base.DoDragDrop(this.TextLanguage.GetDataObjectFromText(this._selection.Text), DragDropEffects.Move | DragDropEffects.Copy);
                }
                else
                {
                    bool flag = false;
                    if (e.Y < 0)
                    {
                        this._verticalSelectionScrollAmount = (e.Y - 9) / 10;
                        flag = true;
                    }
                    else if (e.Y > (base.Height - this.HorizontalScrollBarHeight))
                    {
                        this._verticalSelectionScrollAmount = ((e.Y - (base.Height - this.HorizontalScrollBarHeight)) + 9) / 10;
                        flag = true;
                    }
                    if (e.X < 0)
                    {
                        this._horizontalSelectionScrollAmount = (e.X - 9) / 10;
                        flag = true;
                    }
                    else if (e.X > (base.Width - this.VerticalScrollBarWidth))
                    {
                        this._horizontalSelectionScrollAmount = ((e.X - (base.Width - this.VerticalScrollBarWidth)) + 9) / 10;
                        flag = true;
                    }
                    if (flag)
                    {
                        this.StartSelectionScrollTimer();
                    }
                    else
                    {
                        this.StopSelectionScrollTimer();
                        this.HandleCommand(new TextBufferCommand(0x2a, this._location, true, this.GetLineColFromXY(e.X, e.Y)));
                    }
                }
            }
            else
            {
                Point lineColFromXY = this.GetLineColFromXY(e.X, e.Y);
                if (this.IsInSelection(lineColFromXY.X, lineColFromXY.Y, e.X, e.Y))
                {
                    if (this.Cursor != Cursors.Arrow)
                    {
                        this.Cursor = Cursors.Arrow;
                    }
                }
                else if (this.Cursor != Cursors.IBeam)
                {
                    this.Cursor = Cursors.IBeam;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!this._ignoreMouseUp && (e.Button == MouseButtons.Left))
            {
                Point lineColFromXY = this.GetLineColFromXY(e.X, e.Y);
                this.HandleCommand(new TextBufferCommand(0x2a, this._location, (Control.ModifierKeys & Keys.Shift) != Keys.None, lineColFromXY));
            }
            else
            {
                this._ignoreMouseUp = false;
            }
            this._ignoreMouseMove = false;
            this._mouseDown = false;
            this.StopSelectionScrollTimer();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (this._paintFrozen == 0)
                {
                    Rectangle clipRectangle = e.ClipRectangle;
                    if (!this._owner.ContainsFocus)
                    {
                        clipRectangle = base.ClientRectangle;
                    }
                    IntPtr hdc = e.Graphics.GetHdc();
                    ColorInfo info = this.ColorTable[7];
                    info.SetupHdc(hdc);
                    Interop.RECT lpRect = new Interop.RECT(0, clipRectangle.Top, this.MarginWidth - 1, clipRectangle.Bottom);
                    Interop.ExtTextOutW(hdc, 0, 0, 2, ref lpRect, IntPtr.Zero, 0, null);
                    lpRect.left = this.MarginWidth - 1;
                    lpRect.right = this.MarginWidth;
                    this.ColorTable[8].SetupHdc(hdc);
                    Interop.ExtTextOutW(hdc, 0, 0, 2, ref lpRect, IntPtr.Zero, 0, null);
                    ColorInfo info3 = this.ColorTable[4];
                    info3.SetupHdc(hdc);
                    lpRect.left = this.MarginWidth + this._lineNumbersWidth;
                    lpRect.right = lpRect.left + this.MarginPadding;
                    Interop.ExtTextOutW(hdc, 0, 0, 2, ref lpRect, IntPtr.Zero, 0, null);
                    IntPtr hObject = Interop.SelectObject(hdc, this._fontPtr);
                    Interop.SetBkMode(hdc, 2);
                    int num = clipRectangle.Top / this._fontHeight;
                    int lineIndex = num + this.ViewTopLineNumber;
                    int num3 = (clipRectangle.Height + (this._fontHeight - 1)) / this._fontHeight;
                    TextLine next = null;
                    using (TextBufferLocation location = this._buffer.CreateTextBufferLocation(this._location))
                    {
                        location.GotoLine(lineIndex);
                        next = location.Line;
                        int num4 = 0;
                        int yPos = num * this._fontHeight;
                        for (int i = lineIndex; (num4 < num3) && (next != null); i++)
                        {
                            if (next.AttributesDirty)
                            {
                                location.GotoLine(i);
                                this.ColorizeLine(location);
                            }
                            this.PaintTextLine(next, i, hdc, yPos, this.ViewLeftIndex);
                            next = next.Next;
                            num4++;
                            yPos += this._fontHeight;
                        }
                    }
                    info3.SetupHdc(hdc);
                    lpRect.top = Math.Min(this.VisibleLines, this._buffer.LineCount - this.ViewTopLineNumber) * this._fontHeight;
                    lpRect.left = this.MarginWidth;
                    lpRect.bottom = base.Height - this.HorizontalScrollBarHeight;
                    lpRect.right = base.Width - this.VerticalScrollBarWidth;
                    Interop.ExtTextOutW(hdc, 0, 0, 2, ref lpRect, IntPtr.Zero, 0, null);
                    if (this.LineNumbersVisible)
                    {
                        info.SetupHdc(hdc);
                        int num7 = (this.MarginWidth + this._lineNumbersWidth) - 2;
                        lpRect.top = clipRectangle.Top;
                        lpRect.left = num7;
                        lpRect.right = num7 + 1;
                        Interop.ExtTextOutW(hdc, 0, 0, 2, ref lpRect, IntPtr.Zero, 0, null);
                    }
                    info.SetupHdc(hdc);
                    int height = base.Height;
                    int width = base.Width;
                    lpRect.top = height - this.HorizontalScrollBarHeight;
                    lpRect.left = width - this.VerticalScrollBarWidth;
                    lpRect.right = width;
                    lpRect.bottom = height;
                    if (this.ResizeGripVisible)
                    {
                        Interop.DrawFrameControl(hdc, ref lpRect, 3, 8);
                    }
                    else
                    {
                        Interop.ExtTextOutW(hdc, 0, 0, 2, ref lpRect, IntPtr.Zero, 0, null);
                    }
                    Interop.SelectObject(hdc, hObject);
                    e.Graphics.ReleaseHdc(hdc);
                    if (this._updateCaretOnPaint)
                    {
                        this.UpdateCaret();
                        this._updateCaretOnPaint = false;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        internal void OnParentMouseWheel(int delta)
        {
            if (delta > 0)
            {
                if (this.ViewTopLineNumber < 3)
                {
                    this.ViewTopLineNumber = 0;
                }
                else
                {
                    this.ViewTopLineNumber -= 3;
                }
            }
            else
            {
                int num = this.ViewTopLineNumber + 3;
                if (num > (this._buffer.LineCount - this.VisibleLines))
                {
                    this.ViewTopLineNumber = Math.Max(this.ViewTopLineNumber, this._buffer.LineCount - this.VisibleLines);
                }
                else
                {
                    this.ViewTopLineNumber += 3;
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.UpdateVisibleLinesCols();
            this.UpdateScrollBarVisibility();
            base.Invalidate();
        }

        private void OnSelectionScroll(object source, EventArgs e)
        {
            if (this._verticalSelectionScrollAmount > 0)
            {
                this._location.MoveDown(this._verticalSelectionScrollAmount);
            }
            else if (this._verticalSelectionScrollAmount < 0)
            {
                this._location.MoveUp(-this._verticalSelectionScrollAmount);
            }
            if (this._horizontalSelectionScrollAmount > 0)
            {
                this._location.ColumnIndex = Math.Min(this._location.ColumnIndex + this._horizontalSelectionScrollAmount, this._location.Line.Length);
            }
            else if (this._horizontalSelectionScrollAmount < 0)
            {
                this._location.ColumnIndex = Math.Max(this._location.ColumnIndex + this._horizontalSelectionScrollAmount, 0);
            }
            this.UpdateCaret();
            this.UpdateSelection();
        }

        protected void OnTextBufferChanged(object sender, TextBufferEventArgs e)
        {
            if (this._trimmingWhitespace)
            {
                return;
            }

            this._verticalScrollBar.Maximum = this._buffer.LineCount - 1;
            using (TextBufferLocation location = this._buffer.CreateTextBufferLocation(this._location))
            {
                location.GotoLine(e.StartLineNumber);
                while (location.LineIndex <= e.EndLineNumber)
                {
                    int viewIndex = this.GetViewIndex(location.Line, location.Line.Length);
                    if (viewIndex > this._viewMaxLineLength)
                    {
                        this._viewMaxLineLength = viewIndex;
                    }

                    double realWidth = this.MeasureString(location.Line.Data, 0, location.Line.Length);
                    if (realWidth >= this._viewMaxLineWidth)
                    {
                        this._viewMaxLineWidth = realWidth + 0.1;
                        this._viewMaxLineWidthLineIndex = location.LineIndex;
                    }

                    if (location.MoveDown(1) == 0)
                    {
                        goto Label_008A;
                    }
                }
            }

        Label_008A:
            if (this._fontWidth != 0)
            {
                this._horizontalScrollBar.Maximum = (int)((this._viewMaxLineWidth / ((double)this._fontWidth)) + 0.5);
            }
            else
            {
                this._horizontalScrollBar.Maximum = (int)this._viewMaxLineWidth;
            }

            this.UpdateScrollBarVisibility();
            if ((this.ViewTopLineNumber + this.VisibleLines) > (this._buffer.LineCount - 1))
            {
                this.ViewTopLineNumber = Math.Max(0, this._buffer.LineCount - this.VisibleLines);
            }
            if (this._selectionExists)
            {
                int num2 = e.EndIndex - e.OldEndIndex;
                if ((this._selection.Start.LineIndex == e.OldEndLineNumber) && (this._selection.Start.ColumnIndex > e.OldEndIndex))
                {
                    TextBufferLocation start = this._selection.Start;
                    start.ColumnIndex += num2;
                }
                if ((this._selection.End.LineIndex == e.OldEndLineNumber) && (this._selection.End.ColumnIndex >= e.OldEndIndex))
                {
                    TextBufferLocation end = this._selection.End;
                    end.ColumnIndex += num2;
                }
            }
            if (e.OldEndLineNumber == e.EndLineNumber)
            {
                this.AddDirtyLines(e.StartLineNumber, Math.Max(e.OldEndLineNumber, e.EndLineNumber) + 1);
            }
            else
            {
                this.AddDirtyLines(e.StartLineNumber, this.ViewTopLineNumber + this.VisibleLines);
            }
        }

        protected void OnVerticalScrollBarScroll(object sender, ScrollEventArgs e)
        {
            this.ViewTopLineNumber = Math.Min(e.NewValue, Math.Max(0, this._buffer.LineCount - this._visibleLines));
        }

        private unsafe void PaintColoredText(char[] chars, byte[] colors, IntPtr hdc, int rectLeft, int rectRight, int yPos, int startIndex, int endIndex, bool selected)
        {
            if ((chars == null) || (chars.Length <= 0))
            {
                this.ColorTable[0].SetupHdc(hdc);
                Interop.RECT lpRect = new Interop.RECT(rectLeft, yPos, rectRight, yPos + this._fontHeight);
                Interop.ExtTextOutW(hdc, 0, 0, 2, ref lpRect, IntPtr.Zero, 0, null);
            }
            else
            {
                // 清空行末空间颜色
                if ((endIndex - startIndex) >= 0)
                {
                    Interop.RECT rect = new Interop.RECT(rectLeft + ((endIndex - startIndex) * this._fontWidth), yPos, rectRight, yPos + this._fontHeight);
                    this.ColorTable[colors[chars.Length]].SetupHdc(hdc);
                    Interop.ExtTextOutW(hdc, 0, 0, 2, ref rect, IntPtr.Zero, 0, null);
                }

                // 开始把字符串画到控件上
                Interop.RECT textRect = new Interop.RECT(rectLeft, yPos, rectLeft, yPos + this._fontHeight);
                int startColumnIndex = startIndex;
                int curColumnIndex = startColumnIndex;
                int count = 0;

                while (startColumnIndex < endIndex)
                {
                    int colorIndex = colors[startColumnIndex];
                    while ((curColumnIndex < endIndex) && (colors[curColumnIndex] == colorIndex))
                    {
                        curColumnIndex++;
                    }
                    IntPtr zero = IntPtr.Zero;
                    try
                    {
                        ColorInfo info2 = this.ColorTable[colorIndex];
                        ColorInfo info3 = info2;
                        if (selected)
                        {
                            if (this._owner.ContainsFocus && (this._owner.ActiveView == this))
                            {
                                info2 = this.ColorTable[5];
                            }
                            else
                            {
                                info2 = this.ColorTable[6];
                            }
                        }
                        if (info3.IsStyledFont)
                        {
                            zero = info2.SetupHdc(hdc, this.GetStyledFont(info3.Bold, info3.Italic));
                        }
                        else
                        {
                            info2.SetupHdc(hdc);
                        }

                        count = curColumnIndex - startColumnIndex;
                        string text = new string(chars, startColumnIndex, count);
                        Interop.SIZE size = new Interop.SIZE();
                        bool ret = Interop.GetTextExtentPoint32W(hdc, text, count, ref size);
                        textRect.left = textRect.right;
                        textRect.right += size.x;

                        fixed (char* chRef = chars)
                        {
                            char* chPtr = chRef;
                            chPtr += startColumnIndex;
                            Interop.ExtTextOutW(hdc, textRect.left, yPos, 2, ref textRect, (IntPtr)chPtr, count, null);
                        }
                        continue;
                    }
                    finally
                    {
                        startColumnIndex = curColumnIndex;
                        if (zero != IntPtr.Zero)
                        {
                            Interop.SelectObject(hdc, zero);
                        }
                    }
                }
            }
        }

        private void PaintDirtyLines()
        {
            int num = this._topPaintLineNumber;
            int y = this._bottomPaintLineNumber;
            using (TextBufferLocation location = this._buffer.CreateTextBufferLocation())
            {
                for (int i = num; i <= y; i++)
                {
                    location.GotoLine(i);
                    if (location.Line.AttributesDirty)
                    {
                        Point point = this.ColorizeLine(location);
                        num = Math.Min(point.X, num);
                        if (point.Y > y)
                        {
                            y = point.Y;
                            i = point.Y;
                        }
                    }
                }
            }
            num = (Math.Max(this.ViewTopLineNumber, num) - this.ViewTopLineNumber) * this._fontHeight;
            y = (Math.Min(this.ViewTopLineNumber + this.VisibleLines, y) - this.ViewTopLineNumber) * this._fontHeight;
            this._topPaintLineNumber = 0x7fffffff;
            this._bottomPaintLineNumber = -1;
            base.Invalidate(new Rectangle(0, num, base.Width - this.VerticalScrollBarWidth, y - num));
        }

        private double MeasureString(char[] chars)
        {
            return this.MeasureString(chars, 0, chars.Length);
        }

        private double MeasureString(char[] chars, int start, int length)
        {
            int end = start + length;
            //if (end > chars.Length)
            //    return 0.0;

            double realWidth;
            double width = 0.0;
            if (chars == null)
            {
                return (int)width;
            }
            char[] chArray = chars;

            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr zero = IntPtr.Zero;
            try
            {
                zero = graphics.GetHdc();
                Interop.SelectObject(zero, this._fontPtr);
                for (int i = start; i < end; i++)
                {
                    Interop.SIZE size = new Interop.SIZE();
                    Interop.GetTextExtentPoint32W(zero, new string(chArray[i], 1), 1, ref size);
                    width += size.x;
                }
                realWidth = width;
            }
            catch (Exception exception)
            {
                StackFrame frame = new StackFrame(1);
                StreamWriter writer = new StreamWriter(@"c:\log.txt");
                writer.Write(frame.GetMethod().Name);
                writer.WriteLine("---" + exception.ToString());
                writer.Close();
                realWidth = width;
            }
            finally
            {
                graphics.ReleaseHdc(zero);
            }

            return realWidth;
        }


        //private Interop.SIZE MeasureString(IntPtr hdc, char[] chars, int startIndex, int length)
        //{
        //    Interop.SIZE size = new Interop.SIZE();
        //    if (length > 0)
        //    { 
        //        string text = new string(chars, startIndex, length);
        //        bool ret = Interop.GetTextExtentPoint32W(hdc, text, length, ref size);
        //    }
        //    return size;
        //}

        private unsafe void PaintTextLine(TextLine line, int lineNum, IntPtr hdc, int yPos, int startIndex)
        {
            try
            {
                if (this.LineNumbersVisible)
                {
                    this.ColorTable[4].SetupHdc(hdc);
                    string str = (lineNum + 1).ToString();
                    int length = str.Length;
                    int num2 = (5 - length) * this._fontWidth;
                    Interop.RECT lpRect = new Interop.RECT(this.MarginWidth, yPos, this.MarginWidth + this._lineNumbersWidth, yPos + this._fontHeight);
                    fixed (char* chRef = str.ToCharArray())
                    {
                        Interop.ExtTextOutW(hdc, this.MarginWidth + num2, yPos, 2, ref lpRect, (IntPtr)chRef, length, null);
                    }
                }
                char[] chars = null;
                byte[] colors = null;
                if (line.Length > 0)
                {
                    int num3 = 0;
                    char[] data = line.Data;
                    byte[] attributes = line.Attributes;
                    int capacity = line.Length * 2;
                    StringBuilder builder = new StringBuilder(capacity);
                    MemoryStream stream = new MemoryStream(capacity);
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (data[i] == '\t')
                        {
                            int num6 = this.TabSize - (num3 % this.TabSize);
                            for (int j = 0; j < num6; j++)
                            {
                                if (this._owner.ShowWhitespace)
                                {
                                    builder.Append('→');
                                }
                                else
                                {
                                    builder.Append(' ');
                                }
                                stream.WriteByte(attributes[i]);
                            }
                            num3 += num6;
                        }
                        else
                        {
                            if (this._owner.ShowWhitespace && (data[i] == ' '))
                            {
                                builder.Append('\x00b7');
                            }
                            else
                            {
                                builder.Append(data[i]);
                            }
                            stream.WriteByte(attributes[i]);
                            num3++;
                        }
                    }
                    stream.WriteByte(attributes[data.Length]);
                    chars = builder.ToString().ToCharArray();
                    colors = stream.ToArray();
                }
                else
                {
                    colors = new byte[0];
                    chars = new char[0];
                }
                if (this._selectionExists)
                {
                    if (this._selection.IsSingleLine)
                    {
                        if (lineNum == this._selection.Start.LineIndex)
                        {
                            int endIndex = Math.Min(Math.Max(this.GetViewIndex(line, this._selection.Start.ColumnIndex), startIndex), chars.Length);
                            int minLenght = Math.Min(Math.Max(startIndex, this.GetViewIndex(line, this._selection.End.ColumnIndex)), chars.Length);
                            int rectLeft = (this.MarginWidth + this._lineNumbersWidth) + this.MarginPadding;
                            //int textWidth = Math.Max(0, endIndex - startIndex) * this._fontWidth;
                            int textWidth = (int)this.MeasureString(chars, startIndex, endIndex - startIndex);
                            this.PaintColoredText(chars, colors, hdc, rectLeft, rectLeft + textWidth, yPos, startIndex, endIndex, false);

                            rectLeft += textWidth;
                            //textWidth = Math.Max(0, minLenght - endIndex) * this._fontWidth;
                            textWidth = (int)this.MeasureString(chars, endIndex, minLenght - endIndex);
                            this.PaintColoredText(chars, colors, hdc, rectLeft, rectLeft + textWidth, yPos, endIndex, minLenght, true);

                            rectLeft += textWidth;
                            this.PaintColoredText(chars, colors, hdc, rectLeft, base.Width - this.VerticalScrollBarWidth, yPos, minLenght, chars.Length, false);
                            return;
                        }
                    }
                    else
                    {
                        // �已选择文本中的第一?
                        if (lineNum == this._selection.Start.LineIndex)
                        {
                            int endIndex = Math.Min(Math.Max(this.GetViewIndex(line, this._selection.Start.ColumnIndex), startIndex), chars.Length);
                            int rectLeft = (this.MarginWidth + this._lineNumbersWidth) + this.MarginPadding;

                            // �先画出行前未选中的文?
                            int textWidth = (int)this.MeasureString(chars, startIndex, endIndex - startIndex);
                            //int textWidth = Math.Max(0, endIndex - startIndex) * this._fontWidth;
                            this.PaintColoredText(chars, colors, hdc, rectLeft, rectLeft + textWidth, yPos, startIndex, endIndex, false);
                            rectLeft += textWidth;

                            // �然后画出选中的文?
                            textWidth = (int)this.MeasureString(chars, endIndex, chars.Length - endIndex);
                            if (startIndex <= chars.Length)
                            {
                                textWidth += this._fontWidth;
                            }
                            this.PaintColoredText(chars, colors, hdc, rectLeft, rectLeft + textWidth, yPos, endIndex, chars.Length, true);
                            rectLeft += textWidth;
                            this.PaintColoredText(chars, colors, hdc, rectLeft, base.Width - this.VerticalScrollBarWidth, yPos, 0, 0, false);
                            return;
                        }

                        // �已选择文本中的最后一?
                        if (lineNum == this._selection.End.LineIndex)
                        {
                            int endIndex = Math.Min(Math.Max(this.GetViewIndex(line, this._selection.End.ColumnIndex), startIndex), chars.Length);
                            int rectLeft = (this.MarginWidth + this._lineNumbersWidth) + this.MarginPadding;
                            //int num20 = Math.Max(0, num17 - startIndex) * this._fontWidth;
                            int textWidth = (int)this.MeasureString(chars, startIndex, endIndex - startIndex);
                            this.PaintColoredText(chars, colors, hdc, rectLeft, rectLeft + textWidth, yPos, startIndex, endIndex, true);
                            rectLeft += textWidth;
                            this.PaintColoredText(chars, colors, hdc, rectLeft, base.Width - this.VerticalScrollBarWidth, yPos, endIndex, chars.Length, false);
                            return;
                        }

                        // �选中文本的中间行
                        if ((lineNum > this._selection.Start.LineIndex) && (lineNum < this._selection.End.LineIndex))
                        {
                            int rectLeft = (this.MarginWidth + this._lineNumbersWidth) + this.MarginPadding;
                            int textWidth = (int)this.MeasureString(chars, startIndex, chars.Length - startIndex);
                            if (startIndex <= chars.Length)
                            {
                                textWidth += this._fontWidth;
                            }
                            this.PaintColoredText(chars, colors, hdc, rectLeft, rectLeft + textWidth, yPos, startIndex, chars.Length, true);
                            rectLeft += textWidth;
                            this.PaintColoredText(chars, colors, hdc, rectLeft, base.Width - this.VerticalScrollBarWidth, yPos, 0, 0, false);
                            return;
                        }
                    }
                }
                startIndex = Math.Min(startIndex, chars.Length);
                this.PaintColoredText(chars, colors, hdc, (this.MarginWidth + this._lineNumbersWidth) + this.MarginPadding, base.Width - this.VerticalScrollBarWidth, yPos, startIndex, chars.Length, false);

            }
            catch (Exception)
            {
                throw;
            }
        }

        internal void Paste()
        {
            this.HandleCommand(new TextBufferCommand(0x41));
        }

        internal void Paste(IDataObject dataObj)
        {
            string textFromDataObject = this.TextLanguage.GetTextFromDataObject(dataObj, this.ServiceProvider);
            this.HandleCommand(new TextBufferCommand(4, this._location, textFromDataObject));
        }

        private void PasteInternal()
        {
            this._buffer.BeginBatchUndo();
            try
            {
                DataObject dataObject = (DataObject)Clipboard.GetDataObject();
                string textFromDataObject = this.TextLanguage.GetTextFromDataObject(dataObject, this.ServiceProvider);
                if (textFromDataObject == null)
                {
                    textFromDataObject = (string)dataObject.GetData(DataFormats.Text);
                }
                if (textFromDataObject != null)
                {
                    if (this._selectionExists)
                    {
                        this.DeleteSelection();
                    }
                    this.HandleCommand(new TextBufferCommand(4, this._location, textFromDataObject));
                }
            }
            finally
            {
                this._buffer.EndBatchUndo();
            }
            this.ResetSelection();
        }

        public override bool PreProcessMessage(ref Message msg)
        {
            switch (msg.Msg)
            {
                case 0x100:
                case 0x102:
                    if (this.ProcessCmdKey(ref msg, ((Keys)((int)msg.WParam)) | Control.ModifierKeys))
                    {
                        break;
                    }
                    return false;
            }
            return base.PreProcessMessage(ref msg);
        }

        internal void Redo()
        {
            this.ResetSelection();
            this.HandleCommand(new TextBufferCommand(6, this._location));
        }

        public void RemoveCommandHandler(ICommandHandler handler)
        {
            this._firstHandler = handler;
        }

        internal void RepaintSelection()
        {
            if (this._selectionExists)
            {
                this.AddDirtyLines(this._selection.Start.LineIndex, this._selection.End.LineIndex + 1);
            }
        }

        private bool Replace(string searchString, string replaceString, bool matchCase, bool wholeWord, bool searchUp)
        {
            TextBufferSpan span = null;
            bool flag;
            try
            {
                if (this._selectionExists)
                {
                    span = this.Find(searchString, matchCase, wholeWord, searchUp, true);
                    if (span != null)
                    {
                        this.HandleCommand(new TextBufferCommand(7));
                        this.HandleCommand(new TextBufferCommand(3, span.Start, span.End));
                        this.HandleCommand(new TextBufferCommand(4, span.Start, replaceString));
                        this.HandleCommand(new TextBufferCommand(8));
                        span.Dispose();
                    }
                    span = this.Find(searchString, matchCase, wholeWord, searchUp, false);
                    if (span != null)
                    {
                        this.Select(span);
                    }
                    else
                    {
                        this.ResetSelection();
                    }
                    return (span != null);
                }
                span = this.Find(searchString, matchCase, wholeWord, searchUp, false);
                if (span != null)
                {
                    this.Select(span);
                }
                flag = span != null;
            }
            finally
            {
                if (span != null)
                {
                    span.Dispose();
                }
            }
            return flag;
        }

        public bool Replace(string searchString, string replaceString, bool matchCase, bool wholeWord, bool searchUp, bool replaceAll, bool inSelection)
        {
            if (!replaceAll)
            {
                return this.Replace(searchString, replaceString, matchCase, wholeWord, searchUp);
            }
            this.HandleCommand(new TextBufferCommand(7, this._location));
            TextBufferLocation startLocation = null;
            TextBufferLocation endLocation = null;
            TextBufferSpan span = null;
            try
            {
                if (inSelection)
                {
                    if (!this._selectionExists)
                    {
                        return false;
                    }
                    startLocation = this._selection.Start.Clone();
                    endLocation = this._selection.End.Clone();
                }
                else
                {
                    this.ResetSelection();
                    startLocation = this._buffer.First.Clone();
                    endLocation = this._buffer.Last.Clone();
                }
                span = this._buffer.Find(startLocation, endLocation, searchString, matchCase, wholeWord, false);
                while (span != null)
                {
                    this.HandleCommand(new TextBufferCommand(3, span.Start, span.End));
                    this.HandleCommand(new TextBufferCommand(4, span.Start, replaceString));
                    startLocation.MoveTo(span.Start);
                    span.Dispose();
                    span = null;
                    span = this._buffer.Find(startLocation, endLocation, searchString, matchCase, wholeWord, false);
                }
            }
            finally
            {
                if (startLocation != null)
                {
                    startLocation.Dispose();
                }
                if (endLocation != null)
                {
                    endLocation.Dispose();
                }
                if (span != null)
                {
                    span.Dispose();
                }
                this.HandleCommand(new TextBufferCommand(8, this._location));
            }
            return true;
        }

        internal void ResetSelection()
        {
            if (this._selectionExists)
            {
                this.AddDirtyLines(this._selection.Start.LineIndex, this._selection.End.LineIndex + 1);
                this._selectionExists = false;
                this._selection = null;
                this._owner.OnViewSelectionChanged(this);
            }
            this._selectionStart.MoveTo(this._location);
            this._selectionEnd.MoveTo(this._location);
        }

        internal void Select(TextBufferSpan span)
        {
            this._location.MoveTo(span.Start);
            this.ResetSelection();
            this._location.MoveTo(span.End);
            this.UpdateSelection();
            this.SetSmartCursorIndex();
        }

        internal void SelectAll()
        {
            this.HandleCommand(new TextBufferCommand(0x42));
        }

        public void SetHintHighlightText(string highlightText)
        {
            if (this._autoCompleteHint != null)
            {
                this._autoCompleteHint.SetHighlightText(highlightText);
            }
        }

        private void SetSmartCursorIndex()
        {
            this._smartCursorIndex = this._location.ColumnIndex;
        }

        public void ShowAutoComplete(ITextAutoCompletionList list)
        {
            if (this._autoCompleteForm == null)
            {
                this._autoCompleteForm = new AutoCompleteForm(this, this._owner, this._location, list);
                int x = (((this._fontWidth * (this.GetViewIndex(this._location) - this.ViewLeftIndex)) + this.MarginPadding) + this.MarginWidth) + this._lineNumbersWidth;
                int y = ((this._location.LineIndex + 1) - this.ViewTopLineNumber) * this._fontHeight;
                this._autoCompleteForm.TopLevel = false;
                this._autoCompleteForm.Parent = this;
                if ((this._autoCompleteForm.Height + y) > base.Height)
                {
                    y = (y - this._autoCompleteForm.Height) - this._fontHeight;
                    if (y < 0)
                    {
                        this._autoCompleteForm = null;
                        return;
                    }
                }
                if ((this._autoCompleteForm.Width + x) > base.Width)
                {
                    x -= this._autoCompleteForm.Width;
                    if (x < 0)
                    {
                        this._autoCompleteForm = null;
                        return;
                    }
                }
                this._autoCompleteForm.Location = new Point(x, y);
                this._autoCompleteForm.Closed += new EventHandler(this.OnAutoCompleteFormClosed);
                this._autoCompleteForm.Show();
                this._owner.Focus();
            }
            this.AddCommandHandler(this._autoCompleteForm);
        }

        internal void ShowCaret()
        {
            this.ShowCaret(true);
        }

        internal void ShowCaret(bool checkForFocus)
        {
            if ((!checkForFocus || ((this._owner.ActiveView == this) && this._owner.ContainsFocus)) && this._caretHiding)
            {
                Interop.CreateCaret(base.Handle, IntPtr.Zero, this.CaretWidth, this._fontHeight);
                Interop.ShowCaret(base.Handle);
                this.UpdateCaretPosition();
                this._caretHiding = false;
            }
        }

        public void ShowHint(TextBufferLocation hintLocation, string text, string highlightText)
        {
            if (this._autoCompleteHint == null)
            {
                this._autoCompleteHint = new AutoCompleteLabel(text, highlightText);
                int x = (((this._fontWidth * (this.GetViewIndex(hintLocation) - this.ViewLeftIndex)) + this.MarginPadding) + this.MarginWidth) + this._lineNumbersWidth;
                int y = ((hintLocation.LineIndex + 1) - this.ViewTopLineNumber) * this._fontHeight;
                if ((this._autoCompleteHint.Height + y) > base.Height)
                {
                    y = (y - this._autoCompleteHint.Height) - this._fontHeight;
                    if (y < 0)
                    {
                        this.HideHint();
                        return;
                    }
                }
                if ((this._autoCompleteHint.Width + x) > base.Width)
                {
                    x -= this._autoCompleteHint.Width;
                    if (x < 0)
                    {
                        this.HideHint();
                        return;
                    }
                }
                this._autoCompleteHint.Location = new Point(x, y);
                base.Controls.Add(this._autoCompleteHint);
                this._autoCompleteHint.Visible = true;
            }
            else if (text == this._autoCompleteHint.Text)
            {
                this.SetHintHighlightText(highlightText);
            }
        }

        private void StartSelectionScrollTimer()
        {
            if (this._selectionScrollTimer == null)
            {
                this._selectionScrollTimer = new Timer();
                this._selectionScrollTimer.Tick += new EventHandler(this.OnSelectionScroll);
                this._selectionScrollTimer.Start();
            }
        }

        private void StopSelectionScrollTimer()
        {
            if (this._selectionScrollTimer != null)
            {
                this._verticalSelectionScrollAmount = 0;
                this._horizontalSelectionScrollAmount = 0;
                this._selectionScrollTimer.Stop();
                this._selectionScrollTimer.Tick -= new EventHandler(this.OnSelectionScroll);
                this._selectionScrollTimer = null;
            }
        }

        internal void TrimTrailingWhitespace()
        {
            using (TextBufferLocation location = this._buffer.First.Clone())
            {
                int num = 1;
                this._buffer.BeginBatchUndo();
                try
                {
                    while (num > 0)
                    {
                        if (location.LineIndex != this.LineIndex)
                        {
                            this.TrimTrailingWhitespace(location);
                        }
                        num = location.MoveDown(1);
                    }
                }
                finally
                {
                    this._buffer.EndBatchUndo();
                }
            }
        }

        private void TrimTrailingWhitespace(TextBufferLocation location)
        {
            this._trimmingWhitespace = true;
            try
            {
                TextLine line = location.Line;
                int length = line.Length;
                int num2 = length;
                while ((num2 > 0) && char.IsWhiteSpace(line[num2 - 1]))
                {
                    num2--;
                }
                if (length != num2)
                {
                    using (TextBufferLocation location2 = location.Clone())
                    {
                        location2.ColumnIndex = num2;
                        location.ColumnIndex = length;
                        this._buffer.DeleteText(new TextBufferSpan(location2, location));
                    }
                }
            }
            finally
            {
                this._trimmingWhitespace = false;
            }
        }

        internal void Undo()
        {
            this.ResetSelection();
            this.HandleCommand(new TextBufferCommand(5, this._location));
        }

        public void UnfreezePaint()
        {
            this._paintFrozen--;
            if (this._paintFrozen == 0)
            {
                this.PaintDirtyLines();
            }
        }

        private void UpdateCaret()
        {
            this.UpdateCaretVisibility();
            this.UpdateCaretPosition();
        }

        private void UpdateCaretPosition()
        {
            this.UpdateCaretPosition(true);
        }

        private void UpdateCaretPosition(bool checkForFocus)
        {
            if (((this.Focused || base.Parent.Focused) || !checkForFocus) && (this._paintFrozen == 0))
            {
                if (this._location.ColumnIndex > this._location.Line.Length)
                {
                    this._location.ColumnIndex = this._location.Line.Length;
                }

                TextLine line = _location.Line;
                int x = (int)this.MeasureString(line.ToCharArray(), this.ViewLeftIndex, (this.GetViewIndex(this._location) - this.ViewLeftIndex)) + this.MarginPadding + this.MarginWidth + this._lineNumbersWidth;

                int y = (this._location.LineIndex - this.ViewTopLineNumber) * this._fontHeight;
                Interop.SetCaretPos(x, y);
            }
            else
            {
                this._updateCaretOnPaint = true;
            }
        }

        private void UpdateCaretVisibility()
        {
            if (this._location.ColumnIndex > this._location.Line.Length)
            {
                this._location.ColumnIndex = this._location.Line.Length;
            }
            int viewIndex = this.GetViewIndex(this._location);
            if (((viewIndex < this._viewLeftIndex) || (viewIndex > (this._viewLeftIndex + this.VisibleColumns))) || ((this._location.LineIndex < this._viewTopLineNumber) || (this._location.LineIndex > (this._viewTopLineNumber + this.VisibleLines))))
            {
                this.HideCaret();
            }
            else if (this._owner.ActiveView == this)
            {
                this.ShowCaret();
            }
        }

        internal void UpdateFont()
        {
            this._fontPtr = this.Font.ToHfont();
            this._styledFontPtrs = null;
            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr hdc = graphics.GetHdc();
            Interop.SelectObject(hdc, this._fontPtr);
            Interop.SIZE size = new Interop.SIZE();
            Interop.GetTextExtentPoint32W(hdc, "O", 1, ref size);
            this._fontWidth = size.x;
            this._fontHeight = size.y;
            graphics.ReleaseHdc(hdc);
            if (this.LineNumbersVisible)
            {
                this._lineNumbersWidth = (this._fontWidth * 5) + 3;
            }
            this.UpdateLayout();
            if (base.IsHandleCreated)
            {
                bool flag = !this._caretHiding;
                this.HideCaret();
                if (flag)
                {
                    Interop.DestroyCaret();
                    this.UpdateCaret();
                }
            }
            base.Invalidate();
        }

        internal void UpdateLayout()
        {
            if (base.IsHandleCreated)
            {
                if (this.LineNumbersVisible)
                {
                    this._lineNumbersWidth = (this._fontWidth * 5) + 3;
                }
                else
                {
                    this._lineNumbersWidth = 0;
                }
                this.UpdateVisibleLinesCols();
                this.UpdateScrollBarVisibility();
                this.UpdateCaret();
                base.Invalidate();
            }
            else
            {
                this._updateLayoutRequired = true;
            }
        }

        private void UpdateScrollBars()
        {
            this._verticalScrollBar.Value = this.ViewTopLineNumber;
            this._horizontalScrollBar.Value = this.ViewLeftIndex;
        }

        private void UpdateScrollBarVisibility()
        {
            this.UpdateScrollBarVisibility(false);
        }


        private void UpdateScrollBarVisibility(bool recalc)
        {
            if (recalc)
            {
                using (TextBufferLocation location = this._buffer.CreateTextBufferLocation(this._location))
                {
                    location.GotoLine(0);
                    while (location.LineIndex <= this._buffer.LineCount)
                    {
                        double realWidth = this.MeasureString(location.Line.Data, 0, location.Line.Length);
                        if (realWidth > this._viewMaxLineWidth)
                        {
                            this._viewMaxLineWidth = realWidth;
                            this._viewMaxLineWidthLineIndex = location.LineIndex;
                        }
                        if (location.MoveDown(1) == 0)
                        {
                            goto Label_007E;
                        }
                    }
                }
            }

        Label_007E:
            if ((((this._viewMaxLineWidth + this.MarginPadding) + this._lineNumbersWidth) + this.MarginWidth) < base.Width)
            {
                this._horizontalScrollBar.Visible = false;
                this.ViewLeftIndex = 0;
            }
            else
            {
                this._horizontalScrollBar.Visible = true;
            }
            if (this._buffer.LineCount < this.VisibleLines)
            {
                this._verticalScrollBar.Visible = false;
            }
            else
            {
                this._verticalScrollBar.Visible = true;
            }
            this._verticalScrollBar.Location = new Point(base.Width - this._verticalScrollBar.Width, 0);
            this._verticalScrollBar.Height = base.Height - this.HorizontalScrollBarHeight;
            this._horizontalScrollBar.Location = new Point(0, base.Height - this._horizontalScrollBar.Height);
            this._horizontalScrollBar.Width = base.Width - this.VerticalScrollBarWidth;
            this.UpdateVisibleLinesCols();
        }

        private void UpdateSelection()
        {
            int num = -1;
            int lineIndex = -1;
            if (this._selectionExists)
            {
                lineIndex = this._selection.Start.LineIndex;
                num = this._selection.End.LineIndex;
            }
            this._selectionEnd.MoveTo(this._location);
            this._selection = new TextBufferSpan(this._selectionStart, this._selectionEnd);
            if (this._selection.IsEmpty)
            {
                this.ResetSelection();
            }
            else
            {
                if (this._selectionExists)
                {
                    int startLineIndex = this._selection.Start.LineIndex;
                    int endLineIndex = this._selection.End.LineIndex;
                    if ((lineIndex != startLineIndex) && (num != endLineIndex))
                    {
                        this.AddDirtyLines(Math.Min(startLineIndex, lineIndex), Math.Max(endLineIndex, num) + 1);
                    }
                    else if (lineIndex < startLineIndex)
                    {
                        this.AddDirtyLines(lineIndex, startLineIndex + 1);
                    }
                    else if (lineIndex > startLineIndex)
                    {
                        this.AddDirtyLines(startLineIndex, lineIndex + 1);
                    }
                    else if (num < endLineIndex)
                    {
                        this.AddDirtyLines(num, endLineIndex + 1);
                    }
                    else if (num > endLineIndex)
                    {
                        this.AddDirtyLines(endLineIndex, num + 1);
                    }
                    else
                    {
                        int top = this._location.LineIndex;
                        this.AddDirtyLines(top, top + 1);
                    }
                    this._owner.OnViewSelectionChanged(this);
                }
                else
                {
                    this.AddDirtyLines(this._selection.Start.LineIndex, this._selection.End.LineIndex + 1);
                    this._owner.OnViewSelectionChanged(this);
                }
                this._selectionExists = true;
            }
        }

        internal void UpdateTextLanguage()
        {
            TextBufferLocation location = this._buffer.CreateFirstCharacterLocation();
            TextLine next = location.Line;
            location.Dispose();
            while (next != null)
            {
                next.SetAttributesDirty(true);
                byte[] attributes = next.Attributes;
                if (attributes != null)
                {
                    for (int i = 0; i < attributes.Length; i++)
                    {
                        attributes[i] = 0;
                    }
                }
                next = next.Next;
            }
            base.Invalidate();
        }

        internal bool UpdateViewCommand(Command command)
        {
            if (command.CommandGroup == typeof(TextBufferCommands))
            {
                switch (command.CommandID)
                {
                    case 0x3f:
                        command.Enabled = this._selectionExists;
                        return true;

                    case 0x40:
                        command.Enabled = this._selectionExists;
                        return true;

                    case 0x41:
                        command.Enabled = true;
                        return true;
                }
            }
            return false;
        }

        private void UpdateVisibleLinesCols()
        {
            if ((this._fontHeight != 0) && (this._fontWidth != 0))
            {
                this._visibleLines = Math.Max(0, (base.Height - this.HorizontalScrollBarHeight) / this._fontHeight);
                this._visibleColumns = Math.Max(0, (((((base.Width - this.MarginWidth) - this._lineNumbersWidth) - this.MarginPadding) - this.VerticalScrollBarWidth) - 2) / this._fontWidth);
            }
            if ((this.ViewTopLineNumber + this.VisibleLines) > (this._buffer.LineCount - 1))
            {
                this.ViewTopLineNumber = Math.Max(0, this._buffer.LineCount - this.VisibleLines);
            }
            this._horizontalScrollAmount = this._visibleColumns / 4;
            this._verticalScrollBar.LargeChange = Math.Max(this.VisibleLines, 0);
            this._horizontalScrollBar.LargeChange = Math.Max(this.VisibleColumns, 0);
            this.UpdateScrollBars();
        }

        public bool AutoCompleteVisible
        {
            get
            {
                return (this._autoCompleteForm != null);
            }
        }

        internal int CaretWidth
        {
            get
            {
                if (!this._insertMode)
                {
                    int viewIndex = this.GetViewIndex(this._location);
                    int num2 = (int)this.MeasureString(this._location.Line.Data, viewIndex, 1);
                    if (num2 != 0)
                    {
                        return num2;
                    }
                }
                return 2;
            }
        }

        private ITextColorizer Colorizer
        {
            get
            {
                return this._owner.Colorizer;
            }
        }

        private ColorInfoTable ColorTable
        {
            get
            {
                return this._owner.ColorTable;
            }
        }

        internal int ColumnIndex
        {
            get
            {
                return this._location.ColumnIndex;
            }
            set
            {
                this._location.ColumnIndex = value;
            }
        }

        internal bool ConvertTabsToSpaces
        {
            get
            {
                return this._owner.ConvertTabsToSpaces;
            }
        }

        public bool HintVisible
        {
            get
            {
                return (this._autoCompleteHint != null);
            }
        }

        private int HorizontalScrollBarHeight
        {
            get
            {
                if (this._horizontalScrollBar.Visible)
                {
                    return this._horizontalScrollBar.Height;
                }
                return 0;
            }
        }

        internal bool InsertMode
        {
            get
            {
                return this._insertMode;
            }
            set
            {
                if (this._insertMode != value)
                {
                    this._insertMode = value;
                    if (!this._caretHiding)
                    {
                        this.HideCaret();
                        this.ShowCaret();
                    }
                }
            }
        }

        internal int LineIndex
        {
            get
            {
                return this._location.LineIndex;
            }
            set
            {
                this._location.GotoLine(value);
            }
        }

        internal int LineLength
        {
            get
            {
                return this._location.Line.Length;
            }
        }

        private bool LineNumbersVisible
        {
            get
            {
                return this._owner.LineNumbersVisible;
            }
        }

        internal int MarginPadding
        {
            get
            {
                return this._owner.MarginPadding;
            }
        }

        private int MarginWidth
        {
            get
            {
                return this._owner.MarginWidth;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this._owner.ReadOnly;
            }
        }

        private bool ResizeGripVisible
        {
            get
            {
                return this._owner.ResizeGripVisible;
            }
        }

        public string SelectedText
        {
            get
            {
                if (this._selectionExists)
                {
                    return this._selection.Text;
                }
                return string.Empty;
            }
        }

        public TextBufferSpan Selection
        {
            get
            {
                return this._selection;
            }
        }

        public bool SelectionExists
        {
            get
            {
                return this._selectionExists;
            }
        }

        public IServiceProvider ServiceProvider
        {
            get
            {
                return this._owner.Site;
            }
        }

        internal int TabbedColumnIndex
        {
            get
            {
                return (this.GetViewIndex(this._location) + this.ViewLeftIndex);
            }
        }

        public int TabSize
        {
            get
            {
                return this._owner.TabSize;
            }
        }

        private ITextLanguage TextLanguage
        {
            get
            {
                return this._owner.TextLanguage;
            }
        }

        private int VerticalScrollBarWidth
        {
            get
            {
                if (this._verticalScrollBar.Visible)
                {
                    return this._verticalScrollBar.Width;
                }
                return 0;
            }
        }

        private int ViewLeftIndex
        {
            get
            {
                return this._viewLeftIndex;
            }
            set
            {
                if (this._viewLeftIndex != value)
                {
                    this._viewLeftIndex = value;
                    this._updateCaretOnPaint = true;
                    this.UpdateScrollBars();
                    base.Invalidate();
                }
            }
        }

        private int ViewTopLineNumber
        {
            get
            {
                return this._viewTopLineNumber;
            }
            set
            {
                if (((this._viewTopLineNumber != value) && (value >= 0)) && ((value < ((this._buffer.LineCount - this.VisibleLines) + 1)) || (value < this._viewTopLineNumber)))
                {
                    this._viewTopLineNumber = value;
                    this._updateCaretOnPaint = true;
                    this.UpdateScrollBars();
                    base.Invalidate();
                }
            }
        }

        public int VisibleColumns
        {
            get
            {
                return this._visibleColumns;
            }
        }

        public int VisibleLines
        {
            get
            {
                return this._visibleLines;
            }
        }

        private class AutoCompleteLabel : Control
        {
            private Brush _backBrush;
            private Brush _foreBrush;
            private Pen _forePen;
            private string _highlightText;

            public AutoCompleteLabel(string text, string highlightText)
            {
                this._highlightText = highlightText;
                base.SetStyle(ControlStyles.UserPaint, true);
                base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                base.SetStyle(ControlStyles.Opaque, true);
                base.SetStyle(ControlStyles.DoubleBuffer, true);
                base.SetStyle(ControlStyles.ResizeRedraw, true);
                this.BackColor = SystemColors.Info;
                this._backBrush = new SolidBrush(this.BackColor);
                this.ForeColor = SystemColors.InfoText;
                this._foreBrush = new SolidBrush(this.ForeColor);
                this._forePen = new Pen(this.ForeColor, 1f);
                this.Font = new Font("Tahoma", 8f);
                this.Cursor = Cursors.Arrow;
                this.Text = text;
                base.Size = new Size(100, 0x10);
            }

            protected override void OnLayout(LayoutEventArgs e)
            {
                base.OnLayout(e);
                this.UpdateWidth();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                Graphics graphics = e.Graphics;
                Rectangle rect = new Rectangle(e.ClipRectangle.Top, e.ClipRectangle.Left, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
                graphics.FillRectangle(this._backBrush, rect);
                graphics.DrawRectangle(this._forePen, rect);
                bool flag = false;
                if (this._highlightText.Length > 0)
                {
                    int index = this.Text.IndexOf(this._highlightText);
                    if (index != -1)
                    {
                        int num2 = rect.Left + 2;
                        SizeF ef = graphics.MeasureString(this.Text.Substring(0, index), this.Font);
                        graphics.DrawString(this.Text.Substring(0, index), this.Font, this._foreBrush, (float)num2, (float)rect.Top);
                        num2 += (int)ef.Width;
                        SizeF ef2 = graphics.MeasureString(this.Text.Substring(index, this._highlightText.Length), new Font(this.Font, FontStyle.Bold));
                        graphics.DrawString(this.Text.Substring(index, this._highlightText.Length), new Font(this.Font, FontStyle.Bold), this._foreBrush, (float)num2, (float)rect.Top);
                        num2 += (int)ef2.Width;
                        graphics.DrawString(this.Text.Substring(index + this._highlightText.Length), this.Font, this._foreBrush, (float)num2, (float)rect.Top);
                        flag = true;
                    }
                }
                if (!flag)
                {
                    graphics.DrawString(this.Text, this.Font, this._foreBrush, (float)(rect.Left + 2), (float)rect.Top);
                }
            }

            public void SetHighlightText(string highlightText)
            {
                this._highlightText = highlightText;
                this.UpdateWidth();
                base.Invalidate();
            }

            private void UpdateWidth()
            {
                Graphics graphics = Graphics.FromHwnd(base.Handle);
                int num = 0;
                bool flag = false;
                if (this._highlightText.Length > 0)
                {
                    int index = this.Text.IndexOf(this._highlightText);
                    if (index != -1)
                    {
                        SizeF ef = graphics.MeasureString(this.Text.Substring(0, index), this.Font);
                        num += (int)ef.Width;
                        SizeF ef2 = graphics.MeasureString(this.Text.Substring(index, this._highlightText.Length), new Font(this.Font, FontStyle.Bold));
                        num += (int)ef2.Width;
                        SizeF ef3 = graphics.MeasureString(this.Text.Substring(index + this._highlightText.Length), this.Font);
                        num += (int)ef3.Width;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    SizeF ef4 = graphics.MeasureString(this.Text, this.Font, base.Width);
                    num += (int)ef4.Width;
                }
                base.Width = num + 4;
            }
        }
    }
}

