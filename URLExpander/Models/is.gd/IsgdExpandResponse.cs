namespace URLExpander.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class IsgdExpandResponse : IResponse
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "errormessage")]
        public string ErrorMessage { get; set; }

        [DataMember(Name = "errorcode")]
        public int? ErrorCode { get; set; }

        public bool IsSuccessfulResponse
        {
            get { return ErrorCode == null; }
        }
    }
}
