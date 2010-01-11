namespace Microsoft.Matrix.Core.UserInterface
{
    using System;

    public interface ISearchableDocumentView
    {
        bool PerformFind(string searchString, FindReplaceOptions options);
        bool PerformReplace(string searchString, string replaceString, FindReplaceOptions options);

        FindReplaceOptions FindSupport { get; }

        string InitialSearchString { get; }

        FindReplaceOptions ReplaceSupport { get; }
    }
}

