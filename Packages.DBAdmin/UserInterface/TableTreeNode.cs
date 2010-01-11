namespace Microsoft.Matrix.Packages.DBAdmin.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using System;
    using System.ComponentModel.Design;
    using System.Windows.Forms;

    internal class TableTreeNode : DataTreeNode
    {
        public TableTreeNode(ProjectItem item) : base(item, 7)
        {
        }

        internal override DataObject GetDataObject(IDesignerHost designerHost)
        {
            DataObject obj2 = new DataObject();
            obj2.SetData(DBAdminPackage.MxDataTableDataFormat, ((TableProjectItem) base.ProjectItem).GetTable());
            return obj2;
        }

        internal override void ProcessCommand(DataCommand command)
        {
            base.ProcessCommand(command);
            switch (command)
            {
                case DataCommand.CreateChildren:
                case (DataCommand.DoubleClick | DataCommand.CreateChildren):
                case DataCommand.RightClick:
                    return;

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
                case DataCommand.Refresh:
                    ((TableGroupTreeNode) base.Parent).RefreshChildren();
                    return;

                case DataCommand.Delete:
                    if ((MessageBox.Show(string.Format("Are you sure you want to delete the table '{0}'? This operation cannot be undone.", base.Text), "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes) && base.ProjectItem.Delete())
                    {
                        base.Remove();
                    }
                    break;

                case DataCommand.AddItem:
                    ((TableGroupTreeNode) base.Parent).AddItem();
                    break;
            }
        }

        protected override DataCommand Supported
        {
            get
            {
                return (DataCommand.AddItem | DataCommand.Delete | DataCommand.Refresh | DataCommand.RightClick | DataCommand.DoubleClick | DataCommand.CreateChildren);
            }
        }
    }
}

