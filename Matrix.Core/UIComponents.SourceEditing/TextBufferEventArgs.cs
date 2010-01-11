namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;

    public class TextBufferEventArgs
    {
        public int _endIndex;
        public int _endLineNumber;
        public int _oldEndIndex;
        public int _oldEndLineNumber;
        public int _startIndex;
        public int _startLineNumber;

        public TextBufferEventArgs(int startLineNumber, int startIndex, int oldEndLineNumber, int oldEndIndex, int endLineNumber, int endIndex)
        {
            this._startLineNumber = startLineNumber;
            this._startIndex = startIndex;
            this._oldEndLineNumber = oldEndLineNumber;
            this._oldEndIndex = oldEndIndex;
            this._endLineNumber = endLineNumber;
            this._endIndex = endIndex;
        }

        public int EndIndex
        {
            get
            {
                return this._endIndex;
            }
        }

        public int EndLineNumber
        {
            get
            {
                return this._endLineNumber;
            }
        }

        public int OldEndIndex
        {
            get
            {
                return this._oldEndIndex;
            }
        }

        public int OldEndLineNumber
        {
            get
            {
                return this._oldEndLineNumber;
            }
        }

        public int StartIndex
        {
            get
            {
                return this._startIndex;
            }
        }

        public int StartLineNumber
        {
            get
            {
                return this._startLineNumber;
            }
        }
    }
}

