using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Interfaces
{
    public interface IConsumer
    {
        string ConsumerKey { get; }
        string ConsumerSecret { get; }
    }

    public interface IToken {
        string UserId { get; set; }

        string MerchantId { get; set; }
        string OrganisationId { get; set; }

        string ConsumerKey { get; }
        string ConsumerSecret { get; }

        string TokenKey { get; }
        string TokenSecret { get; }

        string Session { get; }

        DateTime? ExpiresAt { get; }
        DateTime? SessionExpiresAt { get; }

        bool HasExpired { get; }
        bool HasSessionExpired { get; }
    }
}
