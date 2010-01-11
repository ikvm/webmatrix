namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Windows.Forms;

    public interface IWindowManager
    {
        void ActivateChildForm(MxForm childForm);
        void AddToolWindow(ToolWindow toolWindow, DockStyle dockEdge);
        void AddToolWindow(ToolWindow toolWindow, DockStyle dockEdge, int toolWindowGroupIndex);
        void CloseChildForm(MxForm childForm);
        void ShowChildForm(MxForm childForm);
    }
}

