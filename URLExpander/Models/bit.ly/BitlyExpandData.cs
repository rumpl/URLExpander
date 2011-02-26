using System.Collections.Generic;
using System.Runtime.Serialization;

namespace URLExpander.Models
{
  [DataContract]
  public class BitlyExpandData
  {
    [DataMember(Name = "expand")]
    public List<BitlyExpandedUrl> Urls { get; set; }

    [DataMember(Name = "clicks")]
    public List<BitlyClicks> Clicks { get; set; }
  }
}
