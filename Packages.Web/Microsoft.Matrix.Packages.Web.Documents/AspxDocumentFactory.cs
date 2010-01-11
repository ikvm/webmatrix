namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.Packages.Web.Designer;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Web.UI;

    public sealed class AspxDocumentFactory : IDocumentFactory
    {
        private Icon _largeIcon;
        private static Assembly _mobileAssembly;
        private static bool _mobileAssemblyLoadFailed;
        private static readonly string _mobileAssemblyName = "System.Web.Mobile";
        private Icon _smallIcon;

        Document IDocumentFactory.CreateDocument(DocumentProjectItem projectItem, bool readOnly, DocumentMode mode, DocumentViewType initialView, out DocumentWindow documentWindow, out DesignerHost designerHost)
        {
            if (projectItem == null)
            {
                throw new ArgumentNullException("projectItem");
            }
            if (initialView == DocumentViewType.Default)
            {
                initialView = WebPackage.Instance.WebDefaultView;
                if (initialView == DocumentViewType.Source)
                {
                    initialView = DocumentViewType.Composite;
                }
            }
            AspxDocument document = new AspxDocument(projectItem);
            designerHost = new WebFormsDesignerHost(document);
            document.Load(readOnly);
            documentWindow = null;
            Page component = null;
            if (document.DocumentDirective.Inherits.ToLower().IndexOf("mobile") < 0)
            {
                component = new Page();
                component.ID = "Page";
                documentWindow = new AspxDocumentWindow(designerHost, document, initialView);
            }
            else
            {
                if (!_mobileAssemblyLoadFailed)
                {
                    if (_mobileAssembly == null)
                    {
                        _mobileAssembly = Assembly.LoadWithPartialName(_mobileAssemblyName);
                    }
                    if (_mobileAssembly != null)
                    {
                        document.Dispose();
                        document = new AspxDocument(projectItem);
                        component = (Page) Activator.CreateInstance(_mobileAssembly.GetType("System.Web.UI.MobileControls.MobilePage", true, false));
                        Type type = Type.GetType("Microsoft.Matrix.Packages.Web.Mobile.MobileDesignerHost, Microsoft.Matrix.Packages.Web.Mobile");
                        designerHost = (DesignerHost) Activator.CreateInstance(type, new object[] { document });
                        document.Load(readOnly);
                        Type type3 = Type.GetType("Microsoft.Matrix.Packages.Web.Mobile.MobileWebFormsDocumentWindow, Microsoft.Matrix.Packages.Web.Mobile");
                        documentWindow = (DocumentWindow) Activator.CreateInstance(type3, new object[] { designerHost, document, initialView });
                    }
                    else
                    {
                        _mobileAssemblyLoadFailed = true;
                    }
                }
                if (_mobileAssemblyLoadFailed)
                {
                    document.Dispose();
                    TextDocument document2 = new TextDocument(projectItem);
                    designerHost = new DesignerHost(document2);
                    ((IMxUIService) designerHost.GetService(typeof(IMxUIService))).ReportError("Microsoft Mobile Internet Toolkit is required for design-time editing of mobile pages.\r\nPlease visit 'http://www.asp.net/mobile/default.aspx' for more information.\r\nThe page will be opened in the text editor instead.", "Mobile Pages are not enabled.", true);
                    document2.Load(readOnly);
                    documentWindow = new TextDocumentWindow(designerHost, document2);
                    return document2;
                }
            }
            IDesignerHost host = designerHost;
            host.Container.Add(component, "Page");
            component.DesignerInitialize();
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
                return "Create a new ASP.NET page.";
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
                    this._largeIcon = new Icon(typeof(AspxDocumentFactory), "AspxFileLarge.ico");
                }
                return this._largeIcon;
            }
        }

        string IDocumentFactory.Name
        {
            get
            {
                return "ASP.NET Page";
            }
        }

        string IDocumentFactory.OpenFilter
        {
            get
            {
                return "ASP.NET Pages (*.aspx)|*.aspx";
            }
        }

        Icon IDocumentFactory.SmallIcon
        {
            get
            {
                if (this._smallIcon == null)
                {
                    this._smallIcon = new Icon(typeof(AspxDocumentFactory), "AspxFileSmall.ico");
                }
                return this._smallIcon;
            }
        }

        DocumentViewType IDocumentFactory.SupportedViews
        {
            get
            {
                return (DocumentViewType.Code | DocumentViewType.Design | DocumentViewType.Source);
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
                return (TemplateFlags.HasCode | TemplateFlags.HasMacros);
            }
        }
    }
}

