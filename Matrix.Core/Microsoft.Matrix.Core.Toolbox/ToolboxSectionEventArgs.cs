namespace Microsoft.Matrix.Core.Toolbox
{
    using System;

    public class ToolboxSectionEventArgs : EventArgs
    {
        private ToolboxSection _section;

        public ToolboxSectionEventArgs(ToolboxSection section)
        {
            this._section = section;
        }

        public ToolboxSection Section
        {
            get
            {
                return this._section;
            }
        }
    }
}

