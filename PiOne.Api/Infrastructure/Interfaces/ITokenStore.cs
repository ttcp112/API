using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Interfaces
{
    public interface ITokenStore
    {
        IToken Find(string user);
        void Add(IToken token);
        void Delete(IToken token);
    }
}
