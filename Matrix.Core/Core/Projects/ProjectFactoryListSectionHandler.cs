namespace Microsoft.Matrix.Core.Projects
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Xml;

    public sealed class ProjectFactoryListSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            ArrayList list = new ArrayList();
            foreach (XmlNode node in section.ChildNodes)
            {
                if ((node.NodeType != XmlNodeType.Whitespace) && (node.NodeType != XmlNodeType.Comment))
                {
                    if ((node.NodeType != XmlNodeType.Element) || !node.Name.Equals("add"))
                    {
                        throw new ConfigurationException("Unexpected XML node type ", node);
                    }
                    XmlAttribute attribute = node.Attributes["project"];
                    if ((attribute == null) || (attribute.Value.Length == 0))
                    {
                        throw new ConfigurationException("Missing required attribute 'project'", node);
                    }
                    list.Add(attribute.Value);
                }
            }
            return list;
        }
    }
}

