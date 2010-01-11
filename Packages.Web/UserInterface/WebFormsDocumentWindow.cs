namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.UserInterface;
    using Microsoft.Matrix.Packages.Web;
    using Microsoft.Matrix.UIComponents;
    using System;

    public class WebFormsDocumentWindow : HtmlDocumentWindow
    {
        public WebFormsDocumentWindow(IServiceProvider provider, Document document, DocumentViewType initialView) : base(provider, document, initialView)
        {
        }

        protected virtual CodeBehindView CreateCodeBehindView()
        {
            return new WebFormsCodeBehindView(base.ServiceProvider);
        }

        protected override HtmlDesignView CreateDesignView()
        {
            return new WebFormsDesignView(base.ServiceProvider);
        }

        protected override void CreateDocumentViews()
        {
            if (!WebPackage.Instance.DesignModeEnabled)
            {
                HtmlPreviewView viewControl = this.CreatePreviewView();
                WebFormsAllView view2 = this.CreateWebFormsAllView("Source");
                base.AddDocumentView(view2, true);
                base.AddDocumentView(viewControl, false);
            }
            else
            {
                HtmlDesignView view3 = this.CreateDesignView();
                SourceView view4 = this.CreateSourceView();
                CodeBehindView view5 = this.CreateCodeBehindView();
                WebFormsAllView view6 = this.CreateWebFormsAllView("All");
                view5.LineNumbersVisible = false;
                view4.LineNumbersVisible = false;
                base.AddDocumentView(view3, false);
                base.AddDocumentView(view4, false);
                base.AddDocumentView(view5, false);
                base.AddDocumentView(view6, false);
            }
        }

        protected override HtmlPreviewView CreatePreviewView()
        {
            return new WebFormsPreviewView(base.ServiceProvider);
        }

        protected override HtmlSourceView CreateSourceView()
        {
            return new WebFormsSourceView(base.ServiceProvider);
        }

        protected virtual WebFormsAllView CreateWebFormsAllView(string viewName)
        {
            return new WebFormsAllView(base.ServiceProvider, viewName);
        }

        protected override bool UpdateCommand(Command command)
        {
            bool flag = false;
            if (((command.CommandGroup == typeof(GlobalCommands)) && (command.CommandID >= 200)) && (command.CommandID <= 0xcb))
            {
                flag = base.UpdateCommand(command);
                IMultiViewDocumentWindow window = this;
                WebFormsDesignView currentView = window.CurrentView as WebFormsDesignView;
                if ((currentView != null) && currentView.InTemplateMode)
                {
                    command.Enabled = false;
                    flag = true;
                }
            }
            if (!flag)
            {
                flag = base.UpdateCommand(command);
            }
            return flag;
        }
    }
}

