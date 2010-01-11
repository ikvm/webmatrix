namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;

    public class TextBuffer
    {
        private int _batchUndoCount;
        private string _cachedString;
        private TextBufferLocation _first;
        private TextBufferLocation _last;
        private int _lineCount;
        private IList _locationList;
        private TextLine _maxLine;
        private int _maxLineLength;
        private Stack _redoStack;
        private bool _undoing;
        private Stack _undoStack;

        public event TextBufferEventHandler TextBufferChanged;

        public TextBuffer() : this(0)
        {
        }

        public TextBuffer(int initialCapacity)
        {
            this.Initialize();
            for (int i = 0; i < (initialCapacity - 1); i++)
            {
                this.InsertLine(this._first.Line, this._first.LineIndex);
            }
        }

        public TextBuffer(TextReader reader)
        {
            this.Load(reader);
        }

        public TextBuffer(string text)
        {
            this.Text = text;
        }

        private void AddUndo(int command, int startLine, int startIndex, object data)
        {
            this.AddUndo(command, startLine, startIndex, -1, -1, data);
        }

        private void AddUndo(int command, int startLine, int startIndex, int endLine, int endIndex, object data)
        {
            UndoUnit unit = new UndoUnit(command, startLine, startIndex, endLine, endIndex, data);
            if (this._batchUndoCount > 0)
            {
                ((BatchUndoUnit) this.UndoStack.Peek()).AddUndoUnit(unit);
            }
            else
            {
                this.UndoStack.Push(unit);
            }
        }

        public void BeginBatchUndo()
        {
            this._batchUndoCount++;
            if (this._batchUndoCount == 1)
            {
                this.UndoStack.Push(new BatchUndoUnit());
            }
        }

        public TextBufferLocation CreateFirstCharacterLocation()
        {
            return this.CreateTextBufferLocation(this._first);
        }

        public TextBufferLocation CreateLastCharacterLocation()
        {
            return this.CreateTextBufferLocation(this._last);
        }

        private TextBufferSpan CreateStringSpan(TextBufferLocation location, int stringLength)
        {
            TextBufferLocation end = location.Clone();
            end.ColumnIndex += stringLength;
            return new TextBufferSpan(location, end);
        }

        public TextBufferLocation CreateTextBufferLocation()
        {
            TextBufferLocation location = new TextBufferLocation(this._first.Line, 0, 0, this);
            this.LocationList.Add(location);
            return location;
        }

        public TextBufferLocation CreateTextBufferLocation(TextBufferLocation location)
        {
            TextBufferLocation location2 = new TextBufferLocation(location.Line, location.ColumnIndex, location.LineIndex, this);
            this.LocationList.Add(location2);
            return location2;
        }

        public bool DeleteChar(TextBufferLocation location)
        {
            return this.DeleteChar(location, true);
        }

        private bool DeleteChar(TextBufferLocation location, bool addToUndo)
        {
            if (!this._undoing)
            {
                this.RedoStack.Clear();
            }
            TextLine line = location.Line;
            int columnIndex = location.ColumnIndex;
            if (columnIndex >= line.Length)
            {
                return false;
            }
            char data = line.Data[columnIndex];
            line.Delete(columnIndex, 1);
            if (addToUndo)
            {
                this.AddUndo(2, location.LineIndex, columnIndex, data);
            }
            if (line == this._last.Line)
            {
                this._last.ColumnIndex = line.Length;
            }
            int lineIndex = location.LineIndex;
            this.OnTextBufferChanged(new TextBufferEventArgs(lineIndex, columnIndex, lineIndex, columnIndex + 1, lineIndex, columnIndex));
            return true;
        }

        private bool DeleteLine(TextBufferLocation location)
        {
            TextLine line = location.Line;
            if (line == null)
            {
                return false;
            }
            TextLine previous = line.Previous;
            TextLine next = line.Next;
            bool flag = true;
            if (previous != null)
            {
                previous.Next = line.Next;
                if (next != null)
                {
                    next.Previous = previous;
                }
                else
                {
                    this._last.SetLine(previous, this._last.LineIndex - 1);
                    this._last.ColumnIndex = this._last.Line.Length;
                }
                location.SetLine(previous, location.LineIndex - 1);
                this._lineCount--;
            }
            else if (next != null)
            {
                next.Previous = null;
                this._first.SetLine(next, 0);
                location.SetLine(next, 0);
                this._lineCount--;
            }
            else
            {
                line.Clear();
                flag = false;
            }
            if (flag)
            {
                this.FixLocationLineNumbers(location.LineIndex, -1, location.Line, location.LineIndex, location.ColumnIndex);
            }
            return true;
        }

        public void DeleteText(TextBufferSpan span)
        {
            this.DeleteText(span, true);
        }

        private void DeleteText(TextBufferSpan span, bool addToUndo)
        {
            if (!this._undoing)
            {
                this.RedoStack.Clear();
            }
            TextBufferLocation start = span.Start;
            TextBufferLocation end = span.End;
            TextLine line = start.Line;
            TextLine line2 = end.Line;
            ArrayList data = new ArrayList((end.LineIndex - start.LineIndex) + 1);
            int lineIndex = start.LineIndex;
            int columnIndex = start.ColumnIndex;
            int endLine = end.LineIndex;
            int endIndex = end.ColumnIndex;
            if (line == line2)
            {
                int length = end.ColumnIndex - start.ColumnIndex;
                if (length > 0)
                {
                    data.Add(line.ToCharArray(start.ColumnIndex, length));
                    if (end.ColumnIndex == line.Length)
                    {
                        line.Delete(start.ColumnIndex);
                    }
                    else
                    {
                        line.Delete(start.ColumnIndex, length);
                    }
                    if (line == this._last.Line)
                    {
                        this._last.ColumnIndex = this._last.Line.Length;
                    }
                }
            }
            else
            {
                int num6 = line.Length - start.ColumnIndex;
                data.Add(line.ToCharArray(start.ColumnIndex, num6));
                TextLine next = line.Next;
                int num7 = 0;
                while (next != line2)
                {
                    data.Add(next.ToCharArray());
                    next = next.Next;
                    num7++;
                }
                data.Add(line2.ToCharArray(0, end.ColumnIndex));
                line.Delete(start.ColumnIndex);
                line.Next = next;
                line2.Previous = line;
                if (end.ColumnIndex > 0)
                {
                    line2.Delete(0, end.ColumnIndex);
                    if (line2 == this._last.Line)
                    {
                        this._last.ColumnIndex = this._last.Line.Length;
                    }
                }
                if (num7 > 0)
                {
                    this.FixLocationLineNumbers(start.LineIndex, -num7, start.Line, start.LineIndex, start.ColumnIndex);
                }
                this.MergeWithNextLine(start);
                this._lineCount -= num7;
            }
            if (addToUndo && (data.Count > 0))
            {
                this.AddUndo(3, lineIndex, columnIndex, endLine, endIndex, data);
            }
            this.OnTextBufferChanged(new TextBufferEventArgs(lineIndex, columnIndex, endLine, endIndex, lineIndex, columnIndex));
            end.SetLine(start.Line, start.LineIndex);
            end.ColumnIndex = start.ColumnIndex;
        }

        internal void DeleteTextBufferLocation(TextBufferLocation location)
        {
            this.LocationList.Remove(location);
        }

        public void EndBatchUndo()
        {
            if (this._batchUndoCount > 0)
            {
                this._batchUndoCount--;
            }
        }

        public TextBufferSpan Find(string searchString, bool matchCase, bool wholeWord, bool searchUp)
        {
            TextBufferSpan span;
            TextBufferLocation startLocation = this.First.Clone();
            TextBufferLocation endLocation = this.Last.Clone();
            try
            {
                span = this.Find(startLocation, endLocation, searchString, matchCase, wholeWord, searchUp);
            }
            finally
            {
                startLocation.Dispose();
                endLocation.Dispose();
            }
            return span;
        }

        public TextBufferSpan Find(TextBufferLocation startLocation, TextBufferLocation endLocation, string searchString, bool matchCase, bool wholeWord, bool searchUp)
        {
            if (startLocation == null)
            {
                throw new ArgumentNullException("startLocation");
            }
            if (endLocation == null)
            {
                throw new ArgumentNullException("endLocation");
            }
            TextBufferLocation location = null;
            int lineIndex = 0;
            if (searchUp)
            {
                location = endLocation.Clone();
                lineIndex = startLocation.LineIndex;
            }
            else
            {
                location = startLocation.Clone();
                lineIndex = endLocation.LineIndex;
            }
            TextLine line = location.Line;
            int length = searchString.Length;
            int columnIndex = location.ColumnIndex;
            if (searchUp)
            {
                if (columnIndex == 0)
                {
                    location.MoveUp(1);
                    line = location.Line;
                    columnIndex = line.Length;
                }
                else
                {
                    columnIndex--;
                }
            }
            while (location.LineIndex != lineIndex)
            {
                if (searchUp)
                {
                    columnIndex = line.LastIndexOf(searchString, columnIndex, matchCase, wholeWord);
                }
                else
                {
                    columnIndex = line.IndexOf(searchString, columnIndex, matchCase, wholeWord);
                }
                if (columnIndex >= 0)
                {
                    location.ColumnIndex = columnIndex;
                    return this.CreateStringSpan(location, length);
                }
                if (searchUp)
                {
                    location.MoveUp(1);
                }
                else
                {
                    location.MoveDown(1);
                }
                line = location.Line;
                if (searchUp)
                {
                    columnIndex = line.Length;
                }
                else
                {
                    columnIndex = 0;
                }
            }
            if (searchUp)
            {
                columnIndex = line.LastIndexOf(searchString, columnIndex, matchCase, wholeWord);
                if ((columnIndex != -1) && (columnIndex >= startLocation.ColumnIndex))
                {
                    location.ColumnIndex = columnIndex;
                    return this.CreateStringSpan(location, length);
                }
            }
            else
            {
                columnIndex = line.IndexOf(searchString, columnIndex, matchCase, wholeWord);
                if ((columnIndex != -1) && ((columnIndex + length) <= endLocation.ColumnIndex))
                {
                    location.ColumnIndex = columnIndex;
                    return this.CreateStringSpan(location, length);
                }
            }
            location.Dispose();
            return null;
        }

        private void FixLocationLineNumbers(int startLineIndex, int lineIndexDelta, TextLine defaultLine, int defaultLineIndex, int defaultIndex)
        {
            foreach (TextBufferLocation location in this.LocationList)
            {
                if (location.LineIndex > startLineIndex)
                {
                    if ((location.LineIndex + lineIndexDelta) <= startLineIndex)
                    {
                        location.SetLine(defaultLine, defaultLineIndex);
                        location.ColumnIndex = defaultIndex;
                    }
                    else
                    {
                        location.UpdateLineIndex(location.LineIndex + lineIndexDelta);
                    }
                }
            }
        }

        private void Initialize()
        {
            this.UndoStack.Clear();
            this.RedoStack.Clear();
            TextLine line = new TextLine();
            this._first = new TextBufferLocation(line, 0, 0, this);
            this.LocationList.Clear();
            this._last = this.CreateTextBufferLocation();
            this._lineCount = 1;
        }

        public bool InsertChar(TextBufferLocation location, char c)
        {
            return this.InsertChar(location, c, true);
        }

        private bool InsertChar(TextBufferLocation location, char c, bool addToUndo)
        {
            if (!this._undoing)
            {
                this.RedoStack.Clear();
            }
            TextLine line = location.Line;
            int columnIndex = location.ColumnIndex;
            if (columnIndex == line.Length)
            {
                line.Append(c);
            }
            else
            {
                line.Insert(columnIndex, c);
            }
            if (line == this._last.Line)
            {
                this._last.ColumnIndex = line.Length;
            }
            if (line.Length > this._maxLineLength)
            {
                this._maxLineLength = line.Length;
                this._maxLine = line;
            }
            if (addToUndo)
            {
                this.AddUndo(1, location.LineIndex, columnIndex, c);
            }
            int lineIndex = location.LineIndex;
            this.OnTextBufferChanged(new TextBufferEventArgs(lineIndex, columnIndex, lineIndex, columnIndex, lineIndex, columnIndex + 1));
            location.ColumnIndex++;
            return true;
        }

        private void InsertLine(TextLine line, int lineNum)
        {
            this.InsertLine(line, lineNum, new TextLine(), false);
        }

        private void InsertLine(TextLine line, int lineNum, bool isBatching)
        {
            this.InsertLine(line, lineNum, new TextLine(), isBatching);
        }

        private void InsertLine(TextLine line, int lineNum, TextLine newLine, bool isBatching)
        {
            TextLine next = line.Next;
            line.Next = newLine;
            newLine.Previous = line;
            newLine.Next = next;
            if (next != null)
            {
                next.Previous = newLine;
            }
            if (!isBatching)
            {
                if (next == null)
                {
                    this._last.SetLine(newLine, lineNum + 1);
                    this._last.ColumnIndex = this._last.Line.Length;
                }
                this._lineCount++;
            }
        }

        public void InsertText(TextBufferLocation location, ArrayList textList)
        {
            this.InsertText(location, textList, true);
        }

        public void InsertText(TextBufferLocation location, string s)
        {
            ArrayList textList = new ArrayList();
            StringReader reader = new StringReader(s);
            for (string str = reader.ReadLine(); str != null; str = reader.ReadLine())
            {
                textList.Add(str.ToCharArray());
            }
            this.InsertText(location, textList);
        }

        private void InsertText(TextBufferLocation location, ArrayList textList, bool addToUndo)
        {
            this.InsertText(location, textList, addToUndo, false);
        }

        private void InsertText(TextBufferLocation location, ArrayList textList, bool addToUndo, bool loading)
        {
            if (!this._undoing)
            {
                this.RedoStack.Clear();
            }
            int count = textList.Count;
            if (count >= 1)
            {
                TextLine line = location.Line;
                int lineIndex = location.LineIndex;
                int columnIndex = location.ColumnIndex;
                TextLine next = line;
                int lineNum = lineIndex;
                int index = columnIndex;
                int lineIndexDelta = 0;
                bool flag = columnIndex < line.Length;
                for (int i = 0; i < count; i++)
                {
                    char[] chars = (char[]) textList[i];
                    int length = chars.Length;
                    int num9 = 0;
                    if (i > 0)
                    {
                        next = next.Next;
                        lineNum++;
                    }
                    if (length > 0)
                    {
                        next.Insert(index, chars, 0, length);
                        if (next == this._last.Line)
                        {
                            this._last.ColumnIndex = next.Length;
                        }
                        num9 = next.Length;
                        if (num9 > this._maxLineLength)
                        {
                            this._maxLineLength = num9;
                            this._maxLine = next;
                        }
                        index += length;
                    }
                    if (i < (count - 1))
                    {
                        if (flag)
                        {
                            this.SplitLine(next, lineNum, index, true);
                        }
                        else
                        {
                            this.InsertLine(next, lineNum, true);
                        }
                        lineIndexDelta++;
                        index = 0;
                    }
                }
                this.FixLocationLineNumbers(lineIndex, lineIndexDelta, next, lineNum, index);
                this._lineCount += lineIndexDelta;
                this._last.MoveDown(lineIndexDelta);
                this._last.ColumnIndex = this._last.Line.Length;
                if (addToUndo)
                {
                    this.AddUndo(4, lineIndex, columnIndex, lineNum, index, textList);
                }
                if (!loading)
                {
                    this.OnTextBufferChanged(new TextBufferEventArgs(lineIndex, columnIndex, lineIndex, columnIndex, lineNum, index));
                }
                location.SetLine(next, lineNum);
                location.ColumnIndex = index;
            }
        }

        public void Load(TextReader reader)
        {
            this.LocationList.Remove(this._last);
            ArrayList list = new ArrayList(this.LocationList);
            this.Initialize();
            ArrayList textList = new ArrayList();
            for (string str = reader.ReadLine(); str != null; str = reader.ReadLine())
            {
                textList.Add(str.ToCharArray());
            }
            reader.Close();
            TextBufferLocation location = this.CreateTextBufferLocation();
            this.InsertText(location, textList, false, true);
            location.Dispose();
            foreach (TextBufferLocation location2 in list)
            {
                int lineIndex = location2.LineIndex;
                int columnIndex = location2.ColumnIndex;
                location2.MoveTo(this._first);
                location2.GotoLine(lineIndex);
                location2.ColumnIndex = Math.Min(columnIndex, location2.Line.Length);
                this.LocationList.Add(location2);
            }
            this.OnTextBufferChanged(new TextBufferEventArgs(0, 0, 0, 0, this.LineCount - 1, this.Last.Line.Length));
        }

        private bool MergeWithNextLine(TextBufferLocation location)
        {
            int lineIndex = location.LineIndex;
            int columnIndex = location.ColumnIndex;
            TextLine line = location.Line;
            TextLine next = line.Next;
            if ((line == null) || (next == null))
            {
                return false;
            }
            int length = line.Length;
            char[] data = next.Data;
            int num2 = next.Length;
            if ((data != null) && (num2 > 0))
            {
                line.Append(data, num2);
                if (line.Length > this._maxLineLength)
                {
                    this._maxLineLength = line.Length;
                    this._maxLine = line;
                }
            }
            location.MoveDown(1);
            this.DeleteLine(location);
            location.ColumnIndex = length;
            return true;
        }

        protected void OnTextBufferChanged(TextBufferEventArgs e)
        {
            this._cachedString = null;
            if (this.TextBufferChanged != null)
            {
                this.TextBufferChanged(this, e);
            }
        }

        private void ProcessRedoUnit(UndoUnit unit, TextBufferLocation location)
        {
            TextBufferLocation location2 = this.CreateTextBufferLocation();
            location2.GotoLine(unit.StartLineNum);
            location2.ColumnIndex = unit.StartIndex;
            switch (unit.Command)
            {
                case 1:
                    this.InsertChar(location2, (char) unit.Data, false);
                    break;

                case 2:
                    this.DeleteChar(location2, false);
                    break;

                case 3:
                {
                    TextBufferLocation end = this.CreateTextBufferLocation();
                    end.GotoLine(unit.EndLineNum);
                    end.ColumnIndex = unit.EndIndex;
                    this.DeleteText(new TextBufferSpan(location2, end), false);
                    end.Dispose();
                    break;
                }
                case 4:
                    this.InsertText(location2, (ArrayList) unit.Data, false);
                    break;
            }
            if (location != null)
            {
                location.SetLine(location2.Line, location2.LineIndex);
                location.ColumnIndex = location2.ColumnIndex;
            }
            location2.Dispose();
        }

        private void ProcessUndoUnit(UndoUnit unit, TextBufferLocation location)
        {
            TextBufferLocation location2 = this.CreateTextBufferLocation();
            location2.GotoLine(unit.StartLineNum);
            location2.ColumnIndex = unit.StartIndex;
            switch (unit.Command)
            {
                case 1:
                    this.DeleteChar(location2, false);
                    break;

                case 2:
                    this.InsertChar(location2, (char) unit.Data, false);
                    break;

                case 3:
                    this.InsertText(location2, (ArrayList) unit.Data, false);
                    break;

                case 4:
                {
                    TextBufferLocation end = this.CreateTextBufferLocation();
                    end.GotoLine(unit.EndLineNum);
                    end.ColumnIndex = unit.EndIndex;
                    this.DeleteText(new TextBufferSpan(location2, end), false);
                    end.Dispose();
                    break;
                }
            }
            if (location != null)
            {
                location.SetLine(location2.Line, location2.LineIndex);
                location.ColumnIndex = location2.ColumnIndex;
            }
            location2.Dispose();
        }

        public bool Redo(TextBufferLocation location)
        {
            if (this.RedoStack.Count == 0)
            {
                return false;
            }
            this._undoing = true;
            try
            {
                object obj2 = this.RedoStack.Pop();
                if (obj2 is UndoUnit)
                {
                    this.ProcessRedoUnit((UndoUnit) obj2, location);
                }
                else if (obj2 is BatchUndoUnit)
                {
                    ArrayList undoList = ((BatchUndoUnit) obj2).UndoList;
                    for (int i = 0; i < undoList.Count; i++)
                    {
                        this.ProcessRedoUnit((UndoUnit) undoList[i], location);
                    }
                }
                this.UndoStack.Push(obj2);
            }
            finally
            {
                this._undoing = false;
            }
            return true;
        }

        public void ReplaceChar(TextBufferLocation location, char c)
        {
            this.BeginBatchUndo();
            this.DeleteChar(location);
            this.InsertChar(location, c);
            this.EndBatchUndo();
        }

        public bool ReplaceText(TextBufferSpan span, ArrayList textList)
        {
            this.BeginBatchUndo();
            try
            {
                if (span.IsEmpty)
                {
                    this.DeleteText(span, true);
                }
                if (textList.Count > 0)
                {
                    this.InsertText(span.Start, textList, true);
                }
            }
            finally
            {
                this.EndBatchUndo();
            }
            return true;
        }

        private bool SplitLine(TextLine line, int lineNum, int index, bool isBatching)
        {
            if ((index > line.Length) || (index < 0))
            {
                return false;
            }
            char[] data = line.Data;
            this.InsertLine(line, lineNum, new TextLine(data, index, line.Length - index), isBatching);
            line.Delete(index);
            return true;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("**Start Buffer");
            builder.Append(Environment.NewLine);
            builder.Append(this.Text);
            builder.Append(Environment.NewLine);
            builder.Append("**End Buffer");
            builder.Append(Environment.NewLine);
            int num = 0;
            foreach (TextBufferLocation location in this.LocationList)
            {
                builder.Append(string.Concat(new object[] { "Loc ", num, " : ", location, "\n" }));
                num++;
            }
            builder.Append("Last : " + this._last + "\n");
            builder.Append("\n");
            return builder.ToString();
        }

        public bool Undo(TextBufferLocation location)
        {
            if (this.UndoStack.Count == 0)
            {
                return false;
            }
            this._undoing = true;
            try
            {
                object obj2 = this.UndoStack.Pop();
                if (obj2 is UndoUnit)
                {
                    this.ProcessUndoUnit((UndoUnit) obj2, location);
                }
                else if (obj2 is BatchUndoUnit)
                {
                    ArrayList undoList = ((BatchUndoUnit) obj2).UndoList;
                    for (int i = undoList.Count - 1; i >= 0; i--)
                    {
                        this.ProcessUndoUnit((UndoUnit) undoList[i], location);
                    }
                }
                this.RedoStack.Push(obj2);
            }
            finally
            {
                this._undoing = false;
            }
            return true;
        }

        public bool CanRedo
        {
            get
            {
                return (this.RedoStack.Count > 0);
            }
        }

        public bool CanUndo
        {
            get
            {
                return (this.UndoStack.Count > 0);
            }
        }

        internal TextBufferLocation First
        {
            get
            {
                return this._first;
            }
        }

        internal TextBufferLocation Last
        {
            get
            {
                return this._last;
            }
        }

        public int LineCount
        {
            get
            {
                return this._lineCount;
            }
        }

        private IList LocationList
        {
            get
            {
                if (this._locationList == null)
                {
                    this._locationList = new ArrayList();
                }
                return this._locationList;
            }
        }

        internal TextLine MaxLine
        {
            get
            {
                return this._maxLine;
            }
        }

        public int MaxLineLength
        {
            get
            {
                return this._maxLineLength;
            }
        }

        private Stack RedoStack
        {
            get
            {
                if (this._redoStack == null)
                {
                    this._redoStack = new Stack();
                }
                return this._redoStack;
            }
        }

        public string Text
        {
            get
            {
                if (this._cachedString == null)
                {
                    TextBufferSpan span = new TextBufferSpan(this._first, this._last);
                    this._cachedString = span.Text;
                }
                return this._cachedString;
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    int lineIndex = this._last.LineIndex;
                    int columnIndex = this._last.ColumnIndex;
                    this._cachedString = string.Empty;
                    ArrayList list = new ArrayList(this.LocationList);
                    this.LocationList.Remove(this._last);
                    this.Initialize();
                    foreach (TextBufferLocation location in list)
                    {
                        location.MoveTo(this._first);
                        this.LocationList.Add(location);
                    }
                    this.OnTextBufferChanged(new TextBufferEventArgs(0, 0, lineIndex, columnIndex, 0, 0));
                }
                else
                {
                    this._cachedString = value;
                    this.Load(new StringReader(value));
                }
            }
        }

        private Stack UndoStack
        {
            get
            {
                if (this._undoStack == null)
                {
                    this._undoStack = new Stack();
                }
                return this._undoStack;
            }
        }

        private sealed class BatchUndoUnit
        {
            private ArrayList _undoList;

            public void AddUndoUnit(TextBuffer.UndoUnit unit)
            {
                this.UndoList.Add(unit);
            }

            public ArrayList UndoList
            {
                get
                {
                    if (this._undoList == null)
                    {
                        this._undoList = new ArrayList();
                    }
                    return this._undoList;
                }
            }
        }

        private sealed class UndoUnit
        {
            private int _command;
            private object _data;
            private int _endIndex;
            private int _endLineNum;
            private int _startIndex;
            private int _startLineNum;

            public UndoUnit(int command, int startLineNum, int startIndex, int endLineNum, int endIndex, object data)
            {
                this._command = command;
                this._startLineNum = startLineNum;
                this._startIndex = startIndex;
                this._endLineNum = endLineNum;
                this._endIndex = endIndex;
                this._data = data;
            }

            public int Command
            {
                get
                {
                    return this._command;
                }
            }

            public object Data
            {
                get
                {
                    return this._data;
                }
            }

            public int EndIndex
            {
                get
                {
                    return this._endIndex;
                }
            }

            public int EndLineNum
            {
                get
                {
                    return this._endLineNum;
                }
            }

            public int StartIndex
            {
                get
                {
                    return this._startIndex;
                }
            }

            public int StartLineNum
            {
                get
                {
                    return this._startLineNum;
                }
            }
        }
    }
}

