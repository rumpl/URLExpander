namespace URLExpander.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class BitlyDomainData
    {
        /// <summary>
        /// 0 or 1 designating whether this is a current bitly.Pro domain. 
        /// </summary>
        [DataMember(Name = "domain")]
        public string Domain { get; set; }

        /// <summary>
        /// this is an echo back of the request parameter. 
        /// </summary>
        [DataMember(Name = "bitly_pro_domain")]
        public bool IsBitlyProDomain { get; set; }
    }
}