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
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiOne.Api.Business.Consumer.Business
{
    public class NSBusMember : NSBusBase, IBusiness
    {
        public NSApiResponse MemberLogin(string _nickname, string _password, string _deviceToken, byte _deviceType, string _langID, string _uuid)
        {
            var response = new NSApiResponse();

            try
            {
                using (var _db = new PiOneDb())
                {
                    ResponseMemberLogin result = new ResponseMemberLogin();
                    if (string.IsNullOrEmpty(_langID)) _langID = null;

                    var objUser = _db.Members.Where(o => o.LoginID.Trim().ToLower() == _nickname.Trim().ToLower() && o.Password == _password).FirstOrDefault();
                    if (objUser != null && !string.IsNullOrEmpty(objUser.ID))
                    {
                        #region Device

                        if (!string.IsNullOrEmpty(_deviceToken))
                        {
                            var device = _db.Devices.Where(o => o.DeviceToken == _deviceToken).FirstOrDefault();
                            if (device == null)
                            {
                                device = new Device()
                                {
                                    ID = Guid.NewGuid().ToString(),
                                    LanguageID = _langID,
                                    IsDeleted = false,
                                    DeviceToken = _deviceToken,
                                    DeviceType = _deviceType,
                                    Status = (byte)Constants.EStatus.Actived,
                                    CreatedBy = objUser.ID,
                                    CreatedDate = DateTime.UtcNow,
                                    ModifiedBy = objUser.ID,
                                    ModifiedDate = DateTime.UtcNow,
                                    MemberID = objUser.ID,
                                    UUID = _uuid
                                };
                                _db.Devices.Add(device);
                            }
                            else
                            {
                                device.MemberID = objUser.ID;
                                if (!string.IsNullOrEmpty(_langID))
                                    device.LanguageID = _langID;
                                device.Status = (byte)Constants.EStatus.Actived;
                                device.ModifiedBy = objUser.ID;
                                device.ModifiedDate = DateTime.UtcNow;
                                device.UUID = _uuid;
                            }
                            _db.SaveChanges();
                        }

                        #endregion End Device

                        /* Member */
                        MemberDTO dto = new MemberDTO()
                        {
                            ID = objUser.ID,
                            LoginID = objUser.LoginID,
                            ImageURL = string.IsNullOrEmpty(objUser.ProfileImageUrl) ? "" : ConfigurationManager.AppSettings["PublicImages"] + objUser.ProfileImageUrl,
                            Name = objUser.Name,
                            Email = objUser.Email,
                            Mobile = objUser.Mobile,
                            IC = objUser.IC,
                            Passport = objUser.Passport,
                            Points = objUser.Points,
                            MemberTier = objUser.MemberTier,
                            Gender = objUser.Gender,
                            DOB = objUser.DOB,
                            IsMarried = objUser.IsMarried,
                            Anniversary = objUser.Anniversary,
                            MemberSince = objUser.MemberSince,
                            HomeAddress = objUser.HomeAddress,
                            HomeCity = objUser.HomeCity,
                            HomeCountry = objUser.HomeCountry,
                            HomePostalCode = objUser.HomePostalCode,
                            OfficeAddress = objUser.OfficeAddress,
                            OfficeCity = objUser.OfficeCity,
                            OfficeCountry = objUser.OfficeCountry,
                            OfficePostalCode = objUser.OfficePostalCode,
                            Remarks = objUser.Remarks,
                            IsArtist = objUser.IsArtist,
                            IsPDPA = objUser.IsPDPA
                        };
                        result.Member = dto;
                        result.SocketPort = ConfigurationManager.AppSettings["SocketIOPort"];
                        //get tier
                        if (!string.IsNullOrEmpty(dto.MemberTier))
                        {
                            int codeTier = 0;
                            Int32.TryParse(dto.MemberTier, out codeTier);
                            var tierSetting = CommonFunction.GetTierConfig(_db);
                            result.Member.MemberTier = tierSetting.ListTier.Where(o => o.Code == codeTier).Select(o => o.Name).FirstOrDefault();
                        }
                        /* Member Location */
                        var lstMemberLocation = _db.MemberLocations.Where(o => o.Status != (byte)Constants.EStatus.Deleted && o.MemberID == objUser.ID).ToList();
                        var lstStoreId = lstMemberLocation.Select(o => o.StoreID).ToList();
                        var lstStore = _db.Stores.Where(o => o.Status != (byte)Constants.EStatus.Deleted && lstStoreId.Contains(o.ID)).ToList();

                        result.ListLocation = lstMemberLocation.Join(lstStore, m => m.StoreID, s => s.ID, (m, s) =>
                                                                    new MemberLocationDTO()
                                                                    {
                                                                        ID = m.ID,
                                                                        MerchantID = m.MerchantID,
                                                                        StoreID = m.StoreID,
                                                                        StoreName = s.Name,
                                                                        ImageURL = string.IsNullOrEmpty(s.StoreImageUrl) ? "" : ConfigurationManager.AppSettings["PublicImages"] + s.StoreImageUrl
                                                                    }).ToList();

                        // -- NumOfNewMessage
                        List<int> listMessageType = new List<int>()
                        {
                            (int)Constants.EMessageType.Chat,
                            (int)Constants.EMessageType.Gift,
                        };
                        var queryMemNoti = _db.MemberNotifications.Where(o => (o.ArtistID == dto.ID || o.MemberID == dto.ID) && o.Status != (byte)Constants.EStatus.Deleted
                                                                                 && listMessageType.Contains(o.Type));
                        result.NumOfNewMessageChatting = queryMemNoti.Where(o => o.MemberID == dto.ID && !o.IsMemberView).GroupBy(o => o.ArtistID).Count(); // chat
                        result.NumOfNewMessage = queryMemNoti.Where(o => o.ArtistID == dto.ID && !o.IsSenderView && !string.IsNullOrEmpty(o.MemberID)).GroupBy(o => o.MemberID).Count(); // report

                        // -- NumOfBottleService
                        string mess = "";
                        result.NumOfBottleService = NSBusBottle.GetListBottleIntegrate(objUser.ID, ref mess).Count();

                        //--get num of upcoming booking
                        var reqBooking = new RequestGetListBooking() { BookingRequestType = (byte)Constants.EBookingRequestType.Upcoming, MemberID = dto.ID, PageIndex = 1, PageSize = 9999 };
                        NSBusBooking busBook = new NSBusBooking();
                        var resBook = busBook.GetListBooking(reqBooking);
                        if (resBook.Success == true)
                        {
                            var dataBook = (GetListReservationResponse)resBook.Data;
                            if (dataBook != null)
                                result.NumOfUpcomingBooking = dataBook.ListReser.Count();
                        }
                        response.Data = result;
                        response.Success = true;
                    }
                    else
                    {
                        response.Message = "Username or password is incorrect.";
                    }

                    NSLog.Logger.Info("ResponseMemberLogin", response);
                }
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse RegisterMember(MemberDTO _input, string _deviceToken, byte _deviceType, string _langID, string _uuid)
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    if (CommonFunction.CheckValidateAccount(_input.LoginID))
                    {
                        var checkNickname = _db.Members.Where(o => o.LoginID.ToLower().Trim() == _input.LoginID.ToLower().Trim() && o.Status != (byte)Constants.EStatus.Deleted).FirstOrDefault();
                        if (checkNickname == null)
                        {
                            var checkEmail = _db.Members.Where(o => !string.IsNullOrEmpty(o.Email) && o.Email.Trim() == _input.Email.Trim() && o.Status != (byte)Constants.EStatus.Deleted).FirstOrDefault();
                            if (checkEmail == null)
                            {
                                string userID = Guid.NewGuid().ToString();

                                #region Bind Data

                                Member member = new Member()
                                {
                                    ID = userID,
                                    LoginID = _input.LoginID,
                                    Password = _input.Password,
                                    Name = _input.Name ?? _input.LoginID,
                                    Email = _input.Email,
                                    Mobile = _input.Mobile,
                                    IC = _input.IC,
                                    Passport = _input.Passport,
                                    Points = _input.Points,
                                    MemberTier = _input.MemberTier,
                                    Gender = _input.Gender,
                                    DOB = _input.DOB,
                                    IsMarried = _input.IsMarried,
                                    Anniversary = _input.Anniversary,
                                    MemberSince = _input.MemberSince,
                                    HomeAddress = _input.HomeAddress,
                                    HomeCity = _input.HomeCity,
                                    HomeCountry = _input.HomeCountry,
                                    HomePostalCode = _input.HomePostalCode,
                                    OfficeAddress = _input.OfficeAddress,
                                    OfficeCity = _input.OfficeCity,
                                    OfficeCountry = _input.OfficeCountry,
                                    OfficePostalCode = _input.OfficePostalCode,
                                    Remarks = _input.Remarks,
                                    Status = (byte)Constants.EStatus.Actived,
                                    CreatedBy = userID,
                                    CreatedDate = DateTime.UtcNow,
                                    ModifiedBy = userID,
                                    ModifiedDate = DateTime.UtcNow,
                                    IsPDPA = _input.IsPDPA
                                };

                                if (!string.IsNullOrEmpty(_input.ImageData))
                                {
                                    string outImageURL = "";
                                    CommonFunction.UploadImage(_input.ImageData, ref outImageURL);
                                    member.ProfileImageUrl = outImageURL;
                                }

                                #endregion End Bind Data

                                #region Device

                                var currentDevice = _db.Devices.Where(o => o.DeviceToken == _deviceToken).FirstOrDefault();
                                if (!string.IsNullOrEmpty(_deviceToken))
                                {
                                    if (string.IsNullOrEmpty(_langID)) _langID = null;
                                    if (currentDevice != null)
                                    {
                                        currentDevice.MemberID = userID;
                                        currentDevice.DeviceToken = _deviceToken;
                                        currentDevice.DeviceType = _deviceType;
                                        if (!string.IsNullOrEmpty(_langID))
                                            currentDevice.LanguageID = _langID;
                                        currentDevice.IsDeleted = false;
                                        currentDevice.Status = (byte)Constants.EStatus.Actived;
                                        currentDevice.ModifiedBy = userID;
                                        currentDevice.ModifiedDate = DateTime.UtcNow;
                                        currentDevice.UUID = _uuid;
                                    }
                                    else
                                    {
                                        Device device = new Device()
                                        {
                                            ID = Guid.NewGuid().ToString(),
                                            LanguageID = _langID,
                                            IsDeleted = false,
                                            DeviceToken = _deviceToken,
                                            DeviceType = _deviceType,
                                            Status = (byte)Constants.EStatus.Actived,
                                            CreatedBy = userID,
                                            CreatedDate = DateTime.UtcNow,
                                            ModifiedBy = userID,
                                            ModifiedDate = DateTime.UtcNow,
                                            MemberID = userID,
                                            UUID = _uuid
                                        };
                                        _db.Devices.Add(device);
                                    }
                                }

                                #endregion End Device

                                #region Noti Setting

                                NotificationSetting notiSetting = new NotificationSetting()
                                {
                                    ID = Guid.NewGuid().ToString(),
                                    MemberID = userID,
                                    IsReceiveNotification = true,
                                    Status = (byte)Constants.EStatus.Actived,
                                    CreatedBy = userID,
                                    CreatedDate = DateTime.UtcNow,
                                    ModifiedBy = userID,
                                    ModifiedDate = DateTime.UtcNow
                                };

                                #endregion

                                _db.Members.Add(member);
                                _db.NotificationSettings.Add(notiSetting);
                                if (_db.SaveChanges() > 0)
                                {
                                    response.Success = true;
                                    response.Data.ID = member.ID;
                                    response.Data.Description = string.IsNullOrEmpty(member.ProfileImageUrl) ? "" : ConfigurationManager.AppSettings["PublicImages"] + member.ProfileImageUrl;
                                }
                                else
                                    response.Message = "Unable to register member.";
                            }
                            else
                                response.Message = "Email already exists.";
                        }
                        else
                            response.Message = "Nickname already exists.";
                    }
                    else
                    {
                        response.Message = "Nickname format incorrect.";
                    }

                    NSLog.Logger.Info("ReponseRegisterMember", response);
                }
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse ForgotPassword(string _email)
        {
            var response = new NSApiResponse();

            try
            {
                using (var _db = new PiOneDb())
                {
                    var member = _db.Members.Where(o => !string.IsNullOrEmpty(o.Email) && o.Email.Trim() == _email.Trim() && o.Status == (byte)Constants.EStatus.Actived).FirstOrDefault();
                    if (member != null)
                    {
                        var listCode = _db.Members.Where(o => !string.IsNullOrEmpty(o.PasswordCode)).Select(o => o.PasswordCode).ToList();
                        member.PasswordCode = CommonFunction.GeneratePasswordCode(listCode);
                        member.ModifiedDate = DateTime.UtcNow;
                        if (_db.SaveChanges() > 0)
                        {
                            response.Success = true;
                            response.Data.ID = member.ID;
                            response.Data.Description = member.PasswordCode;

                            // Send Mail
                            string content = "Security Code: " + member.PasswordCode;
                            CommonFunction.SendContentMail(member.Email, content, "Newstead", "Forgot Password", "");
                        }
                        else
                            response.Message = "Unable to get password. Please try again!";
                    }
                    else
                    {
                        response.Message = "Please enter a valid email.";
                    }
                    NSLog.Logger.Info("ResponseForgotPasswordMember", response);
                }
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse ChangePassword(string _memberId, string _oldPassword, string _newPassword, string _securCode)
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    if (!string.IsNullOrEmpty(_memberId))
                    {
                        var member = _db.Members.Where(o => o.ID == _memberId && o.Status != (byte)Constants.EStatus.Deleted).FirstOrDefault();
                        if (member != null)
                        {
                            if (!string.IsNullOrEmpty(_oldPassword)) // Flow Change Password 
                            {
                                if (member.Password.ToLower() == _oldPassword.ToLower())
                                {
                                    member.Password = _newPassword;
                                    member.ModifiedDate = DateTime.UtcNow;
                                    if (_db.SaveChanges() > 0)
                                    {
                                        response.Success = true;
                                    }
                                    else
                                        response.Message = "Unable to change password.";
                                }
                                else
                                    response.Message = "Current password is incorrect.";
                            }
                            else if (!string.IsNullOrEmpty(_securCode)) // Flow Reset Password
                            {
                                if (member.PasswordCode.Trim() == _securCode.Trim())
                                {
                                    member.Password = _newPassword;
                                    member.ModifiedDate = DateTime.UtcNow;
                                    if (_db.SaveChanges() > 0)
                                    {
                                        response.Success = true;
                                    }
                                    else
                                        response.Message = "Unable to reset password.";
                                }
                            }
                            else
                            {
                                response.Message = "Unable to find old password or security code.";
                            }
                        }
                        else
                            response.Message = "Unable to find account.";
                    }
                    else
                        response.Message = "Unable to find account.";
                    NSLog.Logger.Info("ResponseChangePasswordMember", response);
                }
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }

            return response;
        }

        public NSApiResponse EditProfile(MemberDTO _input)
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    var checkEmail = _db.Members.Where(o => !string.IsNullOrEmpty(o.Email) && o.Email.Trim() == _input.Email.Trim() && o.Status != (byte)Constants.EStatus.Deleted).FirstOrDefault();

                    if (checkEmail != null && checkEmail.ID != _input.ID)
                    {
                        response.Message = "Email already exists.";
                    }
                    else
                    {
                        var member = _db.Members.Where(o => o.ID == _input.ID).FirstOrDefault();
                        if (member != null)
                        {
                            #region Bind Data

                            member.Name = _input.Name;
                            member.Email = _input.Email;
                            member.Mobile = _input.Mobile;
                            member.IC = _input.IC;
                            member.Passport = _input.Passport;
                            //member.Points = _input.Points;
                            //member.MemberTier = _input.MemberTier;
                            member.Gender = _input.Gender;
                            member.DOB = _input.DOB;
                            member.IsMarried = _input.IsMarried;
                            member.Anniversary = _input.Anniversary;
                            member.MemberSince = _input.MemberSince;
                            member.HomeAddress = _input.HomeAddress;
                            member.HomeCity = _input.HomeCity;
                            member.HomeCountry = _input.HomeCountry;
                            member.HomePostalCode = _input.HomePostalCode;
                            member.OfficeAddress = _input.OfficeAddress;
                            member.OfficeCity = _input.OfficeCity;
                            member.OfficeCountry = _input.OfficeCountry;
                            member.OfficePostalCode = _input.OfficePostalCode;
                            member.Remarks = _input.Remarks;
                            member.ModifiedBy = _input.ID;
                            member.ModifiedDate = DateTime.UtcNow;
                            member.IsPDPA = _input.IsPDPA;

                            if (!string.IsNullOrEmpty(_input.ImageData))
                            {
                                string outImageURL = "";
                                CommonFunction.UploadImage(_input.ImageData, ref outImageURL);
                                member.ProfileImageUrl = outImageURL;
                            }

                            #endregion End Bind Data

                            if (_db.SaveChanges() > 0)
                            {
                                /* Update Customer At IOne */
                                SyncUpdateCustomerIOne(_input.ID);

                                response.Success = true;
                                response.Data.Description = string.IsNullOrEmpty(member.ProfileImageUrl) ? "" : ConfigurationManager.AppSettings["PublicImages"] + member.ProfileImageUrl;
                            }
                            else
                                response.Message = "Unable to edit member profile.";
                        }
                        else
                            response.Message = "Unable to find member profile.";
                    }

                    NSLog.Logger.Info("ReponseEditProfile", response);
                }
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse GetCustomerHangGift(string _memberId, string _storeId, string _merchantId, int _pageSize, int _pageIndex, DateTime _dateFrom, DateTime _dateTo)
        {
            var response = new NSApiResponse();

            try
            {
                ResponseGetCustomerHangGift result = new ResponseGetCustomerHangGift();
                using (var client = new HttpClient())
                {
                    /* Setup Request */
                    var request = new
                    {
                        MemberId = _memberId,
                        StoreId = _storeId,
                        MerchantId = _merchantId,
                        PageSize = _pageSize,
                        PageIndex = _pageIndex,
                        DateFrom = _dateFrom,
                        DateTo = _dateTo,
                    };

                    /* Call Bar */
                    NSLog.Logger.Info("GetCustomerHangGift-NIOne", request);
                    string webURL = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(webURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Customer_HangGift_Get, request).Result;

                    /* Check Result */
                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        if ((bool)dynamicObj["Success"] == true)
                        {
                            var content = JsonConvert.SerializeObject(dynamicObj["Data"]);
                            result = JsonConvert.DeserializeObject<ResponseGetCustomerHangGift>(content);

                            if (result != null && result.ListCustomer != null && result.ListCustomer.Count > 0)
                            {
                                UpdateIsNewMessageCustomer(_memberId, result.ListCustomer);
                            }

                            response.Data = result;
                            response.Success = true;
                        }
                    }
                    NSLog.Logger.Info("ResponseGetCustomerHangGift-NIOne", responseFromNSApi);
                }
                NSLog.Logger.Info("ResponseGetCustomerHangGift", response);
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally
            { /*_db.Refresh();*/
            }
            return response;
        }

        public NSApiResponse AddEditMemberCard(MemberCardDTO dto)
        {
            var response = new NSApiResponse();

            try
            {
                using (var _db = new PiOneDb())
                {
                    byte delete = (byte)Constants.EStatus.Deleted;

                    var card = _db.MemberCards.Where(o => o.MemberID == dto.MemberID && o.Status != delete && o.CardNumber == dto.CardNumber).FirstOrDefault();
                    if (card == null)
                    {
                        card = new MemberCard();
                        card.ID = Guid.NewGuid().ToString();
                        card.MemberID = dto.MemberID;
                        card.CardNumber = dto.CardNumber;
                        card.Name = dto.CardName;
                        card.ExpiryDate = dto.ExpiryDate;
                        card.CSV = dto.CSV;
                        card.CreatedBy = dto.MemberID;
                        card.ModifiedBy = dto.MemberID;
                        card.CreatedDate = DateTime.UtcNow;
                        card.ModifiedDate = DateTime.UtcNow;

                        _db.MemberCards.Add(card);
                        if (_db.SaveChanges() > 0)
                        {
                            response.Success = true;
                            response.Data.ID = card.ID;
                        }
                    }
                    else//updatae
                    {

                        card.CardNumber = dto.CardNumber;
                        card.Name = dto.CardName;
                        card.ExpiryDate = dto.ExpiryDate;
                        card.CSV = dto.CSV;
                        card.ModifiedBy = dto.MemberID;
                        card.ModifiedDate = DateTime.UtcNow;
                        if (_db.SaveChanges() > 0)
                            response.Success = true;

                    }
                    NSLog.Logger.Info("ResponseAddEditMemberCard", response);
                }
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse GetMemberCard(string memberID)
        {
            var response = new NSApiResponse();

            try
            {
                using (var _db = new PiOneDb())
                {
                    byte delete = (byte)Constants.EStatus.Deleted;
                    var data = new ResponseGetMemberCard();

                    var lstCard = _db.MemberCards.Where(o => o.MemberID == memberID && o.Status != delete).Select(o => new MemberCardDTO
                    {
                        ID = o.ID,
                        MemberID = o.MemberID,
                        CardNumber = o.CardNumber,
                        CardName = o.Name,
                        ExpiryDate = o.ExpiryDate,
                        CSV = o.CSV
                    }).ToList();

                    data.ListCard = lstCard;
                    response.Success = true;
                    response.Data = data;

                    NSLog.Logger.Info("ResponseGetMemberCard", response);
                }
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse RegisterArtist(string memberID, string specCode)
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    var member = _db.Members.Where(o => o.ID == memberID && o.Status != (byte)Constants.EStatus.Deleted).FirstOrDefault();
                    if (member != null)
                    {
                        using (var client = new HttpClient())
                        {
                            RegisterArtistRequestIntegrate request = new RegisterArtistRequestIntegrate
                            {
                                Artist = new IntegrateArtistDTO()
                                {
                                    ID = member.ID,
                                    LoginID = member.LoginID,
                                    ImageUrl = string.IsNullOrEmpty(member.ProfileImageUrl) ? "" : ConfigurationManager.AppSettings["PublicImages"] + member.ProfileImageUrl,
                                    Name = member.Name,
                                },
                                SpecCode = specCode,
                            };

                            NSLog.Logger.Info("CallArtistRegister-NIOne", request);
                            string webURL = ConfigurationManager.AppSettings["PosApi"];
                            client.BaseAddress = new Uri(webURL);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Artist_Register, request).Result;

                            if (responseFromNSApi.IsSuccessStatusCode)
                            {
                                dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                                if ((bool)dynamicObj["Success"] == true)
                                {
                                    member.IsArtist = true;
                                    member.ModifiedDate = DateTime.UtcNow;
                                    if (_db.SaveChanges() > 0)
                                        response.Success = true;
                                }
                                else
                                    response.Message = dynamicObj["Message"];
                            }
                            NSLog.Logger.Info("ResponseCallArtistRegister-NIOne", responseFromNSApi);
                        }
                    }
                    else
                        response.Message = "Unable to find member.";

                    NSLog.Logger.Info("ReponseRegisterArtist", response);
                }
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("ErrorRegisterArtist", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorRegisterArtist", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse ArtistCheckInOut(string memberID, string storeID)
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    var member = _db.Members.Where(o => o.ID == memberID && o.Status != (byte)Constants.EStatus.Deleted).FirstOrDefault();
                    if (member != null)
                    {
                        if (member.IsArtist)
                        {
                            using (var client = new HttpClient())
                            {
                                ArtistCheckInOutRequestIntegrate request = new ArtistCheckInOutRequestIntegrate
                                {
                                    Artist = new IntegrateArtistDTO()
                                    {
                                        ID = member.ID,
                                        LoginID = member.LoginID,
                                        Name = member.Name,
                                    },
                                    StoreID = storeID,
                                };

                                NSLog.Logger.Info("CallArtistCheckInOut-NIOne", request);
                                string webURL = ConfigurationManager.AppSettings["PosApi"];
                                client.BaseAddress = new Uri(webURL);
                                client.DefaultRequestHeaders.Accept.Clear();
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Artist_CheckInOut, request).Result;

                                if (responseFromNSApi.IsSuccessStatusCode)
                                {
                                    dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;

                                    response.Success = (bool)dynamicObj["Success"];
                                    if (response.Success)
                                    {
                                        ArtistCheckInOutResponse result = new ArtistCheckInOutResponse();
                                        result.IsCheckIn = (bool)dynamicObj["Data"]["IsCheckIn"];
                                        result.Description = dynamicObj["Data"]["Description"];
                                        response.Data = result;
                                    }
                                    else
                                        response.Message = dynamicObj["Message"];
                                }
                                NSLog.Logger.Info("ResponseCallArtistCheckInOut-NIOne", responseFromNSApi);
                            }
                        }
                        else
                            response.Message = "Invalid member.";
                    }
                    else
                        response.Message = "Unable to find member.";

                    NSLog.Logger.Info("ReponseArtistCheckInOut", response);
                }
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("ErrorArtistCheckInOut", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorArtistCheckInOut", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        private void UpdateIsNewMessageCustomer(string _memberId, List<CustomerDTO> lstCus)
        {
            try
            {
                using (var _db = new PiOneDb())
                {
                    var lstCusId = lstCus.Select(o => o.MemberID).ToList();
                    var lstMessNoView = _db.MemberNotifications.Where(o => o.Status != (byte)Constants.EStatus.Deleted
                                                    //&& (string.IsNullOrEmpty(_storeId) || (!string.IsNullOrEmpty(_storeId) && o.StoreID == _storeId))
                                                    && o.ArtistID == _memberId && lstCusId.Contains(o.MemberID)
                                                    && o.IsSenderView == false).ToList();
                    var lstMemberNoView = lstMessNoView.Select(o => o.MemberID).Distinct().ToList();
                    foreach (var item in lstMemberNoView)
                    {
                        lstCus.Where(o => o.MemberID == item).FirstOrDefault().IsNewMess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Error-UpdateIsNewMessageCustomer", ex);
            }
        }

        public NSApiResponse GetMember(string memberID)
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    ResponseGetMember result = new ResponseGetMember();

                    result.Member = _db.Members.Where(o => o.ID == memberID)
                        .Select(o => new MemberDTO()
                        {
                            ID = o.ID,
                            Email = o.Email,
                            Name = o.Name,
                            Mobile = o.Mobile,
                            IC = o.IC,
                            Anniversary = o.Anniversary,
                            DOB = o.DOB,
                            Gender = o.Gender,
                            HomeAddress = o.HomeAddress,
                            HomeCity = o.HomeCity,
                            HomeCountry = o.HomeCountry,
                            HomePostalCode = o.HomePostalCode,
                            IsMarried = o.IsMarried,
                            OfficeAddress = o.OfficeAddress,
                            OfficeCity = o.OfficeCity,
                            OfficeCountry = o.OfficeCountry,
                            OfficePostalCode = o.OfficePostalCode,
                            Points = o.Points,
                            MemberTier = o.MemberTier,
                            IsArtist = o.IsArtist,
                            IsPDPA = o.IsPDPA
                        }).FirstOrDefault();

                    //get tier
                    if (!string.IsNullOrEmpty(result.Member.MemberTier))
                    {
                        int codeTier = 0;
                        Int32.TryParse(result.Member.MemberTier, out codeTier);
                        var tierSetting = CommonFunction.GetTierConfig(_db);
                        var tier = tierSetting.ListTier.Where(o => o.Code == codeTier).FirstOrDefault();
                        if (tier != null)
                        {
                            result.Member.MemberTier = tier.Name;
                            result.Member.MemberTierColor = tier.Color;
                            result.Member.MemberTierImage = tier.Img;
                        }
                        new Thread(() => ViewMembershipNoti(memberID)).Start();
                    }

                    response.Success = true;
                    response.Data = result;

                    NSLog.Logger.Info("ReponseGetMember", response);
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorGetListMember", null, response, ex); }
            return response;
        }

        public NSApiResponse GetTier()
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    ResponseGetTier result = new ResponseGetTier();

                    result = CommonFunction.GetTierConfig(_db);
                    response.Success = true;
                    response.Data = result;

                    NSLog.Logger.Info("ReponseGetTier", response);
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorGetListTier", null, response, ex); }
            return response;
        }

        private void SyncUpdateCustomerIOne(string _memberId)
        {
            try
            {
                NSLog.Logger.Info("SyncUpdateCustomerIOne - Start");

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
                            };

                            /* Call NIOne */
                            NSLog.Logger.Info("CallUpdateCustomerFromPiOne-NIOne", request);
                            string webURL = ConfigurationManager.AppSettings["PosApi"];
                            client.BaseAddress = new Uri(webURL);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var responseFromNSApi = client.PostAsJsonAsync(NIOneApiRoute.Customer_UpdateFromPiOne, request).Result;

                            /* Check Result */
                            if (responseFromNSApi.IsSuccessStatusCode)
                            {
                            }
                            NSLog.Logger.Info("ResponseCallUpdateCustomerFromPiOne-NIOne", responseFromNSApi);
                        }
                    }

                }
                NSLog.Logger.Info("SyncUpdateCustomerIOne - End");
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Error-SyncUpdateCustomerIOne", ex);
            }
        }

        public NSApiResponse MemberLogout(string deviceToken, byte deviceType)
        {
            var response = new NSApiResponse();

            try
            {
                using (var _db = new PiOneDb())
                {
                    if (!string.IsNullOrEmpty(deviceToken))
                    {
                        var device = _db.Devices.Where(o => o.DeviceToken == deviceToken && o.DeviceType == deviceType).FirstOrDefault();
                        if (device != null)
                        {
                            device.Status = (byte)Constants.EStatus.Deleted;
                            device.ModifiedDate = DateTime.UtcNow;
                            _db.SaveChanges();
                        }
                    }
                    response.Success = true;

                    NSLog.Logger.Info("ResponseMemberLogout", response);
                }
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse MemberLoyaltyUpdate(string memberID, ITierDTO tier, int point)
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    var member = _db.Members.Where(o => o.ID == memberID).FirstOrDefault();
                    if (member != null)
                    {
                        if (point > 0)
                            member.Points += point;
                        if (tier != null)
                        {
                            int curTier = 0;
                            Int32.TryParse(member.MemberTier, out curTier);
                            if (tier.Code > curTier || (tier.Code == curTier))
                                member.MemberTier = tier.Code.ToString();
                        }
                        member.ModifiedBy = memberID;
                        member.ModifiedDate = DateTime.UtcNow;
                        if (_db.SaveChanges() > 0)
                        {
                            response.Success = true;
                        }
                    }
                    else
                        response.Message = "Unable to update member loyalty.";

                    NSLog.Logger.Info("ReponseUpdateMemberLoyalty", response);
                }
            }
            catch (DbEntityValidationException ex) { EntityValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse MemberNoti(MessageDTO message)
        {
            var response = new NSApiResponse();
            try
            {
                NotificationFuction.PushNoti_Membership(message.MemberID, message.Title, message.Message, message.Dict);
                response.Success = true;
                NSLog.Logger.Info("ResponseMemberNoti", response);
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorMemberNoti", null, response, ex); }
            return response;
        }

        public NSApiResponse SearchMember(string memberId, string name, string email, string ic, string phone)
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    var data = new ResponseGetCustomerHangGift();
                    data.ListCustomer = new List<CustomerDTO>();
                    var mem = new Member();
                    var query = _db.Members.Where(o => o.Status != (byte)Constants.EStatus.Deleted);
                    if (!string.IsNullOrEmpty(memberId))
                        query = query.Where(o => o.ID == memberId);
                    if (!string.IsNullOrEmpty(name))
                        query = query.Where(o => o.Name == name || o.LoginID == name);
                    if (!string.IsNullOrEmpty(email))
                        query = query.Where(o => o.Email == email);
                    if (!string.IsNullOrEmpty(ic))
                        query = query.Where(o => o.IC == ic);
                    if (!string.IsNullOrEmpty(phone))
                        query = query.Where(o => o.Mobile == phone);
                    mem = query.FirstOrDefault();
                    if (mem != null)
                    {
                        string imageData = string.Empty;
                        if (!string.IsNullOrEmpty(mem.ProfileImageUrl))
                        {
                            imageData = CommonFunction.ConvertImageURLToBase64(ConfigurationManager.AppSettings["PublicImages"] + mem.ProfileImageUrl);
                        }

                        CustomerDTO cusDto = new CustomerDTO()
                        {
                            MemberID = mem.ID,
                            IC = mem.IC,
                            Name = mem.Name,
                            ImageURL = mem.ProfileImageUrl,
                            ImageData = imageData,
                            IsActive = true,
                            Phone = mem.Mobile,
                            Email = mem.Email,
                            Gender = mem.Gender.HasValue ? mem.Gender.Value : true,
                            Marital = mem.IsMarried,
                            JoinedDate = DateTime.UtcNow,
                            BirthDate = mem.DOB.HasValue ? mem.DOB.Value : DateTime.UtcNow,
                            Anniversary = mem.Anniversary.HasValue ? mem.Anniversary.Value : DateTime.UtcNow,
                            ValidTo = DateTime.UtcNow,
                            IsMembership = !string.IsNullOrEmpty(mem.MemberCard) ? true : false,
                            IsWantMembership = true,
                            HomeStreet = mem.HomeAddress,
                            HomeCity = mem.HomeCity,
                            HomeZipCode = mem.HomePostalCode,
                            HomeCountry = mem.HomeCountry,
                            OfficeStreet = mem.OfficeAddress,
                            OfficeCity = mem.OfficeCity,
                            OfficeZipCode = mem.OfficePostalCode,
                            OfficeCountry = mem.OfficeCountry,
                        };
                        data.ListCustomer.Add(cusDto);
                        response.Data = data;
                        response.Success = true;
                    }

                }
                NSLog.Logger.Info("ResponseSearchMember", response);
            }
            catch (Exception ex)
            {
                NSLog.Logger.Error("Error-SearchMember", ex);
            }
            return response;
        }

        private static void ViewMembershipNoti(string memberID)
        {
            try
            {
                using (var _db = new PiOneDb())
                {
                    byte delete = (byte)Constants.EStatus.Deleted;
                    var listMembershipNoti = _db.MemberNotifications.Where(o => o.Type == (int)Constants.EMessageType.Membership && o.MemberID == memberID && !o.IsMemberView && o.Status != delete).ToList();
                    if (listMembershipNoti.Count > 0)
                    {
                        listMembershipNoti.ForEach(o => o.IsMemberView = true);
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorViewMembershipNoti", ex); }
        }

        public bool CheckData(int minutes)
        {
            NSBusBooking busBook = new NSBusBooking();
            new Thread(() => busBook.GetListBookingRemind()).Start();
            new Thread(() => NSBusBottle.CheckRemidOrExpireBottle(minutes)).Start();
            return true;
        }

    }
}
