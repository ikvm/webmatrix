namespace Microsoft.Matrix.Packages.Web.Documents
{
    using Microsoft.Matrix.Core.Projects;
    using System;

    public class AsmxDocument : AspNetDocument
    {
        public AsmxDocument(DocumentProjectItem projectItem) : base(projectItem)
        {
        }

        protected override DocumentDirective CreateDocumentDirective()
        {
            return new WebServiceDirective();
        }

        protected override string DocumentDirectiveName
        {
            get
            {
                return "WebService";
            }
        }
    }
}

