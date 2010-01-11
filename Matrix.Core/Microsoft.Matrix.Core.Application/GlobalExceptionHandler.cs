namespace Microsoft.Matrix.Core.Application
{
    using Microsoft.Matrix.Core.UserInterface;
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class GlobalExceptionHandler : IDisposable
    {
        private IServiceProvider _serviceProvider;

        public GlobalExceptionHandler(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        private bool CanIgnoreException(Exception e)
        {
            bool flag = false;
            if (this._serviceProvider != null)
            {
                IApplicationIdentity service = (IApplicationIdentity) this._serviceProvider.GetService(typeof(IApplicationIdentity));
                if (service != null)
                {
                    flag = service.OnUnhandledException(e);
                }
            }
            return flag;
        }

        public void Initialize()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(this.OnThreadException);
        }

        private void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (!this.CanIgnoreException(e.Exception))
            {
                DialogResult cancel = DialogResult.Cancel;
                try
                {
                    cancel = this.ReportException(e.Exception);
                }
                catch
                {
                    try
                    {
                        this.ReportFatalException(e.Exception);
                    }
                    finally
                    {
                        cancel = DialogResult.Abort;
                    }
                }
                if (cancel == DialogResult.Abort)
                {
                    Application.Exit();
                }
            }
        }

        private DialogResult ReportException(Exception e)
        {
            IUIService service = null;
            if (this._serviceProvider != null)
            {
                service = (IUIService) this._serviceProvider.GetService(typeof(IUIService));
            }
            Microsoft.Matrix.Core.UserInterface.ThreadExceptionDialog form = new Microsoft.Matrix.Core.UserInterface.ThreadExceptionDialog(this._serviceProvider, e);
            if (service != null)
            {
                return service.ShowDialog(form);
            }
            return form.ShowDialog();
        }

        private void ReportFatalException(Exception e)
        {
            string title;
            IApplicationIdentity service = null;
            if (this._serviceProvider != null)
            {
                service = (IApplicationIdentity) this._serviceProvider.GetService(typeof(IApplicationIdentity));
            }
            if (service != null)
            {
                title = service.Title;
            }
            else
            {
                title = "Fatal Application Error";
            }
            MessageBox.Show("There was an unhandled exception. Clicking OK will end the application.\n" + e.ToString(), title, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        void IDisposable.Dispose()
        {
            Application.ThreadException -= new ThreadExceptionEventHandler(this.OnThreadException);
            this._serviceProvider = null;
        }
    }
}

