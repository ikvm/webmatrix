namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Windows.Forms;

    public class MxToolBarButton : ToolBarButton
    {
        private ToolBarButtonCommand _command;

        public MxToolBarButton() : this(null)
        {
        }

        public MxToolBarButton(string text) : base(text)
        {
        }

        internal ToolBarButtonCommand Command
        {
            get
            {
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
    }
}

