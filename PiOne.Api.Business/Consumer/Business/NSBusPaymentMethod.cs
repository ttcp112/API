using Newtonsoft.Json;
using PiOne.Api.Business.DTO;
using PiOne.Api.Business.SupportObject;
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
    public class NSBusPaymentMethod : NSBusBase, IBusiness
    {
        public NSApiResponse PaymentMethodGetExternal(string storeID)
        {
            var response = new NSApiResponse();

            try
            {
                ResponsePaymentMethodGetExternal result = new ResponsePaymentMethodGetExternal();

                using (var client = new HttpClient())
                {
                    var request = new
                    {
                        StoreId = storeID,
                    };

                    NSLog.Logger.Info("RequestIOne_GetPaymentMethodExternal", request);
                    string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(NSIOnceUrl); client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.PaymentMethod_GetExternal, request).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        NSLog.Logger.Info("ResponseIOne_GetPaymentMethodExternal", dynamicObj);
                        if ((bool)dynamicObj["Success"] == true)
                        {
                            result.ListPaymentMethod = JsonConvert.DeserializeObject<List<PaymentMethodDTO>>(JsonConvert.SerializeObject(dynamicObj["Data"]["ListPaymentMethod"]));

                            response.Data = result;
                            response.Success = true;
                        }
                        else
                            response.Message = dynamicObj["Message"];
                    }
                }
                NSLog.Logger.Info("ResponsePaymentMethodGetExternal", response);
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }
    }
}
