using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Exceptions
{
    [Serializable]
    public class RateExceededException
        : NSApiException 
    {
        public RateExceededException() { }
        public RateExceededException(string message) : base(HttpStatusCode.ServiceUnavailable, message) { }
    }
}
