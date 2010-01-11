namespace Microsoft.Matrix.Utility
{
    using System;

    internal sealed class AsyncTaskMessage
    {
        private bool _completed;
        private object _data;
        private int _percentComplete;
        private AsyncTask _task;

        public AsyncTaskMessage(AsyncTask task, object data, int percentComplete, bool completed)
        {
            this._task = task;
            this._data = data;
            this._percentComplete = percentComplete;
            this._completed = completed;
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
                return this._completed;
            }
        }

        public int PercentComplete
        {
            get
            {
                return this._percentComplete;
            }
        }

        public AsyncTask Task
        {
            get
            {
                return this._task;
            }
        }
    }
}

