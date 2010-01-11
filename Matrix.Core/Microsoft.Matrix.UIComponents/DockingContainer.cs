namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class DockingContainer : Panel
    {
        private Splitter _splitter;
        private ArrayList _toolWindowHosts;
        private IToolWindowManager _toolWindowManager;

        public void AddToolWindow(ToolWindow toolWindow, int toolWindowGroupIndex)
        {
            ToolWindowHost host;

            if (toolWindowGroupIndex != -1)
            {
                if (this.ToolWindowHosts.Count <= toolWindowGroupIndex)
                {
                    toolWindowGroupIndex = this.ToolWindowHosts.Count - 1;
                }
                host = (ToolWindowHost) this._toolWindowHosts[toolWindowGroupIndex];
            }
            else
            {
               host = new DockedToolWindowHost(this._toolWindowManager);

               if (this.ToolWindowHosts.Count != 0)
                {
                    Splitter splitter = new Splitter();
                    splitter.Size = new Size(4, 4);
                    splitter.Dock = host.Dock = this.ContainedControlDockStyle;
                    base.Controls.Add(splitter);
                }
                this._toolWindowHosts.Add(host);
                base.Controls.Add(host);
                host.Visible = true;
            }
            host.AddToolWindow(toolWindow);
        }

        internal void SetToolWindowManager(IToolWindowManager toolWindowManager)
        {
            this._toolWindowManager = toolWindowManager;
        }

        public Splitter AssociatedSplitter
        {
            get
            {
                return this._splitter;
            }
            set
            {
                this._splitter = value;
            }
        }

        private DockStyle ContainedControlDockStyle
        {
            get
            {
                switch (this.Dock)
                {
                    case DockStyle.Top:
                    case DockStyle.Bottom:
                        return DockStyle.Left;

                    case DockStyle.Left:
                    case DockStyle.Right:
                        return DockStyle.Top;
                }
                return DockStyle.None;
            }
        }

        private ArrayList ToolWindowHosts
        {
            get
            {
                if (this._toolWindowHosts == null)
                {
                    this._toolWindowHosts = new ArrayList();
                }
                return this._toolWindowHosts;
            }
        }
    }
}

