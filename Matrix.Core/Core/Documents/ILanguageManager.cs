namespace Microsoft.Matrix.Core.Documents
{
    using System;
    using System.Collections;

    public interface ILanguageManager
    {
        DocumentLanguage GetDocumentLanguage(string name);

        DocumentLanguage DefaultCodeLanguage { get; }

        DocumentLanguage DefaultTextLanguage { get; }

        ICollection DocumentLanguages { get; }
    }
}

