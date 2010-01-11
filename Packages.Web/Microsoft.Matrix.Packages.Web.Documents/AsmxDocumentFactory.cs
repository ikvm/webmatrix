namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public sealed class AsmxDocumentFactory : IDocumentFactory
    {
        private Icon _largeIcon;
        private Icon _smallIcon;

        Document IDocumentFactory.CreateDocument(DocumentProjectItem projectItem, bool readOnly, DocumentMode mode, DocumentViewType initialView, out DocumentWindow documentWindow, out DesignerHost designerHost)
        {
            if (projectItem == null)
            {
                throw new ArgumentNullException("projectItem");
            }
            Document document = new AsmxDocument(projectItem);
            designerHost = new DesignerHost(document);
            document.Load(readOnly);
            documentWindow = new AsmxDocumentWindow(designerHost, document);
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
                return "Create a new XML Web Service.";
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
                if (this._largeIcon == null)
                {
                    this._largeIcon = new Icon(typeof(AscxDocumentFactory), "AsmxFileLarge.ico");
                }
                return this._largeIcon;
            }
        }

        string IDocumentFactory.Name
        {
            get
            {
                return "XML Web Service";
            }
        }

        string IDocumentFactory.OpenFilter
        {
            get
            {
                return "XML Web Services (*.asmx)|*.asmx";
            }
        }

        Icon IDocumentFactory.SmallIcon
        {
            get
            {
                if (this._smallIcon == null)
                {
                    this._smallIcon = new Icon(typeof(AspxDocumentFactory), "AsmxFileSmall.ico");
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
                return (TemplateFlags.CodeRequiresClassName | TemplateFlags.CodeRequiresNamespace | TemplateFlags.HasCode | TemplateFlags.HasMacros);
            }
        }
    }
}

