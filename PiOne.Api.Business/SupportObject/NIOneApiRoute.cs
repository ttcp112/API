using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.SupportObject
{
    public class NIOneApiRoute
    {
        /*------------------------------------------------------------------------------------------------------*\
         * Store/Merchant Route
        \*------------------------------------------------------------------------------------------------------*/
        public const string Merchant_Search = "api/v1/Merchant/Search";

        public const string Store_GetInfo = "api/v1/store/getinfo";
        public const string Store_GetList = "api/v1/store/get/list";

        /*------------------------------------------------------------------------------------------------------*\
         * Customer Route
        \*------------------------------------------------------------------------------------------------------*/
        public const string Customer_InsertOrUpdate = "api/v1/Customer/CreateOrEdit";
        public const string Customer_HangGift_Get = "api/v1/Customer/HangGift/Get";
        public const string Customer_UpdateFromPiOne = "api/v1/Customer/UpdateFromPiOne";

        /*------------------------------------------------------------------------------------------------------*\
        * Order 
        \*------------------------------------------------------------------------------------------------------*/
        public const string Order_CreateOrEdit = "api/v1/Order/CreateOrEdit";
        public const string Order_Add_Item_FromKiosk = "api/v1/Order/Add/ItemFromKiosk";
        public const string Order_Get_Detail = "api/v1/Order/Get";
        public const string Order_Table_ChangeState = "api/v1/Table/ChangeState";
        public const string Order_PrintReceipt = "api/v1/Order/PrintReceipt";
        public const string Order_Delete = "api/v1/Order/Delete";
        public const string BusinessDay_Shift_Drawer_CheckState = "api/v1/BusinessDay/Shift/Drawer/CheckState";
        public const string Pay_By_Payment_Method = "api/v1/Pay/PayBy";
        public const string Order_Temp_Done = "api/v1/Order/Temp/Done";


        //======================================================================================================
        // Kiosk
        //======================================================================================================
        public const string Kiosk_Product_GetList = "api/v1/Kiosk/Product/GetList";

        //======================================================================================================
        // Booking
        //======================================================================================================
        public const string Booking_Get_List = "api/v1/booking/get/list";
        public const string Booking_Get_List_Remind = "api/v1/booking/get/list/remind";
        public const string Booking_Get_Detail = "api/v1/booking/get/detail";
        public const string Booking_CreateOrUpdate = "api/v1/booking/CreateOrEdit";
        public const string Booking_Get_DateTime_Slot = "api/v1/booking/get/datetime/list";
        public const string Booking_Update_Status = "api/v1/booking/update/status";


        //======================================================================================================
        // Promotion
        //======================================================================================================
        public const string Promotion_GetListDetailPromotion = "api/v1/Promotion/GetList/Detail";
        public const string Promotion_GetDetailPromotion = "api/v1/Promotion/Get";


        //======================================================================================================
        // Payment method
        //======================================================================================================
        public const string PaymentMethod_GetExternal = "api/v1/PaymentMethod/GetExternal";

        //======================================================================================================
        // Artist
        //======================================================================================================
        public const string Artist_Register = "api/v1/Artist/Register";
        public const string Artist_CheckInOut = "api/v1/Artist/CheckInOut";
        public const string Artist_CheckMerchant = "api/v1/Artist/CheckMerchant";

        //======================================================================================================
        // Bottle
        //======================================================================================================
        public const string Bottle_Get_Member = "api/v1/Bottle/Get/Member";
        public const string Bottle_Get_RemindOrExpire = "api/v1/Bottle/Get/RemindOrExpire";
    }
}
