namespace Microsoft.Matrix.Packages.DBAdmin.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using System;

    internal class TableGroupTreeNode : DataTreeNode
    {
        public TableGroupTreeNode(ProjectItem item) : base(item, 9, 10)
        {
            base.Nodes.Add(new DummyTreeNode());
        }

        internal void AddItem()
        {
            ProjectItem item = ((FolderProjectItem) base.ProjectItem).AddDocument();
            if (item != null)
            {
                if ((base.Nodes.Count != 1) || !(base.Nodes[0] is DummyTreeNode))
                {
                    base.Nodes.Add(new TableTreeNode(item));
                }
                IDocumentManager service = (IDocumentManager) ((IServiceProvider) base.ProjectItem.Project).GetService(typeof(IDocumentManager));
                if (service != null)
                {
                    service.OpenDocument((DocumentProjectItem) item, false, DocumentViewType.Design);
                }
                if (!base.IsExpanded)
                {
                    base.Expand();
                }
            }
        }

        internal override void ProcessCommand(DataCommand command)
        {
            base.ProcessCommand(command);
            DataCommand command2 = command;
            switch (command2)
            {
                case DataCommand.CreateChildren:
                    base.Nodes.Clear();
                    foreach (ProjectItem item in base.ProjectItem.ChildItems)
                    {
                        base.Nodes.Add(new TableTreeNode(item));
                    }
                    return;

                case DataCommand.Refresh:
                    this.RefreshChildren();
                    return;
            }
            if (command2 == DataCommand.AddItem)
            {
                this.AddItem();
            }
        }

        internal void RefreshChildren()
        {
            bool isExpanded = base.IsExpanded;
            base.ProjectItem.Refresh();
            this.ProcessCommand(DataCommand.CreateChildren);
            if (isExpanded)
            {
                base.Expand();
            }
        }

        protected override DataCommand Supported
        {
            get
            {
                return (DataCommand.AddItem | DataCommand.Refresh | DataCommand.CreateChildren);
            }
        }
    }
}

