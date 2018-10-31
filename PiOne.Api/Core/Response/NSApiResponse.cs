using PiOne.Api.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Core.Response
{
    public class NSApiResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public NSApiErrorResponse Error { get; set; }

        public NSApiResponseBase Data { get; set; }

        public NSApiResponse()
        {
            Success = false;
            Data = new NSApiResponseBase();
        }

        public NSApiResponse(bool success) { Success = success; }

        public NSApiResponse(NSApiResponseBase data)
        {
            Success = true;
            Data = data;
        }

        public NSApiResponse(string errorDescription)
        {
            Success = false;
            SetError("", errorDescription);
        }

        public void SetError(string errorCode, string errorDescription)
        {
            Error = new NSApiErrorResponse(errorCode, errorDescription, null);
        }
        public void SetError(string errorCode, string errorDescription, ExceptionResponse ex)
        {
            Error = new NSApiErrorResponse(errorCode, errorDescription, ex);
        }
    }
}
