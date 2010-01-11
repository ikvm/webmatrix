namespace Microsoft.Matrix.Core.Documents
{
    using Microsoft.Matrix.Core.Projects;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    public interface IDocumentManager
    {
        event ActiveDocumentEventHandler ActiveDocumentChanged;

        event DocumentEventHandler DocumentClosed;

        event DocumentEventHandler DocumentOpened;

        event DocumentEventHandler DocumentRenamed;

        void CloseDocument(Document document, bool discardChanges);
        DocumentProjectItem CreateDocument(Project project, FolderProjectItem parentItem, bool fixedParentItem);
        Document GetDocument(DocumentProjectItem projectItem);
        Document OpenDocument(DocumentProjectItem projectItem, bool readOnly, DocumentViewType initialView);
        void PrintDocument(IPrintableDocument document);
        void PrintPreviewDocument(IPrintableDocument document);
        bool SaveDocument(Document document, bool saveAs);

        Document ActiveDocument { get; }

        ICollection OpenDocuments { get; }
    }
}

