using PiOne.Api.Core.Request;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class ChatTemplateDTO
    {
        public int Index { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int ChatTemplateType { get; set; }
    }

    public class CreateOrEditChatTemplateRequest
    {
        public string CreatedUser { get; set; }
        public List<string> ListStoreID { get; set; }
        public ChatTemplateDTO ChatTemplateDTO { get; set; }
    }

    public class ImportChatTemplateRequest
    {
        public string CreatedUser { get; set; }
        public List<string> ListStoreID { get; set; }
        public List<ChatTemplateDTO> ListTemplate { get; set; }
    }

    public class ExportChatTemplateRequest
    {
        public List<string> ListStoreID { get; set; }
        public int ChatTemplateType { get; set; }
    }

    public class GetChatTemplateRequest
    {
        public string ID { get; set; }
        public List<string> ListStoreID { get; set; }
        public bool IsActive { get; set; }
        public int ChatTemplateType { get; set; }
    }

    public class DeleteChatTemplateRequest
    {
        public string ID { get; set; }
        public string CreatedUser { get; set; }
    }

    public class GetChatTemplateResponse : NSApiResponseBase
    {
        public List<ChatTemplateDTO> ListChatTemplate { get; set; }

        public GetChatTemplateResponse()
        {
            ListChatTemplate = new List<ChatTemplateDTO>();
        }
    }
}
