using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class CategoryDTO
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public string ParentName { get; set; }
        public string GLCode { get; set; }
        public int CategoryType { get; set; }
        public string ImageURL { get; set; }
        public string ImageData { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsShowInReservation { get; set; }
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public bool IsTransaction { get; set; }
        public bool IsIncludeGCPay { get; set; } //default: not include-> false
        public bool IsIncludeGCSale { get; set; }//default: include-> false
        public int Sequence { get; set; }
        public int TotalProduct { get; set; }
        public List<CategoryDTO> ListChild { get; set; }
    }
}
