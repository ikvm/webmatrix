namespace Microsoft.Matrix.Core.Documents
{
    using System;

    [Flags]
    public enum DocumentViewType
    {
        Code = 4,
        Composite = 0x10,
        Default = 0,
        Design = 2,
        Preview = 8,
        Source = 1
    }
}

