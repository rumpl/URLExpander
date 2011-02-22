using System.Runtime.Serialization;

namespace URLExpander.Models
{
  [DataContract]
  public class Clicks
  {
    [DataMember(Name = "user_clicks")]
    public int UserClicks { get; set; }

    [DataMember(Name = "global_clicks")]
    public int GlobalClicks { get; set; }
  }
}