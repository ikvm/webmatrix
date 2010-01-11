namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;

    public sealed class TextBufferCommands
    {
        public const int BeginBatchUndo = 7;
        public const int BlockComment = 0x5c;
        public const int BlockUncomment = 0x5d;
        public const int CenterLocation = 0x34;
        public const int Copy = 0x40;
        public const int Cut = 0x3f;
        public const int DeleteChar = 2;
        public const int DeleteNextWord = 0x5b;
        public const int DeletePreviousWord = 90;
        public const int DeleteSelection = 0x3e;
        public const int DeleteText = 3;
        public const int EndBatchUndo = 8;
        public const int GotoLine = 0x2b;
        public const int IndentSelection = 0x48;
        public const int InsertChar = 1;
        public const int InsertNewLine = 20;
        public const int InsertTab = 70;
        public const int InsertText = 4;
        public const int KeyDown = 80;
        public const int KeyPressed = 0x51;
        public const int MoveBeginningDocument = 0x29;
        public const int MoveBeginningLine = 0x22;
        public const int MoveEndDocument = 40;
        public const int MoveEndLine = 0x23;
        public const int MoveNextChar = 30;
        public const int MoveNextLine = 0x24;
        public const int MoveNextPage = 0x26;
        public const int MoveNextWord = 0x20;
        public const int MovePreviousChar = 0x1f;
        public const int MovePreviousLine = 0x25;
        public const int MovePreviousPage = 0x27;
        public const int MovePreviousWord = 0x21;
        public const int MoveXY = 0x2a;
        public const int Paste = 0x41;
        public const int Redo = 6;
        public const int RemoveTab = 0x47;
        public const int ReplaceChar = 9;
        public const int ResetSelection = 60;
        public const int SelectAll = 0x42;
        public const int SelectCurrentWord = 0x43;
        public const int SetLeftColumnIndex = 0x33;
        public const int SetTopLineIndex = 50;
        public const int ShowHelp = 0x5e;
        public const int Undo = 5;
        public const int UnindentSelection = 0x49;
        public const int UpdateSelection = 0x3d;

        private TextBufferCommands()
        {
        }
    }
}

