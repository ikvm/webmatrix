namespace Microsoft.Matrix.Core.Documents
{
    using Microsoft.Matrix.Core.Documents.Code;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;

    public sealed class TemplateDocumentType : DocumentType
    {
        private DocumentType _baseDocType;
        private string _createNewDescription;
        private string _name;
        private string _templateCategory;
        private Microsoft.Matrix.Core.Documents.TemplateFlags _templateFlags;

        internal TemplateDocumentType(string extension, DocumentType baseDocType, string name, string createNewDescription, string templateCategory, Microsoft.Matrix.Core.Documents.TemplateFlags templateFlags) : base(extension, baseDocType.Factory)
        {
            this._name = name;
            this._baseDocType = baseDocType;
            this._createNewDescription = createNewDescription;
            this._templateCategory = templateCategory;
            this._templateFlags = templateFlags;
        }

        protected override string GetTemplateFilePath()
        {
            StringBuilder builder = new StringBuilder(0x100);
            string templateFileExtension = base.GetTemplateFileExtension(null);
            builder.Append(Path.Combine(base.TemplatesPath, this.TemplateCategory));
            builder.Append('\\');
            builder.Append(this.Name);
            builder.Append('\\');
            builder.Append("NewFile");
            builder.Append('.');
            builder.Append(templateFileExtension);
            return builder.ToString();
        }

        protected override string GetTemplateFilePath(CodeDocumentLanguage codeLanguage)
        {
            StringBuilder builder = new StringBuilder(0x100);
            string templateFileExtension = base.GetTemplateFileExtension(codeLanguage);
            builder.Append(Path.Combine(base.TemplatesPath, this.TemplateCategory));
            builder.Append('\\');
            builder.Append(this.Name);
            builder.Append('\\');
            builder.Append(codeLanguage.Name);
            builder.Append('\\');
            builder.Append("NewFile");
            builder.Append('.');
            builder.Append(templateFileExtension);
            return builder.ToString();
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

