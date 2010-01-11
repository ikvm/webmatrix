namespace Microsoft.Matrix.Packages.Web.Utility.Formatting
{
    using System;

    [Flags]
    internal enum FormattingFlags
    {
        AllowPartialTags = 0x40,
        Comment = 0x20,
        Inline = 1,
        NoEndTag = 4,
        NoIndent = 2,
        None = 0,
        PreserveContent = 8,
        Xml = 0x10
    }
}

