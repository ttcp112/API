using PiOne.Api.Infrastructure.Http;
using PiOne.Api.Infrastructure.Interfaces;
using PiOne.Api.Infrastructure.RateLimiter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Common
{
    public abstract class NSApi
    {
        public string BaseUri { get; protected set; }

        protected NSHttpClient Client { get; private set; }

        private NSApi(string baseUri)
        {
            BaseUri = baseUri;
        }

        protected NSApi(string baseUri, IAuthenticator auth, IConsumer consumer, IUser user, IJsonObjectMapper readMapper, IXmlObjectMapper writeMapper, IRateLimiter rateLimiter)
            : this(baseUri)
        {
            Client = new NSHttpClient(baseUri, auth, consumer, user, readMapper, writeMapper, rateLimiter);
        }

        protected NSApi(string baseUri, ICertificateAuthenticator auth, IConsumer consumer, IUser user, IJsonObjectMapper readMapper, IXmlObjectMapper writeMapper, IRateLimiter rateLimiter)
            : this(baseUri)
        {
            Client = new NSHttpClient(baseUri, auth, consumer, user, readMapper, writeMapper, rateLimiter);
        }

        public string UserAgent
        {
            get { return Client.UserAgent; }
            set { Client.UserAgent = value; }
        }
    }
}
