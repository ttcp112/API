using PiOne.Api.Business.Consumer.Business;
using PiOne.Api.Consumers.RequestDTO;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PiOne.Api.Consumers.Business
{
    public class BUSPaymentMethod: NSBusPaymentMethod
    {
        protected static BUSPaymentMethod _instance;
        public static BUSPaymentMethod Instance
        {
            get
            {
                _instance = _instance != null ? _instance : new BUSPaymentMethod();
                return _instance;
            }
        }

        static BUSPaymentMethod() { }

        public async Task<NSApiResponse> PaymentMethodGetExternal(RequestPaymentMethodGetExternal input)
        {
            NSLog.Logger.Info("PaymentMethodGetExternal", input);
            return base.PaymentMethodGetExternal(input.StoreID);
        }
    }
}