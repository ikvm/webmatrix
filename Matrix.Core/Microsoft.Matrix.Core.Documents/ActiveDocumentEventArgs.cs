namespace Microsoft.Matrix.Core.Documents
{
    using System;

    public class ActiveDocumentEventArgs : EventArgs
    {
        private Document _newDocument;
        private Document _oldDocument;

        public ActiveDocumentEventArgs(Document oldDocument, Document newDocument)
        {
            this._oldDocument = oldDocument;
            this._newDocument = newDocument;
        }

        public Document NewDocument
        {
            get
            {
                return this._newDocument;
            }
        }

        public Document OldDocument
        {
            get
            {
                return this._oldDocument;
            }
        }
    }
}

