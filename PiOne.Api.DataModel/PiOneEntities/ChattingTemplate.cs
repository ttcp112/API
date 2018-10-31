namespace PiOne.Api.DataModel.PiOneEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChattingTemplate")]
    public partial class ChattingTemplate
    {
        [StringLength(150)]
        public string ID { get; set; }

        [StringLength(100)]
        public string MerchantID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
        public byte Type { get; set; }

        public byte? Status { get; set; }
        public DateTime? CreatedDate { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }
        public virtual Merchant Merchant { get; set; }

    }
}
