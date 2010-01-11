namespace Microsoft.Matrix.Packages.Web.Projects.Ftp
{
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.Collections;

    internal class DirectoryProjectItem : FolderProjectItem
    {
        public DirectoryProjectItem(string caption) : base(caption)
        {
            base.SetFlags(ProjectItemFlags.IsDropTarget, true);
        }

        protected override void CreateChildItems()
        {
            ICollection directoryListing = ((FtpProject) this.Project).Connection.GetDirectoryListing(this.Path);
            if ((directoryListing != null) && (directoryListing.Count != 0))
            {
                try
                {
                    ArrayList list = new ArrayList();
                    foreach (FtpEntry entry in directoryListing)
                    {
                        if (entry.EntryType == FtpEntryType.File)
                        {
                            ProjectItem item = new FileProjectItem(entry.FileName);
                            list.Add(item);
                        }
                        else if (!entry.FileName.StartsWith("_vti"))
                        {
                            ProjectItem item2 = new DirectoryProjectItem(entry.FileName);
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
            }
            base.CreateChildItems();
        }
    }
}

