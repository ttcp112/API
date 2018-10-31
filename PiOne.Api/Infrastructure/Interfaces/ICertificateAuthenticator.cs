using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Interfaces
{
    public interface ICertificateAuthenticator: IAuthenticator
    {
        X509Certificate Certificate { get; }
    }
}
