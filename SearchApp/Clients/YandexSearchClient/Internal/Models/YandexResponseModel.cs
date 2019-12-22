using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace YandexSearchClient.Internal.Models
{
    [XmlRoot(ElementName = "yandexsearch")]
    public class YandexResponseModel
    {
        [XmlElement(ElementName = "response")]
        public Response Response { get; set; }
    }

    [XmlRoot(ElementName = "response")]
    public class Response
    {
        [XmlElement(ElementName = "results")]
        public Results Results { get; set; }
    }

    [XmlRoot(ElementName = "results")]
    public class Results
    {
        [XmlElement(ElementName = "grouping")]
        public Grouping Grouping { get; set; }
    }

    [XmlRoot(ElementName = "grouping")]
    public class Grouping
    {
        [XmlElement(ElementName = "group")]
        public List<Group> Group { get; set; }
    }

    [XmlRoot(ElementName = "group")]
    public class Group
    {
        [XmlElement(ElementName = "doc")]
        public Doc Doc { get; set; }
    }

    [XmlRoot(ElementName = "doc")]
    public class Doc
    {
        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        [XmlElement(ElementName = "title")]
        public object Title { get; set; }
    }
}
