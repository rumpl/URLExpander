namespace URLExpander.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class BitlyExpandData
    {
        [DataMember(Name = "expand")]
        public List<BitlyExpandedUrl> Urls { get; set; }

        [DataMember(Name = "clicks")]
        public List<BitlyClicks> Clicks { get; set; }
    }
}