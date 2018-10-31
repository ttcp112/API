using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    class OrderDTO
    {
    }

#region -- response --
    /* reponse data */
    public class ResponseOrderGetDetail : NSApiResponseBase
    {
        public OrderDetailDTO OrderDetail { get; set; }
    }

    public class ResponseAddItemToOrder : NSApiResponseBase
    {
        public OrderDetailDTO OrderDetail { get; set; }
    }
#endregion

    public class AddItemToOrderDTO
    {
        public string ID { get; set; }
        public bool IsWithPoins { get; set; }
        public bool IsMobile { get; set; }
        public string CreatedUser { get; set; }
        public int Mode { get; set; }
        public string StoreId { get; set; }
        public string CashierID { get; set; }
        public string CusID { get; set; }
        public byte Cover { get; set; }
        public string TagNumber { get; set; }
        public int SplitNo { get; set; }
        public int CurrentBill { get; set; }
        public string TableID { get; set; }
        public string OrderID { get; set; }
        public string ParentID { get; set; }
        public bool IsSelfOrdering { get; set; }
        public string PoinsOrderID { get; set; }
        public bool IsOrderFromWallet { get; set; }
        public List<Item> ListItem { get; set; }
        //public List<NuposPromotionItems> ListPromotion { get; set; }
        public byte OrderState { get; set; }

        /* delivery info */
        public string DeliveryNo { get; set; }
        public byte DeliveryState { get; set; }
        public byte DeliveryType { get; set; }
        public string DeliveryCustomerName { get; set; }
        public string DeliveryCustomerPhone { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryTime { get; set; }
        public double DeliveryFree { get; set; }
        public string Remark { get; set; }
        public int DeliveryPayment { get; set; }
        public string DeliveryPaymentMethodID { get; set; }
    }

    public class Item
    {
        public string ID { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string PrintOutText { get; set; }
        public string DiscountID { get; set; }
        public string DiscountName { get; set; }
        public int QueueNumber { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public bool IsSend { get; set; }
        public int DefaultState { get; set; }
        public bool IsAllowDiscount { get; set; }
        public bool IsOpenPrice { get; set; }
        public int ModifierType { get; set; }
        public string ImageUrl { get; set; }
        public double Tax { get; set; }
        public double ServiceCharge { get; set; }
        public double DiscountAmount { get; set; }
        public double ActualPrice { get; set; }
        public double DiscountValue { get; set; }
        public bool IsCurrency { get; set; }
        public string Description { get; set; }
        public bool IsTakeAway { get; set; }
        public int TypeID { get; set; }
        public int ItemState { get; set; }
        public bool IsDelete { get; set; }
        public string EmployeeDelete { get; set; }
        public string ReasonDelete { get; set; }
        public bool IsApplyPromotion { get; set; }
        public string PromotionID { get; set; }
        public string PromotionName { get; set; }
        public double PromotionAmount { get; set; }
        public double PromotionValue { get; set; }
        public int TabSeq { get; set; }
        public int ProductSeq { get; set; }
        public bool IsGiftCard { get; set; }
        public bool IsIncludeNetSale { get; set; }
        public string ParentID { get; set; }
        public List<Item> ListItem { get; set; }
    }
    
    public class OrderDetailDTO
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public string StoreID { get; set; }
        public string NuposOrderID { get; set; }
        public string OrderNO { get; set; }
        public string ReceiptNO { get; set; }
        public DateTime Date { get; set; }
        public string TableID { get; set; }
        public string TableName { get; set; }
        public string TablePosition { get; set; } //x|y|z
        public string ZoneID { get; set; }
        public string ZoneName { get; set; }
        public int Cover { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public bool IsMembership { get; set; }
        public bool IsExpired { get; set; }
        public int NoOfGiftCard { get; set; }
        public string TagNumber { get; set; }
        public List<Item> ListItem { get; set; }
        //public List<Misc> ListMisc { get; set; }
        public BillDiscount BillDiscount { get; set; }
        public double SubTotal { get; set; }
        public double Discount { get; set; }
        public double Promotion { get; set; }
        public double ServiceCharge { get; set; }
        public double Tax { get; set; }
        public double Tip { get; set; }
        public double Total { get; set; }
        public double RawTotal { get; set; }
        public string DrawerID { get; set; }
        public string CashierID { get; set; }
        public string CashierName { get; set; }
        public string CashierImageURL { get; set; }
        public List<SubOrder> SubOrder { get; set; }
        // public bool IsExtraFood { get; set; }
        public bool IsReceipt { get; set; }
        public int NextSequence { get; set; }
        public bool IsSelfOrdering { get; set; }
        public double OccupiedTime { get; set; }
        public bool IsCallForBill { get; set; }
        public byte Status { get; set; }
        public bool IsTemp { get; set; } /* tmp for new delivery */
        public double RoundingAmount { get; set; }
        public string Remark { get; set; }
        public DateTime ReceiptCreatedDate { get; set; }
        public byte OrderState { get; set; }
        /* list promotion */
        public List<PromotionItems> ListPromotion { get; set; }
    }

    public class SubOrder
    {
        public string OrderID { get; set; }
        public string OrderNo { get; set; }
        public string TagNumber { get; set; }
        public string ParentID { get; set; }
        public string MemberID { get; set; }
        public byte OrderState { get; set; }
    }

    public class BillDiscount
    {
        public string ID { get; set; }
        public string DiscountID { get; set; }
        public string Name { get; set; }
        public double DiscountAmount { get; set; }
        public double DiscountValue { get; set; }
        public int DiscountType { get; set; }
        public bool IsApplyTotalBill { get; set; }
    }

    public class Misc
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }

    public class PromotionItems
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public bool IsDelete { get; set; }
    }

}
