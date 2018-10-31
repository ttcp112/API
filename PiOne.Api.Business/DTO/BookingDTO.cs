using PiOne.Api.Core.Request;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Business.DTO
{
    /* Reservation DTO */
    public class ReservationDTO
    {
        public string ID { get; set; }
        public string MerchantID { get; set; }
        public string StoreID { get; set; }
        public string MemberID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerID { get; set; }
        public string Email { get; set; }
        public string ReservationNo { get; set; }
        public string Mobile { get; set; }
        public DateTime DateTime { get; set; }
        public byte Cover { get; set; }
        public string Remark { get; set; }
        public string BookingMethodID { get; set; }
        public string BookingMethodName { get; set; }
        public string TableID { get; set; }
        public string TableName { get; set; }
        public string OrderID { get; set; }
        public byte ReservationStatus { get; set; }
        public DateTime? ChangedDate { get; set; }
        public string ChangedBy { get; set; }
        public string ChangedReason { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsRemove { get; set; } /* true: remove on wallet, false: display on wallet */
        public bool IsEatIn { get; set; }
        public StoreDTO StoreInfo { get; set; }
        public List<ReserHisDTO> ListHistory { get; set; }
        public List<ReservationDetailDTO> ListReservationDetails { get; set; }
        // public NuposOrderDetailDTO OrderDetail { get; set; }
        public ReservationDTO()
        {
            ListHistory = new List<ReserHisDTO>();
            ListReservationDetails = new List<ReservationDetailDTO>();
        }
    }
    public class ReserHisDTO
    {
        public string ID { get; set; }
        public DateTime DateTime { get; set; }
        public double TotalAmount { get; set; }
        public string Remark { get; set; }

    }
    public class ReservationDetailDTO
    {
        public string ProductID { get; set; }  /* contain product of reservation or service */
        public string ProductName { get; set; }
        public byte ProductType { get; set; }
        public byte ModifierType { get; set; }
        public int Quantity { get; set; }
        public double? Price { get; set; }
        public string ID { get; set; }
        public string ParentID { get; set; }
        public List<ReservationDetailDTO> ListChild { get; set; }
    }

    /* Response for get date time slot */
    public class GetDateTimeSlotResponse : NSApiResponseBase
    {
        public List<DateTimeSlotDTO> ListDateTime { get; set; }
        public GetDateTimeSlotResponse()
        {
            ListDateTime = new List<DateTimeSlotDTO>();
        }
    }

    public class DateTimeSlotDTO
    {
        public DateTime DateTimeData { get; set; }
        public DateTime From { get; set; }
        public bool IsPromotion { get; set; }
        public bool IsOff { get; set; } /* Off when business hour is off, or at that time slot don't have table */
    }
    public class RequestGetDetailBooking : RequestModelBase{ }

    public class RequestCreateBooking : RequestModelBase
    {
        public ReservationDTO reservation { get; set; }
        public AddItemToOrderDTO AddItemDTO { get; set; }
    }
    public class RequestGetBookingDateTimeSlot : RequestModelBase
    {
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public string PromotionID { get; set; }
        public bool IsGetDateSlot { get; set; }
        public byte CoverNo { get; set; }
    }

    /* Response for get detail reservation */
    public class GetReservationResponse : NSApiResponseBase
    {
        public ReservationDTO ReserData { get; set; }
    }
    public class RequestUpdateStatusBooking : RequestModelBase
    {
        public List<ReservationDTO> ListReser { get; set; }
    }

    public class RequestGetListMerchant : RequestModelBase
    {
        public int GetMerchantType { get; set; }
        public string SearchString { get; set; }
    }

    public class RequestGetListStore : RequestModelBase { }

    public class RequestGetListBooking : RequestModelBase
    {
        public int BookingRequestType { get; set; }
    }

    public class GetListReservationResponse : NSApiResponseBase
    {
        public List<ReservationDTO> ListReser { get; set; }
        public GetListReservationResponse()
        {
            ListReser = new List<ReservationDTO>();
        }
    }
}
