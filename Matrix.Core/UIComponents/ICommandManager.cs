namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public interface ICommandManager
    {
        void AddGlobalCommandHandler(ICommandHandler handler);
        bool InvokeCommand(Type commandGroup, int commandID);
        void RemoveGlobalCommandHandler(ICommandHandler handler);
        void ResumeCommandUpdate();
        void ShowContextMenu(Type commandGroup, int menuID, ICommandHandlerWithContext commandHandler, object context, Control referenceControl, Point location);
        void SuspendCommandUpdate();
        void UpdateActiveUICommandHandler(ICommandHandler activeUICommandHandler, ICommandHandler activeUIContextCommandHandler);
        void UpdateCommands(bool immediate);
    }
}

