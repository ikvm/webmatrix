namespace Microsoft.Matrix.Core.Projects.FileSystem
{
    using Microsoft.Matrix;
    using Microsoft.Matrix.Core.Projects;
    using System;

    internal sealed class MyComputerProjectItem : RootProjectItem
    {
        public MyComputerProjectItem(Project owningProject) : base(owningProject, "My Computer (" + Environment.MachineName + ")")
        {
            base.SetIconIndex(8);
            base.SetFlags(ProjectItemFlags.SupportsAddItem, false);
            base.SetFlags(ProjectItemFlags.NotDeletable, true);
        }

        protected override void CreateChildItems()
        {
            string[] logicalDrives = Environment.GetLogicalDrives();
            int length = logicalDrives.Length;
            for (int i = 0; i < length; i++)
            {
                switch (Interop.GetDriveType(logicalDrives[i]))
                {
                    case 3:
                    {
                        ProjectItem item = new DirectoryProjectItem(logicalDrives[i], true, false);
                        base.AddChildItem(item);
                        break;
                    }
                    case 4:
                    {
                        ProjectItem item2 = new DirectoryProjectItem(logicalDrives[i], false, true);
                        base.AddChildItem(item2);
                        break;
                    }
                }
            }
            base.CreateChildItems();
        }
    }
}

