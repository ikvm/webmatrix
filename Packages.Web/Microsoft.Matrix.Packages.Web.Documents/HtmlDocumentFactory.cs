namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public sealed class HtmlDocumentFactory : IDocumentFactory
    {
        private Icon _largeIcon;
        private Icon _smallIcon;

        Document IDocumentFactory.CreateDocument(DocumentProjectItem projectItem, bool readOnly, DocumentMode mode, DocumentViewType initialView, out DocumentWindow documentWindow, out DesignerHost designerHost)
        {
            if (projectItem == null)
            {
                throw new ArgumentNullException("projectItem");
            }
            Document document = new HtmlDocument(projectItem);
            designerHost = new DesignerHost(document);
            document.Load(readOnly);
            if (initialView == DocumentViewType.Default)
            {
                initialView = WebPackage.Instance.WebDefaultView;
            }
            documentWindow = new HtmlDocumentWindow(designerHost, document, initialView);
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
                return "Create a new HTML page.";
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
                    this._largeIcon = new Icon(typeof(HtmlDocumentFactory), "HtmlFileLarge.ico");
                }
                return this._largeIcon;
            }
        }

        string IDocumentFactory.Name
        {
            get
            {
                return "HTML Page";
            }
        }

        string IDocumentFactory.OpenFilter
        {
            get
            {
                return "HTML Files (*.htm,*.html)|*.htm;*.html";
            }
        }

        Icon IDocumentFactory.SmallIcon
        {
            get
            {
                if (this._smallIcon == null)
                {
                    this._smallIcon = new Icon(typeof(HtmlDocumentFactory), "HtmlFileSmall.ico");
                }
                return this._smallIcon;
            }
        }

        DocumentViewType IDocumentFactory.SupportedViews
        {
            get
            {
                return (DocumentViewType.Design | DocumentViewType.Source);
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

