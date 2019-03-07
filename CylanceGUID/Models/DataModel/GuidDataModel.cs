using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CylanceGUID.Models
{
    [Table("GuidModel")]
    public class GuidDataModel
    {
        [Key]
        public Guid Guid { get; set; }
        public int Expire { get; set; }
        public string User { get; set; }

        public GuidAPIModel ToAPIModel()
        {
            GuidAPIModel apiModel = new GuidAPIModel()
            {
                Guid = this.Guid,
                Expire = this.Expire,
                User = this.User
            };
            return apiModel;
        }
    }
}
