using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.OAuth
{
    public class OAuthException: Exception
    {
        public OAuthException(string message)
            : base(message)
        {
        }
    }
}
