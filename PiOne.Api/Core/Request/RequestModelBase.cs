using PiOne.Api.Common;
using PiOne.Api.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Core.Request
{
    public class RequestModelBase : BaseModel
    {
        public string CreatedUser { get; set; }
        public string MerchantID { get; set; }
        public string StoreID { get; set; }
        public string ID { get; set; }
        public string MemberID { get; set; }
        public string DeviceName { get; set; }
        public string UUID { get; set; }
        public bool IsWithNupos { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string LanguageID { get; set; }
        public int DeviceType { get; set; }
        public string DeviceToken { get; set; }
    }

    public class RequestWalletBase : BaseModel
    {
        public int DeviceType { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceName { get; set; }
        public string User { get; set; }
        public DateTime DateRequest { get; set; }
        public string PoinSID { get; set; }
        public string ID { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string LanguageID { get; set; }
    }


}
