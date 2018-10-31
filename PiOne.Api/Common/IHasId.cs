using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Common
{
    public interface IHasId
    {
        Guid Id { get; set; }
    }
}
