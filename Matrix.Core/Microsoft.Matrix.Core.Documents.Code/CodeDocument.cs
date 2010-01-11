namespace Microsoft.Matrix.Core.Documents.Code
{
    using Microsoft.Matrix.Core.Documents;
    using Microsoft.Matrix.Core.Documents.Text;
    using Microsoft.Matrix.Core.Projects;
    using System;

    public class CodeDocument : TextDocument
    {
        private CodeDocumentLanguage _language;

        public CodeDocument(DocumentProjectItem projectItem, CodeDocumentLanguage language) : base(projectItem)
        {
            if (language == null)
            {
                throw new ArgumentNullException("language");
            }
            this._language = language;
        }

        protected override IDocumentStorage CreateStorage()
        {
            return new CodeDocumentStorage(this);
        }

        public override DocumentLanguage Language
        {
            get
            {
                return this._language;
            }
        }
    }
}

