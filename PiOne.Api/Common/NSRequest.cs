using PiOne.Api.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Common
{
    public class NSRequest<T> : List<T>, INSRequest<T>
    {
        public NSRequest() { }

        public NSRequest(IEnumerable<T> items)
        {
            AddRange(items);
        }

        public IList<T> Items
        {
            get { return this; }
        }
    }
}
