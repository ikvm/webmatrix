namespace Microsoft.Matrix.Core.UserInterface
{
    using Microsoft.Matrix.UIComponents;
    using System;

    public class OptionsPage : MxUserControl
    {
        private bool _initialVisible;

        public OptionsPage()
        {
        }

        public OptionsPage(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._initialVisible = true;
        }

        public virtual void CommitChanges()
        {
        }

        protected virtual void OnInitialVisibleChanged(EventArgs e)
        {
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (this._initialVisible && base.Visible)
            {
                this._initialVisible = false;
                this.OnInitialVisibleChanged(e);
            }
        }

        public virtual bool IsDirty
        {
            get
            {
                return false;
            }
        }

        public virtual string OptionsPath
        {
            get
            {
                return "(General)";
            }
        }
    }
}

