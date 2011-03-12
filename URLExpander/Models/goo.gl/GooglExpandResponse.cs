namespace URLExpander.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class GooglExpandResponse : IResponse
    {
        [DataMember(Name = "analytics")]
        public GooglAnalytics Analytics { get; set; }

        [DataMember(Name = "longUrl")]
        public string LongUrl { get; set; }

        [DataMember(Name = "status")]
        public string StatusText { get; set; }

        public bool IsSuccessfulResponse
        {
            get { return this.StatusText == "OK"; }
        }
    }
}
