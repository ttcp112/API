using Newtonsoft.Json;
using PiOne.Api.Business.DTO;
using PiOne.Api.Business.SupportObject;
using PiOne.Api.Common;
using PiOne.Api.Core.Response;
using PiOne.Api.DataModel.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.Consumer.Business
{
    public class NSBusPromotion : NSBusBase, IBusiness
    {
        public NSApiResponse GetPromotion(string storeID)
        {
            var response = new NSApiResponse();

            try
            {
                GetPromotionResponse result = new GetPromotionResponse();

                using (var client = new HttpClient())
                {
                    var request = new
                    {
                        Mode = (byte)Constants.EStatus.Actived,
                        StoreId = storeID,
                        IsCheckValid = true,
                    };

                    NSLog.Logger.Info("RequestIOne_GetListDetailPromotion", request);
                    string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(NSIOnceUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Promotion_GetListDetailPromotion, request).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        NSLog.Logger.Info("ResponseIOne_GetListDetailPromotion", dynamicObj);
                        if ((bool)dynamicObj["Success"] == true)
                        {
                            string json = JsonConvert.SerializeObject(dynamicObj["Data"]["ListPromotion"]);
                            result.ListPromotion = JsonConvert.DeserializeObject<List<PromotionDTO>>(json);
                            response.Success = true;
                            response.Data = result;
                        }
                        else
                            response.Message = dynamicObj["Message"];
                    }
                    else
                        response.Message = "Unable to get promotion at this time.";
                }
                NSLog.Logger.Info("ResponseGetPromotion", response);
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse GetPromotionDetail(string storeID, string _promotionId)
        {
            var response = new NSApiResponse();

            try
            {
                GetDetailPromotionResponse result = new GetDetailPromotionResponse();

                using (var client = new HttpClient())
                {
                    var request = new
                    {
                        Mode = (byte)Constants.EStatus.Actived,
                        StoreId = storeID,
                        Id = _promotionId,
                    };

                    NSLog.Logger.Info("RequestIOne_GetPromotionDetail", request);
                    string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(NSIOnceUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Promotion_GetDetailPromotion, request).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        NSLog.Logger.Info("ResponseIOne_GetPromotionDetail", dynamicObj);
                        if ((bool)dynamicObj["Success"] == true)
                        {                        
                            var content = JsonConvert.SerializeObject(dynamicObj["Data"]);
                            result = JsonConvert.DeserializeObject<GetDetailPromotionResponse>(content);
                            response.Success = true;
                            response.Data = result;
                        }
                        else
                            response.Message = dynamicObj["Message"];
                    }
                    else
                        response.Message = "Unable to get promotion at this time.";
                }
                NSLog.Logger.Info("ResponseGetPromotionDetail", response);
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }
    }
}
