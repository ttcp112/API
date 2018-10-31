using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Interfaces
{
    public interface INSResponse<T>
    {
        IList<T> Values { get; }
    }
}
