namespace Microsoft.Matrix.Packages.ClassView.UserInterface
{
    using Microsoft.Matrix.Packages.ClassView.Core;
    using System;

    public sealed class ClassViewToolWindow : ClassViewToolWindowBase
    {
        internal static ClassViewToolWindow Instance;

        public ClassViewToolWindow(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (Instance == this))
            {
                Instance = null;
            }
            base.Dispose(disposing);
        }

        internal override void PerformSearch(TypeSearchTask searchTask)
        {
            if (ClassSearchToolWindow.Instance != null)
            {
                ClassSearchToolWindow.Instance.PerformSearch(searchTask);
            }
            else
            {
                base.PerformSearch(searchTask);
            }
        }
    }
}

