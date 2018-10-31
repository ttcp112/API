using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Infrastructure.Interfaces
{
    public interface INSRequest<T>
    {
        IList<T> Items { get; }
        void Add(T value);
        void AddRange(IEnumerable<T> values);
    }
}
