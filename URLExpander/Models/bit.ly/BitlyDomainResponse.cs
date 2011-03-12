namespace URLExpander.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class BitlyDomainResponse : BitlyResponseBase
    {
        [DataMember(Name = "data")]
        public BitlyDomainData Data { get; set; }
    }
}
