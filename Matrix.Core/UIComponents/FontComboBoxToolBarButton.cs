namespace Microsoft.Matrix.UIComponents
{
    using System;

    public class FontComboBoxToolBarButton : ComboBoxToolBarButton
    {
        public FontComboBoxToolBarButton() : base(null)
        {
        }

        protected override ToolBarComboBox CreateComboBoxControl()
        {
            return new FontComboBox();
        }
    }
}

