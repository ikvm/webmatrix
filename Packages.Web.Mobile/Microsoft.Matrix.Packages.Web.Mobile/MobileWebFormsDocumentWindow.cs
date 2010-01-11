namespace Microsoft.Matrix.Packages.Web.Mobile
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Packages.Web.UserInterface;
    using System;

    public class MobileWebFormsDocumentWindow : AspxDocumentWindow
    {
        public MobileWebFormsDocumentWindow(IServiceProvider provider, Document document, DocumentViewType initialView) : base(provider, document, initialView)
        {
        }

        protected override HtmlDesignView CreateDesignView()
        {
            return new MobileWebFormsDesignView(base.ServiceProvider);
        }

        protected override HtmlPreviewView CreatePreviewView()
        {
            return new MobileWebFormsPreviewView(base.ServiceProvider);
        }

        protected override HtmlSourceView CreateSourceView()
        {
            return new MobileWebFormsSourceView(base.ServiceProvider);
        }

        protected override WebFormsAllView CreateWebFormsAllView(string viewName)
        {
            return new MobileWebFormsAllView(base.ServiceProvider, viewName);
        }
    }
}

