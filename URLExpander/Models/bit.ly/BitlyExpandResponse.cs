namespace URLExpander.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class BitlyExpandResponse : IBitlyResponse
    {
        [DataMember(Name = "data")]
        public BitlyExpandData Data { get; set; }

        [DataMember(Name = "status_code")]
        public int StatusCode { get; set; }

        [DataMember(Name = "status_txt")]
        public string StatusText { get; set; }
    }
}
