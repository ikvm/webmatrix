namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;
    using System.Drawing;

    internal interface IContentElementHost
    {
        void AddTrackedElement(VisualElement element);
        void ClearTrackedElements();

        Color BackColor { get; }

        System.Drawing.Font Font { get; }

        Color ForeColor { get; }
    }
}

