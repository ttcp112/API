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
    public class NSBusChat : NSBusBase, IBusiness
    {
        public NSApiResponse GetChat(string memberID, string langID)
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    GetChatResponse result = new GetChatResponse();
                    byte delete = (byte)Constants.EStatus.Deleted;
                    string serverImage = ConfigurationManager.AppSettings["PublicImages"];
                    List<int> listTypeChat = new List<int>() { (int)Constants.EMessageType.Chat, (int)Constants.EMessageType.Gift };
                    List<int> listTypeNoti = new List<int>() { (int)Constants.EMessageType.Membership, (int)Constants.EMessageType.Booking, (int)Constants.EMessageType.Bottle };

                    result.ListChat = _db.Contacts.Where(o => o.MemberID == memberID && !o.IsDeleted && o.Status != delete)
                        .Join(_db.Members, c => c.ArtistID, m => m.ID, (c, m) => new { c, m })
                        .Select(o => new ChatDTO()
                        {
                            MemberID = memberID,
                            ArtistID = o.m.ID,
                            Name = o.m.Name,
                            ImageUrl = string.IsNullOrEmpty(o.m.ProfileImageUrl) ? "" : serverImage + o.m.ProfileImageUrl,
                            Date = Constants.MinDate,
                            Message = "",
                            NoOfMessage = 0,
                            IsSystem = false,
                        }).ToList();

                    var listMessage = _db.MemberNotifications.Where(o => o.MemberID == memberID && o.Status != delete && (listTypeChat.Contains(o.Type) || listTypeNoti.Contains(o.Type))).ToList();

                    var listNoti = listMessage.Where(o => listTypeNoti.Contains(o.Type)).ToList();
                    if (listNoti.Count > 0)
                    {
                        ChatDTO systemNoti = new ChatDTO()
                        {
                            MemberID = memberID,
                            ArtistID = "System",
                            Name = "System",
                            ImageUrl = "",
                            Date = Constants.MinDate,
                            Message = "",
                            NoOfMessage = 0,
                            IsSystem = true,
                        };

                        result.ListChat.Add(systemNoti);
                    }

                    if (result.ListChat.Count > 0)
                    {
                        foreach (var chat in result.ListChat)
                        {
                            List<MemberNotification> listMess = new List<MemberNotification>();
                            if (chat.IsSystem)
                                listMess = listMessage.Where(o => listTypeNoti.Contains(o.Type)).ToList();
                            else
                                listMess = listMessage.Where(o => o.ArtistID == chat.ArtistID && listTypeChat.Contains(o.Type)).ToList();

                            chat.NoOfMessage = listMess.Where(o => !o.IsMemberView).Count();
                            var lastMess = listMess.OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                            if (lastMess != null)
                            {
                                chat.Date = lastMess.CreatedDate ?? Constants.MinDate;
                                chat.Message = LanguageFuction.GetMultiLanguage(langID, lastMess.MessageContent, lastMess.JsonContent);
                            }
                        }
                    }

                    response.Success = true;
                    response.Data = result;

                    NSLog.Logger.Info("ResponseGetChat", response);
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorGetChat", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse GetMessage(string memberID, string artistID, bool isCustomer, string langID, int pageSize, int pageIndex)
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    GetMessageResponse result = new GetMessageResponse();
                    byte delete = (byte)Constants.EStatus.Deleted;
                    string serverImage = ConfigurationManager.AppSettings["PublicImages"];
                    Guid guid = new Guid();
                    bool isNoti = false;

                    List<int> listTypeChat = new List<int>() { (int)Constants.EMessageType.Chat, (int)Constants.EMessageType.Gift };
                    List<int> listTypeNoti = new List<int>() { (int)Constants.EMessageType.Membership, (int)Constants.EMessageType.Booking, (int)Constants.EMessageType.Bottle };

                    var query = _db.MemberNotifications.Where(o => o.MemberID == memberID && o.Status != delete);
                    if (Guid.TryParse(artistID, out guid))
                        query = query.Where(o => listTypeChat.Contains(o.Type) && o.ArtistID == artistID);
                    else if (artistID.ToLower() == "system")
                    {
                        isNoti = true;
                        query = query.Where(o => listTypeNoti.Contains(o.Type));
                    }
                    var listMessage = query.OrderByDescending(o => o.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                        .Select(o => new
                        {
                            MemberID = o.MemberID,
                            ArtistID = o.ArtistID,
                            Message = o.MessageContent,
                            Json = o.JsonContent,
                            Time = o.CreatedDate ?? Constants.MinDate,
                            IsCustomer = o.CreatedBy == o.MemberID,
                            MessageType = o.Type,
                        }).OrderBy(o => o.Time).ToList();

                    var listMember = _db.Members.Where(o => o.ID == memberID || o.ID == artistID).Select(o => new { o.ID, ImageUrl = o.ProfileImageUrl }).ToList();
                    result.MemberImageUrl = listMember.Where(o => o.ID == memberID).Select(o => string.IsNullOrEmpty(o.ImageUrl) ? "" : serverImage + o.ImageUrl).FirstOrDefault();
                    result.ArtistImageUrl = listMember.Where(o => o.ID == artistID).Select(o => string.IsNullOrEmpty(o.ImageUrl) ? "" : serverImage + o.ImageUrl).FirstOrDefault();

                    if (listMessage.Count > 0)
                    {
                        var listKey = listMessage.Select(o => o.Message).Distinct().ToList();
                        var listLan = LanguageFuction.GetListKey(langID, listKey);

                        foreach (var mess in listMessage)
                        {
                            MessageDTO dto = new MessageDTO()
                            {
                                ArtistID = listTypeNoti.Contains(mess.MessageType) ? "System" : mess.ArtistID,
                                MemberID = mess.MemberID,
                                Time = mess.Time,
                                Message = LanguageFuction.GetMultiLanguage(mess.Message, mess.Json, listLan),
                                IsCustomer = listTypeNoti.Contains(mess.MessageType) ? false : mess.IsCustomer,
                            };
                            result.ListMessage.Add(dto);
                        }

                        new Thread(() => ViewMessage(memberID, artistID, isNoti, isCustomer)).Start();
                    }

                    response.Success = true;
                    response.Data = result;

                    NSLog.Logger.Info("ResponseGetMessage", response);
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorGetMessage", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse SendMessage(MessageDTO message, bool isCustomer, string deviceName)
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    if (!string.IsNullOrEmpty(message?.MemberID) && !string.IsNullOrEmpty(message?.ArtistID) && !string.IsNullOrEmpty(message?.Message))
                    {
                        List<string> listMemID = new List<string>() { message.MemberID, message.ArtistID };
                        var listMem = _db.Members.Where(o => listMemID.Contains(o.ID)).ToList();
                        var member = listMem.Where(o => o.ID == message.MemberID).FirstOrDefault();
                        var artist = listMem.Where(o => o.ID == message.ArtistID && o.IsArtist).FirstOrDefault();
                        if (member != null && artist != null)
                        {
                            MemberNotification noti = new MemberNotification()
                            {
                                ID = Guid.NewGuid().ToString(),
                                ArtistID = artist.ID,
                                MemberID = member.ID,
                                Type = (byte)Constants.EMessageType.Chat,
                                GroupID = "",
                                MessageSubject = "",
                                MessageContent = message.Message,
                                JsonContent = "",
                                IsRead = false,
                                IsSenderView = false,
                                IsMemberView = false,
                                Status = (byte)Constants.EStatus.Actived,
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = isCustomer ? member.ID : artist.ID,
                                ModifiedDate = DateTime.UtcNow,
                                ModifiedBy = isCustomer ? member.ID : artist.ID,
                            };
                            if (isCustomer) noti.IsMemberView = true;
                            else noti.IsSenderView = true;
                            message.Time = DateTime.UtcNow;

                            _db.MemberNotifications.Add(noti);
                            if (_db.SaveChanges() > 0)
                            {
                                response.Success = true;
                                new Thread(() =>
                                {
                                    if (!isCustomer) AddContact(member.ID, artist.ID);
                                    NotificationFuction.PushNoti_SendMessage(message.Message, member.ID, artist.ID, isCustomer);
                                    CommonFunction.ChatSendMessage(message, deviceName);
                                }).Start();
                            }
                            else
                                response.Message = "Unable to send message at this time.";
                        }
                        else
                            response.Message = "Unable to send message at this time.";
                    }
                    else
                        response.Message = "Unable to send message at this time.";

                    NSLog.Logger.Info("ResponseSendMessage", response);
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorSendMessage", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse SendGift(MessageDTO message)
        {
            var response = new NSApiResponse();
            try
            {
                NotificationFuction.PushNoti_SendGift(message.Message, message.MemberID, message.ArtistID, message.Dict);
                response.Success = true;
                NSLog.Logger.Info("ResponseSendGift", response);
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorSendMessage", null, response, ex); }
            return response;
        }


        public NSApiResponse GetTemplateForChat(string artistID, int type)
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    GetChatTemplateResponse result = new GetChatTemplateResponse();
                    byte delete = (byte)Constants.EStatus.Deleted;

                    var listStoreID = GetListStoreIDFromArtist(artistID);
                    var listMerID = _db.Stores.Where(o => listStoreID.Contains(o.ID) && o.Status != delete).Select(o => o.MerchantID).ToList();

                    var query = _db.ChattingTemplates.Where(o => listMerID.Contains(o.MerchantID) && o.Status != (byte)Constants.EStatus.Deleted && o.IsActive);

                    if (Enum.IsDefined(typeof(Constants.EChatTemplate), type))
                        query = query.Where(o => o.Type == (byte)type);

                    result.ListChatTemplate = query.OrderBy(o => o.Name)
                        .Select(o => new ChatTemplateDTO()
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Description = o.Description,
                            IsActive = o.IsActive,
                            ChatTemplateType = o.Type,
                        }).ToList();

                    response.Success = true;
                    response.Data = result;

                    NSLog.Logger.Info("ResponseGetTemplateForChat", response);
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorGetTemplateForChat", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        private List<string> GetListStoreIDFromArtist(string memberID)
        {
            List<string> listStoreID = new List<string>();
            try
            {
                using (var client = new HttpClient())
                {
                    string uri = NIOneApiRoute.Artist_CheckMerchant + string.Format("?memberID={0}", memberID);
                    string webURL = ConfigurationManager.AppSettings["PosApi"];
                    client.BaseAddress = new Uri(webURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var result = client.GetAsync(uri).Result;

                    if (result.IsSuccessStatusCode)
                        listStoreID = result.Content.ReadAsAsync<List<string>>().Result;
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorGetListStoreIDFromArtist", ex); }
            return listStoreID;
        }

        private void ViewMessage(string memberID, string artistID, bool isNoti, bool isCustomer)
        {
            try
            {
                using (var _db = new PiOneDb())
                {
                    byte delete = (byte)Constants.EStatus.Deleted;
                    List<int> listTypeChat = new List<int>() { (int)Constants.EMessageType.Chat, (int)Constants.EMessageType.Gift };
                    List<int> listTypeNoti = new List<int>() { (int)Constants.EMessageType.Membership, (int)Constants.EMessageType.Booking, (int)Constants.EMessageType.Bottle };

                    if (isNoti)
                    {
                        var listNoti = _db.MemberNotifications.Where(o => listTypeNoti.Contains(o.Type) && o.MemberID == memberID && !o.IsMemberView && o.Status != delete).ToList();
                        if (listNoti.Count > 0)
                        {
                            listNoti.ForEach(o => o.IsMemberView = true);
                            _db.SaveChanges();
                        }
                    }
                    else if (isCustomer)
                    {
                        var listMessage = _db.MemberNotifications.Where(o => listTypeChat.Contains(o.Type) && o.MemberID == memberID && o.ArtistID == artistID && !o.IsMemberView && o.Status != delete).ToList();
                        if (listMessage.Count > 0)
                        {
                            listMessage.ForEach(o => o.IsMemberView = true);
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        var listMessage = _db.MemberNotifications.Where(o => listTypeChat.Contains(o.Type) && o.MemberID == memberID && o.ArtistID == artistID && !o.IsSenderView && o.Status != delete).ToList();
                        if (listMessage.Count > 0)
                        {
                            listMessage.ForEach(o => o.IsSenderView = true);
                            _db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorViewMessage", ex); }
        }

        private void AddContact(string memberID, string artistID)
        {
            try
            {
                using (var _db = new PiOneDb())
                {
                    byte delete = (byte)Constants.EStatus.Deleted;

                    var contact = _db.Contacts.Where(o => o.ArtistID == artistID && o.MemberID == memberID && o.Status != delete).FirstOrDefault();
                    if (contact == null)
                    {
                        contact = new Contact()
                        {
                            ID = Guid.NewGuid().ToString(),
                            ArtistID = artistID,
                            MemberID = memberID,
                            IsDeleted = false,
                            Status = (byte)Constants.EStatus.Actived,
                            CreatedBy = artistID,
                            CreatedDate = DateTime.UtcNow,
                            ModifiedBy = artistID,
                            ModifiedDate = DateTime.UtcNow,
                        };
                        _db.Contacts.Add(contact);
                        _db.SaveChanges();
                    }
                    else if (contact.IsDeleted)
                    {
                        contact.IsDeleted = false;
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorAddContact", ex); }
        }
    }
}
