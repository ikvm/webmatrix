namespace Microsoft.Matrix.Packages.DBAdmin.Documents
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.DBAdmin.UserInterface;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public sealed class StoredProcedureDocumentFactory : IDocumentFactory
    {
        private static Icon _smallIcon;

        Document IDocumentFactory.CreateDocument(DocumentProjectItem projectItem, bool readOnly, DocumentMode mode, DocumentViewType initialView, out DocumentWindow documentWindow, out DesignerHost designerHost)
        {
            if (projectItem == null)
            {
                throw new ArgumentNullException("projectItem");
            }
            Document document = new StoredProcedureDocument(projectItem);
            designerHost = new DesignerHost(document);
            document.Load(readOnly);
            documentWindow = new StoredProcedureDocumentWindow(designerHost, document);
            return document;
        }

        bool IDocumentFactory.CanCreateNew
        {
            get
            {
                return false;
            }
        }

        string IDocumentFactory.CreateNewDescription
        {
            get
            {
                return string.Empty;
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
                return null;
            }
        }

        string IDocumentFactory.Name
        {
            get
            {
                return "Database Stored Procedure";
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
                if (_smallIcon == null)
                {
                    _smallIcon = new Icon(typeof(StoredProcedureDocumentFactory), "StoredProcedureFileSmall.ico");
                }
                return _smallIcon;
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

