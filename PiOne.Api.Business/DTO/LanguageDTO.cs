using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    public class GetLanguageResponse : NSApiResponseBase
    {
        public List<LanguageDTO> ListLanguage { get; set; }

        public GetLanguageResponse()
        {
            ListLanguage = new List<LanguageDTO>();
        }
    }

    public class GetLanguageDetailResponse : NSApiResponseBase
    {
        public List<LanguageDetailDTO> ListDetail { get; set; }

        public GetLanguageDetailResponse()
        {
            ListDetail = new List<LanguageDetailDTO>();
        }
    }

    public class LanguageDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public string Locale { get; set; }
    }

    public class LanguageDetailDTO
    {
        public string KeyID { get; set; }
        public string KeyName { get; set; }
        public string ParentID { get; set; }
        public string Text { get; set; }
        public int Code { get; set; }
        public List<LanguageDetailDTO> ListChild { get; set; }
    }
}
