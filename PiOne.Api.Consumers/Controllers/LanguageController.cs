using PiOne.Api.Consumers.Business;
using PiOne.Api.Core.Controller;
using PiOne.Api.Core.Request;
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
    public class LanguageController : BaseController
    {
        [HttpPost, Route(ApiRoutes.Language_Get)]
        public async Task<NSApiResponse> GetLanguage(RequestModelBase request)
        {
            return await BUSLanguage.Instance.GetLanguage(request);
        }

        [HttpPost, Route(ApiRoutes.Language_Get_Detail)]
        public async Task<NSApiResponse> GetLanguageDetail(RequestModelBase request)
        {
            return await BUSLanguage.Instance.GetLanguageDetail(request);
        }
    }
}
