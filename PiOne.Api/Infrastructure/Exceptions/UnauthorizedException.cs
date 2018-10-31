using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Exceptions
{
    [Serializable]
    public class UnauthorizedException
        : NSApiException
    {
        public UnauthorizedException() { }
        public UnauthorizedException(string body)
            : base(HttpStatusCode.Unauthorized, body) { }
    }
}
