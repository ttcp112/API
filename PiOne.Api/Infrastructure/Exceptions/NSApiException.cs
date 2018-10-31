using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Exceptions
{
    public class NSApiException
        : Exception 
    {
        public NSApiException() { }

        public NSApiException(HttpStatusCode statusCode, string body)
            : base(body) { Code = statusCode; }

        public HttpStatusCode Code { get; private set; }
    }
}
