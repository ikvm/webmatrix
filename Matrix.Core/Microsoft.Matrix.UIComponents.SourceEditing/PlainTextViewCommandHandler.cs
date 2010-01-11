namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Collections;

    public class PlainTextViewCommandHandler : ICommandHandler, IDisposable
    {
        private TextBuffer _buffer;
        private ArrayList _newLineTextList;
        private TextView _view;

        public PlainTextViewCommandHandler(TextView view, TextBuffer buffer)
        {
            this._view = view;
            this._buffer = buffer;
            this._newLineTextList = new ArrayList(2);
            this._newLineTextList.Add(new char[0]);
            this._newLineTextList.Add(new char[0]);
        }

        public void Dispose()
        {
            this._view = null;
        }

        public bool HandleCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup == typeof(TextBufferCommands))
            {
                TextBufferCommand command2 = (TextBufferCommand) command;
                if (!this._view.ReadOnly)
                {
                    char data;
                    TextBufferLocation startLocation;
                    switch (command.CommandID)
                    {
                        case 1:
                            data = (char) command2.Data;
                            if (data > '\x001f')
                            {
                                this._buffer.InsertChar(command2.StartLocation, data);
                            }
                            flag = true;
                            break;

                        case 2:
                            this._buffer.DeleteChar(command2.StartLocation);
                            flag = true;
                            break;

                        case 3:
                            this._buffer.DeleteText(new TextBufferSpan(command2.StartLocation, command2.EndLocation));
                            flag = true;
                            break;

                        case 4:
                            this._buffer.InsertText(command2.StartLocation, (string) command2.Data);
                            flag = true;
                            break;

                        case 5:
                            this._buffer.Undo(command2.StartLocation);
                            flag = true;
                            break;

                        case 6:
                            this._buffer.Redo(command2.StartLocation);
                            flag = true;
                            break;

                        case 7:
                            this._buffer.BeginBatchUndo();
                            flag = true;
                            break;

                        case 8:
                            this._buffer.EndBatchUndo();
                            flag = true;
                            break;

                        case 9:
                            data = (char) command2.Data;
                            if (data > '\x001f')
                            {
                                this._buffer.ReplaceChar(command2.StartLocation, data);
                            }
                            flag = true;
                            break;

                        case 20:
                            this._buffer.InsertText(command2.StartLocation, this._newLineTextList);
                            flag = true;
                            break;

                        case 70:
                            startLocation = ((TextBufferCommand) command).StartLocation;
                            ((TextBufferCommand) command).CommandValue = this.InsertTab(startLocation);
                            flag = true;
                            break;

                        case 0x47:
                            startLocation = ((TextBufferCommand) command).StartLocation;
                            ((TextBufferCommand) command).CommandValue = this.RemoveTab(startLocation);
                            flag = true;
                            break;
                    }
                }
            }
            if (!flag)
            {
                flag = this._view.HandleViewCommand(command);
            }
            return flag;
        }

        private int InsertTab(TextBufferLocation location)
        {
            if (this._view.ConvertTabsToSpaces)
            {
                int num = this._view.TabSize - (location.ColumnIndex % this._view.TabSize);
                char[] chArray = new char[num];
                for (int i = 0; i < num; i++)
                {
                    chArray[i] = ' ';
                }
                ArrayList textList = new ArrayList();
                textList.Add(chArray);
                this._buffer.InsertText(location, textList);
                return num;
            }
            this._buffer.InsertChar(location, '\t');
            return 1;
        }

        private int RemoveTab(TextBufferLocation location)
        {
            if (this._view.ConvertTabsToSpaces)
            {
                TextBufferLocation start = this._buffer.CreateTextBufferLocation(location);
                TextLine line = start.Line;
                int index = start.ColumnIndex - 1;
                int num2 = 0;
                if (index > -1)
                {
                    bool flag = line.Data[index] != ' ';
                    while (((index % this._view.TabSize) != 0) && !flag)
                    {
                        index--;
                        flag = line.Data[index] != ' ';
                    }
                    if (flag)
                    {
                        index++;
                    }
                    start.ColumnIndex = index;
                    num2 = location.ColumnIndex - index;
                    this._buffer.DeleteText(new TextBufferSpan(start, location));
                }
                start.Dispose();
                return num2;
            }
            if (location.ColumnIndex > 0)
            {
                int num3 = location.ColumnIndex - 1;
                if (location.Line.Data[num3] == '\t')
                {
                    location.ColumnIndex--;
                    this._buffer.DeleteChar(location);
                    return 1;
                }
            }
            return 0;
        }

        public bool UpdateCommand(Command command)
        {
            bool flag = false;
            if (command.CommandGroup == typeof(TextBufferCommands))
            {
                TextBufferCommand command1 = (TextBufferCommand) command;
                switch (command.CommandID)
                {
                    case 5:
                        command.Enabled = this._buffer.CanUndo;
                        flag = true;
                        break;

                    case 6:
                        command.Enabled = this._buffer.CanRedo;
                        flag = true;
                        break;
                }
            }
            if (!flag)
            {
                flag = this._view.UpdateViewCommand(command);
            }
            return flag;
        }
    }
}

