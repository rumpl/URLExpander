namespace URLExpander.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class BitlyExpandedUrl
    {
        [DataMember(Name = "long_url")]
        public string Url { get; set; }
    }
}