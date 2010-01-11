namespace Microsoft.Matrix.UIComponents
{
    using System;

    public class ToolBarComboBox : MxComboBox
    {
        private ToolBarComboBoxCommand _command;

        internal ToolBarComboBoxCommand Command
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

