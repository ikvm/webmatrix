namespace Microsoft.Matrix.Core.Documents.Design
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.ComponentModel.Design;

    public class DocumentDesigner : ComponentDesigner, ICommandHandler
    {
        protected virtual bool HandleCommand(Command command)
        {
            return false;
        }

        bool ICommandHandler.HandleCommand(Command command)
        {
            return this.HandleCommand(command);
        }

        bool ICommandHandler.UpdateCommand(Command command)
        {
            return this.UpdateCommand(command);
        }

        protected virtual bool UpdateCommand(Command command)
        {
            return false;
        }
    }
}

