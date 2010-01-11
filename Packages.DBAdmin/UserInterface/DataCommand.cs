namespace Microsoft.Matrix.Packages.DBAdmin.UserInterface
{
    using System;

    [Flags]
    internal enum DataCommand
    {
        AddItem = 0x20,
        CreateChildren = 1,
        Delete = 0x10,
        Disconnect = 0x40,
        DoubleClick = 2,
        Refresh = 8,
        RightClick = 4
    }
}

