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
    public class NSBusProduct : NSBusBase, IBusiness
    {
        public NSApiResponse ProductGetListForOrder(string merchantID, string storeID)
        {
            var response = new NSApiResponse();

            try
            {
                using (var _db = new PiOneDb())
                {
                    ResponseGetListProductForOrder result = new ResponseGetListProductForOrder();

                    using (var client = new HttpClient())
                    {
                        var isCheck = true;
                        /* merchant info of select store */
                        var merchantInfo = _db.Stores.Where(o => o.ID == storeID).Select(o => new
                        {
                            StoreID = o.ID,
                            StoreName = o.Name,
                            MerchantID = o.MerchantID,
                            MerchantName = o.Merchant.Name,
                        }).FirstOrDefault();

                        if (merchantInfo != null)
                        {
                            if (!string.IsNullOrEmpty(merchantID)) /* need to check merchant ID vs storeID are matched */
                            {
                                if (merchantID != merchantInfo.MerchantID)
                                {
                                    isCheck = false;
                                    result.StoreID = merchantInfo.StoreID;
                                    result.StoreName = merchantInfo.StoreName;
                                    result.MerchantID = merchantInfo.MerchantID;
                                    result.MerchantName = merchantInfo.MerchantName;
                                    response.Data = result;
                                }
                            }

                            if (isCheck)
                            {
                                var request = new
                                {
                                    StoreId = storeID,
                                };

                                NSLog.Logger.Info("RequestIOne_GetListProductOnKiosk", request);
                                string NSIOnceUrl = ConfigurationManager.AppSettings["PosApi"];
                                client.BaseAddress = new Uri(NSIOnceUrl);
                                client.DefaultRequestHeaders.Accept.Clear();
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Kiosk_Product_GetList, request).Result;

                                if (responseFromNSApi.IsSuccessStatusCode)
                                {
                                    dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                                    NSLog.Logger.Info("ResponseIOne_GetListProductOnKiosk", dynamicObj);
                                    if ((bool)dynamicObj["Success"] == true)
                                    {
                                        result.ListProduct = JsonConvert.DeserializeObject<List<ProductOnKioskDTO>>(JsonConvert.SerializeObject(dynamicObj["Data"]["ListProduct"]));
                                        result.ListSeason = JsonConvert.DeserializeObject<List<SeasonDTO>>(JsonConvert.SerializeObject(dynamicObj["Data"]["ListSeason"]));
                                        result.Tax = JsonConvert.DeserializeObject<TaxDTO>(JsonConvert.SerializeObject(dynamicObj["Data"]["Tax"]));
                                        result.ServiceCharge = JsonConvert.DeserializeObject<ServiceChargeDTO>(JsonConvert.SerializeObject(dynamicObj["Data"]["ServiceCharge"]));
                                        result.ListSetting = JsonConvert.DeserializeObject<List<SettingDTO>>(JsonConvert.SerializeObject(dynamicObj["Data"]["ListSetting"]));

                                        if (result.ListProduct == null) result.ListProduct = new List<ProductOnKioskDTO>();

                                        result.StoreID = storeID;
                                        result.MerchantID = merchantInfo.MerchantID;

                                        /* add list promotion for product */
                                        AddListPromotionForProduct(result.ListProduct, result.StoreID);

                                        /* list product recommend */
                                        result.ListRecommend = result.ListProduct.Where(o => o.IsRecommend).ToList();

                                        /* list cate for product */
                                        result.ListCategory = result.ListProduct.GroupBy(o => o.CategoryID).Select(o => new CategoryDTO()
                                        {
                                            ID = o.Key,
                                            Name = o.Select(c => c.CategoryName).FirstOrDefault(),
                                            GLCode = o.Select(c => c.GLCode).FirstOrDefault(),
                                            Sequence = o.Select(c => c.CategorySequence).FirstOrDefault(),
                                            CategoryType = o.Select(c => c.TypeID).FirstOrDefault(),
                                        }).ToList();
                                        result.ListCategory = result.ListCategory.OrderBy(o => o.CategoryType).ThenBy(o => o.Sequence).ThenBy(o => o.Name).ThenBy(o => o.ID).ToList();

                                        /* response data */
                                        response.Data = result;
                                        response.Success = true;
                                    }
                                    else
                                        response.Message = dynamicObj["Message"];
                                }
                            }
                        }
                        else
                        {
                            response.Message = "Store is invalid.";
                        }
                    }
                    NSLog.Logger.Info("ResponseProductGetListForOrder", response);
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        private void AddListPromotionForProduct(List<ProductOnKioskDTO> listProduct, string storeID)
        {
            try
            {
                if (listProduct.Count > 0)
                {
                    /* check promotion for product */
                    var busPromo = new NSBusPromotion();
                    var listPromotion = ((GetPromotionResponse)busPromo.GetPromotion(storeID).Data).ListPromotion;

                    var listPromoSpecificID = new List<string>();
                    var listPromoEarnOnSpecific = listPromotion.Where(o => o.ListEarningRule.Where(e => e.EarnType == (byte)Constants.EEarnType.SpecificItem).Count() > 0).ToList();
                    listPromoSpecificID.AddRange(listPromoEarnOnSpecific.Select(o => o.ID).ToList());
                    var listPromoEarnOnSpend_Specific = listPromotion.Where(o => !listPromoSpecificID.Contains(o.ID))
                        .Where(o => o.ListEarningRule.Where(e => e.EarnType == (byte)Constants.EEarnType.SpentItem).Count() > 0     /* earn on spent item */
                        && o.ListSpendingRule.Where(s => s.SpendOnType == (byte)Constants.ESpendOnType.SpecificItem).Count() > 0)   /* spend on specific */
                        .ToList();
                    listPromoSpecificID.AddRange(listPromoEarnOnSpend_Specific.Select(o => o.ID).ToList());

                    /* list promotion on total */
                    var listPromotionOnTotal = listPromotion.Where(o => !listPromoSpecificID.Contains(o.ID)).ToList();

                    foreach (var product in listProduct)
                    {
                        product.ListPromotion = GetListPromoEarnOnProduct(listPromoEarnOnSpecific, listPromoEarnOnSpend_Specific, product.ID);
                        if (product.ListPromotion.Count > 0)
                            product.ListPromotion.AddRange(listPromotionOnTotal);
                    }
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorAddListPromotionForProduct", ex); };

        }

        private List<PromotionDTO> GetListPromoEarnOnProduct(List<PromotionDTO> listPromoEarnOnSpecific, List<PromotionDTO> listPromoEarnOnSpend_Specific, string productID)
        {
            var ret = new List<PromotionDTO>();
            try
            {
                listPromoEarnOnSpecific = listPromoEarnOnSpecific.Where(o => o.ListEarningRule.Where(e => e.ListProduct.Select(p => p.ProductID).ToList().Contains(productID)).Count() > 0).ToList();
                listPromoEarnOnSpend_Specific = listPromoEarnOnSpend_Specific.Where(o => o.ListSpendingRule.Where(e => e.ListProduct.Select(p => p.ProductID).ToList().Contains(productID)).Count() > 0).ToList();
                ret.AddRange(listPromoEarnOnSpecific);
                ret.AddRange(listPromoEarnOnSpend_Specific);
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorGetListPromoEanOnProduct", ex); };
            return ret;
        }
    }
}
