namespace Microsoft.Matrix.Core.Documents.Text
{
    using Microsoft.Matrix.Core.Documents;
    using System;
    using System.IO;
    using System.Text;
    using Microsoft.Matrix.Core.Projects.FileSystem;

    public class TextDocumentStorage : IDocumentStorage, IDisposable, IEmbeddedDocumentStorage
    {
        private bool _checkedUnicode;
        private bool _containsUnicodeCharacters;
        private Microsoft.Matrix.Core.Documents.Document _owner;
        private string _text;

        public TextDocumentStorage(TextDocument owner)
        {
            this._owner = owner;
        }

        void IDocumentStorage.Load(Stream contentStream)
        {
            MiscFileProjectItem projectItem = this._owner.ProjectItem as MiscFileProjectItem;
            Encoding encoding = projectItem != null ? projectItem.Encoding : Encoding.UTF8;

            this.Load(contentStream, encoding);
        }

        void Load(Stream contentStream, Encoding encoding)
        {
            StringBuilder builder = new StringBuilder(0x400);
            StreamReader reader = new StreamReader(contentStream, encoding);
            char[] chArray = new char[0x400];
            int charCount = 1;
            while (charCount > 0)
            {
                charCount = reader.Read(chArray, 0, 0x400);
                if (charCount != 0)
                {
                    builder.Append(chArray, 0, charCount);
                }
            }
            this._text = builder.ToString();
            byte[] preamble = reader.CurrentEncoding.GetPreamble();
            if (((preamble.Length == 3) && (preamble[0] == 0xef)) && ((preamble[1] == 0xbb) && (preamble[2] == 0xbf)))
            {
                this._checkedUnicode = true;
                this._containsUnicodeCharacters = true;
            }
        }

        void IDocumentStorage.Save(Stream contentStream)
        {
            MiscFileProjectItem projectItem = this._owner.ProjectItem as MiscFileProjectItem;
            Encoding encoding = projectItem != null ? projectItem.Encoding : Encoding.UTF8;

            this.Save(contentStream, encoding);
        }

        void Save(Stream contentStream, Encoding encoding)
        {
            if ((this._text != null) && (this._text.Length > 0))
            {
                StreamWriter writer = new StreamWriter(contentStream, encoding);
                writer.Write(this._text);
                writer.Flush();
            }
        }

        void IEmbeddedDocumentStorage.SetContainerDocument(Microsoft.Matrix.Core.Documents.Document document)
        {
            this._owner = document;
        }

        void IDisposable.Dispose()
        {
            this._text = null;
            this._owner = null;
        }

        public bool ContainsUnicodeCharacters
        {
            get
            {
                if (!this._checkedUnicode)
                {
                    this._checkedUnicode = true;
                    this._containsUnicodeCharacters = false;
                    for (int i = 0; i < this._text.Length; i++)
                    {
                        if (this._text[i] > '\x007f')
                        {
                            this._containsUnicodeCharacters = true;
                            break;
                        }
                    }
                }
                return this._containsUnicodeCharacters;
            }
        }

        protected TextDocument Document
        {
            get
            {
                if (this._owner is TextDocument)
                {
                    return (TextDocument) this._owner;
                }
                return null;
            }
        }

        public string Text
        {
            get
            {
                if (this._text == null)
                {
                    return string.Empty;
                }
                return this._text;
            }
            set
            {
                this._text = value;
                this._checkedUnicode = false;
                if (this._owner != null)
                {
                    this._owner.SetDirty();
                }
            }
        }
    }
}

