namespace Microsoft.Matrix.Core.Documents
{
    using System;
    using System.Configuration;
    using System.Xml;

    public sealed class DocumentTypesSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            DocumentTypeCollection types = new DocumentTypeCollection();
            foreach (XmlNode node in section.ChildNodes)
            {
                if ((node.NodeType != XmlNodeType.Whitespace) && (node.NodeType != XmlNodeType.Comment))
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        DocumentType docType = null;
                        if (node.Name.Equals("documentType"))
                        {
                            XmlAttribute attribute = node.Attributes["extension"];
                            if ((attribute == null) || (attribute.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'extension'", node);
                            }
                            XmlAttribute attribute2 = node.Attributes["type"];
                            if ((attribute2 == null) || (attribute2.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'type'", node);
                            }
                            try
                            {
                                docType = new DocumentType(attribute.Value, attribute2.Value);
                                types.AddDocumentType(docType);
                                continue;
                            }
                            catch
                            {
                                throw new ConfigurationException("Invalid document type section", node);
                            }
                        }
                        if (node.Name.Equals("customDocumentType"))
                        {
                            XmlAttribute attribute3 = node.Attributes["extension"];
                            if ((attribute3 == null) || (attribute3.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'extension'", node);
                            }
                            XmlAttribute attribute4 = node.Attributes["baseType"];
                            if ((attribute4 == null) || (attribute4.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'baseType'", node);
                            }
                            string name = null;
                            XmlAttribute attribute5 = node.Attributes["name"];
                            if (attribute5 != null)
                            {
                                name = attribute5.Value;
                            }
                            string createNewDescription = null;
                            XmlAttribute attribute6 = node.Attributes["createNewDescription"];
                            if (attribute6 != null)
                            {
                                createNewDescription = attribute6.Value;
                            }
                            XmlAttribute attribute7 = node.Attributes["openFilter"];
                            if ((attribute7 == null) || (attribute7.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'openFilter'", node);
                            }
                            string smallIconName = null;
                            XmlAttribute attribute8 = node.Attributes["smallIcon"];
                            if (attribute8 != null)
                            {
                                smallIconName = attribute8.Value;
                            }
                            string largeIconName = null;
                            XmlAttribute attribute9 = node.Attributes["largeIcon"];
                            if (attribute9 != null)
                            {
                                largeIconName = attribute9.Value;
                            }
                            bool overrideCreateUsingTemplate = false;
                            bool createUsingTemplate = false;
                            XmlAttribute attribute10 = node.Attributes["createUsingTemplate"];
                            if (attribute10 != null)
                            {
                                try
                                {
                                    createUsingTemplate = bool.Parse(attribute10.Value);
                                    overrideCreateUsingTemplate = true;
                                }
                                catch
                                {
                                    throw new ConfigurationException("Invalid value for 'createUsingTemplate' attribute", node);
                                }
                            }
                            string templateInstanceName = null;
                            XmlAttribute attribute11 = node.Attributes["templateInstanceName"];
                            if (attribute11 != null)
                            {
                                templateInstanceName = attribute11.Value;
                            }
                            bool overrideTemplateFlags = false;
                            TemplateFlags none = TemplateFlags.None;
                            XmlAttribute attribute12 = node.Attributes["templateFlags"];
                            if (attribute12 != null)
                            {
                                try
                                {
                                    none = (TemplateFlags) Enum.Parse(typeof(TemplateFlags), attribute12.Value, true);
                                    overrideTemplateFlags = true;
                                }
                                catch
                                {
                                    throw new ConfigurationException("Invalid value for 'templateFlags' attribute", node);
                                }
                            }
                            try
                            {
                                docType = new CustomizedDocumentType(attribute3.Value, attribute4.Value, name, createNewDescription, attribute7.Value, smallIconName, largeIconName, createUsingTemplate, overrideCreateUsingTemplate, none, overrideTemplateFlags, templateInstanceName);
                                types.AddCustomizedDocumentType(docType);
                                continue;
                            }
                            catch
                            {
                                throw new ConfigurationException("Invalid document type section", node);
                            }
                        }
                        if (node.Name.Equals("aliasedDocumentType"))
                        {
                            XmlAttribute attribute13 = node.Attributes["extension"];
                            if ((attribute13 == null) || (attribute13.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'extension'", node);
                            }
                            XmlAttribute attribute14 = node.Attributes["mapsTo"];
                            if ((attribute14 == null) || (attribute14.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'mapsTo'", node);
                            }
                            docType = types[attribute14.Value];
                            if (docType == null)
                            {
                                throw new ConfigurationException("Invalid aliased document type", node);
                            }
                            try
                            {
                                types.AddAliasedDocumentType(docType, attribute13.Value);
                                continue;
                            }
                            catch
                            {
                                throw new ConfigurationException("Invalid document type section", node);
                            }
                        }
                        if (node.Name.Equals("templateDocumentType"))
                        {
                            XmlAttribute attribute15 = node.Attributes["extension"];
                            if ((attribute15 == null) || (attribute15.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'extension'", node);
                            }
                            XmlAttribute attribute16 = node.Attributes["name"];
                            if ((attribute16 == null) || (attribute16.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'name'", node);
                            }
                            XmlAttribute attribute17 = node.Attributes["templateCategory"];
                            if ((attribute17 == null) || (attribute17.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'templateCategory'", node);
                            }
                            string str6 = null;
                            XmlAttribute attribute18 = node.Attributes["createNewDescription"];
                            if (attribute18 != null)
                            {
                                str6 = attribute18.Value;
                            }
                            DocumentType baseDocType = types[attribute15.Value];
                            if (baseDocType == null)
                            {
                                throw new ConfigurationException("Invalid template document type", node);
                            }
                            TemplateFlags templateFlags = baseDocType.TemplateFlags;
                            XmlAttribute attribute19 = node.Attributes["templateFlags"];
                            if (attribute19 != null)
                            {
                                try
                                {
                                    templateFlags = (TemplateFlags) Enum.Parse(typeof(TemplateFlags), attribute19.Value, true);
                                }
                                catch
                                {
                                    throw new ConfigurationException("Invalid value for 'templateFlags' attribute", node);
                                }
                            }
                            try
                            {
                                docType = new TemplateDocumentType(attribute15.Value, baseDocType, attribute16.Value, str6, attribute17.Value, templateFlags);
                                types.AddTemplateDocumentType(docType);
                                continue;
                            }
                            catch
                            {
                                throw new ConfigurationException("Invalid document type section", node);
                            }
                        }
                        if (node.Name.Equals("wizardDocumentType"))
                        {
                            XmlAttribute attribute20 = node.Attributes["extension"];
                            if ((attribute20 == null) || (attribute20.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'extension'", node);
                            }
                            XmlAttribute attribute21 = node.Attributes["type"];
                            if ((attribute21 == null) || (attribute21.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'type'", node);
                            }
                            XmlAttribute attribute22 = node.Attributes["name"];
                            if ((attribute22 == null) || (attribute22.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'name'", node);
                            }
                            XmlAttribute attribute23 = node.Attributes["templateCategory"];
                            if ((attribute23 == null) || (attribute23.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'templateCategory'", node);
                            }
                            string str7 = null;
                            XmlAttribute attribute24 = node.Attributes["createNewDescription"];
                            if (attribute24 != null)
                            {
                                str7 = attribute24.Value;
                            }
                            DocumentType type3 = types[attribute20.Value];
                            if (type3 == null)
                            {
                                throw new ConfigurationException("Invalid template document type", node);
                            }
                            TemplateFlags flags3 = type3.TemplateFlags;
                            XmlAttribute attribute25 = node.Attributes["templateFlags"];
                            if (attribute25 != null)
                            {
                                try
                                {
                                    flags3 = (TemplateFlags) Enum.Parse(typeof(TemplateFlags), attribute25.Value, true);
                                }
                                catch
                                {
                                    throw new ConfigurationException("Invalid value for 'templateFlags' attribute", node);
                                }
                            }
                            try
                            {
                                docType = new WizardDocumentType(attribute20.Value, type3, attribute21.Value, attribute22.Value, str7, attribute23.Value, flags3);
                                types.AddTemplateDocumentType(docType);
                                continue;
                            }
                            catch
                            {
                                throw new ConfigurationException("Invalid document type section", node);
                            }
                        }
                    }
                    throw new ConfigurationException("Unexpected XML node type", node);
                }
            }
            return types;
        }
    }
}

