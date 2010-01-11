namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Windows.Forms;

    public class MxCheckBox : CheckBox
    {
        public MxCheckBox()
        {
            if (MxTheme.IsAppThemed)
            {
                base.FlatStyle = FlatStyle.System;
            }
            else
            {
                base.FlatStyle = FlatStyle.Popup;
            }
        }

        private bool ShouldSerializeFlatStyle()
        {
            return false;
        }
    }
}

