using PiOne.Api.Business.Consumer.Business;
using PiOne.Api.Core.Request;
using PiOne.Api.Core.Response;
using System.Threading.Tasks;

namespace PiOne.Api.Consumers.Business
{
    public class BUSLanguage : NSBusLanguage
    {
        protected static BUSLanguage _instance;

        public static BUSLanguage Instance
        {
            get
            {
                _instance = _instance != null ? _instance : new BUSLanguage();
                return _instance;
            }
        }

        static BUSLanguage() { }

        public async Task<NSApiResponse> GetLanguage(RequestModelBase input)
        {
            NSLog.Logger.Info("GetLanguageWallet", input);
            return base.GetLanguage(input.StoreID);
        }

        public async Task<NSApiResponse> GetLanguageDetail(RequestModelBase input)
        {
            NSLog.Logger.Info("GetLanguageDetailWallet", input);
            return base.GetLanguageDetail(input.ID);
        }
    }
}