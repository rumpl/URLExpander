namespace URLExpander.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public abstract class BitlyResponseBase : IResponse
    {
        [DataMember(Name = "status_code")]
        public int StatusCode { get; set; }

        [DataMember(Name = "status_txt")]
        public string StatusText { get; set; }

        public bool IsSuccessfulResponse
        {
            get { return this.StatusCode == 200; }
        }
    }
}