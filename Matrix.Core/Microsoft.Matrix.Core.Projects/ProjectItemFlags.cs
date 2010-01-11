namespace Microsoft.Matrix.Core.Projects
{
    using System;

    [Flags]
    public enum ProjectItemFlags
    {
        IsDragSource = 8,
        IsDropTarget = 0x10,
        NonExpandable = 1,
        NotDeletable = 2,
        SupportsAddItem = 4
    }
}

