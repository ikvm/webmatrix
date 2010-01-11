namespace Microsoft.Matrix.Core.Documents
{
    using Microsoft.Matrix.Core.Application;
    using Microsoft.Matrix.Core.Documents.Code;
    using System;
    using System.CodeDom.Compiler;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    public class DocumentType
    {
        private string _extension;
        private IDocumentFactory _factory;
        private int _iconIndex;
        private DocumentInstanceArguments _instanceArgs;
        protected const string TemplateFileName = "NewFile";

        internal DocumentType(string extension, IDocumentFactory factory)
        {
            this._extension = extension.ToUpper();
            this._factory = factory;
            this._iconIndex = -1;
        }

        internal DocumentType(string extension, string factoryTypeName)
        {
            Type type = Type.GetType(factoryTypeName);
            this._factory = (IDocumentFactory) Activator.CreateInstance(type);
            this._extension = extension.ToUpper(CultureInfo.InvariantCulture);
            this._iconIndex = -1;
        }

        protected virtual byte[] GetTemplateContent(IServiceProvider serviceProvider, DocumentInstanceArguments instanceArguments)
        {
            byte[] buffer = null;
            Stream stream = null;
            try
            {
                string templateFilePath;
                bool flag = (this.TemplateFlags & Microsoft.Matrix.Core.Documents.TemplateFlags.HasCode) != Microsoft.Matrix.Core.Documents.TemplateFlags.None;
                bool flag2 = flag | ((this.TemplateFlags & Microsoft.Matrix.Core.Documents.TemplateFlags.HasMacros) != Microsoft.Matrix.Core.Documents.TemplateFlags.None);
                if ((this.TemplateFlags & Microsoft.Matrix.Core.Documents.TemplateFlags.HasCode) == Microsoft.Matrix.Core.Documents.TemplateFlags.None)
                {
                    templateFilePath = this.GetTemplateFilePath();
                }
                else
                {
                    templateFilePath = this.GetTemplateFilePath(instanceArguments.CodeLanguage);
                }
                stream = new FileStream(templateFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                int length = (int) stream.Length;
                buffer = new byte[length];
                stream.Read(buffer, 0, length);
                if (flag2)
                {
                    this._instanceArgs = instanceArguments;
                    buffer = this.ProcessMacros(buffer);
                }
            }
            finally
            {
                this._instanceArgs = null;
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
            if (buffer == null)
            {
                buffer = new byte[0];
            }
            return buffer;
        }

        protected string GetTemplateFileExtension(CodeDocumentLanguage codeLanguage)
        {
            if ((this.TemplateFlags & Microsoft.Matrix.Core.Documents.TemplateFlags.IsCode) != Microsoft.Matrix.Core.Documents.TemplateFlags.None)
            {
                CodeDomProvider codeDomProvider = codeLanguage.CodeDomProvider;
                if (codeDomProvider != null)
                {
                    string fileExtension = codeDomProvider.FileExtension;
                    if (fileExtension.StartsWith("."))
                    {
                        fileExtension = fileExtension.Substring(1);
                    }
                    return fileExtension;
                }
            }
            return this.Extension;
        }

        protected virtual string GetTemplateFilePath()
        {
            StringBuilder builder = new StringBuilder(0x100);
            string templateFileExtension = this.GetTemplateFileExtension(null);
            builder.Append(Path.Combine(this.TemplatesPath, templateFileExtension));
            builder.Append('\\');
            builder.Append("NewFile");
            builder.Append('.');
            builder.Append(templateFileExtension);
            return builder.ToString();
        }

        protected virtual string GetTemplateFilePath(CodeDocumentLanguage codeLanguage)
        {
            StringBuilder builder = new StringBuilder(0x100);
            string templateFileExtension = this.GetTemplateFileExtension(codeLanguage);
            builder.Append(Path.Combine(this.TemplatesPath, templateFileExtension));
            builder.Append('\\');
            builder.Append(codeLanguage.Name);
            builder.Append('\\');
            builder.Append("NewFile");
            builder.Append('.');
            builder.Append(templateFileExtension);
            return builder.ToString();
        }

        public byte[] Instantiate(IServiceProvider serviceProvider, DocumentInstanceArguments instanceArguments)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            if (instanceArguments == null)
            {
                throw new ArgumentNullException("instanceArguments");
            }
            if (!this.CanCreateNew)
            {
                throw new InvalidOperationException();
            }
            if (this.CreateUsingTemplate)
            {
                return this.GetTemplateContent(serviceProvider, instanceArguments);
            }
            return new byte[0];
        }

        private byte[] ProcessMacros(byte[] originalContent)
        {
            string pattern = "(?<Macro>(%%[^%]+%%))";
            string s = Regex.Replace(Encoding.UTF8.GetString(originalContent), pattern, new MatchEvaluator(this.ReplaceMacro), RegexOptions.Singleline | RegexOptions.Multiline);
            return Encoding.UTF8.GetBytes(s);
        }

        private string ReplaceMacro(Match match)
        {
            string str = match.ToString();
            string str2 = str.Substring(2, str.Length - 4);
            string format = null;
            int index = str2.IndexOf(',');
            if (index > 0)
            {
                string str4 = str2;
                str2 = str4.Substring(0, index);
                format = str4.Substring(index + 1);
            }
            string fileName = null;
            if (str2.Equals("FileName"))
            {
                fileName = this._instanceArgs.FileName;
            }
            else if (str2.Equals("CodeLanguage"))
            {
                fileName = this._instanceArgs.CodeLanguage.Name;
            }
            else if (str2.Equals("NamespaceName"))
            {
                fileName = this._instanceArgs.NamespaceName;
            }
            else if (str2.Equals("ClassName"))
            {
                fileName = this._instanceArgs.ClassName;
            }
            if (fileName == null)
            {
                return str;
            }
            if (fileName.Length == 0)
            {
                return string.Empty;
            }
            if (format == null)
            {
                return fileName;
            }
            return string.Format(format, fileName);
        }

        internal void SetIconIndex(int index)
        {
            this._iconIndex = index;
        }

        public virtual bool CanCreateNew
        {
            get
            {
                return this._factory.CanCreateNew;
            }
        }

        public virtual string CreateNewDescription
        {
            get
            {
                return this._factory.CreateNewDescription;
            }
        }

        public virtual bool CreateUsingTemplate
        {
            get
            {
                return this._factory.CreateUsingTemplate;
            }
        }

        public string Extension
        {
            get
            {
                return this._extension;
            }
        }

        public IDocumentFactory Factory
        {
            get
            {
                return this._factory;
            }
        }

        public int IconIndex
        {
            get
            {
                return this._iconIndex;
            }
        }

        public virtual Icon LargeIcon
        {
            get
            {
                return this._factory.LargeIcon;
            }
        }

        public virtual string Name
        {
            get
            {
                return this._factory.Name;
            }
        }

        public virtual string OpenFilter
        {
            get
            {
                return this._factory.OpenFilter;
            }
        }

        public virtual Icon SmallIcon
        {
            get
            {
                return this._factory.SmallIcon;
            }
        }

        public DocumentViewType SupportedViews
        {
            get
            {
                return this._factory.SupportedViews;
            }
        }

        public virtual string TemplateCategory
        {
            get
            {
                return this._factory.TemplateCategory;
            }
        }

        public virtual Microsoft.Matrix.Core.Documents.TemplateFlags TemplateFlags
        {
            get
            {
                return this._factory.TemplateFlags;
            }
        }

        public virtual string TemplateInstanceFileName
        {
            get
            {
                return "NewFile";
            }
        }

        protected string TemplatesPath
        {
            get
            {
                return MxApplication.Current.TemplatesPath;
            }
        }
    }
}

