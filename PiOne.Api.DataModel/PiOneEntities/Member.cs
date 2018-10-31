namespace PiOne.Api.DataModel.PiOneEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Member")]
    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            Awards = new HashSet<Award>();
            MemberLocations = new HashSet<MemberLocation>();
            Orders = new HashSet<Order>();
            NotificationSettings = new HashSet<NotificationSetting>();
        }

        [StringLength(150)]
        public string ID { get; set; }

        [Required]
        [StringLength(150)]
        public string LoginID { get; set; }
        [StringLength(800)]
        public string Password { get; set; }

        [StringLength(10)]
        public string PasswordCode { get; set; }

        [StringLength(100)]
        public string PasswordHint { get; set; }
        public bool IsVerified { get; set; }

        [StringLength(200)]
        public string ProfileImageUrl { get; set; }

        [StringLength(70)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Mobile { get; set; }

        [StringLength(50)]
        public string IC { get; set; }

        [StringLength(50)]
        public string Passport { get; set; }

        public int Points { get; set; }

        [StringLength(250)]
        public string MemberTier { get; set; }

        public string MemberCard { get; set; }

        public bool? Gender { get; set; }

        public DateTime? DOB { get; set; }

        public bool IsMarried { get; set; }

        public DateTime? Anniversary { get; set; }

        public DateTime? MemberSince { get; set; }

        [StringLength(120)]
        public string HomeAddress { get; set; }

        [StringLength(120)]
        public string HomeCity { get; set; }

        [StringLength(120)]
        public string HomeCountry { get; set; }

        [StringLength(50)]
        public string HomePostalCode { get; set; }

        [StringLength(150)]
        public string OfficeAddress { get; set; }

        [StringLength(150)]
        public string OfficeCity { get; set; }

        [StringLength(150)]
        public string OfficeCountry { get; set; }

        [StringLength(50)]
        public string OfficePostalCode { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }
        public bool IsArtist { get; set; }
        public bool IsPDPA { get; set; }

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


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberLocation> MemberLocations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotificationSetting> NotificationSettings { get; set; }


    }
}
