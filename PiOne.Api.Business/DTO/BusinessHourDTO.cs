using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class BusinessHourDTO
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public int DayOfWeek { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public bool IsOff { get; set; }
        public List<BusinessHourDTO> ListChild { get; set; }
    }
}
