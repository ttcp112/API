namespace PiOne.Api.DataModel.PiOneEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Merchant")]
    public partial class Merchant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Merchant()
        {
            Merchant1 = new HashSet<Merchant>();
            MemberLocations = new HashSet<MemberLocation>();
            NotificationSettings = new HashSet<NotificationSetting>();
        }

        [StringLength(150)]
        public string ID { get; set; }

        [StringLength(800)]
        public string MerchantCode { get; set; }

        [StringLength(200)]
        public string MerchantImageUrl { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(120)]
        public string Address { get; set; }

        [StringLength(120)]
        public string City { get; set; }

        [StringLength(120)]
        public string Country { get; set; }

        [StringLength(50)]
        public string PostalCode { get; set; }

        [StringLength(70)]
        public string ContactPerson { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Telephone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string ColorCode { get; set; }
        public bool IsFree { get; set; }

        [StringLength(20)]
        public string ActiveCode { get; set; }
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
        public virtual ICollection<Merchant> Merchant1 { get; set; }

        public virtual Merchant Merchant2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberLocation> MemberLocations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Store> Stores { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotificationSetting> NotificationSettings { get; set; }

    }

}
