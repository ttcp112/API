using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PiOne.Api.Consumers.Controllers
{
    public class ApiRoutes
    {
        //======================================================================================================
        // User
        //======================================================================================================
        public const string User_Register = "api/wallet/user/register";
        public const string User_Verify = "api/wallet/user/verify";
        public const string User_Forgot = "api/wallet/user/forgot";
        public const string User_Login = "api/wallet/user/login";
        public const string User_Logout = "api/wallet/user/logout";
        public const string User_Resend = "api/wallet/user/resend";
        public const string User_Get_Info = "api/wallet/user/get/info";
        public const string User_Get_History = "api/wallet/user/get/history";
        public const string User_SecurityCode = "api/securitycode";

        //public const string User_Delete_GiftCard = "api/wallet/user/delete/giftcard";
        //public const string User_Delete_Promotion = "api/wallet/user/delete/promotion";

        //======================================================================================================
        // Notification
        //======================================================================================================
        public const string Notification_Get_List = "api/wallet/notification/get/list";
        public const string Notification_Edit = "api/wallet/notification/edit";
        public const string Notification_View = "api/wallet/notification/view";

        //======================================================================================================
        // Merchant
        //======================================================================================================
        public const string Merchant_Get_List_Search = "api/v1/merchant/get/list/search";
        public const string Merchant_Get_Store_List = "api/v1/merchant/get/store/list";


        public const string Merchant_Get_List = "api/wallet/merchant/get/list";
        public const string Merchant_Get_Info = "api/wallet/merchant/get/info";
        public const string Merchant_User_Edit = "api/wallet/merchant/user/edit";
        public const string Merchant_Scan = "api/wallet/merchant/scan";
        public const string Merchant_Register_Online = "api/wallet/merchant/register";
        public const string Merchant_Reordering = "api/wallet/merchant/reordering";

        //======================================================================================================
        // Promotion
        //======================================================================================================
        //public const string Promotion_Get_List = "api/wallet/promotion/get/list";
        //public const string Promotion_Get_Detail = "api/wallet/promotion/get/detail";
        public const string Promotion_Save = "api/wallet/promotion/save";
        public const string Promotion_Scan = "api/wallet/promotion/scan";
        public const string Promotion_Reordering = "api/wallet/promotion/reordering";

        //======================================================================================================
        // Gift Card
        //======================================================================================================
        public const string GiftCard_Get_List = "api/wallet/giftcard/get/list";
        public const string GiftCard_Get_Detail = "api/wallet/giftcard/get/detail";
        public const string GiftCard_Remove = "api/wallet/giftcard/remove";
        public const string GiftCard_Scan = "api/wallet/giftcard/scan";
        public const string GiftCard_Reordering = "api/wallet/giftcard/reordering";
        public const string GiftCard_Item_ListStore = "api/wallet/giftcard/item/getstore";

        //======================================================================================================
        // Receipt
        //======================================================================================================
        public const string Receipt_Get_List = "api/wallet/receipt/get/list";
        public const string Receipt_Get_Detail = "api/wallet/receipt/get/detail";

        //======================================================================================================
        // Booking
        //======================================================================================================
        public const string Booking_Get_List = "api/v1/booking/get/list";
        public const string Booking_Get_Detail = "api/v1/booking/get/detail";
        public const string Booking_CreateOrUpdate = "api/v1/booking/CreateOrEdit";
        public const string Booking_Check_Date = "api/v1/booking/check/date";
        public const string Booking_Check_Time = "api/v1/booking/check/time";
        public const string Booking_Get_Service_List = "api/v1/booking/get/service/list";
        public const string Booking_Get_Technician_List = "api/v1/booking/get/technician/list";
        public const string Booking_Get_DateTime_Slot = "api/v1/booking/get/datetime/list";
        public const string Booking_Update_Status = "api/v1/booking/update/status";
        public const string Booking_Delivery_Get_List = "api/v1/booking/delivery/get/list";

        //======================================================================================================
        // Setting
        //======================================================================================================
        public const string Setting_Change_User_Info = "api/wallet/setting/change/user/info";
        public const string Setting_PDPA_Terms = "api/wallet/setting/pdpa/terms";
        public const string Setting_PDPA_Merchant = "api/wallet/setting/pdpa/merchant";
        public const string Setting_Change_User_Password = "api/wallet/setting/change/user/password";
        public const string Setting_Notification_Get = "api/wallet/setting/notification/get";
        public const string Setting_Notification_Save = "api/wallet/setting/notification/save";
        public const string Setting_Get = "api/wallet/setting/get";

        //======================================================================================================
        // Support
        //======================================================================================================
        public const string Support_Check_Data = "api/wallet/support/check/data";

        //======================================================================================================
        // Language
        //======================================================================================================
        public const string Language_Get = "api/v1/language/get";
        public const string Language_Get_Detail = "api/v1/language/get/detail";

        //======================================================================================================
        // Order Route
        //======================================================================================================
        public const string Order_Create = "api/v1/order/create";
        public const string Order_CheckDrawer = "api/v1/order/check/drawer";
        public const string Order_Delete = "api/v1/order/delete";
        public const string Order_Add_Item = "api/v1/order/add";
        public const string Order_Get_Detail = "api/v1/order/get/detail";
        public const string Order_ChangeStateTable = "api/v1/order/changeStateTable";
        public const string Order_Pay_ByExternal = "api/v1/order/pay/byExternal";
        public const string Order_Done = "api/v1/order/done";
        public const string Order_Sync = "api/v1/order/sync";
        public const string Order_Temp_Done = "api/v1/order/temp/done";


        /*------------------------------------------------------------------------------------------------------*\
         * Paymentmethod Route
        \*------------------------------------------------------------------------------------------------------*/
        public const string PaymentMethod_Get_External = "api/v1/paymentmethod/get/external";

        /*------------------------------------------------------------------------------------------------------*\
         * Product Route
        \*------------------------------------------------------------------------------------------------------*/
        public const string Product_GetList_ForOrder = "api/v1/product/getlist/forOrder";

        /*------------------------------------------------------------------------------------------------------*\
         * Member Route
        \*------------------------------------------------------------------------------------------------------*/
        public const string Member_Login = "api/v1/member/login";
        public const string Member_Register = "api/v1/member/register";
        public const string Member_ForgotPassword = "api/v1/member/forgotpassword";
        public const string Member_ChangePassword = "api/v1/member/changepassword";
        public const string Member_EditProfile = "api/v1/member/editprofile";
        public const string Member_GetCustomerHangGift = "api/v1/member/getcustomerhanggift";
        public const string Member_Card_Get = "api/v1/member/card/get";
        public const string Member_Card_AddEdit = "api/v1/member/card/addedit";
        public const string Member_Artist_Register = "api/v1/member/artist/register";
        public const string Member_Artist_CheckInOut = "api/v1/member/artist/checkinout";
        public const string Member_Get = "api/v1/member/get";
        public const string Member_Get_Tier = "api/v1/member/get/tier";
        public const string Member_Logout = "api/v1/member/logout";
        public const string Member_Loyalty_Update = "api/v1/member/loyalty/update";
        public const string Member_Loyalty_Noti = "api/v1/member/loyalty/noti";
        public const string Member_Search = "api/v1/member/search";
        public const string Member_Check_Data = "api/v1/member/check/data";
        /*------------------------------------------------------------------------------------------------------*\
         * Store Route
        \*------------------------------------------------------------------------------------------------------*/
        public const string Store_GetInfo = "api/v1/store/getinfo";
        public const string Store_ScanQR = "api/v1/store/scanqr";

        /*------------------------------------------------------------------------------------------------------*\
        * Store Route
       \*------------------------------------------------------------------------------------------------------*/
        public const string Promotion_Get_List = "api/v1/promotion/get/list";
        public const string Promotion_Get_Detail = "api/v1/promotion/get/detail";

        //======================================================================================================
        // Chatting Template
        //======================================================================================================
        public const string ChattingTemplate_InsertOrUpdate = "api/v1/ChattingTemplate/CreateOrEdit";
        public const string ChattingTemplate_Delete = "api/v1/ChattingTemplate/Delete";
        public const string ChattingTemplate_Get = "api/v1/ChattingTemplate/Get";
        public const string ChattingTemplate_Import = "api/v1/ChattingTemplate/Import";
        public const string ChattingTemplate_Export = "api/v1/ChattingTemplate/Export";

        //======================================================================================================
        // Chat
        //======================================================================================================
        public const string Chat_Get = "api/v1/Chat/Get";
        public const string Chat_Get_Template = "api/v1/Chat/Get/Template";
        public const string Chat_Get_Message = "api/v1/Chat/Get/Message";
        public const string Chat_Send_Message = "api/v1/Chat/Send/Message";
        public const string Chat_Send_Gift = "api/v1/Chat/Send/Gift";

        //======================================================================================================
        // Bottle
        //======================================================================================================
        public const string Bottle_Get_List = "api/v1/Bottle/Get/List";
        public const string Bottle_Notify = "api/v1/Bottle/Notify";
    }
}