using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CylanceGUID.Models
{
    
    public class GuidAPIModel
    {

        public Guid? Guid { get; set; }
        public int? Expire { get; set; }
        public string User { get; set; }

        public GuidDataModel ToDataModel()
        {

            Guid guid = Guid.HasValue ? new Guid(this.Guid.Value.ToString().ToUpper()) : new Guid(System.Guid.NewGuid().ToString().ToUpper());

            int expire = Expire.HasValue ? this.Expire.Value : (Int32)(new DateTimeOffset(DateTime.UtcNow.AddDays(30))).ToUnixTimeSeconds();

            GuidDataModel dataModel = new GuidDataModel()
            {
                Guid = guid,
                Expire = expire,
                User = this.User
            };
            return dataModel;
        }
    }

    
}
