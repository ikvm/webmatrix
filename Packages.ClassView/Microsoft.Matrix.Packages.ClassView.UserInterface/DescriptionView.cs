namespace Microsoft.Matrix.Packages.ClassView.UserInterface
{
    using Microsoft.Matrix.Core.Services;
    using Microsoft.Matrix.Packages.ClassView.Core;
    using Microsoft.Matrix.Packages.ClassView.Documents;
    using Microsoft.Matrix.Packages.ClassView.Projects;
    using Microsoft.Matrix.UIComponents.HtmlLite;
    using System;

    internal sealed class DescriptionView : PageViewer
    {
        private TypeDocument _currentDocument;
        private bool _enableSearches;
        private IServiceProvider _serviceProvider;

        public DescriptionView(IServiceProvider serviceProvider) : this(serviceProvider, false)
        {
        }

        internal DescriptionView(IServiceProvider serviceProvider, bool enableSearches)
        {
            this._serviceProvider = serviceProvider;
            this._enableSearches = enableSearches;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._serviceProvider = null;
            }
            base.Dispose(disposing);
        }

        protected override void OnElementClick(ElementEventArgs e)
        {
            base.OnElementClick(e);
            if (this._enableSearches)
            {
                Type type = e.Element.UserData as Type;
                if (type != null)
                {
                    IClassViewService service = (IClassViewService) this._serviceProvider.GetService(typeof(IClassViewService));
                    if (service != null)
                    {
                        service.ShowType(type);
                    }
                    return;
                }
                TypeSearchTask searchTask = e.Element.UserData as TypeSearchTask;
                if (searchTask != null)
                {
                    if (ClassViewToolWindow.Instance != null)
                    {
                        ClassViewToolWindow.Instance.PerformSearch(searchTask);
                    }
                    return;
                }
            }
            string userData = e.Element.UserData as string;
            if (userData != null)
            {
                IWebBrowsingService service2 = (IWebBrowsingService) this._serviceProvider.GetService(typeof(IWebBrowsingService));
                if (service2 != null)
                {
                    service2.BrowseUrl(userData);
                }
            }
        }

        public void SetCurrentDocument(TypeDocument document)
        {
            this._currentDocument = document;
        }

        public void SetCurrentItem(TypeDocumentItem item)
        {
            Page descriptionPage = null;
            if (item == null)
            {
                descriptionPage = this._currentDocument.DescriptionPage;
            }
            else
            {
                descriptionPage = item.DescriptionPage;
            }
            if (descriptionPage == null)
            {
                ClassViewProjectData projectData = ((ClassViewProject) this._currentDocument.ProjectItem.Project).ProjectData;
                PageBuilder builder = null;
                if (item == null)
                {
                    builder = new TypePageBuilder(projectData, this._currentDocument);
                    descriptionPage = (ClassViewPage) builder.CreatePage(typeof(ClassViewPage));
                    this._currentDocument.DescriptionPage = descriptionPage;
                }
                else
                {
                    builder = new MemberPageBuilder(projectData, this._currentDocument, item);
                    descriptionPage = (ClassViewPage) builder.CreatePage(typeof(MemberPage));
                    item.DescriptionPage = descriptionPage;
                }
            }
            base.Page = descriptionPage;
        }
    }
}

