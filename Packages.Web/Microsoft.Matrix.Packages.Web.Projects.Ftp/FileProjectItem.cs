namespace Microsoft.Matrix.Packages.Web.Projects.Ftp
{
    using Microsoft.Matrix.Core.Projects;
    using System;

    internal class FileProjectItem : DocumentProjectItem
    {
        public FileProjectItem(string caption) : base(caption)
        {
            base.SetFlags(ProjectItemFlags.IsDragSource, true);
        }
    }
}

