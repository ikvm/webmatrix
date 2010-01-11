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

    public sealed class ImageDocumentFactory : IDocumentFactory
    {
        private Icon _smallIcon;

        Document IDocumentFactory.CreateDocument(DocumentProjectItem projectItem, bool readOnly, DocumentMode mode, DocumentViewType initialView, out DocumentWindow documentWindow, out DesignerHost designerHost)
        {
            if (projectItem == null)
            {
                throw new ArgumentNullException("projectItem");
            }
            Document document = new ImageDocument(projectItem);
            designerHost = new DesignerHost(document);
            document.Load(readOnly);
            documentWindow = new ImageDocumentWindow(designerHost, document);
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
                throw new NotSupportedException();
            }
        }

        bool IDocumentFactory.CreateUsingTemplate
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        Icon IDocumentFactory.LargeIcon
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        string IDocumentFactory.Name
        {
            get
            {
                return "Image Document";
            }
        }

        string IDocumentFactory.OpenFilter
        {
            get
            {
                return "Image Files (*.bmp,*.gif,*.jpg,*.png,*.tif,*.tiff)|*.bmp;*.gif;*.jpg;*.png;*.tif;*.tiff";
            }
        }

        Icon IDocumentFactory.SmallIcon
        {
            get
            {
                if (this._smallIcon == null)
                {
                    this._smallIcon = new Icon(typeof(ImageDocumentFactory), "ImageFileSmall.ico");
                }
                return this._smallIcon;
            }
        }

        DocumentViewType IDocumentFactory.SupportedViews
        {
            get
            {
                return DocumentViewType.Default;
            }
        }

        string IDocumentFactory.TemplateCategory
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        TemplateFlags IDocumentFactory.TemplateFlags
        {
            get
            {
                throw new NotSupportedException();
            }
        }
    }
}

