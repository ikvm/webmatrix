namespace Microsoft.Matrix.UIComponents
{
    using Microsoft.Matrix;
    using System;
    using System.Windows.Forms;

    public class MxRichTextBox : RichTextBox
    {
        private int _updateCount;

        public void BeginUpdate()
        {
            if (base.IsHandleCreated)
            {
                this._updateCount++;
                if (this._updateCount == 1)
                {
                    Interop.SendMessage(base.Handle, 11, 0, 0);
                }
            }
        }

        public void EndUpdate(bool invalidate)
        {
            if (this._updateCount > 0)
            {
                this._updateCount--;
                if (this._updateCount == 0)
                {
                    Interop.SendMessage(base.Handle, 11, -1, 0);
                    if (invalidate)
                    {
                        base.Invalidate();
                    }
                }
            }
        }

        public int CurrentColumn
        {
            get
            {
                int wParam = this.CurrentLine - 1;
                int num2 = (int) Interop.SendMessage(base.Handle, 0xbb, wParam, 0);
                if (num2 == -1)
                {
                    return 0;
                }
                return ((base.SelectionStart - num2) + 1);
            }
        }

        public int CurrentLine
        {
            get
            {
                int num = -1;
                if (base.IsHandleCreated)
                {
                    num = (int) Interop.SendMessage(base.Handle, 0xc9, -1, 0);
                }
                return (num + 1);
            }
            set
            {
                if (value <= 1)
                {
                    base.Select(0, 0);
                }
                else
                {
                    string text = this.Text;
                    int num = 0;
                    int num2 = 0;
                    for (int i = 0; i < value; i++)
                    {
                        int index = text.IndexOf("\n", (int) (num2 + 1));
                        if (index == -1)
                        {
                            break;
                        }
                        num = num2;
                        num2 = index;
                    }
                    base.Select(num + 1, 0);
                }
                base.ScrollToCaret();
            }
        }
    }
}

