namespace Microsoft.Matrix.Packages.ClassView.UserInterface
{
    using System;

    public sealed class ClassSearchToolWindow : ClassViewToolWindowBase
    {
        internal static ClassSearchToolWindow Instance;

        public ClassSearchToolWindow(IServiceProvider serviceProvider) : base(serviceProvider, true)
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
    }
}

