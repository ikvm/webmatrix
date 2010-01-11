namespace Microsoft.Matrix.UIComponents
{
    using System;

    public class EditorStatusBarPanel : MxStatusBarPanel
    {
        private int _column = -1;
        private int _line = -1;
        private const string LineColumnFormat = "Line {0}, Col {1}";
        private const string LineFormat = "Line {0}";

        public void SetCurrentLine(int line)
        {
            if (this._line != line)
            {
                this._line = line;
                this._column = -1;
                this.UpdateText();
            }
        }

        public void SetCurrentLineAndColumn(int line, int column)
        {
            if ((this._line != line) || (this._column != column))
            {
                this._line = line;
                this._column = column;
                this.UpdateText();
            }
        }

        private void UpdateText()
        {
            string str = string.Empty;
            if (this._column == -1)
            {
                if (this._line != -1)
                {
                    str = string.Format("Line {0}", this._line);
                }
            }
            else
            {
                str = string.Format("Line {0}, Col {1}", this._line, this._column);
            }
            base.Text = str;
        }

        public int Column
        {
            get
            {
                return this._column;
            }
        }

        public int Line
        {
            get
            {
                return this._line;
            }
        }
    }
}

