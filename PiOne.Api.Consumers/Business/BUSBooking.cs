using PiOne.Api.Business.Consumer.Business;
using PiOne.Api.Business.DTO;
using PiOne.Api.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PiOne.Api.Consumers.Business
{
    public class BUSBooking : NSBusBooking
    {
        protected static BUSBooking _instance;
        public static BUSBooking Instance
        {
            get
            {
                _instance = _instance != null ? _instance : new BUSBooking();
                return _instance;
            }
        }
        static BUSBooking() { }

        public async Task<NSApiResponse> GetDetailBooking(RequestGetDetailBooking input)
        {
            NSLog.Logger.Info("GetDetailBooking", input);
            return base.GetDetailBooking(input);
        }

        public async Task<NSApiResponse> CreateOrUpdateBooking(RequestCreateBooking input)
        {
            NSLog.Logger.Info("CreateBooking", input);
            return base.CreateOrUpdateBooking(input);
        }
        public async Task<NSApiResponse> GetBookingDateTimeSlot(RequestGetBookingDateTimeSlot input)
        {
            NSLog.Logger.Info("GetBookingDateTimeSlot", input);
            return base.GetBookingDateTimeSlot(input);
        }
        public async Task<NSApiResponse> UpdateStatusBooking(RequestUpdateStatusBooking input)
        {
            NSLog.Logger.Info("UpdateStatusBooking", input);
            return base.UpdateStatusBooking(input);
        }

        public async Task<NSApiResponse> GetListBooking(RequestGetListBooking input)
        {
            NSLog.Logger.Info("GetListBooking", input);
            return base.GetListBooking(input);
        }
    }
}