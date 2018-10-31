using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.RateLimiter
{
    public interface IRateLimiter
    {
        void WaitUntilLimit();
        bool CheckLimit();
    }
}
