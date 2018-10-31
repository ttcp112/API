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
    public class ChatController : BaseController
    {
        [HttpPost, Route(ApiRoutes.Chat_Get)]
        public async Task<NSApiResponse> GetChat(GetChatRequest request)
        {
            return await BUSChat.Instance.GetChat(request);
        }

        [HttpPost, Route(ApiRoutes.Chat_Get_Message)]
        public async Task<NSApiResponse> GetMessage(GetMessageRequest request)
        {
            return await BUSChat.Instance.GetMessage(request);
        }

        [HttpPost, Route(ApiRoutes.Chat_Send_Message)]
        public async Task<NSApiResponse> SendMessage(SendMessageRequest request)
        {
            return await BUSChat.Instance.SendMessage(request);
        }

        [HttpPost, Route(ApiRoutes.Chat_Send_Gift)]
        public async Task<NSApiResponse> SendGift(SendGiftRequest request)
        {
            return await BUSChat.Instance.SendGift(request);
        }

        [HttpPost, Route(ApiRoutes.Chat_Get_Template)]
        public async Task<NSApiResponse> GetTemplateForChat(GetChatTemplateForChatRequest request)
        {
            return await BUSChat.Instance.GetTemplateForChat(request);
        }
    }
}
