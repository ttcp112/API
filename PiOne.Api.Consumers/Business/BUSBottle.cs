using PiOne.Api.Business.Consumer.Business;
using PiOne.Api.Business.DTO;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PiOne.Api.Consumers.Business
{
    public class BUSBottle : NSBusBottle
    {
        protected static BUSBottle _instance;
        public static BUSBottle Instance
        {
            get
            {
                _instance = _instance != null ? _instance : new BUSBottle();
                return _instance;
            }
        }
        static BUSBottle() { }

        public async Task<NSApiResponse> GetListBottle(GetListBottleRequest input)
        {
            NSLog.Logger.Info("GetListBottle", input);
            return base.GetListBottle(input.MemberID);
        }

        public async Task<NSApiResponse> BottlePushNoti(BottlePushNotiRequest input)
        {
            NSLog.Logger.Info("BottlePushNoti", input);
            return base.BottlePushNoti(input.ListBottle, input.TypePush);
        }
    }
}