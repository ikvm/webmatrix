namespace Microsoft.Matrix.Packages.DBAdmin.UserInterface
{
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.ComponentModel.Design;
    using System.Windows.Forms;

    internal class DataTreeNode : TreeNode
    {
        private Microsoft.Matrix.Core.Projects.ProjectItem _item;

        protected DataTreeNode(Microsoft.Matrix.Core.Projects.ProjectItem item, int imageIndex) : this(item, imageIndex, imageIndex)
        {
        }

        protected DataTreeNode(Microsoft.Matrix.Core.Projects.ProjectItem item, int imageIndex, int selectedImageIndex)
        {
            this._item = item;
            base.Text = item.Caption;
            this._item.ItemNode = this;
            base.ImageIndex = imageIndex;
            base.SelectedImageIndex = selectedImageIndex;
        }

        internal virtual DataObject GetDataObject(IDesignerHost designerHost)
        {
            return null;
        }

        internal virtual void ProcessCommand(DataCommand command)
        {
        }

        internal bool SupportsCommand(DataCommand command)
        {
            return ((this.Supported & command) != 0);
        }

        public Microsoft.Matrix.Core.Projects.ProjectItem ProjectItem
        {
            get
            {
                return this._item;
            }
        }

        protected virtual DataCommand Supported
        {
            get
            {
                return 0;
            }
        }
    }
}

