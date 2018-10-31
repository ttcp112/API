using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Exceptions
{
    [Serializable]
    public class NotAvailableException
        : NSApiException
    {
        public NotAvailableException() { }
        public NotAvailableException(string body)
            : base(HttpStatusCode.ServiceUnavailable, body) { }
    }
}
