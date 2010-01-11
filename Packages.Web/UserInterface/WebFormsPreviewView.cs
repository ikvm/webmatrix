namespace Microsoft.Matrix.Packages.Web.UserInterface
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Packages.Web.Documents;
    using Microsoft.Matrix.Packages.Web.Html;
    using Microsoft.Matrix.Packages.Web.Html.WebForms;
    using System;

    public class WebFormsPreviewView : HtmlPreviewView
    {
        private WebFormsEditorMode _editorMode;

        public WebFormsPreviewView(IServiceProvider serviceProvider) : this(serviceProvider, WebFormsEditorMode.Page)
        {
        }

        public WebFormsPreviewView(IServiceProvider serviceProvider, WebFormsEditorMode editorMode) : base(serviceProvider)
        {
            this._editorMode = editorMode;
        }

        protected override HtmlEditor CreateEditor()
        {
            return new WebFormsEditor(base.ServiceProvider, this._editorMode);
        }

        protected override void LoadFromDocument(HtmlDocument document)
        {
            WebFormsEditor editor = base.Editor as WebFormsEditor;
            IDocumentDesignerHost service = (IDocumentDesignerHost) base.ServiceProvider.GetService(typeof(IDocumentDesignerHost));
            foreach (RegisterDirective directive in ((WebFormsDocument) service.Document).RegisterDirectives)
            {
                string tagPrefix = directive.TagPrefix;
                if (tagPrefix.Length > 0)
                {
                    editor.RegisterNamespace(tagPrefix);
                }
            }
            string html = ((WebFormsDocument) document).Html;
            if (html != null)
            {
                base.Editor.LoadHtml(html, document.ProjectItem.Url);
            }
        }
    }
}

