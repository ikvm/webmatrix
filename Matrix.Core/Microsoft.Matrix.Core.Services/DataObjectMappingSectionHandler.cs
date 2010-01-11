namespace Microsoft.Matrix.Core.Services
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Xml;

    public sealed class DataObjectMappingSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            ArrayList list = new ArrayList();
            foreach (XmlNode node in section.ChildNodes)
            {
                if (((node.NodeType != XmlNodeType.Whitespace) && (node.NodeType != XmlNodeType.Comment)) && ((node.NodeType == XmlNodeType.Element) && node.Name.Equals("dataObjectMapping")))
                {
                    SingleTagSectionHandler handler = new SingleTagSectionHandler();
                    Hashtable hashtable = (Hashtable) handler.Create(null, null, node);
                    if (hashtable != null)
                    {
                        string fromDataFormat = (string) hashtable["fromDataFormat"];
                        if ((fromDataFormat != null) && (fromDataFormat.Length != 0))
                        {
                            string toDataFormat = (string) hashtable["toDataFormat"];
                            if ((toDataFormat != null) && (toDataFormat.Length != 0))
                            {
                                string typeName = (string) hashtable["type"];
                                if ((typeName != null) && (typeName.Length != 0))
                                {
                                    list.Add(new DataObjectMappingInfo(fromDataFormat, toDataFormat, typeName));
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }
    }
}

