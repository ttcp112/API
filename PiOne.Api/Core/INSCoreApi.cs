using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Core
{
    public interface INSCoreApi
    {
        string BaseUri { get; }
        string UserAgent { get; set; }
    }
}
