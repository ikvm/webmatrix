namespace Microsoft.Matrix.Core.Documents
{
    using System;
    using System.Drawing;
    using System.IO;
    using Microsoft.Matrix.Core.Documents.Text;

    public sealed class CustomizedDocumentType : DocumentType
    {
        private string _createNewDescription;
        private bool _createUsingTemplate;
        private Icon _largeIcon;
        private string _largeIconName;
        private string _name;
        private string _openFilter;
        private bool _overrideCreateUsingTemplate;
        private bool _overrideTemplateFlags;
        private Icon _smallIcon;
        private string _smallIconName;
        private Microsoft.Matrix.Core.Documents.TemplateFlags _templateFlags;
        private string _templateInstanceName;
        internal static readonly CustomizedDocumentType Generic = new CustomizedDocumentType();

        internal CustomizedDocumentType() : base("*", typeof(TextDocumentFactory).FullName)
        {
            this._smallIcon = new Icon(typeof(CustomizedDocumentType), "GenericDocumentSmall.ico");
            this._openFilter = "All Files (*.*)|*.*";
        }

        internal CustomizedDocumentType(string extension, string factoryTypeName, string name, string createNewDescription, string openFilter, string smallIconName, string largeIconName, bool createUsingTemplate, bool overrideCreateUsingTemplate, Microsoft.Matrix.Core.Documents.TemplateFlags templateFlags, bool overrideTemplateFlags, string templateInstanceName) : base(extension, factoryTypeName)
        {
            this._name = name;
            this._createNewDescription = createNewDescription;
            this._openFilter = openFilter;
            this._smallIconName = smallIconName;
            this._largeIconName = largeIconName;
            this._createUsingTemplate = createUsingTemplate;
            this._overrideCreateUsingTemplate = overrideCreateUsingTemplate;
            this._templateFlags = templateFlags;
            this._overrideTemplateFlags = overrideTemplateFlags;
            this._templateInstanceName = templateInstanceName;
        }

        public override bool CanCreateNew
        {
            get
            {
                if (base.Extension.Length == 0)
                {
                    return false;
                }
                return base.CanCreateNew;
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
                if (this._overrideCreateUsingTemplate)
                {
                    return this._createUsingTemplate;
                }
                return base.CreateUsingTemplate;
            }
        }

        public override Icon LargeIcon
        {
            get
            {
                if (this._largeIcon == null)
                {
                    if ((this._largeIconName != null) && (this._largeIconName.Length != 0))
                    {
                        string fileName = Path.Combine(Path.Combine(base.TemplatesPath, base.Extension), this._largeIconName);
                        this._largeIconName = null;
                        try
                        {
                            this._largeIcon = new Icon(fileName);
                            return this._largeIcon;
                        }
                        catch
                        {
                        }
                    }
                    this._largeIcon = base.LargeIcon;
                }
                return this._largeIcon;
            }
        }

        public override string Name
        {
            get
            {
                if (this._name == null)
                {
                    this._name = base.Extension + " File";
                }
                return this._name;
            }
        }

        public override string OpenFilter
        {
            get
            {
                return this._openFilter;
            }
        }

        public override Icon SmallIcon
        {
            get
            {
                if (this._smallIcon == null)
                {
                    if ((this._smallIconName != null) && (this._smallIconName.Length != 0))
                    {
                        string fileName = Path.Combine(Path.Combine(base.TemplatesPath, base.Extension), this._smallIconName);
                        this._smallIconName = null;
                        try
                        {
                            this._smallIcon = new Icon(fileName);
                            return this._smallIcon;
                        }
                        catch
                        {
                        }
                    }
                    this._smallIcon = base.SmallIcon;
                }
                return this._smallIcon;
            }
        }

        public override Microsoft.Matrix.Core.Documents.TemplateFlags TemplateFlags
        {
            get
            {
                if (this._overrideTemplateFlags)
                {
                    return this._templateFlags;
                }
                return base.TemplateFlags;
            }
        }

        public override string TemplateInstanceFileName
        {
            get
            {
                if (this._templateInstanceName != null)
                {
                    return this._templateInstanceName;
                }
                return base.TemplateInstanceFileName;
            }
        }
    }
}

