namespace Microsoft.Matrix.Core.Documents.Code
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.UserInterface;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public sealed class CodeDocumentFactory : IDocumentFactory
    {
        private static Icon largeIcon;
        private static Icon smallIcon;

        Document IDocumentFactory.CreateDocument(DocumentProjectItem projectItem, bool readOnly, DocumentMode mode, DocumentViewType initialView, out DocumentWindow documentWindow, out DesignerHost designerHost)
        {
            throw new NotSupportedException("Invalid use of CodeDocumentFactory. This class only exists to represent a arbitrary code file.");
        }

        bool IDocumentFactory.CanCreateNew
        {
            get
            {
                return true;
            }
        }

        string IDocumentFactory.CreateNewDescription
        {
            get
            {
                return "Create a new class.";
            }
        }

        bool IDocumentFactory.CreateUsingTemplate
        {
            get
            {
                return true;
            }
        }

        Icon IDocumentFactory.LargeIcon
        {
            get
            {
                if (largeIcon == null)
                {
                    largeIcon = new Icon(typeof(CodeDocumentFactory), "CodeFileLarge.ico");
                }
                return largeIcon;
            }
        }

        string IDocumentFactory.Name
        {
            get
            {
                return "Class";
            }
        }

        string IDocumentFactory.OpenFilter
        {
            get
            {
                return string.Empty;
            }
        }

        Icon IDocumentFactory.SmallIcon
        {
            get
            {
                if (smallIcon == null)
                {
                    smallIcon = new Icon(typeof(CodeDocumentFactory), "CodeFileSmall.ico");
                }
                return smallIcon;
            }
        }

        DocumentViewType IDocumentFactory.SupportedViews
        {
            get
            {
                return DocumentViewType.Source;
            }
        }

        string IDocumentFactory.TemplateCategory
        {
            get
            {
                return string.Empty;
            }
        }

        TemplateFlags IDocumentFactory.TemplateFlags
        {
            get
            {
                return (TemplateFlags.IsCode | TemplateFlags.CodeRequiresClassName | TemplateFlags.CodeRequiresNamespace | TemplateFlags.HasCode | TemplateFlags.HasMacros);
            }
        }
    }
}

