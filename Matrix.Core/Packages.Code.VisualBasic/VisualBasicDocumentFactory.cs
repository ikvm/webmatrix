namespace Microsoft.Matrix.Packages.Code.VisualBasic
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Documents.Code;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.UserInterface;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public sealed class VisualBasicDocumentFactory : IDocumentFactory
    {
        private Icon _smallIcon;

        Document IDocumentFactory.CreateDocument(DocumentProjectItem projectItem, bool readOnly, DocumentMode mode, DocumentViewType initialView, out DocumentWindow documentWindow, out DesignerHost designerHost)
        {
            if (projectItem == null)
            {
                throw new ArgumentNullException("projectItem");
            }
            Document document = new CodeDocument(projectItem, VisualBasicCodeDocumentLanguage.Instance);
            designerHost = new DesignerHost(document);
            document.Load(readOnly);
            documentWindow = new CodeDocumentWindow(designerHost, document);
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
                return "Visual Basic Class";
            }
        }

        string IDocumentFactory.OpenFilter
        {
            get
            {
                return "Visual Basic Code Files (*.vb)|*.vb";
            }
        }

        Icon IDocumentFactory.SmallIcon
        {
            get
            {
                if (this._smallIcon == null)
                {
                    this._smallIcon = new Icon(typeof(VisualBasicDocumentFactory), "VisualBasicFileSmall.ico");
                }
                return this._smallIcon;
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

