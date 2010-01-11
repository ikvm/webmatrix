namespace Microsoft.Matrix.Core.UserInterface
{
    using System;
    using System.Xml.Serialization;

    [XmlType(Namespace="http://www.asp.net/matrix/services/ComponentGalleryService")]
    public class ComponentInfo
    {
        [XmlAttribute]
        public int Id;
        [XmlIgnore]
        public bool IdSpecified;
        [XmlAttribute]
        public string Name;
        public string PackageUrl;
        [XmlAttribute]
        public double Rating;
        [XmlIgnore]
        public bool RatingSpecified;
        public string Summary;
        [XmlAttribute]
        public string Version;
    }
}

