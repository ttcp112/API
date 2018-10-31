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
using System.Threading.Tasks;

namespace PiOne.Api.Business.Consumer.Business
{
    public class NSBusOrder : NSBusBase, IBusiness
    {
        public NSApiResponse OrderCreate(string storeID, string tableID, string memberID, string languageID)
        {
            var response = new NSApiResponse();

            try
            {
                ResponseOrderGetDetail result = new ResponseOrderGetDetail();

                using (var client = new HttpClient())
                {
                    var request = new
                    {
                        CreatedUser = "PIOne",
                        CurrentBill = 1,
                        SplitNo = 1,
                        CusID = memberID,
                        //IsSelfOrdering = false, /* wallet order is not self ordering */
                       // IsWithPoins = true,
                        Cover = 1,
                        Mode = (byte)Constants.EStatus.Actived,
                        StoreId = storeID,
                        TableID = tableID,
                        LanguageID = languageID,
                        OrderState = (byte)Constants.EOrderState.AddToTab,
                        IsDontCheckTable = true,
                    };

                    NSLog.Logger.Info("RequestIOne_CreateOrder", request);
                    string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(NSIOnceUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Order_CreateOrEdit, request).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        NSLog.Logger.Info("ResponseIOne_CreateOrder", dynamicObj);
                        if ((bool)dynamicObj["Success"] == true)
                        {
                            string json = JsonConvert.SerializeObject(dynamicObj["Data"]["OrderDetail"]);
                            result.OrderDetail = JsonConvert.DeserializeObject<OrderDetailDTO>(json);
                            response.Success = true;
                            response.Data = result;
                        }
                        else
                            response.Message = dynamicObj["Message"];
                    }
                    else
                        response.Message = "Unable to make order at this time.";
                }
                NSLog.Logger.Info("ResponseOrderCreate", response);
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse OrderCheckDrawer(string storeID, string languageID)
        {
            var response = new NSApiResponse();

            try
            {
                ResponseOrderGetDetail result = new ResponseOrderGetDetail();

                using (var client = new HttpClient())
                {
                    var request = new
                    {
                        Mode = (byte)Constants.EStatus.Actived,
                        StoreId = storeID,
                        LanguageID = languageID,
                        IsPoins = true //for check diff businessday
                    };

                    /* request IOne */
                    NSLog.Logger.Info("RequestIOne_CheckDrawer", request);
                    string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(NSIOnceUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.BusinessDay_Shift_Drawer_CheckState, request).Result;

                    /* check response */
                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        NSLog.Logger.Info("ResponseIOne_CheckDrawer", dynamicObj);
                        if ((bool)dynamicObj["Success"] == true)
                        {
                            response.Success = true;
                        }
                        else
                        {
                            response.Message = "We are still not ready to serve you now. Please ask our staff for help.";
                        }
                    }
                    else
                        response.Message = "Unable to connect to server.";
                }
                NSLog.Logger.Info("ResponseOrderCheckDrawer", response);
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse OrderDelete(string id)
        {
            var response = new NSApiResponse();

            try
            {
                NSApiResponseBase result = new ResponseOrderGetDetail();

                /* delete order in nupos */
                using (var client = new HttpClient())
                {
                    var request = new
                    {
                        //StoreId = storeID,
                        Id = id,
                        CreatedUser = "PIOne",
                        Reason = "",
                        DeviceName = "",
                        Mode = (byte)Constants.EStatus.Actived,
                       // IsWithPoins = true,
                    };

                    NSLog.Logger.Info("RequestIOne_OrderDelete", request);
                    string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(NSIOnceUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Order_Delete, request).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        NSLog.Logger.Info("ResponseIOne_OrderDelete", dynamicObj);
                        if ((bool)dynamicObj["Success"] == true)
                        {
                            response.Success = true;
                        }
                        else
                            response.Message = dynamicObj["Message"];
                    }
                    else
                        response.Message = "Unable to delete order at this time.";
                }

                NSLog.Logger.Info("ResponseOrderDelete", response);
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse OrderAddItem(AddItemToOrderDTO input, string storeID, string memberID)
        {
            var response = new NSApiResponse();

            try
            {
                ResponseAddItemToOrder result = new ResponseAddItemToOrder();

                /* call nupos to update order */
                using (var client = new HttpClient())
                {
                    input.OrderID = input.ID;
                    input.CreatedUser = "PIOne";
                    input.CurrentBill = 1;
                    input.SplitNo = 1;
                    input.CusID = memberID;
                    //  input.IsSelfOrdering = false; /* wallet order is not self ordering */
                 //   input.IsWithPoins = true;
                    input.IsMobile = true;
                    input.Mode = (byte)Constants.EStatus.Actived;
                    input.Cover = 1;
                    if (string.IsNullOrEmpty(input.StoreId))
                        input.StoreId = storeID;
                    /* request IOne */
                    NSLog.Logger.Info("RequestIOne_AddItemToOrderFromKiosk", input);
                    string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(NSIOnceUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Order_Add_Item_FromKiosk, input).Result;

                    /* check response */
                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        NSLog.Logger.Info("ResponseIOne_AddItemToOrderFromKiosk", dynamicObj);
                        if ((bool)dynamicObj["Success"] == true)
                        {
                            string json = JsonConvert.SerializeObject(dynamicObj["Data"]["OrderDetail"]);
                            result.OrderDetail = JsonConvert.DeserializeObject<OrderDetailDTO>(json);

                            /* response data */
                            response.Success = true;
                            response.Data = result;
                        }
                        else
                            response.Message = dynamicObj["Message"];
                    }
                    else
                        response.Message = "Unable to make order at this time.";
                }
                NSLog.Logger.Info("ResponseOrderAddItemToOrder", response);
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse OrderGetDetail(string storeID, string memberID, string id)
        {
            var response = new NSApiResponse();

            try
            {
                ResponseOrderGetDetail result = new ResponseOrderGetDetail();
                using (var client = new HttpClient())
                {
                    var request = new
                    {
                        Mode = (byte)Constants.EStatus.Actived,
                        Id = id,
                        MemberID = memberID,
                        StoreId = storeID,
                    };

                    /* request IOne */
                    NSLog.Logger.Info("RequestIOne_OrderGetDeail", request);
                    string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(NSIOnceUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Order_Get_Detail, request).Result;

                    /* check response */
                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        NSLog.Logger.Info("ResponseIOne_OrderGetDeail", dynamicObj);
                        if ((bool)dynamicObj["Success"] == true)
                        {
                            string json = JsonConvert.SerializeObject(dynamicObj["Data"]["OrderDetail"]);
                            result.OrderDetail = JsonConvert.DeserializeObject<OrderDetailDTO>(json);

                            /* response data */
                            response.Data = result;
                            response.Success = true;
                        }
                        else
                            response.Message = (string)dynamicObj["Message"];

                        if (string.IsNullOrEmpty(id)) /* response true if check order of memberID */
                            response.Success = true;
                    }
                    else
                        response.Message = "Unable to get order detail.";
                }

                NSLog.Logger.Info("ResponseOrderGetDetail", response);
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse OrderChangeStateTable(string storeID, string id, int state)
        {
            var response = new NSApiResponse();

            try
            {
                using (var client = new HttpClient())
                {
                    var request = new
                    {
                        Id = id,
                        Mode = (byte)Constants.EStatus.Actived,
                        TableID = "",
                        State = state,
                        DeviceName = "",
                    };

                    /* request Ione */
                    NSLog.Logger.Info("RequestIOne_OrderChangeStateTable", request);
                    string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(NSIOnceUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Order_Table_ChangeState, request).Result;

                    /* check response */
                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        NSLog.Logger.Info("ResponseIOne_OrderChangeStateTable", dynamicObj);
                        if ((bool)dynamicObj["Success"] == true)
                            response.Success = true;
                        else
                            response.Message = (string)dynamicObj["Message"];
                    }
                    else
                        response.Message = "Unable to change state.";
                }

                NSLog.Logger.Info("ResponseOrderChangeStateTable", response);
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse OrderPayByExternal(string storeID, string memberID, string id, string paymentMethodID, double amount)
        {
            var response = new NSApiResponse();

            try
            {
                var responseCheckDrawer = OrderCheckDrawer(storeID, "");
                if (responseCheckDrawer.Success == true)
                {
                    if (amount > 0)
                    {
                        /* pay by external - update order paid in nupos */
                        using (var client = new HttpClient())
                        {
                            var request = new
                            {
                                Id = id,
                                CreatedUser = "PIOne",
                                Mode = (byte)Constants.EStatus.Actived,
                                AmountPay = amount,
                                PaymentMethodID = paymentMethodID,
                                StoreId = storeID,
                                OrderID = id,
                               // IsWithPoins = true,
                                IsWalletPayment = true,
                                CustomerID = memberID,
                            };

                            /* request IOne */
                            NSLog.Logger.Info("RequestIOne_OrderPayByExternal", request);
                            string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                            client.BaseAddress = new Uri(NSIOnceUrl);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Pay_By_Payment_Method, request).Result;

                            /* check response */
                            if (responseFromNSApi.IsSuccessStatusCode)
                            {
                                dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                                NSLog.Logger.Info("ResponseIOne_OrderPayByExternal", dynamicObj);
                                if ((bool)dynamicObj["Success"] == true)
                                {
                                    /* done order */
                                    response = OrderDone(storeID, id);
                                    CommonFunction.RefreshMembershipInfo(memberID, "ExternalPayment");
                                }
                                else
                                    response.Message = dynamicObj["Message"];
                            }
                        }
                    }
                    else
                        response.Message = "Unable to make payment at this time.";
                }
                else
                    response.Message = responseCheckDrawer.Message;

                NSLog.Logger.Info("ResponseOrderPayByExternal", response);
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse OrderDone(string storeID, string id)
        {
            var response = new NSApiResponse();

            try
            {
                using (var client = new HttpClient())
                {
                    var request = new
                    {
                        ReceiptID = id,
                        Mode = (byte)Constants.EStatus.Actived,
                        IsReceipt = true,
                        CurrentBill = 1,
                        IsWalletDonePayment = true, /* don't close order */
                        CreatedUser = "PiOne",
                        StoreId = storeID,
                       // IsWithPoins = true,
                    };

                    /* request IOne */
                    NSLog.Logger.Info("RequestIOne_OrderDone", request);
                    string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(NSIOnceUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Order_PrintReceipt, request).Result;

                    /* check response */
                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        NSLog.Logger.Info("ResponseIOne_OrderDone", dynamicObj);
                        if ((bool)dynamicObj["Success"] == true)
                            response.Success = true;
                        else
                            response.Message = (string)dynamicObj["Message"];
                    }
                    else
                        response.Message = "Unable to done order.";
                }

                NSLog.Logger.Info("ResponseOrderDone", response);
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse OrderSync(OrderDetailDTO input)
        {
            var response = new NSApiResponse();

            try
            {
                using (var _db = new PiOneDb())
                {
                    var order = new Order()
                    {
                        ID = input.ID,
                        StoreID = input.StoreID,
                        MemberID = input.CustomerID,
                        ParentID = input.ParentID,
                        TableName = "",
                        PosOrderID = "",
                        ReceiptNo = input.ReceiptNO,
                        OrderNo = input.OrderNO,
                        TagNo = input.TagNumber,
                        SplitNo = 0,
                        Cover = (byte)input.Cover,
                        TotalBill = input.Total,
                        TotalDiscount = input.Discount,
                        SubTotal = input.SubTotal,
                        Tip = input.Tip,
                        Tax = input.Tax,
                        Remaining = 0,
                        ServiceCharged = input.ServiceCharge,
                        RoundingAmount = input.RoundingAmount,
                        Remark = input.Remark,
                        TotalPromotion = input.Promotion,
                        ReceiptCreatedDate = input.ReceiptCreatedDate,
                        IsInput = false,
                        TransactionType = 0,
                        Status = (byte)Constants.EStatus.Actived,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = input.CashierID,
                        ModifiedBy = input.CashierID,
                        ModifiedDate = DateTime.UtcNow,
                    };

                    /* save data */
                    _db.Orders.Add(order);
                    if (_db.SaveChanges() > 0)
                        response.Success = true;
                    else
                        response.Message = "Unable to sync order.";
                    NSLog.Logger.Info("ResponseSyncOrder", response);
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse OrderDoneTemp(string id, string storeID, string paymentMethodID, double amount)
        {
            var response = new NSApiResponse();

            try
            {
                ResponseOrderGetDetail result = new ResponseOrderGetDetail();

                using (var client = new HttpClient())
                {
                    var request = new
                    {
                        Mode = (byte)Constants.EStatus.Actived,
                        StoreId = storeID,
                        OrderID = id,
                        PaymentMethodID = paymentMethodID,
                        Amount = amount
                    };

                    /* request IOne */
                    NSLog.Logger.Info("RequestIOne_DoneTemp", request);
                    string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(NSIOnceUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Order_Temp_Done, request).Result;

                    /* check response */
                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        NSLog.Logger.Info("ResponseIOne_DoneTemp", dynamicObj);
                        if ((bool)dynamicObj["Success"] == true)
                        {
                            response.Success = true;
                        }
                        else
                            response.Message = dynamicObj["Message"];
                    }
                    else
                        response.Message = "Unable to connect to server.";
                }
                NSLog.Logger.Info("ResponseOrderDoneTemp", response);
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

    }
}
