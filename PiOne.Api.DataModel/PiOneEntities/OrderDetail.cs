namespace PiOne.Api.DataModel.PiOneEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderDetail()
        {
            OrderDetail1 = new HashSet<OrderDetail>();
        }

        [StringLength(150)]
        public string ID { get; set; }

        [Required]
        [StringLength(150)]
        public string OrderID { get; set; }

        [StringLength(150)]
        public string ParentID { get; set; }

        [StringLength(150)]
        public string ProductName { get; set; }

        [StringLength(150)]
        public string PromotionName { get; set; }

        public decimal Quantity { get; set; }

        public double Price { get; set; }

        public double Tax { get; set; }

        public double ServiceCharged { get; set; }

        public int QueueNumber { get; set; }

        public byte State { get; set; }

        public double DiscountAmount { get; set; }

        public bool IsTakeAway { get; set; }

        public double DiscountValue { get; set; }

        public byte DiscountType { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }

        public byte ModifierType { get; set; }

        public double PromotionValue { get; set; }

        public double PromotionAmount { get; set; }

        public byte PromotionType { get; set; }

        public bool IsApplyPromotion { get; set; }

        public byte Status { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(150)]
        public string CreatedBy { get; set; }

        [Required]
        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual Order Order { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetail1 { get; set; }

        public virtual OrderDetail OrderDetail2 { get; set; }

    }
}
