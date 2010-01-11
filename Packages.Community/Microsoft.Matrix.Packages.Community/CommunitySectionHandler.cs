namespace Microsoft.Matrix.Packages.Community
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Xml;

    public sealed class CommunitySectionHandler : IConfigurationSectionHandler
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
                        throw new ConfigurationException("Unexpected XML node type", node);
                    }
                    XmlAttribute attribute = node.Attributes["tab"];
                    if ((attribute == null) || (attribute.Value.Length == 0))
                    {
                        throw new ConfigurationException("Missing required attribute 'tab'", node);
                    }
                    string str = string.Empty;
                    XmlAttribute attribute2 = node.Attributes["data"];
                    if (attribute2 != null)
                    {
                        str = attribute2.Value;
                    }
                    list.Add(attribute.Value);
                    list.Add(str);
                }
            }
            return list;
        }
    }
}

