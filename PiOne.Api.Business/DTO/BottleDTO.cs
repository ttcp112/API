using PiOne.Api.Core.Request;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class BottleIntegrateDTO
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double Remaining { get; set; }
        public int BottleStatus { get; set; }
        public bool IsRemind { get; set; }
        public int RemindDays { get; set; }
        public bool IsExpire { get; set; }
        public string MemberID { get; set; }
    }

    public class GetListBottleResponse : NSApiResponseBase
    {
        public List<BottleIntegrateDTO> ListBottle { get; set; }

        public GetListBottleResponse()
        {
            ListBottle = new List<BottleIntegrateDTO>();
        }
    }

    public class GetListBottleRequest : RequestModelBase { }

    public class BottlePushNotiRequest
    {
        public List<BottleIntegrateDTO> ListBottle { get; set; }
        public int TypePush { get; set; }
    }
}
