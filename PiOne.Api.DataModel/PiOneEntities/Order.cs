namespace PiOne.Api.DataModel.PiOneEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Order1 = new HashSet<Order>();
            OrderDetails = new HashSet<OrderDetail>();
            Awards = new HashSet<Award>();
        }

        [StringLength(150)]
        public string ID { get; set; }

        [Required]
        [StringLength(150)]
        public string StoreID { get; set; }

        [StringLength(150)]
        public string MemberID { get; set; }

        [StringLength(150)]
        public string ParentID { get; set; }

        [StringLength(150)]
        public string TableName { get; set; }

        [StringLength(100)]
        public string PosOrderID { get; set; }

        [StringLength(150)]
        public string ReceiptNo { get; set; }

        [StringLength(150)]
        public string OrderNo { get; set; }

        [StringLength(50)]
        public string TagNo { get; set; }

        public byte SplitNo { get; set; }

        public byte Cover { get; set; }

        public double TotalBill { get; set; }

        public double TotalDiscount { get; set; }

        public double SubTotal { get; set; }

        public double Tip { get; set; }

        public double Tax { get; set; }

        public double Remaining { get; set; }

        public double ServiceCharged { get; set; }

        public double RoundingAmount { get; set; }

        public string Remark { get; set; }

        public double TotalPromotion { get; set; }

        public DateTime ReceiptCreatedDate { get; set; }
        public bool IsInput { get; set; }
        public int TransactionType { get; set; }

        public byte Status { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(150)]
        public string CreatedBy { get; set; }

        [Required]
        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order1 { get; set; }

        public virtual Order Order2 { get; set; }

        public virtual Member Member { get; set; }

        public virtual Store Store { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Award> Awards { get; set; }

    }
}
