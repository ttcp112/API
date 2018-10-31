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
    public class ChatTemplateController : BaseController
    {
        [HttpPost, Route(ApiRoutes.ChattingTemplate_InsertOrUpdate)]
        public async Task<NSApiResponse> CreateOrUpdateChatTemplate(CreateOrEditChatTemplateRequest request)
        {
            return await BUSChatTemplate.Instance.CreateOrUpdateChatTemplate(request);
        }

        [HttpPost, Route(ApiRoutes.ChattingTemplate_Delete)]
        public async Task<NSApiResponse> DeleteChatTemplate(DeleteChatTemplateRequest request)
        {
            return await BUSChatTemplate.Instance.DeleteChatTemplate(request);
        }

        [HttpPost, Route(ApiRoutes.ChattingTemplate_Get)]
        public async Task<NSApiResponse> GetChatTemplate(GetChatTemplateRequest request)
        {
            return await BUSChatTemplate.Instance.GetChatTemplate(request);
        }

        [HttpPost, Route(ApiRoutes.ChattingTemplate_Import)]
        public async Task<NSApiResponse> ImportChatTemplate(ImportChatTemplateRequest request)
        {
            return await BUSChatTemplate.Instance.ImportChatTemplate(request);
        }

        [HttpPost, Route(ApiRoutes.ChattingTemplate_Export)]
        public async Task<NSApiResponse> ExportChatTemplate(ExportChatTemplateRequest request)
        {
            return await BUSChatTemplate.Instance.ExportChatTemplate(request);
        }
    }
}
