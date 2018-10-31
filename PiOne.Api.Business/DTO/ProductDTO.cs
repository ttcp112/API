using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{

    #region -- response
    public class ResponseGetListProductForOrder : NSApiResponseBase
    {
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public string MerchantID { get; set; }
        public string MerchantName { get; set; }
        public bool IsCustomer { get; set; }
        public TaxDTO Tax { get; set; }                         /* Tax value */
        public List<SettingDTO> ListSetting { get; set; }
        public ServiceChargeDTO ServiceCharge { get; set; }     /* service charge of this store */
        public List<ProductOnKioskDTO> ListProduct { get; set; }
        public List<ProductOnKioskDTO> ListRecommend { get; set; }
        public List<SeasonDTO> ListSeason { get; set; }
        public List<CategoryDTO> ListCategory { get; set; }

        public ResponseGetListProductForOrder()
        {
            ListSeason = new List<SeasonDTO>();
            ListProduct = new List<ProductOnKioskDTO>();
            ListRecommend = new List<ProductOnKioskDTO>();
        }
    }

    #endregion
    public class ProductOnKioskDTO
    {
        public string ID { get; set; }
        public string ImageURL { get; set; }
        public string Code { get; set; }
        public int Sequence { get; set; }
        public int TypeID { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string GLCode { get; set; }
        public int CategorySequence { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public int Limit { get; set; }
        public bool IsAllowedDiscount { get; set; }
        public bool IsAllowedOpenPrice { get; set; }
        public double DefaultPrice { get; set; }
        public double SeasonPrice { get; set; }
        public SeasonDTO SeasonPriceDTO { get; set; }
        public bool IsComingSoon { get; set; }
        public bool DiscountType { get; set; }
        public int DefaultState { get; set; }
        public bool IsShowMessage { get; set; }
        public string Info { get; set; }
        public string Message { get; set; }
        public double Tax { get; set; }
        public double ServiceCharge { get; set; }
        public DateTime ExpiredDate { get; set; }
        public List<string> ListSeasonID { get; set; }
        public List<CompositionOnPOS> ListComposition { get; set; }
        public bool IsForce { get; set; }
        public bool IsOptional { get; set; }
        public bool IsRecommend { get; set; }
        public bool IsPromo { get; set; }
        public List<PromotionDTO> ListPromotion { get; set; }
        public string Description { get; set; }
        public ProductOnKioskDTO()
        {
            ListPromotion = new List<PromotionDTO>();
        }
    }

    public class SeasonDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //public bool IsRepeat { get; set; }
        public int RepeatType { get; set; }
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public List<int> ListDay { get; set; }
        public string Index { get; set; }       /* Index of season in excel file when importing season */
    }

    public class CompositionOnPOS
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string DisplayMessage { get; set; }
        public int Quantity { get; set; }
        public int GroupType { get; set; }
        public int Sequence { get; set; }
        public List<ForceModifierDTO> ListForceModifier { get; set; }
        public CompositionOnPOS()
        {
            ListForceModifier = new List<ForceModifierDTO>();
        }
    }

    public class ForceModifierDTO
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public double ExtraPrice { get; set; }
        public int Seq { get; set; }
        public decimal Qty { get; set; }
    }

    public class ProductLiteDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string ProductCode { get; set; }
        public string CategoryID { get; set; }
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public string Description { get; set; }
    }
}
