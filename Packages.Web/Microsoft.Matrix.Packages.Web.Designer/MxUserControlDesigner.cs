namespace Microsoft.Matrix.Packages.Web.Designer
{
    using Microsoft.Matrix.Core.Designer;
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.Web.Documents;
    using Microsoft.Matrix.UIComponents;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Web.UI;
    using System.Web.UI.Design;

    internal sealed class MxUserControlDesigner : UserControlDesigner
    {
        private DesignerVerb _editDocumentVerb;
        private DesignerVerb _refreshVerb;
        private bool _regenerateDesignTimeHtml;
        private string _tagName;
        private string _tagPrefix;
        private const string DesignTimeHtmlTemplate = "<div style=\"border:solid 1px buttonshadow\">\r\n                <div style=\"font:messagebox;color:buttontext;background-color:buttonface;border-bottom: solid 1px buttonshadow;padding:4px\">\r\n                    <span style=\"font-weight:bold\">UserControl</span> - {0}\r\n                </div>\r\n                <div style=\"padding:2px\">{1}</div>\r\n              </div>";

        private DocumentProjectItem GetAssociatedProjectItem()
        {
            DocumentProjectItem item = null;
            IDocumentDesignerHost service = (IDocumentDesignerHost) this.GetService(typeof(IDocumentDesignerHost));
            WebFormsDocument document = (WebFormsDocument) service.Document;
            RegisterDirective directive = document.RegisterDirectives[this._tagPrefix, this._tagName];
            if (directive != null)
            {
                Project project = document.ProjectItem.Project;
                string sourceFile = directive.SourceFile;
                string documentPath = document.DocumentPath;
                string path = project.CombineRelativePath(documentPath, sourceFile);
                item = project.ParsePath(path, false) as DocumentProjectItem;
            }
            return item;
        }

        public override string GetDesignTimeHtml()
        {
            if (this._tagName == null)
            {
                return base.GetDesignTimeHtml();
            }
            DocumentProjectItem associatedProjectItem = this.GetAssociatedProjectItem();
            string designTimeHtml = null;
            if (associatedProjectItem != null)
            {
                UserControlDesignTimeHtmlGenerator instance = UserControlDesignTimeHtmlGenerator.Instance;
                bool forceRegenerate = this._regenerateDesignTimeHtml;
                this._regenerateDesignTimeHtml = false;
                designTimeHtml = instance.GetDesignTimeHtml(associatedProjectItem, forceRegenerate);
            }
            if ((designTimeHtml == null) || (designTimeHtml.Length == 0))
            {
                return base.CreatePlaceHolderDesignTimeHtml("<" + this._tagPrefix + ":" + this._tagName + ">");
            }
            return string.Format("<div style=\"border:solid 1px buttonshadow\">\r\n                <div style=\"font:messagebox;color:buttontext;background-color:buttonface;border-bottom: solid 1px buttonshadow;padding:4px\">\r\n                    <span style=\"font-weight:bold\">UserControl</span> - {0}\r\n                </div>\r\n                <div style=\"padding:2px\">{1}</div>\r\n              </div>", base.Component.Site.Name, designTimeHtml);
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            IUserControlDesignerAccessor accessor = (IUserControlDesignerAccessor) component;
            string tagName = accessor.TagName;
            int index = tagName.IndexOf(':');
            if ((index > 0) && (index < (tagName.Length - 1)))
            {
                this._tagPrefix = tagName.Substring(0, index);
                this._tagName = tagName.Substring(index + 1);
            }
        }

        private void OnEditDocumentInvoked(object sender, EventArgs e)
        {
            DocumentProjectItem associatedProjectItem = this.GetAssociatedProjectItem();
            if (associatedProjectItem == null)
            {
                ((IMxUIService) this.GetService(typeof(IMxUIService))).ReportError("Unable to locate the document associated with the user control.", "Edit Control Layout", true);
            }
            else
            {
                associatedProjectItem.Project.OpenProjectItem(associatedProjectItem, false, DocumentViewType.Default);
            }
        }

        private void OnRefreshInvoked(object sender, EventArgs e)
        {
            this._regenerateDesignTimeHtml = true;
            this.UpdateDesignTimeHtml();
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerbCollection verbs = base.Verbs;
                if (this._tagName != null)
                {
                    if (this._editDocumentVerb == null)
                    {
                        this._editDocumentVerb = new DesignerVerb("Edit UserControl Layout", new EventHandler(this.OnEditDocumentInvoked));
                        verbs.Add(this._editDocumentVerb);
                    }
                    if (this._refreshVerb == null)
                    {
                        this._refreshVerb = new DesignerVerb("Refresh Layout", new EventHandler(this.OnRefreshInvoked));
                        verbs.Add(this._refreshVerb);
                    }
                }
                return verbs;
            }
        }
    }
}

