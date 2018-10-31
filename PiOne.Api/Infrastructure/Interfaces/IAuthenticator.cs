using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Interfaces
{
    public interface IAuthenticator
    {
        string GetSignature(IConsumer consumner, IUser user, Uri uri, string verb, IConsumer consumer1);
        IToken GetToken(IConsumer consumer, IUser user);
        IUser User { get; set; }
    }
}
