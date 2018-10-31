using PiOne.Api.Business.Consumer.Business;
using PiOne.Api.Business.DTO;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PiOne.Api.Consumers.Business
{
    public class BUSChat : NSBusChat
    {
        protected static BUSChat _instance;
        public static BUSChat Instance
        {
            get
            {
                _instance = _instance != null ? _instance : new BUSChat();
                return _instance;
            }
        }
        static BUSChat() { }

        public async Task<NSApiResponse> GetChat(GetChatRequest input)
        {
            NSLog.Logger.Info("GetChat", input);
            return base.GetChat(input.MemberID, input.LanguageID);
        }

        public async Task<NSApiResponse> GetMessage(GetMessageRequest input)
        {
            NSLog.Logger.Info("GetMessage", input);
            return base.GetMessage(input.MemberID, input.ArtistID, input.IsCustomer, input.LanguageID, input.PageSize, input.PageIndex);
        }

        public async Task<NSApiResponse> SendMessage(SendMessageRequest input)
        {
            NSLog.Logger.Info("SendMessage", input);
            return base.SendMessage(input.Message, input.IsCustomer, input.DeviceName);
        }

        public async Task<NSApiResponse> SendGift(SendGiftRequest input)
        {
            NSLog.Logger.Info("SendGift", input);
            return base.SendGift(input.Message);
        }

        public async Task<NSApiResponse> GetTemplateForChat(GetChatTemplateForChatRequest input)
        {
            NSLog.Logger.Info("GetTemplateForChat", input);
            return base.GetTemplateForChat(input.ArtistID, input.ChatTemplateType);
        }
    }
}