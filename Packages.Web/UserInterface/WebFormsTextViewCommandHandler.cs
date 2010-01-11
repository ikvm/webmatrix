namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.UIComponents;
    using Microsoft.Matrix.UIComponents.SourceEditing;
    using System;

    public class WebFormsTextViewCommandHandler : ICommandHandler, IDisposable
    {
        private ICommandHandler _nextHandler;
        private TextView _view;

        public WebFormsTextViewCommandHandler(TextView view)
        {
            this._view = view;
            this._nextHandler = view.AddCommandHandler(this);
        }

        private void BlockComment(TextBufferSpan span)
        {
            using (TextBufferSpan span2 = this.ShrinkToText(span))
            {
                this._nextHandler.HandleCommand(new TextBufferCommand(7));
                try
                {
                    this._nextHandler.HandleCommand(new TextBufferCommand(4, span2.End, "--%>"));
                    this._nextHandler.HandleCommand(new TextBufferCommand(4, span2.Start, "<%--"));
                    span.Start.MoveTo(span2.Start);
                    TextBufferLocation start = span.Start;
                    start.ColumnIndex -= 4;
                    span.End.MoveTo(span2.End);
                    TextBufferLocation end = span.End;
                    end.ColumnIndex += 4;
                }
                finally
                {
                    this._nextHandler.HandleCommand(new TextBufferCommand(8));
                }
            }
        }

        private void BlockUncomment(TextBufferSpan span)
        {
            using (TextBufferSpan span2 = this.ShrinkToText(span))
            {
                this._nextHandler.HandleCommand(new TextBufferCommand(7));
                try
                {
                    if (((span2.Start.ColumnIndex < (span2.Start.Line.Length - 3)) && (span2.Start.Line.Substring(span2.Start.ColumnIndex, 4) == "<%--")) && ((span2.End.ColumnIndex > 3) && (span2.End.Line.Substring(span2.End.ColumnIndex - 4, 4) == "--%>")))
                    {
                        using (TextBufferLocation location = span2.End.Clone())
                        {
                            using (TextBufferLocation location2 = span2.End.Clone())
                            {
                                location.ColumnIndex -= 4;
                                this._nextHandler.HandleCommand(new TextBufferCommand(3, location, location2));
                                span.End.MoveTo(location2);
                                location.MoveTo(span2.Start);
                                location2.MoveTo(span2.Start);
                                location2.ColumnIndex += 4;
                                this._nextHandler.HandleCommand(new TextBufferCommand(3, location, location2));
                                span.Start.MoveTo(location2);
                            }
                        }
                    }
                }
                finally
                {
                    this._nextHandler.HandleCommand(new TextBufferCommand(8));
                }
            }
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
                switch (command.CommandID)
                {
                    case 0x5c:
                    {
                        TextBufferLocation endLocation = command2.EndLocation;
                        this.BlockComment(new TextBufferSpan(startLocation, endLocation));
                        flag = true;
                        break;
                    }
                    case 0x5d:
                    {
                        TextBufferLocation end = command2.EndLocation;
                        this.BlockUncomment(new TextBufferSpan(startLocation, end));
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)
            {
                flag = this._nextHandler.HandleCommand(command);
            }
            return flag;
        }

        private TextBufferSpan ShrinkToText(TextBufferSpan originalSpan)
        {
            TextBufferSpan span = new TextBufferSpan(originalSpan.Start.Clone(), originalSpan.End.Clone());
            bool flag = true;
            while (flag)
            {
                if (span.Start.ColumnIndex == span.Start.Line.Length)
                {
                    if (span.Start.MoveDown(1) != 1)
                    {
                        break;
                    }
                    span.Start.ColumnIndex = 0;
                    if (span.IsEmpty)
                    {
                        break;
                    }
                }
                flag = char.IsWhiteSpace(span.Start.Line[span.Start.ColumnIndex]);
                if (flag)
                {
                    TextBufferLocation start = span.Start;
                    start.ColumnIndex++;
                }
                if (span.IsEmpty)
                {
                    break;
                }
            }
            if (!span.IsEmpty)
            {
                flag = true;
                while (flag)
                {
                    if (span.End.ColumnIndex == 0)
                    {
                        if (span.End.MoveUp(1) != 1)
                        {
                            break;
                        }
                        span.End.ColumnIndex = span.End.Line.Length;
                        if (span.IsEmpty)
                        {
                            break;
                        }
                    }
                    if (char.IsWhiteSpace(span.End.Line[span.End.ColumnIndex - 1]))
                    {
                        TextBufferLocation end = span.End;
                        end.ColumnIndex--;
                    }
                    if (span.IsEmpty)
                    {
                        break;
                    }
                }
            }
            if (span.IsEmpty)
            {
                span.Start.MoveTo(originalSpan.Start);
                span.End.MoveTo(originalSpan.End);
            }
            return span;
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

