using System.Collections.Generic;
using System.Runtime.Serialization;

namespace URLExpander.Models
{
  [DataContract]
  public class BitlyData
  {
    [DataMember(Name = "expand")]
    public List<ExpandedUrl> Urls { get; set; }

    [DataMember(Name = "clicks")]
    public List<Clicks> Clicks { get; set; }
  }
}
