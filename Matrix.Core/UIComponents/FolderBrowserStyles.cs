namespace Microsoft.Matrix.UIComponents
{
    using System;

    [Flags]
    public enum FolderBrowserStyles
    {
        BrowseForComputer = 0x1000,
        BrowseForEverything = 0x4000,
        BrowseForPrinter = 0x2000,
        RestrictToDomain = 2,
        RestrictToFileSystem = 1,
        RestrictToSubfolders = 8,
        ShowTextBox = 0x10
    }
}

