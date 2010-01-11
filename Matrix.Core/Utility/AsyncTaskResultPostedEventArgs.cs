namespace Microsoft.Matrix.Utility
{
    using System;

    public sealed class AsyncTaskResultPostedEventArgs : EventArgs
    {
        private object _data;
        private bool _isComplete;
        private int _percentComplete;

        internal AsyncTaskResultPostedEventArgs(object data, int percentComplete, bool isComplete)
        {
            this._data = data;
            this._percentComplete = percentComplete;
            this._isComplete = isComplete;
        }

        public object Data
        {
            get
            {
                return this._data;
            }
        }

        public bool IsComplete
        {
            get
            {
                return this._isComplete;
            }
        }

        public int PercentComplete
        {
            get
            {
                return this._percentComplete;
            }
        }
    }
}

