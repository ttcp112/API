using PiOne.Api.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class NotiInfo
    {
        public string DeviceToken { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string MemberID { get; set; }
        public string ArtistID { get; set; }
        public int Badge { get; set; }
        public int CustomerNo { get; set; }
        public int DeviceType { get; set; }
        public int MessageType { get; set; }

        public NotiInfo()
        {
            DeviceType = (int)Constants.EDeviceType.iOS;
        }
    }

    #region iOS
    public class AppleNotificationObj
    {
        public Aps aps { get; set; }
        public string member_id { get; set; }
        public string artist_id { get; set; }
        public int content_code { get; set; }
        public int customer_no { get; set; }
        public AppleNotificationObj()
        {
            aps = new Aps();
            member_id = "";
            artist_id = "";
            content_code = 0;
            customer_no = 0;
        }

        public override string ToString()
        {
            return "{" + string.Format("\"aps\":{0},\"member_id\":\"{1}\",\"artist_id\":\"{2}\",\"content_code\":{3},\"customer_no\":{4}", aps.ToString(), member_id, artist_id, content_code, customer_no) + "}";
        }
    }

    public class Aps
    {
        public Alert alert { get; set; }
        public int badge { get; set; }
        public string sound { get; set; }
        public int content_available { get; set; }
        public Aps()
        {
            alert = new Alert();
            badge = 1;
            sound = "default";
            content_available = 1;
        }

        public override string ToString()
        {
            return "{" + string.Format("\"alert\":{0},\"badge\":{1},\"sound\":\"{2}\",\"content-available\":{3}", alert.ToString(), badge, sound, content_available) + "}";
        }
    }

    public class Alert
    {
        public string title { get; set; }
        public string body { get; set; }

        public Alert()
        {
            title = "";
            body = "";
        }

        public override string ToString()
        {
            return "{" + string.Format("\"title\":\"{0}\",\"body\":\"{1}\"", title, body) + "}";
        }
    }
    #endregion

    #region Android
    public class FCMNotification
    {
        public string title { get; set; }
        public string body { get; set; }
        public string sound { get; set; }
        public FCMNotification()
        {
            title = "";
            body = "";
            sound = "default";
        }
    }

    public class FCMData
    {
        public string member_id { get; set; }
        public string artist_id { get; set; }
        public int content_code { get; set; }
        public int customer_no { get; set; }
        public int badge { get; set; }

        public string title { get; set; }
        public string body { get; set; }
        public string sound { get; set; }

        public FCMData()
        {
            member_id = "";
            artist_id = "";
            content_code = 0;
            customer_no = 0;
            badge = 1;

            title = "";
            body = "";
            sound = "default";
        }
    }

    public class AndroidNotificationObject
    {
        public bool content_available { get; set; }
        public string collapse_key { get; set; }
        public int time_to_live { get; set; }
        public bool delay_while_idle { get; set; }
        public string priority { get; set; }
        public List<string> registration_ids { get; set; }
        //public FCMNotification notification { get; set; }
        public FCMData data { get; set; }

        public AndroidNotificationObject()
        {
            content_available = true;
            collapse_key = "score_update";
            time_to_live = 10;
            delay_while_idle = true;
            priority = "normal";
            registration_ids = new List<string>();
            data = new FCMData();
            //notification = new FCMNotification();
        }
    }

    public class AndroidNotificationResponse
    {
        public Int64 multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<ResultAndroid> results { get; set; }
    }

    public class ResultAndroid
    {
        public string error { get; set; }
        public string message_id { get; set; }
    }
    #endregion
}
