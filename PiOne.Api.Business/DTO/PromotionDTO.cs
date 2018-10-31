using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    #region -- response --
    public class GetPromotionResponse : NSApiResponseBase
    {
        public List<PromotionDTO> ListPromotion { get; set; }

        public GetPromotionResponse()
        {
            ListPromotion = new List<PromotionDTO>();
        }
    }

    public class GetDetailPromotionResponse : NSApiResponseBase
    {
        public PromotionDTO PromotionData { get; set; }
    }


    #endregion
    public class PromotionDTO   /* struct for response list promotion */
    {
        /*********************/
        /* General Infor     */
        public string ID { get; set; }     /* promotion ID */
        public string ImageURL { get; set; }  /* promotion image */
        public string Name { get; set; }   /* promotion name */
        public string Description { get; set; }
        public string ShortName { get; set; }
        public string PromoteCode { get; set; }
        public byte? PromotionType { get; set; }
        public int? Priority { get; set; }
        public bool? IsActive { get; set; }
        public int Status { get; set; }
        public bool? IsAllowedCombined { get; set; } /* apply with other discounts */
        public int? MaximumUsedQty { get; set; }
        public double? MaximumEarnAmount { get; set; }
        public int? MaximumQtyPerUser { get; set; }
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public bool? IsMemberShip { get; set; }
        public List<TierLiteDTO> ListMemberTier { get; set; }
        public string CurrencySymbol { get; set; }
        public string Text { get; set; }

        /********************/
        /* Promotion Period */
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public string DateOfWeek { get; set; }
        public string DateOfMonth { get; set; }
        public bool? IsRepeat { get; set; }
        public bool? IsLimited { get; internal set; }

        /*********************/
        /* Promotion rule    */
        public bool isSpendOperatorAnd { get; set; }
        public bool isEarnOperatorAnd { get; set; }
        public List<SpendingRuleDTO> ListSpendingRule { get; set; }
        public List<EarningRuleDTO> ListEarningRule { get; set; }
        public string Index { get; set; }           /* Index of promotion in excel file, use for importing function */
        public PromotionDTO()
        {
            ListSpendingRule = new List<SpendingRuleDTO>();
            ListEarningRule = new List<EarningRuleDTO>();
        }
    }

    public class TierLiteDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class SpendingRuleDTO     /* Pending Rule to support response detail promotion */
    {
        public string ID { get; set; }
        public byte? SpendType { get; set; }   /*  Constants.SpendType */
        public byte? SpendOnType { get; set; }  /* Constants.SpendOnType */
        public double? Amount { get; set; }
        public List<Promotion_ProductDTO> ListProduct { get; set; }
        public List<PromotionCategoryDTO> ListCategory { get; set; }
    }
    public class EarningRuleDTO     /* Earning Rule to support response detail promotion */
    {
        public string ID { get; set; }
        public bool? DiscountType { get; set; } /* Constants.EValueType */
        public double? DiscountValue { get; set; }
        public byte EarnType { get; set; }   /* Constants.EarnType */
        public int Quantity { get; set; }
        public List<Promotion_ProductDTO> ListProduct { get; set; }
        public List<PromotionCategoryDTO> ListCategory { get; set; }
    }

    public class Promotion_ProductDTO /* Product DTO to support response detail promotion */
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public string CategoryID { get; set; }
    }

    public class PromotionCategoryDTO /* Promotion Category to support response detail promotion */
    {
        public string CategoryID { get; set; }
        public string Name { get; set; }
        public int? TotalItem { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<ProductDTO> ListProduct { get; set; }
    }

    public class ProductDTO
    {
        public string ID { get; set; }
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public string ParentID { get; set; }
        public string ProductTypeID { get; set; }
        public string ProductTypeName { get; set; }
        public int ProductType { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string TaxID { get; set; }
        public string TaxName { get; set; }
        public int OrderByIndex { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public string BarCode { get; set; }
        public string Description { get; set; }
        public string PrintOutText { get; set; }
        public double Cost { get; set; }
        public int Unit { get; set; }
        public string Measure { get; set; }
        public double Quantity { get; set; }
        public int Limit { get; set; }
        public bool IsCheckedStock { get; set; }
        public bool HasServiceCharge { get; set; }
        public double ServiceCharge { get; set; }
        public bool IsAllowedDiscount { get; set; }
        public bool IsRecommend { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowedOpenPrice { get; set; }
        public double DefaultPrice { get; set; }
        public string SeasonPriceID { get; set; }
        public string SeasonPriceName { get; set; }
        public double SeasonPrice { get; set; }
        public double ExtraPrice { get; set; }
        public bool IsPrintedOnCheck { get; set; }
        public DateTime ExpiredDate { get; set; }
        public bool IsAutoAddToOrder { get; set; }
        public bool IsExtraFood { get; set; }
        public bool IsComingSoon { get; set; }
        public string ColorCode { get; set; }
        public string ImageURL { get; set; }
        public string ImageData { get; set; }
        public double Tax { get; set; }
        public bool IsForce { get; set; }
        public bool IsOptional { get; set; }
        public bool IsAddition { get; set; }
        public int DefaultState { get; set; }
        public bool IsShowMessage { get; set; }
        public string Info { get; set; }
        public string Message { get; set; }
        public bool IsShowInReservation { get; set; }
        public bool IsMultiStore { get; set; }
        public string CreateUser { get; set; }
        public string Index { get; set; }           /* Index of product in excel file when importing product */
        public List<ProductSeasonDTO> ListProductSeason { get; set; }
        public List<GroupProductDTO> ListGroup { get; set; }
        public List<PrinterOnProductDTO> ListPrinter { get; set; }

        public ProductDTO()
        {
            ListProductSeason = new List<ProductSeasonDTO>();
            ListGroup = new List<GroupProductDTO>();
            ListPrinter = new List<PrinterOnProductDTO>();
        }
    }

    public class ProductSeasonDTO
    {
        public string StoreID { get; set; }
        public string SeasonID { get; set; }
        public string SeasonName { get; set; }
        public bool IsPOS { get; set; }
    }

    public class GroupProductDTO
    {
        public string ID { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int Type { get; set; }
        public List<ProductOnGroupDTO> ListProductOnGroup { get; set; }
    }

    public class ProductOnGroupDTO
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiredDate { get; set; }
        public double ExtraPrice { get; set; }
        public int Sequence { get; set; }
        public string CreatedUser { get; set; }
    }

    public class PrinterOnProductDTO
    {
        public string ID { get; set; }
        public string StoreID { get; set; }
        public string PrinterID { get; set; }
        public string PrinterName { get; set; }
        public bool IsActive { get; set; }
    }
}
