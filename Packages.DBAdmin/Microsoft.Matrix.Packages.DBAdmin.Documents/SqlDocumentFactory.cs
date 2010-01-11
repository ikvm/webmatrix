namespace Microsoft.Matrix.Packages.DBAdmin.Documents
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.UserInterface;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public sealed class SqlDocumentFactory : IDocumentFactory
    {
        private static Icon largeIcon;
        private static Icon smallIcon;

        Document IDocumentFactory.CreateDocument(DocumentProjectItem projectItem, bool readOnly, DocumentMode mode, DocumentViewType initialView, out DocumentWindow documentWindow, out DesignerHost designerHost)
        {
            if (projectItem == null)
            {
                throw new ArgumentNullException("projectItem");
            }
            Document document = new SqlDocument(projectItem);
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
                return "Create a new SQL Script.";
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
                    largeIcon = new Icon(typeof(SqlDocumentFactory), "SqlFileLarge.ico");
                }
                return largeIcon;
            }
        }

        string IDocumentFactory.Name
        {
            get
            {
                return "SQL Script";
            }
        }

        string IDocumentFactory.OpenFilter
        {
            get
            {
                return "SQL Scripts (*.sql)|*.sql";
            }
        }

        Icon IDocumentFactory.SmallIcon
        {
            get
            {
                if (smallIcon == null)
                {
                    smallIcon = new Icon(typeof(SqlDocumentFactory), "SqlFileSmall.ico");
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

