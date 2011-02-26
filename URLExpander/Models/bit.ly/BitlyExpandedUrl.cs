using System.Runtime.Serialization;

namespace URLExpander.Models
{
  [DataContract]
  public class BitlyExpandedUrl
  {
    [DataMember(Name = "long_url")]
    public string Url { get; set; }
  }
}
