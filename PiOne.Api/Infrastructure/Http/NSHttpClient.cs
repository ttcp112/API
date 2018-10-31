using PiOne.Api.Common;
using PiOne.Api.Infrastructure.Exceptions;
using PiOne.Api.Infrastructure.Interfaces;
using PiOne.Api.Infrastructure.RateLimiter;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Http
{
    public class NSHttpClient
    {
        internal readonly IJsonObjectMapper JSonMapper;
        internal readonly IXmlObjectMapper XmlMapper;
        internal readonly HttpClient Client;

        private NSHttpClient(IJsonObjectMapper jsonMapper, IXmlObjectMapper xmlMapper)
        {
            JSonMapper = jsonMapper;
            XmlMapper = xmlMapper;
        }

        public NSHttpClient(string baseUri, IAuthenticator auth, IConsumer consumer, IUser user, IJsonObjectMapper jsonMapper, IXmlObjectMapper xmlMapper)
            : this(baseUri, auth, consumer, user, jsonMapper, xmlMapper, null) { }

        public NSHttpClient(string baseUri, IAuthenticator auth, IConsumer consumer, IUser user, IJsonObjectMapper jsonMapper, IXmlObjectMapper xmlMapper, IRateLimiter rateLimiter)
            : this(jsonMapper, xmlMapper)
        {
            Client = new HttpClient(baseUri, auth, consumer, user, rateLimiter);
        }

        public NSHttpClient(string baseUri, ICertificateAuthenticator auth, IConsumer consumer, IUser user, IJsonObjectMapper jsonMapper, IXmlObjectMapper xmlMapper)
            : this(baseUri, auth, consumer, user, jsonMapper, xmlMapper, null) { }

        public NSHttpClient(string baseUri, ICertificateAuthenticator auth, IConsumer consumer, IUser user, IJsonObjectMapper jsonMapper, IXmlObjectMapper xmlMapper, IRateLimiter rateLimiter)
            : this(jsonMapper, xmlMapper)
        {
            Client = new HttpClient(baseUri, auth, consumer, user, rateLimiter)
            {
                ClientCertificate = auth.Certificate
            };
        }

        DateTime? ModifiedSince { get; set; }
        public string Where { get; set; }
        public string Order { get; set; }
        public NameValueCollection Parameters { get; set; }

        public string UserAgent
        {
            get { return Client.UserAgent; }
            set { Client.UserAgent = value; }
        }

        public IEnumerable<TResult> Get<TResult, TResponse>(string endPoint) where TResponse : INSResponse<TResult>, new()
        {
            Client.ModifiedSince = ModifiedSince;
            return Read<TResult, TResponse>(Client.Get(endPoint, new QueryGenerator(Where, Order, Parameters).UrlEncodedQueryString));
        }

        internal IEnumerable<TResult> Post<TResult, TResponse>(string endPoint, byte[] data, string mimeType)
            where TResponse : INSResponse<TResult>, new()
        {
            return Read<TResult, TResponse>(Client.Post(endPoint, data, mimeType, new QueryGenerator(null, null, Parameters).UrlEncodedQueryString));
        }

        public IEnumerable<TResult> Post<TResult, TResponse>(string endPoint, object data)
            where TResponse : INSResponse<TResult>, new()
        {
            return Read<TResult, TResponse>(Client.Post(endPoint, XmlMapper.To(data), query: new QueryGenerator(null, null, Parameters).UrlEncodedQueryString));
        }

        public IEnumerable<TResult> Put<TResult, TResponse>(string endPoint, object data)
            where TResponse : INSResponse<TResult>, new()
        {
            return Read<TResult, TResponse>(Client.Put(endPoint, XmlMapper.To(data), new QueryGenerator(null, null, Parameters).UrlEncodedQueryString));
        }

        public IEnumerable<TResult> Delete<TResult, TResponse>(string endPoint)
            where TResponse : INSResponse<TResult>, new()
        {
            return Read<TResult, TResponse>(Client.Delete(endPoint));
        }

        internal IEnumerable<TResult> Read<TResult, TResponse>(Response response)
            where TResponse : INSResponse<TResult>, new()
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JSonMapper.From<TResponse>(response.Body).Values;
            }

            HandleErrors(response);

            return null;
        }

        internal void HandleErrors(Response response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var data = JSonMapper.From<ApiException>(response.Body);

                if (data.Elements != null && data.Elements.Any())
                {
                    throw new ValidationException(data);
                }
                throw new BadRequestException(data);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException(response.Body);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException(response.Body);
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new NSApiException(response.StatusCode, response.Body);
            }

            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                var body = response.Body;
                if (body.Contains("oauth_problem"))
                {
                    throw new RateExceededException(body);
                }

                throw new NotAvailableException(body);
            }

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return;
            }

            throw new NSApiException(response.StatusCode, response.Body);
        }
    }
}
