namespace Microsoft.Matrix.UIComponents.SourceEditing
{
    using System;

    public class TextViewEventArgs
    {
        private TextView _view;

        public TextViewEventArgs(TextView view)
        {
            this._view = view;
        }

        public TextView View
        {
            get
            {
                return this._view;
            }
        }
    }
}

