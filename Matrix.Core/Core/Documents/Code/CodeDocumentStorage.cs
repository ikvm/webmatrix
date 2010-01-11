namespace Microsoft.Matrix.Core.Documents.Code
{
    using Microsoft.Matrix.Core.Documents.Text;
    using System;

    public class CodeDocumentStorage : TextDocumentStorage
    {
        private CodeDocumentLanguage _language;

        public CodeDocumentStorage(CodeDocument owner) : base(owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            this._language = (CodeDocumentLanguage) owner.Language;
        }

        public CodeDocumentStorage(CodeDocumentLanguage language) : base(null)
        {
            if (language == null)
            {
                throw new ArgumentNullException("language");
            }
            this._language = language;
        }

        public CodeDocumentLanguage Language
        {
            get
            {
                return this._language;
            }
        }
    }
}

