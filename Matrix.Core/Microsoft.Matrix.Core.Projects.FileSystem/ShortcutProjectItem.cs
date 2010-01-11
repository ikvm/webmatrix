namespace Microsoft.Matrix.Core.Projects.FileSystem
{
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.Collections;
    using System.IO;

    internal sealed class ShortcutProjectItem : RootProjectItem
    {
        private string _path;

        public ShortcutProjectItem(Project owningProject, string path) : base(owningProject, path)
        {
            base.SetIconIndex(6);
            base.SetFlags(ProjectItemFlags.IsDropTarget, true);
            this._path = path;
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

        public override string Path
        {
            get
            {
                return this._path;
            }
        }

        public override bool ShowChildrenByDefault
        {
            get
            {
                return false;
            }
        }
    }
}

