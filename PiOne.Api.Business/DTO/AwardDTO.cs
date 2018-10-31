using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class AwardDTO
    {
        public string ID { get; set; }
        public string PoinsID { get; set; }
        public double Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public double RemainingValue { get; set; }
        public string ReceiptNo { get; set; }
     //   public ProductDTO ProductData { get; set; }
        public int AwardType { get; set; }
    }
}
