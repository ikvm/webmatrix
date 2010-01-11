namespace Microsoft.Matrix.Core.UserInterface
{
    using System;
    using System.Xml.Serialization;

    [XmlType(Namespace="http://www.asp.net/matrix/services/ComponentGalleryService")]
    public class ComponentDescription
    {
        [XmlAttribute]
        public string Author;
        [XmlAttribute]
        public double AverageRating;
        [XmlIgnore]
        public bool AverageRatingSpecified;
        [XmlAttribute]
        public int ComponentCategory;
        [XmlIgnore]
        public bool ComponentCategorySpecified;
        [XmlAttribute]
        public int ComponentType;
        [XmlIgnore]
        public bool ComponentTypeSpecified;
        [XmlAttribute]
        public DateTime DateReleased;
        [XmlIgnore]
        public bool DateReleasedSpecified;
        [XmlAttribute]
        public string DiscussionForumUrl;
        [XmlAttribute]
        public int DownloadCount;
        [XmlIgnore]
        public bool DownloadCountSpecified;
        [XmlAttribute]
        public string Email;
        [XmlAttribute]
        public string FullDescriptionUrl;
        [XmlElement(DataType="base64Binary")]
        public byte[] Glyph;
        [XmlAttribute]
        public int Id;
        [XmlIgnore]
        public bool IdSpecified;
        [XmlAttribute]
        public string Keywords;
        public string LicenseInformation;
        public string LocalesSupported;
        [XmlAttribute]
        public string Name;
        public string PackageContents;
        [XmlAttribute]
        public string PackageUrl;
        [XmlAttribute]
        public int RatingCount;
        [XmlIgnore]
        public bool RatingCountSpecified;
        public string ShortDescription;
        [XmlAttribute]
        public int Size;
        [XmlIgnore]
        public bool SizeSpecified;
        [XmlAttribute]
        public bool SourceCodeAvailable;
        [XmlIgnore]
        public bool SourceCodeAvailableSpecified;
        public string Summary;
        public string TargetedPlatforms;
        [XmlAttribute]
        public string Version;
    }
}

