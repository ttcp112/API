namespace PiOne.Api.DataModel.PiOneEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Award")]
    public partial class Award
    {
        [StringLength(150)]
        public string ID { get; set; }

        [StringLength(150)]
        public string StoreID { get; set; }

        [StringLength(150)]
        public string MemberID { get; set; }
        public byte AwardType { get; set; }

        [StringLength(150)]
        public string ProductName { get; set; }

        [StringLength(150)]
        public string OrderID { get; set; }
        public double Value { get; set; }
        public double RemainingValue { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public byte? Status { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public virtual Member Member { get; set; }
        public virtual Store Store { get; set; }
        public virtual Order Order { get; set; }
    }
}
