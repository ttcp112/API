using PiOne.Api.Business.Consumer.Business;
using PiOne.Api.Consumers.RequestDTO;
using PiOne.Api.Core.Request;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PiOne.Api.Consumers.Business
{
    public class BUSPromotion : NSBusPromotion
    {
        protected static BUSPromotion _instance;
        public static BUSPromotion Instance
        {
            get
            {
                _instance = _instance != null ? _instance : new BUSPromotion();
                return _instance;
            }
        }
        static BUSPromotion() { }

        public async Task<NSApiResponse> GetListPromotion(RequestModelBase input)
        {
            NSLog.Logger.Info("GetPromotion", input);
            return base.GetPromotion(input.StoreID);
        }

        public async Task<NSApiResponse> GetPromotionDetail(RequestModelBase input)
        {
            NSLog.Logger.Info("GetPromotionDetail", input);
            return base.GetPromotionDetail(input.StoreID, input.ID);
        }
    }
}