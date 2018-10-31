using PiOne.Api.Common;
using PiOne.Api.Infrastructure.Interfaces;
using PiOne.Api.Infrastructure.RateLimiter;
using PiOne.Api.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Core
{
    public class NSCoreApi: NSApi, INSCoreApi
    {
        public NSCoreApi(string baseUri, IAuthenticator auth, IConsumer consumer, IUser user,
            IJsonObjectMapper readMapper, IXmlObjectMapper writeMapper)
            : this(baseUri, auth, consumer, user, readMapper, writeMapper, null)
        {
        }

        public NSCoreApi(string baseUri, IAuthenticator auth, IConsumer consumer, IUser user, IJsonObjectMapper readMapper, IXmlObjectMapper writeMapper, IRateLimiter rateLimiter)
            : base(baseUri, auth, consumer, user, readMapper, writeMapper, rateLimiter)
        {
            Connect();
        }

        public NSCoreApi(string baseUri, ICertificateAuthenticator auth, IConsumer consumer, IUser user,
            IJsonObjectMapper readMapper, IXmlObjectMapper writeMapper)
            : this(baseUri, auth, consumer, user, readMapper, writeMapper, null)
        {
        }

        public NSCoreApi(string baseUri, ICertificateAuthenticator auth, IConsumer consumer, IUser user, IJsonObjectMapper readMapper, IXmlObjectMapper writeMapper, IRateLimiter rateLimiter)
            : base(baseUri, auth, consumer, user, readMapper, writeMapper, rateLimiter)
        {
            Connect();
        }

        public NSCoreApi(string baseUri, IAuthenticator auth, IConsumer consumer, IUser user)
            : this(baseUri, auth, consumer, user, null)
        {
        }

        public NSCoreApi(string baseUri, IAuthenticator auth, IConsumer consumer, IUser user, IRateLimiter rateLimiter)
            : this(baseUri, auth, consumer, user, new DefaultMapper(), new DefaultMapper())
        {
        }

        public NSCoreApi(string baseUri, ICertificateAuthenticator auth, IConsumer consumer, IUser user)
            : this(baseUri, auth, consumer, user, null)
        {
        }

        public NSCoreApi(string baseUri, ICertificateAuthenticator auth, IConsumer consumer, IUser user, IRateLimiter rateLimiter)
            : this(baseUri, auth, consumer, user, new DefaultMapper(), new DefaultMapper(), rateLimiter)
        {
        }

        private void Connect() { }
    }
}
