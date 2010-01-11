namespace Microsoft.Matrix.Core.Documents
{
    using Microsoft.Matrix.Core.Documents.Code;
    using System;

    public sealed class DocumentInstanceArguments
    {
        private string _className;
        private CodeDocumentLanguage _codeLanguage;
        private string _fileName;
        private string _namespaceName;

        public DocumentInstanceArguments(string fileName)
        {
            if ((fileName == null) || (fileName.Length == 0))
            {
                throw new ArgumentNullException("fileName");
            }
            this._fileName = fileName;
        }

        public DocumentInstanceArguments(string fileName, CodeDocumentLanguage codeLanguage, string namespaceName, string className)
        {
            if ((fileName == null) || (fileName.Length == 0))
            {
                throw new ArgumentNullException("fileName");
            }
            if (codeLanguage == null)
            {
                throw new ArgumentNullException("codeLanguage");
            }
            this._fileName = fileName;
            this._codeLanguage = codeLanguage;
            this._namespaceName = namespaceName;
            this._className = className;
        }

        public string ClassName
        {
            get
            {
                if (this._codeLanguage == null)
                {
                    throw new InvalidOperationException();
                }
                if (this._className == null)
                {
                    return string.Empty;
                }
                return this._className;
            }
        }

        public CodeDocumentLanguage CodeLanguage
        {
            get
            {
                if (this._codeLanguage == null)
                {
                    throw new InvalidOperationException();
                }
                return this._codeLanguage;
            }
        }

        public string FileName
        {
            get
            {
                return this._fileName;
            }
        }

        public string NamespaceName
        {
            get
            {
                if (this._codeLanguage == null)
                {
                    throw new InvalidOperationException();
                }
                if (this._namespaceName == null)
                {
                    return string.Empty;
                }
                return this._namespaceName;
            }
        }
    }
}

