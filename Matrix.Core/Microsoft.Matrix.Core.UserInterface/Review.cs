namespace Microsoft.Matrix.Core.UserInterface
{
    using System;
    using System.Xml.Serialization;

    [XmlType(Namespace="http://www.asp.net/matrix/services/ComponentGalleryService")]
    public class Review
    {
        [XmlAttribute]
        public string Contents;
        [XmlAttribute]
        public DateTime Date;
        [XmlIgnore]
        public bool DateSpecified;
        [XmlAttribute]
        public string Email;
        [XmlAttribute]
        public double Rating;
        [XmlIgnore]
        public bool RatingSpecified;
        [XmlAttribute]
        public string Title;
    }
}

