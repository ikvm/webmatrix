namespace Microsoft.Matrix.Core.UserInterface
{
    using System;
    using System.Xml.Serialization;

    [XmlType(Namespace="http://www.asp.net/matrix/services/SendFeedbackService")]
    public class FeedbackData
    {
        [XmlAttribute]
        public string appVersion;
        public string AssemblyInfo;
        public string exceptionData;
        [XmlAttribute]
        public bool isExceptionReport;
        [XmlIgnore]
        public bool isExceptionReportSpecified;
        [XmlAttribute]
        public string osInfo;
        public string Text;
        [XmlAttribute]
        public string title;
    }
}

