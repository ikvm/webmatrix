namespace Microsoft.Matrix.UIComponents
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class ToolWindow : MxUserControl
    {
        private System.Drawing.Icon _icon;
        private string _shortText;
        private static readonly object EventEnsureActive = new object();

        internal event EventHandler EnsureActive
        {
            add
            {
                base.Events.AddHandler(EventEnsureActive, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventEnsureActive, value);
            }
        }

        public ToolWindow()
        {
        }

        public ToolWindow(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            base.Dock = DockStyle.Fill;
        }

        protected void Activate()
        {
            EventHandler handler = (EventHandler) base.Events[EventEnsureActive];
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
                base.Focus();
            }
        }

        public System.Drawing.Icon Icon
        {
            get
            {
                return this._icon;
            }
            set
            {
                this._icon = value;
            }
        }

        public string ShortText
        {
            get
            {
                if (this._shortText == null)
                {
                    return this.Text;
                }
                return this._shortText;
            }
            set
            {
                this._shortText = value;
            }
        }
    }
}

