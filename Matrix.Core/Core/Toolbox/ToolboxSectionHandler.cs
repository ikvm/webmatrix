namespace Microsoft.Matrix.Core.Toolbox
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Xml;

    public sealed class ToolboxSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            ArrayList list = new ArrayList();
            foreach (XmlNode node in section.ChildNodes)
            {
                try
                {
                    if ((node.NodeType != XmlNodeType.Whitespace) && (node.NodeType != XmlNodeType.Comment))
                    {
                        if ((node.NodeType != XmlNodeType.Element) || !node.Name.Equals("toolboxSection"))
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
                        ToolboxSection section2 = null;
                        try
                        {
                            section2 = (ToolboxSection) Activator.CreateInstance(Type.GetType(attribute2.Value, true), (object[]) new string[] { attribute.Value });
                        }
                        catch
                        {
                            throw new ConfigurationException("Invalid toolboxSection", node);
                        }
                        this.ProcessToolboxSection(node, section2);
                        list.Add(section2);
                    }
                    continue;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return list;
        }

        private void ProcessToolboxSection(XmlNode configSection, ToolboxSection section)
        {
            foreach (XmlNode node in configSection.ChildNodes)
            {
                try
                {
                    if ((node.NodeType != XmlNodeType.Whitespace) && (node.NodeType != XmlNodeType.Comment))
                    {
                        if ((node.NodeType != XmlNodeType.Element) || !node.Name.Equals("toolboxItem"))
                        {
                            throw new ConfigurationException("Unexpected XML node type ", node);
                        }
                        XmlAttribute attribute = node.Attributes["data"];
                        if ((attribute == null) || (attribute.Value.Length == 0))
                        {
                            throw new ConfigurationException("Missing required attribute 'data'", node);
                        }
                        try
                        {
                            ToolboxDataItem item = section.CreateToolboxDataItem(attribute.Value);
                            section.AddToolboxDataItem(item);
                        }
                        catch
                        {
                            throw new ConfigurationException("Invalid toolboxItem", node);
                        }
                    }
                    continue;
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}

