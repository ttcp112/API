using Newtonsoft.Json;
using PiOne.Api.Business.DTO;
using PiOne.Api.Business.SupportObject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.Support
{
    public class LanguageFuction
    {
        public static List<IntegrateLanguageDetailDTO> GetListKey(string langID, List<string> listKey = null)
        {
            List<IntegrateLanguageDetailDTO> listData = new List<IntegrateLanguageDetailDTO>();
            try
            {
                if (!string.IsNullOrEmpty(langID))
                {
                    using (var client = new HttpClient())
                    {
                        var request = new
                        {
                            LanguageID = langID,
                            Type = int.Parse(ConfigurationManager.AppSettings["LanguageType"]),
                            ListKey = listKey,
                        };
                        NSLog.Logger.Info("RequestGetLanguageIntegrate", request);

                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["LanguageApi"]);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var responseFromNSApi = client.PostAsJsonAsync("api/v1/get", request).Result;

                        if (responseFromNSApi.IsSuccessStatusCode)
                        {
                            var result = responseFromNSApi.Content.ReadAsAsync<ResponseGetLanguage>().Result;
                            NSLog.Logger.Info("ResponseGetLanguageIntegrate", result);
                            if (result.Success)
                            {
                                var lan = result.ListData.FirstOrDefault();
                                if (lan != null) listData = lan.ListText;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorGetListKey", ex); }
            return listData;
        }

        public static string GetMultiLanguage(string message, string jsonContent, List<IntegrateLanguageDetailDTO> listLanguage)
        {
            try
            {
                var lang = listLanguage.Where(o => o.KeyName.Contains(message)).FirstOrDefault();
                if (lang != null) message = lang.Text;

                if (!string.IsNullOrEmpty(jsonContent))
                {
                    Dictionary<int, string> dictionary = JsonConvert.DeserializeObject<Dictionary<int, string>>(jsonContent);
                    return string.Format(message, dictionary.OrderBy(o => o.Key).Select(o => o.Value).ToArray());
                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorGetMultiLanguage", ex); }
            return message;
        }

        public static string GetMultiLanguage(string langID, string message, string jsonContent)
        {
            try
            {
                Dictionary<int, string> dictionary = new Dictionary<int, string>();
                if (!string.IsNullOrEmpty(jsonContent))
                    dictionary = JsonConvert.DeserializeObject<Dictionary<int, string>>(jsonContent);

                return GetMultiLanguage(langID, message, dictionary);
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorGetMultiLanguage", ex); }
            return message;
        }

        public static string GetMultiLanguage(string langID, string message, Dictionary<int, string> dictionary = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(langID))
                {
                    using (var client = new HttpClient())
                    {
                        var request = new
                        {
                            LanguageID = langID,
                            Key = message,
                        };
                        NSLog.Logger.Info("RequestGetLanguageDetailIntegrate", request);

                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["LanguageApi"]);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var responseFromNSApi = client.PostAsJsonAsync("api/v1/get/detail", request).Result;

                        if (responseFromNSApi.IsSuccessStatusCode)
                        {
                            message = responseFromNSApi.Content.ReadAsAsync<string>().Result;
                            NSLog.Logger.Info("ResponseGetDetailLanguageIntegrate", message);
                        }
                    }
                }

                if (dictionary != null)
                    message = string.Format(message, dictionary.OrderBy(o => o.Key).Select(o => o.Value).ToArray());
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorGetMultiLanguage", ex); }
            return message;
        }

        public static List<LanguageDTO> GetLanguage()
        {
            return GetLanguageIntegrate().Select(o => new LanguageDTO()
            {
                ID = o.ID,
                Name = o.Name,
                IsActive = o.IsActive,
                IsDefault = o.IsDefault,
                Locale = o.Locale,
            }).ToList(); ;
        }

        public static List<LanguageDetailDTO> GetLanguageDetail(string lanID, ref string mess, List<string> listKey = null)
        {
            List<LanguageDetailDTO> listData = new List<LanguageDetailDTO>();

            var lan = GetLanguageIntegrate(lanID, listKey).FirstOrDefault();
            if (lan != null)
            {
                listData = lan.ListText.Select(o => new LanguageDetailDTO()
                {
                    KeyID = o.KeyID,
                    KeyName = o.KeyName,
                    Text = o.Text,
                }).ToList();
            }
            else
                mess = "Unable to find language detail.";
            return listData;
        }

        private static List<IntegrateLanguageDTO> GetLanguageIntegrate(string lanID = null, List<string> listKey = null)
        {
            List<IntegrateLanguageDTO> listData = new List<IntegrateLanguageDTO>();
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new
                    {
                        LanguageID = lanID,
                        Type = int.Parse(ConfigurationManager.AppSettings["LanguageType"]),
                        ListKey = listKey,
                    };
                    NSLog.Logger.Info("RequestGetLanguageIntegrate", request);

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["LanguageApi"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseFromNSApi = client.PostAsJsonAsync("api/v1/get", request).Result;

                    if (responseFromNSApi.IsSuccessStatusCode)
                    {
                        dynamic dynamicObj = responseFromNSApi.Content.ReadAsAsync<object>().Result;
                        NSLog.Logger.Info("ResponseGetLanguageIntegrate", dynamicObj);
                        if ((bool)dynamicObj["Success"] == true)
                            listData = JsonConvert.DeserializeObject<List<IntegrateLanguageDTO>>(JsonConvert.SerializeObject(dynamicObj["ListData"]));
                    }

                }
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorGetLanguageIntegrate", ex); }
            return listData;
        }
    }
}
