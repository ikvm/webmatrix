namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web;
    using System;

    public class HtmlDocumentWindow : MultiViewDocumentWindow
    {
        public HtmlDocumentWindow(IServiceProvider provider, Document document, DocumentViewType initialView) : base(provider, document)
        {
            if (initialView == DocumentViewType.Default)
            {
                initialView = DocumentViewType.Design;
            }
            IDocumentView viewByType = ((IMultiViewDocumentWindow) this).GetViewByType(initialView);
            if (viewByType != null)
            {
                ((IMultiViewDocumentWindow) this).ActivateView(viewByType);
            }
        }

        protected virtual HtmlDesignView CreateDesignView()
        {
            return new HtmlDesignView(base.ServiceProvider);
        }

        protected override void CreateDocumentViews()
        {
            if (!WebPackage.Instance.DesignModeEnabled)
            {
                HtmlPreviewView viewControl = this.CreatePreviewView();
                SourceView view2 = this.CreateSourceView();
                base.AddDocumentView(view2, true);
                base.AddDocumentView(viewControl, false);
            }
            else
            {
                HtmlDesignView view3 = this.CreateDesignView();
                SourceView view4 = this.CreateSourceView();
                view4.LineNumbersVisible = false;
                base.AddDocumentView(view3, false);
                base.AddDocumentView(view4, false);
            }
        }

        protected virtual HtmlPreviewView CreatePreviewView()
        {
            return new HtmlPreviewView(base.ServiceProvider);
        }

        protected virtual HtmlSourceView CreateSourceView()
        {
            return new HtmlSourceView(base.ServiceProvider);
        }
    }
}

