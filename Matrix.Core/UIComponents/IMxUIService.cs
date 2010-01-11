namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public interface IMxUIService
    {
        void ReportError(Exception exception, string caption, bool isWarning);
        void ReportError(string error, string caption, bool isWarning);
        DialogResult ShowDialog(Form dialog);
        void ShowMessage(string text, string caption);
        DialogResult ShowMessage(string text, string caption, MessageBoxIcon icon, MessageBoxButtons buttons);
        DialogResult ShowMessage(string text, string caption, MessageBoxIcon icon, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton);

        IWin32Window DialogOwner { get; }

        Font UIFont { get; }
    }
}

