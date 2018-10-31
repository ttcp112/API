using PiOne.Api.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiOne.Api.Core.Response
{
    public class NSApiResponseBase : BaseModel
    {
        public string Description { get; set; }
        public string ID { get; set; }
    }
}
