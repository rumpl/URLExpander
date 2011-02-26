using System.Runtime.Serialization;

namespace URLExpander.Models
{
    [DataContract]
    public class BitlyDomainResponse : IBitlyResponse
    {
        [DataMember(Name = "data")]
        public BitlyDomainData Data { get; set; }

        [DataMember(Name = "status_code")]
        public int StatusCode { get; set; }

        [DataMember(Name = "status_txt")]
        public string StatusText { get; set; }
    }
}
