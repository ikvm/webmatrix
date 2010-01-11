namespace Microsoft.Matrix.Framework.Mobile.UI.Design.Editors
{
    using System;
    using System.Security.Permissions;

    [SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
    internal sealed class StyleDeletedEventArgs : EventArgs
    {
        private string _name;

        internal StyleDeletedEventArgs(string name)
        {
            this._name = name;
        }

        internal string Name
        {
            get
            {
                return this._name;
            }
        }
    }
}

