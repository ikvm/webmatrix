namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;

    public class ShowContextMenuEventArgs : EventArgs
    {
        private bool _keyboard;
        private Point _location;

        public ShowContextMenuEventArgs(Point location, bool keyboard)
        {
            this._location = location;
            this._keyboard = keyboard;
        }

        public Point Location
        {
            get
            {
                return this._location;
            }
        }

        public bool UsingKeyboard
        {
            get
            {
                return this._keyboard;
            }
        }
    }
}

