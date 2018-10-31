using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.DataModel.PiOneEntities
{
    [Table("MemberNotification")]
    public class MemberNotification
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MemberNotification() { }

        [StringLength(150)]
        public string ID { get; set; }

        [StringLength(150)]
        public string MerchantID { get; set; }

        [StringLength(150)]
        public string StoreID { get; set; }

        [StringLength(100)]
        public string ArtistID { get; set; }

        [StringLength(100)]
        public string EmployeeID { get; set; }

        [StringLength(150)]
        public string MemberID { get; set; }
        public int Type { get; set; }

        [StringLength(150)]
        public string GroupID { get; set; }

        [StringLength(450)]
        public string MessageSubject { get; set; }

        [StringLength(1500)]
        public string MessageContent { get; set; }

        public string JsonContent { get; set; }

        public bool IsRead { get; set; }
        public bool IsSenderView { get; set; }
        public bool IsMemberView { get; set; }

        public byte? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(150)]
        public string ModifiedBy { get; set; }

        public virtual Merchant Merchant { get; set; }

        public virtual Store Store { get; set; }

        public virtual Member Member { get; set; }
    }
}
