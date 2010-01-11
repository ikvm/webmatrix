namespace Microsoft.Matrix.Core.UserInterface
{
    using System;
    using System.Xml.Serialization;

    [XmlType(Namespace="http://www.asp.net/matrix/services/ComponentGalleryService")]
    public class ComponentTypeInfo
    {
        [XmlAttribute]
        public int Id;
        [XmlIgnore]
        public bool IdSpecified;
        [XmlAttribute]
        public string Name;
    }
}

