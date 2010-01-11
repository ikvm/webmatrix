namespace Microsoft.Matrix.Core.Documents
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Reflection;

    internal sealed class DocumentTypeCollection : ICollection, IEnumerable
    {
        private ArrayList _creatableDocTypes = new ArrayList();
        private HybridDictionary _docTypes = new HybridDictionary(false);
        private ArrayList _uniqueDocTypes = new ArrayList();

        public void AddAliasedDocumentType(DocumentType docType, string aliasExtension)
        {
            this._docTypes.Add(aliasExtension.ToUpper(), docType);
        }

        public void AddCustomizedDocumentType(DocumentType docType)
        {
            this.AddDocumentType(docType);
        }

        public void AddDocumentType(DocumentType docType)
        {
            this._docTypes.Add(docType.Extension, docType);
            this._uniqueDocTypes.Add(docType);
            if (docType.CanCreateNew)
            {
                this._creatableDocTypes.Add(docType);
            }
        }

        public void AddTemplateDocumentType(DocumentType docType)
        {
            this._creatableDocTypes.Add(docType);
        }

        public void AddWizardDocumentType(DocumentType docType)
        {
            this._creatableDocTypes.Add(docType);
        }

        public void CopyTo(Array array, int index)
        {
            this._uniqueDocTypes.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this._uniqueDocTypes.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return this._uniqueDocTypes.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public DocumentType this[string extension]
        {
            get
            {
                if ((extension == null) || (extension.Length == 0))
                {
                    return null;
                }
                if (extension[0] == '.')
                {
                    extension = extension.Substring(1);
                }
                return (DocumentType) this._docTypes[extension.ToUpper()];
            }
        }

        public ICollection RegisteredCreatableDocumentTypes
        {
            get
            {
                return this._creatableDocTypes;
            }
        }

        public ICollection RegisteredDocumentTypes
        {
            get
            {
                return this._uniqueDocTypes;
            }
        }

        public ICollection RegisteredExtensions
        {
            get
            {
                return this._docTypes.Keys;
            }
        }

        public object SyncRoot
        {
            get
            {
                return null;
            }
        }
    }
}

