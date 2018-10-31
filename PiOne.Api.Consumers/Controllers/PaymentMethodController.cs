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
    public class PaymentMethodController : BaseController
    {
        [HttpPost, Route(ApiRoutes.PaymentMethod_Get_External)]
        public async Task<NSApiResponse> PaymentMethodGetExternal(RequestPaymentMethodGetExternal request)
        {
            return await BUSPaymentMethod.Instance.PaymentMethodGetExternal(request);
        }
    }
}
