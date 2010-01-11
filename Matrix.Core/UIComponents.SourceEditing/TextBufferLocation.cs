namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;

    public sealed class TextBufferLocation : IDisposable
    {
        private int _beginUpdateColumnIndex;
        private int _beginUpdateLineIndex;
        private TextBuffer _buffer;
        private int _columnIndex;
        private TextLine _line;
        private int _lineIndex;
        private int _updateCount;

        public event EventHandler LocationChanged;

        internal TextBufferLocation(TextLine line, int index, int lineNumber, TextBuffer buffer)
        {
            this._line = line;
            this._columnIndex = index;
            this._lineIndex = lineNumber;
            this._buffer = buffer;
        }

        private void BeginUpdate()
        {
            if (this._updateCount == 0)
            {
                this._beginUpdateColumnIndex = this._columnIndex;
                this._beginUpdateLineIndex = this._lineIndex;
            }
            this._updateCount++;
        }

        public TextBufferLocation Clone()
        {
            return this._buffer.CreateTextBufferLocation(this);
        }

        public void Dispose()
        {
            this._buffer.DeleteTextBufferLocation(this);
        }

        private void EndUpdate()
        {
            this._updateCount--;
            if (this._updateCount < 0)
            {
                this._updateCount = 0;
            }
            if ((this._updateCount == 0) && ((this._beginUpdateColumnIndex != this._columnIndex) || (this._beginUpdateLineIndex != this._lineIndex)))
            {
                this.OnLocationChanged();
            }
        }

        public bool GotoLine(int lineIndex)
        {
            if ((lineIndex > this._buffer.LineCount) || (lineIndex < 0))
            {
                return false;
            }
            if (lineIndex > this._lineIndex)
            {
                this.MoveDown(lineIndex - this._lineIndex);
            }
            else if (lineIndex < this._lineIndex)
            {
                this.MoveUp(this._lineIndex - lineIndex);
            }
            return true;
        }

        public bool GotoLineColumn(int lineIndex, int columnIndex)
        {
            bool flag = false;
            this.BeginUpdate();
            try
            {
                flag = this.GotoLine(lineIndex);
                this.ColumnIndex = columnIndex;
            }
            finally
            {
                this.EndUpdate();
            }
            return flag;
        }

        public int MoveDown(int lines)
        {
            int num = 0;
            TextLine next = this._line;
            int lineIndex = this._lineIndex;
            while ((num < lines) && (next != null))
            {
                next = next.Next;
                if (next != null)
                {
                    this._line = next;
                    lineIndex++;
                    num++;
                }
            }
            this.BeginUpdate();
            try
            {
                if (this.ColumnIndex > this._line.Length)
                {
                    this.ColumnIndex = this._line.Length;
                }
                this.UpdateLineIndex(lineIndex);
            }
            finally
            {
                this.EndUpdate();
            }
            return num;
        }

        public void MoveTo(TextBufferLocation location)
        {
            this.SetLine(location.Line, location.LineIndex);
            this.ColumnIndex = location.ColumnIndex;
        }

        public int MoveUp(int lines)
        {
            int num = 0;
            TextLine previous = this._line;
            int lineIndex = this._lineIndex;
            while ((num < lines) && (previous != null))
            {
                previous = previous.Previous;
                if (previous != null)
                {
                    this._line = previous;
                    lineIndex--;
                    num++;
                }
            }
            this.BeginUpdate();
            try
            {
                if (this.ColumnIndex > this._line.Length)
                {
                    this.ColumnIndex = this._line.Length;
                }
                this.UpdateLineIndex(lineIndex);
            }
            finally
            {
                this.EndUpdate();
            }
            return num;
        }

        private void OnLocationChanged()
        {
            if ((this._updateCount == 0) && (this.LocationChanged != null))
            {
                this.LocationChanged(this, EventArgs.Empty);
            }
        }

        internal void SetLine(TextLine line, int lineIndex)
        {
            this._line = line;
            if (this.ColumnIndex > this._line.Length)
            {
                this.ColumnIndex = this._line.Length;
            }
            this.UpdateLineIndex(lineIndex);
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "(", this.LineIndex, ",", this.ColumnIndex, ")" });
        }

        internal void UpdateLineIndex(int lineIndex)
        {
            if (this._lineIndex != lineIndex)
            {
                this._lineIndex = lineIndex;
                this.OnLocationChanged();
            }
        }

        public int ColumnIndex
        {
            get
            {
                return this._columnIndex;
            }
            set
            {
                if ((value >= 0) && (value <= this._line.Length))
                {
                    this._columnIndex = value;
                    this.OnLocationChanged();
                }
            }
        }

        public TextLine Line
        {
            get
            {
                return this._line;
            }
        }

        public int LineIndex
        {
            get
            {
                return this._lineIndex;
            }
        }
    }
}

