namespace Microsoft.Matrix.UIComponents
{
    using System;

    public interface IStatusBar
    {
        void SetProgress(int percentComplete);
        void SetText(string text);
    }
}

