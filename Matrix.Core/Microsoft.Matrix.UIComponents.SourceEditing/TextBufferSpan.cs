namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;
    using System.Text;

    public class TextBufferSpan : IDisposable
    {
        private TextBufferLocation _end;
        private TextBufferLocation _start;

        public TextBufferSpan(TextBufferLocation start, TextBufferLocation end)
        {
            if ((start.LineIndex > end.LineIndex) || ((start.LineIndex == end.LineIndex) && (start.ColumnIndex > end.ColumnIndex)))
            {
                this._start = end;
                this._end = start;
            }
            else
            {
                this._start = start;
                this._end = end;
            }
        }

        public bool Contains(TextBufferLocation location)
        {
            return this.Contains(location.LineIndex, location.ColumnIndex);
        }

        public bool Contains(TextBufferSpan span)
        {
            return (this.Contains(span.Start) && this.Contains(span.End));
        }

        public bool Contains(int lineIndex, int columnIndex)
        {
            if (this.IsSingleLine && (lineIndex == this._start.LineIndex))
            {
                if ((columnIndex >= this._start.ColumnIndex) && (columnIndex <= this._end.ColumnIndex))
                {
                    return true;
                }
            }
            else
            {
                if ((lineIndex > this._start.LineIndex) && (lineIndex < this._end.LineIndex))
                {
                    return true;
                }
                if (lineIndex == this._start.LineIndex)
                {
                    if (columnIndex >= this._start.ColumnIndex)
                    {
                        return true;
                    }
                }
                else if ((lineIndex == this._end.LineIndex) && (columnIndex <= this._end.ColumnIndex))
                {
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            this._start.Dispose();
            this._end.Dispose();
        }

        public override string ToString()
        {
            return (this._start.ToString() + " to " + this._end.ToString());
        }

        public TextBufferLocation End
        {
            get
            {
                return this._end;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return ((this._start.Line == this._end.Line) && (this._start.ColumnIndex == this._end.ColumnIndex));
            }
        }

        public bool IsSingleLine
        {
            get
            {
                return (this._start.LineIndex == this._end.LineIndex);
            }
        }

        public TextBufferLocation Start
        {
            get
            {
                return this._start;
            }
        }

        public string Text
        {
            get
            {
                if (this.IsSingleLine)
                {
                    return new string(this._start.Line.ToCharArray(this._start.ColumnIndex, this._end.ColumnIndex - this._start.ColumnIndex));
                }
                StringBuilder builder = new StringBuilder((this._end.LineIndex - this._start.LineIndex) * 160);
                int lineIndex = this._start.LineIndex;
                int columnIndex = this._start.ColumnIndex;
                TextLine line = this._start.Line;
                TextLine line2 = this._end.Line;
                builder.Append(line.ToCharArray(columnIndex, line.Length - columnIndex));
                builder.Append(Environment.NewLine);
                for (line = line.Next; (line != line2) && (line != null); line = line.Next)
                {
                    builder.Append(line.ToString());
                    builder.Append(Environment.NewLine);
                }
                builder.Append(line2.ToCharArray(0, this._end.ColumnIndex));
                builder.Append(Environment.NewLine);
                return builder.ToString();
            }
        }
    }
}

