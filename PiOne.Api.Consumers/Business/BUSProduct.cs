using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PiOne.Api.Business.Consumer.Business;
using PiOne.Api.Core.Response;
using System.Threading.Tasks;
using PiOne.Api.Consumers.RequestDTO;

namespace PiOne.Api.Consumers.Business
{
    public class BUSProduct : NSBusProduct
    {
        protected static BUSProduct _instance;
        public static BUSProduct Instance
        {
            get
            {
                _instance = _instance != null ? _instance : new BUSProduct();
                return _instance;
            }
        }
        static BUSProduct() { }


        public async Task<NSApiResponse> ProductGetListForOrder(RequestGetListProductForOrder input)
        {
            NSLog.Logger.Info("ProductGetListForOrder", input);
            return base.ProductGetListForOrder(input.MerchantID, input.StoreID);
        }
    }
}