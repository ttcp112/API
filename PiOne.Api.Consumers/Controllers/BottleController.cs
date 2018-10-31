using PiOne.Api.Business.DTO;
using PiOne.Api.Consumers.Business;
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
    public class BottleController : BaseController
    {
        [HttpPost, Route(ApiRoutes.Bottle_Get_List)]
        public async Task<NSApiResponse> GetListBottle(GetListBottleRequest request)
        {
            return await BUSBottle.Instance.GetListBottle(request);
        }

        [HttpPost, Route(ApiRoutes.Bottle_Notify)]
        public async Task<NSApiResponse> BottlePushNoti(BottlePushNotiRequest request)
        {
            return await BUSBottle.Instance.BottlePushNoti(request);
        }
    }
}
