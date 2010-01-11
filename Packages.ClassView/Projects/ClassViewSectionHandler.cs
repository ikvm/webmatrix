namespace Microsoft.Matrix.Packages.ClassView.Projects
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Xml;

    public sealed class ClassViewSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            ClassViewProjectData data = new ClassViewProjectData();
            foreach (XmlNode node in section.ChildNodes)
            {
                if ((node.NodeType != XmlNodeType.Whitespace) && (node.NodeType != XmlNodeType.Comment))
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        if (node.Name.Equals("assembly"))
                        {
                            XmlAttribute attribute = node.Attributes["name"];
                            if ((attribute == null) || (attribute.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'name'", node);
                            }
                            XmlAttribute attribute2 = node.Attributes["displayName"];
                            if ((attribute2 == null) || (attribute2.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'displayName'", node);
                            }
                            try
                            {
                                data.AddAssemblyEntry(attribute.Value, attribute2.Value);
                                continue;
                            }
                            catch
                            {
                                throw new ConfigurationException("Invalid assembly entry", node);
                            }
                        }
                        if (node.Name.Equals("group"))
                        {
                            XmlAttribute attribute3 = node.Attributes["name"];
                            if ((attribute3 == null) || (attribute3.Value.Length == 0))
                            {
                                throw new ConfigurationException("Missing required attribute 'name'", node);
                            }
                            ArrayList items = this.ProcessGroup(node);
                            try
                            {
                                data.AddGroupEntry(attribute3.Value, items);
                                continue;
                            }
                            catch
                            {
                                throw new ConfigurationException("Invalid group entry", node);
                            }
                        }
                    }
                    throw new ConfigurationException("Unexpected XML node type", node);
                }
            }
            return data;
        }

        private ArrayList ProcessGroup(XmlNode groupSection)
        {
            ArrayList list = new ArrayList();
            foreach (XmlNode node in groupSection.ChildNodes)
            {
                if ((node.NodeType != XmlNodeType.Whitespace) && (node.NodeType != XmlNodeType.Comment))
                {
                    if ((node.NodeType == XmlNodeType.Element) && node.Name.Equals("groupItem"))
                    {
                        XmlAttribute attribute = node.Attributes["name"];
                        if ((attribute == null) || (attribute.Value.Length == 0))
                        {
                            throw new ConfigurationException("Missing required attribute 'name'", node);
                        }
                        XmlAttribute attribute2 = node.Attributes["type"];
                        if ((attribute2 == null) || (attribute2.Value.Length == 0))
                        {
                            throw new ConfigurationException("Missing required attribute 'type'", node);
                        }
                        try
                        {
                            list.Add(new ClassViewProjectData.GroupItem(attribute.Value, attribute2.Value));
                            continue;
                        }
                        catch
                        {
                            throw new ConfigurationException("Invalid group entry", node);
                        }
                    }
                    throw new ConfigurationException("Unexpected XML node type", node);
                }
            }
            return list;
        }
    }
}

