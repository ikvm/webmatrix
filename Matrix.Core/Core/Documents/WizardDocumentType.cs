namespace Microsoft.Matrix.Core.Documents
{
    using Microsoft.Matrix.Core.Plugins;
    using System;
    using System.Drawing;

    public sealed class WizardDocumentType : DocumentType
    {
        private DocumentType _baseDocType;
        private string _createNewDescription;
        private string _name;
        private string _templateCategory;
        private Microsoft.Matrix.Core.Documents.TemplateFlags _templateFlags;
        private string _wizardType;

        internal WizardDocumentType(string extension, DocumentType baseDocType, string wizardType, string name, string createNewDescription, string templateCategory, Microsoft.Matrix.Core.Documents.TemplateFlags templateFlags) : base(extension, baseDocType.Factory)
        {
            this._name = name;
            this._baseDocType = baseDocType;
            this._wizardType = wizardType;
            this._createNewDescription = createNewDescription;
            this._templateCategory = templateCategory;
            this._templateFlags = templateFlags;
        }

        protected override byte[] GetTemplateContent(IServiceProvider serviceProvider, DocumentInstanceArguments instanceArguments)
        {
            DocumentWizardHost host = new DocumentWizardHost(serviceProvider);
            return host.RunDocumentWizard(this._wizardType, instanceArguments);
        }

        public override bool CanCreateNew
        {
            get
            {
                return true;
            }
        }

        public override string CreateNewDescription
        {
            get
            {
                if (this._createNewDescription != null)
                {
                    return this._createNewDescription;
                }
                return string.Empty;
            }
        }

        public override bool CreateUsingTemplate
        {
            get
            {
                return true;
            }
        }

        public override Icon LargeIcon
        {
            get
            {
                return this._baseDocType.LargeIcon;
            }
        }

        public override string Name
        {
            get
            {
                return this._name;
            }
        }

        public override string OpenFilter
        {
            get
            {
                return string.Empty;
            }
        }

        public override Icon SmallIcon
        {
            get
            {
                return this._baseDocType.SmallIcon;
            }
        }

        public override string TemplateCategory
        {
            get
            {
                return this._templateCategory;
            }
        }

        public override Microsoft.Matrix.Core.Documents.TemplateFlags TemplateFlags
        {
            get
            {
                return this._templateFlags;
            }
        }

        public override string TemplateInstanceFileName
        {
            get
            {
                return this._baseDocType.TemplateInstanceFileName;
            }
        }
    }
}

