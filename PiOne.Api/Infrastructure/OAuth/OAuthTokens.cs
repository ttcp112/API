using PiOne.Api.Infrastructure.Http;
using PiOne.Api.Infrastructure.Interfaces;
using PiOne.Api.Infrastructure.ThirdParty.HttpUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.OAuth
{
    public class OAuthTokens
    {
        private readonly string _authorizeUri;
        private readonly string _tokenUri;
        private readonly X509Certificate2 _clientCertificate;
        private const string NSRequestUri = "oauth/RequestToken";
        private const string NSAccessTokenUri = "oauth/AccessToken";
        private const string NSAuthorizeUri = "oauth/Authorize";

        public OAuthTokens(string authorizeUri, string tokenUri, X509Certificate2 clientCertificate = null)
        {
            _authorizeUri = authorizeUri;
            _tokenUri = tokenUri;
            _clientCertificate = clientCertificate;
        }

        public OAuthTokens() { }

        public string AuthorizeUri
        {
            get
            {
                var uri = new UriBuilder(_authorizeUri)
                {
                    Path = NSAuthorizeUri
                };

                return uri.ToString();
            }
        }

        public string RequestUri
        {
            get
            {
                return NSRequestUri;
            }
        }

        public string AccessUri
        {
            get
            {
                return NSAccessTokenUri;
            }
        }

        public IToken GetRequestToken(IConsumer consumer, string header)
        {
            return GetToken(_tokenUri, new Token { ConsumerKey = consumer.ConsumerKey, ConsumerSecret = consumer.ConsumerSecret }, NSRequestUri, header);
        }

        public IToken GetAccessToken(IToken token, string header)
        {
            return GetToken(_tokenUri, token, NSAccessTokenUri, header);
        }

        public IToken RenewAccessToken(IToken token, string header)
        {
            return GetToken(_tokenUri, token, NSAccessTokenUri, header);
        }

        public IToken GetToken(string baseUri, IToken consumer, string endPoint, string header)
        {
            var req = new HttpClient(baseUri)
            {
                UserAgent = "Newstead Api wrapper - " + consumer.ConsumerKey
            };

            if (_clientCertificate != null)
                req.ClientCertificate = _clientCertificate;

            req.AddHeader("Authorization", header);

            var response = req.Post(endPoint, string.Empty);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new OAuthException(response.Body);
            }

            var qs = HttpUtility.ParseQueryString(response.Body);
            var expires = qs["oauth_expires_in"];
            var session = qs["oauth_session_handle"];

            var token = new Token(consumer.ConsumerKey, consumer.ConsumerSecret)
            {
                TokenKey = qs["oauth_token"],
                TokenSecret = qs["oauth_token_secret"],
                OrganisationId = qs["newstead_org_muid"]
            };

            if (!string.IsNullOrWhiteSpace(expires))
            {
                token.ExpiresAt = DateTime.UtcNow.AddSeconds(int.Parse(expires));
            }

            if (!string.IsNullOrWhiteSpace(session))
            {
                token.Session = session;
                token.SessionExpiresAt = DateTime.UtcNow.AddSeconds(int.Parse(qs["oauth_authorization_expires_in"]));
            }

            return token;
        }
    }
}
