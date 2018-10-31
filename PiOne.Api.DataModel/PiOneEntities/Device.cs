namespace PiOne.Api.DataModel.PiOneEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Device")]
    public partial class Device
    {
        [StringLength(150)]
        public string ID { get; set; }

        [StringLength(150)]
        public string MemberID { get; set; }

        [StringLength(150)]
        public string UUID { get; set; }


        [StringLength(150)]
        public string LanguageID { get; set; }

        public bool IsDeleted { get; set; }

        public byte? Status { get; set; }
        public string DeviceToken { get; set; }
        public byte DeviceType { get; set; }
        public byte Type { get; set; }
        public DateTime? CreatedDate { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

    }
}
