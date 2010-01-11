namespace Microsoft.Matrix.Core.Documents
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.UserInterface;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public interface IDocumentFactory
    {
        Document CreateDocument(DocumentProjectItem projectItem, bool readOnly, DocumentMode mode, DocumentViewType initialView, out DocumentWindow documentWindow, out DesignerHost designerHost);

        bool CanCreateNew { get; }

        string CreateNewDescription { get; }

        bool CreateUsingTemplate { get; }

        Icon LargeIcon { get; }

        string Name { get; }

        string OpenFilter { get; }

        Icon SmallIcon { get; }

        DocumentViewType SupportedViews { get; }

        string TemplateCategory { get; }

        Microsoft.Matrix.Core.Documents.TemplateFlags TemplateFlags { get; }
    }
}

