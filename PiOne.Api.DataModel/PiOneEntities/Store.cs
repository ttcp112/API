namespace PiOne.Api.DataModel.PiOneEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Store")]
    public partial class Store
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Store()
        {
            Awards = new HashSet<Award>();
            MemberLocations = new HashSet<MemberLocation>();
            Orders = new HashSet<Order>();
            NotificationSettings = new HashSet<NotificationSetting>();
        }

        [StringLength(150)]
        public string ID { get; set; }

        [StringLength(150)]
        public string MerchantID { get; set; }

        [StringLength(200)]
        public string StoreImageUrl { get; set; }

        [StringLength(800)]
        public string StoreCode { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(70)]
        public string ContactPerson { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Telephone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(120)]
        public string Address { get; set; }

        [StringLength(120)]
        public string City { get; set; }
        [StringLength(120)]
        public string District { get; set; }

        [StringLength(120)]
        public string Country { get; set; }

        [StringLength(50)]
        public string PostalCode { get; set; }

        [StringLength(100)]
        public string Longtitude { get; set; }

        [StringLength(100)]
        public string Latitude { get; set; }

        [StringLength(50)]
        public string GSTRegNo { get; set; }
        public int TimeZone { get; set; }
        public bool IsBooking { get; set; }
        public bool IsActive { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        public byte? Status { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        [StringLength(150)]
        public string ModifiedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Award> Awards { get; set; }

        public virtual Merchant Merchant { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberLocation> MemberLocations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotificationSetting> NotificationSettings { get; set; }

    }
}
