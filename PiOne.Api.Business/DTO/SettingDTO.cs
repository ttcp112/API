using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class SettingDTO
    {
        public string SettingID { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public int Code { get; set; }
    }

}
