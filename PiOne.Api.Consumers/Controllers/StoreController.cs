using PiOne.Api.Consumers.Business;
using PiOne.Api.Consumers.RequestDTO;
using PiOne.Api.Core.Controller;
using PiOne.Api.Core.Request;
using PiOne.Api.Core.Response;
using PiOne.Api.Consumers.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using PiOne.Api.Business.DTO;

namespace PiOne.Api.Consumers.Controllers
{
    public class StoreController : BaseController
    {
        [HttpPost, Route(ApiRoutes.Store_GetInfo)]
        public async Task<NSApiResponse> GetStoreInfo(RequestModelBase request)
        {
            return await BUSStore.Instance.GetStoreInfo(request);
        }

        [HttpPost, Route(ApiRoutes.Store_ScanQR)]
        public async Task<NSApiResponse> ScanQRCode(RequestModelBase request)
        {
            return await BUSStore.Instance.ScanQRCode(request);
        }

        [HttpPost, Route(ApiRoutes.Merchant_Get_List_Search)]
        public async Task<NSApiResponse> SearchMerchant(RequestGetListMerchant request)
        {
            return await BUSStore.Instance.SearchMerchant(request);
        }

        [HttpPost, Route(ApiRoutes.Merchant_Get_Store_List)]
        public async Task<NSApiResponse> GetListStore(RequestGetListStore request)
        {
            return await BUSStore.Instance.GetListStore(request);
        }
    }
}