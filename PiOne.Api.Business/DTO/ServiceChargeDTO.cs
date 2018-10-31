using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class ServiceChargeDTO
    {
        public string ID { get; set; }
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public bool IsApplyForEatIn { get; set; }
        public bool IsApplyForTakeAway { get; set; }
        public bool IsIncludedOnBill { get; set; }
        public bool IsAllowTip { get; set; }
        public bool IsCurrency { get; set; }
        public double Value { get; set; }
    }
}
