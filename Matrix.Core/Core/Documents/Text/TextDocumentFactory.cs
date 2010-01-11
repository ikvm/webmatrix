namespace Microsoft.Matrix.Core.Documents.Text
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.UserInterface;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public sealed class TextDocumentFactory : IDocumentFactory
    {
        private static Icon largeIcon;
        private static Icon smallIcon;

        Document IDocumentFactory.CreateDocument(DocumentProjectItem projectItem, bool readOnly, DocumentMode mode, DocumentViewType initialView, out DocumentWindow documentWindow, out DesignerHost designerHost)
        {
            if (projectItem == null)
            {
                throw new ArgumentNullException("projectItem");
            }
            Document document = new TextDocument(projectItem);
            designerHost = new DesignerHost(document);
            document.Load(readOnly);
            documentWindow = new TextDocumentWindow(designerHost, document);
            return document;
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
                return "Create a new plain text file.";
            }
        }

        bool IDocumentFactory.CreateUsingTemplate
        {
            get
            {
                return false;
            }
        }

        Icon IDocumentFactory.LargeIcon
        {
            get
            {
                if (largeIcon == null)
                {
                    largeIcon = new Icon(typeof(TextDocumentFactory), "TextFileLarge.ico");
                }
                return largeIcon;
            }
        }

        string IDocumentFactory.Name
        {
            get
            {
                return "Text File";
            }
        }

        string IDocumentFactory.OpenFilter
        {
            get
            {
                return "Text Files (*.txt)|*.txt";
            }
        }

        Icon IDocumentFactory.SmallIcon
        {
            get
            {
                if (smallIcon == null)
                {
                    smallIcon = new Icon(typeof(TextDocumentFactory), "TextFileSmall.ico");
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
                return TemplateFlags.None;
            }
        }
    }
}

