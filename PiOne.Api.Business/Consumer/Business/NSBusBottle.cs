using Newtonsoft.Json;
using PiOne.Api.Business.DTO;
using PiOne.Api.Business.Support;
using PiOne.Api.Business.SupportObject;
using PiOne.Api.Common;
using PiOne.Api.Core.Response;
using PiOne.Api.DataModel.Context;
using PiOne.Api.DataModel.PiOneEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiOne.Api.Business.Consumer.Business
{
    public class NSBusBottle : NSBusBase, IBusiness
    {
        public NSApiResponse GetListBottle(string memberID)
        {
            var response = new NSApiResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    string mess = "";
                    GetListBottleResponse result = new GetListBottleResponse()
                    {
                        ListBottle = GetListBottleIntegrate(memberID, ref mess)
                    };

                    if (string.IsNullOrEmpty(mess))
                    {
                        response.Success = true;
                        response.Data = result;
                        new Thread(() => ViewBottleNoti(memberID)).Start();
                    }
                    else
                        response.Message = mess;

                    NSLog.Logger.Info("ResponseGetListBottle", response);
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorGetListBottle", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public static List<BottleIntegrateDTO> GetListBottleIntegrate(string memberID, ref string mess)
        {
            List<BottleIntegrateDTO> listData = new List<BottleIntegrateDTO>();
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new { MemberID = memberID };

                    NSLog.Logger.Info("RequestIOne_GetListBottle", request);
                    string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(NSIOnceUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Bottle_Get_Member, request).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        NSLog.Logger.Info("ResponseIOne_GetListBottle", dynamicObj);
                        if ((bool)dynamicObj["Success"] == true)
                        {
                            listData = JsonConvert.DeserializeObject<List<BottleIntegrateDTO>>(JsonConvert.SerializeObject(dynamicObj["Data"]["ListBottle"]));
                            listData = listData.OrderBy(o => o.ExpiryDate).ToList();
                        }
                        else
                            mess = dynamicObj["Message"];
                    }
                    NSLog.Logger.Info("ResponseGetListBottle", listData);
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorGetListBottleIntegrate", null, listData, ex); }
            return listData;
        }

        public NSApiResponse BottlePushNoti(List<BottleIntegrateDTO> listBottle, int typePush)
        {
            NSApiResponse response = new NSApiResponse();
            try
            {
                if (listBottle.Count > 0)
                {
                    BottlePushNotiFunc(listBottle, typePush);
                    response.Success = true;
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorBottlePushNoti", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public static void CheckRemidOrExpireBottle(int min)
        {
            try
            {
                using (var _db = new PiOneDb())
                {
                    byte delete = (byte)Constants.EStatus.Deleted;

                    var listMemberID = _db.Members.Join(_db.NotificationSettings, m => m.ID, n => n.MemberID, (m, n) => new { m, n })
                        .Join(_db.Devices, mn => mn.m.ID, d => d.MemberID, (mn, d) => new { mn.m, mn.n, d })
                        .Where(o => o.n.IsReceiveNotification && o.d.Status != delete).Select(o => o.m.ID).Distinct().ToList();
                    var listBottle = GetDataListBottleRemindOrExpire(listMemberID, min);

                    if (listBottle.Count > 0)
                    {
                        var listBottleRemind = listBottle.Where(o => o.IsRemind).ToList();
                        if (listBottleRemind.Count > 0)
                            BottlePushNotiFunc(listBottleRemind, (int)Constants.EBottlePushNoti.Remind);

                        var listBottleExpire = listBottle.Where(o => o.IsExpire).ToList();
                        if (listBottleExpire.Count > 0)
                            BottlePushNotiFunc(listBottleRemind, (int)Constants.EBottlePushNoti.Expire);
                    }
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorCheckRemidOrExpireBottle", ex); }
        }

        private static List<BottleIntegrateDTO> GetDataListBottleRemindOrExpire(List<string> listMemberID, int min)
        {
            List<BottleIntegrateDTO> listData = new List<BottleIntegrateDTO>();
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new { ListMemberID = listMemberID, Minutes = min };

                    NSLog.Logger.Info("RequestIOne_GetBottleRemindOrExpire", request);
                    string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(NSIOnceUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Bottle_Get_RemindOrExpire, request).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        NSLog.Logger.Info("ResponseIOne_GetBottleRemindOrExpire", dynamicObj);
                        if ((bool)dynamicObj["Success"] == true)
                        {
                            listData = JsonConvert.DeserializeObject<List<BottleIntegrateDTO>>(JsonConvert.SerializeObject(dynamicObj["Data"]["ListBottle"]));
                            listData = listData.OrderBy(o => o.ExpiryDate).ToList();
                        }
                    }
                    NSLog.Logger.Info("ResponseCheckRemidOrExpireBottle", listData);
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorGetDataListBottleRemindOrExpire", null, listData, ex); }
            return listData;
        }

        private static void ViewBottleNoti(string memberID)
        {
            try
            {
                using (var _db = new PiOneDb())
                {
                    byte delete = (byte)Constants.EStatus.Deleted;
                    var listBottleNoti = _db.MemberNotifications.Where(o => o.Type == (int)Constants.EMessageType.Bottle && o.MemberID == memberID && !o.IsMemberView && o.Status != delete).ToList();
                    if (listBottleNoti.Count > 0)
                    {
                        listBottleNoti.ForEach(o => o.IsMemberView = true);
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorViewBottleNoti", ex); }
        }

        private static void BottlePushNotiFunc(List<BottleIntegrateDTO> listBottle, int typePush)
        {
            try
            {
                if (Enum.IsDefined(typeof(Constants.EBottlePushNoti), typePush))
                {
                    Parallel.ForEach(listBottle, new ParallelOptions { MaxDegreeOfParallelism = 10 }, bottle =>
                    {
                        if (!string.IsNullOrEmpty(bottle.MemberID))
                        {
                            string title = "IONE2 DRINK";
                            string message = "";
                            Dictionary<int, string> dict = new Dictionary<int, string>();
                            int count = 0;
                            switch ((Constants.EBottlePushNoti)typePush)
                            {
                                case Constants.EBottlePushNoti.CheckIn:
                                    message = "Your {0} just checked in at {1}. Bottle will be expired at {2}.";
                                    dict.Add(count++, bottle.Name);
                                    dict.Add(count++, bottle.StoreName);
                                    dict.Add(count++, bottle.ExpiryDate.ToString("dd/MM/yyyy"));
                                    break;
                                case Constants.EBottlePushNoti.CheckOut:
                                    message = "Your {0} just checked out from {1}.";
                                    dict.Add(count++, bottle.Name);
                                    dict.Add(count++, bottle.StoreName);
                                    break;
                                case Constants.EBottlePushNoti.Remind:
                                    message = "Your {0} will be expired in {1} days.";
                                    dict.Add(count++, bottle.Name);
                                    dict.Add(count++, bottle.RemindDays.ToString());
                                    break;
                                case Constants.EBottlePushNoti.Expire:
                                    message = "Your {0} just expired.";
                                    dict.Add(count++, bottle.Name);
                                    break;
                                case Constants.EBottlePushNoti.Dispose:
                                    message = "Your {0} just disposed from {1}.";
                                    dict.Add(count++, bottle.Name);
                                    dict.Add(count++, bottle.StoreName);
                                    break;
                            }

                            NotificationFuction.PushNoti_Bottle(bottle.MemberID, title, message, dict);
                        }
                    });
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorBottlePushNotiFunc", ex); }
        }
    }
}
