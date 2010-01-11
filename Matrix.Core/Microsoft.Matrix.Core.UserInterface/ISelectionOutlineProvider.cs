namespace Microsoft.Matrix.Core.UserInterface
{
    using System;
    using System.Collections;

    public interface ISelectionOutlineProvider
    {
        ICollection GetOutline(object selectedObject);

        string OutlineTitle { get; }
    }
}

