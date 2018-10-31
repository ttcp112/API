using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.SupportObject
{
    public class PosApiRoute
    {
        //======================================================================================================
        // Table
        //======================================================================================================
        public const string Table_Get = "api/v1/Table/Get";

        //======================================================================================================
        // Product
        //======================================================================================================
        public const string Product_Get = "api/v1/Product/Get/List/ForPoins";
        public const string Product_GetOnPos = "api/v1/Product/GetOnPos/List/ForPoins";
        public const string Category_Get = "api/v1/Category/GetForPoins";

        //======================================================================================================
        // Order
        //======================================================================================================
        public const string Order_Add_GiftCard = "api/v1/Order/Add/GiftCard";
        public const string Order_GetList_ForPoins = "api/v1/Order/GetList/ForPoins";
        public const string Order_GetRawOrder_ForPoins = "api/v1/Order/GetRawOrder/ForPoins";
        public const string Order_GetOrderDetailInActivity = "api/v1/Activity/GetOrderDetailInActivityById";
        public const string Order_GetDataPrintReceipt = "api/v1/Order/GetDataPrintReceipt";
        //public const string Order_Add_Item = "api/v1/Order/Add/Item";
        public const string Order_CreateOrEdit = "api/v1/Order/CreateOrEdit";
        public const string Order_Add_Item_FromKiosk = "api/v1/Order/Add/ItemFromKiosk";
        public const string Order_Get_Detail = "api/v1/Order/Get";
        public const string Order_Table_ChangeState = "api/v1/Table/ChangeState";
        public const string Order_PrintReceipt = "api/v1/Order/PrintReceipt";
        public const string Order_Delete = "api/v1/Order/Delete";
        public const string Order_Get_Sumary = "api/v1/Order/Get/Sumary";
        public const string BusinessDay_Shift_Drawer_CheckState = "api/v1/BusinessDay/Shift/Drawer/CheckState";
        public const string PaymentMethod_GetExternal = "api/v1/PaymentMethod/GetExternal";


        //======================================================================================================
        // Delivery
        //======================================================================================================
        public const string Delivery_Add = "api/v1/Delivery/Add";
        public const string Delivery_UpdateInfo = "api/v1/Delivery/UpdateInfo";
        public const string Delivery_Generate_DeliveryNo = "api/v1/Delivery/GenerateDeliveryNo";
        public const string Delivery_Get = "api/v1/Delivery/Get";
        public const string Delivery_Update = "api/v1/Delivery/Update";
        public const string Delivery_Setting_Get = "api/v1/Delivery/Setting/Get";
        public const string Delivery_Update_InfoAndItem = "api/v1/Delivery/Update/InfoAndItem";
        
        //======================================================================================================
        // Pay
        //======================================================================================================
        public const string Pay_By_GiftCard = "api/v1/Pay/GiftCard";
        public const string Pay_By_Payment_Method = "api/v1/Pay/PayBy";


        //======================================================================================================
        // Employee
        //======================================================================================================
        public const string Employee_Get_By_Email = "api/v1/Employee/Get/ByEmail";
        public const string Employee_Get_Intergrate = "api/v1/Employee/Get/Intergrate";


        //======================================================================================================
        // Reference
        //======================================================================================================
        //public const string Reference_Get = "api/v1/Reference/Get";

        //======================================================================================================
        // Kiosk
        //======================================================================================================
        public const string Kiosk_Product_GetList = "api/v1/Kiosk/Product/GetList";

        //======================================================================================================
        // Promotion
        //======================================================================================================
        public const string Promotion_SyncCreateOrUpdate = "api/v1/Promotion/SyncCreateOrUpdate";
        public const string Promotion_Delete = "api/v1/Promotion/Delete";


        #region For Poins report
        public const string Poins_GetPromotionItemRedeems = "api/v1/HQ/GetItemRedeemForPromotionPoins";
        public const string Poins_GetPromotionReport = "api/v1/HQ/GetPromotionsReportPoins";
        public const string Poins_GetGiftCardsReport = "api/v1/HQ/GetGiftCardsReportPoins";
        public const string Poins_GetBookingReport = "api/v1/HQ/GetBookingReportPoins";
        public const string Poins_GetMembersActivitiesReport = "api/v1/HQ/GetMembersActivitiesReportPoins";
        #endregion End For Poins report
    }
}
