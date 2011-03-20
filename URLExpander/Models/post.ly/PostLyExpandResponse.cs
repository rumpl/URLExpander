using System.Xml.Serialization;

namespace URLExpander.Models
{
    [XmlRoot("rsp", Namespace = "", IsNullable = false)]
    public class PostLyExpandResponse : IResponse
    {
        [XmlAttribute(AttributeName = "stat")]
        public string Status { get; set; }

        [XmlElement(ElementName = "err")]
        public PostLyError Error { get; set; }

        [XmlElement(ElementName = "post")]
        public Post Post { get; set; }

        // We are not using the rest...

        public bool IsSuccessfulResponse
        {
            get
            {
                return Status == "ok";
            }
        }
    }

    [XmlRoot("post")]
    public class Post
    {
        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        [XmlElement(ElementName = "link")]
        public string Link { get; set; }
    }

    [XmlRoot("err")]
    public class PostLyError
    {
        [XmlAttribute(AttributeName = "code")]
        public int Code { get; set; }

        [XmlAttribute(AttributeName = "msg")]
        public string Message { get; set; }
    }
}
