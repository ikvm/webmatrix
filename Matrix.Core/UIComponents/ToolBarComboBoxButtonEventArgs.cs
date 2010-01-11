namespace Microsoft.Matrix.UIComponents
{
    using System;

    public class ToolBarComboBoxButtonEventArgs : EventArgs
    {
        private ComboBoxToolBarButton _button;

        public ToolBarComboBoxButtonEventArgs(ComboBoxToolBarButton button)
        {
            this._button = button;
        }

        public ComboBoxToolBarButton Button
        {
            get
            {
                return this._button;
            }
        }
    }
}

