namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Windows.Forms;

    public class MxLabel : Label
    {
        public MxLabel()
        {
            base.FlatStyle = FlatStyle.System;
        }

        private bool ShouldSerializeFlatStyle()
        {
            return false;
        }
    }
}

