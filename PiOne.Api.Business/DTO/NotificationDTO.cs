using PiOne.Api.Core.Request;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class NotificationDTO
    {
        public string ID { get; set; }
        public int NotificationCode { get; set; }
        public string FunctionID { get; set; }
        public string ImageURL { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
    }

    public class NotificationSettingDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public bool IsReceive { get; set; }
    }

    //public class GroupNotificationDTO
    //{
    //    public int Month { get; set; }
    //    public int Year { get; set; }
    //    public List<NotificationDTO> ListNotificationDTO { get; set; }

    //    public GroupNotificationDTO()
    //    {
    //        ListNotificationDTO = new List<NotificationDTO>();
    //    }
    //}

    public class ResponseListNotification : NSApiResponseBase
    {
        public List<NotificationDTO> ListNotificationDTO { get; set; }

        public ResponseListNotification()
        {
            ListNotificationDTO = new List<NotificationDTO>();
        }
    }

    public class ResponseNotificationSetting : NSApiResponseBase
    {
        public List<NotificationSettingDTO> ListNotificationSetting { get; set; }

        public ResponseNotificationSetting()
        {
            ListNotificationSetting = new List<NotificationSettingDTO>();
        }
    }
}
