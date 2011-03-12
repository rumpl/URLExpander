namespace URLExpander.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class BitlyExpandResponse : BitlyResponseBase
    {
        [DataMember(Name = "data")]
        public BitlyExpandData Data { get; set; }
    }
}
