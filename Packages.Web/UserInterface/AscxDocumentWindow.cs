namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Packages.Web.Html.WebForms;
    using System;

    public class AscxDocumentWindow : WebFormsDocumentWindow
    {
        public AscxDocumentWindow(IServiceProvider provider, Document document, DocumentViewType initialView) : base(provider, document, initialView)
        {
        }

        protected override HtmlDesignView CreateDesignView()
        {
            return new WebFormsDesignView(base.ServiceProvider, WebFormsEditorMode.UserControl);
        }

        protected override HtmlPreviewView CreatePreviewView()
        {
            return new WebFormsPreviewView(base.ServiceProvider, WebFormsEditorMode.UserControl);
        }
    }
}

