using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Exceptions
{
    [Serializable]
    public class NotFoundException
        : NSApiException
    {
        public NotFoundException() { }
        public NotFoundException(string body)
            : base(HttpStatusCode.NotFound, body) { }
    }
}
