namespace Microsoft.Matrix.Utility
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public sealed class AsyncTaskManager
    {
        private static Control uiController;

        internal static void PostMessage(AsyncTaskMessage message)
        {
            if (uiController != null)
            {
                uiController.BeginInvoke(new PostMessageCallback(AsyncTaskManager.UIThreadCallback), new object[] { message });
            }
        }

        public static void RegisterUIThread(Control uiController)
        {
            AsyncTaskManager.uiController = uiController;
            IntPtr handle = uiController.Handle;
        }

        private static void UIThreadCallback(AsyncTaskMessage message)
        {
            message.Task.RaiseResultPostedEvent(message.Data, message.PercentComplete, message.IsComplete);
        }

        private delegate void PostMessageCallback(AsyncTaskMessage message);
    }
}

