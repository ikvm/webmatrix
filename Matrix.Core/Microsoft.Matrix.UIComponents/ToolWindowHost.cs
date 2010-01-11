namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Windows.Forms;

    internal abstract class ToolWindowHost : Form
    {
        private ToolWindowHolder _toolWindowHolder;
        private IToolWindowManager _toolWindowManager;

        public ToolWindowHost(IToolWindowManager toolWindowManager)
        {
            this._toolWindowManager = toolWindowManager;
            base.ShowInTaskbar = false;
            base.MinimizeBox = false;
            base.MaximizeBox = false;
            this._toolWindowHolder = new ToolWindowHolder();
            base.Controls.Add(this._toolWindowHolder);
        }

        public void AddToolWindow(ToolWindow toolWindow)
        {
            this.AddToolWindow(toolWindow, false);
        }

        public void AddToolWindow(ToolWindow toolWindow, bool makeActive)
        {
            this._toolWindowHolder.AddToolWindow(toolWindow, makeActive);
        }

        protected IToolWindowManager ToolWindowManager
        {
            get
            {
                return this._toolWindowManager;
            }
        }
    }
}

