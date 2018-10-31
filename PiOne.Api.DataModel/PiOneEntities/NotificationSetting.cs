using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.DataModel.PiOneEntities
{
    [Table("NotificationSetting")]
    public class NotificationSetting
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NotificationSetting() { }

        [StringLength(150)]
        public string ID { get; set; }

        [StringLength(150)]
        public string MerchantID { get; set; }

        [StringLength(150)]
        public string StoreID { get; set; }

        [StringLength(150)]
        public string MemberID { get; set; }

        public bool IsReceiveNotification { get; set; }

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
