using PiOne.Api.Business.DTO;
using PiOne.Api.Common;
using PiOne.Api.Core.Response;
using PiOne.Api.DataModel.Context;
using PiOne.Api.DataModel.PiOneEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.Consumer.Business
{
    public class NSBusChatTemplate : NSBusBase, IBusiness
    {
        public NSApiResponse CreateOrUpdateChatTemplate(ChatTemplateDTO dto, List<string> listStoreID, string empID)
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    byte delete = (byte)Constants.EStatus.Deleted.GetHashCode();
                    string merchantID = _db.Stores.Where(o => listStoreID.Contains(o.ID) && o.Status != delete).Select(o => o.MerchantID).FirstOrDefault();

                    if (string.IsNullOrEmpty(dto.ID))
                    {
                        var template = _db.ChattingTemplates.Where(o => o.MerchantID == merchantID && o.Type == (byte)dto.ChatTemplateType && o.Name.ToLower() == dto.Name.ToLower().Trim()).FirstOrDefault();
                        if (template == null)
                        {
                            template = new ChattingTemplate()
                            {
                                ID = Guid.NewGuid().ToString(),
                                MerchantID = merchantID,
                                Name = dto.Name.Trim(),
                                Description = dto.Description.Trim(),
                                IsActive = dto.IsActive,
                                Type = (byte)dto.ChatTemplateType,
                                Status = (byte)Constants.EStatus.Actived,
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = empID,
                                ModifiedBy = empID,
                                ModifiedDate = DateTime.UtcNow,
                            };
                            _db.ChattingTemplates.Add(template);

                            if (_db.SaveChanges() > 0)
                                response.Success = true;
                            else
                                response.Message = "Unable to add new chatting template.";
                        }
                        else if (template.Status == delete)
                        {
                            template.Name = dto.Name.Trim();
                            template.Description = dto.Description.Trim();
                            template.IsActive = dto.IsActive;
                            template.Status = (byte)Constants.EStatus.Actived;
                            template.ModifiedBy = empID;
                            template.ModifiedDate = DateTime.UtcNow;

                            if (_db.SaveChanges() > 0)
                                response.Success = true;
                            else
                                response.Message = "Unable to add new chatting template.";
                        }
                        else
                            response.Message = "Duplicate name.";
                    }
                    else
                    {
                        var checkTemplate = _db.ChattingTemplates.Where(o => o.MerchantID == merchantID && o.Type == (byte)dto.ChatTemplateType && o.Name.ToLower() == dto.Name.ToLower().Trim() && o.ID != dto.ID && o.Status != delete).FirstOrDefault();
                        if (checkTemplate == null)
                        {
                            var template = _db.ChattingTemplates.Where(o => o.MerchantID == merchantID && o.ID == dto.ID && o.Status != delete).FirstOrDefault();
                            if (template != null)
                            {
                                template.Name = dto.Name.Trim();
                                template.Description = dto.Description.Trim();
                                template.IsActive = dto.IsActive;
                                template.Status = (byte)Constants.EStatus.Actived;
                                template.ModifiedBy = empID;
                                template.ModifiedDate = DateTime.UtcNow;

                                if (_db.SaveChanges() > 0)
                                    response.Success = true;
                                else
                                    response.Message = "Unable to edit chatting template.";
                            }
                            else
                                response.Message = "Unable to find chatting template.";
                        }
                        else
                            response.Message = "Duplicate name.";
                    }

                    NSLog.Logger.Info("ResponseCreateOrUpdateChatTemplate", response);
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorCreateOrUpdateChatTemplate", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse DeleteChatTemplate(string id, string empID)
        {
            var response = new NSApiResponse();
            try
            {
                using (var _db = new PiOneDb())
                {
                    var template = _db.ChattingTemplates.Where(o => o.ID == id).FirstOrDefault();
                    if (template != null)
                    {
                        template.Status = (byte)Constants.EStatus.Deleted;
                        template.ModifiedBy = empID;
                        template.ModifiedDate = DateTime.UtcNow;

                        if (_db.SaveChanges() > 0)
                            response.Success = true;
                        else
                            response.Message = "Unable to delete chatting template.";
                    }
                    else
                        response.Message = "Unable to find chatting template.";

                    NSLog.Logger.Info("ResponseDeleteChatTemplate", response);
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorDeleteChatTemplate", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse GetChatTemplate(string id, List<string> listStoreID, bool isActive, int type)
        {
            var response = new NSApiResponse();

            try
            {
                using (var _db = new PiOneDb())
                {
                    GetChatTemplateResponse result = new GetChatTemplateResponse();
                    byte delete = (byte)Constants.EStatus.Deleted;
                    string merchantID = _db.Stores.Where(o => listStoreID.Contains(o.ID) && o.Status != delete).Select(o => o.MerchantID).FirstOrDefault();

                    var query = _db.ChattingTemplates.Where(o =>o.MerchantID == merchantID && o.Status != (byte)Constants.EStatus.Deleted);

                    if (!string.IsNullOrEmpty(id))
                        query = query.Where(o => o.ID == id);
                    if (Enum.IsDefined(typeof(Constants.EChatTemplate), type))
                        query = query.Where(o => o.Type == (byte)type);
                    if (isActive)
                        query = query.Where(o => o.IsActive);

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

                    NSLog.Logger.Info("ResponseGetChatTemplate", response);
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorGetChatTemplate", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse ImportChatTemplate(List<ChatTemplateDTO> listTemplate, List<string> listStoreID, string empID)
        {
            var response = new NSApiResponse();

            try
            {
                using (var _db = new PiOneDb())
                {
                    ListError listError = new ListError();
                    using (var transaction = _db.Database.BeginTransaction())
                    {
                        try
                        {
                            List<ChattingTemplate> listInsert = new List<ChattingTemplate>();
                            byte delete = (byte)Constants.EStatus.Deleted;
                            string merchantID = _db.Stores.Where(o => listStoreID.Contains(o.ID) && o.Status != delete).Select(o => o.MerchantID).FirstOrDefault();

                            var listTemplateDB = _db.ChattingTemplates.Where(o => o.MerchantID == merchantID).ToList();

                            foreach (var item in listTemplate)
                            {
                                if (!Enum.IsDefined(typeof(Constants.EChatTemplate), item.ChatTemplateType))
                                {
                                    listError.ListProperty.Add(new PropertyRequired(item.Index, "", item.Name, "Chatting template is invalid type."));
                                    continue;
                                }

                                var template = listTemplateDB.Where(o => o.Name.ToLower() == item.Name.ToLower().Trim() && o.Type == item.ChatTemplateType).FirstOrDefault();
                                if (template != null)
                                {
                                    listError.ListProperty.Add(new PropertyRequired(item.Index, "", item.Name, "Chatting template is already exists."));
                                    continue;
                                }

                                template = listInsert.Where(o => o.Name.ToLower() == item.Name.ToLower().Trim() && o.Type == item.ChatTemplateType).FirstOrDefault();
                                if (template != null)
                                {
                                    listError.ListProperty.Add(new PropertyRequired(item.Index, "", item.Name, "Chatting template is already exists."));
                                    continue;
                                }

                                template = new ChattingTemplate()
                                {
                                    ID = Guid.NewGuid().ToString(),
                                    MerchantID = merchantID,
                                    Name = item.Name.Trim(),
                                    Description = item.Description.Trim(),
                                    IsActive = item.IsActive,
                                    Type = (byte)item.ChatTemplateType,
                                    Status = (byte)Constants.EStatus.Actived,
                                    CreatedDate = DateTime.UtcNow,
                                    CreatedBy = empID,
                                    ModifiedBy = empID,
                                    ModifiedDate = DateTime.UtcNow,
                                };
                                listInsert.Add(template);
                            }

                            if (listError.ListProperty.Count == 0)
                            {
                                _db.ChattingTemplates.AddRange(listInsert);
                                if (_db.SaveChanges() > 0)
                                {
                                    response.Success = true;
                                    transaction.Commit();
                                }
                                else
                                    response.Message = "Unable to import chatting template.";
                            }
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var eve in ex.EntityValidationErrors)
                                foreach (var ve in eve.ValidationErrors)
                                    listError.ListProperty.Add(new PropertyRequired(0, "", ve.PropertyName, ve.ErrorMessage));

                            NSLog.Logger.Error("ErrorImportChatTemplate", listTemplate, response, ex);
                            transaction.Rollback();
                        }
                    }
                    listError.ListProperty = listError.ListProperty.GroupBy(o => new { o.Index, o.StoreName, o.Property, o.Error })
                            .Select(o => new PropertyRequired(o.Key.Index, o.Key.StoreName, o.Key.Property, o.Key.Error)).ToList();
                    response.Data = listError;
                    NSLog.Logger.Info("ResponseImportChatTemplate", response);
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorImportChatTemplate", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse ExportChatTemplate(List<string> listStoreID, int type)
        {
            var response = new NSApiResponse();

            try
            {
                using (var _db = new PiOneDb())
                {
                    GetChatTemplateResponse result = new GetChatTemplateResponse();
                    byte delete = (byte)Constants.EStatus.Deleted;
                    string merchantID = _db.Stores.Where(o => listStoreID.Contains(o.ID) && o.Status != delete).Select(o => o.MerchantID).FirstOrDefault();

                    var query = _db.ChattingTemplates.Where(o => o.MerchantID == merchantID && o.Status != (byte)Constants.EStatus.Deleted);

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

                    NSLog.Logger.Info("ResponseExportChatTemplate", response);
                }
            }
            catch (Exception ex) { ValidationException(ref response, ex); NSLog.Logger.Error("ErrorExportChatTemplate", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }
    }
}
