namespace Microsoft.Matrix.Core.Documents
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    internal sealed class DocumentTypeManager : IDocumentTypeManager, IDisposable
    {
        private ImageList _documentIcons;
        private DocumentTypeCollection _documentTypes;
        private string _openFilters;
        private IServiceProvider _serviceProvider;

        public DocumentTypeManager(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public void LoadDocumentTypes()
        {
            this._documentIcons = new ImageList();
            this._documentIcons.ImageSize = new Size(0x10, 0x10);
            this._documentIcons.ColorDepth = ColorDepth.Depth32Bit;
            ImageList.ImageCollection images = this._documentIcons.Images;
            images.Add(CustomizedDocumentType.Generic.SmallIcon);
            CustomizedDocumentType.Generic.SetIconIndex(0);
            this._documentTypes = (DocumentTypeCollection) ConfigurationSettings.GetConfig("microsoft.matrix/documentTypes");
            int index = 0;
            foreach (DocumentType type in this._documentTypes.RegisteredDocumentTypes)
            {
                Icon smallIcon = type.SmallIcon;
                if (smallIcon != null)
                {
                    images.Add(smallIcon);
                    index++;
                    type.SetIconIndex(index);
                }
                else
                {
                    type.SetIconIndex(0);
                }
            }
        }

        DocumentType IDocumentTypeManager.GetDocumentType(string extension)
        {
            DocumentType generic = null;
            if (this._documentTypes != null)
            {
                generic = this._documentTypes[extension];
            }
            if (generic == null)
            {
                generic = CustomizedDocumentType.Generic;
            }
            return generic;
        }

        bool IDocumentTypeManager.IsRegisteredDocumentType(string extension)
        {
            if (this._documentTypes != null)
            {
                DocumentType type = this._documentTypes[extension];
                if ((type != null) && (type != CustomizedDocumentType.Generic))
                {
                    return true;
                }
            }
            return false;
        }

        void IDisposable.Dispose()
        {
            this._documentTypes = null;
            this._documentIcons.Dispose();
            this._documentIcons = null;
            this._serviceProvider = null;
        }

        ICollection IDocumentTypeManager.CreatableDocumentTypes
        {
            get
            {
                return this._documentTypes.RegisteredCreatableDocumentTypes;
            }
        }

        ImageList IDocumentTypeManager.DocumentIcons
        {
            get
            {
                return this._documentIcons;
            }
        }

        ICollection IDocumentTypeManager.DocumentTypes
        {
            get
            {
                return this._documentTypes.RegisteredDocumentTypes;
            }
        }

        string IDocumentTypeManager.OpenFilters
        {
            get
            {
                if (this._openFilters == null)
                {
                    StringBuilder builder = new StringBuilder(0x40 * this._documentTypes.Count);
                    StringBuilder builder2 = new StringBuilder(4 * this._documentTypes.Count);
                    builder2.Append("Common Files|");
                    int num = 0;
                    foreach (DocumentType type in this._documentTypes.RegisteredDocumentTypes)
                    {
                        string openFilter = type.OpenFilter;
                        if ((openFilter.Length != 0) && (openFilter.Length != 0))
                        {
                            builder.Append(type.OpenFilter);
                            builder.Append('|');
                            int num2 = openFilter.LastIndexOf('|');
                            if ((num2 != -1) && (num2 != (openFilter.Length - 1)))
                            {
                                if (num != 0)
                                {
                                    builder2.Append(';');
                                }
                                builder2.Append(openFilter.Substring(num2 + 1));
                                num++;
                            }
                        }
                    }
                    builder.Append(CustomizedDocumentType.Generic.OpenFilter);
                    if (num != 0)
                    {
                        this._openFilters = builder2.ToString() + "|" + builder.ToString();
                    }
                    else
                    {
                        this._openFilters = builder.ToString();
                    }
                }
                return this._openFilters;
            }
        }
    }
}

