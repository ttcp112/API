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
    public class BUSChatTemplate : NSBusChatTemplate
    {
        protected static BUSChatTemplate _instance;
        public static BUSChatTemplate Instance
        {
            get
            {
                _instance = _instance != null ? _instance : new BUSChatTemplate();
                return _instance;
            }
        }
        static BUSChatTemplate() { }

        public async Task<NSApiResponse> CreateOrUpdateChatTemplate(CreateOrEditChatTemplateRequest input)
        {
            NSLog.Logger.Info("CreateOrUpdateChatTemplate", input);
            return base.CreateOrUpdateChatTemplate(input.ChatTemplateDTO, input.ListStoreID, input.CreatedUser);
        }

        public async Task<NSApiResponse> DeleteChatTemplate(DeleteChatTemplateRequest input)
        {
            NSLog.Logger.Info("DeleteChatTemplate", input);
            return base.DeleteChatTemplate(input.ID, input.CreatedUser);
        }

        public async Task<NSApiResponse> GetChatTemplate(GetChatTemplateRequest input)
        {
            NSLog.Logger.Info("GetChatTemplate", input);
            return base.GetChatTemplate(input.ID, input.ListStoreID, input.IsActive, input.ChatTemplateType);
        }

        public async Task<NSApiResponse> ImportChatTemplate(ImportChatTemplateRequest input)
        {
            NSLog.Logger.Info("ImportChatTemplate", input);
            return base.ImportChatTemplate(input.ListTemplate, input.ListStoreID, input.CreatedUser);
        }

        public async Task<NSApiResponse> ExportChatTemplate(ExportChatTemplateRequest input)
        {
            NSLog.Logger.Info("ExportChatTemplate", input);
            return base.ExportChatTemplate(input.ListStoreID, input.ChatTemplateType);
        }
    }
}