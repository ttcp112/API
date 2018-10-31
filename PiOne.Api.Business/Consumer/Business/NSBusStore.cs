using Newtonsoft.Json;
using PiOne.Api.Business.DTO;
using PiOne.Api.Business.Support;
using PiOne.Api.Business.SupportObject;
using PiOne.Api.Common;
using PiOne.Api.Core.Request;
using PiOne.Api.Core.Response;
using PiOne.Api.DataModel.Context;
using PiOne.Api.DataModel.PiOneEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiOne.Api.Business.Consumer.Business
{
    public class NSBusStore : NSBusBase, IBusiness
    {
        public NSApiResponse GetStoreInfo(string _storeId, string _merchantId)
        {
            var response = new NSApiResponse();

            try
            {
                using (var _db = new PiOneDb())
                {
                    if (!string.IsNullOrEmpty(_storeId))
                    {
                        GetListStoreDTO result = CallGetStoreInfo_NIOne(_storeId, _merchantId, null);
                        if (result != null)
                        {
                            UpdateStoreInfo(result.ListStore);

                            result.ListStore.ForEach(o => o.ImageData = "");
                            response.Data = result;
                            response.Success = true;
                        }
                    }
                    else
                    {
                        response.Message = "Unable to find store.";
                    }

                    NSLog.Logger.Info("ResponseGetStoreInfo", response);
                }
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally
            { /*_db.Refresh();*/
            }
            return response;
        }

        public NSApiResponse ScanQRCode(string _tableId, string _memberId, string _storeId)
        {
            var response = new NSApiResponse();

            try
            {
                GetListStoreDTO result = new GetListStoreDTO();

                if (_tableId.Length < 30) // -- Flow Scan To Register Artist (_tableId = SpecCode)
                {
                    NSLog.Logger.Info("ScanQRCode-RegisterArtist");
                    NSBusMember nsBusMember = new NSBusMember();
                    NSApiResponse responseRegis = nsBusMember.RegisterArtist(_memberId, _tableId);
                    if (responseRegis.Success)
                    {
                        result.IsArtist = responseRegis.Success;
                        response.Data = result;
                        response.Success = true;
                    }
                    else
                    {
                        response.Message = responseRegis.Message;
                    }                 
                }
                else // -- Flow Scan QR Table
                {
                    /* Return Info Of Store */
                    result = CallGetStoreInfo_NIOne(_storeId, null, _tableId); // scan TableId
                    if (result != null)
                    {
                        if (result.ListStore != null && result.ListStore.Count > 0)
                        {
                            StoreDTO storeDto = result.ListStore[0];

                            if (!string.IsNullOrEmpty(storeDto.ID))
                            {
                                // Insert Merchant + Store + MemberLocation
                                bool validInsert = InsertInfoScan(storeDto, _memberId);

                                if (validInsert)
                                {
                                    response.Data = result;
                                    response.Success = true;
                                    /* Sync member to IOne */
                                    new Thread(() =>
                                    {
                                        SyncMemberToIOne(storeDto.ID, _memberId);
                                    }).Start();
                                }
                                else
                                {
                                    response.Message = "Unable to scan QR Code.";
                                }
                            }
                        }
                    }
                }                

                NSLog.Logger.Info("ResponseScanQRCode", response);
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally
            { /*_db.Refresh();*/
            }
            return response;
        }

        private GetListStoreDTO CallGetStoreInfo_NIOne(string _storeId, string _merchantId, string _tableId)
        {
            GetListStoreDTO result = null;
            try
            {                
                using (var client = new HttpClient())
                {
                    /* Setup Request */
                    var request = new
                    {
                        Id = _storeId,
                        MerchantID = _merchantId,
                        TableID = _tableId
                    };

                    /* Call Bar */
                    NSLog.Logger.Info("CallGetStoreInfo-NIOne", request);
                    string webURL = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(webURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Store_GetInfo, request).Result;

                    /* Check Result */
                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        if ((bool)dynamicObj["Success"] == true)
                        {
                            var content = JsonConvert.SerializeObject(dynamicObj["Data"]);
                            result = JsonConvert.DeserializeObject<GetListStoreDTO>(content);
                        }
                    }
                    NSLog.Logger.Info("ResponseCallGetStoreInfo-NIOne", responseFromNSApi);
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Error-CallGetStoreInfo_NIOne", ex);
            }
            finally
            { /*_db.Refresh();*/
            }
            return result;
        }

        private void SyncMemberToIOne(string _storeId, string _memberId)
        {
            try
            {
                NSLog.Logger.Info("SyncMemberToIOne - Start");

                using (var _db = new PiOneDb())
                {
                    var objMember = _db.Members.Where(o => o.Status != (byte)Constants.EStatus.Deleted && o.ID == _memberId).FirstOrDefault();

                    if (objMember != null)
                    {
                        string imageData = string.Empty;
                        if (!string.IsNullOrEmpty(objMember.ProfileImageUrl))
                        {
                            imageData = CommonFunction.ConvertImageURLToBase64(ConfigurationManager.AppSettings["PublicImages"] + objMember.ProfileImageUrl);
                        }

                        CustomerDTO cusDto = new CustomerDTO()
                        {
                            MemberID = objMember.ID,
                            IC = objMember.IC,
                            Name = objMember.Name,
                            ImageURL = objMember.ProfileImageUrl,
                            ImageData = imageData,
                            IsActive = true,
                            Phone = objMember.Mobile,
                            Email = objMember.Email,
                            Gender = objMember.Gender.HasValue ? objMember.Gender.Value : true,
                            Marital = objMember.IsMarried,
                            JoinedDate = DateTime.UtcNow,
                            BirthDate = objMember.DOB.HasValue ? objMember.DOB.Value : DateTime.UtcNow,
                            Anniversary = objMember.Anniversary.HasValue ? objMember.Anniversary.Value : DateTime.UtcNow,
                            ValidTo = DateTime.UtcNow,
                            IsMembership = !string.IsNullOrEmpty(objMember.MemberCard) ? true : false,
                            IsWantMembership = true,
                            HomeStreet = objMember.HomeAddress,
                            HomeCity = objMember.HomeCity,
                            HomeZipCode = objMember.HomePostalCode,
                            HomeCountry = objMember.HomeCountry,
                            OfficeStreet = objMember.OfficeAddress,
                            OfficeCity = objMember.OfficeCity,
                            OfficeZipCode = objMember.OfficePostalCode,
                            OfficeCountry = objMember.OfficeCountry,
                            TotalPaidAmout = 0,
                            ByCash = 0,
                            ByExTerminal = 0,
                            ByGiftCard = 0,
                            TotalRefund = 0,
                            LastVisited = DateTime.UtcNow,
                            Reservation = 0,
                            Cancelation = 0,
                            WalkIn = 0, 
                            Index = null,
                            PoinsID = null,
                            CompanyReg = null,
                        };

                        using (var client = new HttpClient())
                        {
                            /* Setup Request */
                            var request = new
                            {
                                CustomerDTO = cusDto,
                                CreatedUser = _memberId,
                                StoreId = _storeId,
                            };

                            /* Call NIOne */
                            NSLog.Logger.Info("CallCreateOrUpdateCustomer-NIOne", request);
                            string webURL = ConfigurationManager.AppSettings["PosApi"];
                            client.BaseAddress = new Uri(webURL);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Customer_InsertOrUpdate, request).Result;

                            /* Check Result */
                            if (responseFromNSApi.IsSuccessStatusCode)
                            {
                            }
                            NSLog.Logger.Info("ResponseCallCreateOrUpdateCustomer-NIOne", responseFromNSApi);
                        }
                    }

                }
                NSLog.Logger.Info("SyncMemberToIOne - End");
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Error-SyncMemberToIOne", ex);
            }
        }

        private bool InsertInfoScan(StoreDTO _storeDto, string _memberId)
        {
            bool result = false;
            try
            {                
                NSLog.Logger.Info("InsertInfoScan - Start");
                using (var _db = new PiOneDb())
                {
                    var delete = (byte)Constants.EStatus.Deleted;
                    bool flag_update_db = false;
                    
                    if (_storeDto.MerchantPiOne != null)
                    {
                        MerchantDTO merchantDto = _storeDto.MerchantPiOne;

                        #region Insert Merchant

                        /* Insert Merchant */
                        var merchantDup = _db.Merchants.Where(o => o.ID == merchantDto.ID).FirstOrDefault();
                        if (merchantDup == null)
                        {
                            Merchant objMerchant = new Merchant()
                            {
                                ID = merchantDto.ID,
                                MerchantCode = merchantDto.MerchantCode,
                                MerchantImageUrl = merchantDto.MerchantImageUrl,
                                Name = merchantDto.Name,
                                Description = merchantDto.Description,
                                Address = merchantDto.Address,
                                City = merchantDto.City,
                                Country = merchantDto.Country,
                                PostalCode = merchantDto.PostalCode,
                                ContactPerson = merchantDto.ContactPerson,
                                Email = merchantDto.Email,
                                Telephone = merchantDto.Telephone,
                                Fax = merchantDto.Fax,
                                Remarks = merchantDto.Remarks,
                                ColorCode = merchantDto.ColorCode,
                                IsFree = merchantDto.IsFree,
                                ActiveCode = merchantDto.ActiveCode,
                                Status = (byte)Constants.EStatus.Actived,
                                CreatedBy = _memberId,
                                CreatedDate = DateTime.UtcNow,
                                ModifiedBy = _memberId,
                                ModifiedDate = DateTime.UtcNow
                            };
                            _db.Merchants.Add(objMerchant);
                            flag_update_db = true;
                        }
                        else
                        {
                            if (merchantDup.Status == delete)
                            {
                                merchantDup.Status = (byte)Constants.EStatus.Actived;
                                flag_update_db = true;
                            }
                        }

                        #endregion End Insert Merchant

                        #region Insert Store

                        /* Insert Store */
                        var storeDup = _db.Stores.Where(o => o.ID == _storeDto.ID).FirstOrDefault();
                        if (storeDup == null)
                        {
                            Store objStore = new Store()
                            {
                                ID = _storeDto.ID,
                                MerchantID = merchantDto.ID,
                                StoreImageUrl = _storeDto.ImageURL,
                                StoreCode = _storeDto.StoreCode,
                                Name = _storeDto.Name,
                                Description = _storeDto.Description,
                                ContactPerson = null,
                                Email = _storeDto.Email,
                                Telephone = _storeDto.Phone,
                                Fax = null,
                                Address = _storeDto.Address,
                                City = _storeDto.City,
                                District = null,
                                Country = _storeDto.Country,
                                PostalCode = null,
                                Longtitude = null,
                                Latitude = null,
                                GSTRegNo = _storeDto.GSTRegNo,
                                TimeZone = 0,
                                IsBooking = false,
                                IsActive = _storeDto.IsActive,
                                Remarks = null,
                                Status = (byte)Constants.EStatus.Actived,
                                CreatedBy = _memberId,
                                CreatedDate = DateTime.UtcNow,
                                ModifiedBy = _memberId,
                                ModifiedDate = DateTime.UtcNow
                            };
                            _db.Stores.Add(objStore);
                            flag_update_db = true;
                        }
                        else
                        {
                            if (storeDup.Status == delete)
                            {
                                storeDup.Status = (byte)Constants.EStatus.Actived;
                                flag_update_db = true;
                            }
                        }

                        #endregion End Insert Store

                        #region Insert MemberLocation

                        /* Insert MemberLocation */
                        var memLocDup = _db.MemberLocations.Where(o => o.Status != delete && o.MerchantID == merchantDto.ID 
                                                                    && o.StoreID == _storeDto.ID && o.MemberID == _memberId).FirstOrDefault();
                        if (memLocDup == null)
                        {
                            MemberLocation objMemberLocation = new MemberLocation()
                            {
                                ID = Guid.NewGuid().ToString(),
                                MerchantID = merchantDto.ID,
                                StoreID = _storeDto.ID,
                                MemberID = _memberId,
                                Status = (byte)Constants.EStatus.Actived,
                                CreatedBy = _memberId,
                                CreatedDate = DateTime.UtcNow,
                                ModifiedBy = _memberId,
                                ModifiedDate = DateTime.UtcNow
                            };
                            _db.MemberLocations.Add(objMemberLocation);
                            flag_update_db = true;
                        }

                        #endregion End Insert MemberLocation

                        if (_db.SaveChanges() > 0 || flag_update_db == false)
                        {
                            result = true;
                        }
                    }                    
                }
                NSLog.Logger.Info("InsertInfoScan - End");
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Error-InsertInfoScan", ex);
            }

            return result;
        }

        public NSApiResponse SearchMerchant(RequestGetListMerchant input)
        {
            var response = new NSApiResponse();

            try
            {
                using (var client = new HttpClient())
                {
                    var info = new RequestModelBase { ID = input.ID, DeviceName = input.SearchString, PageIndex = input.PageIndex, PageSize = input.PageSize };
                    var result = new ResponseGetListMerchantSearch();
                    NSLog.Logger.Info("CallSearchMerchant-NIOne", input);
                    string webURL = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(webURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Merchant_Search, info).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        response.Success = (bool)dynamicObj["Success"];
                        if (response.Success == true)
                        {
                            var content = JsonConvert.SerializeObject(dynamicObj["Data"]);
                            result = JsonConvert.DeserializeObject<ResponseGetListMerchantSearch>(content);
                            response.Data = result;
                        }
                        response.Message = dynamicObj["Message"];
                    }
                    NSLog.Logger.Info("ResponseCallSearchMerchant-NIOne", responseFromNSApi);
                }

                NSLog.Logger.Info("ResponseSearchMerchant", response);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("ErrorSearchMerchant", null, response, ex);
            }
            return response;
        }

        public NSApiResponse GetListStore(RequestGetListStore input)
        {
            var response = new NSApiResponse();

            try
            {
                using (var client = new HttpClient())
                {
                    var info = new RequestModelBase { ID = input.ID, PageIndex = input.PageIndex, PageSize = input.PageSize };

                    var result = new GetListStoreDTO();
                    NSLog.Logger.Info("CallGetListStore-NIOne", input);
                    string webURL = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(webURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Store_GetList, info).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        response.Success = (bool)dynamicObj["Success"];
                        if (response.Success == true)
                        {
                            var content = JsonConvert.SerializeObject(dynamicObj["Data"]);
                            result = JsonConvert.DeserializeObject<GetListStoreDTO>(content);
                            response.Data = result;
                        }
                        response.Message = dynamicObj["Message"];
                    }
                    NSLog.Logger.Info("ResponseCallGetListStore-NIOne", responseFromNSApi);
                }

                NSLog.Logger.Info("ResponseGetListStore", response);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("ErrorGetListStore", null, response, ex);
            }
            return response;
        }

        private NSApiResponse UpdateStoreInfo(List<StoreDTO> _lstStoreDto)
        {
            NSApiResponse response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    if (_lstStoreDto != null && _lstStoreDto.Count > 0)
                    {
                        var lstStoreId = _lstStoreDto.Select(o => o.ID).ToList();
                        var lstStoreDb = _db.Stores.Where(o => lstStoreId.Contains(o.ID) && o.Status != (byte)Constants.EStatus.Deleted).ToList();
                        bool isUpdate = false;

                        foreach (var item in _lstStoreDto)
                        {
                            var _storeDb = lstStoreDb.Where(o => o.ID == item.ID).FirstOrDefault();

                            if (_storeDb != null)
                            {
                                if (!string.IsNullOrEmpty(item.Name) && item.Name.Trim() != _storeDb.Name.Trim())
                                {
                                    _storeDb.Name = item.Name;
                                    isUpdate = true;
                                }

                                if (!string.IsNullOrEmpty(item.ImageData))
                                {
                                    string _imageDataDb = "";
                                    if (!string.IsNullOrEmpty(_storeDb.StoreImageUrl))
                                    {
                                        _imageDataDb = CommonFunction.ConvertImageURLToBase64(ConfigurationManager.AppSettings["PublicImages"] + _storeDb.StoreImageUrl);
                                    }

                                    if (_imageDataDb != item.ImageData)
                                    {
                                        string outImageURL = item.ImageURL;
                                        CommonFunction.UploadImage(item.ImageData, ref outImageURL);
                                        _storeDb.StoreImageUrl = outImageURL;
                                        isUpdate = true;
                                    }
                                }                                
                            }
                            else
                            {
                                response.Message = "Unable to find store.";
                            }
                        }

                        if (isUpdate && _db.SaveChanges() > 0)
                        {
                            response.Success = true;
                        }
                        else if (!isUpdate)
                        {
                            response.Success = true;
                            response.Message = "No update.";
                        }
                        else
                            response.Message = "Unable to update store info.";
                    }
                    NSLog.Logger.Info("UpdateStoreInfo", response);
                }
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }
    }
}
