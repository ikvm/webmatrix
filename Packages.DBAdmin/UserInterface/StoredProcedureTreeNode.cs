namespace Microsoft.Matrix.Packages.DBAdmin.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.Windows.Forms;

    internal class StoredProcedureTreeNode : DataTreeNode
    {
        public StoredProcedureTreeNode(ProjectItem item) : base(item, 8)
        {
        }

        internal override void ProcessCommand(DataCommand command)
        {
            base.ProcessCommand(command);
            switch (command)
            {
                case DataCommand.DoubleClick:
                {
                    DocumentProjectItem projectItem = base.ProjectItem as DocumentProjectItem;
                    if (projectItem != null)
                    {
                        try
                        {
                            projectItem.Project.OpenProjectItem(projectItem, false, DocumentViewType.Default);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    return;
                }
                case (DataCommand.DoubleClick | DataCommand.CreateChildren):
                case DataCommand.RightClick:
                    return;

                case DataCommand.Refresh:
                    ((StoredProcedureGroupTreeNode) base.Parent).RefreshChildren();
                    return;

                case DataCommand.Delete:
                    if ((MessageBox.Show(string.Format("Are you sure you want to delete the stored procedure '{0}'? This operation cannot be undone.", base.Text), "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes) && base.ProjectItem.Delete())
                    {
                        base.Remove();
                    }
                    break;

                case DataCommand.AddItem:
                    ((StoredProcedureGroupTreeNode) base.Parent).AddItem();
                    break;
            }
        }

        protected override DataCommand Supported
        {
            get
            {
                return (DataCommand.AddItem | DataCommand.Delete | DataCommand.Refresh | DataCommand.RightClick | DataCommand.DoubleClick);
            }
        }
    }
}

