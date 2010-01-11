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
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Web.UI;

    public sealed class AscxDocumentFactory : IDocumentFactory
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
            documentWindow = null;
            AscxDocument document = new AscxDocument(projectItem);
            designerHost = new WebFormsDesignerHost(document);
            document.Load(readOnly);
            Page page = null;
            UserControl child = null;
            if (document.DocumentDirective.Inherits.ToLower().IndexOf("mobile") < 0)
            {
                page = new Page();
                child = new UserControl();
                child.ID = "Page";
                documentWindow = new AscxDocumentWindow(designerHost, document, initialView);
                page.Site = new AscxPageSite(designerHost, page);
                page.DesignerInitialize();
                page.Controls.Add(child);
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
                        document = new AscxDocument(projectItem);
                        Type type = _mobileAssembly.GetType("System.Web.UI.MobileControls.MobilePage");
                        Type type2 = _mobileAssembly.GetType("System.Web.UI.MobileControls.MobileUserControl");
                        Type type3 = _mobileAssembly.GetType("System.Web.UI.MobileControls.Form");
                        page = (Page) Activator.CreateInstance(type);
                        child = (UserControl) Activator.CreateInstance(type2);
                        Control control2 = (Control) Activator.CreateInstance(type3);
                        Type type4 = Type.GetType("Microsoft.Matrix.Packages.Web.Mobile.MobileDesignerHost, Microsoft.Matrix.Packages.Web.Mobile");
                        designerHost = (DesignerHost) Activator.CreateInstance(type4, new object[] { document });
                        document.Load(readOnly);
                        Type type5 = Type.GetType("Microsoft.Matrix.Packages.Web.Mobile.MobileWebFormsDocumentWindow, Microsoft.Matrix.Packages.Web.Mobile");
                        documentWindow = (DocumentWindow) Activator.CreateInstance(type5, new object[] { designerHost, document, initialView });
                        page.Site = new AscxPageSite(designerHost, page);
                        page.DesignerInitialize();
                        page.Controls.Add(control2);
                        control2.Controls.Add(child);
                    }
                    else
                    {
                        _mobileAssemblyLoadFailed = true;
                    }
                }
                if (_mobileAssemblyLoadFailed)
                {
                    document.Dispose();
                    Document document2 = new TextDocument(projectItem);
                    designerHost = new DesignerHost(document2);
                    ((IMxUIService) designerHost.GetService(typeof(IMxUIService))).ReportError("Microsoft Mobile Internet Toolkit is required for design-time editing of mobile user controls.\r\nPlease visit 'http://www.asp.net/mobile/default.aspx' for more information.\r\nThe user control will be opened in the text editor instead.", "Mobile User Controls are not enabled.", true);
                    document2.Load(readOnly);
                    documentWindow = new TextDocumentWindow(designerHost, document2);
                    return document2;
                }
            }
            IDesignerHost host = designerHost;
            host.Container.Add(child, "UserControl");
            child.DesignerInitialize();
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
                return "Create a new ASP.NET user control.";
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
                    this._largeIcon = new Icon(typeof(AscxDocumentFactory), "AscxFileLarge.ico");
                }
                return this._largeIcon;
            }
        }

        string IDocumentFactory.Name
        {
            get
            {
                return "ASP.NET User Control";
            }
        }

        string IDocumentFactory.OpenFilter
        {
            get
            {
                return "ASP.NET User Controls (*.ascx)|*.ascx";
            }
        }

        Icon IDocumentFactory.SmallIcon
        {
            get
            {
                if (this._smallIcon == null)
                {
                    this._smallIcon = new Icon(typeof(AspxDocumentFactory), "AscxFileSmall.ico");
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

        private sealed class AscxPageSite : ISite, IServiceProvider
        {
            private IComponent _component;
            private IDesignerHost _designerHost;

            public AscxPageSite(IDesignerHost designerHost, Page page)
            {
                this._designerHost = designerHost;
                this._component = page;
            }

            public object GetService(Type serviceType)
            {
                return this._designerHost.GetService(serviceType);
            }

            public IComponent Component
            {
                get
                {
                    return this._component;
                }
            }

            public IContainer Container
            {
                get
                {
                    return this._designerHost.Container;
                }
            }

            public bool DesignMode
            {
                get
                {
                    return true;
                }
            }

            public string Name
            {
                get
                {
                    return "Page";
                }
                set
                {
                }
            }
        }
    }
}

