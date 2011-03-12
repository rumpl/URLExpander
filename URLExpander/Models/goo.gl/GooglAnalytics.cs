namespace URLExpander.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GooglAnalytics
    {
        [DataMember(Name = "allTime")]
        public GoogleAnalyticsDetails AllTime { get; set; }
    }
}