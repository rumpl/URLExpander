namespace URLExpander.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GoogleAnalyticsDetails
    {
        [DataMember(Name = "shortUrlClicks")]
        public int ShortUrlClicks { get; set; }

        [DataMember(Name = "longUrlClicks")]
        public int LongUrlClicks { get; set; }
    }
}