namespace Microsoft.Matrix.Core.UserInterface
{
    using System;

    [Flags]
    public enum FindReplaceOptions
    {
        All = 0x10,
        InSelection = 8,
        MatchCase = 1,
        None = 0,
        SearchUp = 4,
        WholeWord = 2
    }
}

