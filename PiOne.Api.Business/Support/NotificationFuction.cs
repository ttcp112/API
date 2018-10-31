using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PiOne.Api.Business.DTO;
using PiOne.Api.Common;
using PiOne.Api.DataModel.Context;
using PiOne.Api.DataModel.PiOneEntities;
using PushSharp.Apple;
using PushSharp.Google;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiOne.Api.Business.Support
{
    public class NotificationFuction
    {
        public static void PushNoti_Bottle(string memberID, string title, string message, Dictionary<int, string> dictionary = null)
        {
            try
            {
                using (var _db = new PiOneDb())
                {
                    byte delete = (byte)Constants.EStatus.Deleted;

                    MemberNotification noti = new MemberNotification()
                    {
                        ID = Guid.NewGuid().ToString(),
                        MemberID = memberID,
                        Type = (byte)Constants.EMessageType.Bottle,
                        MessageSubject = title,
                        MessageContent = message,
                        JsonContent = JsonConvert.SerializeObject(dictionary),
                        IsRead = false,
                        IsSenderView = true,
                        IsMemberView = false,
                        Status = (byte)Constants.EStatus.Actived,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = memberID,
                        ModifiedDate = DateTime.UtcNow,
                        ModifiedBy = memberID,
                    };
                    _db.MemberNotifications.Add(noti);

                    if (_db.SaveChanges() > 0)
                    {
                        bool isPush = _db.NotificationSettings.Where(o => o.MemberID == memberID && o.Status != delete).Select(o => o.IsReceiveNotification).FirstOrDefault();

                        if (isPush) /* push noti */
                        {
                            var listDevice = _db.Devices.Where(o => o.MemberID == memberID && !o.IsDeleted && o.Status != delete).ToList();
                            if (listDevice.Count > 0)
                            {
                                List<int> listChatType = new List<int>() { (int)Constants.EMessageType.Chat, (int)Constants.EMessageType.Gift };
                                List<int> listFuncType = new List<int>() { (int)Constants.EMessageType.Membership, (int)Constants.EMessageType.Bottle };
                                int badge = _db.MemberNotifications.Where(o => ((listFuncType.Contains(o.Type) && o.MemberID == memberID && !o.IsMemberView) || (listChatType.Contains(o.Type) && o.ArtistID == memberID && !o.IsSenderView)) && o.Status != delete).Select(o => o.ID).Count();
                                int customerNo = _db.MemberNotifications.Where(o => o.ArtistID == memberID && o.Status != delete && !o.IsSenderView && listChatType.Contains(o.Type)).Select(o => o.MemberID).Distinct().Count();

                                List<NotiInfo> listNotiInfo = new List<NotiInfo>();
                                foreach (var device in listDevice)
                                {
                                    NotiInfo notiInfo = new NotiInfo()
                                    {
                                        DeviceToken = device.DeviceToken,
                                        DeviceType = device.DeviceType,
                                        Title = LanguageFuction.GetMultiLanguage(device.LanguageID, title),
                                        Message = LanguageFuction.GetMultiLanguage(device.LanguageID, message, dictionary),
                                        Badge = badge,
                                        CustomerNo = customerNo,
                                        ArtistID = "System",
                                        MemberID = memberID,
                                        MessageType = (int)Constants.EMessageType.Bottle,
                                    };
                                    listNotiInfo.Add(notiInfo);
                                }

                                if (listNotiInfo.Count > 0) PushNotifications(listNotiInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorPushNoti_SendGift", ex); }
        }

        public static void PushNoti_Membership(string memberID, string title, string message, Dictionary<int, string> dictionary = null)
        {
            try
            {
                using (var _db = new PiOneDb())
                {
                    byte delete = (byte)Constants.EStatus.Deleted;

                    MemberNotification noti = new MemberNotification()
                    {
                        ID = Guid.NewGuid().ToString(),
                        MemberID = memberID,
                        Type = (byte)Constants.EMessageType.Membership,
                        MessageSubject = title,
                        MessageContent = message,
                        JsonContent = JsonConvert.SerializeObject(dictionary),
                        IsRead = false,
                        IsSenderView = true,
                        IsMemberView = false,
                        Status = (byte)Constants.EStatus.Actived,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = memberID,
                        ModifiedDate = DateTime.UtcNow,
                        ModifiedBy = memberID,
                    };
                    _db.MemberNotifications.Add(noti);

                    if (_db.SaveChanges() > 0)
                    {
                        bool isPush = _db.NotificationSettings.Where(o => o.MemberID == memberID && o.Status != delete).Select(o => o.IsReceiveNotification).FirstOrDefault();
                        if (isPush)
                        {
                            var listDevice = _db.Devices.Where(o => o.MemberID == memberID && !o.IsDeleted && o.Status != delete).ToList();
                            if (listDevice.Count > 0)
                            {
                                List<int> listChatType = new List<int>() { (int)Constants.EMessageType.Chat, (int)Constants.EMessageType.Gift };
                                List<int> listFuncType = new List<int>() { (int)Constants.EMessageType.Membership, (int)Constants.EMessageType.Bottle };
                                int badge = _db.MemberNotifications.Where(o => ((listFuncType.Contains(o.Type) && o.MemberID == memberID && !o.IsMemberView) || (listChatType.Contains(o.Type) && ((o.MemberID == memberID && !o.IsMemberView) || (o.ArtistID == memberID && !o.IsSenderView)))) && o.Status != delete).Select(o => o.ID).Count();

                                List<NotiInfo> listNotiInfo = new List<NotiInfo>();
                                foreach (var device in listDevice)
                                {
                                    NotiInfo notiInfo = new NotiInfo()
                                    {
                                        DeviceToken = device.DeviceToken,
                                        DeviceType = device.DeviceType,
                                        Title = "System",
                                        Message = LanguageFuction.GetMultiLanguage(device.LanguageID, message, dictionary),
                                        Badge = badge,
                                        ArtistID = "System",
                                        MemberID = memberID,
                                        MessageType = (int)Constants.EMessageType.Membership,
                                    };
                                    listNotiInfo.Add(notiInfo);
                                }

                                if (listNotiInfo.Count > 0) PushNotifications(listNotiInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorPushNoti_Membership", ex); }
        }

        public static void PushNoti_SendGift(string message, string memberID, string artistID, Dictionary<int, string> dictionary = null)
        {
            try
            {
                using (var _db = new PiOneDb())
                {
                    byte delete = (byte)Constants.EStatus.Deleted;

                    /* save message content */
                    MemberNotification noti = new MemberNotification()
                    {
                        ID = Guid.NewGuid().ToString(),
                        ArtistID = artistID,
                        MemberID = string.IsNullOrEmpty(memberID) ? null : memberID,
                        Type = (byte)Constants.EMessageType.Gift,
                        MessageSubject = "",
                        MessageContent = message,
                        JsonContent = JsonConvert.SerializeObject(dictionary),
                        IsRead = false,
                        IsSenderView = false,
                        IsMemberView = true,
                        Status = (byte)Constants.EStatus.Actived,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = memberID,
                        ModifiedDate = DateTime.UtcNow,
                        ModifiedBy = memberID,
                    };
                    _db.MemberNotifications.Add(noti);

                    if (_db.SaveChanges() > 0)
                    {
                        bool isPush = _db.NotificationSettings.Where(o => o.MemberID == artistID && o.Status != delete).Select(o => o.IsReceiveNotification).FirstOrDefault();

                        if (isPush) /* push noti */
                        {
                            var listDevice = _db.Devices.Where(o => o.MemberID == artistID && !o.IsDeleted && o.Status != delete).ToList();
                            if (listDevice.Count > 0)
                            {
                                string memName = _db.Members.Where(o => o.ID == memberID).Select(o => o.Name).FirstOrDefault();

                                List<int> listChatType = new List<int>() { (int)Constants.EMessageType.Chat, (int)Constants.EMessageType.Gift };
                                List<int> listFuncType = new List<int>() { (int)Constants.EMessageType.Membership, (int)Constants.EMessageType.Bottle };
                                int badge = _db.MemberNotifications.Where(o => ((listFuncType.Contains(o.Type) && o.MemberID == artistID && !o.IsMemberView) || (listChatType.Contains(o.Type) && o.ArtistID == artistID && !o.IsSenderView)) && o.Status != delete).Select(o => o.ID).Count();
                                int customerNo = _db.MemberNotifications.Where(o => o.ArtistID == artistID && o.Status != delete && !o.IsSenderView && listChatType.Contains(o.Type)).Select(o => o.MemberID).Distinct().Count();

                                List<NotiInfo> listNotiInfo = new List<NotiInfo>();
                                foreach (var device in listDevice)
                                {
                                    NotiInfo notiInfo = new NotiInfo()
                                    {
                                        DeviceToken = device.DeviceToken,
                                        DeviceType = device.DeviceType,
                                        Title = memName,
                                        Message = LanguageFuction.GetMultiLanguage(device.LanguageID, message, dictionary),
                                        Badge = badge,
                                        CustomerNo = customerNo,
                                        ArtistID = artistID,
                                        MemberID = memberID,
                                        MessageType = (int)Constants.EMessageType.Gift,
                                    };
                                    listNotiInfo.Add(notiInfo);
                                }

                                if (listNotiInfo.Count > 0) PushNotifications(listNotiInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorPushNoti_SendGift", ex); }
        }

        public static void PushNoti_SendMessage(string message, string memberID, string artistID, bool isCustomer)
        {
            try
            {
                using (var _db = new PiOneDb())
                {
                    byte delete = (byte)Constants.EStatus.Deleted;
                    string tempID = isCustomer ? artistID : memberID;
                    string memName = "";
                    int badge = 0;
                    int customerNo = 0;

                    bool isPush = _db.NotificationSettings.Where(o => o.MemberID == tempID && o.Status != delete).Select(o => o.IsReceiveNotification).FirstOrDefault();
                    if (isPush)
                    {
                        var listDevice = _db.Devices.Where(o => o.MemberID == tempID && !o.IsDeleted && o.Status != delete).ToList();
                        if (listDevice.Count > 0)
                        {
                            List<int> listChatType = new List<int>() { (int)Constants.EMessageType.Chat, (int)Constants.EMessageType.Gift };
                            List<int> listFuncType = new List<int>() { (int)Constants.EMessageType.Membership, (int)Constants.EMessageType.Bottle };

                            if (isCustomer)
                            {
                                badge = _db.MemberNotifications.Where(o => ((listFuncType.Contains(o.Type) && o.MemberID == artistID && !o.IsMemberView) || (listChatType.Contains(o.Type) && o.ArtistID == artistID && !o.IsSenderView)) && o.Status != delete).Select(o => o.ID).Count();
                                memName = _db.Members.Where(o => o.ID == memberID).Select(o => o.Name).FirstOrDefault();
                                customerNo = _db.MemberNotifications.Where(o => o.ArtistID == artistID && o.Status != delete && !o.IsSenderView && listChatType.Contains(o.Type)).Select(o => o.MemberID).Distinct().Count();
                            }
                            else
                            {
                                badge = _db.MemberNotifications.Where(o => ((listFuncType.Contains(o.Type) && o.MemberID == memberID && !o.IsMemberView) || (listChatType.Contains(o.Type) && o.MemberID == memberID && !o.IsMemberView)) && o.Status != delete).Select(o => o.ID).Count();
                                memName = _db.Members.Where(o => o.ID == artistID).Select(o => o.Name).FirstOrDefault();
                            }

                            List<NotiInfo> listNotiInfo = new List<NotiInfo>();
                            foreach (var device in listDevice)
                            {
                                NotiInfo notiInfo = new NotiInfo()
                                {
                                    DeviceToken = device.DeviceToken,
                                    DeviceType = device.DeviceType,
                                    Title = memName,
                                    Message = LanguageFuction.GetMultiLanguage(device.LanguageID, message),
                                    Badge = badge,
                                    CustomerNo = customerNo,
                                    ArtistID = artistID,
                                    MemberID = memberID,
                                    MessageType = (int)Constants.EMessageType.Chat,
                                };
                                listNotiInfo.Add(notiInfo);
                            }

                            if (listNotiInfo.Count > 0) PushNotifications(listNotiInfo);
                        }
                    }
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorPushNoti_SendMessage", ex); }
        }

        public static void PushNoti_Booking(string memberID, string title, string message, int numOfBooking, string bookingID, Dictionary<int, string> dictionary = null)
        {
            try
            {
                using (var _db = new PiOneDb())
                {
                    byte delete = (byte)Constants.EStatus.Deleted;

                    MemberNotification noti = new MemberNotification()
                    {
                        ID = Guid.NewGuid().ToString(),
                        MemberID = memberID,
                        Type = (byte)Constants.EMessageType.Booking,
                        MessageSubject = title,
                        MessageContent = message,
                        JsonContent = JsonConvert.SerializeObject(dictionary),
                        IsRead = false,
                        IsSenderView = true,
                        IsMemberView = false,
                        Status = (byte)Constants.EStatus.Actived,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = memberID,
                        ModifiedDate = DateTime.UtcNow,
                        ModifiedBy = memberID,
                    };
                    _db.MemberNotifications.Add(noti);

                    if (_db.SaveChanges() > 0)
                    {
                        bool isPush = _db.NotificationSettings.Where(o => o.MemberID == memberID && o.Status != delete).Select(o => o.IsReceiveNotification).FirstOrDefault();
                        if (isPush)
                        {
                            var listDevice = _db.Devices.Where(o => o.MemberID == memberID && !o.IsDeleted && o.Status != delete).ToList();
                            if (listDevice.Count > 0)
                            {
                                List<int> listChatType = new List<int>() { (int)Constants.EMessageType.Chat, (int)Constants.EMessageType.Gift };
                                List<int> listFuncType = new List<int>() { (int)Constants.EMessageType.Membership, (int)Constants.EMessageType.Bottle };
                                int badge = _db.MemberNotifications.Where(o => ((listFuncType.Contains(o.Type) && o.MemberID == memberID && !o.IsMemberView) || (listChatType.Contains(o.Type) && ((o.MemberID == memberID && !o.IsMemberView) || (o.ArtistID == memberID && !o.IsSenderView)))) && o.Status != delete).Select(o => o.ID).Count();

                                List<NotiInfo> listNotiInfo = new List<NotiInfo>();
                                foreach (var device in listDevice)
                                {
                                    NotiInfo notiInfo = new NotiInfo()
                                    {
                                        DeviceToken = device.DeviceToken,
                                        DeviceType = device.DeviceType,
                                        Title = "System",
                                        Message = LanguageFuction.GetMultiLanguage(device.LanguageID, message, dictionary),
                                        Badge = badge,
                                        CustomerNo = numOfBooking,
                                        ArtistID = "System",//bookingID, // id of booking -> click goto detail
                                        MemberID = memberID,
                                        MessageType = (int)Constants.EMessageType.Booking,
                                    };
                                    listNotiInfo.Add(notiInfo);
                                }

                                if (listNotiInfo.Count > 0) PushNotifications(listNotiInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorPushNoti_Booking", ex); }
        }

        public static void PushNotifications(List<NotiInfo> listNotiInfo)
        {
            try
            {
                if (listNotiInfo.Count > 0)
                {
                    NSLog.Logger.Info("PushNotifications:", listNotiInfo);
                    var listType = listNotiInfo.Select(o => o.DeviceType).Distinct().ToList();
                    foreach (var type in listType)
                    {
                        var listNoti = listNotiInfo.Where(o => o.DeviceType == type).ToList();
                        switch (type)
                        {
                            case (int)Constants.EDeviceType.iOS:
                                string certificates = ConfigurationManager.AppSettings["Certificates"];
                                string pass = ConfigurationManager.AppSettings["CertPassword"];
                                if (!string.IsNullOrEmpty(certificates) && !string.IsNullOrEmpty(pass))
                                {
                                    new Thread(() =>
                                    {
                                        byte[] appleCert = new System.Net.WebClient().DownloadData(certificates);
                                        var apnsConfig = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox, appleCert, pass);

                                        var apnsBroker = new ApnsServiceBroker(apnsConfig);
                                        apnsBroker.OnNotificationSucceeded += ApnsBroker_OnNotificationSucceeded;
                                        apnsBroker.OnNotificationFailed += ApnsBroker_OnNotificationFailed;
                                        apnsBroker.Start();

                                        foreach (var item in listNoti)
                                        {
                                            apnsBroker.QueueNotification(new ApnsNotification()
                                            {
                                                DeviceToken = item.DeviceToken,   /* token of device to push */
                                                Payload = JObject.Parse(GetJsonPushNotification(item))
                                            });
                                        }
                                        apnsBroker.Stop();
                                    }).Start();

                                    new Thread(() =>
                                    {
                                        byte[] appleCert = new System.Net.WebClient().DownloadData(certificates);
                                        var apnsConfig = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production, appleCert, pass);

                                        var apnsBroker = new ApnsServiceBroker(apnsConfig);
                                        apnsBroker.OnNotificationSucceeded += ApnsBroker_OnNotificationSucceeded;
                                        apnsBroker.OnNotificationFailed += ApnsBroker_OnNotificationFailed;
                                        apnsBroker.Start();

                                        foreach (var item in listNoti)
                                        {
                                            apnsBroker.QueueNotification(new ApnsNotification()
                                            {
                                                DeviceToken = item.DeviceToken,   /* token of device to push */
                                                Payload = JObject.Parse(GetJsonPushNotification(item))
                                            });
                                        }
                                        apnsBroker.Stop();
                                    }).Start();
                                }
                                break;

                            case (int)Constants.EDeviceType.Android:
                                string SERVER_API_KEY = ConfigurationManager.AppSettings["ServerApiKey"];
                                string SENDER_ID = ConfigurationManager.AppSettings["SenderID"];
                                string GOOGLE_API_URI = ConfigurationManager.AppSettings["GoogleAPI"];

                                if (!string.IsNullOrEmpty(SERVER_API_KEY) && !string.IsNullOrEmpty(SENDER_ID) && !string.IsNullOrEmpty(GOOGLE_API_URI))
                                {
                                    Parallel.ForEach(listNoti, new ParallelOptions { MaxDegreeOfParallelism = 10 }, item =>
                                    {
                                        WebRequest tRequest = WebRequest.Create(GOOGLE_API_URI);
                                        tRequest.Method = "post";
                                        tRequest.ContentType = "application/json";
                                        tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));
                                        tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

                                        var json = GetJsonPushNotification(item);
                                        Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                                        tRequest.ContentLength = byteArray.Length;

                                        Stream dataStream = tRequest.GetRequestStream();
                                        dataStream.Write(byteArray, 0, byteArray.Length);
                                        dataStream.Close();

                                        WebResponse tResponse = tRequest.GetResponse();
                                        dataStream = tResponse.GetResponseStream();
                                        StreamReader tReader = new StreamReader(dataStream);

                                        var log = JsonConvert.DeserializeObject<AndroidNotificationResponse>(tReader.ReadToEnd());
                                        NSLog.Logger.Info("PushNotificationsAndroid", log);
                                        NSLog.Logger.Info("PushNotificationsAndroid" + item.DeviceToken, item);

                                        if (log.success > 0) LogPushNotify(item.DeviceToken, false, Constants.EDeviceType.Android);
                                        else if (log.failure > 0) LogPushNotify(item.DeviceToken, true, Constants.EDeviceType.Android);

                                        tReader.Close();
                                        dataStream.Close();
                                        tResponse.Close();
                                    });

                                    //new Thread(() =>
                                    //{
                                    //    var config = new GcmConfiguration(SENDER_ID, SERVER_API_KEY, null);
                                    //    config.GcmUrl = GOOGLE_API_URI;

                                    //    var gcmBroker = new GcmServiceBroker(config);
                                    //    gcmBroker.OnNotificationFailed += GcmBroker_OnNotificationFailed;
                                    //    gcmBroker.OnNotificationSucceeded += GcmBroker_OnNotificationSucceeded;

                                    //    gcmBroker.Start();

                                    //    foreach (var item in listNoti)
                                    //    {
                                    //        gcmBroker.QueueNotification(new GcmNotification
                                    //        {
                                    //            To = item.DeviceToken,
                                    //            //RegistrationIds = new List<string>() { },
                                    //            TimeToLive = 3,
                                    //            ContentAvailable = true,
                                    //            Priority = GcmNotificationPriority.High,
                                    //            DryRun = true,
                                    //            Notification = JObject.Parse(JsonConvert.SerializeObject(new FCMNotification()
                                    //            {
                                    //                body = item.Message,
                                    //                title = item.Title,
                                    //            })),
                                    //            Data = JObject.Parse(JsonConvert.SerializeObject(new FCMData()
                                    //            {
                                    //                body = item.Message,
                                    //                title = item.Title,
                                    //                artist_id = item.ArtistID,
                                    //                member_id = item.MemberID,
                                    //                content_code = item.MessageType,
                                    //                badge = item.Badge,
                                    //            }))
                                    //        });
                                    //    }

                                    //    gcmBroker.Stop();
                                    //}).Start();
                                }
                                break;

                            case (int)Constants.EDeviceType.WindowPhone:
                                break;
                        }
                    }
                }
                else
                    NSLog.Logger.Info("PushNotifications:", "No record");
            }
            catch (Exception ex) { NSLog.Logger.Error("Push Notifications Error: ", ex); }
        }

        private static void GcmBroker_OnNotificationSucceeded(GcmNotification notification)
        {
            NSLog.Logger.Info("PushNotificationsAndroidSucceeded. - DeviceToken: " + notification.To, notification);
            LogPushNotify(notification.To, false, Constants.EDeviceType.Android);
        }

        private static void GcmBroker_OnNotificationFailed(GcmNotification notification, AggregateException exception)
        {
            NSLog.Logger.Info("PushNotificationsAndroidFailed. - DeviceToken: " + notification.To, notification);
            NSLog.Logger.Error("PushNotificationsAndroidFailed. - DeviceToken: " + notification.To, exception);
            LogPushNotify(notification.To, true, Constants.EDeviceType.Android);
        }

        private static void ApnsBroker_OnNotificationFailed(ApnsNotification notification, AggregateException exception)
        {
            NSLog.Logger.Info("PushNotificationsiOSFailed. - DeviceToken: " + notification.DeviceToken, notification);
            NSLog.Logger.Error("PushNotificationsiOSFailed. - DeviceToken: " + notification.DeviceToken, exception);
            LogPushNotify(notification.DeviceToken, true, Constants.EDeviceType.iOS);
        }

        private static void ApnsBroker_OnNotificationSucceeded(ApnsNotification notification)
        {
            NSLog.Logger.Info("PushNotificationsiOSSucceeded. - DeviceToken: " + notification.DeviceToken, notification);
            LogPushNotify(notification.DeviceToken, false, Constants.EDeviceType.iOS);
        }

        private static string GetJsonPushNotification(NotiInfo notiInfo)
        {
            switch (notiInfo.DeviceType)
            {
                case (int)Constants.EDeviceType.iOS:
                    AppleNotificationObj appleObj = new AppleNotificationObj();
                    appleObj.aps.alert.title = notiInfo.Title;
                    appleObj.aps.alert.body = notiInfo.Message;
                    appleObj.aps.badge = notiInfo.Badge;
                    appleObj.member_id = notiInfo.MemberID;
                    appleObj.artist_id = notiInfo.ArtistID;
                    appleObj.content_code = notiInfo.MessageType;
                    appleObj.customer_no = notiInfo.CustomerNo;
                    //return JsonConvert.SerializeObject(appleObj);
                    return appleObj.ToString();

                case (int)Constants.EDeviceType.Android:
                    AndroidNotificationObject androidObj = new AndroidNotificationObject();
                    androidObj.registration_ids = new List<string>() { notiInfo.DeviceToken };
                    androidObj.data.title = notiInfo.Title;
                    androidObj.data.body = notiInfo.Message;
                    androidObj.data.member_id = notiInfo.MemberID;
                    androidObj.data.artist_id = notiInfo.ArtistID;
                    androidObj.data.badge = notiInfo.Badge;
                    androidObj.data.content_code = notiInfo.MessageType;
                    androidObj.data.customer_no = notiInfo.CustomerNo;
                    return JsonConvert.SerializeObject(androidObj);
            }
            return "";
        }

        private static void LogPushNotify(string deviceToken, bool isFail, Constants.EDeviceType deviceType)
        {
            semaphore.WaitOne();
            try
            {
                using (var _db = new PiOneDb())
                {
                    var device = _db.Devices.Where(o => o.DeviceToken == deviceToken && o.DeviceType == (byte)deviceType).FirstOrDefault();
                    if (device != null)
                    {
                        if (isFail) device.Type++;
                        else device.Type = 0;
                        if (device.Type >= 50) _db.Devices.Remove(device);
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorLogPushNotifyFail", ex); }
            finally { semaphore.Release(); }
        }

        private static Semaphore semaphore = new Semaphore(1, 1);
    }
}