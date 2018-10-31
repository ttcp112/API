using PiOne.Api.Business.Consumer.Business;
using PiOne.Api.Business.DTO;
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
    public class BUSStore : NSBusStore
    {
        protected static BUSStore _instance;
        public static BUSStore Instance
        {
            get
            {
                _instance = _instance != null ? _instance : new BUSStore();
                return _instance;
            }
        }
        static BUSStore() { }

        public async Task<NSApiResponse> GetStoreInfo(RequestModelBase input)
        {
            NSLog.Logger.Info("GetStoreInfo", input);
            return base.GetStoreInfo(input.StoreID, input.MerchantID);
        }

        public async Task<NSApiResponse> ScanQRCode(RequestModelBase input)
        {
            NSLog.Logger.Info("ScanQRCode", input);
            return base.ScanQRCode(input.ID, input.MemberID, input.StoreID);
        }

        public async Task<NSApiResponse> SearchMerchant(RequestGetListMerchant input)
        {
            NSLog.Logger.Info("SearchMerchant", input);
            return base.SearchMerchant(input);
        }

        public async Task<NSApiResponse> GetListStore(RequestGetListStore input)
        {
            NSLog.Logger.Info("GetListStore", input);
            return base.GetListStore(input);
        }
    }
}