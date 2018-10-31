using PiOne.Api.Consumers.Business;
using PiOne.Api.Consumers.RequestDTO;
using PiOne.Api.Core.Controller;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PiOne.Api.Consumers.Controllers
{
    public class OrderController : BaseController
    {
        [HttpPost, Route(ApiRoutes.Order_Create)]
        public async Task<NSApiResponse> OrderCreate(RequestOrderCreate request)
        {
            return await BUSOrder.Instance.OrderCreate(request);
        }

        [HttpPost, Route(ApiRoutes.Order_CheckDrawer)]
        public async Task<NSApiResponse> OrderCheckDrawer(RequestOrderCheckDrawer request)
        {
            return await BUSOrder.Instance.OrderCheckDrawer(request);
        }

        [HttpPost, Route(ApiRoutes.Order_Delete)]
        public async Task<NSApiResponse> OrderDelete(RequestOrderDelete request)
        {
            return await BUSOrder.Instance.OrderDelete(request);
        }

        [HttpPost, Route(ApiRoutes.Order_Add_Item)]
        public async Task<NSApiResponse> OrderAddItem(RequestOrderAddItem request)
        {
            return await BUSOrder.Instance.OrderAddItem(request);
        }

        [HttpPost, Route(ApiRoutes.Order_Get_Detail)]
        public async Task<NSApiResponse> OrderGetDeail(RequestOrderGetDetail request)
        {
            return await BUSOrder.Instance.OrderGetDetail(request);
        }

        [HttpPost, Route(ApiRoutes.Order_ChangeStateTable)]
        public async Task<NSApiResponse> OrderChangeStateTable(RequestOrderChangeStateTable request)
        {
            return await BUSOrder.Instance.OrderChangeStateTable(request);
        }

        [HttpPost, Route(ApiRoutes.Order_Pay_ByExternal)]
        public async Task<NSApiResponse> OrderPayByExternal(RequestOrderPayByExternal request)
        {
            return await BUSOrder.Instance.OrderPayByExternal(request);
        }

        [HttpPost, Route(ApiRoutes.Order_Done)]
        public async Task<NSApiResponse> OrderDone(RequestOrderDone request)
        {
            return await BUSOrder.Instance.OrderDone(request);
        }

        [HttpPost, Route(ApiRoutes.Order_Sync)]
        public async Task<NSApiResponse> OrderSync(RequestOrderSync request)
        {
            return await BUSOrder.Instance.OrderSync(request);
        }

        [HttpPost, Route(ApiRoutes.Order_Temp_Done)]
        public async Task<NSApiResponse> OrderTempDone(DoneTempOrderRequest request)
        {
            return await BUSOrder.Instance.OrderTempDone(request);
        }
    }
}
