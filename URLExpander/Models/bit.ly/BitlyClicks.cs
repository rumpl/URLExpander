namespace URLExpander.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class BitlyClicks
    {
        [DataMember(Name = "user_clicks")]
        public int UserClicks { get; set; }

        [DataMember(Name = "global_clicks")]
        public int GlobalClicks { get; set; }
    }
}