using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Exceptions
{
    public class BadRequestException
        :NSApiException
    {
        public BadRequestException() { }

        public BadRequestException(ApiException apiException)
            : base(HttpStatusCode.BadRequest, apiException.Message) {
                ErrorNumber = apiException.ErrorNumber;
                Type = apiException.Type;
        }

        public int ErrorNumber { get; private set; }
        public string Type { get; private set; }
    }
}
