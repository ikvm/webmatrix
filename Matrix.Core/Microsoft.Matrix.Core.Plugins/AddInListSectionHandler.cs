namespace Microsoft.Matrix.Core.Plugins
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Xml;

    public sealed class AddInListSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            ArrayList list = new ArrayList();
            foreach (XmlNode node in section.ChildNodes)
            {
                if ((node.NodeType != XmlNodeType.Whitespace) && (node.NodeType != XmlNodeType.Comment))
                {
                    if ((node.NodeType != XmlNodeType.Element) || !node.Name.Equals("addIn"))
                    {
                        throw new ConfigurationException("Unexpected XML node type ", node);
                    }
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
                    XmlAttribute attribute3 = node.Attributes["description"];
                    string description = null;
                    if ((attribute3 != null) && (attribute3.Value.Length != 0))
                    {
                        description = attribute3.Value;
                    }
                    XmlAttribute attribute4 = node.Attributes["scope"];
                    AddInScope global = AddInScope.Global;
                    if ((attribute4 != null) && (attribute4.Value.Length != 0))
                    {
                        try
                        {
                            global = (AddInScope) Enum.Parse(typeof(AddInScope), attribute4.Value);
                        }
                        catch
                        {
                            throw new ConfigurationException("Invalid attribute 'scope'", node);
                        }
                    }
                    AddInEntry entry = new AddInEntry(attribute2.Value, attribute.Value, description, global);
                    list.Add(entry);
                }
            }
            return list;
        }
    }
}

