namespace Microsoft.Matrix.Plugins.CodeWizards.Data
{
    using System;
    using System.Windows.Forms;

    internal abstract class ClauseNode : TreeNode
    {
        protected ClauseNode()
        {
        }

        public abstract string[] Tables { get; }
    }
}

