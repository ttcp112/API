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

namespace PiOne.Api.Consumers.Controllers
{
    public class PromotionController : BaseController
    {
        [HttpPost, Route(ApiRoutes.Promotion_Get_List)]
        public async Task<NSApiResponse> GetListPromotion(RequestModelBase request)
        {
            return await BUSPromotion.Instance.GetListPromotion(request);
        }

        [HttpPost, Route(ApiRoutes.Promotion_Get_Detail)]
        public async Task<NSApiResponse> GetPromotionDetail(RequestModelBase request)
        {
            return await BUSPromotion.Instance.GetPromotionDetail(request);
        }
    }
}