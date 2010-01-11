namespace Microsoft.Matrix.Packages.DBAdmin.UserInterface
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.DBAdmin.Projects;
    using System;
    using System.Windows.Forms;

    internal class DatabaseProjectTreeNode : DataTreeNode
    {
        public DatabaseProjectTreeNode(ProjectItem item) : base(item, 6)
        {
            base.Nodes.Add(new DummyTreeNode());
        }

        internal override void ProcessCommand(DataCommand command)
        {
            base.ProcessCommand(command);
            DataCommand command2 = command;
            if (command2 != DataCommand.CreateChildren)
            {
                if (command2 != DataCommand.Disconnect)
                {
                    return;
                }
            }
            else
            {
                base.Nodes.Clear();
                foreach (ProjectItem item in base.ProjectItem.ChildItems)
                {
                    if (item is TableGroupProjectItem)
                    {
                        base.Nodes.Add(new TableGroupTreeNode(item));
                    }
                    else if (item is StoredProcedureGroupProjectItem)
                    {
                        base.Nodes.Add(new StoredProcedureGroupTreeNode(item));
                    }
                }
                return;
            }
            if (MessageBox.Show("Are you sure you want to disconnect '" + base.Text + "'?", "Disconnect database", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Project project = base.ProjectItem.Project;
                IServiceProvider provider = project;
                ((IProjectManager) provider.GetService(typeof(IProjectManager))).CloseProject(project);
            }
        }

        protected override DataCommand Supported
        {
            get
            {
                return (DataCommand.Disconnect | DataCommand.CreateChildren);
            }
        }
    }
}

