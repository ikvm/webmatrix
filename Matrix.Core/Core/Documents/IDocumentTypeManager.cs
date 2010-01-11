namespace Microsoft.Matrix.Core.Documents
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    public interface IDocumentTypeManager
    {
        DocumentType GetDocumentType(string extension);
        bool IsRegisteredDocumentType(string extension);

        ICollection CreatableDocumentTypes { get; }

        ImageList DocumentIcons { get; }

        ICollection DocumentTypes { get; }

        string OpenFilters { get; }
    }
}

