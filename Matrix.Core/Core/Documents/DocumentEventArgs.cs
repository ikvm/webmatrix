namespace Microsoft.Matrix.Core.Documents
{
    using System;

    public class DocumentEventArgs : EventArgs
    {
        private Microsoft.Matrix.Core.Documents.Document _document;

        public DocumentEventArgs(Microsoft.Matrix.Core.Documents.Document document)
        {
            this._document = document;
        }

        public Microsoft.Matrix.Core.Documents.Document Document
        {
            get
            {
                return this._document;
            }
        }
    }
}

