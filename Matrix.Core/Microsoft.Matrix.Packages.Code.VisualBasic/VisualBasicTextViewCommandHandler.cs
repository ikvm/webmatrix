namespace Microsoft.Matrix.Packages.Code.VisualBasic
{
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;

    public class VisualBasicTextViewCommandHandler : ICommandHandler, IDisposable
    {
        private ICommandHandler _nextHandler;
        private TextView _view;
        private static TextBufferCommand beginBatchUndo = new TextBufferCommand(7);
        private static TextBufferCommand endBatchUndo = new TextBufferCommand(8);

        public VisualBasicTextViewCommandHandler(TextView view)
        {
            this._view = view;
            this._nextHandler = view.AddCommandHandler(this);
        }

        private void BlockComment(TextBufferSpan span)
        {
            this._nextHandler.HandleCommand(new TextBufferCommand(7));
            try
            {
                using (TextBufferLocation location = span.Start.Clone())
                {
                    int lineIndex = span.End.LineIndex;
                    if (span.End.ColumnIndex == 0)
                    {
                        lineIndex--;
                    }
                    int num2 = 0x7fffffff;
                    while (location.LineIndex <= lineIndex)
                    {
                        int num3 = 0;
                        TextLine line = location.Line;
                        while ((num3 < line.Length) && char.IsWhiteSpace(line[num3]))
                        {
                            num3++;
                        }
                        num2 = Math.Min(num2, num3);
                        if (location.MoveDown(1) == 0)
                        {
                            break;
                        }
                    }
                    location.MoveTo(span.Start);
                    while (location.LineIndex <= lineIndex)
                    {
                        location.ColumnIndex = num2;
                        this._nextHandler.HandleCommand(new TextBufferCommand(4, location, "' "));
                        if (location.MoveDown(1) == 0)
                        {
                            return;
                        }
                    }
                }
            }
            finally
            {
                this._nextHandler.HandleCommand(new TextBufferCommand(8));
            }
        }

        private void BlockUncomment(TextBufferSpan span)
        {
            this._nextHandler.HandleCommand(new TextBufferCommand(7));
            try
            {
                using (TextBufferLocation location = span.Start.Clone())
                {
                    while (location.LineIndex <= span.End.LineIndex)
                    {
                        int num = 0;
                        TextLine line = location.Line;
                        while ((num < line.Length) && char.IsWhiteSpace(line[num]))
                        {
                            num++;
                        }
                        if ((num < line.Length) && (line[num] == '\''))
                        {
                            location.ColumnIndex = num;
                            this._nextHandler.HandleCommand(new TextBufferCommand(2, location));
                            if ((num < line.Length) && (line[num] == ' '))
                            {
                                this._nextHandler.HandleCommand(new TextBufferCommand(2, location));
                            }
                        }
                        if (location.MoveDown(1) == 0)
                        {
                            return;
                        }
                    }
                }
            }
            finally
            {
                this._nextHandler.HandleCommand(new TextBufferCommand(8));
            }
        }

        private int CountLeadingSpace(TextLine s)
        {
            int num = 0;
            int num2 = 0;
            int tabSize = this._view.TabSize;
            while (num < s.Length)
            {
                if (s[num] == '\t')
                {
                    num2 = ((num2 + tabSize) / tabSize) * tabSize;
                }
                else
                {
                    if (s[num] != ' ')
                    {
                        return num2;
                    }
                    num2++;
                }
                num++;
            }
            return num2;
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
                TextBufferLocation endLocation = command2.EndLocation;
                switch (command.CommandID)
                {
                    case 0x5c:
                        this.BlockComment(new TextBufferSpan(startLocation, endLocation));
                        flag = true;
                        break;

                    case 0x5d:
                        this.BlockUncomment(new TextBufferSpan(startLocation, endLocation));
                        flag = true;
                        break;
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
            bool flag = false;
            if (command.CommandGroup == typeof(TextBufferCommands))
            {
                switch (command.CommandID)
                {
                    case 0x5c:
                        command.Enabled = this._view.SelectionExists;
                        flag = true;
                        break;

                    case 0x5d:
                        command.Enabled = this._view.SelectionExists;
                        flag = true;
                        break;
                }
            }
            if (!flag)
            {
                flag = this._nextHandler.UpdateCommand(command);
            }
            return flag;
        }
    }
}

