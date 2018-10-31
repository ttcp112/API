using PiOne.Api.Business.DTO;
using PiOne.Api.Consumers.Business;
using PiOne.Api.Consumers.RequestDTO;
using PiOne.Api.Core.Controller;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PiOne.Api.Consumers.Controllers
{
    public class BookingController : BaseController
    {
        [HttpPost, Route(ApiRoutes.Booking_Get_Detail)]
        public async Task<NSApiResponse> GetDetailBooking(RequestGetDetailBooking request)
        {
            return await BUSBooking.Instance.GetDetailBooking(request);
        }

        [HttpPost, Route(ApiRoutes.Booking_CreateOrUpdate)]
        public async Task<NSApiResponse> CreateOrUpdateBooking(RequestCreateBooking request)
        {
            return await BUSBooking.Instance.CreateOrUpdateBooking(request);
        }
        [HttpPost, Route(ApiRoutes.Booking_Get_DateTime_Slot)]
        public async Task<NSApiResponse> GetBookingDateTimeSlot(RequestGetBookingDateTimeSlot request)
        {
            return await BUSBooking.Instance.GetBookingDateTimeSlot(request);
        }
        [HttpPost, Route(ApiRoutes.Booking_Update_Status)]
        public async Task<NSApiResponse> UpdateStatusBooking(RequestUpdateStatusBooking request)
        {
            return await BUSBooking.Instance.UpdateStatusBooking(request);
        }

        [HttpPost, Route(ApiRoutes.Booking_Get_List)]
        public async Task<NSApiResponse> GetListBooking(RequestGetListBooking request)
        {
            return await BUSBooking.Instance.GetListBooking(request);
        }
    }
}
