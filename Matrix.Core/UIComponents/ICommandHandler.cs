namespace Microsoft.Matrix.UIComponents
{
    using System;

    public interface ICommandHandler
    {
        bool HandleCommand(Command command);
        bool UpdateCommand(Command command);
    }
}

