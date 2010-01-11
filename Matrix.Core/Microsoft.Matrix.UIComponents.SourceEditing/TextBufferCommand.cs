namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.Drawing;

    public class TextBufferCommand : Command
    {
        private object _data;
        private TextBufferLocation _endLocation;
        private Point _point;
        private bool _selecting;
        private TextBufferLocation _startLocation;
        private int _value;

        public TextBufferCommand(int command) : this(command, null, (TextBufferLocation) null, null)
        {
        }

        public TextBufferCommand(int command, TextBufferLocation startLocation) : this(command, startLocation, (TextBufferLocation) null, null)
        {
        }

        public TextBufferCommand(int command, int commandVal) : this(command, null, null, false, Point.Empty, commandVal, null)
        {
        }

        public TextBufferCommand(int command, object data) : this(command, null, null, data)
        {
        }

        public TextBufferCommand(int command, TextBufferLocation startLocation, TextBufferLocation endLocation) : this(command, startLocation, endLocation, null)
        {
        }

        public TextBufferCommand(int command, TextBufferLocation startLocation, bool selecting) : this(command, startLocation, null, selecting, Point.Empty, -1, null)
        {
        }

        public TextBufferCommand(int command, TextBufferLocation startLocation, object data) : this(command, startLocation, null, data)
        {
        }

        public TextBufferCommand(int command, TextBufferLocation startLocation, TextBufferLocation endLocation, object data) : this(command, startLocation, endLocation, false, Point.Empty, -1, data)
        {
        }

        public TextBufferCommand(int command, TextBufferLocation startLocation, bool selecting, Point p) : this(command, startLocation, null, selecting, p, -1, null)
        {
        }

        public TextBufferCommand(int command, TextBufferLocation startLocation, TextBufferLocation endLocation, bool selecting, Point p, int commandVal, object data) : base(typeof(TextBufferCommands), command, new object())
        {
            this._startLocation = startLocation;
            this._endLocation = endLocation;
            this._selecting = selecting;
            this._point = p;
            this._value = commandVal;
            this._data = data;
        }

        public override void UpdateCommandUI()
        {
        }

        public Point CommandPosition
        {
            get
            {
                return this._point;
            }
            set
            {
                this._point = value;
            }
        }

        public int CommandValue
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        public object Data
        {
            get
            {
                return this._data;
            }
        }

        public TextBufferLocation EndLocation
        {
            get
            {
                return this._endLocation;
            }
        }

        public bool IsSelecting
        {
            get
            {
                return this._selecting;
            }
            set
            {
                this._selecting = value;
            }
        }

        public TextBufferLocation StartLocation
        {
            get
            {
                return this._startLocation;
            }
        }
    }
}

