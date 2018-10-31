namespace PiOne.Api.DataModel.PiOneEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberLocation")]
    public partial class MemberLocation
    {
        [StringLength(150)]
        public string ID { get; set; }

        [StringLength(150)]
        public string MerchantID { get; set; }

        [StringLength(150)]
        public string StoreID { get; set; }

        [StringLength(150)]
        public string MemberID { get; set; }

        public byte? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public virtual Merchant Merchant { get; set; }

        public virtual Member Member { get; set; }

        public virtual Store Store { get; set; }
    }
}
