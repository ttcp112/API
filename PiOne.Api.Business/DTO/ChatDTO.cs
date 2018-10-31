using PiOne.Api.Core.Request;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class ChatDTO
    {
        public string MemberID { get; set; }
        public string ArtistID { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public int NoOfMessage { get; set; }
        public bool IsSystem { get; set; }
    }

    public class MessageDTO
    {
        public string MemberID { get; set; }
        public string ArtistID { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public bool IsCustomer { get; set; }
        public string Title { get; set; }
        public Dictionary<int, string> Dict { get; set; }
    }

    public class GetChatRequest : RequestModelBase { }

    public class GetChatTemplateForChatRequest
    {
        public string ArtistID { get; set; }
        public int ChatTemplateType { get; set; }

    }

    public class GetMessageRequest : RequestModelBase
    {
        public string ArtistID { get; set; }
        public bool IsCustomer { get; set; }
    }

    public class SendMessageRequest : RequestModelBase
    {
        public MessageDTO Message { get; set; }
        public bool IsCustomer { get; set; }
    }

    public class SendGiftRequest : RequestModelBase
    {
        public MessageDTO Message { get; set; }
    }

    public class GetChatResponse : NSApiResponseBase
    {
        public List<ChatDTO> ListChat { get; set; }

        public GetChatResponse()
        {
            ListChat = new List<ChatDTO>();
        }
    }

    public class GetMessageResponse : NSApiResponseBase
    {
        public string ArtistImageUrl { get; set; }
        public string MemberImageUrl { get; set; }
        public List<MessageDTO> ListMessage { get; set; }

        public GetMessageResponse()
        {
            ListMessage = new List<MessageDTO>();
        }
    }
}
