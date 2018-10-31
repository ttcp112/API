using PiOne.Api.Business.DTO;
using PiOne.Api.Business.Support;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.Consumer.Business
{
    public class NSBusLanguage : NSBusBase, IBusiness
    {
        public NSApiResponse GetLanguage(string storeID)
        {
            var response = new NSApiResponse();

            try
            {
                GetLanguageResponse result = new GetLanguageResponse();

                result.ListLanguage = LanguageFuction.GetLanguage();

                response.Data = result;
                response.Success = true;

                NSLog.Logger.Info("ResponseGetLanguage", response);
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorGetLanguage", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }

        public NSApiResponse GetLanguageDetail(string id)
        {
            var response = new NSApiResponse();

            try
            {
                var data = new GetLanguageDetailResponse();

                string mess = "";
                data.ListDetail = LanguageFuction.GetLanguageDetail(id, ref mess);
                response.Data = data;

                if (string.IsNullOrEmpty(mess))
                    response.Success = true;
                else
                    response.Message = mess;

                NSLog.Logger.Info("ResponseGetLanguageDetail", response);
            }
            catch (Exception ex) { NSLog.Logger.Error("ErrorGetLanguageDetail", null, response, ex); }
            finally { /*_db.Refresh();*/ }
            return response;
        }
    }
}
