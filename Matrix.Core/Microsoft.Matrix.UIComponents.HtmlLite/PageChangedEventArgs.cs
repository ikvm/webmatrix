namespace Microsoft.Matrix.UIComponents.HtmlLite
{
    using System;

    public class PageChangedEventArgs : EventArgs
    {
        private bool _requiresLayoutUpdate;

        public PageChangedEventArgs(bool requiresLayoutUpdate)
        {
            this._requiresLayoutUpdate = requiresLayoutUpdate;
        }

        public bool RequiresLayoutUpdate
        {
            get
            {
                return this._requiresLayoutUpdate;
            }
        }
    }
}

