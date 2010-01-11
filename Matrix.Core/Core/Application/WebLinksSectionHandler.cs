namespace Microsoft.Matrix.Core.Application
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Xml;

    public sealed class WebLinksSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            Hashtable hashtable = new Hashtable();
            foreach (XmlNode node in section.ChildNodes)
            {
                if ((node.NodeType != XmlNodeType.Whitespace) && (node.NodeType != XmlNodeType.Comment))
                {
                    if ((node.NodeType == XmlNodeType.Element) && node.Name.Equals("webLink"))
                    {
                        XmlAttribute attribute = node.Attributes["name"];
                        if ((attribute == null) || (attribute.Value.Length == 0))
                        {
                            throw new ConfigurationException("Missing required attribute 'name'", node);
                        }
                        XmlAttribute attribute2 = node.Attributes["title"];
                        if ((attribute2 == null) || (attribute2.Value.Length == 0))
                        {
                            throw new ConfigurationException("Missing required attribute 'title'", node);
                        }
                        XmlAttribute attribute3 = node.Attributes["url"];
                        if ((attribute3 == null) || (attribute3.Value.Length == 0))
                        {
                            throw new ConfigurationException("Missing required attribute 'url'", node);
                        }
                        WebLink link = new WebLink(attribute2.Value, attribute3.Value);
                        try
                        {
                            hashtable[attribute.Value] = link;
                            continue;
                        }
                        catch
                        {
                            throw new ConfigurationException("Invalid WebLink. The specified name is already in use.", node);
                        }
                    }
                    throw new ConfigurationException("Unexpected XML node type ", node);
                }
            }
            return hashtable;
        }
    }
}

