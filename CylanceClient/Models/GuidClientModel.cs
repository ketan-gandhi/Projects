using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CylanceClient.Models
{
    [DataContract(Name ="guidmodel")]
    public class GuidClientModel
    {
        [DataMember(Name ="guid")]
        public Guid? Guid { get; set; }
        [DataMember(Name ="expire")]
        public int? Expire { get; set; }
        [DataMember(Name ="user")]
        public string User { get; set; }
    }
}
