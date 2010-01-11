namespace Microsoft.Matrix.Utility
{
    using System;
    using System.Threading;
    using System.Windows.Forms;

    public abstract class AsyncTask
    {
        private bool _isBusy;
        private int _isCanceled;
        private AsyncTaskResultPostedEventHandler _resultPostedHandler;

        protected AsyncTask()
        {
        }

        public void Cancel()
        {
            Interlocked.Increment(ref this._isCanceled);
        }

        private void InvokePerformTask()
        {
            try
            {
                this.PerformTask();
            }
            catch (Exception)
            {
            }
        }

        protected abstract void PerformTask();
        protected void PostResults(object data, int percentComplete, bool completed)
        {
            if (this._resultPostedHandler != null)
            {
                AsyncTaskManager.PostMessage(new AsyncTaskMessage(this, data, percentComplete, completed));
            }
        }

        internal void RaiseResultPostedEvent(object data, int percentComplete, bool completed)
        {
            this._resultPostedHandler(this, new AsyncTaskResultPostedEventArgs(data, percentComplete, completed));
        }

        public void Start(AsyncTaskResultPostedEventHandler handler)
        {
            this._resultPostedHandler = handler;
            this._isBusy = true;
            try
            {
                new MethodInvoker(this.InvokePerformTask).BeginInvoke(null, null);
            }
            catch (Exception)
            {
            }
            finally
            {
                this._isBusy = false;
            }
        }

        public void StartSynchronous(AsyncTaskResultPostedEventHandler handler)
        {
            this._resultPostedHandler = handler;
            this._isBusy = true;
            try
            {
                this.InvokePerformTask();
            }
            catch (Exception)
            {
            }
            finally
            {
                this._isBusy = false;
            }
        }

        public bool IsBusy
        {
            get
            {
                return this._isBusy;
            }
        }

        public bool IsCanceled
        {
            get
            {
                return (this._isCanceled != 0);
            }
        }
    }
}

