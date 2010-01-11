namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Windows.Forms;

    public class MxGroupBox : GroupBox
    {
        public MxGroupBox()
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

