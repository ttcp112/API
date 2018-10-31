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
    public class NSBusBooking : NSBusBase, IBusiness
    {
        public NSApiResponse CreateOrUpdateBooking(RequestCreateBooking input)
        {
            var response = new NSApiResponse();

            try
            {
                using (var client = new HttpClient())
                {
                    var result = new GetReservationResponse();
                    input.reservation.MemberID = input.MemberID;
                    NSLog.Logger.Info("CallBooking-NIOne", input);
                    string webURL = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(webURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Booking_CreateOrUpdate, input).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        response.Success = (bool)dynamicObj["Success"];
                        if (response.Success == true)
                        {
                            var content = JsonConvert.SerializeObject(dynamicObj["Data"]);
                            result = JsonConvert.DeserializeObject<GetReservationResponse>(content);
                            response.Data = result;
                        }
                        response.Message = dynamicObj["Message"];
                    }
                    NSLog.Logger.Info("ResponseCallBooking-NIOne", responseFromNSApi);
                }

                NSLog.Logger.Info("ResponseCreateOrUpdateBooking", response);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("ErrorCreateOrUpdateBooking", null, response, ex);
            }
            return response;
        }

        public NSApiResponse GetDetailBooking(RequestGetDetailBooking input)
        {
            var response = new NSApiResponse();

            try
            {
                using (var client = new HttpClient())
                {
                    var result = new GetReservationResponse();
                    NSLog.Logger.Info("CallGetBooking-NIOne", input);
                    string webURL = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(webURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Booking_Get_Detail, input).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        response.Success = (bool)dynamicObj["Success"];
                        if (response.Success == true)
                        {
                            var content = JsonConvert.SerializeObject(dynamicObj["Data"]);
                            result = JsonConvert.DeserializeObject<GetReservationResponse>(content);
                            response.Data = result;
                        }
                        response.Message = dynamicObj["Message"];
                    }
                    NSLog.Logger.Info("ResponseCallGetBooking-NIOne", responseFromNSApi);
                }

                NSLog.Logger.Info("ResponseGetDetailBooking", response);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("ErrorGetDetailBooking", null, response, ex);
            }
            return response;
        }

        public NSApiResponse GetBookingDateTimeSlot(RequestGetBookingDateTimeSlot input)
        {
            var response = new NSApiResponse();

            try
            {
                using (var client = new HttpClient())
                {
                    var result = new GetDateTimeSlotResponse();
                    NSLog.Logger.Info("CallGetBookingDateTimeSlot-NIOne", input);
                    string webURL = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(webURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Booking_Get_DateTime_Slot, input).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        response.Success = (bool)dynamicObj["Success"];
                        if (response.Success == true)
                        {
                            var content = JsonConvert.SerializeObject(dynamicObj["Data"]);
                            result = JsonConvert.DeserializeObject<GetDateTimeSlotResponse>(content);
                            response.Data = result;
                        }
                        response.Message = dynamicObj["Message"];
                    }
                    NSLog.Logger.Info("ResponseCallBookingDateTimeSlot-NIOne", responseFromNSApi);
                }

                NSLog.Logger.Info("ResponseGetBookingDateTimeSlot", response);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("ErrorGetBookingDateTimeSlot", null, response, ex);
            }
            return response;
        }

        public NSApiResponse UpdateStatusBooking(RequestUpdateStatusBooking input)
        {
            var response = new NSApiResponse();

            try
            {
                using (var client = new HttpClient())
                {
                    NSLog.Logger.Info("CallUpdateStatusBooking-NIOne", input);
                    string webURL = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(webURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Booking_Update_Status, input).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        response.Success = (bool)dynamicObj["Success"];
                        if (response.Success == true)
                        {
                            var content = JsonConvert.SerializeObject(dynamicObj["Data"]);
                        }
                        response.Message = dynamicObj["Message"];
                    }
                    NSLog.Logger.Info("ResponseCallUpdateStatusBooking-NIOne", responseFromNSApi);
                }

                NSLog.Logger.Info("ResponseUpdateStatusBooking", response);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("ErrorGetDetailBooking", null, response, ex);
            }
            return response;
        }

        public NSApiResponse GetListBooking(RequestGetListBooking input)
        {
            var response = new NSApiResponse();

            try
            {
                using (var client = new HttpClient())
                {
                    var result = new GetListReservationResponse();
                    NSLog.Logger.Info("CallGetListtBooking-NIOne", input);
                    input.ID = input.MemberID;
                    string webURL = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(webURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Booking_Get_List, input).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        response.Success = (bool)dynamicObj["Success"];
                        if (response.Success == true)
                        {
                            var content = JsonConvert.SerializeObject(dynamicObj["Data"]);
                            result = JsonConvert.DeserializeObject<GetListReservationResponse>(content);
                            response.Data = result;
                        }
                        response.Message = dynamicObj["Message"];
                    }
                    NSLog.Logger.Info("ResponseCallGetListBooking-NIOne", responseFromNSApi);
                }

                NSLog.Logger.Info("ResponseGetListBooking", response);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("ErrorGetListBooking", null, response, ex);
            }
            return response;
        }

        public NSApiResponse GetListBookingRemind()
        {
            var response = new NSApiResponse();

            try
            {
                using (var client = new HttpClient())
                {
                    var result = new GetListReservationResponse();
                    NSLog.Logger.Info("CallGetListtBookingRemind-NIOne");
                    string webURL = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(webURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Booking_Get_List_Remind, "").Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        response.Success = (bool)dynamicObj["Success"];
                        if (response.Success == true)
                        {
                            var content = JsonConvert.SerializeObject(dynamicObj["Data"]);
                            result = JsonConvert.DeserializeObject<GetListReservationResponse>(content);
                            foreach (var item in result.ListReser)
                            {
                                int countDict = 0;
                                Dictionary<int, string> dictionary = new Dictionary<int, string>()
                                    {
                                        {countDict++, item.ReservationNo}, //time distance
                                        {countDict++, item.StoreID }, //store name
                                        {countDict++, item.Cover.ToString()}
                                    };
                                int no = 0;
                                Int32.TryParse(item.Mobile, out no); //num of upcoming booking
                                NotificationFuction.PushNoti_Booking(item.MemberID, "", "{0} minute(s) left for reservation at {1} with {2} person(s).", no, item.ID, dictionary);
                            }
                        }
                        response.Message = dynamicObj["Message"];
                    }
                    NSLog.Logger.Info("ResponseCallGetListBookingRemind-NIOne", responseFromNSApi);
                }

                NSLog.Logger.Info("ResponseGetListBookingRemind", response);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("ErrorGetListBookingRemind", null, response, ex);
            }
            return response;
        }


    }
}
