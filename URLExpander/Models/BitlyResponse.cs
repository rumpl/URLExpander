using System.Runtime.Serialization;

namespace URLExpander.Models
{
  [DataContract]
  public class BitlyResponse
  {
    [DataMember(Name = "data")]
    public BitlyData Data { get; set; }

    [DataMember(Name = "status_code")]
    public int StatusCode { get; set; }
  }
}
