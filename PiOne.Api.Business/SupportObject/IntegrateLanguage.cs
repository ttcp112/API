using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.SupportObject
{
    public class IntegrateLanguageDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public string Locale { get; set; }
        public List<IntegrateLanguageDetailDTO> ListText { get; set; }

        public IntegrateLanguageDTO()
        {
            ListText = new List<IntegrateLanguageDetailDTO>();
        }
    }

    public class IntegrateLanguageDetailDTO
    {
        public string KeyID { get; set; }
        public string KeyName { get; set; }
        public string Text { get; set; }
    }

    public class ResponseGetLanguage
    {
        public bool Success { get; set; }
        public List<IntegrateLanguageDTO> ListData { get; set; }

        public ResponseGetLanguage()
        {
            Success = false;
            ListData = new List<IntegrateLanguageDTO>();
        }
    }
}
