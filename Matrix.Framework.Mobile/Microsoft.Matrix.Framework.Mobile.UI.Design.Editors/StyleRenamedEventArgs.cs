namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using System;
    using System.Security.Permissions;

    [SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
    internal sealed class StyleRenamedEventArgs : EventArgs
    {
        private string _newName;
        private string _oldName;

        internal StyleRenamedEventArgs(string oldName, string newName)
        {
            this._oldName = oldName;
            this._newName = newName;
        }

        internal string NewName
        {
            get
            {
                return this._newName;
            }
        }

        internal string OldName
        {
            get
            {
                return this._oldName;
            }
        }
    }
}

