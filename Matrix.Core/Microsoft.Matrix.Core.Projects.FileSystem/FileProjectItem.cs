namespace Microsoft.Matrix.Core.Projects.FileSystem
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

