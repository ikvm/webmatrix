namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using Microsoft.Matrix.UIComponents;
    using System;

    public class BlockIndentTextViewCommandHandler : ICommandHandler, IDisposable
    {
        private ICommandHandler _nextHandler;
        private TextView _view;

        public BlockIndentTextViewCommandHandler(TextView view)
        {
            this._view = view;
            this._nextHandler = view.AddCommandHandler(this);
        }

        public void Dispose()
        {
            this._view.RemoveCommandHandler(this._nextHandler);
            this._view = null;
        }

        public bool HandleCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup == typeof(TextBufferCommands))
            {
                TextBufferCommand command2 = (TextBufferCommand) command;
                TextBufferLocation startLocation = command2.StartLocation;
                if (command.CommandID == 20)
                {
                    this._nextHandler.HandleCommand(new TextBufferCommand(7));
                    this._nextHandler.HandleCommand(command);
                    TextLine previous = startLocation.Line.Previous;
                    int length = 0;
                    while (length < previous.Length)
                    {
                        if ((previous[length] != '\t') && (previous[length] != ' '))
                        {
                            break;
                        }
                        length++;
                    }
                    if (length != 0)
                    {
                        this._nextHandler.HandleCommand(new TextBufferCommand(4, startLocation, previous.Substring(0, length)));
                    }
                    this._nextHandler.HandleCommand(new TextBufferCommand(8));
                    flag = true;
                }
            }
            if (!flag)
            {
                flag = this._nextHandler.HandleCommand(command);
            }
            return flag;
        }

        public bool UpdateCommand(Command command)
        {
            return this._nextHandler.UpdateCommand(command);
        }
    }
}

