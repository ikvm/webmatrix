namespace Microsoft.Matrix.Core.Projects.FileSystem
{
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.Collections;
    using System.IO;

    internal class DirectoryProjectItem : FolderProjectItem
    {
        public DirectoryProjectItem(string caption) : this(caption, false, false)
        {
        }

        public DirectoryProjectItem(string caption, bool isDrive, bool isShare) : base(caption)
        {
            if (isDrive)
            {
                base.SetIconIndex(4);
                base.SetFlags(ProjectItemFlags.NotDeletable, true);
            }
            else if (isShare)
            {
                base.SetIconIndex(3);
                base.SetFlags(ProjectItemFlags.NotDeletable, true);
            }
            base.SetFlags(ProjectItemFlags.IsDropTarget, true);
        }

        protected override void CreateChildItems()
        {
            DirectoryInfo info = new DirectoryInfo(this.Path);
            try
            {
                int num;
                DirectoryInfo[] directories = info.GetDirectories();
                int length = directories.Length;
                FileInfo[] files = info.GetFiles();
                int num3 = files.Length;
                ArrayList list = new ArrayList(length + num3);
                for (num = 0; num < length; num++)
                {
                    if ((directories[num].Attributes & (FileAttributes.System | FileAttributes.Hidden)) == 0)
                    {
                        ProjectItem item = new DirectoryProjectItem(directories[num].Name);
                        list.Add(item);
                    }
                }
                for (num = 0; num < num3; num++)
                {
                    if ((files[num].Attributes & (FileAttributes.System | FileAttributes.Hidden)) == 0)
                    {
                        ProjectItem item2 = new FileProjectItem(files[num].Name);
                        list.Add(item2);
                    }
                }
                list.Sort(this.Project.ProjectItemComparer);
                foreach (ProjectItem item3 in list)
                {
                    base.AddChildItem(item3);
                }
            }
            catch
            {
            }
            base.CreateChildItems();
        }
    }
}

