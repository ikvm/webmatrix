namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Windows.Forms;

    public class MxButton : Button
    {
        public MxButton()
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

