namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Projects;
    using Microsoft.Matrix.Packages.Web.Designer;
    using System;
    using System.IO;

    public class AscxDocument : WebFormsDocument
    {
        public AscxDocument(DocumentProjectItem projectItem) : base(projectItem)
        {
        }

        protected override DocumentDirective CreateDocumentDirective()
        {
            return new ControlDirective();
        }

        protected override bool IsValidDocumentDirective(DocumentDirective directive)
        {
            return (directive is ControlDirective);
        }

        protected override void Run()
        {
            throw new NotSupportedException("Cannot run a UserControl");
        }

        protected override void SaveStorageToStream(Stream stream)
        {
            base.SaveStorageToStream(stream);
            UserControlDesignTimeHtmlGenerator.Instance.ClearDesignTimeHtml(base.ProjectItem);
        }

        protected override bool CanRun
        {
            get
            {
                return false;
            }
        }

        protected override string DocumentDirectiveName
        {
            get
            {
                return "Control";
            }
        }
    }
}

