namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Projects;
    using System;

    public class AshxDocument : AspNetDocument
    {
        public AshxDocument(DocumentProjectItem projectItem) : base(projectItem)
        {
        }

        protected override DocumentDirective CreateDocumentDirective()
        {
            return new WebHandlerDirective();
        }

        protected override string DocumentDirectiveName
        {
            get
            {
                return "WebHandler";
            }
        }
    }
}

